using my_own_project.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.DAL
{
    public class OrderDAL
    {
        // ==================== CREATE ====================
        public static int Insert(OrderDTO order)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", order.CustomerID ?? (object)DBNull.Value),
                    new SqlParameter("@RestaurantID", order.RestaurantID ?? (object)DBNull.Value),
                    new SqlParameter("@TableID", order.TableID ?? (object)DBNull.Value),
                    new SqlParameter("@OrderType", order.OrderType ?? "DineIn"),
                    new SqlParameter("@StaffID", order.StaffID ?? (object)DBNull.Value),
                    new SqlParameter("@PromotionID", order.PromotionID ?? (object)DBNull.Value),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Orders_Insert", parameters);
                return (int)parameters[6].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Orders_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static OrderDTO GetByID(int orderID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderID", orderID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Orders_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByTable(int tableID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TableID", tableID)
                };

                return DataHelper.ExecuteSPGetTable("sp_Orders_GetByTable", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.GetByTable Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByStatus(string status)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Status", status)
                };

                return DataHelper.ExecuteSPGetTable("sp_Orders_GetByStatus", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.GetByStatus Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateStatus(int orderID, string newStatus, int? changedBy = null)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderID", orderID),
                    new SqlParameter("@NewStatus", newStatus),
                    new SqlParameter("@ChangedBy", changedBy ?? (object)DBNull.Value)
                };

                DataHelper.ExecuteSP("sp_Orders_UpdateStatus", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.UpdateStatus Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Cancel(int orderID)
        {
            try
            {
                return UpdateStatus(orderID, "Cancelled");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDAL.Cancel Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static OrderDTO MapDTO(DataRow row)
        {
            return new OrderDTO
            {
                OrderID = (int)row["OrderID"],
                CustomerID = row["CustomerID"] != DBNull.Value ? (int)row["CustomerID"] : (int?)null,
                RestaurantID = row["RestaurantID"] != DBNull.Value ? (int)row["RestaurantID"] : (int?)null,
                TableID = row["TableID"] != DBNull.Value ? (int)row["TableID"] : (int?)null,
                OrderDate = (DateTime)row["OrderDate"],
                OrderType = row["OrderType"]?.ToString() ?? "",
                Status = row["Status"]?.ToString() ?? "Pending",
                TotalAmount = row["TotalAmount"] != DBNull.Value ? (decimal)row["TotalAmount"] : 0,
                UpdatedAt = row["UpdatedAt"] != DBNull.Value ? (DateTime)row["UpdatedAt"] : DateTime.Now,
                StaffID = row["StaffID"] != DBNull.Value ? (int)row["StaffID"] : (int?)null,
                PromotionID = row["PromotionID"] != DBNull.Value ? (int)row["PromotionID"] : (int?)null,
                CustomerName = row["CustomerName"]?.ToString() ?? "",
                RestaurantName = row["RestaurantName"]?.ToString() ?? "",
                TableNumber = row["TableNumber"] != DBNull.Value ? (int)row["TableNumber"] : 0,
                StaffName = row["StaffName"]?.ToString() ?? ""
            };
        }
    }
}
