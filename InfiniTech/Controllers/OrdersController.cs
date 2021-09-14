using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using InfiniTech.Data;
using Microsoft.AspNetCore.Authorization;

namespace InfiniTech.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [HttpGet("")]
        public async Task<IActionResult> Index(Guid? OrderCode)
        {
            /// TODO : Add Pagination - Maybe With The Clean Code Structure
            var orders = _context.Orders.Include(o => o.BuyerDetails).AsQueryable();
            if (OrderCode != null)
            {
                orders = OrderCode != Guid.Empty ? orders.Where(o => o.Id == OrderCode) : orders;
            }
            return View(await orders.ToListAsync());
        }


        // GET: Orders/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
