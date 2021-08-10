using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public double Price { get; set; }
        public string Description { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string ThumbnailURL { get; set; }
        // Delete This If Not Needed !
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public DateTime DateAdded { get; set; }
        // Foreign Key For Category
        public Category Category { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        //
        public IEnumerable<ProductImage> ProductImages { get; set; }

        public bool isVisible { get; set; }

        public string Color { get; set; }
        public ProductCondition ProductCondition { get; set; }

        // Foreign Key For Brand
        public Brand Brand { get; set; }
        [Required]
        public Guid BrandId { get; set; }
        //
        [Required]
        public int NumberInStock { get; set; }
    }

    public enum ProductCondition
    {
        [Description("New, Sealed From Factory")]
        InBox,
        [Description("New With Opened Box")]
        NewOpenBox,
        [Description("Refurbished")]
        Refurbished,
        [Description("Used Like New")]
        UsedLikeNew,
        [Description("Good Condition")]
        UsedGoodCondition,
        [Description("Bad Condition")]
        BadCondition,        
    }

    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImageFile { get; set; }
    }
}
