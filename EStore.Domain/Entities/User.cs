using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.Entities
{
    public class User
    {

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedDate { get; set; }

        //navigation Properties 
      //  public virtual ICollection<Order> Orders { get; set; }
      //  public virtual WishList WishList { get; set; }

       // public virtual ICollection<ProductReview> ProductReviews { get; set; }

     //   public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; }

    }
}
