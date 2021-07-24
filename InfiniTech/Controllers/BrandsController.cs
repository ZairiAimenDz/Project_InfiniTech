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
using Application.Dtos.Brand;
using Microsoft.AspNetCore.Authorization;

namespace InfiniTech.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class BrandsController : Controller
    {
        private readonly IBrandRepository _repo;
        private readonly IFileManager fileManager;

        public BrandsController(IBrandRepository repo,IFileManager fileManager)
        {
            _repo = repo;
            this.fileManager = fileManager;
        }

        // GET: Brands
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetBrandsList());
        }

        // GET: Brands/Details/5

        [HttpGet("Details/{id?}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _repo.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BrandImage,ImageFile")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.Id = Guid.NewGuid();
                brand.BrandImage = await fileManager.UploadImage(brand.ImageFile);
                _repo.AddBrand(brand);
                await _repo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _repo.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,BrandImage,ImageFile")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(brand.ImageFile != null)
                    {
                        fileManager.DeleteFile(brand.BrandImage);
                        brand.BrandImage = await fileManager.UploadImage(brand.ImageFile);
                    }                    
                    _repo.UpdateBrand(brand);

                    await _repo.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            return View(brand);
        }

        // GET: Brands/Delete/5
        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _repo.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost("Delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var brand = await _repo.GetBrandAsync(id);
            fileManager.DeleteFile(brand.BrandImage);
            _repo.DeleteBrand(brand);
            await _repo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(Guid id)
        {
            return _repo.GetBrandAsync(id) != null;
        }
    }
}
