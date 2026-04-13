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
        // ==================== CREATE ====================
        public static int Insert(InventoryTransactionDTO transaction)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IngredientID", transaction.IngredientID),
                    new SqlParameter("@QuantityChanged", transaction.QuantityChanged),
                    new SqlParameter("@TransactionType", transaction.TransactionType ?? "Import"),
                    new SqlParameter("@StaffID", transaction.StaffID),
                    new SqlParameter("@Note", transaction.Note ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_InventoryTransaction_Insert", parameters);
                return (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_InventoryTransaction_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByIngredient(int ingredientID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IngredientID", ingredientID)
                };

                return DataHelper.ExecuteSPGetTable("sp_InventoryTransaction_GetByIngredient", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionDAL.GetByIngredient Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate)
                };

                return DataHelper.ExecuteSPGetTable("sp_InventoryTransaction_GetByDateRange", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventoryTransactionDAL.GetByDateRange Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static InventoryTransactionDTO MapDTO(DataRow row)
        {
            return new InventoryTransactionDTO
            {
                TransactionID = (int)row["TransactionID"],
                IngredientID = (int)row["IngredientID"],
                QuantityChanged = row["QuantityChanged"] != DBNull.Value ? (float)row["QuantityChanged"] : 0,
                TransactionType = row["TransactionType"]?.ToString() ?? "",
                TransactionDate = (DateTime)row["TransactionDate"],
                StaffID = (int)row["StaffID"],
                Note = row["Note"]?.ToString() ?? "",
                IngredientName = row["IngredientName"]?.ToString() ?? "",
                StaffName = row["StaffName"]?.ToString() ?? "",
                Unit = row["Unit"]?.ToString() ?? ""
            };
        }
    }
}
