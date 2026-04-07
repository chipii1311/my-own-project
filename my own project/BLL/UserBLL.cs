using my_own_project.DAL;
using my_own_project.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.BLL
{
    
        public class UserBLL
        {
            // ==================== VALIDATE ====================
            /// <summary>
            /// Kiểm tra thông tin User hợp lệ
            /// </summary>
            private static bool ValidateUser(UserDTO user)
            {
                if (string.IsNullOrWhiteSpace(user.FullName))
                    throw new Exception("Tên đầy đủ không được để trống!");

                if (string.IsNullOrWhiteSpace(user.Email))
                    throw new Exception("Email không được để trống!");

                if (!IsValidEmail(user.Email))
                    throw new Exception("Email không hợp lệ!");

                if (string.IsNullOrWhiteSpace(user.PasswordHash))
                    throw new Exception("Mật khẩu không được để trống!");

                if (user.PasswordHash.Length < 6)
                    throw new Exception("Mật khẩu phải tối thiểu 6 ký tự!");

                return true;
            }

            /// <summary>
            /// Kiểm tra email hợp lệ
            /// </summary>
            private static bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }

            // ==================== CREATE ====================
            /// <summary>
            /// Thêm User mới (với validate + hash password)
            /// </summary>
            public static int AddUser(UserDTO user)
            {
                try
                {
                    ValidateUser(user);

                    // Kiểm tra email đã tồn tại
                    if (UserDAL.GetByEmail(user.Email) != null)
                        throw new Exception("Email đã được sử dụng!");

                    // Hash password
                    user.PasswordHash = HashPassword(user.PasswordHash);

                    return UserDAL.Insert(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.AddUser Error: {ex.Message}");
                    throw;
                }
            }

            // ==================== READ ====================
            /// <summary>
            /// Lấy User theo ID
            /// </summary>
            public static UserDTO GetUserByID(int userID)
            {
                try
                {
                    if (userID <= 0)
                        throw new Exception("UserID không hợp lệ!");

                    return UserDAL.GetByID(userID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.GetUserByID Error: {ex.Message}");
                    throw;
                }
            }

            /// <summary>
            /// Lấy tất cả Users
            /// </summary>
            public static DataTable GetAllUsers()
            {
                try
                {
                    return UserDAL.GetAll();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.GetAllUsers Error: {ex.Message}");
                    throw;
                }
            }

            // ==================== UPDATE ====================
            /// <summary>
            /// Cập nhật thông tin User
            /// </summary>
            public static bool UpdateUser(UserDTO user)
            {
                try
                {
                    ValidateUser(user);

                    if (user.UserID <= 0)
                        throw new Exception("UserID không hợp lệ!");

                    return UserDAL.Update(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.UpdateUser Error: {ex.Message}");
                    throw;
                }
            }

            /// <summary>
            /// Cập nhật mật khẩu User
            /// </summary>
            public static bool ChangePassword(int userID, string oldPassword, string newPassword)
            {
                try
                {
                    if (userID <= 0)
                        throw new Exception("UserID không hợp lệ!");

                    if (string.IsNullOrWhiteSpace(oldPassword))
                        throw new Exception("Mật khẩu cũ không được để trống!");

                    if (string.IsNullOrWhiteSpace(newPassword))
                        throw new Exception("Mật khẩu mới không được để trống!");

                    if (newPassword.Length < 6)
                        throw new Exception("Mật khẩu mới phải tối thiểu 6 ký tự!");

                    // Lấy user hiện tại
                    UserDTO user = UserDAL.GetByID(userID);
                    if (user == null)
                        throw new Exception("Người dùng không tồn tại!");

                    // Kiểm tra mật khẩu cũ
                    string hashedOldPassword = HashPassword(oldPassword);
                    if (user.PasswordHash != hashedOldPassword)
                        throw new Exception("Mật khẩu cũ không chính xác!");

                    // Hash mật khẩu mới
                    string hashedNewPassword = HashPassword(newPassword);

                    return UserDAL.Update(new UserDTO
                    {
                        UserID = userID,
                        FullName = user.FullName,
                        Email = user.Email,
                        Phone = user.Phone,
                        PasswordHash = hashedNewPassword,
                        Role = user.Role,
                        IsActive = user.IsActive
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.ChangePassword Error: {ex.Message}");
                    throw;
                }
            }

            // ==================== DELETE ====================
            /// <summary>
            /// Xóa User (soft delete - đặt IsActive = false)
            /// </summary>
            public static bool DeleteUser(int userID)
            {
                try
                {
                    if (userID <= 0)
                        throw new Exception("UserID không hợp lệ!");

                    UserDTO user = UserDAL.GetByID(userID);
                    if (user == null)
                        throw new Exception("Người dùng không tồn tại!");

                    // Soft delete
                    user.IsActive = false;
                    return UserDAL.Update(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.DeleteUser Error: {ex.Message}");
                    throw;
                }
            }

            // ==================== AUTHENTICATION ====================
            /// <summary>
            /// Đăng nhập (Login)
            /// </summary>
            public static UserDTO Login(string email, string password)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(email))
                        throw new Exception("Email không được để trống!");

                    if (string.IsNullOrWhiteSpace(password))
                        throw new Exception("Mật khẩu không được để trống!");

                    // Hash password
                    string hashedPassword = HashPassword(password);

                    // Kiểm tra thông tin đăng nhập
                    UserDTO user = UserDAL.Login(email, hashedPassword);

                    if (user == null)
                        throw new Exception("Email hoặc mật khẩu không chính xác!");

                    if (!user.IsActive)
                        throw new Exception("Tài khoản đã bị khóa!");

                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.Login Error: {ex.Message}");
                    throw;
                }
            }

            // ==================== HELPER METHODS ====================
            /// <summary>
            /// Hash password using SHA256
            /// </summary>
            private static string HashPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashedBytes);
                }
            }

            /// <summary>
            /// Kiểm tra quyền hạn của User
            /// </summary>
            public static bool HasRole(int userID, string role)
            {
                try
                {
                    UserDTO user = UserDAL.GetByID(userID);
                    if (user == null)
                        return false;

                    return user.Role == role;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.HasRole Error: {ex.Message}");
                    return false;
                }
            }

            /// <summary>
            /// Lấy danh sách User theo Role
            /// </summary>
            public static DataTable GetUsersByRole(string role)
            {
                try
                {
                    DataTable allUsers = UserDAL.GetAll();
                    DataTable result = allUsers.Clone();

                    foreach (DataRow row in allUsers.Rows)
                    {
                        if (row["Role"].ToString() == role)
                            result.ImportRow(row);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UserBLL.GetUsersByRole Error: {ex.Message}");
                    throw;
                }
            }
        }
    }

