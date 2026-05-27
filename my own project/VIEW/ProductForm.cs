using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace my_own_project.VIEW
{
    public partial class ProductForm : Form
    {
        private string currentImagePath = "";
        private string imageFolder = Path.Combine(Application.StartupPath, "MenuImages");
        private string currentUserRole;

        public ProductForm(string role)
        {
            InitializeComponent();

            // Lưu lại quyền để sử dụng
            this.currentUserRole = role;

            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            // Gọi hàm dựng giao diện (được định nghĩa bên Designer.cs)
            BuildUI();

            this.Load += ProductForm_Load;
        }

        // ========================================================
        // 1. PHÂN QUYỀN & DATA BINDING
        // ========================================================

        private void ApplyRolePermissions()
        {
            if (currentUserRole == "Nhân viên")
            {
                txtName.ReadOnly = true;
                txtPrice.ReadOnly = true;
                txtName.FillColor = Color.FromArgb(243, 244, 246);
                txtPrice.FillColor = Color.FromArgb(243, 244, 246);
                cboInputCategory.Enabled = false;

                btnAddNewProduct.Visible = false;
                btnDelete.Visible = false;
                btnBrowse.Visible = false;
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
                if (filterCatID > 0) query += $" AND CategoryID = {filterCatID}";
                query += " ORDER BY MenuItemID DESC";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                foreach (DataRow row in dt.Rows)
                {
                    // Xây dựng các Card Món ăn động (Dynamic UI)
                    Guna2Panel card = new Guna2Panel
                    {
                        Size = new Size(180, 240),
                        BorderRadius = 15,
                        FillColor = Color.White,
                        BorderThickness = 1,
                        BorderColor = Color.FromArgb(220, 220, 220),
                        Margin = new Padding(10, 10, 15, 15),
                        Cursor = Cursors.Hand,
                        Tag = row
                    };

                    string status = row["Trạng thái"].ToString();
                    if (status == "Hết")
                    {
                        Label lblOut = new Label { Text = "HẾT MÓN", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(255, 71, 87), AutoSize = true, Location = new Point(10, 10), Padding = new Padding(3) };
                        card.Controls.Add(lblOut);
                        lblOut.BringToFront();
                        card.FillColor = Color.FromArgb(245, 245, 245);
                    }

                    Guna2PictureBox pic = new Guna2PictureBox { Location = new Point(15, 15), Size = new Size(150, 130), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent, UseTransparentBackground = true };
                    string imgName = row["Ảnh"].ToString();
                    string imgPath = Path.Combine(imageFolder, imgName);
                    try { if (File.Exists(imgPath)) { using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read)) { pic.Image = Image.FromStream(fs); } } } catch { }

                    Label lblName = new Label { Text = row["Tên món"].ToString(), Location = new Point(10, 150), Size = new Size(160, 45), TextAlign = ContentAlignment.TopCenter, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = (status == "Hết") ? Color.Gray : Color.FromArgb(64, 64, 64), BackColor = Color.Transparent };

                    decimal price = Convert.ToDecimal(row["Giá bán"]);
                    Label lblPrice = new Label { Text = price.ToString("N0") + " đ", Location = new Point(10, 195), Size = new Size(160, 30), TextAlign = ContentAlignment.MiddleCenter, ForeColor = (status == "Hết") ? Color.Gray : Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 12F, FontStyle.Bold), BackColor = Color.Transparent };

                    card.Controls.Add(pic); card.Controls.Add(lblName); card.Controls.Add(lblPrice);

                    EventHandler clickEvent = (s, e) => { Card_Click(row); };
                    card.Click += clickEvent; pic.Click += clickEvent; lblName.Click += clickEvent; lblPrice.Click += clickEvent;
                    if (card.Controls.Count > 3) card.Controls[0].Click += clickEvent;

                    flpProducts.Controls.Add(card);
                }
                ClearInputs();
            }
            catch (Exception ex) { MessageBox.Show("Có lỗi khi tải Menu: " + ex.Message); }
        }

        private void ClearInputs()
        {
            txtID.Text = ""; txtName.Text = ""; txtPrice.Text = "";
            picFood.Image = null; currentImagePath = "";
            if (cboInputCategory.Items.Count > 0) cboInputCategory.SelectedIndex = 0;
            if (cboInputStatus.Items.Count > 0) cboInputStatus.SelectedIndex = 0;
        }

        // ========================================================
        // 2. SỰ KIỆN (EVENTS)
        // ========================================================

        private void ProductForm_Load(object sender, EventArgs e) { LoadCategories(); LoadProductData(); ApplyRolePermissions(); }
        public void CboFilterCategory_SelectedIndexChanged(object sender, EventArgs e) => LoadProductData();

        private void Card_Click(DataRow row)
        {
            txtID.Text = row["Mã món"].ToString();
            txtName.Text = row["Tên món"].ToString();
            txtPrice.Text = Math.Round(Convert.ToDecimal(row["Giá bán"])).ToString();
            if (row["CategoryID"] != DBNull.Value) cboInputCategory.SelectedValue = row["CategoryID"];
            cboInputStatus.Text = (row["Trạng thái"].ToString() == "Hết") ? "Hết" : "Còn";

            currentImagePath = Path.Combine(imageFolder, row["Ảnh"].ToString());
            try { if (File.Exists(currentImagePath)) { using (FileStream fs = new FileStream(currentImagePath, FileMode.Open, FileAccess.Read)) { picFood.Image = Image.FromStream(fs); } } else picFood.Image = null; }
            catch { picFood.Image = null; }
        }

        public void BtnAddNewProduct_Click(object sender, EventArgs e)
        {
            using (my_own_project.VIEW.ProductAddForm addForm = new my_own_project.VIEW.ProductAddForm())
            {
                Form blackBg = new Form { StartPosition = FormStartPosition.Manual, FormBorderStyle = FormBorderStyle.None, Opacity = 0.5d, BackColor = Color.Black, Size = this.Size };
                try { blackBg.Location = this.Parent.PointToScreen(this.Location); } catch { blackBg.Location = this.PointToScreen(Point.Empty); }
                blackBg.Show();

                if (addForm.ShowDialog() == DialogResult.OK) LoadProductData();
                blackBg.Dispose();
            }
        }

        public void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog { Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp" };
            if (open.ShowDialog() == DialogResult.OK) { currentImagePath = open.FileName; picFood.Image = new Bitmap(open.FileName); }
        }

        public void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text)) { MessageBox.Show("Vui lòng click chọn 1 món ăn từ danh sách để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show($"Bạn có chắc chắn muốn lưu các thay đổi cho món '{txtName.Text}' không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            string savedImageFileName = "";
            if (currentUserRole != "Nhân viên" && currentUserRole != "User")
            {
                if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath) && !currentImagePath.Contains(imageFolder))
                {
                    savedImageFileName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(currentImagePath);
                    File.Copy(currentImagePath, Path.Combine(imageFolder, savedImageFileName), true);
                }
            }

            try
            {
                int id = Convert.ToInt32(txtID.Text);
                int catID = Convert.ToInt32(cboInputCategory.SelectedValue);
                bool isDone = my_own_project.BLL.MenuItemBLL.UpdateProductWithRole(currentUserRole, id, txtPrice.Text, catID, txtName.Text, cboInputStatus.Text, savedImageFileName);

                if (isDone)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Message.Contains("Giá")) txtPrice.Focus(); else if (ex.Message.Contains("Tên")) txtName.Focus();
            }
        }

        public void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text)) { MessageBox.Show("Vui lòng click chọn 1 món ăn từ danh sách để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa món '{txtName.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE MenuItem SET ItemStatus = 0 WHERE MenuItemID = {txtID.Text}");
                    MessageBox.Show("Đã xóa món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi khi xóa: " + ex.Message); }
            }
        }
    }
}