using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class OrderHistoryDTO
    {
        public int HistoryID { get; set; }
        public int OrderID { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public int? ChangedBy { get; set; }

        // Thông tin liên quan
        public string ChangedByName { get; set; }

        public override string ToString()
        {
            return $"{OldStatus} → {NewStatus} at {ChangedAt:HH:mm:ss}";
        }
    }
}
