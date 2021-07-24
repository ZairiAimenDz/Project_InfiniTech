using Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos.Product
{
    public class ProductParameters : BasePaginationParameters
    {
        public string SearchTerm { get; set; }
        public Guid Categoryid { get; set; }
        public Guid BrandId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double minprice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double maxprice { get; set; }
    }
}
