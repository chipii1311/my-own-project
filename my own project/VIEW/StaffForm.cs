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
        private Guna2TextBox txtUserID;
        private Guna2TextBox txtFullName;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtPhone;
        private Guna2TextBox txtPassword;
        private Guna2ComboBox cboRole;
        private Guna2ComboBox cboStatus;

        private Guna2Button btnAdd, btnEdit, btnLock, btnClear;

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
            // GIẢI PHÁP LƯỚI TỔNG CHỐNG ĐÈ 100% (MATHEMATICALLY BULLETPROOF)
            // =================================================================
            TableLayoutPanel tlpForm = new TableLayoutPanel();
            tlpForm.Dock = DockStyle.Fill;
            tlpForm.ColumnCount = 1;
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpForm.RowCount = 2;
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F)); // Nới trần nhà lên 90px cho thoáng
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpForm.Margin = new Padding(0);
            this.Controls.Add(tlpForm);

            // --- 1. HEADER (Tự do tuyệt đối) ---
            Guna2Panel pnlHeader = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.Transparent };

            Label lblTitle = new Label
            {
                Text = "\nQUẢN LÝ NHÂN VIÊN", // Bí quyết: Thêm \n để nhân đôi chiều cao khung
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 28, 230),
                AutoSize = true,
                // Kéo ngược tọa độ Y lên (số âm) để giấu dòng trống, đặt chữ vào đúng vị trí vàng
                Location = new Point(30, -5)
            };

            pnlHeader.Controls.Add(lblTitle);
            tlpForm.Controls.Add(pnlHeader, 0, 0);

            // --- 2. LAYOUT CHÍNH (60 - 40) (Nằm gọn trong Hàng 2) ---
            TableLayoutPanel tlpMain = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(30, 10, 30, 30) };
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpForm.Controls.Add(tlpMain, 0, 1);

            // ==========================================
            // CỘT TRÁI: BẢNG DANH SÁCH NHÂN VIÊN
            // ==========================================
            Guna2Panel cardGrid = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(0, 0, 15, 0), Padding = new Padding(10) };
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
            dgvStaff.ColumnHeadersHeight = 45;
            dgvStaff.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(88, 28, 230);
            dgvStaff.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvStaff.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvStaff.RowTemplate.Height = 45;
            dgvStaff.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(240, 235, 255);
            dgvStaff.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvStaff.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgvStaff.CellClick += DgvStaff_CellClick;
            cardGrid.Controls.Add(dgvStaff);

            // ==========================================
            // CỘT PHẢI: KHU VỰC NHẬP LIỆU
            // ==========================================
            Guna2Panel cardInput = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(15, 0, 0, 0), Padding = new Padding(30) };
            tlpMain.Controls.Add(cardInput, 1, 0);

            FlowLayoutPanel flp = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent };
            cardInput.Controls.Add(flp);

            // Đã căn chỉnh Margin để đảm bảo Label không bị cắt lẹm ở trên
            Label lblDetail = new Label { Text = "THÔNG TIN TÀI KHOẢN", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), AutoSize = true, Margin = new Padding(0, 10, 0, 25) };
            flp.Controls.Add(lblDetail);

            int ctrlWidth = 380; // Giữ form nhập liệu ở mức vừa phải để không bị vỡ trên màn hình khác nhau

            // 1. Tên nhân viên
            flp.Controls.Add(new Label { Text = "HỌ VÀ TÊN:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtFullName = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Nhập họ tên...", Margin = new Padding(0, 0, 0, 15) };
            flp.Controls.Add(txtFullName);

            // 2. Email (Tài khoản)
            flp.Controls.Add(new Label { Text = "EMAIL (DÙNG ĐỂ ĐĂNG NHẬP):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtEmail = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Ví dụ: nhanvien@gmail.com", Margin = new Padding(0, 0, 0, 15) };
            flp.Controls.Add(txtEmail);

            // 3. Mật khẩu
            flp.Controls.Add(new Label { Text = "MẬT KHẨU:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtPassword = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PasswordChar = '●', PlaceholderText = "Nhập mật khẩu...", Margin = new Padding(0, 0, 0, 15) };
            flp.Controls.Add(txtPassword);

            // 4. Số điện thoại
            flp.Controls.Add(new Label { Text = "SỐ ĐIỆN THOẠI:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtPhone = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Ví dụ: 0987654321", Margin = new Padding(0, 0, 0, 20) };
            flp.Controls.Add(txtPhone);

            // 5. Vai trò & Trạng thái (Chia đôi hàng)
            TableLayoutPanel tlpCombos = new TableLayoutPanel { Width = ctrlWidth, Height = 75, ColumnCount = 2, RowCount = 2, Margin = new Padding(0, 0, 0, 30) };
            tlpCombos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCombos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            tlpCombos.Controls.Add(new Label { Text = "VAI TRÒ:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true }, 0, 0);
            cboRole = new Guna2ComboBox { Dock = DockStyle.Fill, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 5, 10, 0) };
            cboRole.Items.AddRange(new object[] { "Nhân viên", "Quản lý" });
            cboRole.SelectedIndex = 0;
            tlpCombos.Controls.Add(cboRole, 0, 1);

            tlpCombos.Controls.Add(new Label { Text = "TRẠNG THÁI:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(10, 0, 0, 0) }, 1, 0);
            cboStatus = new Guna2ComboBox { Dock = DockStyle.Fill, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(10, 5, 0, 0) };
            cboStatus.Items.AddRange(new object[] { "Đang hoạt động", "Đã khóa" });
            cboStatus.SelectedIndex = 0;
            tlpCombos.Controls.Add(cboStatus, 1, 1);

            flp.Controls.Add(tlpCombos);

            txtUserID = new Guna2TextBox { Visible = false, Size = new Size(0, 0) };
            flp.Controls.Add(txtUserID);

            // ==========================================
            // BỘ NÚT THAO TÁC 
            // ==========================================
            TableLayoutPanel tlpBtns = new TableLayoutPanel { Width = ctrlWidth, Height = 110, ColumnCount = 2, RowCount = 2, Margin = new Padding(0) };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            btnAdd = new Guna2Button { Text = "THÊM NHÂN VIÊN MỚI", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 15), BorderRadius = 5, Font = new Font("Segoe UI", 11F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnAdd.Click += BtnAdd_Click;
            tlpBtns.SetColumnSpan(btnAdd, 2);
            tlpBtns.Controls.Add(btnAdd, 0, 0);

            btnClear = new Guna2Button { Text = "LÀM MỚI", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 7, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(149, 165, 166), Cursor = Cursors.Hand };
            btnClear.Click += BtnClear_Click;
            tlpBtns.Controls.Add(btnClear, 0, 1);

            btnEdit = new Guna2Button { Text = "LƯU CẬP NHẬT", Dock = DockStyle.Fill, Margin = new Padding(7, 0, 0, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(52, 152, 219), Cursor = Cursors.Hand };
            btnEdit.Click += BtnEdit_Click;
            tlpBtns.Controls.Add(btnEdit, 1, 1);

            flp.Controls.Add(tlpBtns);
        }

        #endregion


        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC DATABASE
        // ========================================================

        private void LoadStaffData()
        {
            try
            {
                // Lấy danh sách nhân viên từ Database
                string query = "SELECT UserID AS [Mã NV], FullName AS [Họ Tên], Email, Phone AS [SĐT], Role AS [Vai trò], IsActive FROM Users ORDER BY UserID DESC";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                // Clone bảng để định dạng lại cột IsActive cho dễ nhìn
                DataTable dtDisplay = dt.Clone();
                dtDisplay.Columns["IsActive"].DataType = typeof(string); // Đổi bit thành chữ

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

                // Trang trí Grid
                if (dgvStaff.Columns.Contains("Mã NV")) dgvStaff.Columns["Mã NV"].Width = 70;
                if (dgvStaff.Columns.Contains("Vai trò")) dgvStaff.Columns["Vai trò"].Width = 90;
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
            cboRole.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            dgvStaff.ClearSelection();
        }

        #endregion


        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN (EVENTS)
        // ========================================================

        private void StaffForm_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void DgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];
                txtUserID.Text = row.Cells["Mã NV"].Value.ToString();
                txtFullName.Text = row.Cells["Họ Tên"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtPhone.Text = row.Cells["SĐT"].Value.ToString();
                cboRole.Text = row.Cells["Vai trò"].Value.ToString();

                string status = row.Cells["IsActive"].Value.ToString();
                cboStatus.Text = (status == "Hoạt động") ? "Đang hoạt động" : "Đã khóa";

                // Khi click vào để sửa, mật khẩu bị ẩn đi để bảo mật.
                // Trừ khi họ gõ mật khẩu mới, còn không thì sẽ giữ nguyên mật khẩu cũ
                txtPassword.Text = "";
                txtPassword.PlaceholderText = "(Giữ nguyên nếu không đổi mật khẩu)";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, Email và Mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Kiểm tra xem Email đã bị trùng chưa
                string checkQuery = $"SELECT COUNT(*) FROM Users WHERE Email = N'{txtEmail.Text}'";
                int count = Convert.ToInt32(my_own_project.DAL.DataHelper.ExecuteQuery(checkQuery).Rows[0][0]);
                if (count > 0)
                {
                    MessageBox.Show("Email này đã được sử dụng! Vui lòng nhập Email khác.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int isActive = (cboStatus.Text == "Đang hoạt động") ? 1 : 0;
                string query = $"INSERT INTO Users (FullName, Email, Phone, PasswordHash, Role, CreatedAt, IsActive) " +
                               $"VALUES (N'{txtFullName.Text}', N'{txtEmail.Text}', N'{txtPhone.Text}', N'{txtPassword.Text}', N'{cboRole.Text}', GETDATE(), {isActive})";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputs();
                LoadStaffData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                MessageBox.Show("Vui lòng click chọn 1 nhân viên từ danh sách để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int isActive = (cboStatus.Text == "Đang hoạt động") ? 1 : 0;

                // Nếu txtPassword trống, nghĩa là họ không muốn đổi mật khẩu -> Không UPDATE cột PasswordHash
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