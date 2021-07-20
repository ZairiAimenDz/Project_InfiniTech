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
    [Authorize(Roles ="Admin")]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAnnouncementRepository announcementRepository;
        private readonly IFileManager fileManager;

        public AnnouncementsController(ApplicationDbContext context
            ,IAnnouncementRepository announcementRepository,
            IFileManager fileManager)
        {
            _context = context;
            this.announcementRepository = announcementRepository;
            this.fileManager = fileManager;
        }


        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await announcementRepository.GetAnnouncementAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ImageFile,Title,Text")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.ID = Guid.NewGuid();
                announcement.Image = await fileManager.UploadImage(announcement.ImageFile);
                _context.Add(announcement);
                await _context.SaveChangesAsync();
                return Redirect("/Admin");
            }
            return View(announcement);
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await announcementRepository.GetAnnouncementAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,ImageFile,Title,Text")] Announcement announcement)
        {
            if (id != announcement.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (announcement.ImageFile != null) {
                        fileManager.DeleteFile(announcement.Image);
                        announcement.Image = await fileManager.UploadImage(announcement.ImageFile);
                    }
                    announcementRepository.UpdateAnnouncement(announcement);
                    await announcementRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Admin");
            }
            return View(announcement);
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await announcementRepository.GetAnnouncementAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var announcement = await announcementRepository.GetAnnouncementAsync(id);
            announcementRepository.DeleteAnnouncement(announcement);
            await announcementRepository.SaveAsync();
            return Redirect("/Admin");
        }

        private bool AnnouncementExists(Guid id)
        {
            return _context.Announcements.Any(e => e.ID == id);
        }
    }
}
