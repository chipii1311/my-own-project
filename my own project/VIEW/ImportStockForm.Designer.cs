using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class ImportStockForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== CONTROLS =====================
        private Guna2ComboBox cboIngredient;
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtPrice;
        private Guna2TextBox txtNote;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;

        // ── Palette (Đồng bộ với ExportStockForm) ───────────────
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_BG = Color.FromArgb(248, 249, 254);
        private static readonly Color C_PURPLE = Color.FromArgb(99, 88, 255);
        private static readonly Color C_PURPLE_DARK = Color.FromArgb(78, 68, 220);
        private static readonly Color C_TEXT = Color.FromArgb(22, 22, 38);
        private static readonly Color C_MUTED = Color.FromArgb(130, 128, 158);
        private static readonly Color C_BORDER = Color.FromArgb(225, 224, 240);
        private static readonly Color C_CANCEL_BG = Color.FromArgb(243, 244, 246);

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
            // Kích thước cao hơn Export form (560 thay vì 480) để chứa thêm trường Đơn Giá
            this.ClientSize = new System.Drawing.Size(500, 560);
            this.Name = "ImportStockForm";
            this.StartPosition = FormStartPosition.CenterParent; 
            this.Text = "Nhập kho nguyên liệu";
            this.ResumeLayout(false);
        }
        #endregion

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // ─── Header ───
            Label lblTitle = new Label
            {
                Text = "Nhập kho nguyên liệu",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Location = new Point(30, 25),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            int startY = 80;
            int spacing = 80;

            // ─── 1. Combo Box Nguyên Liệu ───
            Label lblIngredient = new Label { Text = "Nguyên liệu", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, Location = new Point(30, startY), AutoSize = true };
            cboIngredient = new Guna2ComboBox
            {
                Location = new Point(30, startY + 25),
                Size = new Size(420, 42),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(lblIngredient);
            this.Controls.Add(cboIngredient);

            // ─── 2. Text Box Số Lượng ───
            Label lblQuantity = new Label { Text = "Số lượng nhập", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, Location = new Point(30, startY + spacing), AutoSize = true };
            txtQuantity = new Guna2TextBox
            {
                Location = new Point(30, startY + spacing + 25),
                Size = new Size(420, 42),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập số lượng..."
            };
            this.Controls.Add(lblQuantity);
            this.Controls.Add(txtQuantity);

            // ─── 3. Text Box Đơn Giá ───
            Label lblPrice = new Label { Text = "Đơn giá nhập", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, Location = new Point(30, startY + spacing * 2), AutoSize = true };
            txtPrice = new Guna2TextBox
            {
                Location = new Point(30, startY + spacing * 2 + 25),
                Size = new Size(420, 42),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập đơn giá (VNĐ)..."
            };
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);

            // ─── 4. Text Box Ghi chú ───
            Label lblNote = new Label { Text = "Ghi chú (Tên NSX, lô hàng...)", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, Location = new Point(30, startY + spacing * 3), AutoSize = true };
            txtNote = new Guna2TextBox
            {
                Location = new Point(30, startY + spacing * 3 + 25),
                Size = new Size(420, 70),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập ghi chú tùy chọn...",
                Multiline = true
            };
            this.Controls.Add(lblNote);
            this.Controls.Add(txtNote);

            // ─── Nút Hủy ───
            btnCancel = new Guna2Button
            {
                Text = "Hủy bỏ",
                Size = new Size(120, 45),
                Location = new Point(200, 480), // Dịch xuống dưới vì form đã cao hơn
                BorderRadius = 8,
                FillColor = C_CANCEL_BG,
                ForeColor = C_MUTED,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => this.Close();

            // ─── Nút Lưu ───
            btnSave = new Guna2Button
            {
                Text = "Lưu nhập kho",
                Size = new Size(140, 45),
                Location = new Point(330, 480),
                BorderRadius = 8,
                FillColor = C_PURPLE,
                ForeColor = C_WHITE,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            // Gán sự kiện cho file .cs xử lý
            btnSave.HoverState.FillColor = C_PURPLE_DARK;
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnCancel);
            this.Controls.Add(btnSave);
        }
    }
}