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
    public class RecipeDAL
    {
        // ==================== CREATE ====================
        public static int Insert(RecipeDTO recipe)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MenuItemID", recipe.MenuItemID),
                    new SqlParameter("@IngredientID", recipe.IngredientID),
                    new SqlParameter("@Quantity", recipe.Quantity),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Recipe_Insert", parameters);
                return (int)parameters[3].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RecipeDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetByMenuItem(int menuItemID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MenuItemID", menuItemID)
                };

                return DataHelper.ExecuteSPGetTable("sp_Recipe_GetByMenuItem", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RecipeDAL.GetByMenuItem Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int recipeID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RecipeID", recipeID)
                };

                DataHelper.ExecuteSP("sp_Recipe_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RecipeDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static RecipeDTO MapDTO(DataRow row)
        {
            return new RecipeDTO
            {
                RecipeID = (int)row["RecipeID"],
                MenuItemID = (int)row["MenuItemID"],
                IngredientID = (int)row["IngredientID"],
                Quantity = row["Quantity"] != DBNull.Value ? (float)row["Quantity"] : 0,
                ItemName = row["ItemName"]?.ToString() ?? "",
                IngredientName = row["IngredientName"]?.ToString() ?? "",
                Unit = row["Unit"]?.ToString() ?? ""
            };
        }
    }
}
