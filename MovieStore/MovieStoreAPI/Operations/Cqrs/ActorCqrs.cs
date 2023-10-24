using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Models.ViewModel;

namespace MovieStoreAPI.Operations.Cqrs;

public record CreateActorCommand(ActorDTO Model) : IRequest<ApiResponse<ActorViewModel>>;
public record UpdateActorCommand(ActorDTO Model,int Id) : IRequest<ApiResponse>;
public record DeleteActorCommand(int Id) : IRequest<ApiResponse>;
public record GetAllActorQuery() : IRequest<ApiResponse<List<ActorViewModel>>>;
public record GetActorByIdQuery(int Id) : IRequest<ApiResponse<ActorViewModel>>;