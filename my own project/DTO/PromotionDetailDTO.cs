using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class PromotionDetailDTO
    {
        public int PromotionDetailID { get; set; }
        public int PromotionID { get; set; }
        public int MenuItemID { get; set; }

        // Thông tin liên quan
        public string PromotionName { get; set; }
        public string ItemName { get; set; }
        public decimal DiscountPercent { get; set; }// claude


        public override string ToString()
        {
            return $"{PromotionName} - {ItemName}";
        }
    }
}
