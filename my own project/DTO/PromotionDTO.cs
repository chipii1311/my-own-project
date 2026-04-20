using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class PromotionDTO
    {
        public int PromotionID { get; set; }
        public string PromotionName { get; set; }
        public decimal DiscountPercent { get; set; }  // 0-100
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }  // Active, Inactive, Expired
        public int ApplyType { get; set; }

        // Tính toán thêm
        public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate && Status == "Active";

        public override string ToString()
        {
            return $"{PromotionName} ({DiscountPercent}% off)";
        }
    }
}
