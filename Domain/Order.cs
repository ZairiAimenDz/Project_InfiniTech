using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ShippingAddress { get; set; }
        public string OrderPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isReady { get; set; }
        public bool isShipping { get; set; }
        public bool isShipped { get; set; }
        public bool isReceived { get; set; }
        public List<OrderedProducts> OrderedProducts { get; set; }
    }
}
