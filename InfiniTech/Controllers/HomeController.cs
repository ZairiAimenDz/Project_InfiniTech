using Application.Dtos.Product;
using Application.Interfaces;
using InfiniTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger
            ,IProductRepository productrepo,
            ICategoryRepository categoryrepo,
            IBrandRepository brandsrepo,
            IAnnouncementRepository announcementrepo)
        {
            _logger = logger;
            this.productrepo = productrepo;
            this.categoryrepo = categoryrepo;
            this.brandsrepo = brandsrepo;
            this.announcementrepo = announcementrepo;
        }

        public async Task<IActionResult> Index()
        {
            // TODO : Add Announcements
            var viewmodel = new HomePageViewModel() {
                Announcements = await announcementrepo.GetAnnouncements(),
                LatestProducts = await productrepo.GetLatestProductsList(),
                RandomProducts = await productrepo.GetRandomProductsList(),
                RandomCategories = (await categoryrepo.GetCategoryList()).Take(6),
                Brands = await brandsrepo.GetBrandsList(new Application.Dtos.Brand.BrandParameters())
            };
            return View(viewmodel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("/Details/{id}")]
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

            return View(product);
        }

        [Route("/Catalogue")]
        public async Task<IActionResult> AllProducts([FromQuery] ProductParameters parameters)
        {
            var products = await productrepo.GetProductsList(parameters);
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
