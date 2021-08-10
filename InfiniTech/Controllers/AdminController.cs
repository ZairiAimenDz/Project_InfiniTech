using Application.Interfaces;
using Domain;
using InfiniTech.Data;
using InfiniTech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAnnouncementRepository _repo;
        private readonly ApplicationDbContext context;

        public AdminController(IAnnouncementRepository repository, ApplicationDbContext context)
        {
            _repo = repository;
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var viewmodel = new DashboardViewModel
            {
                Announcements = await _repo.GetAnnouncements(),
                YearTotalOrders = context.Orders.Where(o => o.OrderDate.Year == DateTime.Now.Year).Count(),
                YearFinishedOrders = context.Orders.Where(o => o.OrderDate.Year == DateTime.Now.Year && o.State == DeliveryState.isFinished).Count(),
                YearlyIncome = context.OrderedProducts.Include(o => o.Order).Include(o => o.Product).Where(o => o.Order.OrderDate.Year == DateTime.Now.Year).Sum(o => o.Product.Price),
                YearlyProfit = context.OrderedProducts.Include(o => o.Order).Include(o => o.Product).Where(o => o.Order.OrderDate.Year == DateTime.Now.Year).Sum(o => (o.Product.Price - o.Product.PurchasePrice)),
                MonthTotalOrders = context.Orders.Where(o => o.OrderDate.Month == DateTime.Now.Month).Count(),
                MonthFinishedOrders = context.Orders.Where(o => o.OrderDate.Month == DateTime.Now.Month && o.State == DeliveryState.isFinished).Count(),
                MonthlyIncome = context.OrderedProducts.Include(o => o.Order).Include(o => o.Product).Where(o => o.Order.OrderDate.Month == DateTime.Now.Month).Sum(o => o.Product.Price),
                MonthlyProfit = context.OrderedProducts.Include(o => o.Order).Include(o => o.Product).Where(o => o.Order.OrderDate.Month == DateTime.Now.Month).Sum(o => (o.Product.Price - o.Product.PurchasePrice)),
            };
            return View(viewmodel);
        }
    }
}
