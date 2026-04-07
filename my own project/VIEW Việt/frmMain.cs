using my_own_project.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW_Việt
{
    public partial class frmMain : Form
    {
        private Timer timerClock;
        public frmMain()
        {
            InitializeComponent();
            StyleForm();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập!", "Thông báo");
                this.Close();
                return;
            }

            DisplayUserInfo();
            SetupMenuByRole();
            StartClock();

            MessageBox.Show($"🎉 Chào mừng {CurrentUser.FullName}!", "Đăng Nhập Thành Công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void StyleForm()
        {
            this.Text = "Hệ Thống Quản Lý Nhà Hàng";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.FormBorderStyle = FormBorderStyle.None;

            // Thiết lập topbar
            pnlTopBar.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            lblLogo.ForeColor = System.Drawing.Color.White;

            // Thiết lập sidebar
            pnlSidebar.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            pnlSidebar.FillColor = System.Drawing.Color.FromArgb(44, 62, 80);

            // Thiết lập status bar
            pnlStatusBar.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
        }

        /// <summary>
        /// Hiển thị thông tin user
        /// </summary>
        private void DisplayUserInfo()
        {
            lblUserName.Text = CurrentUser.FullName;
            lblUserRole.Text = $"({CurrentUser.Role})";
            lblStatus.Text = $"👤 {CurrentUser.FullName} | Vai trò: {CurrentUser.Role}";
        }

        /// <summary>
        /// Thiết lập menu dựa trên role
        /// </summary>
        private void SetupMenuByRole()
        {
            // Chỉ Admin mới thấy Settings
            btnSettings.Visible = CurrentUser.IsAdmin;

            // Chef chỉ thấy một số menu
            if (CurrentUser.IsChef)
            {
                btnMenu.Visible = false;
                btnPayment.Visible = false;
                btnSettings.Visible = false;
                btnReport.Visible = false;
            }

            // Cashier chỉ thấy Payment
            if (CurrentUser.IsCashier)
            {
                btnDashboard.Visible = false;
                btnMenu.Visible = false;
                btnTable.Visible = false;
                btnInventory.Visible = false;
                btnReport.Visible = false;
                btnSettings.Visible = false;
            }
        }

        /// <summary>
        /// Bắt đầu clock
        /// </summary>
        private void StartClock()
        {
            timerClock = new Timer();
            timerClock.Interval = 1000;
            timerClock.Tick += (s, e) => UpdateClock();
            timerClock.Start();
        }

        /// <summary>
        /// Cập nhật đồng hồ
        /// </summary>
        private void UpdateClock()
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss | dddd, dd/MM/yyyy");
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "📊 Dashboard - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Dashboard sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsWaiter && !CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "🍽️ Quản lý Đơn Hàng - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Đơn Hàng sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "📋 Quản lý Menu - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Menu sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "🪑 Quản lý Bàn Ăn - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Bàn Ăn sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsCashier && !CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "💳 Thanh Toán - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Thanh Toán sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "📦 Quản lý Kho - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Kho sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "📈 Báo Cáo - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Báo Cáo sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin)
            {
                MessageBox.Show("Chỉ Admin mới có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "⚙️ Cài Đặt Hệ Thống - Chưa hoàn thành";
            MessageBox.Show("✨ Trang Cài Đặt sẽ được cập nhật sớm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác Nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CurrentUser.Logout();
                timerClock.Stop();

                frmLogin loginForm = new frmLogin();
                this.Hide();
                loginForm.Show();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát ứng dụng?", "Xác Nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                CurrentUser.Logout();
                timerClock.Stop();
                Application.Exit();
            }
        }
        public void UpdateStatus(string message)
        {
            lblStatus.Text = message;
        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }
    }
}
