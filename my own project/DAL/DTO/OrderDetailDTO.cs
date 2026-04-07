using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DAL.DTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int MenuItemID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string Note { get; set; }

        // Thông tin liên quan
        public string ItemName { get; set; }
        public string ImageUrl { get; set; }
        public override string ToString()
        {
            return $"{ItemName} x{Quantity} = {SubTotal:C}";
        }
    }
}
