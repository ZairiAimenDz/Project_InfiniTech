using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Review
    {
        public Guid id { get; set; }

        public ApplicationUser Reviewer { get; set; }
        public string ReviewerID { get; set; }

        public DateTime DatePosted { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        [Required]
        public string Reviewtext { get; set; }
        [Required]
        public int Score { get; set; }
    }
}
