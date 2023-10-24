using Microsoft.AspNetCore.Mvc;
using MediatR;
using MovieStoreAPI.Operations.Cqrs;
using MovieStoreAPI.Models.DTO;
using MovieStoreAPI.Base.Response;
using MovieStoreAPI.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace CustomerStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private IMediator mediator;

        public CustomersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public Task<ApiResponse<CustomerViewModel>>  CreateCustomer([FromBody] CustomerDTO model) { 
            var operation = new CreateCustomerCommand(model);
            var result = mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse> DeleteCustomer(int id) { 
            var operation = new DeleteCustomerCommand(id);
            var result = mediator.Send(operation);
            return result;
        }
    }
}