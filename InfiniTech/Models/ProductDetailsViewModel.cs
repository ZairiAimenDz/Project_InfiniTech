using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Models
{
    public class ProductDetailsViewModel
    {
        public Product ProdDetails{ get; set; }
        public IEnumerable<Product> OtherProducts { get; set; }
        // TODO : Comments And Ratings
    }
}
