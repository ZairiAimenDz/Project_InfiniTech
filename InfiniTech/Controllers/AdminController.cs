using Application.Interfaces;
using InfiniTech.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAnnouncementRepository _repo;

        public AdminController(IAnnouncementRepository repository)
        {
            _repo = repository;
        }
        public async Task<IActionResult> Index()
        {
            var viewmodel = new DashboardViewModel {
                Announcements = await _repo.GetAnnouncements(),
            };
            return View(viewmodel);
        }
    }
}
