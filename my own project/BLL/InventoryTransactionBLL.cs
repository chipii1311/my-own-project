using my_own_project.DAL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace my_own_project.BLL
{
    public class InventoryTransactionBLL
    {
        /// <summary>
        /// Nhập kho nguyên liệu.
        /// staffID = 0 được chấp nhận khi người dùng là Admin (StaffID sẽ là NULL trong DB).
        /// </summary>
        public static void ImportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            if (ingredientID <= 0)
                throw new Exception("Nguyên liệu không hợp lệ.");

            if (quantity <= 0)
                throw new Exception("Số lượng nhập phải lớn hơn 0.");

            // staffID == 0 → Admin (cho phép, truyền NULL vào DB)
            // staffID < 0  → lỗi thực sự
            if (staffID < 0)
                throw new Exception("Không tìm thấy thông tin nhân viên.");

            InventoryTransactionDAL.ImportIngredient(ingredientID, quantity, staffID, note);
        }

        /// <summary>
        /// Xuất kho nguyên liệu.
        /// staffID = 0 được chấp nhận khi người dùng là Admin (StaffID sẽ là NULL trong DB).
        /// </summary>
        public static void ExportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            if (ingredientID <= 0)
                throw new Exception("Nguyên liệu không hợp lệ.");

            if (quantity <= 0)
                throw new Exception("Số lượng xuất phải lớn hơn 0.");

            if (staffID < 0)
                throw new Exception("Không tìm thấy thông tin nhân viên.");

            InventoryTransactionDAL.ExportIngredient(ingredientID, quantity, staffID, note);
        }

        public static DataTable GetRecentTransactions()
        {
            return InventoryTransactionDAL.GetRecentTransactions();
        }

        public static DataTable CheckStockForOrder(int orderID)
        {
            if (orderID <= 0)
                throw new Exception("Hóa đơn không hợp lệ.");

            return InventoryTransactionDAL.CheckStockForOrder(orderID);
        }

        /// <summary>
        /// Xuất kho theo công thức của đơn hàng.
        /// staffID = 0 được chấp nhận cho Admin.
        /// </summary>
        public static void ExportByOrderRecipe(int orderID, int staffID, string note)
        {
            if (orderID <= 0)
                throw new Exception("Hóa đơn không hợp lệ.");

            if (staffID < 0)
                throw new Exception("Không tìm thấy thông tin nhân viên.");

            InventoryTransactionDAL.ExportByOrderRecipe(orderID, staffID, note);
        }
    }
}