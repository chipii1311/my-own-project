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
        private Guna2TextBox txtID, txtName, txtPrice;
        private Guna2PictureBox picFood;
        private Guna2Button btnBrowse, btnAdd, btnEdit, btnDelete, btnClear;

        private Guna2ComboBox cboFilterCategory;
        private Guna2ComboBox cboInputCategory;

        private FlowLayoutPanel flpProducts;

        private string currentImagePath = "";
        private string imageFolder = Path.Combine(Application.StartupPath, "MenuImages");

        public ProductForm()
        {
            InitializeComponent();
            this.Controls.Clear();

            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            InitializeModernUI();

            this.Load += (s, e) => {
                LoadCategories();
                LoadProductData();
            };
        }

        private void InitializeModernUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            // ==========================================
            // CỘT BÊN PHẢI (THUẬT TOÁN TÍNH TỌA ĐỘ ĐỘNG CHỐNG ĐÈ 100%)
            // ==========================================
            Guna2Panel pnlRight = new Guna2Panel();
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Width = 380;
            pnlRight.FillColor = Color.White;
            pnlRight.CustomBorderThickness = new Padding(1, 0, 0, 0);
            pnlRight.CustomBorderColor = Color.LightGray;
            this.Controls.Add(pnlRight);

            // Bọc bằng Panel cuộn để chống tràn màn hình
            Panel pnlRightScroll = new Panel();
            pnlRightScroll.Dock = DockStyle.Fill;
            pnlRightScroll.AutoScroll = true;
            pnlRight.Controls.Add(pnlRightScroll);

            // Bắt đầu tính toán Y từ trên xuống dưới
            int currentY = 30;
            int leftMargin = 30;
            int controlWidth = 320;

            // 1. Tiêu đề
            Label lblRightTitle = new Label { Text = "THÔNG TIN CHI TIẾT", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), AutoSize = true };
            lblRightTitle.Location = new Point((380 - lblRightTitle.PreferredWidth) / 2, currentY); // Căn giữa
            pnlRightScroll.Controls.Add(lblRightTitle);
            currentY += lblRightTitle.PreferredHeight + 25; // Lấy chiều cao thực tế + khoảng cách 25px

            // 2. Ảnh
            picFood = new Guna2PictureBox { Size = new Size(160, 160), BorderRadius = 10, SizeMode = PictureBoxSizeMode.Zoom, FillColor = Color.FromArgb(240, 240, 240) };
            picFood.Location = new Point((380 - 160) / 2, currentY); // Căn giữa ảnh
            pnlRightScroll.Controls.Add(picFood);
            currentY += picFood.Height + 15;

            // 3. Nút Chọn Ảnh
            btnBrowse = new Guna2Button { Text = "Chọn ảnh mới", Size = new Size(160, 35), BorderRadius = 8, FillColor = Color.Gray, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnBrowse.Location = new Point((380 - 160) / 2, currentY);
            btnBrowse.Click += BtnBrowse_Click;
            pnlRightScroll.Controls.Add(btnBrowse);
            currentY += btnBrowse.Height + 25;

            // 4. Nhãn Danh mục
            Label lblCatInput = new Label { Text = "Danh mục:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true };
            lblCatInput.Location = new Point(leftMargin, currentY);
            pnlRightScroll.Controls.Add(lblCatInput);
            currentY += lblCatInput.PreferredHeight + 5;

            // 5. Cbo Danh mục
            cboInputCategory = new Guna2ComboBox { Size = new Size(controlWidth, 36), BorderRadius = 5, Font = new Font("Segoe UI", 11F), BorderColor = Color.LightGray };
            cboInputCategory.Location = new Point(leftMargin, currentY);
            pnlRightScroll.Controls.Add(cboInputCategory);
            currentY += cboInputCategory.Height + 15;

            // 6. Nhãn Tên món
            Label lblName = new Label { Text = "Tên món ăn:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true };
            lblName.Location = new Point(leftMargin, currentY);
            pnlRightScroll.Controls.Add(lblName);
            currentY += lblName.PreferredHeight + 5;

            // 7. Textbox Tên món (Fix lỗi tàng hình: Thêm BorderColor và BorderThickness)
            txtName = new Guna2TextBox { Size = new Size(controlWidth, 40), BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "VD: Cơm chiên hải sản", BorderThickness = 1, BorderColor = Color.Gray, ForeColor = Color.Black };
            txtName.Location = new Point(leftMargin, currentY);
            pnlRightScroll.Controls.Add(txtName);
            currentY += txtName.Height + 15;

            // 8. Nhãn Giá
            Label lblPrice = new Label { Text = "Giá bán (VNĐ):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true };
            lblPrice.Location = new Point(leftMargin, currentY);
            pnlRightScroll.Controls.Add(lblPrice);
            currentY += lblPrice.PreferredHeight + 5;

            // 9. Textbox Giá
            txtPrice = new Guna2TextBox { Size = new Size(controlWidth, 40), BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "VD: 45000", BorderThickness = 1, BorderColor = Color.Gray, ForeColor = Color.Black };
            txtPrice.Location = new Point(leftMargin, currentY);
            pnlRightScroll.Controls.Add(txtPrice);
            currentY += txtPrice.Height + 25;

            // Textbox ID Ẩn
            txtID = new Guna2TextBox { Visible = false };
            pnlRightScroll.Controls.Add(txtID);

            // 10. Cụm 3 nút Thêm / Sửa / Xóa
            btnAdd = new Guna2Button { Text = "THÊM", Size = new Size(95, 45), BorderRadius = 5, FillColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAdd.Location = new Point(leftMargin, currentY);
            btnAdd.Click += BtnAdd_Click;
            pnlRightScroll.Controls.Add(btnAdd);

            btnEdit = new Guna2Button { Text = "SỬA", Size = new Size(100, 45), BorderRadius = 5, FillColor = Color.FromArgb(52, 152, 219), Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnEdit.Location = new Point(leftMargin + 105, currentY);
            btnEdit.Click += BtnEdit_Click;
            pnlRightScroll.Controls.Add(btnEdit);

            btnDelete = new Guna2Button { Text = "XÓA", Size = new Size(105, 45), BorderRadius = 5, FillColor = Color.FromArgb(231, 76, 60), Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnDelete.Location = new Point(leftMargin + 215, currentY);
            btnDelete.Click += BtnDelete_Click;
            pnlRightScroll.Controls.Add(btnDelete);
            currentY += btnAdd.Height + 15;

            // 11. Nút dọn dẹp form
            btnClear = new Guna2Button { Text = "Tạo món mới (Dọn sạch Form)", Size = new Size(controlWidth, 40), BorderRadius = 5, FillColor = Color.FromArgb(240, 240, 240), ForeColor = Color.Black, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnClear.Location = new Point(leftMargin, currentY);
            btnClear.Click += (s, e) => { ClearInputs(); };
            pnlRightScroll.Controls.Add(btnClear);
            // Cuối cùng: Thêm khoảng trống padding bottom
            currentY += btnClear.Height + 30;
            Label lblSpacer = new Label { Size = new Size(10, 10), Location = new Point(0, currentY) };
            pnlRightScroll.Controls.Add(lblSpacer);


            // ==========================================
            // BÊN TRÁI - DANH SÁCH & THANH LỌC CATEGORY (GIỮ NGUYÊN)
            // ==========================================
            Guna2Panel pnlCenter = new Guna2Panel();
            pnlCenter.Dock = DockStyle.Fill;
            this.Controls.Add(pnlCenter);

            Guna2Panel pnlTopCenter = new Guna2Panel();
            pnlTopCenter.Dock = DockStyle.Top;
            pnlTopCenter.Height = 80;
            pnlTopCenter.FillColor = Color.Transparent;
            pnlCenter.Controls.Add(pnlTopCenter);

            Label lblTitle = new Label { Text = "QUẢN LÝ THỰC ĐƠN", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(30, 25), AutoSize = true };
            pnlTopCenter.Controls.Add(lblTitle);

            Label lblFilter = new Label { Text = "Lọc theo:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(350, 35), AutoSize = true };
            pnlTopCenter.Controls.Add(lblFilter);

            cboFilterCategory = new Guna2ComboBox
            {
                Location = new Point(430, 25),
                Size = new Size(200, 36),
                BorderRadius = 5,
                Font = new Font("Segoe UI", 10F),
                FillColor = Color.White,
                BorderColor = Color.LightGray
            };
            cboFilterCategory.SelectedIndexChanged += (s, e) => { LoadProductData(); };
            pnlTopCenter.Controls.Add(cboFilterCategory);

            flpProducts = new FlowLayoutPanel();
            flpProducts.Dock = DockStyle.Fill;
            flpProducts.AutoScroll = true;
            flpProducts.Padding = new Padding(25, 10, 20, 20);
            pnlCenter.Controls.Add(flpProducts);

            pnlCenter.BringToFront();
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

                string query = "SELECT MenuItemID AS [Mã món], CategoryID, ItemName AS [Tên món], Price AS [Giá bán], ISNULL(ImageUrl, '') AS [Ảnh] FROM MenuItem WHERE ItemStatus = 1";

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
                    lblName.ForeColor = Color.FromArgb(64, 64, 64);
                    lblName.BackColor = Color.Transparent;

                    Label lblPrice = new Label();
                    decimal price = Convert.ToDecimal(row["Giá bán"]);
                    lblPrice.Text = price.ToString("N0") + " đ";
                    lblPrice.Location = new Point(10, 195);
                    lblPrice.Size = new Size(160, 30);
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.ForeColor = Color.FromArgb(46, 204, 113);
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

                    flpProducts.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi tải Menu: " + ex.Message);
            }
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và giá món ăn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int catID = Convert.ToInt32(cboInputCategory.SelectedValue);
                string fileName = "";
                if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath) && !currentImagePath.Contains(imageFolder))
                {
                    fileName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(currentImagePath);
                    string destPath = Path.Combine(imageFolder, fileName);
                    File.Copy(currentImagePath, destPath, true);
                }

                string query = $"INSERT INTO MenuItem (CategoryID, ItemName, Price, Status, ImageUrl, ItemStatus, CreatedAt) " +
                               $"VALUES ({catID}, N'{txtName.Text}', {txtPrice.Text}, N'Còn', '{fileName}', 1, GETDATE())";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Thêm món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputs();
                LoadProductData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng nhấp vào 1 món ăn để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int catID = Convert.ToInt32(cboInputCategory.SelectedValue);
                string fileNameQuery = "";
                if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath) && !currentImagePath.Contains(imageFolder))
                {
                    string fileName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(currentImagePath);
                    string destPath = Path.Combine(imageFolder, fileName);
                    File.Copy(currentImagePath, destPath, true);
                    fileNameQuery = $", ImageUrl = '{fileName}'";
                }

                string query = $"UPDATE MenuItem SET CategoryID = {catID}, ItemName = N'{txtName.Text}', Price = {txtPrice.Text} {fileNameQuery} " +
                               $"WHERE MenuItemID = {txtID.Text}";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputs();
                LoadProductData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Vui lòng nhấp vào 1 món ăn để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa món này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string query = $"UPDATE MenuItem SET ItemStatus = 0 WHERE MenuItemID = {txtID.Text}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(query);

                    MessageBox.Show("Đã xóa món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadProductData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
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
        }
    }
}