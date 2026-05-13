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
    public class InventoryTransactionDAL
    {
        public static int ImportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", ingredientID),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@StaffID", staffID),
                new SqlParameter("@Note", string.IsNullOrWhiteSpace(note) ? "" : note)
            };

            return DataHelper.ExecuteSP("sp_Inventory_Import", parameters);
        }

        public static int ExportIngredient(int ingredientID, float quantity, int staffID, string note)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IngredientID", ingredientID),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@StaffID", staffID),
                new SqlParameter("@Note", string.IsNullOrWhiteSpace(note) ? "" : note)
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

        public static int ExportByOrderRecipe(int orderID, int staffID, string note)
        {
            SqlParameter[] parameters =
            {
        new SqlParameter("@OrderID", orderID),
        new SqlParameter("@StaffID", staffID),
        new SqlParameter("@Note", string.IsNullOrWhiteSpace(note) ? "Tự động trừ kho từ POS" : note)
    };

            return DataHelper.ExecuteSP("sp_Inventory_ExportByOrderRecipe", parameters);
        }
    }
}
