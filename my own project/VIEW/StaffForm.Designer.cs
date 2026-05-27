using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class StaffForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls UI
        private Guna2DataGridView dgvStaff;
        private Guna2TextBox txtUserID, txtFullName, txtEmail, txtPhone;
        private Guna2ComboBox cboRole, cboStatus;
        private Guna2Button btnEdit, btnClear, btnAddAccount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "StaffForm";
            this.Text = "StaffForm";
            this.ResumeLayout(false);
        }
        #endregion

        private void BuildUI()
        {
            // Panel bên trái: Danh sách Grid
            Guna2Panel pnlLeft = new Guna2Panel { Dock = DockStyle.Left, Width = 650, FillColor = Color.White, Padding = new Padding(20) };
            dgvStaff = new Guna2DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvStaff.CellClick += DgvStaff_CellClick;
            pnlLeft.Controls.Add(dgvStaff);
            this.Controls.Add(pnlLeft);

            // Panel bên phải: Form chỉnh sửa
            Guna2Panel pnlRight = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.WhiteSmoke, Padding = new Padding(20) };

            txtUserID = new Guna2TextBox { Visible = false };
            txtFullName = MakeField("Họ và tên", 0);
            txtEmail = MakeField("Email", 80);
            txtPhone = MakeField("SĐT", 160);
            cboRole = MakeCombo(new[] { "Quản lý", "Nhân viên" }, 240);
            cboStatus = MakeCombo(new[] { "Đang hoạt động", "Đã nghỉ" }, 320);

            btnEdit = new Guna2Button { Text = "Lưu thay đổi", Size = new Size(120, 40), Location = new Point(20, 400), FillColor = Color.Purple };
            btnEdit.Click += BtnEdit_Click;

            btnClear = new Guna2Button { Text = "Hủy", Size = new Size(80, 40), Location = new Point(150, 400), FillColor = Color.Gray };
            btnClear.Click += BtnClear_Click;

            btnAddAccount = new Guna2Button { Text = "Tạo tài khoản mới", Size = new Size(200, 40), Location = new Point(20, 460), FillColor = Color.Green };
            btnAddAccount.Click += BtnAddAccount_Click;

            pnlRight.Controls.AddRange(new Control[] { txtUserID, txtFullName, txtEmail, txtPhone, cboRole, cboStatus, btnEdit, btnClear, btnAddAccount });
            this.Controls.Add(pnlRight);
        }

        private Guna2TextBox MakeField(string label, int y)
        {
            return new Guna2TextBox { PlaceholderText = label, Size = new Size(300, 40), Location = new Point(20, y) };
        }

        private Guna2ComboBox MakeCombo(string[] items, int y)
        {
            var cbo = new Guna2ComboBox { Size = new Size(300, 40), Location = new Point(20, y) };
            cbo.Items.AddRange(items);
            return cbo;
        }
    }
}