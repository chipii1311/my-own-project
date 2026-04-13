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
    public class PaymentBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidatePayment(PaymentDTO payment)
        {
            if (payment.OrderID <= 0)
                throw new Exception("OrderID không hợp lệ!");

            if (string.IsNullOrWhiteSpace(payment.Method))
                throw new Exception("Phương thức thanh toán không được để trống!");

            if (payment.Amount <= 0)
                throw new Exception("Số tiền thanh toán phải lớn hơn 0!");

            string[] validMethods = { "Cash", "Card", "Bank Transfer", "E-Wallet" };
            bool isValidMethod = false;

            foreach (string method in validMethods)
            {
                if (method == payment.Method)
                {
                    isValidMethod = true;
                    break;
                }
            }

            if (!isValidMethod)
                throw new Exception("Phương thức thanh toán không hợp lệ!");

            return true;
        }

        // ==================== CREATE ====================
        public static int CreatePayment(PaymentDTO payment)
        {
            try
            {
                ValidatePayment(payment);

                // Kiểm tra order tồn tại
                OrderDTO order = OrderDAL.GetByID(payment.OrderID);
                if (order == null)
                    throw new Exception("Đơn hàng không tồn tại!");

                return PaymentDAL.Insert(payment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentBLL.CreatePayment Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static PaymentDTO GetPaymentByOrderID(int orderID)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                return PaymentDAL.GetByOrderID(orderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentBLL.GetPaymentByOrderID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetAllPayments()
        {
            try
            {
                return PaymentDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentBLL.GetAllPayments Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Kiểm tra đơn hàng đã thanh toán chưa
        /// </summary>
        public static bool IsOrderPaid(int orderID)
        {
            try
            {
                PaymentDTO payment = PaymentDAL.GetByOrderID(orderID);
                return payment != null && payment.Status == "Completed";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentBLL.IsOrderPaid Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy tổng doanh thu
        /// </summary>
        public static decimal GetTotalRevenue()
        {
            try
            {
                DataTable dt = PaymentDAL.GetAll();
                decimal total = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Status"].ToString() == "Completed")
                        total += (decimal)row["Amount"];
                }

                return total;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentBLL.GetTotalRevenue Error: {ex.Message}");
                return 0;
            }
        }
    }
}
