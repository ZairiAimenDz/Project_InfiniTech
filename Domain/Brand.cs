using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string BrandImage { get; set; }
        [NotMapped]
        public IFormFile ImageFile{ get; set; }

        public List<Product> Products { get; set; }
    }
}
