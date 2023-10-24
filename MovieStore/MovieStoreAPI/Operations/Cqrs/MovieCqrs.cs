using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Models.ViewModel;

namespace MovieStoreAPI.Operations.Cqrs;  

public record CreateMovieCommand(MovieDTO Model) : IRequest<ApiResponse<MovieViewModel>>;
public record UpdateMovieCommand(MovieDTO Model,int Id) : IRequest<ApiResponse>;
public record DeleteMovieCommand(int Id) : IRequest<ApiResponse>;
public record GetAllMovieQuery() : IRequest<ApiResponse<List<MovieViewModel>>>;
public record GetMovieByIdQuery(int Id) : IRequest<ApiResponse<MovieViewModel>>;
