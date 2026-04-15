using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.DAL
{
    public class OrderDetailDAL
    {
        // ==================== CREATE ====================
        public static int Insert(OrderDetailDTO detail)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderID", detail.OrderID),
                    new SqlParameter("@MenuItemID", detail.MenuItemID),
                    new SqlParameter("@Quantity", detail.Quantity),
                    new SqlParameter("@Note", detail.Note ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_OrderDetail_Insert", parameters);
                return Convert.ToInt32(parameters[4].Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetByOrderID(int orderID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderID", orderID)
                };

                return DataHelper.ExecuteSPGetTable("sp_OrderDetail_GetByOrderID", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailDAL.GetByOrderID Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int orderDetailID, int orderID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderDetailID", orderDetailID),
                    new SqlParameter("@OrderID", orderID)
                };

                DataHelper.ExecuteSP("sp_OrderDetail_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static OrderDetailDTO MapDTO(DataRow row)
        {
            return new OrderDetailDTO
            {
                OrderDetailID = (int)row["OrderDetailID"],
                OrderID = (int)row["OrderID"],
                MenuItemID = (int)row["MenuItemID"],
                Quantity = (int)row["Quantity"],
                UnitPrice = (decimal)row["UnitPrice"],
                SubTotal = (decimal)row["SubTotal"],
                Note = row["Note"]?.ToString() ?? "",
                ItemName = row["ItemName"]?.ToString() ?? ""
            };
        }
    }
}
