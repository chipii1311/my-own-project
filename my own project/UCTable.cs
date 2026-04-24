using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project
{
    public partial class UCTable : UserControl
    {
        public UCTable()
        {
            InitializeComponent();

            lblName.Parent = picTable;
            lblName.BackColor = Color.Transparent;

            lblName.Location = new Point((picTable.Width - lblName.Width) / 2, (picTable.Height - lblName.Height) / 2);

            guna2Panel1.BorderRadius = 10;
            guna2Panel1.BorderThickness = 1;
            guna2Panel1.BorderColor = Color.FromArgb(240, 240, 240);
            guna2Panel1.FillColor = Color.White;
        }

        public void SetTableData(string tableName, int capacity, string status, double price = 0)
        {
            lblName.Text = tableName;
            lblCapacity.Text = capacity + " Person";

            // Xử lý Phân loại Size dựa vào Capacity (Sức chứa)
            if (capacity <= 2)
            {
                lblSize.Text = "Small";
                // picTable.Image = Properties.Resources.icon_ban_2; // Bỏ comment khi bạn có ảnh
            }
            else if (capacity <= 4)
            {
                lblSize.Text = "Medium";
                // picTable.Image = Properties.Resources.icon_ban_4;
            }
            else
            {
                lblSize.Text = "Large";
                // picTable.Image = Properties.Resources.icon_ban_8;
            }

            // Căn giữa lại lblName sau khi đổi Text
            lblName.Location = new Point((picTable.Width - lblName.Width) / 2, (picTable.Height - lblName.Height) / 2);

            // Xử lý Màu sắc Trạng thái (Giống hệt hình mẫu)
            cipStatus.Text = status;
            if (status == "Available")
            {
                cipStatus.FillColor = Color.FromArgb(235, 250, 240); // Nền xanh lá nhạt
                cipStatus.ForeColor = Color.FromArgb(40, 167, 69);   // Chữ xanh lá
            }
            else if (status == "Occupied")
            {
                cipStatus.FillColor = Color.FromArgb(255, 235, 235); // Nền đỏ nhạt
                cipStatus.ForeColor = Color.FromArgb(220, 53, 69);   // Chữ đỏ
            }
            else if (status == "Reserved")
            {
                cipStatus.FillColor = Color.FromArgb(235, 240, 255); // Nền xanh dương nhạt
                cipStatus.ForeColor = Color.FromArgb(0, 123, 255);   // Chữ xanh dương
            }

            // Nếu bạn muốn dùng btnConfirm để hiển thị giá tiền (giống số $126.00 trong hình)
            btnConfirm.Text = "$" + price.ToString("N2");
            btnConfirm.FillColor = Color.Transparent;
            btnConfirm.ForeColor = Color.FromArgb(220, 53, 69); // Màu cam đỏ
            btnConfirm.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

    }
}
