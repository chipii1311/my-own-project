using my_own_project.DAL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.BLL
{
    public class MenuItemBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateMenuItem(MenuItemDTO item)
        {
            if (string.IsNullOrWhiteSpace(item.ItemName))
                throw new Exception("Tên món ăn không được để trống!");

            if (item.ItemName.Length > 100)
                throw new Exception("Tên món ăn không quá 100 ký tự!");

            if (item.Price < 0)
                throw new Exception("Giá không được âm!");

            // ĐÃ XÓA CHECK RESTAURANT ID Ở ĐÂY

            if (item.CategoryID <= 0)
                throw new Exception("CategoryID không hợp lệ!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddMenuItem(MenuItemDTO item)
        {
            try
            {
                ValidateMenuItem(item);
                return MenuItemDAL.Insert(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.AddMenuItem Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllMenuItems()
        {
            try
            {
                return MenuItemDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.GetAllMenuItems Error: {ex.Message}");
                throw;
            }
        }

        public static MenuItemDTO GetMenuItemByID(int menuItemID)
        {
            try
            {
                if (menuItemID <= 0)
                    throw new Exception("MenuItemID không hợp lệ!");

                return MenuItemDAL.GetByID(menuItemID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.GetMenuItemByID Error: {ex.Message}");
                throw;
            }
        }

        // ĐÃ XÓA HÀM GetMenuItemsByRestaurant Ở ĐÂY

        public static DataTable GetMenuItemsByCategory(int categoryID)
        {
            try
            {
                if (categoryID <= 0)
                    throw new Exception("CategoryID không hợp lệ!");

                return MenuItemDAL.GetByCategory(categoryID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.GetMenuItemsByCategory Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdateMenuItem(MenuItemDTO item)
        {
            try
            {
                ValidateMenuItem(item);

                if (item.MenuItemID <= 0)
                    throw new Exception("MenuItemID không hợp lệ!");

                return MenuItemDAL.Update(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.UpdateMenuItem Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeleteMenuItem(int menuItemID)
        {
            try
            {
                if (menuItemID <= 0)
                    throw new Exception("MenuItemID không hợp lệ!");

                return MenuItemDAL.Delete(menuItemID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.DeleteMenuItem Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Tìm kiếm menu items
        /// </summary>
        public static DataTable SearchMenuItems(string keyword)
        {
            try
            {
                DataTable allItems = MenuItemDAL.GetAll();
                DataTable result = allItems.Clone();

                foreach (DataRow row in allItems.Rows)
                {
                    if (row["ItemName"].ToString().Contains(keyword) ||
                        row["Description"].ToString().Contains(keyword))
                    {
                        result.ImportRow(row);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MenuItemBLL.SearchMenuItems Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy menu items available (còn hàng)
        /// </summary>
        public static DataTable GetAllAvailableItems()
        {
            // Dùng hàm này chuẩn hơn vì nó gọi trực tiếp SP sp_MenuItem_GetAllAvailable
            return MenuItemDAL.GetAllAvailableItems();
        }

        // ĐÃ XÓA hàm GetAvailableMenuItems (bị lỗi logic cũ)


        public static bool UpdateProductWithRole(string role, int id, string priceText, int catID, string name, string status, string newImageUrl)
        {
            MenuItemDTO currentItem = GetMenuItemByID(id);
            if (currentItem == null) throw new Exception("Không tìm thấy món!");

            // Bỏ chữ "User" ra khỏi danh sách bị cấm sửa giá
            if (role == "Nhân viên")
            {
                currentItem.Status = status;
            }
            else
            {
                if (!decimal.TryParse(priceText, out decimal validPrice) || validPrice < 0)
                    throw new Exception("Giá tiền không hợp lệ!");

                currentItem.CategoryID = catID;
                currentItem.ItemName = name;
                currentItem.Price = validPrice;
                currentItem.Status = status;
                if (!string.IsNullOrEmpty(newImageUrl)) currentItem.ImageUrl = newImageUrl;
            }

            return MenuItemDAL.Update(currentItem);
        }
    }



}




