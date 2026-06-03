using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class StaffForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls UI
        protected Guna2DataGridView dgvStaff;
        protected Guna2TextBox txtFullName, txtEmail, txtPhone;
        protected Guna2ComboBox cboRole, cboStatus;
        protected Guna2Button btnEdit, btnDelete, btnClear, btnAddAccount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1400, 750);
            this.Name = "StaffForm";
            this.Text = "QUẢN LÝ TÀI KHOẢN";
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildUI();
            this.ResumeLayout(false);
        }

        #endregion

        private void BuildUI()
        {
            // =================================================================
            // GIẢI PHÁP LƯỚI TỔNG CHỐNG ĐÈ 100% (FIX LỖI LẸM BẢNG)
            // =================================================================
            TableLayoutPanel tlpForm = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F)); // Hàng 1: Dành 90px cho Header
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Hàng 2: Phần còn lại cho Nội dung
            this.Controls.Add(tlpForm);

            // --- 1. HEADER (Nằm trong Hàng 1) ---
            Guna2Panel pnlHeader = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.Transparent };

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ TÀI KHOẢN",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 28, 230),
                AutoSize = true,
                Location = new Point(30, 25)
            };

            btnAddAccount = new Guna2Button
            {
                Text = "➕ THÊM TÀI KHOẢN MỚI",
                Size = new Size(240, 48),
                BorderRadius = 6,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FillColor = Color.FromArgb(46, 204, 113),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.Width - 270, 20)
            };
            btnAddAccount.Click += BtnAddAccount_Click;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnAddAccount);

            // Neo nút Thêm bên phải khi thu phóng
            pnlHeader.Resize += (s, e) => {
                btnAddAccount.Location = new Point(pnlHeader.Width - btnAddAccount.Width - 30, 20);
            };

            tlpForm.Controls.Add(pnlHeader, 0, 0);

            // --- 2. LAYOUT CHÍNH (Nằm trong Hàng 2 - Chia 60 Trái / 40 Phải) ---
            TableLayoutPanel tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(30, 0, 30, 30)
            };
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpForm.Controls.Add(tlpMain, 0, 1);

            // ==========================================
            // CỘT TRÁI: BẢNG DANH SÁCH NHÂN VIÊN
            // ==========================================
            Guna2Panel cardGrid = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 10,
                Margin = new Padding(0, 0, 15, 0),
                Padding = new Padding(15)
            };
            tlpMain.Controls.Add(cardGrid, 0, 0);

            dgvStaff = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                GridColor = Color.FromArgb(235, 235, 235),
                Cursor = Cursors.Hand
            };

            dgvStaff.ColumnHeadersHeight = 50;
            dgvStaff.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(88, 28, 230);
            dgvStaff.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvStaff.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvStaff.RowTemplate.Height = 48;
            dgvStaff.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(240, 235, 255);
            dgvStaff.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvStaff.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10.5F);

            dgvStaff.CellClick += DgvStaff_CellClick;
            cardGrid.Controls.Add(dgvStaff);

            // ==========================================
            // CỘT PHẢI: KHU VỰC SỬA / CẬP NHẬT THÔNG TIN
            // ==========================================
            Guna2Panel cardInput = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderRadius = 10,
                Margin = new Padding(15, 0, 0, 0),
                Padding = new Padding(30)
            };
            tlpMain.Controls.Add(cardInput, 1, 0);

            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            cardInput.Controls.Add(flp);

            Label lblDetail = new Label
            {
                Text = "✏️ CẬP NHẬT THÔNG TIN",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };
            flp.Controls.Add(lblDetail);

            int ctrlWidth = 360; // Độ rộng ô nhập liệu

            // Helper để tạo Label nhanh
            Label MakeLbl(string text) => new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 5)
            };

            flp.Controls.Add(MakeLbl("HỌ VÀ TÊN:"));
            txtFullName = new Guna2TextBox
            {
                Width = ctrlWidth,
                Height = 42,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 11F),
                Margin = new Padding(0, 0, 0, 15)
            };
            flp.Controls.Add(txtFullName);

            flp.Controls.Add(MakeLbl("EMAIL (TÀI KHOẢN):"));
            txtEmail = new Guna2TextBox
            {
                Width = ctrlWidth,
                Height = 42,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 11F),
                Margin = new Padding(0, 0, 0, 15)
            };
            flp.Controls.Add(txtEmail);

            flp.Controls.Add(MakeLbl("SỐ ĐIỆN THOẠI:"));
            txtPhone = new Guna2TextBox
            {
                Width = ctrlWidth,
                Height = 42,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 11F),
                Margin = new Padding(0, 0, 0, 20)
            };
            flp.Controls.Add(txtPhone);

            // Combo Vai trò & Trạng thái nằm ngang nhau
            TableLayoutPanel tlpCombos = new TableLayoutPanel
            {
                Width = ctrlWidth,
                Height = 75,
                ColumnCount = 2,
                RowCount = 2,
                Margin = new Padding(0, 0, 0, 30)
            };
            tlpCombos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCombos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            tlpCombos.Controls.Add(MakeLbl("VAI TRÒ:"), 0, 0);
            cboRole = new Guna2ComboBox
            {
                Dock = DockStyle.Fill,
                Height = 42,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 11F),
                Margin = new Padding(0, 5, 10, 0),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRole.Items.AddRange(new object[] { "Nhân viên", "Quản lý" });
            tlpCombos.Controls.Add(cboRole, 0, 1);

            tlpCombos.Controls.Add(MakeLbl("TRẠNG THÁI:"), 1, 0);
            cboStatus = new Guna2ComboBox
            {
                Dock = DockStyle.Fill,
                Height = 42,
                BorderRadius = 5,
                Font = new Font("Segoe UI", 11F),
                Margin = new Padding(10, 5, 0, 0),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[] { "Hoạt động", "Đã khóa" });
            tlpCombos.Controls.Add(cboStatus, 1, 1);
            flp.Controls.Add(tlpCombos);

            // ==========================================
            // NÚT CẬP NHẬT, XÓA VÀ LÀM MỚI
            // ==========================================
            TableLayoutPanel tlpBtns = new TableLayoutPanel
            {
                Width = ctrlWidth,
                Height = 50,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0)
            };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            btnClear = new Guna2Button
            {
                Text = "LÀM MỚI",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 5, 0),
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(149, 165, 166),
                Cursor = Cursors.Hand
            };
            btnClear.Click += BtnClear_Click;
            tlpBtns.Controls.Add(btnClear, 0, 0);

            btnEdit = new Guna2Button
            {
                Text = "LƯU CẬP NHẬT",
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 0, 5, 0),
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(52, 152, 219),
                Cursor = Cursors.Hand
            };
            btnEdit.Click += BtnEdit_Click;
            tlpBtns.Controls.Add(btnEdit, 1, 0);

            btnDelete = new Guna2Button
            {
                Text = "XÓA TÀI KHOẢN",
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 0, 0, 0),
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(231, 76, 60),
                Cursor = Cursors.Hand
            };
            btnDelete.Click += BtnDelete_Click;
            tlpBtns.Controls.Add(btnDelete, 2, 0);

            flp.Controls.Add(tlpBtns);
        }
    }
}
