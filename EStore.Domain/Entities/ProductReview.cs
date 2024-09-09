using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.Entities
{
    public class ProductReview
    {
        public int ReviewId { get; set; }

     //   public int UserId { get; set; }

      //  public int ProductId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
       // public virtual User User { get; set; }
       // public virtual Product Product { get; set; }

    }
}
