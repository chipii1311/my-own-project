using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DAL.DTO
{
    public class RecipeDTO
    {
        public int RecipeID { get; set; }
        public int MenuItemID { get; set; }
        public int IngredientID { get; set; }
        public float Quantity { get; set; }

        // Thông tin liên quan
        public string ItemName { get; set; }
        public string IngredientName { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return $"{IngredientName} {Quantity} {Unit}";
        }
    }
}
