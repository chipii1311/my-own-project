using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class MenuItemDTO
    {
        public int MenuItemID { get; set; }
        public int RestaurantID { get; set; }
        public int CategoryID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }  // Active, Inactive, OutOfStock
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Thông tin liên quan (để dễ hiển thị)
        public string CategoryName { get; set; }
        public string RestaurantName { get; set; }

        public override string ToString()
        {
            return $"{ItemName} - {Price:C}";
        }
    }
}
