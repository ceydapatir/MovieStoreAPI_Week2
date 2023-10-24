
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

public class DirectorQueryHandler :
    IRequestHandler<GetAllDirectorQuery, ApiResponse<List<DirectorViewModel>>>,
    IRequestHandler<GetDirectorByIdQuery, ApiResponse<DirectorViewModel>>

{
    private readonly MovieStoreDBContext dbContext;
    private readonly IMapper mapper;

    public DirectorQueryHandler(MovieStoreDBContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<DirectorViewModel>>> Handle(GetAllDirectorQuery request, CancellationToken cancellationToken)
    {
        List<Director> list = await dbContext.Set<Director>()
            .Include(x => x.Movies).ToListAsync(cancellationToken);
        List<DirectorViewModel> mapped = mapper.Map<List<DirectorViewModel>>(list);
        return new ApiResponse<List<DirectorViewModel>>(mapped);
    }

    public async Task<ApiResponse<DirectorViewModel>> Handle(GetDirectorByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(request.Id);    
        }
        catch (Exception ex)
        {
            return  new ApiResponse<DirectorViewModel>(ex.Message.Replace("\r\n", ""));
        }
        
        var entity = await dbContext.Set<Director>()
            .Include(x => x.Movies).FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
        if (entity == null)
        {
            return new ApiResponse<DirectorViewModel>("Record not found!");
        }
        
        DirectorViewModel mapped = mapper.Map<DirectorViewModel>(entity);
        return new ApiResponse<DirectorViewModel>(mapped);
    }
}