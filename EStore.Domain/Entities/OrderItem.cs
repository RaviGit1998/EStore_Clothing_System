﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.Entities
{
    public class OrderItem
    {

        public int OrderItemId { get; set; }

       // public int OrderId { get; set; }

      //  public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation Properties
   //    public virtual Order Order { get; set; }
      //  public virtual Product Products { get; set; }

    }
}
