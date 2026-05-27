using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class ProductForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== BIẾN UI TOÀN CỤC =====================
        private Guna2TextBox txtID, txtName, txtPrice;
        private Guna2PictureBox picFood;
        private Guna2Button btnBrowse, btnEdit, btnDelete, btnAddNewProduct;
        private Guna2ComboBox cboFilterCategory, cboInputCategory, cboInputStatus;
        private FlowLayoutPanel flpProducts;

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Name = "ProductForm";
            this.Text = "ProductForm";
            this.ResumeLayout(false);
        }
        #endregion

        // ===================== BUILD UI (Chuyển từ InitializeModernUI) =====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            // --- CỘT BÊN PHẢI (CHỈNH SỬA VÀ THÔNG TIN CHI TIẾT) ---
            Guna2Panel pnlRight = new Guna2Panel
            {
                Dock = DockStyle.Right,
                Width = 380,
                FillColor = Color.White,
                CustomBorderThickness = new Padding(1, 0, 0, 0),
                CustomBorderColor = Color.LightGray
            };
            this.Controls.Add(pnlRight);

            FlowLayoutPanel flpInput = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(25, 50, 25, 40)
            };
            pnlRight.Controls.Add(flpInput);

            int cWidth = 320;
            int cCenter = (cWidth - 150) / 2;

            flpInput.Controls.Add(new Label { Text = "THÔNG TIN CHI TIẾT", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), AutoSize = false, Size = new Size(cWidth, 40), TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(0, 0, 0, 20) });

            picFood = new Guna2PictureBox { Size = new Size(150, 140), BorderRadius = 10, SizeMode = PictureBoxSizeMode.Zoom, FillColor = Color.FromArgb(240, 240, 240), Margin = new Padding(cCenter, 0, 0, 15) };
            flpInput.Controls.Add(picFood);

            btnBrowse = new Guna2Button { Text = "Đổi ảnh khác", Size = new Size(150, 35), BorderRadius = 8, FillColor = Color.Gray, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(cCenter, 0, 0, 25) };
            btnBrowse.Click += BtnBrowse_Click;
            flpInput.Controls.Add(btnBrowse);

            flpInput.Controls.Add(new Label { Text = "Danh mục:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            cboInputCategory = new Guna2ComboBox { Size = new Size(cWidth, 40), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 15) };
            flpInput.Controls.Add(cboInputCategory);

            flpInput.Controls.Add(new Label { Text = "Tên món ăn:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtName = new Guna2TextBox { Size = new Size(cWidth, 42), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), ForeColor = Color.Black, BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 15) };
            flpInput.Controls.Add(txtName);

            flpInput.Controls.Add(new Label { Text = "Giá bán (VNĐ):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
            txtPrice = new Guna2TextBox { Size = new Size(cWidth, 42), BorderRadius = 5, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(245, 246, 250), ForeColor = Color.Black, BorderColor = Color.FromArgb(213, 218, 223), Margin = new Padding(0, 0, 0, 15) };
            flpInput.Controls.Add(txtPrice);

            flpInput.Controls.Add(new Label { Text = "Trạng thái:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(0, 0, 0, 5) });
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

            // --- CỘT BÊN TRÁI (TOP BAR & DANH SÁCH MÓN ĂN) ---
            Guna2Panel pnlCenter = new Guna2Panel { Dock = DockStyle.Fill };
            this.Controls.Add(pnlCenter);

            TableLayoutPanel tlpLeft = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlCenter.Controls.Add(tlpLeft);

            Guna2Panel pnlTopCenter = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.Transparent, Margin = new Padding(0) };
            tlpLeft.Controls.Add(pnlTopCenter, 0, 0);

            pnlTopCenter.Controls.Add(new Label { Text = "QUẢN LÝ THỰC ĐƠN", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(30, 45), AutoSize = true });
            pnlTopCenter.Controls.Add(new Label { Text = "Lọc theo:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(340, 55), AutoSize = true });

            cboFilterCategory = new Guna2ComboBox { Location = new Point(420, 45), Size = new Size(200, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F), FillColor = Color.White, BorderColor = Color.LightGray };
            cboFilterCategory.SelectedIndexChanged += CboFilterCategory_SelectedIndexChanged;
            pnlTopCenter.Controls.Add(cboFilterCategory);

            btnAddNewProduct = new Guna2Button { Text = "➕ THÊM MÓN MỚI", Location = new Point(650, 45), Size = new Size(180, 36), BorderRadius = 5, FillColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAddNewProduct.Click += BtnAddNewProduct_Click;
            pnlTopCenter.Controls.Add(btnAddNewProduct);

            flpProducts = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(25, 15, 20, 20), Margin = new Padding(0) };
            tlpLeft.Controls.Add(flpProducts, 0, 1);
        }
    }
}