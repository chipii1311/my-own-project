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
    public class RestaurantBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateRestaurant(RestaurantDTO restaurant)
        {
            if (string.IsNullOrWhiteSpace(restaurant.RestaurantName))
                throw new Exception("Tên nhà hàng không được để trống!");

            if (restaurant.RestaurantName.Length > 100)
                throw new Exception("Tên nhà hàng không quá 100 ký tự!");

            if (string.IsNullOrWhiteSpace(restaurant.Address))
                throw new Exception("Địa chỉ không được để trống!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddRestaurant(RestaurantDTO restaurant)
        {
            try
            {
                ValidateRestaurant(restaurant);
                return RestaurantDAL.Insert(restaurant);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantBLL.AddRestaurant Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllRestaurants()
        {
            try
            {
                return RestaurantDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantBLL.GetAllRestaurants Error: {ex.Message}");
                throw;
            }
        }

        public static RestaurantDTO GetRestaurantByID(int restaurantID)
        {
            try
            {
                if (restaurantID <= 0)
                    throw new Exception("RestaurantID không hợp lệ!");

                return RestaurantDAL.GetByID(restaurantID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantBLL.GetRestaurantByID Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateRestaurant(RestaurantDTO restaurant)
        {
            try
            {
                ValidateRestaurant(restaurant);

                if (restaurant.RestaurantID <= 0)
                    throw new Exception("RestaurantID không hợp lệ!");

                return RestaurantDAL.Update(restaurant);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantBLL.UpdateRestaurant Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteRestaurant(int restaurantID)
        {
            try
            {
                if (restaurantID <= 0)
                    throw new Exception("RestaurantID không hợp lệ!");

                return RestaurantDAL.Delete(restaurantID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RestaurantBLL.DeleteRestaurant Error: {ex.Message}");
                throw;
            }
        }
    }
}
