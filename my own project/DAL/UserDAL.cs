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
    public class UserDAL
    {
        // ==================== CREATE ====================
        public static int Insert(UserDTO user)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FullName", user.FullName ?? ""),
                    new SqlParameter("@Email", user.Email ?? ""),
                    new SqlParameter("@Phone", user.Phone ?? ""),
                    new SqlParameter("@PasswordHash", user.PasswordHash ?? ""),
                    new SqlParameter("@Role", user.Role ?? "User"),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Users_Insert", parameters);
                return (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static UserDTO GetByID(int userID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", userID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Users_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        public static UserDTO GetByEmail(string email)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Email", email)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Users_GetByEmail", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.GetByEmail Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Users_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool Update(UserDTO user)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", user.UserID),
                    new SqlParameter("@FullName", user.FullName ?? ""),
                    new SqlParameter("@Email", user.Email ?? ""),
                    new SqlParameter("@Phone", user.Phone ?? ""),
                    new SqlParameter("@Role", user.Role ?? "User"),
                    new SqlParameter("@IsActive", user.IsActive)
                };

                DataHelper.ExecuteSP("sp_Users_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int userID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", userID)
                };

                DataHelper.ExecuteSP("sp_Users_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== AUTHENTICATION ====================
        public static UserDTO Login(string email, string passwordHash)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Email", email),
                    new SqlParameter("@PasswordHash", passwordHash)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Users_Login", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserDAL.Login Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER METHODS ====================
        private static UserDTO MapDTO(DataRow row)
        {
            return new UserDTO
            {
                UserID = (int)row["UserID"],
                FullName = row["FullName"]?.ToString() ?? "",
                Email = row["Email"]?.ToString() ?? "",
                Phone = row["Phone"]?.ToString() ?? "",
                PasswordHash = row["PasswordHash"]?.ToString() ?? "",
                Role = row["Role"]?.ToString() ?? "",
                CreatedAt = row["CreatedAt"] != DBNull.Value ? (DateTime)row["CreatedAt"] : DateTime.Now,
                IsActive = row["IsActive"] != DBNull.Value && (bool)row["IsActive"],
                LastLogin = row["LastLogin"] != DBNull.Value ? (DateTime?)row["LastLogin"] : null
            };
        }
    }
}
