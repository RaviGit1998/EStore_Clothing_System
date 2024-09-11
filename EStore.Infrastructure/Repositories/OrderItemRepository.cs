using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
  
        private readonly IOrderRepository _orderRepository;
        private readonly EStoreDbContext _eStoreDbContext;

        public OrderItemRepository( IOrderRepository orderRepository,EStoreDbContext eStoreDbContext)
        {
           
            _orderRepository = orderRepository;
                _eStoreDbContext = eStoreDbContext;
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemid)
        {
            return await _eStoreDbContext.OrderItems.FirstOrDefaultAsync(a=>a.OrderItemId==orderItemid);
        }

        public async Task<Order> RemoveOrderItemAsync(int orderItemId)
        {
            var orderItem = await GetOrderItemByIdAsync(orderItemId);

            if (orderItem == null)
                return null;

            var order = await _orderRepository.GetOrderByIdAsync(orderItem.OrderId);
            if (order == null)
                return null;

            _eStoreDbContext.OrderItems.Remove(orderItem);
            await _eStoreDbContext.SaveChangesAsync();

            order.OrderItems.Remove(orderItem);
            await _orderRepository.UpdateOrderasync(order);

            return order;
        }

    }
}
