
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

public class DirectorCommandHandler :
    IRequestHandler<CreateDirectorCommand, ApiResponse<DirectorViewModel>>,
    IRequestHandler<UpdateDirectorCommand, ApiResponse>,
    IRequestHandler<DeleteDirectorCommand, ApiResponse>
{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public DirectorCommandHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<DirectorViewModel>> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            DirectorValidator validator = new DirectorValidator();
            validator.ValidateAndThrow(request.Model); 
        }
        catch (Exception ex)
        {
            return  new ApiResponse<DirectorViewModel>(ex.Message.Replace("\r\n", ""));
        }

        Director mapped = mapper.Map<Director>(request.Model);
        var director = await dbContext.Set<Director>().AsQueryable<Director>().FirstOrDefaultAsync(i => i.Name == mapped.Name && i.Surname == mapped.Surname);
        if (director is not null)
        {
            return new ApiResponse<DirectorViewModel>("This director already exists!");
        }

        var entity = await dbContext.Set<Director>().AddAsync(mapped,cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<DirectorViewModel>(entity.Entity);
        return new ApiResponse<DirectorViewModel>(response);
    }

    public async Task<ApiResponse> Handle(UpdateDirectorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            DirectorValidator validator = new DirectorValidator();
            validator.ValidateAndThrow(request.Model); 
            IdValidator id_validator = new IdValidator();
            id_validator.ValidateAndThrow(request.Id);
        }
        catch (Exception ex)
        {
            return  new ApiResponse(ex.Message.Replace("\r\n", ""));
        }
        
        var entity = await dbContext.Set<Director>().AsQueryable<Director>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        
        entity = mapper.Map(request.Model, entity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteDirectorCommand request, CancellationToken cancellationToken)
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

        var entity = await dbContext.Set<Director>().AsQueryable<Director>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        else if(entity.Movies.Count() > 0)
        {
            return new ApiResponse("This director cannot be deleted because it is in an existing movie!");
        }
        
        entity.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}