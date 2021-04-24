using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserLikesProduct
    {
        public Product Product { get; set; }
        public Guid ProductId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
