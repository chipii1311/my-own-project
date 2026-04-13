using my_own_project.DAL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.BLL
{
    public class OrderDetailBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateOrderDetail(OrderDetailDTO detail)
        {
            if (detail.OrderID <= 0)
                throw new Exception("OrderID không hợp lệ!");

            if (detail.MenuItemID <= 0)
                throw new Exception("MenuItemID không hợp lệ!");

            if (detail.Quantity <= 0)
                throw new Exception("Số lượng phải lớn hơn 0!");

            if (detail.UnitPrice < 0)
                throw new Exception("Giá không được âm!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddOrderDetail(OrderDetailDTO detail)
        {
            try
            {
                ValidateOrderDetail(detail);
                return OrderDetailDAL.Insert(detail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailBLL.AddOrderDetail Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetOrderDetailsByOrderID(int orderID)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                return OrderDetailDAL.GetByOrderID(orderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailBLL.GetOrderDetailsByOrderID Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool RemoveOrderDetail(int orderDetailID, int orderID)
        {
            try
            {
                if (orderDetailID <= 0)
                    throw new Exception("OrderDetailID không hợp lệ!");

                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                return OrderDetailDAL.Delete(orderDetailID, orderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailBLL.RemoveOrderDetail Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Tính tổng tiền order
        /// </summary>
        public static decimal GetOrderTotal(int orderID)
        {
            try
            {
                DataTable dt = OrderDetailDAL.GetByOrderID(orderID);
                decimal total = 0;

                foreach (DataRow row in dt.Rows)
                {
                    total += (decimal)row["SubTotal"];
                }

                return total;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailBLL.GetOrderTotal Error: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Lấy số lượng item trong order
        /// </summary>
        public static int GetOrderItemCount(int orderID)
        {
            try
            {
                DataTable dt = OrderDetailDAL.GetByOrderID(orderID);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderDetailBLL.GetOrderItemCount Error: {ex.Message}");
                return 0;
            }
        }
    }
}
