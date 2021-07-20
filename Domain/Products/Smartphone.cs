using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Smartphone : Product
    {
        public string SPScreen { get; set; }
        public int SPMemory { get; set; }
        public int SPStorage { get; set; }
        public string SPCamera { get; set; }
        public int SPBattery { get; set; }
    }
}
