using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class ProductAddForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== KHAI BÁO CONTROLS =====================
        private Guna2TextBox txtItemName;
        private Guna2ComboBox cboCategory;
        private Guna2TextBox txtPrice;
        private Guna2PictureBox picItem;
        private Guna2Button btnChooseImg, btnSave;
        private Guna2ControlBox btnClose;
        private Guna2ShadowForm shadowForm;
        private Guna2Elipse elipseForm;
        private Guna2DragControl dragControl;

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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 650);
            this.Text = "ProductAddForm";
        }

        #endregion

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            this.Controls.Clear();

            // CÂU LỆNH CHỐNG VỠ FORM
            this.AutoScaleMode = AutoScaleMode.None;
            this.Size = new Size(500, 650);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            // Đổ bóng & Bo góc cho Form
            shadowForm = new Guna2ShadowForm(this);
            elipseForm = new Guna2Elipse { TargetControl = this, BorderRadius = 15 };

            // --- 1. THANH TIÊU ĐỀ ---
            Guna2Panel pnlTop = new Guna2Panel { Dock = DockStyle.Top, Height = 50, FillColor = Color.FromArgb(88, 28, 230) };
            this.Controls.Add(pnlTop);

            Label lblTitle = new Label { Text = "THÊM MÓN MỚI", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 12) };
            pnlTop.Controls.Add(lblTitle);

            btnClose = new Guna2ControlBox { Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(50, 50), Location = new Point(450, 0), FillColor = Color.Transparent, BackColor = Color.Transparent, IconColor = Color.White, Cursor = Cursors.Hand, CustomClick = true };
            btnClose.Click += BtnClose_Click; // Liên kết sự kiện bên .cs
            pnlTop.Controls.Add(btnClose);

            dragControl = new Guna2DragControl { TargetControl = pnlTop };

            // --- 2. LƯỚI TABLELAYOUT (CHỐNG ĐÈ 100%) ---
            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 9,
                Padding = new Padding(40, 25, 40, 20), // Tự động ép lề Trái - Phải đúng 40px
                BackColor = Color.White
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 9; i++) tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Tự giãn theo content

            this.Controls.Add(tlp);
            tlp.BringToFront(); // Để TableLayout nằm dưới thanh tiêu đề

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

            btnChooseImg = new Guna2Button { Text = "Tải ảnh lên", Size = new Size(120, 35), Location = new Point(120, 32), BorderRadius = 5, Font = new Font("Segoe UI", 9F, FontStyle.Bold), FillColor = Color.FromArgb(224, 224, 224), ForeColor = Color.Black, Cursor = Cursors.Hand };
            btnChooseImg.Click += BtnChooseImg_Click; // Liên kết sự kiện

            pnlImage.Controls.Add(picItem);
            pnlImage.Controls.Add(btnChooseImg);
            tlp.Controls.Add(pnlImage, 0, 7);

            // --- HÀNG 9: NÚT XÁC NHẬN ---
            btnSave = new Guna2Button { Text = "XÁC NHẬN THÊM", Dock = DockStyle.Fill, MinimumSize = new Size(0, 45), Height = 45, BorderRadius = 5, Font = new Font("Segoe UI", 11F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click; // Liên kết sự kiện
            tlp.Controls.Add(btnSave, 0, 8);
        }
    }
}