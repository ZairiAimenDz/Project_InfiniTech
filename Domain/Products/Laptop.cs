using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Laptop : Product
    {
        public string LaptopCPU { get; set; }
        public string LaptopGPU { get; set; }
        public int LaptopRAM { get; set; }
        public string LaptopDisplay { get; set; }
        public int LaptopStorage { get; set; }
    }
}
