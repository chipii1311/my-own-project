using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    // Đã đổi tên Class thành NewPromotionForm
    public partial class NewPromotionForm : Form
    {
        // ════════════════════════════════════════════════════════
        // DESIGN TOKENS (Đồng bộ với SettingForm)
        // ════════════════════════════════════════════════════════
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_S = Color.FromArgb(237, 233, 254);
        private static readonly Color C_GREEN = Color.FromArgb(22, 163, 74);
        private static readonly Color C_BLUE = Color.FromArgb(37, 99, 235);
        private static readonly Color C_RED = Color.FromArgb(220, 38, 38);
        private static readonly Color C_TEXT = Color.FromArgb(17, 24, 39);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_LABEL = Color.FromArgb(75, 85, 99);

        // ════════════════════════════════════════════════════════
        // CONTROLS
        // ════════════════════════════════════════════════════════
        private DataGridView dgvPromotions;
        private Guna2TextBox txtPromoID, txtPromoName, txtDiscount;
        private Guna2DateTimePicker dtpStartDate, dtpEndDate;
        private Guna2ComboBox cboStatus, cboApplyType;
        private Label lblHint;
        private Guna2Button btnAdd, btnSave, btnDelete, btnClear;

        // Đã đổi tên Constructor thành NewPromotionForm
        public NewPromotionForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildUI();

            // Tạm thời comment hàm load data lại để bạn ngắm giao diện trước
            // this.Load += (s, e) => LoadPromotionData(); 
        }

        // ════════════════════════════════════════════════════════
        // UI BUILDER
        // ════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.SuspendLayout();

            // ── HEADER ─────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = C_WHITE };
            pnlHeader.Paint += PaintBottomBorderLight;

            var lblTitle = new Label
            {
                Text = "🎁 Quản lý Khuyến mãi",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(24, 15)
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── MAIN LAYOUT (Chia 2 cột) ────────────────────────
            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 20, 24, 24) };

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlBody.Controls.Add(tlp);

            // ── LEFT: GRID CARD ────────────────────────────────
            var cardLeft = new Panel { Dock = DockStyle.Fill, BackColor = C_WHITE, Margin = new Padding(0, 0, 12, 0) };
            cardLeft.Region = RoundRegion(cardLeft, 12);
            cardLeft.Resize += (s, e) => cardLeft.Region = RoundRegion(cardLeft, 12);

            var cardHdr = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;
            var lblGridTitle = new Label { Text = "Danh sách chương trình", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(18, 15) };
            cardHdr.Controls.Add(lblGridTitle);

            dgvPromotions = MakeGrid();
            // dgvPromotions.CellClick += DgvPromotions_CellClick; // Bật sau khi nối DB

            cardLeft.Controls.Add(dgvPromotions);
            cardLeft.Controls.Add(cardHdr);
            tlp.Controls.Add(cardLeft, 0, 0);

            // ── RIGHT: FORM CARD ───────────────────────────────
            var cardRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Margin = new Padding(12, 0, 0, 0),
                Padding = new Padding(24)
            };
            cardRight.Region = RoundRegion(cardRight, 12);
            cardRight.Resize += (s, e) => cardRight.Region = RoundRegion(cardRight, 12);

            var lblFormTitle = new Label { Text = "Chi tiết khuyến mãi", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = C_TEXT, Dock = DockStyle.Top, Height = 36 };
            var sep = new Panel { Dock = DockStyle.Top, Height = 2, BackColor = C_PURPLE, Margin = new Padding(0, 0, 0, 16) };

            lblHint = new Label
            {
                Text = "👆 Nhấp vào danh sách bên trái để xem/sửa",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.Transparent
            };

            txtPromoID = new Guna2TextBox { Visible = false };

            var lName = MakeFieldLabel("Tên chương trình *");
            txtPromoName = MakeTextBox("VD: Lễ hội bia giảm giá...");

            var lType = MakeFieldLabel("Hình thức áp dụng");
            cboApplyType = MakeComboBox(new object[] { "Toàn bộ hóa đơn", "Theo món cụ thể" });

            var lDisc = MakeFieldLabel("Phần trăm giảm (%) *");
            txtDiscount = MakeTextBox("VD: 10, 20...");

            var lStart = MakeFieldLabel("Ngày bắt đầu");
            dtpStartDate = MakeDatePicker();

            var lEnd = MakeFieldLabel("Ngày kết thúc");
            dtpEndDate = MakeDatePicker();

            var lStatus = MakeFieldLabel("Trạng thái");
            cboStatus = MakeComboBox(new object[] { "Active", "Inactive" });

            // ── BUTTONS ─────────────────────────────────────────
            var pnlBtns = new Panel { Dock = DockStyle.Top, Height = 100 };

            btnAdd = MakeBtn("➕  Thêm mới", C_PURPLE, C_WHITE);
            btnAdd.Location = new Point(0, 0);

            btnSave = MakeBtn("💾  Lưu thay đổi", C_BLUE, C_WHITE);
            btnSave.Location = new Point(0, 50);
            btnSave.Enabled = false;

            btnDelete = MakeBtn("🗑️  Kết thúc sớm", C_RED, C_WHITE);
            btnDelete.Location = new Point(0, 0); // Vị trí set ở Resize
            btnDelete.Enabled = false;

            btnClear = MakeBtn("✕  Hủy", Color.FromArgb(229, 231, 235), C_MUTED);
            btnClear.Location = new Point(0, 50);

            pnlBtns.Controls.AddRange(new Control[] { btnAdd, btnSave, btnDelete, btnClear });

            // Assemble Form (Add ngược từ dưới lên trên do Dock.Top)
            foreach (var c in new Control[] { pnlBtns, cboStatus, lStatus, dtpEndDate, lEnd, dtpStartDate, lStart, txtDiscount, lDisc, cboApplyType, lType, txtPromoName, lName, lblHint, sep, lblFormTitle, txtPromoID })
            {
                cardRight.Controls.Add(c);
            }

            // Responsive Layout
            cardRight.Resize += (s, e) =>
            {
                int w = cardRight.ClientSize.Width - 48;
                txtPromoName.Width = w;
                cboApplyType.Width = w;
                txtDiscount.Width = w;
                dtpStartDate.Width = w;
                dtpEndDate.Width = w;
                cboStatus.Width = w;

                pnlBtns.Width = w;
                int halfW = (w - 10) / 2;
                btnAdd.Width = halfW;
                btnSave.Width = halfW;

                btnDelete.Width = halfW;
                btnDelete.Location = new Point(halfW + 10, 0);

                btnClear.Width = halfW;
                btnClear.Location = new Point(halfW + 10, 50);
            };

            tlp.Controls.Add(cardRight, 1, 0);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlHeader);

            this.ResumeLayout(false);
        }

        // ════════════════════════════════════════════════════════
        // FACTORY HELPERS
        // ════════════════════════════════════════════════════════
        private DataGridView MakeGrid()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(243, 244, 246),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Cursor = Cursors.Hand,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 44,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgv.DefaultCellStyle.ForeColor = C_TEXT;
            dgv.DefaultCellStyle.SelectionBackColor = C_PURPLE_S;
            dgv.DefaultCellStyle.SelectionForeColor = C_PURPLE;
            dgv.DefaultCellStyle.BackColor = C_WHITE;
            dgv.DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.RowTemplate.Height = 46;
            return dgv;
        }

        private Label MakeFieldLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = C_LABEL,
            Dock = DockStyle.Top,
            Height = 26,
            BackColor = Color.Transparent
        };

        private Guna2TextBox MakeTextBox(string placeholder) => new Guna2TextBox
        {
            PlaceholderText = placeholder,
            Dock = DockStyle.Top,
            Height = 38,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = Color.FromArgb(249, 250, 251),
            Margin = new Padding(0, 0, 0, 16)
        };

        private Guna2ComboBox MakeComboBox(object[] items)
        {
            var cbo = new Guna2ComboBox
            {
                Dock = DockStyle.Top,
                Height = 38,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10F),
                FillColor = Color.FromArgb(249, 250, 251),
                Margin = new Padding(0, 0, 0, 16)
            };
            cbo.Items.AddRange(items);
            cbo.SelectedIndex = 0;
            return cbo;
        }

        private Guna2DateTimePicker MakeDatePicker() => new Guna2DateTimePicker
        {
            Dock = DockStyle.Top,
            Height = 38,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = Color.FromArgb(249, 250, 251),
            Format = DateTimePickerFormat.Short,
            Margin = new Padding(0, 0, 0, 16)
        };

        private Guna2Button MakeBtn(string text, Color fill, Color fore) => new Guna2Button
        {
            Text = text,
            Height = 40,
            BorderRadius = 8,
            BorderThickness = 0,
            FillColor = fill,
            ForeColor = fore,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        private void PaintBottomBorderLight(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (var pen = new System.Drawing.Pen(Color.FromArgb(243, 244, 246), 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private System.Drawing.Region RoundRegion(Control c, int r)
        {
            var path = new GraphicsPath();
            int w = c.Width, h = c.Height;
            if (w <= 0 || h <= 0) return null;
            path.AddArc(0, 0, r * 2, r * 2, 180, 90);
            path.AddArc(w - r * 2, 0, r * 2, r * 2, 270, 90);
            path.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(0, h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseAllFigures();
            return new System.Drawing.Region(path);
        }
    }
}