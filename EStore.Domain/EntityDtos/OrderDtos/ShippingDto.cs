using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.EntityDtos.OrderDtos
{
    public class ShippingDto
    {
      
        public string TrackingNumber { get; set; }      
        public DateTime EstimatedDeliveryDate { get; set; }
    }
}
