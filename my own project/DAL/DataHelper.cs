using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace my_own_project.DAL
{
    public class DataHelper
    {
        // ========== CONNECTION STRING ==========
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;

        /// <summary>
        /// Lấy kết nối SQL (dùng để test)
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Kiểm tra kết nối Database
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Test Failed: " + ex.Message);
                return false;
            }
        }

        // ========== EXECUTE STORED PROCEDURES ==========

        /// <summary>
        /// Thực thi SP không trả về kết quả (INSERT, UPDATE, DELETE)
        /// </summary>
        public static int ExecuteSP(string spName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 30;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in {spName}: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExecuteSP ({spName}): {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi SP trả về DataTable (SELECT)
        /// </summary>
        public static DataTable ExecuteSPGetTable(string spName, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 30;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(parameters);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in {spName}: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExecuteSPGetTable ({spName}): {ex.Message}");
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Thực thi SP trả về giá trị scalar (COUNT, SUM, v.v...)
        /// </summary>
        public static object ExecuteSPScalar(string spName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 30;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in {spName}: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExecuteSPScalar ({spName}): {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi SP có OUTPUT parameter (lấy ID sau INSERT)
        /// </summary>
        public static int ExecuteSPWithOutput(string spName, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 30;

                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        // Lấy OUTPUT parameter (thường là ID cuối cùng)
                        SqlParameter outputParam = cmd.Parameters["@ID"];
                        if (outputParam != null)
                            return (int)outputParam.Value;

                        return 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error in {spName}: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExecuteSPWithOutput ({spName}): {ex.Message}");
                throw;
            }
        }
    }
}
