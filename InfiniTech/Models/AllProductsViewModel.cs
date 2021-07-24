using Application.Dtos.Product;
using Application.Wrappers;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Models
{
    public class AllProductsViewModel
    {
        public PagedList<Product> Products{ get; set; }
        public ProductParameters Parameters{ get; set; }
    }
}
