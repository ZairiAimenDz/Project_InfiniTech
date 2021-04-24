using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Laptop : Product
    {
        public string CPU { get; set; }
        public string GPU { get; set; }
        public int RAM { get; set; }
        public string Display { get; set; }
        public int Storage { get; set; }
    }
}
