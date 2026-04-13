using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public string Method { get; set; }  // Cash, Card, Bank Transfer, E-Wallet
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public string Status { get; set; }  // Completed, Failed, Pending, Refunded
        public string TransactionID { get; set; }

        // Thông tin liên quan
        public string CustomerName { get; set; }
        public int TableNumber { get; set; }// claude
        public override string ToString()
        {
            return $"Payment #{PaymentID} - {Amount:C} ({Method})";
        }
    }
}
