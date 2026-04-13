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
    public class RecipeBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateRecipe(RecipeDTO recipe)
        {
            if (recipe.MenuItemID <= 0)
                throw new Exception("MenuItemID không hợp lệ!");

            if (recipe.IngredientID <= 0)
                throw new Exception("IngredientID không hợp lệ!");

            if (recipe.Quantity <= 0)
                throw new Exception("Số lượng phải lớn hơn 0!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddRecipe(RecipeDTO recipe)
        {
            try
            {
                ValidateRecipe(recipe);
                return RecipeDAL.Insert(recipe);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RecipeBLL.AddRecipe Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetRecipeByMenuItem(int menuItemID)
        {
            try
            {
                if (menuItemID <= 0)
                    throw new Exception("MenuItemID không hợp lệ!");

                return RecipeDAL.GetByMenuItem(menuItemID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RecipeBLL.GetRecipeByMenuItem Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteRecipe(int recipeID)
        {
            try
            {
                if (recipeID <= 0)
                    throw new Exception("RecipeID không hợp lệ!");

                return RecipeDAL.Delete(recipeID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RecipeBLL.DeleteRecipe Error: {ex.Message}");
                throw;
            }
        }
    }
}
