using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Product
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailURL { get; set; }
        public DateTime DateAdded { get; set; }
        // Foreign Key For Category
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        //

        // Foreign Key For Brand
        public Brand Brand { get; set; }
        public Guid BrandId { get; set; }
        //

        public int NumberInStock { get; set; }
        public bool isStockUnlimited { get; set; }
    }
}
