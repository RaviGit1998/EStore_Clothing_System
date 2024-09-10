using AutoMapper;
using EStore.Application.Interfaces;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos.NewFolder;
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
        public async Task<IActionResult> CreateAnOder([FromBody] OrderReq orderReq)
        {
            if (orderReq == null) return BadRequest("Order Cannot be null");
            try
            {        
                var orderResponse = await _orderService.CreateAnOrderAsync(orderReq);
                              
                return Ok(orderResponse);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{Id}")]
       public async Task<IActionResult> GetOrderById(int Id)
        {
          
            var order =await _orderService.GetOrderByIdAsync(Id);
            return Ok(order);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            if (userId == null)
            {
                return BadRequest("userId cannot be null");
            }

            try
            {
               var orders= await _orderService.GetOrdersByUserIdAsync(userId);
                if(orders==null || !orders.Any())
                {
                    return NotFound("No Orders forun for the specific User ID.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
