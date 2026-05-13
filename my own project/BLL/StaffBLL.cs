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
    public class StaffBLL
    {
        public static int GetStaffIDByUserID(int userID)
        {
            if (userID <= 0)
                return 0;

            return StaffDAL.GetStaffIDByUserID(userID);
        }
    }
}
