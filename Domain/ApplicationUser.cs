using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public List<Order> Orders { get; set; }
        public List<UserFavProduct> LikedProducts { get; set; }
    }
}
