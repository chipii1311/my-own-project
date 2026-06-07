using my_own_project.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace my_own_project.DAL
{
    public class PromotionDAL
    {
        // ==================== CREATE ====================
        public static int Insert(PromotionDTO promotion)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PromotionName", promotion.PromotionName ?? ""),
                    new SqlParameter("@DiscountPercent", promotion.DiscountPercent),
                    new SqlParameter("@StartDate", promotion.StartDate),
                    new SqlParameter("@EndDate", promotion.EndDate),
                    new SqlParameter("@Status", promotion.Status ?? "Active"),
                    new SqlParameter("@ApplyType", promotion.ApplyType), // <-- BỔ SUNG DÒNG NÀY
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Promotion_Insert", parameters);
                return (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Promotion_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static PromotionDTO GetByID(int promotionID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PromotionID", promotionID)
                };

                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Promotion_GetByID", parameters);
                if (dt.Rows.Count > 0)
                    return MapDTO(dt.Rows[0]);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.GetByID Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetActive()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Promotion_GetActive");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.GetActive Error: {ex.Message}");
                throw;
            }
        }

        // ==================== UPDATE ====================
        // 1. Sửa lại hàm Update (thêm @ApplyType vào mảng parameters)
        public static bool Update(PromotionDTO promotion)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PromotionID", promotion.PromotionID),
                    new SqlParameter("@PromotionName", promotion.PromotionName ?? ""),
                    new SqlParameter("@DiscountPercent", promotion.DiscountPercent),
                    new SqlParameter("@StartDate", promotion.StartDate),
                    new SqlParameter("@EndDate", promotion.EndDate),
                    new SqlParameter("@Status", promotion.Status ?? "Active"),
                    new SqlParameter("@ApplyType", promotion.ApplyType) // BỔ SUNG DÒNG NÀY
                };

                DataHelper.ExecuteSP("sp_Promotion_Update", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.Update Error: {ex.Message}");
                throw;
            }
        }

        // 2. Thêm hàm này vào cuối class PromotionDAL
        public static DataTable GetPromotionDetails(int promotionID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PromotionID", promotionID)
                };
                return DataHelper.ExecuteSPGetTable("sp_PromotionDetail_GetByPromotionID", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.GetPromotionDetails Error: {ex.Message}");
                throw;
            }
        }

        // ==================== DELETE ====================
        public static bool Delete(int promotionID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PromotionID", promotionID)
                };

                DataHelper.ExecuteSP("sp_Promotion_Delete", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.Delete Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static PromotionDTO MapDTO(DataRow row)
        {
            return new PromotionDTO
            {
                PromotionID = (int)row["PromotionID"],
                PromotionName = row["PromotionName"]?.ToString() ?? "",
                DiscountPercent = (decimal)row["DiscountPercent"],
                StartDate = (DateTime)row["StartDate"],
                EndDate = (DateTime)row["EndDate"],
                Status = row["Status"]?.ToString() ?? "Active",
                // BẮT BUỘC PHẢI CÓ DÒNG NÀY ĐỂ HIỂN THỊ ĐÚNG LOẠI ÁP DỤNG
                ApplyType = row.Table.Columns.Contains("ApplyType") && row["ApplyType"] != DBNull.Value
                            ? Convert.ToInt32(row["ApplyType"]) : 0
            };
        }

        public static void InsertPromotionDetail(PromotionDetailDTO detail)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PromotionID", detail.PromotionID),
                    new SqlParameter("@MenuItemID", detail.MenuItemID)
                };

                // Gọi hàm ExecuteSP thay vì lệnh SQL thuần
                DataHelper.ExecuteSP("sp_PromotionDetail_Insert", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.InsertDetail Error: {ex.Message}");
                throw;
            }
        
           
        }
        public static DataTable GetActivePromotionsForOrder(int orderID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@OrderID", orderID)
                };
                return DataHelper.ExecuteSPGetTable("sp_Promotion_GetActiveForOrder", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.GetActivePromotionsForOrder Error: {ex.Message}");
                throw;
            }
        }

        /*
         * Tính tổng tiền các món đủ điều kiện áp dụng khuyến mãi theo món trong đơn hàng.
         * Thay thế DataHelper.ExecuteScalar() trong PaymentForm.CalculateFinalAmount().
         */
        public static decimal GetEligibleAmountForPromotion(int orderID, int promotionID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@OrderID",     orderID),
            new SqlParameter("@PromotionID", promotionID)
                };
                object result = DataHelper.ExecuteSPScalar("sp_Promotion_GetEligibleAmount", parameters);
                return (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0m;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PromotionDAL.GetEligibleAmountForPromotion Error: {ex.Message}");
                throw;
            }
        }
        public static DataTable GetAllFiltered(string keyword, string status)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@Keyword", string.IsNullOrEmpty(keyword) ? (object)DBNull.Value : keyword),
            new SqlParameter("@Status",  status == "Tất cả"            ? (object)DBNull.Value : status)
            };
            return DataHelper.ExecuteSPGetTable("sp_Promotion_GetAllFiltered", parameters);
        }
        public static void DeletePromotionDetails(int promotionID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@PromotionID", promotionID)
            };
            DataHelper.ExecuteSP("sp_PromotionDetail_DeleteByPromotion", parameters);
        }


    }
}
    