using Application.Dtos.Product;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InfiniTech.Controllers.v1
{
    [Route("api/[Controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> AllProducts([FromQuery]ProductParameters parameters)
        {
            var coll = await repository.GetProductsList(parameters);
            // Return Paged List Products

            var paginationMetadata = new
            {
                totalCount = coll.TotalCount,
                pageSize = coll.PageSize,
                currentPageSize = coll.CurrentPageSize,
                currentStartIndex = coll.CurrentStartIndex,
                currentEndIndex = coll.CurrentEndIndex,
                pageNumber = coll.PageNumber,
                totalPages = coll.TotalPages,
                hasPrevious = coll.HasPrevious,
                hasNext = coll.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(new { Products = coll.ToList() });
        }

        // GET api/<ProductController>/5
        [HttpGet("{productid}")]
        public async Task<IActionResult> GetProduct(Guid productid)
        {
            var product = await repository.GetProductAsync(productid);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

    }
}
