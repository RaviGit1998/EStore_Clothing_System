using EStore.Application.Interfaces;
using EStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnOder([FromBody] Order order)
        {
            if (order == null) return BadRequest("Order Cannot be null");

            try
            {
                var createdOrder = await _orderService.CreateAnOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
       public async Task<IActionResult> GetOrderById(int Id)
        {
            
            var order =_orderService.GetOrderByIdAsync(Id);
            return Ok(order);
        }
    }
}
