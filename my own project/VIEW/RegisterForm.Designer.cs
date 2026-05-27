using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        // Các Control UI
        private Guna2Panel pnlLeft;
        private Guna2TextBox txtFullName, txtEmail, txtPhone, txtPassword, txtConfirmPassword;
        private Guna2Button btnCreate, btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Name = "RegisterForm";
            this.Text = "Đăng Ký Tài Khoản";
            this.ResumeLayout(false);
        }
        #endregion

        private void BuildUI()
        {
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- Panel trang trí bên trái ---
            pnlLeft = new Guna2Panel { Dock = DockStyle.Left, Width = 300, FillColor = Color.FromArgb(88, 28, 230) };
            Label lblWelcome = new Label { Text = "Chào mừng bạn đến với\nHệ thống Quản lý!", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(30, 100), AutoSize = true };
            pnlLeft.Controls.Add(lblWelcome);
            this.Controls.Add(pnlLeft);

            // --- Khu vực nhập liệu bên phải ---
            Label lblTitle = new Label { Text = "Tạo tài khoản mới", Font = new Font("Segoe UI", 18F, FontStyle.Bold), Location = new Point(350, 40), AutoSize = true };
            this.Controls.Add(lblTitle);

            txtFullName = MakeField("Họ và tên", 350, 100);
            txtEmail = MakeField("Email", 350, 170);
            txtPhone = MakeField("Số điện thoại", 350, 240);
            txtPassword = MakeField("Mật khẩu", 350, 310, true);
            txtConfirmPassword = MakeField("Xác nhận mật khẩu", 350, 380, true);

            btnCreate = new Guna2Button { Text = "ĐĂNG KÝ", Size = new Size(180, 45), Location = new Point(350, 450), BorderRadius = 8, FillColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCreate.Click += BtnCreate_Click;

            btnBack = new Guna2Button { Text = "Quay lại", Size = new Size(100, 45), Location = new Point(540, 450), BorderRadius = 8, FillColor = Color.LightGray, ForeColor = Color.Black, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnBack.Click += BtnBack_Click;

            this.Controls.AddRange(new Control[] { txtFullName, txtEmail, txtPhone, txtPassword, txtConfirmPassword, btnCreate, btnBack });
        }

        private Guna2TextBox MakeField(string placeholder, int x, int y, bool isPass = false)
        {
            var txt = new Guna2TextBox { PlaceholderText = placeholder, Size = new Size(400, 45), Location = new Point(x, y), BorderRadius = 8, Font = new Font("Segoe UI", 10F) };
            if (isPass) txt.PasswordChar = '●';
            return txt;
        }
    }
}