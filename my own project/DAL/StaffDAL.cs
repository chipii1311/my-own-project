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
    public class StaffDAL
    {
        // ==================== CREATE ====================
        public static int Insert(StaffDTO staff)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", staff.UserID),
                   
                    new SqlParameter("@Position", staff.Position ?? ""),
                    new SqlParameter("@Salary", staff.Salary),
                    new SqlParameter("@HireDate", staff.HireDate),
                    new SqlParameter("@Status", staff.Status ?? "Active"),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Staff_Insert", parameters);
                return (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Staff_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static StaffDTO GetByID(int staffID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StaffID", staffID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Staff_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

       

        public static DataTable GetByPosition(string position)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Position", position)
                };

                return DataHelper.ExecuteSPGetTable("sp_Staff_GetByPosition", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffDAL.GetByPosition Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(StaffDTO staff)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StaffID", staff.StaffID),
                    new SqlParameter("@Position", staff.Position ?? ""),
                    new SqlParameter("@Salary", staff.Salary),
                    new SqlParameter("@Status", staff.Status ?? "Active")
                };

                DataHelper.ExecuteSP("sp_Staff_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int staffID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StaffID", staffID)
                };

                DataHelper.ExecuteSP("sp_Staff_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static StaffDTO MapDTO(DataRow row)
        {
            return new StaffDTO
            {
                StaffID = (int)row["StaffID"],
                UserID = (int)row["UserID"],
              
                Position = row["Position"]?.ToString() ?? "",
                Salary = (decimal)row["Salary"],
                HireDate = (DateTime)row["HireDate"],
                Status = row["Status"]?.ToString() ?? "Active",
                FullName = row["FullName"]?.ToString() ?? "",
                Email = row["Email"]?.ToString() ?? "",
                Phone = row["Phone"]?.ToString() ?? "",
               
            };
        }
    }
}
