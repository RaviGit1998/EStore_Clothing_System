using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.IRepositories
{
    public interface IOrderItemRepository
    {

        Task<OrderItem> GetOrderItemByIdAsync(int orderItemid);

        //Remove an Order item from the Orders
        Task<Order> RemoveOrderItemAsync(int orderItemId);
    }
}
