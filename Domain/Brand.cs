using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
