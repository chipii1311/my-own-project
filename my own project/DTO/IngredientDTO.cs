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
        public string Unit { get; set; }  // kg, lít, cái, gram, ml
        public float StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public float MinStock { get; set; }

        // Tính toán thêm
        public bool IsLowStock => StockQuantity < MinStock;

        public override string ToString()
        {
            return $"{IngredientName} ({StockQuantity} {Unit})";
        }
    }
}
