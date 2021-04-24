using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
    }
}
