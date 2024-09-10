using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Interfaces
{
    public interface IOrderService
    {
        //create an Order
        Task<Order> CreateAnOrderAsync(Order order);

        //Getting and order by its Id
        Task<Order> GetOrderByIdAsync(int orderId);

        //Getting all Orders of a Specific User
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);

        //Cancelling and Order
        Task<Order> CancelOrderAsync(int orderId);

        //Remove an Order item from the Orders
        Task<Order> RemoveOrderItemAsync(int orderItemId);

        //Applying a Coupon
        Task<bool> ApplyCouponAsync(int orderId, string couponCode);

        //Calculating the Total amount of Order
        Task<decimal> CalculateTotalAmountAsync(int orderId);

        //IsPayment Success or not for Order
        Task<bool> ProcessPayment(int orderId);
    }
}
