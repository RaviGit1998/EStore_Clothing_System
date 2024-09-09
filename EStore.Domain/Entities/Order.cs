using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
      //  public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
     //   public int? CouponId { get; set; }

        public string OrderStatus { get; set; }

        // Navigation Properties
       /* public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Coupon Coupon { get; set; }*/
    }
}
