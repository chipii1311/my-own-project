using my_own_project.DAL;
using my_own_project.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.BLL
{
    public class IngredientBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateIngredient(IngredientDTO ingredient)
        {
            if (string.IsNullOrWhiteSpace(ingredient.IngredientName))
                throw new Exception("Tên nguyên liệu không được để trống!");

            if (string.IsNullOrWhiteSpace(ingredient.Unit))
                throw new Exception("Đơn vị không được để trống!");

            if (ingredient.StockQuantity < 0)
                throw new Exception("Số lượng tồn kho không được âm!");

            if (ingredient.MinStock < 0)
                throw new Exception("Tồn kho tối thiểu không được âm!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddIngredient(IngredientDTO ingredient)
        {
            try
            {
                ValidateIngredient(ingredient);
                return IngredientDAL.Insert(ingredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.AddIngredient Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllIngredients()
        {
            try
            {
                return IngredientDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.GetAllIngredients Error: {ex.Message}");
                throw;
            }
        }

        public static IngredientDTO GetIngredientByID(int ingredientID)
        {
            try
            {
                if (ingredientID <= 0)
                    throw new Exception("IngredientID không hợp lệ!");

                return IngredientDAL.GetByID(ingredientID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.GetIngredientByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetLowStockIngredients()
        {
            try
            {
                return IngredientDAL.GetLowStock();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.GetLowStockIngredients Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateIngredient(IngredientDTO ingredient)
        {
            try
            {
                ValidateIngredient(ingredient);

                if (ingredient.IngredientID <= 0)
                    throw new Exception("IngredientID không hợp lệ!");

                return IngredientDAL.Update(ingredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.UpdateIngredient Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteIngredient(int ingredientID)
        {
            try
            {
                if (ingredientID <= 0)
                    throw new Exception("IngredientID không hợp lệ!");

                return IngredientDAL.Delete(ingredientID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.DeleteIngredient Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Kiểm tra nguyên liệu sắp hết
        /// </summary>
        public static bool IsLowStock(int ingredientID)
        {
            try
            {
                IngredientDTO ingredient = IngredientDAL.GetByID(ingredientID);
                if (ingredient == null)
                    return false;

                return ingredient.StockQuantity < ingredient.MinStock;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.IsLowStock Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra nguyên liệu hết hàng
        /// </summary>
        public static bool IsOutOfStock(int ingredientID)
        {
            try
            {
                IngredientDTO ingredient = IngredientDAL.GetByID(ingredientID);
                if (ingredient == null)
                    return true;

                return ingredient.StockQuantity <= 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IngredientBLL.IsOutOfStock Error: {ex.Message}");
                return true;
            }
        }
    }
}
