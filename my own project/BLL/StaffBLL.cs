using my_own_project.DAL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.BLL
{
    public class StaffBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateStaff(StaffDTO staff)
        {
            if (staff.UserID <= 0)
                throw new Exception("UserID không hợp lệ!");

            if (staff.RestaurantID <= 0)
                throw new Exception("RestaurantID không hợp lệ!");

            if (string.IsNullOrWhiteSpace(staff.Position))
                throw new Exception("Vị trí không được để trống!");

            if (staff.Salary < 0)
                throw new Exception("Lương không được âm!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddStaff(StaffDTO staff)
        {
            try
            {
                ValidateStaff(staff);
                return StaffDAL.Insert(staff);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.AddStaff Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllStaff()
        {
            try
            {
                return StaffDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.GetAllStaff Error: {ex.Message}");
                throw;
            }
        }

        public static StaffDTO GetStaffByID(int staffID)
        {
            try
            {
                if (staffID <= 0)
                    throw new Exception("StaffID không hợp lệ!");

                return StaffDAL.GetByID(staffID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.GetStaffByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetStaffByRestaurant(int restaurantID)
        {
            try
            {
                if (restaurantID <= 0)
                    throw new Exception("RestaurantID không hợp lệ!");

                return StaffDAL.GetByRestaurant(restaurantID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.GetStaffByRestaurant Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetStaffByPosition(string position)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(position))
                    throw new Exception("Vị trí không được để trống!");

                return StaffDAL.GetByPosition(position);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.GetStaffByPosition Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateStaff(StaffDTO staff)
        {
            try
            {
                ValidateStaff(staff);

                if (staff.StaffID <= 0)
                    throw new Exception("StaffID không hợp lệ!");

                return StaffDAL.Update(staff);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.UpdateStaff Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteStaff(int staffID)
        {
            try
            {
                if (staffID <= 0)
                    throw new Exception("StaffID không hợp lệ!");

                return StaffDAL.Delete(staffID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StaffBLL.DeleteStaff Error: {ex.Message}");
                throw;
            }
        }
    }
}
