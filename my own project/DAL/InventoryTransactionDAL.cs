using my_own_project.DAL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace my_own_project.DAL
{
    public class InventoryTransactionDAL
    {
        /// <summary>
        /// Nhập kho. Nếu staffID = 0 (Admin) → truyền NULL vào @StaffID.
        /// </summary>
        public static int ImportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", ingredientID),
                new SqlParameter("@Quantity",     quantity),
                new SqlParameter("@StaffID",      staffID > 0 ? (object)staffID : DBNull.Value),
                new SqlParameter("@Note",         string.IsNullOrWhiteSpace(note) ? "" : note)
            };

            return DataHelper.ExecuteSP("sp_Inventory_Import", parameters);
        }

        /// <summary>
        /// Xuất kho. Nếu staffID = 0 (Admin) → truyền NULL vào @StaffID.
        /// </summary>
        public static int ExportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", ingredientID),
                new SqlParameter("@Quantity",     quantity),
                new SqlParameter("@StaffID",      staffID > 0 ? (object)staffID : DBNull.Value),
                new SqlParameter("@Note",         string.IsNullOrWhiteSpace(note) ? "" : note)
            };

            return DataHelper.ExecuteSP("sp_Inventory_Export", parameters);
        }

        public static DataTable GetRecentTransactions()
        {
            return DataHelper.ExecuteSPGetTable("sp_InventoryTransaction_GetRecent", null);
        }

        public static DataTable CheckStockForOrder(int orderID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderID", orderID)
            };

            return DataHelper.ExecuteSPGetTable("sp_Inventory_CheckStockForOrder", parameters);
        }

        /// <summary>
        /// Xuất kho theo công thức đơn hàng. staffID = 0 (Admin) → NULL.
        /// </summary>
        public static int ExportByOrderRecipe(int orderID, int staffID, string note)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderID",  orderID),
                new SqlParameter("@StaffID",  staffID > 0 ? (object)staffID : DBNull.Value),
                new SqlParameter("@Note",     string.IsNullOrWhiteSpace(note)
                                              ? "Tự động trừ kho từ POS" : note)
            };

            return DataHelper.ExecuteSP("sp_Inventory_ExportByOrderRecipe", parameters);
        }
    }
}