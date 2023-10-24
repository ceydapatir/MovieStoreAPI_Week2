
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

public class ActorQueryHandler :
    IRequestHandler<GetAllActorQuery, ApiResponse<List<ActorViewModel>>>,
    IRequestHandler<GetActorByIdQuery, ApiResponse<ActorViewModel>>

{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public ActorQueryHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<ActorViewModel>>> Handle(GetAllActorQuery request, CancellationToken cancellationToken)
    {
        List<Actor> list = await dbContext.Set<Actor>()
            .Include(x => x.ActorMovies).ToListAsync(cancellationToken);
        List<ActorViewModel> mapped = mapper.Map<List<ActorViewModel>>(list);
        return new ApiResponse<List<ActorViewModel>>(mapped);
    }

    public async Task<ApiResponse<ActorViewModel>> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse<ActorViewModel>(ex.Message.Replace("\r\n", ""));
        }

        var entity = await dbContext.Set<Actor>()
            .Include(x => x.ActorMovies).FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
        if (entity == null)
        {
            return new ApiResponse<ActorViewModel>("Record not found!");
        }
        
        ActorViewModel mapped = mapper.Map<ActorViewModel>(entity);
        return new ApiResponse<ActorViewModel>(mapped);
    }
}