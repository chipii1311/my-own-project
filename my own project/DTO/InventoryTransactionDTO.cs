using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class InventoryTransactionDTO
    {
        public int TransactionID { get; set; }
        public int IngredientID { get; set; }
        public float QuantityChanged { get; set; }
        public string TransactionType { get; set; }  // Import, Export, Adjustment
        public DateTime TransactionDate { get; set; }
        public int StaffID { get; set; }
        public string Note { get; set; }

        // Thông tin liên quan
        public string IngredientName { get; set; }
        public string StaffName { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return $"{TransactionType}: {IngredientName} {QuantityChanged} {Unit}";
        }
    }
}
