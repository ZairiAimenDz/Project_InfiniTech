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
                PageSize=12
            });
            return View(products);
        }

        // GET: Products/Details/5
        [HttpGet("Details/{id?}")]
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: Products/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            if (_context.Brands.Count() == 0)
                return RedirectToAction("Create", "Brands");
            if (_context.Categories.Count() == 0)
                return RedirectToAction("Create", "Categories");
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Price,Description,ShortDescription,ThumbnailURL,ImageFile,CategoryId,BrandId,NumberInStock,isStockUnlimited")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ID = Guid.NewGuid();
                product.ThumbnailURL = await fileManager.UploadImage(product.ImageFile);
                await repository.AddProductAsync(product);
                await repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(Guid? id)
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("ID,Name,Price,Description,ShortDescription,ThumbnailURL,CategoryId,BrandId,NumberInStock,ImageFile,isStockUnlimited")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(product.ImageFile != null)
                    {
                        fileManager.DeleteFile(product.ThumbnailURL);
                        product.ThumbnailURL = await fileManager.UploadImage(product.ImageFile);
                    }
                    repository.UpdateProduct(product);
                    await repository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
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
