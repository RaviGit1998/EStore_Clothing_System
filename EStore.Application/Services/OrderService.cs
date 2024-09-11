using AutoMapper;
using EStore.Application.Interfaces;
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos.NewFolder;
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
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;   
        }

        public async Task<bool> ApplyCouponAsync(int orderId, string couponCode)
        {
           var success=await _orderRepository.ApplyCouponAsync(orderId, couponCode);
            if (!success)
                throw new InvalidOperationException("Failed to apply Coupon");

            await CalculateTotalAmountAsync(orderId, couponCode);
            return success;
        }

        public async Task<decimal> CalculateTotalAmountAsync(int orderId,string? couponCode)
        {
            if(orderId <= 0)
            {
                throw new ArgumentException("Invalid Order Id");
            }

            var order=await GetOrderByIdAsync(orderId);
           return await _orderRepository.CalculateTotalAmountAsync(orderId,couponCode);
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

        public async Task<OrderRes> CreateAnOrderAsync(OrderReq orderReq)
        {
            var order=_mapper.Map<Order>(orderReq);
            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new ArgumentException("Order must contain at least one order item.");

            var createdOrder= await _orderRepository.CreateAnOrderAsync(order);
            return _mapper.Map<OrderRes>(createdOrder);
        }

        public async Task<OrderRes> GetOrderByIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            return _mapper.Map<OrderRes>(order);
        }

     

        public async Task<IEnumerable<OrderRes>> GetOrdersByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid User Id");

           var orders= await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orderResponses = _mapper.Map<IEnumerable<OrderRes>>(orders);
           
            return orderResponses.ToList();
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

        public async Task<OrderRes> RemoveOrderItemAsync(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var orderItem = await _orderRepository.GetOrderItemByIdAsync(orderItemId);
            var order = await GetOrderByIdAsync(orderItem.OrderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderItem.OrderId} not found");

            await _orderRepository.RemoveOrderItemAsync(orderItemId);


            await UpdateOrderasync(_mapper.Map<OrderReq>(order));

            var updatedOrder = await _orderRepository.GetOrderByIdAsync(order.Id);

            return _mapper.Map<OrderRes>(updatedOrder);
        }

        public async Task UpdateOrderasync(OrderReq ordeRreq)
        {
            var order = _mapper.Map<Order>(ordeRreq);
            await _orderRepository.UpdateOrderasync(order);
        }
    }

}
