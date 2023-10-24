using MovieStoreAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MovieStoreAPI.Operations.Cqrs;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace MovieStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private IMediator mediator;

        public ActorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<List<ActorViewModel>>> GetActors() { 
            var operation = new GetAllActorQuery();
            var result = mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse<ActorViewModel>>  CreateActor([FromBody] ActorDTO model) { 
            var operation = new CreateActorCommand(model);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<ActorViewModel>>  GetActorById(int id) { 
            var operation = new GetActorByIdQuery(id);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse>  UpdateActor(int id, [FromBody] ActorDTO model) { 
            var operation = new UpdateActorCommand(model, id);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse>  DeleteActor(int id) { 
            var operation = new DeleteActorCommand(id);
            var result = mediator.Send(operation);
            return result;
        }
    }
}