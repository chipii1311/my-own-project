using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace my_own_project.DAL
{
    public class FeedbackDAL
    {
        // ==================== CREATE ====================
        public static int Insert(FeedbackDTO feedback)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderID", feedback.OrderID),
                    new SqlParameter("@UserID", feedback.UserID),
                    new SqlParameter("@Rating", feedback.Rating),
                    new SqlParameter("@Comment", feedback.Comment ?? ""),
                    new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                DataHelper.ExecuteSPWithOutput("sp_Feedback_Insert", parameters);
                return (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static DataTable GetAll()
        {
            try
            {
                return DataHelper.ExecuteSPGetTable("sp_Feedback_GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackDAL.GetAll Error: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetByRating(int rating)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Rating", rating)
                };

                return DataHelper.ExecuteSPGetTable("sp_Feedback_GetByRating", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FeedbackDAL.GetByRating Error: {ex.Message}");
                throw;
            }
        }

        // ==================== HELPER ====================
        private static FeedbackDTO MapDTO(DataRow row)
        {
            return new FeedbackDTO
            {
                FeedbackID = (int)row["FeedbackID"],
                OrderID = (int)row["OrderID"],
                UserID = (int)row["UserID"],
                Rating = (int)row["Rating"],
                Comment = row["Comment"]?.ToString() ?? "",
                CreatedAt = (DateTime)row["CreatedAt"],
                UserName = row["UserName"]?.ToString() ?? ""
            };
        }
    }
}
