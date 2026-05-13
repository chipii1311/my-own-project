using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class IngredientDTO
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public string Unit { get; set; }
        public float StockQuantity { get; set; }
        public float MinStock { get; set; }
        public decimal PurchasePrice { get; set; }
        public bool IsActive { get; set; }
    }
}
