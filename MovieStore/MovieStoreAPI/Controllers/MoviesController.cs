using Microsoft.AspNetCore.Mvc;
using MovieStoreAPI.Models.DTO;
using MediatR;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Operations.Cqrs;
using Microsoft.AspNetCore.Authorization;

namespace MovieStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private IMediator mediator;

        public MoviesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpGet]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<List<MovieViewModel>>> GetMovies() { 
            var operation = new GetAllMovieQuery();
            var result = mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse<MovieViewModel>>  CreateMovie([FromBody] MovieDTO model) { 
            var operation = new CreateMovieCommand(model);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<MovieViewModel>>  GetMovieById(int id) { 
            var operation = new GetMovieByIdQuery(id);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse>  UpdateMovie(int id, [FromBody] MovieDTO model) { 
            var operation = new UpdateMovieCommand(model, id);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public Task<ApiResponse>  DeleteMovie(int id) { 
            var operation = new DeleteMovieCommand(id);
            var result = mediator.Send(operation);
            return result;
        }
    }
}