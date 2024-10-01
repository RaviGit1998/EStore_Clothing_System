using AutoMapper;
using EStore.Application.Interfaces;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos.NewFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;
        public OrderController(IOrderService orderService,IEmailService emailservice)
        {
            _orderService = orderService;
            _emailService = emailservice;
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
        [Route("{orderId}")]
       public async Task<IActionResult> GetOrderById(int orderId)
        {

            if (orderId <= 0)
            {
                return BadRequest("Invalid order ID.");
            }

            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
    
        [Route("user/{userId}")]
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

        [HttpPost]
        [Route("Confirmation/{orderId}")]
        public async Task<IActionResult> ChangeStatusOfOrder(int orderId)
        {
            try
            {
                var orderResponse = await _orderService.ChangeStatusOfOrder(orderId);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [Route("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrderByIdasync(int orderId)
        {
            try
            {
                var orderResponse = await _orderService.CancelOrderById(orderId);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("{orderId}/total-amount")]
        public async Task<ActionResult<decimal>> GetTotalAmount(int orderId, [FromQuery] string couponCode = null)
        {
            try
            {
                var totalAmount = await _orderService.CalculateTotalAmountAsync(orderId, couponCode);

                // Return the updated total amount
                return Ok(totalAmount);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                // Call the service to delete the order
                var result = await _orderService.DeleteOrderByIdAsync(orderId);

                if (result)
                {
                    
                    return Ok("Order deleted successfully.");
                }
                else
                {
                   
                    return NotFound("Order not found.");
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost("sendOrderCancelDetails")]
        public async Task<IActionResult> SendOrderCancelDetails(OrderEmailRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Invalid request data.");
            }

            // Generate email content
            var subject = $"Order Cancelled - Order #{request.OrderId}";
            var body = BuildOrderCancelDetails(request);

            // Send the email
            _emailService.SendMailNotification(request.Email, subject, body);

            return Ok("Order Cancelled Successfully.");
        }
        [HttpPost("sendOrderDetails")]
        public async Task<IActionResult> SendOrderDetails(OrderEmailRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Invalid request data.");
            }

            // Generate email content
            var subject = $"Order Confirmation - Order #{request.OrderId}";
            var body = BuildOrderDetailsEmail(request);

            // Send the email
            _emailService.SendMailNotification(request.Email, subject, body);

            return Ok("Order details sent successfully.");
        }
        private string BuildOrderDetailsEmail(OrderEmailRequest order)
        {
            var sb = new StringBuilder();
            sb.Append("<h2>Thank you for your order!</h2>");
            sb.Append($"<p>Order Number: {order.OrderId}</p>");
            sb.Append($"<p>Order Date: {order.OrderDate}</p>");
            sb.Append($"<p>Order amount: {order.TotalAmount}</p>");
            sb.Append($"<p>Thank You</p>");
            sb.Append($"<p>Vastra</p>");
            sb.Append("<ul>");
            sb.Append("</ul>");
            // sb.Append($"<p>Total Amount: ${order.}</p>");
            sb.Append("<p>We will notify you once your order is shipped.</p>");

            return sb.ToString();
        }

        private string BuildOrderCancelDetails(OrderEmailRequest order)
        {
            var sb = new StringBuilder();
            sb.Append("<h2>ohh oo?? Your Order has been cancelled</h2>");
            sb.Append($"<p>Order Id: {order.OrderId}</p>");     
            sb.Append($"<p>Order amount: {order.TotalAmount}</p>");
            sb.Append($"<p>Please reach out to our Customer Service if you have any queries</p>");
            sb.Append($"<p>Thank You</p>");
            sb.Append($"<p>Vastra</p>");
            sb.Append("<ul>");
            sb.Append("</ul>");
            // sb.Append($"<p>Total Amount: ${order.}</p>");
          

            return sb.ToString();
        }
        public class OrderEmailRequest
        {
            public string Email { get; set; }
            public int OrderId { get; set; }
            public decimal TotalAmount { get; set; }
            public string OrderDate { get; set; }
        }

    }
}
