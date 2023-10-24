using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Models.ViewModel;

namespace MovieStoreAPI.Operations.Cqrs;

public record CreateCustomerCommand(CustomerDTO Model) : IRequest<ApiResponse<CustomerViewModel>>;
public record DeleteCustomerCommand(int Id) : IRequest<ApiResponse>;