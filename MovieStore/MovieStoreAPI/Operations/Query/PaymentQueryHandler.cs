
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

namespace MovieStoreAPI.Operations.Query;

public class PaymentQueryHandler :
    IRequestHandler<GetPaymentByCustomerIdQuery, ApiResponse<List<PaymentViewModel>>>

{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public PaymentQueryHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<PaymentViewModel>>> Handle(GetPaymentByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse<List<PaymentViewModel>>(ex.Message.Replace("\r\n", ""));
        }

        List<Payment> entity = await dbContext.Set<Payment>().Where(i => i.CustomerId == request.Id).AsQueryable<Payment>().ToListAsync();        
        var paymentviews = new List<PaymentViewModel>();

        var customer_entity = await dbContext.Set<Customer>().AsQueryable<Customer>().FirstOrDefaultAsync(i => i.Id == request.Id);
        foreach (var payment in entity)
        {
            var movie_entity = await dbContext.Set<Movie>().AsQueryable<Movie>().FirstOrDefaultAsync(i => i.Id == payment.MovieId);
            var paymentview = new PaymentViewModel(){
                CustomerFullName = customer_entity.Name + " " + customer_entity.Surname,
                MovieName = movie_entity.Name,
                Price = movie_entity.Price,
                PaymentDate = payment.PaymentDate.ToString("dd/MM/yyy")
                };
            paymentviews.Add(paymentview);
        }
        return new ApiResponse<List<PaymentViewModel>>(paymentviews);
    }
}