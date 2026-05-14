using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class StaffForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private Guna2DataGridView dgvStaff;
        private Guna2TextBox txtUserID, txtFullName, txtEmail, txtPhone, txtPassword;
        private Guna2ComboBox cboRole, cboStatus;
        private Guna2Button btnEdit, btnClear, btnAddAccount;

        public StaffForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildUI();

            this.Load += StaffForm_Load;
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void BuildUI()
        {
            // =================================================================
            // GIẢI PHÁP LƯỚI TỔNG CHỐNG ĐÈ 100% (FIX LỖI LẸM BẢNG)
            // =================================================================
            TableLayoutPanel tlpForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Margin = new Padding(0) };
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
            TableLayoutPanel tlpMain = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(30, 0, 30, 30) };
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpForm.Controls.Add(tlpMain, 0, 1);

            // ==========================================
            // CỘT TRÁI: BẢNG DANH SÁCH NHÂN VIÊN
            // ==========================================
            Guna2Panel cardGrid = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(0, 0, 15, 0), Padding = new Padding(15) };
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
            Guna2Panel cardInput = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(15, 0, 0, 0), Padding = new Padding(30) };
            tlpMain.Controls.Add(cardInput, 1, 0);

            FlowLayoutPanel flp = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent };
            cardInput.Controls.Add(flp);

            Label lblDetail = new Label { Text = "✏️ CẬP NHẬT THÔNG TIN", Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), AutoSize = true, Margin = new Padding(0, 0, 0, 25) };
            flp.Controls.Add(lblDetail);

            int ctrlWidth = 360; // Độ rộng ô nhập liệu

            // Helper để tạo Label nhanh
            Label MakeLbl(string text) => new Label { Text = text, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(127, 140, 141), AutoSize = true, Margin = new Padding(0, 0, 0, 5) };

            flp.Controls.Add(MakeLbl("HỌ VÀ TÊN:"));
            txtFullName = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 0, 0, 15) };
            flp.Controls.Add(txtFullName);

            flp.Controls.Add(MakeLbl("EMAIL (TÀI KHOẢN):"));
            txtEmail = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 0, 0, 15) };
            flp.Controls.Add(txtEmail);

            flp.Controls.Add(MakeLbl("MẬT KHẨU:"));
            txtPassword = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PasswordChar = '●', PlaceholderText = "(Giữ nguyên nếu không đổi mật khẩu)", Margin = new Padding(0, 0, 0, 15) };
            flp.Controls.Add(txtPassword);

            flp.Controls.Add(MakeLbl("SỐ ĐIỆN THOẠI:"));
            txtPhone = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 0, 0, 20) };
            flp.Controls.Add(txtPhone);

            // Combo Vai trò & Trạng thái nằm ngang nhau
            TableLayoutPanel tlpCombos = new TableLayoutPanel { Width = ctrlWidth, Height = 75, ColumnCount = 2, RowCount = 2, Margin = new Padding(0, 0, 0, 30) };
            tlpCombos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCombos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            tlpCombos.Controls.Add(MakeLbl("VAI TRÒ:"), 0, 0);
            cboRole = new Guna2ComboBox { Dock = DockStyle.Fill, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 5, 10, 0) };
            cboRole.Items.AddRange(new object[] { "Nhân viên", "Quản lý" });
            tlpCombos.Controls.Add(cboRole, 0, 1);

            tlpCombos.Controls.Add(MakeLbl("TRẠNG THÁI:"), 1, 0);
            cboStatus = new Guna2ComboBox { Dock = DockStyle.Fill, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(10, 5, 0, 0) };
            cboStatus.Items.AddRange(new object[] { "Đang hoạt động", "Đã khóa" });
            tlpCombos.Controls.Add(cboStatus, 1, 1);
            flp.Controls.Add(tlpCombos);

            txtUserID = new Guna2TextBox { Visible = false, Size = new Size(0, 0) };
            flp.Controls.Add(txtUserID);

            // ==========================================
            // NÚT CẬP NHẬT VÀ LÀM MỚI (Đã mang trở lại)
            // ==========================================
            TableLayoutPanel tlpBtns = new TableLayoutPanel { Width = ctrlWidth, Height = 50, ColumnCount = 2, RowCount = 1, Margin = new Padding(0) };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            btnClear = new Guna2Button { Text = "LÀM MỚI", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(149, 165, 166), Cursor = Cursors.Hand };
            btnClear.Click += BtnClear_Click;
            tlpBtns.Controls.Add(btnClear, 0, 0);

            btnEdit = new Guna2Button { Text = "LƯU CẬP NHẬT", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(52, 152, 219), Cursor = Cursors.Hand };
            btnEdit.Click += BtnEdit_Click;
            tlpBtns.Controls.Add(btnEdit, 1, 0);

            flp.Controls.Add(tlpBtns);
        }

        #endregion

        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC DATABASE
        // ========================================================

        private void StaffForm_Load(object sender, EventArgs e) { LoadStaffData(); }

        private void LoadStaffData()
        {
            try
            {
                string query = "SELECT UserID AS [Mã NV], FullName AS [Họ Tên], Email, Phone AS [SĐT], Role AS [Vai trò], IsActive FROM Users ORDER BY UserID DESC";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                DataTable dtDisplay = dt.Clone();
                dtDisplay.Columns["IsActive"].DataType = typeof(string);

                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = dtDisplay.NewRow();
                    newRow["Mã NV"] = row["Mã NV"];
                    newRow["Họ Tên"] = row["Họ Tên"];
                    newRow["Email"] = row["Email"];
                    newRow["SĐT"] = row["SĐT"];
                    newRow["Vai trò"] = row["Vai trò"];
                    newRow["IsActive"] = Convert.ToBoolean(row["IsActive"]) ? "Hoạt động" : "Đã khóa";
                    dtDisplay.Rows.Add(newRow);
                }

                dgvStaff.DataSource = dtDisplay;

                if (dgvStaff.Columns.Contains("Mã NV")) dgvStaff.Columns["Mã NV"].Width = 80;
                if (dgvStaff.Columns.Contains("Vai trò")) dgvStaff.Columns["Vai trò"].Width = 100;
                if (dgvStaff.Columns.Contains("IsActive")) dgvStaff.Columns["IsActive"].Width = 110;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtUserID.Text = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtPassword.Text = "";
            if (cboRole.Items.Count > 0) cboRole.SelectedIndex = 0;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;
            dgvStaff.ClearSelection();
        }

        private void BtnClear_Click(object sender, EventArgs e) { ClearInputs(); }

        private void DgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];

                txtUserID.Text = row.Cells["Mã NV"].Value?.ToString();
                txtFullName.Text = row.Cells["Họ Tên"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtPhone.Text = row.Cells["SĐT"].Value?.ToString();

                string role = row.Cells["Vai trò"].Value?.ToString();
                if (cboRole.Items.Contains(role)) cboRole.Text = role;

                string status = row.Cells["IsActive"].Value?.ToString();
                if (cboStatus.Items.Contains(status)) cboStatus.Text = status;

                txtPassword.Text = ""; // Ẩn password cũ, chỉ nhập khi cần đổi
            }
        }

        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            NewAccountAddForm frmAdd = new NewAccountAddForm();

            // 2. Mở Form lên dưới dạng Dialog (Popup)
            // Lệnh ShowDialog() sẽ làm đóng băng màn hình StaffForm ở dưới, 
            // bắt buộc người dùng phải thao tác xong trên form Thêm mới được quay lại.
            frmAdd.ShowDialog();

            // 3. Refresh (Làm mới) lại bảng dữ liệu ngay lập tức
            // Sau khi Form Thêm đóng lại (dù là thêm thành công hay bấm dấu X tắt đi), 
            // code sẽ tiếp tục chạy xuống dòng này để tải lại danh sách vào DataGridView.
            LoadStaffData();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                MessageBox.Show("Vui lòng click chọn 1 nhân viên từ bảng bên trái để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int isActive = (cboStatus.Text == "Đang hoạt động") ? 1 : 0;

                string pwdUpdate = "";
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    pwdUpdate = $", PasswordHash = N'{txtPassword.Text}'";
                }

                string query = $"UPDATE Users SET FullName = N'{txtFullName.Text}', Email = N'{txtEmail.Text}', Phone = N'{txtPhone.Text}', " +
                               $"Role = N'{cboRole.Text}', IsActive = {isActive} {pwdUpdate} " +
                               $"WHERE UserID = {txtUserID.Text}";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputs();
                LoadStaffData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        #endregion
    }
}