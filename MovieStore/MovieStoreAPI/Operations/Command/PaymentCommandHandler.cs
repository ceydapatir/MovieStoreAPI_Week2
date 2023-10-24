
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

public class PaymentCommandHandler :
    IRequestHandler<CreatePaymentCommand, ApiResponse<PaymentViewModel>>
{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public PaymentCommandHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<PaymentViewModel>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            PaymentValidator validator = new PaymentValidator(dbContext);
            validator.ValidateAndThrow(request.Model); 
        }
        catch (Exception ex)
        {
            return  new ApiResponse<PaymentViewModel>(ex.Message.Replace("\r\n", ""));
        }
        Payment mapped = mapper.Map<Payment>(request.Model);
        var entity = await dbContext.Set<Payment>().AddAsync(mapped,cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var customer_entity = await dbContext.Set<Customer>().FirstOrDefaultAsync(i => i.Id == mapped.CustomerId);
        var movie_entity = await dbContext.Set<Movie>().FirstOrDefaultAsync(i => i.Id == mapped.MovieId);
        var response = new PaymentViewModel(){
                CustomerFullName = customer_entity.Name + " " + customer_entity.Surname,
                MovieName = movie_entity.Name,
                Price = movie_entity.Price,
                PaymentDate = mapped.PaymentDate.ToString("dd/MM/yyy")
                };
        return new ApiResponse<PaymentViewModel>(response);
    }
}