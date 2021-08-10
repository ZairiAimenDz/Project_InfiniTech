using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using InfiniTech.Data;
using Application.Interfaces;
using Application.Dtos.Product;
using Microsoft.AspNetCore.Authorization;

namespace InfiniTech.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository repository;
        private readonly IFileManager fileManager;

        public ProductsController(ApplicationDbContext context, 
            IProductRepository repository,
            IFileManager fileManager)
        {
            this._context = context;
            this.repository = repository;
            this.fileManager = fileManager;
        }

        // GET: Products
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchterm,int pagenum)
        {
            var products = await repository.GetProductsList(new() {
                SearchTerm=searchterm??"",
                PageNumber=pagenum<1?1:pagenum,
                PageSize=12,
                ShowInvisible = true
            });
            return View(products);
        }

        // GET: Products/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            if (_context.Brands.Count() == 0)
                return RedirectToAction("Create", "Brands");
            if (_context.Categories.Count() == 0)
                return RedirectToAction("Create", "Categories");

            return View();
        }

        // GET: Products/Delete/5
        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await repository.GetProductAsync((Guid)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await repository.GetProductAsync(id);
            fileManager.DeleteFile(product.ThumbnailURL);
            repository.DeleteProduct(product);
            await repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
