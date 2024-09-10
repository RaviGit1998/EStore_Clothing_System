using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.EntityDtos.NewFolder
{
    public class OrderRes
    {
        public int Id { get; set; }
       
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int? CouponId { get; set; }
        public bool IsCancelled { get; set; }
        public List<OrderItemRes> OrderItems { get; set; }
       
        

    }
}
