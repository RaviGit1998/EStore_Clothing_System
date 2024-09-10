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
    public class OrderRepository : IOrderRepository
    {
        private readonly EStoreDbContext _eStoreDbContext;

        public OrderRepository(EStoreDbContext eStoreDbContext)
        {
            _eStoreDbContext = eStoreDbContext;
        }

        public async Task<bool> ApplyCouponAsync(int orderId, string couponCode)
        {
            var order=await GetOrderByIdAsync(orderId);

            if (order == null) 
                return false;

            var coupon=await _eStoreDbContext.Coupons
                        .FirstOrDefaultAsync(c=>c.CouponCode == couponCode);    

            if (coupon == null || !coupon.IsActive || coupon.ExpirationDate<DateTime.Now) 
                return false;

            //Applying Coupon Siscount to the Order
            order.CouponId = coupon.CouponId;
            _eStoreDbContext.Orders.Update(order);
            await _eStoreDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateTotalAmountAsync(int orderId)
        {
            var order=await GetOrderByIdAsync(orderId);

            if (order == null)
                throw new ArgumentException("Order was Not Found");

            decimal totalAmount = 0;
              totalAmount=order.OrderItems.Sum(o=>o.Quantity*o.Price);

            //Applying Coupon 
            if (order.CouponId.HasValue)
            {
                var coupon = await _eStoreDbContext.Coupons.FindAsync(order.CouponId.HasValue);
                if(coupon !=null && coupon.IsActive && coupon.ExpirationDate >= DateTime.Now)
                {
                    totalAmount -= coupon.DiscountedAmount;
                }
            }
            return totalAmount;
        }

        public async Task<Order> CancelOrderAsync(int orderId)
        {
           var order=await GetOrderByIdAsync(orderId);
            if(order == null)
            {
                return null;
            }
            order.IsCancelled = true;
            _eStoreDbContext.Orders.Update(order);
            await _eStoreDbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order> CreateAnOrderAsync(Order order)
        {
            _eStoreDbContext.Orders.Add(order);
            await _eStoreDbContext.SaveChangesAsync();
            return order;   
                   
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order=await _eStoreDbContext.Orders
                            .Include(o=>o.OrderItems)
                            .Include(o=>o.Coupon)
                            .Include(o=>o.Payment)
                            .FirstOrDefaultAsync(o=>o.Id==orderId);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var userOrders = await _eStoreDbContext.Orders
                            .Where(o => o.UserId == userId)
                            .Include(o => o.OrderItems)
                            .Include(o => o.Coupon)
                            .Include(o => o.Payment)
                            .ToListAsync();
            return userOrders;
        }

        public async Task<bool> ProcessPayment(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if(order == null)
               return false;

            var payment = new Payment
            {
                OrderId= orderId,
                Amount=order.TotalAmount,
                Date=DateTime.Now,
                PaymentMode="Credit Card",
                PaymentStatus="Success"
            };

            _eStoreDbContext.Payment.Add(payment);
            await _eStoreDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<Order> RemoveOrderItemAsync(int orderItemId)
        {
           var orderItem=await _eStoreDbContext.OrderItems.FindAsync(orderItemId);

            if (orderItem == null)
                return null;

            var order=await GetOrderByIdAsync(orderItem.OrderId);
            if (order == null)
                return null;

            _eStoreDbContext.OrderItems.Remove(orderItem);
            await _eStoreDbContext.SaveChangesAsync();

            return order;
        }
    }
}
