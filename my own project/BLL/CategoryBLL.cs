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
    public class CategoryBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateCategory(CategoryDTO category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new Exception("Tên danh mục không được để trống!");

            if (category.CategoryName.Length > 100)
                throw new Exception("Tên danh mục không quá 100 ký tự!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddCategory(CategoryDTO category)
        {
            try
            {
                ValidateCategory(category);
                return CategoryDAL.Insert(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryBLL.AddCategory Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllCategories()
        {
            try
            {
                return CategoryDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryBLL.GetAllCategories Error: {ex.Message}");
                throw;
            }
        }

        public static CategoryDTO GetCategoryByID(int categoryID)
        {
            try
            {
                if (categoryID <= 0)
                    throw new Exception("CategoryID không hợp lệ!");

                return CategoryDAL.GetByID(categoryID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryBLL.GetCategoryByID Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateCategory(CategoryDTO category)
        {
            try
            {
                ValidateCategory(category);

                if (category.CategoryID <= 0)
                    throw new Exception("CategoryID không hợp lệ!");

                return CategoryDAL.Update(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryBLL.UpdateCategory Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteCategory(int categoryID)
        {
            try
            {
                if (categoryID <= 0)
                    throw new Exception("CategoryID không hợp lệ!");

                return CategoryDAL.Delete(categoryID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryBLL.DeleteCategory Error: {ex.Message}");
                throw;
            }
        }
    }
}
