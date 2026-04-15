using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DAL.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public int? RestaurantID { get; set; }
        public int? TableID { get; set; }
        public DateTime OrderDate { get; set; }// đoạn này có thể thêm ?
        public string OrderType { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? StaffID { get; set; }
        public int? PromotionID { get; set; }

        public string CustomerName { get; set; }
        public string RestaurantName { get; set; }
        public int TableNumber { get; set; }
        public string StaffName { get; set; }
        public string PromotionName { get; set; }

        public override string ToString()
        {
            return $"Order #{OrderID} - {TotalAmount:C} ({Status})";
        }
    }
}
