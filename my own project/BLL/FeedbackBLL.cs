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
    public class FeedbackBLL
    {
        // ==================== VALIDATE ====================
        private static bool ValidateFeedback(FeedbackDTO feedback)
        {
            if (feedback.OrderID <= 0)
                throw new Exception("OrderID không hợp lệ!");

            if (feedback.UserID <= 0)
                throw new Exception("UserID không hợp lệ!");

            if (feedback.Rating < 1 || feedback.Rating > 5)
                throw new Exception("Rating phải từ 1 đến 5 sao!");

            return true;
        }

        // ==================== CREATE ====================
        public static int AddFeedback(FeedbackDTO feedback)
        {
            try
            {
                ValidateFeedback(feedback);
                return FeedbackDAL.Insert(feedback);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackBLL.AddFeedback Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAllFeedback()
        {
            try
            {
                return FeedbackDAL.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackBLL.GetAllFeedback Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetFeedbackByRating(int rating)
        {
            try
            {
                if (rating < 1 || rating > 5)
                    throw new Exception("Rating phải từ 1 đến 5!");

                return FeedbackDAL.GetByRating(rating);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackBLL.GetFeedbackByRating Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        /// <summary>
        /// Lấy rating trung bình
        /// </summary>
        public static double GetAverageRating()
        {
            try
            {
                DataTable dt = FeedbackDAL.GetAll();
                if (dt.Rows.Count == 0)
                    return 0;

                double total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    total += (int)row["Rating"];
                }

                return Math.Round(total / dt.Rows.Count, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackBLL.GetAverageRating Error: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Lấy tổng số feedback
        /// </summary>
        public static int GetTotalFeedback()
        {
            try
            {
                DataTable dt = FeedbackDAL.GetAll();
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackBLL.GetTotalFeedback Error: {ex.Message}");
                return 0;
            }
        }
    }
}
