using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Models.ViewModel;

namespace MovieStoreAPI.Operations.Cqrs;

public record CreateDirectorCommand(DirectorDTO Model) : IRequest<ApiResponse<DirectorViewModel>>;
public record UpdateDirectorCommand(DirectorDTO Model,int Id) : IRequest<ApiResponse>;
public record DeleteDirectorCommand(int Id) : IRequest<ApiResponse>;
public record GetAllDirectorQuery() : IRequest<ApiResponse<List<DirectorViewModel>>>;
public record GetDirectorByIdQuery(int Id) : IRequest<ApiResponse<DirectorViewModel>>;