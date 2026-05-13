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
    public class InventoryTransactionBLL
    {
        public static void ImportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            if (ingredientID <= 0)
                throw new Exception("Nguyên liệu không hợp lệ.");

            if (quantity <= 0)
                throw new Exception("Số lượng nhập phải lớn hơn 0.");

            if (staffID <= 0)
                throw new Exception("Không tìm thấy thông tin nhân viên.");

            InventoryTransactionDAL.ImportIngredient(ingredientID, quantity, staffID, note);
        }

        public static void ExportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            if (ingredientID <= 0)
                throw new Exception("Nguyên liệu không hợp lệ.");

            if (quantity <= 0)
                throw new Exception("Số lượng xuất phải lớn hơn 0.");

            if (staffID <= 0)
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

        public static void ExportByOrderRecipe(int orderID, int staffID, string note)
        {
            if (orderID <= 0)
                throw new Exception("Hóa đơn không hợp lệ.");

            if (staffID <= 0)
                throw new Exception("Không tìm thấy thông tin nhân viên.");

            InventoryTransactionDAL.ExportByOrderRecipe(orderID, staffID, note);
        }
    }
}

