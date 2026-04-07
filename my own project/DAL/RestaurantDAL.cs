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
    public class RestaurantDAL
    {
        // ==================== CREATE ====================
        public static int Insert(RestaurantDTO restaurant)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantName", restaurant.RestaurantName ?? ""),
                    new SqlParameter("@Address", restaurant.Address ?? ""),
                    new SqlParameter("@Phone", restaurant.Phone ?? ""),
                    new SqlParameter("@Email", restaurant.Email ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Restaurant_Insert", parameters);
                return (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Restaurant_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static RestaurantDTO GetByID(int restaurantID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantID", restaurantID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Restaurant_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(RestaurantDTO restaurant)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantID", restaurant.RestaurantID),
                    new SqlParameter("@RestaurantName", restaurant.RestaurantName ?? ""),
                    new SqlParameter("@Address", restaurant.Address ?? ""),
                    new SqlParameter("@Phone", restaurant.Phone ?? ""),
                    new SqlParameter("@Email", restaurant.Email ?? "")
                };

                DataHelper.ExecuteSP("sp_Restaurant_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int restaurantID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantID", restaurantID)
                };

                DataHelper.ExecuteSP("sp_Restaurant_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static RestaurantDTO MapDTO(DataRow row)
        {
            return new RestaurantDTO
            {
                RestaurantID = (int)row["RestaurantID"],
                RestaurantName = row["RestaurantName"]?.ToString() ?? "",
                Address = row["Address"]?.ToString() ?? "",
                Phone = row["Phone"]?.ToString() ?? "",
                Email = row["Email"]?.ToString() ?? ""
            };
        }
    }
}
