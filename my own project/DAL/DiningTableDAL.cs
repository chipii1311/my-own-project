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
    public class DiningTableDAL
    {
        // ==================== CREATE ====================
        public static int Insert(DiningTableDTO table)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {

                    new SqlParameter("@TableNumber", table.TableNumber),
                    new SqlParameter("@Capacity", table.Capacity),
                    new SqlParameter("@Status", table.Status ?? "Available"),
                    new SqlParameter("@Notes", table.Notes ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_DiningTable_Insert", parameters);
                return (int)parameters[4].Value;
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



        public static DataTable GetAvailableTables()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {

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

        /// <summary>
        /// Cập nhật nhanh trạng thái bàn (Trống / Có khách / Đặt trước)
        /// </summary>
        public static bool UpdateStatus(int tableID, string status)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TableID", tableID),
                    new SqlParameter("@Status",  status ?? "Trống")
                };

                DataHelper.ExecuteSP("sp_DiningTable_UpdateStatus", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableDAL.UpdateStatus Error: {ex.Message}");
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

                TableNumber = (int)row["TableNumber"],
                Capacity = (int)row["Capacity"],
                Status = row["Status"]?.ToString() ?? "Available",
                Notes = row["Notes"]?.ToString() ?? "",

            };
        }
    }
}