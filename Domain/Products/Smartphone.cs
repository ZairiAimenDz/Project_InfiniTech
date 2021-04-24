using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Smartphone : Product
    {
        public string Screen { get; set; }
        public int Ram { get; set; }
        public int Storage { get; set; }
        public string Cameras { get; set; }
        public int BatteryCapacity { get; set; }
    }
}
