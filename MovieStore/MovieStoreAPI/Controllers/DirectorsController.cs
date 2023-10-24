using MovieStoreAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Operations.Cqrs;
using Microsoft.AspNetCore.Authorization;

namespace MovieStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private IMediator mediator;

        public DirectorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<List<DirectorViewModel>>> GetDirectors() { 
            var operation = new GetAllDirectorQuery();
            var result = mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse<DirectorViewModel>>  CreateDirector([FromBody] DirectorDTO model) { 
            var operation = new CreateDirectorCommand(model);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<DirectorViewModel>>  GetDirectorById(int id) { 
            var operation = new GetDirectorByIdQuery(id);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse>  UpdateDirector(int id, [FromBody] DirectorDTO model) { 
            var operation = new UpdateDirectorCommand(model, id);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse>  DeleteDirector(int id) { 
            var operation = new DeleteDirectorCommand(id);
            var result = mediator.Send(operation);
            return result;
        }
    }
}