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
using Microsoft.AspNetCore.Authorization;

namespace InfiniTech.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _repository;
        private readonly IFileManager fileManager;

        public CategoriesController(ICategoryRepository repository,IFileManager FileManager)
        {
            _repository = repository;
            fileManager = FileManager;
        }

        // GET: Categories
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetCategoryList());
        }

        // GET: Categories/Details/5
        [HttpGet("Details/{id?}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _repository.GetCategoryAsync((Guid)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                category.CategoryImage = await fileManager.UploadImage(category.ImageFile);
                await _repository.AddCategoryAsync(category);
                await _repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [HttpGet("Edit/{id?}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _repository.GetCategoryAsync((Guid)id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,CategoryImage,ImageFile")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ImageFile != null)
                    {
                        fileManager.DeleteFile(category.CategoryImage);
                        category.CategoryImage = await fileManager.UploadImage(category.ImageFile);
                    }
                    _repository.UpdateCategory(category);
                    await _repository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _repository.GetCategoryAsync((Guid)id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost("Delete/{id?}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _repository.GetCategoryAsync(id);
            fileManager.DeleteFile(category.CategoryImage);
            _repository.DeleteCategory(category);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _repository.GetCategory(id) != null;
        }
    }
}
