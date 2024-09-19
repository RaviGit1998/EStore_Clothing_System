using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos.NewFolder;
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

        public OrderItemRepository(IOrderRepository orderRepository, EStoreDbContext eStoreDbContext)
        {

            _orderRepository = orderRepository;
            _eStoreDbContext = eStoreDbContext;
        }

        public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
        {
            var productVariant = await _eStoreDbContext.ProductVariants
                                 .FirstOrDefaultAsync(pv => pv.ProductVariantId == orderItem.ProductVariantId);
            orderItem.Price = productVariant.PricePerUnit;
            _eStoreDbContext.OrderItems.Add(orderItem);

            await _eStoreDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemid)
        {
            return await _eStoreDbContext.OrderItems.FirstOrDefaultAsync(a => a.OrderItemId == orderItemid);
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

        public async Task<OrderItem> UpdateOrderItemAsync(int orderItemId, OrderItem orderItem)
        {
            var existingOrderItem = await _eStoreDbContext.OrderItems
                                      .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
            if (existingOrderItem == null)
                return null;
            // Fetch the product variant to get the latest price
            var productVariant = await _eStoreDbContext.ProductVariants
                .FirstOrDefaultAsync(pv => pv.ProductVariantId == orderItem.ProductVariantId);
            if (productVariant == null)
                throw new KeyNotFoundException("Product variant not found.");

            existingOrderItem.Price = productVariant.PricePerUnit;

            existingOrderItem.ProductVariantId = orderItem.ProductVariantId;

            _eStoreDbContext.OrderItems.Update(existingOrderItem);
            await _eStoreDbContext.SaveChangesAsync();

            return existingOrderItem;
        }
        /*    public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
            {
                var productVariants = await _eStoreDbContext.ProductVariants
                                         .Where(pv => orderItem.ProductVariantId.Contains(pv.ProductVariantId))
                                         .ToListAsync();

                if (!productVariants.Any())
                    throw new KeyNotFoundException("No valid product variants found.");

                orderItem.Price = productVariants.Sum(pv => pv.PricePerUnit * orderItem.Quantity); // Adjusted for multiple variants
                _eStoreDbContext.OrderItems.Add(orderItem);

                await _eStoreDbContext.SaveChangesAsync();
                return true;
            }

            public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId)
            {
                return await _eStoreDbContext.OrderItems.FirstOrDefaultAsync(a => a.OrderItemId == orderItemId);
            }

            public async Task<Order> RemoveOrderItemAsync(int orderItemId)
            {
                var orderItem = await GetOrderItemByIdAsync(orderItemId);
                if (orderItem == null) return null;

                var order = await _orderRepository.GetOrderByIdAsync(orderItem.OrderId);
                if (order == null) return null;

                _eStoreDbContext.OrderItems.Remove(orderItem);
                await _eStoreDbContext.SaveChangesAsync();

                order.OrderItems.Remove(orderItem);
                await _orderRepository.UpdateOrderasync(order);

                return order;
            }

            public async Task<OrderItem> UpdateOrderItemAsync(int orderItemId, OrderItem orderItem)
            {
                var existingOrderItem = await _eStoreDbContext.OrderItems
                                              .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
                if (existingOrderItem == null) return null;

                var productVariants = await _eStoreDbContext.ProductVariants
                    .Where(pv => orderItem.ProductVariantId.Contains(pv.ProductVariantId))
                    .ToListAsync();

                if (!productVariants.Any())
                    throw new KeyNotFoundException("No valid product variants found.");

                existingOrderItem.ProductVariantId = orderItem.ProductVariantId; // Assuming this is now a collection
                existingOrderItem.Price = productVariants.Sum(pv => pv.PricePerUnit * orderItem.Quantity); // Adjusted for multiple variants

                _eStoreDbContext.OrderItems.Update(existingOrderItem);
                await _eStoreDbContext.SaveChangesAsync();

                return existingOrderItem;
            }

            // New method to fetch product variants by their IDs
            public async Task<List<ProductVariant>> GetProductVariantsByIdsAsync(List<int> productVariantIds)
            {
                return await _eStoreDbContext.ProductVariants
                    .Where(pv => productVariantIds.Contains(pv.ProductVariantId))
                    .ToListAsync();
            }*/
    }
}
