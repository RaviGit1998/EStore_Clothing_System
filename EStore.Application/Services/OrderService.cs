using EStore.Application.Interfaces;
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> ApplyCouponAsync(int orderId, string couponCode)
        {
           var success=await _orderRepository.ApplyCouponAsync(orderId, couponCode);
            if (!success)
                throw new InvalidOperationException("Failed to apply Coupon");

            await CalculateTotalAmountAsync(orderId);
            return success;
        }

        public async Task<decimal> CalculateTotalAmountAsync(int orderId)
        {
            if(orderId <= 0)
            {
                throw new ArgumentException("Invalid Order Id");
            }

            var order=await GetOrderByIdAsync(orderId);
           return await _orderRepository.CalculateTotalAmountAsync(orderId);
        }

        public async Task<Order> CancelOrderAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var order = await GetOrderByIdAsync(orderId);
            if (order.IsCancelled)
                throw new InvalidOperationException("Order is already cancelled");

            return await _orderRepository.CancelOrderAsync(orderId);
        }

        public async Task<Order> CreateAnOrderAsync(Order order)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new ArgumentException("Order must contain at least one order item.");

            return await _orderRepository.CreateAnOrderAsync(order);

        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User Id");
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<bool> ProcessPayment(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("Inavlid Order Id");

            var order=await GetOrderByIdAsync(orderId);
            if (order == null) throw new KeyNotFoundException($"Order with {orderId}");

            var success=await _orderRepository.ProcessPayment(orderId);
            if (!success) throw new InvalidOperationException("Payment processing Failed");
             return success;
        }

        public async Task<Order> RemoveOrderItemAsync(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var orderItem=await _orderRepository.RemoveOrderItemAsync(orderItemId);
            var order = await GetOrderByIdAsync(orderItem.Id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderItem.Id} not found");

            return order;
        }
    }

}
