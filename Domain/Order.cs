using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public BuyerDetails BuyerDetails { get; set; }
        public string UserID { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        public string OrderPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isReady { get; set; }
        public bool isShipping { get; set; }
        public bool isShipped { get; set; }
        public bool isReceived { get; set; }
        public bool isFinished { get; set; }
        public List<OrderedProduct> OrderedProducts { get; set; }
    }
}
