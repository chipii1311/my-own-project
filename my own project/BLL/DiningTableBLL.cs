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
            // Đã xóa check RestaurantID

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

        // ĐÃ XÓA HÀM GetTablesByRestaurant VÌ KHÔNG CÒN TÁC DỤNG

        // Đã gỡ tham số int restaurantID
        public static DataTable GetAvailableTables()
        {
            try
            {
                return DiningTableDAL.GetAvailableTables();
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

        /// <summary>
        /// Cập nhật nhanh trạng thái bàn (Trống / Có khách / Đặt trước)
        /// </summary>
        public static bool UpdateStatus(int tableID, string status)
        {
            try
            {
                if (tableID <= 0)
                    throw new Exception("TableID không hợp lệ!");

                if (string.IsNullOrWhiteSpace(status))
                    throw new Exception("Trạng thái không được để trống!");

                return DiningTableDAL.UpdateStatus(tableID, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiningTableBLL.UpdateStatus Error: {ex.Message}");
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
        public static int GetAvailableTableCount() // Đã gỡ tham số int restaurantID
        {
            try
            {
                DataTable dt = DiningTableDAL.GetAvailableTables();
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
        public static DataTable FindTableByCapacity(int capacity) // Đã gỡ tham số int restaurantID
        {
            try
            {
                DataTable availableTables = DiningTableDAL.GetAvailableTables();
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