using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.Entities
{
    public class WishList
    {
        public int WishListId { get; set; }
       // public int ProductVariantId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual User User { get; set; }
      //  public virtual ICollection<ProductVariant> ProductVariants { get; set; }

    }
}
