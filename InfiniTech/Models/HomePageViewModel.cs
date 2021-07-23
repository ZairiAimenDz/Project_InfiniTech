using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Product> LatestProducts{ get; set; }
        public IEnumerable<Product> RandomProducts{ get; set; }
        public IEnumerable<Category> RandomCategories{ get; set; }
        public IEnumerable<Brand> Brands{ get; set; }
        public IEnumerable<Announcement> Announcements{ get; set; }
    }
}
