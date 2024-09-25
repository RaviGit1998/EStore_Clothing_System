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

        public async Task<OrderRes> CancelOrderById(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var order = await GetOrderByIdAsync(orderId);
            if (order.status == "Cancelled")
                throw new InvalidOperationException("Order is already Canceled");

            var orderRes = await _orderRepository.CancelOrderById(orderId);
            return _mapper.Map<OrderRes>(orderRes);
        }

        public async Task<OrderRes> ChangeStatusOfOrder(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.");

            var order = await GetOrderByIdAsync(orderId);
            if (order.status== "Confirmed")
                throw new InvalidOperationException("Order is in confirmed");

            var orderRes= await _orderRepository.ChangeStatusOfOrder(orderId);
            return _mapper.Map<OrderRes>(orderRes);
        }

        public async Task<OrderRes> CreateAnOrderAsync(OrderReq orderReq)
        {
            /*var order=_mapper.Map<Order>(orderReq);
            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new ArgumentException("Order must contain at least one order item.");
           
            var createdOrder= await _orderRepository.CreateAnOrderAsync(order);
            return _mapper.Map<OrderRes>(createdOrder);*/
        
            var productVariantPrices = await _orderRepository.GetProductVariantPricesAsync(orderReq.OrderItemreq.Select(item => item.ProductVariantId).ToList());

           // var order = _mapper.Map<Order>(orderReq);

            if (orderReq.OrderItemreq == null || !orderReq.OrderItemreq.Any())
                throw new ArgumentException("Order must contain at least one order item.");

            var orderItems=new List<OrderItem>();
            int totalQuantity = 0;
            foreach (var itemReq in orderReq.OrderItemreq)
            {
                if (productVariantPrices.TryGetValue(itemReq.ProductVariantId, out var price))
                {
                    var orderItem = new OrderItem
                    {
                        ProductVariantId = itemReq.ProductVariantId,
                        Quantity=itemReq.Quantity,
                        Price=price
                        
                    };
                    orderItems.Add(orderItem); // Add to the order's items

                    totalQuantity += itemReq.Quantity;
                }
                else
                {
                    throw new KeyNotFoundException($"Product variant price not found for ID {itemReq.ProductVariantId}");
                }
            }
          
            var order=_mapper.Map<Order>(orderReq);
            
            order.OrderItems = orderItems;
            order.OrderQuantity = totalQuantity;
          
            var createdOrder = await _orderRepository.CreateAnOrderAsync(order);
            await _orderRepository.UpdateOrderasync(createdOrder);

            return _mapper.Map<OrderRes>(createdOrder);
        }

        public async Task<bool> DeleteOrderByIdAsync(int orderId)
        {
           return await _orderRepository.DeleteOderByIdAsync(orderId);
            
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

     

        public async Task UpdateOrderasync(OrderReq ordeRreq)
        {
            var order = _mapper.Map<Order>(ordeRreq);
            await _orderRepository.UpdateOrderasync(order);
        }

      
    }

}
