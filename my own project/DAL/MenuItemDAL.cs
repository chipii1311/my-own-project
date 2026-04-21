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
    public class MenuItemDAL
    {
        // ==================== CREATE ====================
        public static int Insert(MenuItemDTO item)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                   
                    new SqlParameter("@CategoryID", item.CategoryID),
                    new SqlParameter("@ItemName", item.ItemName ?? ""),
                    new SqlParameter("@Description", item.Description ?? ""),
                    new SqlParameter("@Price", item.Price),
                    new SqlParameter("@Status", item.Status ?? "Active"),
                    new SqlParameter("@ImageUrl", item.ImageUrl ?? ""),
                    new SqlParameter("@ItemStatus", item.ItemStatus),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_MenuItem_Insert", parameters);
                return (int)parameters[7].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static MenuItemDTO GetByID(int menuItemID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MenuItemID", menuItemID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByRestaurant(int restaurantID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantID", restaurantID)
                };

                return DataHelper.ExecuteSPGetTable("sp_MenuItem_GetByRestaurant", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.GetByRestaurant Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByCategory(int categoryID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoryID", categoryID)
                };

                return DataHelper.ExecuteSPGetTable("sp_MenuItem_GetByCategory", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.GetByCategory Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(MenuItemDTO item)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MenuItemID", item.MenuItemID),
                    new SqlParameter("@CategoryID", item.CategoryID),
                    new SqlParameter("@ItemName", item.ItemName ?? ""),
                    new SqlParameter("@Description", item.Description ?? ""),
                    new SqlParameter("@Price", item.Price),
                    new SqlParameter("@Status", item.Status ?? "Active"),
                    new SqlParameter("@ImageUrl", item.ImageUrl ?? ""),
                    new SqlParameter("@ItemStatus", item.ItemStatus)
                };

                DataHelper.ExecuteSP("sp_MenuItem_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int menuItemID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MenuItemID", menuItemID)
                };

                DataHelper.ExecuteSP("sp_MenuItem_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static MenuItemDTO MapDTO(DataRow row)
        {
            return new MenuItemDTO
            {
                MenuItemID = (int)row["MenuItemID"],
              
                CategoryID = (int)row["CategoryID"],
                ItemName = row["ItemName"]?.ToString() ?? "",
                Description = row["Description"]?.ToString() ?? "",
                Price = (decimal)row["Price"],
                Status = row["Status"]?.ToString() ?? "Active",
                ImageUrl = row["ImageUrl"]?.ToString() ?? "",
                ItemStatus = row["ItemStatus"] != DBNull.Value ? Convert.ToInt32(row["ItemStatus"]) : 1,
                CreatedAt = row["CreatedAt"] != DBNull.Value ? (DateTime)row["CreatedAt"] : DateTime.Now,
                UpdatedAt = row["UpdatedAt"] != DBNull.Value ? (DateTime?)row["UpdatedAt"] : null,
                CategoryName = row["CategoryName"]?.ToString() ?? "",
                
            };
        }

        public static DataTable GetAllAvailableItems()
        {
            try
            {
                // Sử dụng DataHelper chuẩn của nhóm để gọi Stored Procedure
                return DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllAvailable");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemDAL.GetAllAvailableItems Error: {ex.Message}");
                throw;
            }
        }
    }
}
