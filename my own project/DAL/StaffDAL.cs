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
    public class StaffDAL
    {
        public static int GetStaffIDByUserID(int userID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userID)
            };

            DataTable dt = DataHelper.ExecuteSPGetTable("sp_Staff_GetByUserID", parameters);

            if (dt == null || dt.Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dt.Rows[0]["StaffID"]);
        }
    }
}

