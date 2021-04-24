using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Category ParentCategory { get; set; }
        public Guid ParentCategoryId { get; set; }
        
        // Navigation Property
        public List<Product> Products { get; set; }
    }
}
