using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.Helpers
{
    /// <summary>
    /// Class lưu thông tin user đang đăng nhập (Static Session)
    /// Dùng để truy cập user info từ bất kỳ form nào
    /// </summary>
    public static class CurrentUser
    {
        /// <summary>
        /// Thông tin user hiện tại
        /// </summary>
        public static UserDTO User { get; set; }

        /// <summary>
        /// ID của user hiện tại
        /// </summary>
        public static int UserID => User?.UserID ?? 0;

        /// <summary>
        /// Tên đầy đủ của user
        /// </summary>
        public static string FullName => User?.FullName ?? "Guest";

        /// <summary>
        /// Email của user
        /// </summary>
        public static string Email => User?.Email ?? string.Empty;

        /// <summary>
        /// Vai trò của user (Admin, Manager, Chef, Waiter, Cashier)
        /// </summary>
        public static string Role => User?.Role ?? "User";

        /// <summary>
        /// Kiểm tra user đã đăng nhập chưa
        /// </summary>
        public static bool IsLoggedIn => User != null && User.UserID > 0;

        /// <summary>
        /// Kiểm tra user có phải Admin không
        /// </summary>
        public static bool IsAdmin => Role == "Admin";

        /// <summary>
        /// Kiểm tra user có phải Manager không
        /// </summary>
        public static bool IsManager => Role == "Manager";

        /// <summary>
        /// Kiểm tra user có phải Chef không
        /// </summary>
        public static bool IsChef => Role == "Chef";

        /// <summary>
        /// Kiểm tra user có phải Waiter không
        /// </summary>
        public static bool IsWaiter => Role == "Waiter";

        /// <summary>
        /// Kiểm tra user có phải Cashier không
        /// </summary>
        public static bool IsCashier => Role == "Cashier";

        /// <summary>
        /// Đăng nhập user
        /// </summary>
        public static void Login(UserDTO user)
        {
            if (user != null)
            {
                User = user;
                Console.WriteLine($"User {user.FullName} logged in successfully.");
            }
        }

        /// <summary>
        /// Đăng xuất user
        /// </summary>
        public static void Logout()
        {
            User = null;
            Console.WriteLine("User logged out.");
        }

        /// <summary>
        /// Lấy thông tin user dưới dạng text
        /// </summary>
        public static string GetUserInfo()
        {
            if (!IsLoggedIn)
                return "No user logged in";

            return $"{FullName} ({Role}) - {Email}";
        }

        /// <summary>
        /// Kiểm tra quyền hạn (dùng cho authorization)
        /// </summary>
        public static bool HasPermission(string requiredRole)
        {
            if (!IsLoggedIn)
                return false;

            // Admin có mọi quyền
            if (IsAdmin)
                return true;

            // Kiểm tra role của user
            return Role == requiredRole;
        }

        /// <summary>
        /// Kiểm tra user có quyền xem/sửa dữ liệu này không
        /// </summary>
        public static bool CanAccessRestaurant(int restaurantID)
        {
            // Admin có quyền truy cập tất cả
            if (IsAdmin)
                return true;

            // Manager chỉ xem nhà hàng của họ
            if (IsManager)
                return CurrentRestaurant == restaurantID;

            return false;
        }

        /// <summary>
        /// ID nhà hàng hiện tại (đối với Manager)
        /// </summary>
        public static int CurrentRestaurant { get; set; }

        /// <summary>
        /// Tên nhà hàng hiện tại
        /// </summary>
        public static string CurrentRestaurantName { get; set; }
    }
}
