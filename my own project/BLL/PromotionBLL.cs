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
    public class PromotionBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidatePromotion(PromotionDTO promotion)
        {
            if (string.IsNullOrWhiteSpace(promotion.PromotionName))
                throw new Exception("Tên khuyến mãi không được để trống!");

            if (promotion.DiscountPercent < 0 || promotion.DiscountPercent > 100)
                throw new Exception("Giảm giá phải từ 0 đến 100%!");

            if (promotion.StartDate >= promotion.EndDate)
                throw new Exception("Ngày bắt đầu phải trước ngày kết thúc!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddPromotion(PromotionDTO promotion)
        {
            try
            {
                ValidatePromotion(promotion);
                return PromotionDAL.Insert(promotion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.AddPromotion Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllPromotions()
        {
            try
            {
                return PromotionDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.GetAllPromotions Error: {ex.Message}");
                throw;
            }
        }

        public static PromotionDTO GetPromotionByID(int promotionID)
        {
            try
            {
                if (promotionID <= 0)
                    throw new Exception("PromotionID không hợp lệ!");

                return PromotionDAL.GetByID(promotionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.GetPromotionByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetActivePromotions()
        {
            try
            {
                return PromotionDAL.GetActive();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.GetActivePromotions Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        public static bool UpdatePromotion(PromotionDTO promotion)
        {
            try
            {
                ValidatePromotion(promotion);

                if (promotion.PromotionID <= 0)
                    throw new Exception("PromotionID không hợp lệ!");

                return PromotionDAL.Update(promotion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.UpdatePromotion Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool DeletePromotion(int promotionID)
        {
            try
            {
                if (promotionID <= 0)
                    throw new Exception("PromotionID không hợp lệ!");

                return PromotionDAL.Delete(promotionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.DeletePromotion Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Kiểm tra khuyến mãi còn hiệu lực
        /// </summary>
        public static bool IsPromotionActive(int promotionID)
        {
            try
            {
                PromotionDTO promotion = PromotionDAL.GetByID(promotionID);
                if (promotion == null)
                    return false;

                return promotion.IsActive;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.IsPromotionActive Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tính tiền giảm
        /// </summary>
        public static decimal CalculateDiscount(int promotionID, decimal amount)
        {
            try
            {
                PromotionDTO promotion = PromotionDAL.GetByID(promotionID);
                if (promotion == null || !IsPromotionActive(promotionID))
                    return 0;

                return amount * (promotion.DiscountPercent / 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.CalculateDiscount Error: {ex.Message}");
                return 0;
            }
        }
        public static DataTable GetActivePromotionsForOrder(int orderID)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                return PromotionDAL.GetActivePromotionsForOrder(orderID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.GetActivePromotionsForOrder Error: {ex.Message}");
                throw;
            }
        }

        /*
         * Tính tiền giảm dựa trên loại khuyến mãi và đơn hàng.
         *   applyType == 0 → giảm toàn bill   : discount = subTotal * percent / 100
         *   applyType == 1 → giảm theo món    : chỉ tính trên món được khuyến mãi
         */
        public static decimal CalculateDiscountForOrder(int orderID, int promotionID, int applyType, decimal subTotal, decimal discountPercent)
        {
            try
            {
                if (orderID <= 0)
                    throw new Exception("OrderID không hợp lệ!");

                if (applyType == 0)
                {
                    // Giảm toàn bill
                    return subTotal * (discountPercent / 100m);
                }
                else if (applyType == 1)
                {
                    // Giảm theo món — lấy tổng tiền các món hợp lệ từ DAL
                    decimal eligibleAmount = PromotionDAL.GetEligibleAmountForPromotion(orderID, promotionID);
                    return eligibleAmount * (discountPercent / 100m);
                }

                return 0m;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionBLL.CalculateDiscountForOrder Error: {ex.Message}");
                throw;
            }
        }
        public static DataTable GetAllPromotionsFiltered(string keyword, string status)
        {
            return PromotionDAL.GetAllFiltered(keyword, status);
        }
        public static void DeletePromotionDetails(int promotionID)
        {
            PromotionDAL.DeletePromotionDetails(promotionID);
        }
        public static void AddPromotionDetail(PromotionDetailDTO detail)
        {
            PromotionDAL.InsertPromotionDetail(detail);
        }
        public static DataTable GetPromotionDetails(int promotionID)
        {
            return PromotionDAL.GetPromotionDetails(promotionID);
        }
    }
}
