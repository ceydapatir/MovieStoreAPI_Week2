
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

public class MovieQueryHandler :
    IRequestHandler<GetAllMovieQuery, ApiResponse<List<MovieViewModel>>>,
    IRequestHandler<GetMovieByIdQuery, ApiResponse<MovieViewModel>>

{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public MovieQueryHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<MovieViewModel>>> Handle(GetAllMovieQuery request, CancellationToken cancellationToken)
    {
        List<Movie> list = await dbContext.Set<Movie>()
            .Include(x => x.Director)
            .Include(x => x.ActorMovies).ToListAsync(cancellationToken);
        List<MovieViewModel> mapped = mapper.Map<List<MovieViewModel>>(list);
        return new ApiResponse<List<MovieViewModel>>(mapped);
    }

    public async Task<ApiResponse<MovieViewModel>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse<MovieViewModel>(ex.Message.Replace("\r\n", ""));
        }

        var entity = await dbContext.Set<Movie>()
            .Include(x => x.Director)
            .Include(x => x.ActorMovies).FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
        if (entity == null)
        {
            return new ApiResponse<MovieViewModel>("Record not found!");
        }
        
        MovieViewModel mapped = mapper.Map<MovieViewModel>(entity);
        return new ApiResponse<MovieViewModel>(mapped);
    }
}