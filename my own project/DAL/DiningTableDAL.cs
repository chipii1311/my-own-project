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
    public class DiningTableDAL
    {
        // ==================== CREATE ====================
        public static int Insert(DiningTableDTO table)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantID", table.RestaurantID),
                    new SqlParameter("@TableNumber", table.TableNumber),
                    new SqlParameter("@Capacity", table.Capacity),
                    new SqlParameter("@Status", table.Status ?? "Available"),
                    new SqlParameter("@Notes", table.Notes ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_DiningTable_Insert", parameters);
                return (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_DiningTable_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static DiningTableDTO GetByID(int tableID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TableID", tableID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_DiningTable_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.GetByID Error: {ex.Message}");
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

                return DataHelper.ExecuteSPGetTable("sp_DiningTable_GetByRestaurant", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.GetByRestaurant Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetAvailableTables(int restaurantID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RestaurantID", restaurantID),
                    new SqlParameter("@Status", "Available")
                };

                return DataHelper.ExecuteSPGetTable("sp_DiningTable_GetByStatus", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.GetAvailableTables Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(DiningTableDTO table)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TableID", table.TableID),
                    new SqlParameter("@TableNumber", table.TableNumber),
                    new SqlParameter("@Capacity", table.Capacity),
                    new SqlParameter("@Status", table.Status ?? "Available"),
                    new SqlParameter("@Notes", table.Notes ?? "")
                };

                DataHelper.ExecuteSP("sp_DiningTable_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int tableID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TableID", tableID)
                };

                DataHelper.ExecuteSP("sp_DiningTable_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static DiningTableDTO MapDTO(DataRow row)
        {
            return new DiningTableDTO
            {
                TableID = (int)row["TableID"],
                RestaurantID = (int)row["RestaurantID"],
                TableNumber = (int)row["TableNumber"],
                Capacity = (int)row["Capacity"],
                Status = row["Status"]?.ToString() ?? "Available",
                Notes = row["Notes"]?.ToString() ?? "",
                RestaurantName = row["RestaurantName"]?.ToString() ?? ""
            };
        }
    }
}
