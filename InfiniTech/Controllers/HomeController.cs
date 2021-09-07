using Application.Dtos.Product;
using Application.Interfaces;
using Domain;
using InfiniTech.Data;
using InfiniTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository productrepo;
        private readonly ICategoryRepository categoryrepo;
        private readonly IBrandRepository brandsrepo;
        private readonly IAnnouncementRepository announcementrepo;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger
            , IProductRepository productrepo,
            ICategoryRepository categoryrepo,
            IBrandRepository brandsrepo,
            IAnnouncementRepository announcementrepo,
            ApplicationDbContext context)
        {
            _logger = logger;
            this.productrepo = productrepo;
            this.categoryrepo = categoryrepo;
            this.brandsrepo = brandsrepo;
            this.announcementrepo = announcementrepo;
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewmodel = new HomePageViewModel()
            {
                Announcements = await announcementrepo.GetAnnouncements(),
                LatestProducts = await productrepo.GetLatestProductsList(),
                RandomProducts = await productrepo.GetRandomProductsList(),
                RandomCategories = (await categoryrepo.GetCategoryList()).Take(6),
                Brands = await brandsrepo.GetBrandsList()
            };
            return View(viewmodel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("/SearchForOrder")]
        public IActionResult SearchForOrder([FromQuery] Guid? id)
        {
            if (id == null)
                return View();
            else
                return Redirect($"/OrderDetails/{id}");
        }

        [Authorize]
        [Route("/User/OrdersList")]
        public async Task<IActionResult> UserOrdersList([FromQuery] int pagenum = 1)
        {
            var orders = await _context.Orders.Where(o => o.UserID == User.Claims.FirstOrDefault().Value).Skip((pagenum - 1) * 9).Take(12).ToListAsync();
            ViewData["hasnext"] = orders.Count() == 9;
            ViewData["hasprev"] = pagenum > 1;
            ViewData["currentpage"] = pagenum;
            return View(orders);
        }

        [Authorize]
        [Route("/User/Wishlist")]
        public async Task<IActionResult> UserWishList([FromQuery] int pagenum = 1)
        {
            var wishlisted = await _context.UserLikedProducts.Where(o => o.ApplicationUserId == User.Claims.FirstOrDefault().Value)
                                        .OrderBy(o=>o.ProductId).Skip((pagenum-1)*9).Take(12).ToListAsync();
            var products = new List<Product>();
            wishlisted.ForEach(o => products.Add(productrepo.GetProduct(o.ProductId)));

            ViewData["hasnext"] = wishlisted.Count() == 9;
            ViewData["hasprev"] = pagenum > 1;
            ViewData["currentpage"] = pagenum;
            return View(products);
        }

        [HttpGet("/Details/{id}", Name = "Details")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await productrepo.GetProductAsync((Guid)id);
            if (product == null)
            {
                return NotFound();
            }

            product.Views++;
            productrepo.UpdateProduct(product);
            await productrepo.SaveAsync();
            
            var viewmodel = new ProductDetailsViewModel() {
                ProdDetails = product,
                OtherProducts = await productrepo.GetRandomProductsList(),
            };

            return View(viewmodel);
        }

        [Route("/CompareProducts")]
        public IActionResult CompareProducts([FromQuery] Guid ID)
        {
            return View(ID);
        }

        [Route("/Catalogue")]
        public async Task<IActionResult> AllProducts([FromQuery] ProductParameters parameters, int pagenum = 1)
        {
            parameters.PageNumber = pagenum;
            ViewData["BrandId"] = new SelectList(await brandsrepo.GetBrandsList(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(await categoryrepo.GetCategoryList(), "Id", "Name");
            var viewmodel = new AllProductsViewModel { Products = await productrepo.GetProductsList(parameters), Parameters = new() };
            return View(viewmodel);
        }

        [Route("/Cart")]
        public IActionResult Cart()
        {
            return View();
        }

        [HttpGet("/OrderDetails/{id}")]
        public async Task<IActionResult> OrderDetails(Guid id)
        {
            var order = await _context.Orders.Include(o => o.BuyerDetails).Include(o => o.OrderedProducts).FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                var viewmodel = new OrderDetailsViewModel()
                {
                    Order = order,
                    OrderedProduct = new(),
                    Buyer = order.BuyerDetails
                };
                order.OrderedProducts.ForEach(o => viewmodel.OrderedProduct.Add(_context.Products.Include(o => o.Brand).FirstOrDefault(p => p.ID == o.ProductId)));
                return View(viewmodel);
            }
            else
                return Redirect("/SearchForOrder");
        }

        [HttpPost("/OrderDetails/{id}")]
        public async Task<IActionResult> ChangePaymentMehtod(Guid id, [Bind("ID,ClientName,")] BuyerDetails details)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(details);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(OrderDetails), new { id = id });
            }
            var order = await _context.Orders.Include(o => o.BuyerDetails).Include(o => o.OrderedProducts).FirstOrDefaultAsync(o => o.Id == id);
            var viewmodel = new OrderDetailsViewModel()
            {
                Order = order,
                OrderedProduct = new(),
                Buyer = order.BuyerDetails
            };
            order.OrderedProducts.ForEach(o => viewmodel.OrderedProduct.Add(_context.Products.Include(o => o.Brand).FirstOrDefault(p => p.ID == o.ProductId)));
            return View(viewmodel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
