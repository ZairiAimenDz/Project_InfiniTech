using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Product
    {
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double PurchasePrice { get; set; }
        public double OldPrice { get; set; }
        [Required]
        public double Price { get; set; } = 1;
        [Required]
        public string Description { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string ThumbnailURL { get; set; }
        public DateTime DateAdded { get; set; }
        // Foreign Key For Category
        public Category Category { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        //
        public bool Available { get; set; }
        public bool isVisible { get; set; }

        // Foreign Key For Brand
        public Brand Brand { get; set; }
        [Required]
        public Guid BrandId { get; set; }
        //

        public int NumberInStock { get; set; } = 0;
        [Required]
        public bool isStockUnlimited { get; set; }
    }
}
