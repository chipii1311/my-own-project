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
    public class DiningTableBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateTable(DiningTableDTO table)
        {
            if (table.RestaurantID <= 0)
                throw new Exception("RestaurantID không hợp lệ!");

            if (table.TableNumber <= 0)
                throw new Exception("Số bàn phải lớn hơn 0!");

            if (table.Capacity <= 0)
                throw new Exception("Sức chứa phải lớn hơn 0!");

            if (string.IsNullOrWhiteSpace(table.Status))
                throw new Exception("Trạng thái không được để trống!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddTable(DiningTableDTO table)
        {
            try
            {
                ValidateTable(table);
                return DiningTableDAL.Insert(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.AddTable Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllTables()
        {
            try
            {
                return DiningTableDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.GetAllTables Error: {ex.Message}");
                throw;
            }
        }

        public static DiningTableDTO GetTableByID(int tableID)
        {
            try
            {
                if (tableID <= 0)
                    throw new Exception("TableID không hợp lệ!");

                return DiningTableDAL.GetByID(tableID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.GetTableByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetTablesByRestaurant(int restaurantID)
        {
            try
            {
                if (restaurantID <= 0)
                    throw new Exception("RestaurantID không hợp lệ!");

                return DiningTableDAL.GetByRestaurant(restaurantID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.GetTablesByRestaurant Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetAvailableTables(int restaurantID)
        {
            try
            {
                if (restaurantID <= 0)
                    throw new Exception("RestaurantID không hợp lệ!");

                return DiningTableDAL.GetAvailableTables(restaurantID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.GetAvailableTables Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateTable(DiningTableDTO table)
        {
            try
            {
                ValidateTable(table);

                if (table.TableID <= 0)
                    throw new Exception("TableID không hợp lệ!");

                return DiningTableDAL.Update(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.UpdateTable Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteTable(int tableID)
        {
            try
            {
                if (tableID <= 0)
                    throw new Exception("TableID không hợp lệ!");

                return DiningTableDAL.Delete(tableID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.DeleteTable Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Lấy số bàn trống
        /// </summary>
        public static int GetAvailableTableCount(int restaurantID)
        {
            try
            {
                DataTable dt = DiningTableDAL.GetAvailableTables(restaurantID);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.GetAvailableTableCount Error: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Tìm bàn trống với sức chứa yêu cầu
        /// </summary>
        public static DataTable FindTableByCapacity(int restaurantID, int capacity)
        {
            try
            {
                DataTable availableTables = DiningTableDAL.GetAvailableTables(restaurantID);
                DataTable result = availableTables.Clone();

                foreach (DataRow row in availableTables.Rows)
                {
                    if ((int)row["Capacity"] >= capacity)
                        result.ImportRow(row);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.FindTableByCapacity Error: {ex.Message}");
                throw;
            }
        }
    }
}
