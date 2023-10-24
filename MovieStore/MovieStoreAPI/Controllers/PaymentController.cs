using Microsoft.AspNetCore.Mvc;
using MovieStoreAPI.Models.DTO;
using MediatR;
using MovieStoreAPI.Models.ViewModel;
using MovieStoreAPI.Operations.Cqrs;
using MovieStoreAPI.Base.Response;
using Microsoft.AspNetCore.Authorization;

namespace MovieStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private IMediator mediator;

        public PaymentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "customer, admin")]
        public Task<ApiResponse<List<PaymentViewModel>>> GetPaymentByCustomerId(int id){
            var operation = new GetPaymentByCustomerIdQuery(id);
            var result = mediator.Send(operation);
            return result;
        }
        
        [HttpPost]
        [Authorize(Roles = "customer")]
        public Task<ApiResponse<PaymentViewModel>> CreatePayment([FromBody] PaymentDTO paymentDTO){
            var operation = new CreatePaymentCommand(paymentDTO);
            var result = mediator.Send(operation);
            return result;
        }
    }
}