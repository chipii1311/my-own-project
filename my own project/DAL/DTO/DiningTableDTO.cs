using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DAL.DTO
{
    public class DiningTableDTO
    {
        public int TableID { get; set; }
        public int RestaurantID { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }  // Số ghế
        public string Status { get; set; }  // Available, Occupied, Maintenance
        public string Notes { get; set; }

        // Thông tin liên quan
        public string RestaurantName { get; set; }

        public override string ToString()
        {
            return $"Bàn {TableNumber} ({Status}) - {Capacity} chỗ";
        }
    }
}
