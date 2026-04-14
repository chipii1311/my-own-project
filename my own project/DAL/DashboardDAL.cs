using my_own_project.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using my_own_project.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DAL
{
    public class DashboardDAL
    {
        private static DataTable GetDataWithDateRange(string spName, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            return DataHelper.ExecuteSPGetTable(spName, parameters);
        }

        public static DataTable GetSummary(DateTime startDate, DateTime endDate)
            => GetDataWithDateRange("sp_Dashboard_GetSummary", startDate, endDate);

        public static DataTable GetRecentOrders(DateTime startDate, DateTime endDate)
            => GetDataWithDateRange("sp_Dashboard_GetRecentOrders", startDate, endDate);

        public static DataTable GetRevenueChart(DateTime startDate, DateTime endDate)
            => GetDataWithDateRange("sp_Dashboard_GetRevenueByDate", startDate, endDate);

        public static DataTable GetTopProducts(DateTime startDate, DateTime endDate)
            => GetDataWithDateRange("sp_Dashboard_GetTopProducts", startDate, endDate);
    }
}