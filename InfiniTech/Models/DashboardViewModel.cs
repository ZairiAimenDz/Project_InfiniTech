using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Announcement> Announcements{ get; set; }
    }
}
