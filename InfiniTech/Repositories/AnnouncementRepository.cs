using Application.Interfaces;
using Domain;
using InfiniTech.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public void AddAnnouncement(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
        }

        public async Task AddAnnouncementAsync(Announcement announcement)
        {
            await _context.Announcements.AddAsync(announcement);
        }

        public void DeleteAnnouncement(Announcement announcement)
        {
            _context.Announcements.Remove(announcement);
        }

        public Announcement GetAnnouncement(Guid? announcID)
        {
            return _context.Announcements.FirstOrDefault(a => a.ID == announcID);
        }

        public Task<Announcement> GetAnnouncementAsync(Guid? announcID)
        {
            return _context.Announcements.FirstOrDefaultAsync(a => a.ID == announcID);
        }

        public Task<List<Announcement>> GetAnnouncements()
        {
            return _context.Announcements.ToListAsync();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateAnnouncement(Announcement announcement)
        {
            _context.Announcements.Update(announcement);
        }
    }
}
