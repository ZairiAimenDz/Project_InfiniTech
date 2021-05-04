using Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos.Product
{
    public class ProductParameters : BasePaginationParameters
    {
        public Guid Categoryid { get; set; }
        public string name { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double minprice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double maxprice { get; set; }
    }
}
