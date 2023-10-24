using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Models.ViewModel;


namespace MovieStoreAPI.Operations.Cqrs;  
public record CreatePaymentCommand(PaymentDTO Model) : IRequest<ApiResponse<PaymentViewModel>>;
public record GetPaymentByCustomerIdQuery(int Id) : IRequest<ApiResponse<List<PaymentViewModel>>>;
