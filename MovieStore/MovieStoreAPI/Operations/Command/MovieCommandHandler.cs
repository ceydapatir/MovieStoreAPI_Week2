
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

public class MovieCommandHandler :
    IRequestHandler<CreateMovieCommand, ApiResponse<MovieViewModel>>,
    IRequestHandler<UpdateMovieCommand, ApiResponse>,
    IRequestHandler<DeleteMovieCommand, ApiResponse>
{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public MovieCommandHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<MovieViewModel>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        try
        {
            MovieValidator validator = new MovieValidator(dbContext);
            validator.ValidateAndThrow(request.Model);
        }
        catch (Exception ex)
        {
            return  new ApiResponse<MovieViewModel>(ex.Message.Replace("\r\n", ""));
        }
        Movie mapped = mapper.Map<Movie>(request.Model);
        var movie = await dbContext.Set<Movie>().AsQueryable<Movie>().FirstOrDefaultAsync(i => i.Name == mapped.Name && i.DirectorId == mapped.DirectorId);
        if (movie is not null)
        {
            return new ApiResponse<MovieViewModel>("This movie already exists!");
        }

        var entity = await dbContext.Set<Movie>().AddAsync(mapped,cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<MovieViewModel>(entity.Entity);
        
        foreach (var actorId in request.Model.ActorIds)
        {
            dbContext.ActorMovies.Add(new ActorMovie(){ActorId = actorId, MovieId = response.Id});
        }
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<MovieViewModel>(response);
    }

    public async Task<ApiResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        try
        {
            MovieValidator validator = new MovieValidator(dbContext);
            validator.ValidateAndThrow(request.Model); 
            IdValidator id_validator = new IdValidator();
            id_validator.ValidateAndThrow(request.Id);
        }
        catch (Exception ex)
        {
            return  new ApiResponse(ex.Message.Replace("\r\n", ""));
        }
        
        var entity = await dbContext.Set<Movie>().AsQueryable<Movie>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        
        entity = mapper.Map(request.Model, entity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse(ex.Message.Replace("\r\n", ""));
        }

        var entity = await dbContext.Set<Movie>().AsQueryable<Movie>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        else if(entity.ActorMovies.Count() > 0 || entity.Director is not null)
        {
            return new ApiResponse("This movie cannot be deleted because it is associated with the director and actors!");
        }
        
        entity.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}