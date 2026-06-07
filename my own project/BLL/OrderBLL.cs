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
    public class OrderBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateOrder(OrderDTO order)
        {
            // ĐÃ XÓA CHECK RESTAURANT ID

            if (string.IsNullOrWhiteSpace(order.OrderType))
                throw new Exception("Loại đơn hàng không được để trống!");

            if (order.OrderType != "DineIn" && order.OrderType != "TakeAway" && order.OrderType != "Delivery")
                throw new Exception("Loại đơn hàng không hợp lệ!");

            return true;
        }

        // ==================== CREATE ====================
        public static int CreateOrder(OrderDTO order)
        {
            try
            {
                ValidateOrder(order);
                return OrderDAL.Insert(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.CreateOrder Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllOrders()
        {
            try
            {
                return OrderDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.GetAllOrders Error: {ex.Message}");
                throw;
            }
        }

        public static OrderDTO GetOrderByID(int orderID)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                return OrderDAL.GetByID(orderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.GetOrderByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetOrdersByTable(int tableID)
        {
            try
            {
                if (tableID <= 0)
                    throw new Exception("TableID không hợp lệ!");

                return OrderDAL.GetByTable(tableID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.GetOrdersByTable Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetOrdersByStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                    throw new Exception("Status không được để trống!");

                return OrderDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.GetOrdersByStatus Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateOrderStatus(int orderID, string newStatus, int? changedBy = null)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                if (string.IsNullOrWhiteSpace(newStatus))
                    throw new Exception("Status không được để trống!");

                // Validate status
                string[] validStatus = { "Pending", "Cooking", "Ready", "Completed", "Cancelled" };
                bool isValidStatus = false;

                foreach (string status in validStatus)
                {
                    if (status == newStatus)
                    {
                        isValidStatus = true;
                        break;
                    }
                }

                if (!isValidStatus)
                    throw new Exception("Status không hợp lệ!");

                return OrderDAL.UpdateStatus(orderID, newStatus, changedBy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.UpdateOrderStatus Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool CancelOrder(int orderID)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                OrderDTO order = OrderDAL.GetByID(orderID);
                if (order == null)
                    throw new Exception("Đơn hàng không tồn tại!");

                if (order.Status == "Completed" || order.Status == "Cancelled")
                    throw new Exception("Không thể hủy đơn hàng đã hoàn thành hoặc đã hủy!");

                return OrderDAL.Cancel(orderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.CancelOrder Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Hoàn tất đơn hàng sau khi thanh toán: cập nhật Status, TotalAmount, PromotionID, StaffID.
        /// Thay thế cho việc ghép chuỗi SQL trực tiếp ở PaymentForm.
        /// </summary>
        public static bool CompleteOrder(int orderID, decimal totalAmount, int? promotionID, int staffID)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                if (totalAmount < 0)
                    throw new Exception("Tổng tiền không hợp lệ!");

                return OrderDAL.Complete(orderID, totalAmount, promotionID, staffID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.CompleteOrder Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Lấy tổng số đơn hàng
        /// </summary>
        public static int GetTotalOrders()
        {
            try
            {
                DataTable dt = OrderDAL.GetAll();
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.GetTotalOrders Error: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Lấy số đơn hàng theo status
        /// </summary>
        public static int GetOrderCountByStatus(string status)
        {
            try
            {
                DataTable dt = OrderDAL.GetByStatus(status);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OrderBLL.GetOrderCountByStatus Error: {ex.Message}");
                return 0;
            }
        }
    }
}