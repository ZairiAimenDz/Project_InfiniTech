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
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public DeliveryState State { get; set; }
        public List<OrderedProduct> OrderedProducts { get; set; }
    }

    public enum DeliveryState
    {
        JustCreated,// Created The Order
        ForCheckUp, // Added A Payment Method And Payment Evidence
        Invalid,    // Payment Invalid
        isReady,    // Order Ready
        isShipping, // Order Getting Shipped
        isShipped,  // The Order Reached Its Destination
        isReceived, // The Order Has Been Received
        isFinished
    }
}
