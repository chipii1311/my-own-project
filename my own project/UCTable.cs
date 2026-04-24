using System;
using System.Drawing;
using System.Windows.Forms;
using my_own_project.DTO; // Thêm namespace DTO của bạn

namespace my_own_project
{
    public partial class UCTable : UserControl
    {
        public DiningTableDTO TableData { get; private set; }

        public UCTable()
        {
            InitializeComponent();
            // Thiết lập lblName nằm trên picTable
            lblName.Parent = picTable;
            lblName.BackColor = Color.Transparent;
            // Căn giữa tên bàn
            lblName.Location = new Point((picTable.Width - lblName.Width) / 2, (picTable.Height - lblName.Height) / 2);
        }

        // Thêm tham số 'int index' vào hàm
        public void SetTableData(DiningTableDTO table)
        {
            this.TableData = table;

            // Hiển thị tên bàn dạng T-Stt (Ví dụ: bàn số 1 hiển thị là T-01)
            lblName.Text = "T-" + table.TableNumber.ToString("D2");

            lblCapacity.Text = table.Capacity + " Person";

            // Phân loại nhãn kích thước dựa trên sức chứa
            if (table.Capacity <= 2) lblSize.Text = "Small";
            else if (table.Capacity <= 4) lblSize.Text = "Medium";
            else lblSize.Text = "Large";

            // Căn giữa nhãn tên bàn vào PictureBox
            lblName.Location = new Point((picTable.Width - lblName.Width) / 2, (picTable.Height - lblName.Height) / 2);

            // Cập nhật giao diện trạng thái
            cipStatus.Text = table.Status;
            if (table.Status == "Available")
            {
                cipStatus.FillColor = Color.FromArgb(235, 250, 240);
                cipStatus.ForeColor = Color.FromArgb(40, 167, 69);
                btnConfirm.Visible = false;
            }
            else
            {
                cipStatus.FillColor = Color.FromArgb(255, 235, 235);
                cipStatus.ForeColor = Color.FromArgb(220, 53, 69);
                btnConfirm.Visible = true;
                btnConfirm.Text = "$0.00";
            }
        }
    }
}