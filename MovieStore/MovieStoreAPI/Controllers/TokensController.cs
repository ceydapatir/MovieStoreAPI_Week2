using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStoreAPI.Base.Middleware;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Operations.Cqrs;
using Vk.Schema;

namespace MovieStoreAPI.Controllers;

[ApiController]
[Route("vk/api/v1/[controller]")]
public class TokensController : ControllerBase
{
    private IMediator mediator;

    public TokensController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    
    [HttpPost]
    public async Task<ApiResponse<TokenViewModel>> Post([FromBody] TokenDTO request)
    {
        var operation = new CreateTokenCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }   
    
    [TypeFilter(typeof(LogResourceFilter))]
    [TypeFilter(typeof(LogActionFilter))]
    [TypeFilter(typeof(LogAuthorizationFilter))]
    [TypeFilter(typeof(LogResourceFilter))]
    [TypeFilter(typeof(LogExceptionFilter))]
    [HttpGet("Test")]
    public ApiResponse Get()
    {
        return new ApiResponse();
    }
}
