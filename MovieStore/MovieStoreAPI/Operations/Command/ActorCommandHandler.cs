
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

public class ActorCommandHandler :
    IRequestHandler<CreateActorCommand, ApiResponse<ActorViewModel>>,
    IRequestHandler<UpdateActorCommand, ApiResponse>,
    IRequestHandler<DeleteActorCommand, ApiResponse>
{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public ActorCommandHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<ActorViewModel>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ActorValidator validator = new ActorValidator();
            validator.ValidateAndThrow(request.Model);
        }
        catch (Exception ex)
        {
            return  new ApiResponse<ActorViewModel>(ex.Message.Replace("\r\n", "").Replace("\r\n",""));
        }

        Actor mapped = mapper.Map<Actor>(request.Model);
        var actor = await dbContext.Set<Actor>().AsQueryable<Actor>().FirstOrDefaultAsync(i => i.Name == mapped.Name && i.Surname == mapped.Surname);
        if (actor is not null)
        {
            return new ApiResponse<ActorViewModel>("This actor already exists!");
        }

        var entity = await dbContext.Set<Actor>().AddAsync(mapped,cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ActorViewModel>(entity.Entity);
        return new ApiResponse<ActorViewModel>(response);
    }

    public async Task<ApiResponse> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ActorValidator validator = new ActorValidator();
            validator.ValidateAndThrow(request.Model);
            IdValidator id_validator = new IdValidator();
            id_validator.ValidateAndThrow(request.Id);  
        }
        catch (Exception ex)
        {
            return  new ApiResponse(ex.Message.Replace("\r\n", ""));
        }

        var entity = await dbContext.Set<Actor>().AsQueryable<Actor>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        
        entity = mapper.Map(request.Model, entity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse(ex.Message.Replace("\r\n", "").Replace("\r\n", ""));
        }

        var entity = await dbContext.Set<Actor>().AsQueryable<Actor>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        else if(entity.ActorMovies.Count() > 0)
        {
            return new ApiResponse("This actor cannot be deleted because it is in an existing movie!");
        }
        
        entity.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

}