using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ProductAddForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private Guna2TextBox txtItemName;
        private Guna2ComboBox cboCategory;
        private Guna2TextBox txtPrice;
        private Guna2PictureBox picItem;
        private string selectedImagePath = "";

        public ProductAddForm()
        {
            InitializeComponent();
            this.Controls.Clear();

            // 1. CÂU LỆNH CHỐNG VỠ FORM (Khóa tính năng tự Scale của Windows)
            this.AutoScaleMode = AutoScaleMode.None;

            this.Size = new Size(500, 650);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            BuildPopupUI_Unbreakable(); // Dùng lưới bất tử

            Guna2ShadowForm shadow = new Guna2ShadowForm(this);

            // Gắn sự kiện Load form
            this.Load += ProductAddForm_Load;
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void BuildPopupUI_Unbreakable()
        {
            // Viền bo góc
            Guna2Elipse elipse = new Guna2Elipse { TargetControl = this, BorderRadius = 15 };

            // --- 1. THANH TIÊU ĐỀ ---
            Guna2Panel pnlTop = new Guna2Panel { Dock = DockStyle.Top, Height = 50, FillColor = Color.FromArgb(88, 28, 230) };
            this.Controls.Add(pnlTop);

            Label lblTitle = new Label { Text = "THÊM MÓN MỚI", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 12) };
            pnlTop.Controls.Add(lblTitle);

            Guna2ControlBox btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(50, 50), Location = new Point(450, 0), FillColor = Color.Transparent, BackColor = Color.Transparent, IconColor = Color.White, Cursor = Cursors.Hand, CustomClick = true };
            btnClose.Click += BtnClose_Click; // Tách sự kiện đóng Form
            pnlTop.Controls.Add(btnClose);

            Guna2DragControl drag = new Guna2DragControl { TargetControl = pnlTop };

            // --- 2. LƯỚI TABLELAYOUT (CHỐNG ĐÈ 100%) ---
            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 9,
                Padding = new Padding(40, 25, 40, 20), // Tự động ép lề Trái - Phải đúng 40px, canh giữa tuyệt đối!
                BackColor = Color.White
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 9; i++) tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Các hàng tự giãn theo content

            this.Controls.Add(tlp);
            tlp.BringToFront(); // Để TableLayout nằm dưới thanh màu tím

            // --- HÀNG 1: LABEL DANH MỤC ---
            Label lblCat = new Label { Text = "DANH MỤC:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            tlp.Controls.Add(lblCat, 0, 0);

            // --- HÀNG 2: COMBOBOX DANH MỤC ---
            cboCategory = new Guna2ComboBox { Dock = DockStyle.Fill, MinimumSize = new Size(0, 40), Height = 40, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 0, 0, 15) };
            tlp.Controls.Add(cboCategory, 0, 1);

            // --- HÀNG 3: LABEL TÊN MÓN ---
            Label lblName = new Label { Text = "TÊN MÓN ĂN:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            tlp.Controls.Add(lblName, 0, 2);

            // --- HÀNG 4: TEXTBOX TÊN MÓN ---
            txtItemName = new Guna2TextBox { Dock = DockStyle.Fill, MinimumSize = new Size(0, 40), Height = 40, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Nhập tên món...", Margin = new Padding(0, 0, 0, 15) };
            tlp.Controls.Add(txtItemName, 0, 3);

            // --- HÀNG 5: LABEL GIÁ ---
            Label lblPrice = new Label { Text = "GIÁ BÁN (VNĐ):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            tlp.Controls.Add(lblPrice, 0, 4);

            // --- HÀNG 6: TEXTBOX GIÁ ---
            txtPrice = new Guna2TextBox { Dock = DockStyle.Fill, MinimumSize = new Size(0, 40), Height = 40, BorderRadius = 5, Font = new Font("Segoe UI", 11F), PlaceholderText = "Ví dụ: 50000", Margin = new Padding(0, 0, 0, 15) };
            tlp.Controls.Add(txtPrice, 0, 5);

            // --- HÀNG 7: LABEL ẢNH ---
            Label lblImage = new Label { Text = "HÌNH ẢNH:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) };
            tlp.Controls.Add(lblImage, 0, 6);

            // --- HÀNG 8: KHU VỰC CHỌN ẢNH ---
            Panel pnlImage = new Panel { Dock = DockStyle.Fill, MinimumSize = new Size(0, 100), Height = 100, Margin = new Padding(0, 0, 0, 30) };
            picItem = new Guna2PictureBox { Size = new Size(100, 100), Location = new Point(0, 0), SizeMode = PictureBoxSizeMode.Zoom, BorderRadius = 10, FillColor = Color.FromArgb(245, 246, 250) };
            Guna2Button btnChooseImg = new Guna2Button { Text = "Tải ảnh lên", Size = new Size(120, 35), Location = new Point(120, 32), BorderRadius = 5, Font = new Font("Segoe UI", 9F, FontStyle.Bold), FillColor = Color.FromArgb(224, 224, 224), ForeColor = Color.Black, Cursor = Cursors.Hand };
            btnChooseImg.Click += BtnChooseImg_Click;
            pnlImage.Controls.Add(picItem);
            pnlImage.Controls.Add(btnChooseImg);
            tlp.Controls.Add(pnlImage, 0, 7);

            // --- HÀNG 9: NÚT XÁC NHẬN ---
            Guna2Button btnSave = new Guna2Button { Text = "XÁC NHẬN THÊM", Dock = DockStyle.Fill, MinimumSize = new Size(0, 45), Height = 45, BorderRadius = 5, Font = new Font("Segoe UI", 11F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;
            tlp.Controls.Add(btnSave, 0, 8);
        }

        #endregion


        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC DATABASE
        // ========================================================

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT CategoryID, CategoryName FROM Category WHERE IsActive = 1";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                cboCategory.DataSource = dt;
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh mục: " + ex.Message); }
        }

        #endregion


        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN (EVENTS)
        // ========================================================

        private void ProductAddForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnChooseImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picItem.Image = Image.FromFile(ofd.FileName);
                    // Lưu lại TOÀN BỘ đường dẫn gốc của ảnh trên máy bạn
                    selectedImagePath = ofd.FileName;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Tên món và Giá bán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Giá bán chỉ được nhập số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int catId = Convert.ToInt32(cboCategory.SelectedValue);
                string finalImageName = "";

                // --- LOGIC COPY ẢNH VÀO THƯ MỤC CỦA APP ---
                if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
                {
                    // Lấy đường dẫn thư mục MenuImages của app
                    string imageFolder = Path.Combine(Application.StartupPath, "MenuImages");
                    if (!Directory.Exists(imageFolder)) Directory.CreateDirectory(imageFolder);

                    // Đổi tên ảnh tránh bị trùng lặp
                    finalImageName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(selectedImagePath);
                    string destPath = Path.Combine(imageFolder, finalImageName);

                    // Thực hiện copy file
                    File.Copy(selectedImagePath, destPath, true);
                }

                // Lưu tên ảnh mới vào Database
                string query = $"INSERT INTO MenuItem (CategoryID, ItemName, Price, Status, ImageUrl, ItemStatus, CreatedAt) " +
                               $"VALUES ({catId}, N'{txtItemName.Text}', {price}, N'Còn', N'{finalImageName}', 1, GETDATE())";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);

                MessageBox.Show("Đã thêm món mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message);
            }
        }

        #endregion
    }
}