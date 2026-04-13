using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class StaffDTO
    {
        public int StaffID { get; set; }
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        public string Position { get; set; }  // Chef, Waiter, Cashier, Manager
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string Status { get; set; }  // Active, Inactive, OnLeave

        // Thông tin liên quan
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RestaurantName { get; set; }

        public override string ToString()
        {
            return $"{FullName} - {Position}";
        }
    }
}
