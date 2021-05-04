using Application.Dtos.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Brand
{
    public class BrandParameters : BasePaginationParameters
    {
        public string BrandName { get; set; }
    }
}
