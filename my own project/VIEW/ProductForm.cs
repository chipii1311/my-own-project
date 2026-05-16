using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ProductForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private Guna2TextBox txtID, txtName, txtPrice;
        private Guna2PictureBox picFood;
        private Guna2Button btnBrowse, btnEdit, btnDelete;
        private Guna2Button btnAddNewProduct;

        private Guna2ComboBox cboFilterCategory;
        private Guna2ComboBox cboInputCategory;
        private Guna2ComboBox cboInputStatus;

        private FlowLayoutPanel flpProducts;

        private string currentImagePath = "";
        private string imageFolder = Path.Combine(Application.StartupPath, "MenuImages");

        // 👉 THÊM BIẾN LƯU QUYỀN
        private string currentUserRole;

        // 👉 SỬA HÀM TẠO ĐỂ NHẬN QUYỀN TRUYỀN VÀO TỪ MAIN FORM
        public ProductForm(string role)
        {
            InitializeComponent();
            this.Controls.Clear();

            // Lưu lại quyền để sử dụng
            this.currentUserRole = role;

            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            InitializeModernUI();

            this.Load += ProductForm_Load;
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void InitializeModernUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            // --- CỘT BÊN PHẢI (SỬA VÀ XÓA) ---
            Guna2Panel pnlRight = new Guna2Panel();
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Width = 380;
            pnlRight.FillColor = Color.White;
            pnlRight.CustomBorderThickness = new Padding(1, 0, 0, 0);
            pnlRight.CustomBorderColor = Color.LightGray;
            this.Controls.Add(pnlRight);

            FlowLayoutPanel flpInput = new FlowLayoutPanel();
            flpInput.Dock = DockStyle.Fill;
            flpInput.FlowDirection = FlowDirection.TopDown;
            flpInput.WrapContents = false;
            flpInput.AutoScroll = true;
            flpInput.Padding = new Padding(25, 50, 25, 40);
            pnlRight.Controls.Add(flpInput);

            int cWidth = 320;
            int cCenter = (cWidth - 150) / 2;

            Label lblRightTitle = new Label { Text = "THÔNG TIN CHI TIẾT", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), AutoSize = false, Size = new Size(cWidth, 40), TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(0, 0, 0, 20) };
            flpInput.Controls.Add(lblRightTitle);

            picFood = new Guna2PictureBox { Size = new Size(150, 140), BorderRadius = 10, SizeMode = PictureBoxSizeMode.Zoom, FillColor = Color.FromArgb(240, 240, 240), Margin = new Padding(cCenter, 0, 0, 15) };
            flpInput.Controls.Add(picFood);

            btnBrowse = new Guna2Button { Text = "Đổi ảnh khác", Size = new Size(150, 35), BorderRadius = 8, FillColor = Color.Gray, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(cCenter, 0, 0, 25) };
            btnBrowse.Click += BtnBrowse_Click;
            flpInput.Controls.Add(btnBrowse);

            Label lblCatInput = new Label { Text = "Danh mục:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            flpInput.Controls.Add(lblCatInput);

            cboInputCategory = new Guna2ComboBox { Size = new Size(cWidth, 40), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 15) };
            flpInput.Controls.Add(cboInputCategory);

            Label lblName = new Label { Text = "Tên món ăn:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            flpInput.Controls.Add(lblName);

            txtName = new Guna2TextBox { Size = new Size(cWidth, 42), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), ForeColor = Color.Black, BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 15) };
            flpInput.Controls.Add(txtName);

            Label lblPrice = new Label { Text = "Giá bán (VNĐ):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            flpInput.Controls.Add(lblPrice);

            txtPrice = new Guna2TextBox { Size = new Size(cWidth, 42), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), ForeColor = Color.Black, BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 15) };
            flpInput.Controls.Add(txtPrice);

            // --- CHỌN TRẠNG THÁI (ĐANG BÁN / HẾT MÓN) ---
            Label lblStatus = new Label { Text = "Trạng thái:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            flpInput.Controls.Add(lblStatus);

            cboInputStatus = new Guna2ComboBox { Size = new Size(cWidth, 40), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 25) };
            cboInputStatus.Items.AddRange(new object[] { "Còn", "Hết" });
            flpInput.Controls.Add(cboInputStatus);

            txtID = new Guna2TextBox { Visible = false, Size = new Size(0, 0) };
            flpInput.Controls.Add(txtID);

            TableLayoutPanel tlpAction = new TableLayoutPanel { Size = new Size(cWidth, 45), ColumnCount = 2, Margin = new Padding(0, 0, 0, 15) };
            tlpAction.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAction.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnEdit = new Guna2Button { Text = "CẬP NHẬT", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 5, 0), BorderRadius = 5, FillColor = Color.FromArgb(52, 152, 219), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnEdit.Click += BtnEdit_Click;
            tlpAction.Controls.Add(btnEdit, 0, 0);

            btnDelete = new Guna2Button { Text = "XÓA MÓN", Dock = DockStyle.Fill, Margin = new Padding(5, 0, 0, 0), BorderRadius = 5, FillColor = Color.FromArgb(255, 107, 129), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnDelete.Click += BtnDelete_Click;
            tlpAction.Controls.Add(btnDelete, 1, 0);

            flpInput.Controls.Add(tlpAction);

            // --- BÊN TRÁI - THANH TOP BAR ---
            Guna2Panel pnlCenter = new Guna2Panel();
            pnlCenter.Dock = DockStyle.Fill;
            this.Controls.Add(pnlCenter);

            TableLayoutPanel tlpLeft = new TableLayoutPanel();
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.ColumnCount = 1;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.RowCount = 2;
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlCenter.Controls.Add(tlpLeft);

            Guna2Panel pnlTopCenter = new Guna2Panel();
            pnlTopCenter.Dock = DockStyle.Fill;
            pnlTopCenter.FillColor = Color.Transparent;
            pnlTopCenter.Margin = new Padding(0);
            tlpLeft.Controls.Add(pnlTopCenter, 0, 0);

            Label lblTitle = new Label { Text = "QUẢN LÝ THỰC ĐƠN", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(30, 45), AutoSize = true };
            pnlTopCenter.Controls.Add(lblTitle);

            Label lblFilter = new Label { Text = "Lọc theo:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(340, 55), AutoSize = true };
            pnlTopCenter.Controls.Add(lblFilter);

            cboFilterCategory = new Guna2ComboBox { Location = new Point(420, 45), Size = new Size(200, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F), FillColor = Color.White, BorderColor = Color.LightGray };
            cboFilterCategory.SelectedIndexChanged += CboFilterCategory_SelectedIndexChanged;
            pnlTopCenter.Controls.Add(cboFilterCategory);

            btnAddNewProduct = new Guna2Button { Text = "➕ THÊM MÓN MỚI", Location = new Point(650, 45), Size = new Size(180, 36), BorderRadius = 5, FillColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAddNewProduct.Click += BtnAddNewProduct_Click;
            pnlTopCenter.Controls.Add(btnAddNewProduct);

            // --- Danh sách thẻ món ăn ---
            flpProducts = new FlowLayoutPanel();
            flpProducts.Dock = DockStyle.Fill;
            flpProducts.AutoScroll = true;
            flpProducts.Padding = new Padding(25, 15, 20, 20);
            flpProducts.Margin = new Padding(0);
            tlpLeft.Controls.Add(flpProducts, 0, 1);
        }

        #endregion

        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC DATABASE
        // ========================================================

        // 👉 HÀM MỚI: PHÂN QUYỀN TRÊN GIAO DIỆN
        private void ApplyRolePermissions()
        {
            if (currentUserRole == "Nhân viên")
            {
                // 1. Khóa TextBox Tên món và Giá
                txtName.ReadOnly = true;
                txtPrice.ReadOnly = true;

                // Đổi màu nền xám nhạt để báo hiệu ReadOnly
                txtName.FillColor = Color.FromArgb(243, 244, 246);
                txtPrice.FillColor = Color.FromArgb(243, 244, 246);

                // 2. Khóa Combobox Danh mục
                cboInputCategory.Enabled = false;

                // 3. Ẩn các nút thêm/xóa/đổi ảnh
                btnAddNewProduct.Visible = false;
                btnDelete.Visible = false;
                btnBrowse.Visible = false;

                // Nút "LƯU CẬP NHẬT" và Combobox "Trạng thái" vẫn được giữ lại để thao tác
            }
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT CategoryID, CategoryName FROM Category WHERE IsActive = 1";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                cboInputCategory.DataSource = dt;
                cboInputCategory.DisplayMember = "CategoryName";
                cboInputCategory.ValueMember = "CategoryID";

                DataTable dtFilter = dt.Copy();
                DataRow row = dtFilter.NewRow();
                row["CategoryID"] = 0;
                row["CategoryName"] = "-- Tất cả món ăn --";
                dtFilter.Rows.InsertAt(row, 0);

                cboFilterCategory.DataSource = dtFilter;
                cboFilterCategory.DisplayMember = "CategoryName";
                cboFilterCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        private void LoadProductData()
        {
            try
            {
                flpProducts.Controls.Clear();

                int filterCatID = 0;
                if (cboFilterCategory.SelectedValue != null && int.TryParse(cboFilterCategory.SelectedValue.ToString(), out int id))
                {
                    filterCatID = id;
                }

                string query = "SELECT MenuItemID AS [Mã món], CategoryID, ItemName AS [Tên món], Price AS [Giá bán], ISNULL(ImageUrl, '') AS [Ảnh], ISNULL(Status, N'Còn') AS [Trạng thái] FROM MenuItem WHERE ItemStatus = 1";

                if (filterCatID > 0)
                    query += $" AND CategoryID = {filterCatID}";

                query += " ORDER BY MenuItemID DESC";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                foreach (DataRow row in dt.Rows)
                {
                    Guna2Panel card = new Guna2Panel();
                    card.Size = new Size(180, 240);
                    card.BorderRadius = 15;
                    card.FillColor = Color.White;
                    card.BorderThickness = 1;
                    card.BorderColor = Color.FromArgb(220, 220, 220);
                    card.Margin = new Padding(10, 10, 15, 15);
                    card.Cursor = Cursors.Hand;
                    card.Tag = row;

                    // --- TRANG TRÍ: NHÃN BÁO HẾT MÓN ---
                    string status = row["Trạng thái"].ToString();
                    if (status == "Hết")
                    {
                        Label lblOut = new Label();
                        lblOut.Text = "HẾT MÓN";
                        lblOut.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        lblOut.ForeColor = Color.White;
                        lblOut.BackColor = Color.FromArgb(255, 71, 87);
                        lblOut.AutoSize = true;
                        lblOut.Location = new Point(10, 10);
                        lblOut.Padding = new Padding(3);
                        card.Controls.Add(lblOut);
                        lblOut.BringToFront();

                        card.FillColor = Color.FromArgb(245, 245, 245);
                    }

                    Guna2PictureBox pic = new Guna2PictureBox();
                    pic.Location = new Point(15, 15);
                    pic.Size = new Size(150, 130);
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.BackColor = Color.Transparent;
                    pic.UseTransparentBackground = true;

                    string imgName = row["Ảnh"].ToString();
                    string imgPath = Path.Combine(imageFolder, imgName);
                    try
                    {
                        if (File.Exists(imgPath))
                        {
                            using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                            {
                                pic.Image = Image.FromStream(fs);
                            }
                        }
                    }
                    catch { }

                    Label lblName = new Label();
                    lblName.Text = row["Tên món"].ToString();
                    lblName.Location = new Point(10, 150);
                    lblName.Size = new Size(160, 45);
                    lblName.TextAlign = ContentAlignment.TopCenter;
                    lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    lblName.ForeColor = (status == "Hết") ? Color.Gray : Color.FromArgb(64, 64, 64);
                    lblName.BackColor = Color.Transparent;

                    Label lblPrice = new Label();
                    decimal price = Convert.ToDecimal(row["Giá bán"]);
                    lblPrice.Text = price.ToString("N0") + " đ";
                    lblPrice.Location = new Point(10, 195);
                    lblPrice.Size = new Size(160, 30);
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.ForeColor = (status == "Hết") ? Color.Gray : Color.FromArgb(46, 204, 113);
                    lblPrice.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                    lblPrice.BackColor = Color.Transparent;

                    card.Controls.Add(pic);
                    card.Controls.Add(lblName);
                    card.Controls.Add(lblPrice);

                    EventHandler clickEvent = (s, e) => { Card_Click(row); };
                    card.Click += clickEvent;
                    pic.Click += clickEvent;
                    lblName.Click += clickEvent;
                    lblPrice.Click += clickEvent;

                    if (card.Controls.Count > 3) card.Controls[0].Click += clickEvent;

                    flpProducts.Controls.Add(card);
                }

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi tải Menu: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            picFood.Image = null;
            currentImagePath = "";
            if (cboInputCategory.Items.Count > 0) cboInputCategory.SelectedIndex = 0;
            if (cboInputStatus.Items.Count > 0) cboInputStatus.SelectedIndex = 0;
        }

        #endregion

        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN (EVENTS)
        // ========================================================

        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProductData();

            // 👉 GỌI HÀM KHÓA QUYỀN KHI LOAD FORM
            ApplyRolePermissions();
        }

        private void CboFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProductData();
        }

        private void Card_Click(DataRow row)
        {
            txtID.Text = row["Mã món"].ToString();
            txtName.Text = row["Tên món"].ToString();

            decimal price = Convert.ToDecimal(row["Giá bán"]);
            txtPrice.Text = Math.Round(price).ToString();

            if (row["CategoryID"] != DBNull.Value)
            {
                cboInputCategory.SelectedValue = row["CategoryID"];
            }

            string status = row["Trạng thái"].ToString();
            cboInputStatus.Text = (status == "Hết") ? "Hết" : "Còn";

            string imgName = row["Ảnh"].ToString();
            currentImagePath = Path.Combine(imageFolder, imgName);
            try
            {
                if (File.Exists(currentImagePath))
                {
                    using (FileStream fs = new FileStream(currentImagePath, FileMode.Open, FileAccess.Read))
                    {
                        picFood.Image = Image.FromStream(fs);
                    }
                }
                else picFood.Image = null;
            }
            catch { picFood.Image = null; }
        }

        private void BtnAddNewProduct_Click(object sender, EventArgs e)
        {
            using (my_own_project.VIEW.ProductAddForm addForm = new my_own_project.VIEW.ProductAddForm())
            {
                Form blackBackground = new Form();
                blackBackground.StartPosition = FormStartPosition.Manual;
                blackBackground.FormBorderStyle = FormBorderStyle.None;
                blackBackground.Opacity = 0.5d;
                blackBackground.BackColor = Color.Black;
                blackBackground.Size = this.Size;

                try { blackBackground.Location = this.Parent.PointToScreen(this.Location); }
                catch { blackBackground.Location = this.PointToScreen(Point.Empty); }

                blackBackground.Show();

                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProductData();
                }

                blackBackground.Dispose();
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                currentImagePath = open.FileName;
                picFood.Image = new Bitmap(open.FileName);
            }
        }

        // 👉 SỬA LẠI SỰ KIỆN CẬP NHẬT ĐỂ ÁP DỤNG LUỒNG SQL THEO QUYỀN
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng click chọn 1 món ăn từ danh sách để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn lưu các thay đổi cho món '{txtName.Text}' không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;

            // --- LƯU ẢNH (Chỉ Quản lý mới xử lý ảnh) ---
            string savedImageFileName = "";
            if (currentUserRole != "Nhân viên" && currentUserRole != "User")
            {
                if (!string.IsNullOrEmpty(currentImagePath) && System.IO.File.Exists(currentImagePath) && !currentImagePath.Contains(imageFolder))
                {
                    savedImageFileName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + System.IO.Path.GetExtension(currentImagePath);
                    string destPath = System.IO.Path.Combine(imageFolder, savedImageFileName);
                    System.IO.File.Copy(currentImagePath, destPath, true);
                }
            }

            // --- GỌI BLL XỬ LÝ VÀ BẮT LỖI BẰNG TRY...CATCH ---
            try
            {
                int id = Convert.ToInt32(txtID.Text);
                int catID = Convert.ToInt32(cboInputCategory.SelectedValue);

                // Gọi hàm BLL mới tạo
                bool isDone = my_own_project.BLL.MenuItemBLL.UpdateProductWithRole(
                    currentUserRole, id, txtPrice.Text, catID, txtName.Text, cboInputStatus.Text, savedImageFileName
                );

                if (isDone)
                {
                    var checkDB = my_own_project.BLL.MenuItemBLL.GetMenuItemByID(id);
                   

                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                }
            }
            catch (Exception ex)
            {
                // Hứng trọn mọi lỗi do hàm ValidateMenuItem hoặc do sai giá tiền ném ra!
                MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Trợ giúp UX: Đưa con trỏ chuột về ô Giá nếu lỗi liên quan đến giá
                if (ex.Message.Contains("Giá")) txtPrice.Focus();
                else if (ex.Message.Contains("Tên")) txtName.Focus();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng click chọn 1 món ăn từ danh sách để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa món '{txtName.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string query = $"UPDATE MenuItem SET ItemStatus = 0 WHERE MenuItemID = {txtID.Text}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(query);

                    MessageBox.Show("Đã xóa món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }

        #endregion
    }
}