using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_own_project.DTO
{
    public class FeedbackDTO
    {
        public int FeedbackID { get; set; }
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int Rating { get; set; }  // 1-5 sao
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thông tin liên quan
        public string UserName { get; set; }

        public override string ToString()
        {
            return $"{Rating}★ - {Comment}";
        }
    }
}
