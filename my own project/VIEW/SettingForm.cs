using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class SettingForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private Panel pageTables, pageCategories, pageInfo;
        private Guna2Button btnMenuTable, btnMenuCategory, btnMenuInfo;

        // BIẾN CHO TAB DANH MỤC
        private Guna2DataGridView dgvCategories;
        private Guna2TextBox txtCategoryName;
        private Guna2TextBox txtCategoryID;

        // BIẾN CHO TAB BÀN ĂN
        private Guna2DataGridView dgvTables;
        private Guna2TextBox txtTableName;
        private Guna2ComboBox cboTableStatus;
        private Guna2TextBox txtTableID;

        public SettingForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildBulletproofLayout(); // Vẽ giao diện

            // Chuyển việc tải dữ liệu xuống sự kiện Load
            this.Load += SettingForm_Load;
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void BuildBulletproofLayout()
        {
            TableLayoutPanel tlpMain = new TableLayoutPanel();
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(tlpMain);

            // --- SIDEBAR ---
            Guna2Panel pnlMenu = new Guna2Panel();
            pnlMenu.Dock = DockStyle.Fill;
            pnlMenu.FillColor = Color.White;
            pnlMenu.CustomBorderThickness = new Padding(0, 0, 1, 0);
            pnlMenu.CustomBorderColor = Color.LightGray;
            tlpMain.Controls.Add(pnlMenu, 0, 0);

            FlowLayoutPanel flpMenu = new FlowLayoutPanel();
            flpMenu.Dock = DockStyle.Fill;
            flpMenu.FlowDirection = FlowDirection.TopDown;
            flpMenu.Padding = new Padding(15, 30, 15, 10);
            flpMenu.BackColor = Color.Transparent;
            pnlMenu.Controls.Add(flpMenu);

            Label lblTitle = new Label();
            lblTitle.Text = "CÀI ĐẶT";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Margin = new Padding(10, 20, 0, 30);
            flpMenu.Controls.Add(lblTitle);

            btnMenuTable = CreateMenuButton("Quản lý Bàn");
            btnMenuTable.Click += BtnMenuTable_Click;
            flpMenu.Controls.Add(btnMenuTable);

            btnMenuCategory = CreateMenuButton("Danh mục món");
            btnMenuCategory.Click += BtnMenuCategory_Click;
            flpMenu.Controls.Add(btnMenuCategory);

            btnMenuInfo = CreateMenuButton("Thông tin quán");
            btnMenuInfo.Click += BtnMenuInfo_Click;
            flpMenu.Controls.Add(btnMenuInfo);

            // --- NỘI DUNG (CONTENT) ---
            Guna2Panel pnlContent = new Guna2Panel();
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(30);
            pnlContent.BackColor = Color.Transparent;
            tlpMain.Controls.Add(pnlContent, 1, 0);

            pageTables = BuildPageTables();
            pageCategories = BuildPageCategories();
            pageInfo = BuildPageInfo();

            pnlContent.Controls.Add(pageTables);
            pnlContent.Controls.Add(pageCategories);
            pnlContent.Controls.Add(pageInfo);

            SwitchPage(pageTables, btnMenuTable); // Mặc định mở trang Bàn
        }

        private Panel BuildPageTables()
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(20) };

            Label lblHeader = new Label
            {
                Text = "QUẢN LÝ BÀN ĂN",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 28, 230),
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 5, 0, 10) // Nới viền trên 5px, dưới 10px để không bị lẹm dấu
            };

            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlp.Padding = new Padding(0, 20, 0, 0);
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnl.Controls.Add(tlp);

            // CARD TRÁI: BẢNG DỮ LIỆU BÀN
            Guna2Panel cardLeft = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(0, 0, 10, 0), Padding = new Padding(5) };
            tlp.Controls.Add(cardLeft, 0, 0);

            dgvTables = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                GridColor = Color.FromArgb(235, 235, 235)
            };
            dgvTables.ColumnHeadersHeight = 45;
            dgvTables.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(88, 28, 230);
            dgvTables.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvTables.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvTables.RowTemplate.Height = 45;
            dgvTables.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvTables.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(64, 64, 64);
            dgvTables.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(240, 235, 255);
            dgvTables.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvTables.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 11F);
            dgvTables.CellClick += DgvTables_CellClick;
            cardLeft.Controls.Add(dgvTables);

            // CARD PHẢI: NHẬP LIỆU BÀN
            Guna2Panel cardRight = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(10, 0, 0, 0), Padding = new Padding(25) };
            tlp.Controls.Add(cardRight, 1, 0);

            FlowLayoutPanel flp = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.Transparent };
            cardRight.Controls.Add(flp);

            Label lblDetail = new Label { Text = "THÔNG TIN BÀN", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), AutoSize = true, Margin = new Padding(0, 0, 0, 25) };
            flp.Controls.Add(lblDetail);

            int ctrlWidth = 350;

            flp.Controls.Add(new Label { Text = "TÊN BÀN:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtTableName = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Ví dụ: Bàn 01", Margin = new Padding(0, 0, 0, 20) };
            flp.Controls.Add(txtTableName);

            flp.Controls.Add(new Label { Text = "TRẠNG THÁI:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            cboTableStatus = new Guna2ComboBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 0, 0, 25) };
            cboTableStatus.Items.AddRange(new object[] { "Trống", "Đang có khách", "Sửa chữa" });
            cboTableStatus.SelectedIndex = 0;
            flp.Controls.Add(cboTableStatus);

            txtTableID = new Guna2TextBox { Visible = false };
            flp.Controls.Add(txtTableID);

            TableLayoutPanel tlpBtns = new TableLayoutPanel { Width = ctrlWidth, Height = 100, ColumnCount = 2, RowCount = 2, Margin = new Padding(0) };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            Guna2Button btnAdd = new Guna2Button { Text = "THÊM", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 5, 5), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnAdd.Click += BtnAddTable_Click;
            tlpBtns.Controls.Add(btnAdd, 0, 0);

            Guna2Button btnEdit = new Guna2Button { Text = "SỬA", Dock = DockStyle.Fill, Margin = new Padding(5, 0, 0, 5), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(52, 152, 219), Cursor = Cursors.Hand };
            btnEdit.Click += BtnEditTable_Click;
            tlpBtns.Controls.Add(btnEdit, 1, 0);

            Guna2Button btnDelete = new Guna2Button { Text = "XÓA", Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(255, 107, 129), Cursor = Cursors.Hand };
            btnDelete.Click += BtnDeleteTable_Click;
            tlpBtns.SetColumnSpan(btnDelete, 2);
            tlpBtns.Controls.Add(btnDelete, 0, 1);

            flp.Controls.Add(tlpBtns);

            return pnl;
        }

        private Panel BuildPageCategories()
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(20) };

            Label lblHeader = new Label { Text = "DANH MỤC MÓN ĂN", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), AutoSize = true, Dock = DockStyle.Top };
            pnl.Controls.Add(lblHeader);

            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlp.Padding = new Padding(0, 20, 0, 0);
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnl.Controls.Add(tlp);

            // CARD TRÁI: BẢNG DỮ LIỆU DANH MỤC
            Guna2Panel cardLeft = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(0, 0, 10, 0), Padding = new Padding(5) };
            tlp.Controls.Add(cardLeft, 0, 0);

            dgvCategories = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                GridColor = Color.FromArgb(235, 235, 235)
            };
            dgvCategories.ColumnHeadersHeight = 45;
            dgvCategories.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(88, 28, 230);
            dgvCategories.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvCategories.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvCategories.RowTemplate.Height = 45;
            dgvCategories.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvCategories.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(64, 64, 64);
            dgvCategories.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(240, 235, 255);
            dgvCategories.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            dgvCategories.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 11F);
            dgvCategories.CellClick += DgvCategories_CellClick;
            cardLeft.Controls.Add(dgvCategories);

            // CARD PHẢI: NHẬP LIỆU DANH MỤC
            Guna2Panel cardRight = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(10, 0, 0, 0), Padding = new Padding(25) };
            tlp.Controls.Add(cardRight, 1, 0);

            FlowLayoutPanel flp = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.Transparent };
            cardRight.Controls.Add(flp);

            Label lblDetail = new Label { Text = "THÔNG TIN CHI TIẾT", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), AutoSize = true, Margin = new Padding(0, 0, 0, 25) };
            flp.Controls.Add(lblDetail);

            int ctrlWidth = 350;

            flp.Controls.Add(new Label { Text = "TÊN DANH MỤC:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtCategoryName = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Ví dụ: Đồ uống", Margin = new Padding(0, 0, 0, 25) };
            flp.Controls.Add(txtCategoryName);

            txtCategoryID = new Guna2TextBox { Visible = false };
            flp.Controls.Add(txtCategoryID);

            TableLayoutPanel tlpBtns = new TableLayoutPanel { Width = ctrlWidth, Height = 100, ColumnCount = 2, RowCount = 2, Margin = new Padding(0) };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            Guna2Button btnAdd = new Guna2Button { Text = "THÊM", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 5, 5), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnAdd.Click += BtnAddCategory_Click;
            tlpBtns.Controls.Add(btnAdd, 0, 0);

            Guna2Button btnEdit = new Guna2Button { Text = "SỬA", Dock = DockStyle.Fill, Margin = new Padding(5, 0, 0, 5), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(52, 152, 219), Cursor = Cursors.Hand };
            btnEdit.Click += BtnEditCategory_Click;
            tlpBtns.Controls.Add(btnEdit, 1, 0);

            Guna2Button btnDelete = new Guna2Button { Text = "XÓA", Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(255, 107, 129), Cursor = Cursors.Hand };
            btnDelete.Click += BtnDeleteCategory_Click;
            tlpBtns.SetColumnSpan(btnDelete, 2);
            tlpBtns.Controls.Add(btnDelete, 0, 1);

            flp.Controls.Add(tlpBtns);

            return pnl;
        }

        private Panel BuildPageInfo()
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill };
            Label lblHeader = new Label { Text = "THIẾT LẬP THÔNG TIN (Sắp hoàn thiện)", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), AutoSize = true, Dock = DockStyle.Top };
            pnl.Controls.Add(lblHeader);
            return pnl;
        }

        private Guna2Button CreateMenuButton(string text)
        {
            Guna2Button btn = new Guna2Button();
            btn.Size = new Size(200, 45);
            btn.Margin = new Padding(0, 0, 0, 10);
            btn.BorderRadius = 8;
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.TextAlign = HorizontalAlignment.Left;
            btn.TextOffset = new Point(10, 0);
            btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btn.Cursor = Cursors.Hand;
            btn.FillColor = Color.Transparent;
            btn.ForeColor = Color.FromArgb(64, 64, 64);
            btn.CheckedState.FillColor = Color.FromArgb(240, 235, 255);
            btn.CheckedState.ForeColor = Color.FromArgb(88, 28, 230);
            return btn;
        }

        #endregion

        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC
        // ========================================================

        private void LoadTableData()
        {
            try
            {
                string query = "SELECT TableID AS [Mã], TableNumber AS [Số Bàn], Status AS [Trạng Thái] FROM DiningTable";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvTables.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bàn: " + ex.Message);
            }
        }

        private void LoadCategoryData()
        {
            try
            {
                string query = "SELECT CategoryID AS [Mã DM], CategoryName AS [Tên Danh Mục] FROM Category WHERE IsActive = 1";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvCategories.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh mục: " + ex.Message); }
        }

        private void SwitchPage(Panel targetPage, Guna2Button targetButton)
        {
            pageTables.Visible = false;
            pageCategories.Visible = false;
            pageInfo.Visible = false;

            targetPage.Visible = true;
            targetPage.BringToFront();

            btnMenuTable.Checked = false;
            btnMenuCategory.Checked = false;
            btnMenuInfo.Checked = false;

            targetButton.Checked = true;
        }

        #endregion

        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN (EVENTS)
        // ========================================================

        private void SettingForm_Load(object sender, EventArgs e)
        {
            LoadTableData();
            LoadCategoryData();
        }

        // --- SỰ KIỆN CHUYỂN TAB ---
        private void BtnMenuTable_Click(object sender, EventArgs e)
        {
            SwitchPage(pageTables, btnMenuTable);
        }

        private void BtnMenuCategory_Click(object sender, EventArgs e)
        {
            SwitchPage(pageCategories, btnMenuCategory);
        }

        private void BtnMenuInfo_Click(object sender, EventArgs e)
        {
            SwitchPage(pageInfo, btnMenuInfo);
        }

        // --- SỰ KIỆN TAB BÀN ĂN ---
        private void DgvTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTables.Rows[e.RowIndex];
                txtTableID.Text = row.Cells["Mã"].Value.ToString();
                txtTableName.Text = row.Cells["Số Bàn"].Value.ToString();
                cboTableStatus.Text = row.Cells["Trạng Thái"].Value.ToString();
            }
        }

        private void BtnAddTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show("Vui lòng nhập số bàn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtTableName.Text, out int tableNum))
            {
                MessageBox.Show("Số bàn chỉ được phép nhập số (Ví dụ: 1, 2, 3...), không được nhập chữ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query = $"INSERT INTO DiningTable (TableNumber, Status) VALUES ({tableNum}, N'{cboTableStatus.Text}')";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Thêm bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTableName.Text = "";
                LoadTableData();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi thêm: " + ex.Message); }
        }

        private void BtnEditTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableID.Text))
            {
                MessageBox.Show("Vui lòng nhấp chọn 1 bàn trên bảng để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtTableName.Text, out int tableNum))
            {
                MessageBox.Show("Số bàn chỉ được phép nhập số (Ví dụ: 1, 2, 3...), không được nhập chữ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query = $"UPDATE DiningTable SET TableNumber = {tableNum}, Status = N'{cboTableStatus.Text}' WHERE TableID = {txtTableID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTableName.Text = "";
                txtTableID.Text = "";
                LoadTableData();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi sửa: " + ex.Message); }
        }

        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableID.Text))
            {
                MessageBox.Show("Vui lòng chọn 1 bàn trên bảng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string query = $"DELETE FROM DiningTable WHERE TableID = {txtTableID.Text}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTableName.Text = "";
                    txtTableID.Text = "";
                    LoadTableData();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi khi xóa: " + ex.Message); }
            }
        }

        // --- SỰ KIỆN TAB DANH MỤC ---
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCategories.Rows[e.RowIndex];
                txtCategoryID.Text = row.Cells["Mã DM"].Value.ToString();
                txtCategoryName.Text = row.Cells["Tên Danh Mục"].Value.ToString();
            }
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string query = $"INSERT INTO Category (CategoryName, IsActive) VALUES (N'{txtCategoryName.Text}', 1)";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCategoryName.Text = "";
                LoadCategoryData();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi thêm: " + ex.Message); }
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            {
                MessageBox.Show("Vui lòng nhấp chọn 1 danh mục trên bảng để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string query = $"UPDATE Category SET CategoryName = N'{txtCategoryName.Text}' WHERE CategoryID = {txtCategoryID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCategoryName.Text = "";
                txtCategoryID.Text = "";
                LoadCategoryData();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi sửa: " + ex.Message); }
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            {
                MessageBox.Show("Vui lòng nhấp chọn 1 danh mục trên bảng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string query = $"UPDATE Category SET IsActive = 0 WHERE CategoryID = {txtCategoryID.Text}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCategoryName.Text = "";
                    txtCategoryID.Text = "";
                    LoadCategoryData();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi khi xóa: " + ex.Message); }
            }
        }

        #endregion
    }
}