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

       // public int ProductId { get; set; }

        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }

       // public virtual Product Product { get; set; }

    }
}
