
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Data.Context;
using MovieStoreAPI.Data.Entities;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Operations.Cqrs;
using MovieStoreAPI.Operations.Validations;

namespace MovieStoreAPI.Operations.Command;

public class CustomerCommandHandler :
    IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerViewModel>>,
    IRequestHandler<DeleteCustomerCommand, ApiResponse>
{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public CustomerCommandHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<CustomerViewModel>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {   
            CustomerValidator validator = new CustomerValidator(dbContext);
            validator.ValidateAndThrow(request.Model);   
        }
        catch (Exception ex)
        {
            return  new ApiResponse<CustomerViewModel>(ex.Message.Replace("\r\n", ""));
        }
        
        Customer mapped = mapper.Map<Customer>(request.Model);
        var customer = await dbContext.Set<Customer>().AsQueryable<Customer>().FirstOrDefaultAsync(i => i.Username == mapped.Username && i.Role == mapped.Role);
        if (customer is not null)
        {
            return new ApiResponse<CustomerViewModel>("This customer already exists!");
        }
        
        var entity = await dbContext.Set<Customer>().AddAsync(mapped,cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<CustomerViewModel>(entity.Entity);
        foreach (var favgenreId in request.Model.FavGenreIds)
        {
            dbContext.FavGenreCustomers.Add(new FavGenreCustomer(){GenreId = favgenreId, CustomerId = response.Id});
        }
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<CustomerViewModel>(response);
    }

    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse(ex.Message.Replace("\r\n", ""));
        }
        var entity = await dbContext.Set<Customer>().AsQueryable<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        
        entity.IsActive = false;

        var genres = await dbContext.Set<FavGenreCustomer>().Where(i => i.Id == request.Id).ToListAsync();
        foreach (var genre in genres)
        {
            genre.IsActive = false;
        }

        var payments = await dbContext.Set<Payment>().Where(i => i.Id == request.Id).ToListAsync();
        foreach (var payment in payments)
        {
            payment.IsActive = false;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}