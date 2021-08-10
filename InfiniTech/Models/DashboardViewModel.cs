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
        public double YearlyIncome { get; set; }
        public double YearlyProfit{ get; set; }
        public int YearTotalOrders { get; set; }
        public int YearFinishedOrders { get; set; }
        public double MonthlyIncome { get; set; }
        public double MonthlyProfit { get; set; }
        public int MonthTotalOrders { get; set; }
        public int MonthFinishedOrders { get; set; }
    }
}
