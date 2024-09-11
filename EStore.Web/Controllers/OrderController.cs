﻿using AutoMapper;
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
        [Route("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var orderResponse = await _orderService.CancelOrderAsync(orderId);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("RemoveOrderItem{orderItemid}")]
        public async Task<IActionResult> RemoveItemFromOrder(int orderItemid)
        {
            try
            {
                var updateOrder = await _orderService.RemoveOrderItemAsync(orderItemid);
                return Ok(updateOrder);
            }
            catch(Exception ex) 
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
    }
}
