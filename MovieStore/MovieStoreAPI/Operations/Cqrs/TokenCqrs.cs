using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;
using Vk.Schema;

namespace MovieStoreAPI.Operations.Cqrs;  

public record CreateTokenCommand(TokenDTO Model) : IRequest<ApiResponse<TokenViewModel>>;
