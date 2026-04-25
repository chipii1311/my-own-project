using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEWSTAFF
{
    public partial class StaffMainFormgpt : Form
    {
        public StaffMainFormgpt()
        {
            InitializeComponent();
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        // Navigation
        private void BtnTrangChu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Trang chủ");
        }

        private void BtnBan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form quản lý bàn");
            // new TableForm().Show();
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form gọi món");
        }

        private void BtnHoaDon_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form hóa đơn");
        }

    }
}
