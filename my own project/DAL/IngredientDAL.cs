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
    public class IngredientDAL
    {
        // ==================== CREATE ====================
        public static int Insert(IngredientDTO ingredient)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IngredientName", ingredient.IngredientName ?? ""),
                    new SqlParameter("@Unit", ingredient.Unit ?? ""),
                    new SqlParameter("@StockQuantity", ingredient.StockQuantity),
                    new SqlParameter("@MinStock", ingredient.MinStock),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Ingredient_Insert", parameters);
                return (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Ingredient_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static IngredientDTO GetByID(int ingredientID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IngredientID", ingredientID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Ingredient_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetLowStock()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Ingredient_GetLowStock");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientDAL.GetLowStock Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(IngredientDTO ingredient)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IngredientID", ingredient.IngredientID),
                    new SqlParameter("@IngredientName", ingredient.IngredientName ?? ""),
                    new SqlParameter("@Unit", ingredient.Unit ?? ""),
                    new SqlParameter("@MinStock", ingredient.MinStock),
                    new SqlParameter("@IsActive", ingredient.IsActive)
                };

                DataHelper.ExecuteSP("sp_Ingredient_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int ingredientID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@IngredientID", ingredientID)
                };

                DataHelper.ExecuteSP("sp_Ingredient_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static IngredientDTO MapDTO(DataRow row)
        {
            return new IngredientDTO
            {
                IngredientID = (int)row["IngredientID"],
                IngredientName = row["IngredientName"]?.ToString() ?? "",
                Unit = row["Unit"]?.ToString() ?? "",
                StockQuantity = row["StockQuantity"] != DBNull.Value ? (float)row["StockQuantity"] : 0,
                MinStock = row["MinStock"] != DBNull.Value ? (float)row["MinStock"] : 0,
                IsActive = row["IsActive"] != DBNull.Value && (bool)row["IsActive"],
                PurchasePrice = row["PurchasePrice"] != DBNull.Value ? (decimal)row["PurchasePrice"] : 0
            };
        }
    }
}
