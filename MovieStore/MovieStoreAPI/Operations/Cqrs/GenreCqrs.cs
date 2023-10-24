using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;

namespace MovieStoreAPI.Operations.Cqrs;   

public record GetAllGenreQuery() : IRequest<ApiResponse<List<GenreViewModel>>>;
public record GetGenreByIdQuery(int Id) : IRequest<ApiResponse<GenreViewModel>>;
