using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using my_own_project.DTO;

namespace my_own_project.DAL
{
    public class CategoryDAL
    {
        // ==================== CREATE ====================
        public static int Insert(CategoryDTO category)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoryName", category.CategoryName ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Category_Insert", parameters);
                return (int)parameters[1].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Category_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static CategoryDTO GetByID(int categoryID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoryID", categoryID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Category_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(CategoryDTO category)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoryID", category.CategoryID),
                    new SqlParameter("@CategoryName", category.CategoryName ?? ""),
                    new SqlParameter("@IsActive", category.IsActive)
                };

                DataHelper.ExecuteSP("sp_Category_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int categoryID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoryID", categoryID)
                };

                DataHelper.ExecuteSP("sp_Category_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static CategoryDTO MapDTO(DataRow row)
        {
            return new CategoryDTO
            {
                CategoryID = (int)row["CategoryID"],
                CategoryName = row["CategoryName"]?.ToString() ?? "",
                IsActive = row["IsActive"] != DBNull.Value && (bool)row["IsActive"]
            };
        }
    }
}
