using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Smartphone : Product
    {
        public string Screen { get; set; }
        public int Memory { get; set; }
        public int Sm_Storage { get; set; }
        public string Cameras { get; set; }
        public int BatteryCapacity { get; set; }
    }
}
