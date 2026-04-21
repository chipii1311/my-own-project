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
        public class PaymentDAL
        {
        // ==================== CREATE ====================
        // ==================== CREATE ====================
        public static int Insert(PaymentDTO payment)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@OrderID", payment.OrderID),
            new SqlParameter("@Method", payment.Method ?? "Cash"),
            new SqlParameter("@Amount", payment.Amount),
            new SqlParameter("@TransactionID", payment.TransactionID ?? ""),
            new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                // GỌI THẲNG HÀM VÀ RETURN LUÔN (Vì DataHelper.ExecuteSPWithOutput đã cấu hình sẵn việc trả về ID)
                return DataHelper.ExecuteSPWithOutput("sp_Payment_Insert", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PaymentDAL.Insert Error: {ex.Message}");
                throw;
            }
        }

        // ==================== READ ====================
        public static PaymentDTO GetByOrderID(int orderID)
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@OrderID", orderID)
                    };

                    DataTable dt = DataHelper.ExecuteSPGetTable("sp_Payment_GetByOrderID", parameters);
                    if (dt.Rows.Count > 0)
                        return MapDTO(dt.Rows[0]);

                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PaymentDAL.GetByOrderID Error: {ex.Message}");
                    throw;
                }
            }

            public static DataTable GetAll()
            {
                try
                {
                    return DataHelper.ExecuteSPGetTable("sp_Payment_GetAll");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PaymentDAL.GetAll Error: {ex.Message}");
                    throw;
                }
            }

            // ==================== HELPER ====================
            private static PaymentDTO MapDTO(DataRow row)
            {
                return new PaymentDTO
                {
                    PaymentID = (int)row["PaymentID"],
                    OrderID = (int)row["OrderID"],
                    Method = row["Method"]?.ToString() ?? "",
                    Amount = (decimal)row["Amount"],
                    PaymentTime = (DateTime)row["PaymentTime"],
                    Status = row["Status"]?.ToString() ?? "Completed",
                    TransactionID = row["TransactionID"]?.ToString() ?? ""
                };
            }
        }
    }
