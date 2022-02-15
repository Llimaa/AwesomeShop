using System;
using System.Threading.Tasks;
using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeShop.Services.Orders.Api.Controllers
{
    // [Route("api/customers/{customerId}/orders")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) {
            var query = new GetOrderById(id);

            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddOrder command) {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { id = id }, command);
        }
    }
}