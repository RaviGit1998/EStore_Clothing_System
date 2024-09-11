using EStore.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;

        }
        [HttpDelete]
        [Route("RemoveOrderItem/{orderItemid}")]
        public async Task<IActionResult> RemoveItemFromOrder(int orderItemid)
        {
            try
            {
               
                var updateOrder = await _orderItemService.RemoveOrderItemAsync(orderItemid);
               
                    if (updateOrder == null)
                    {
                        return NotFound("No Orders forun for the specific User ID.");
                    }
                return Ok(updateOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{orderrItemId}")]
        public async Task<IActionResult> GetOrderItemById(int orderrItemId)
        {
            try
            {
                var orderItem = await _orderItemService.GetOrderItemByIdAsync(orderrItemId);
                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
