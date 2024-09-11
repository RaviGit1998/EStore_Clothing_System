using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.EntityDtos.NewFolder
{
    public class OrderReq
    {
        public DateTime OrderDate { get; set; } 
       
        public decimal TotalAmount { get; set; }
        public int orderQuantity { get; set; }
        public bool IsCancelled { get; set; }
        public int? couponId { get; set; }

      
       
    }
}
