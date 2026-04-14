using my_own_project.DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.BLL
{
    public class DashboardBLL
    {
        public static DataTable GetSummary(DateTime start, DateTime end) => DashboardDAL.GetSummary(start, end);
        public static DataTable GetRecentOrders(DateTime start, DateTime end) => DashboardDAL.GetRecentOrders(start, end);
        public static DataTable GetRevenueChart(DateTime start, DateTime end) => DashboardDAL.GetRevenueChart(start, end);
        public static DataTable GetTopProducts(DateTime start, DateTime end) => DashboardDAL.GetTopProducts(start, end);
    }
}