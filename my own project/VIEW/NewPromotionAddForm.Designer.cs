using Guna.UI2.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class NewPromotionAddForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Palette (Thiết kế hiện đại) ──────────────────────────
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_BG = Color.FromArgb(246, 247, 252);
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_S = Color.FromArgb(237, 233, 255);
        private static readonly Color C_GREEN = Color.FromArgb(16, 185, 129);
        private static readonly Color C_GREEN_D = Color.FromArgb(5, 150, 105);
        private static readonly Color C_TEXT = Color.FromArgb(31, 41, 55);
        private static readonly Color C_LABEL = Color.FromArgb(75, 85, 99);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(220, 218, 240);
        private static readonly Color C_FIELD = Color.FromArgb(249, 250, 251);

        // ── Controls UI toàn cục ─────────────────────────────────
        private Guna2TextBox txtPromoName, txtDiscount;
        private Guna2DateTimePicker dtpStart, dtpEnd;
        private Guna2ComboBox cboStatus, cboApplyType;
        private Guna2Button btnSave, btnCancel;
        public Label lblTitle;
        private Panel pnlItemPicker;
        private CheckedListBox clbItems;
        private Guna2DragControl dragControl;
        private FlowLayoutPanel flpForm;

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
            this.ClientSize = new System.Drawing.Size(480, 660);
            this.Text = "NewPromotionAddForm";
        }

        #endregion

        // ========================================================
        // BUILD UI TĨNH VÀ CÁC THÀNH PHẦN TRỢ GIÚP (HELPERS)
        // ========================================================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.SuspendLayout();

            // ── 1. Header (Có Gradient và vòng tròn chìm) ──
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 66, BackColor = C_PURPLE };
            pnlHeader.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(pnlHeader.ClientRectangle, Color.FromArgb(35, Color.White), Color.Transparent, 40f))
                    e.Graphics.FillRectangle(br, pnlHeader.ClientRectangle);

                using (var br2 = new SolidBrush(Color.FromArgb(16, Color.White)))
                    e.Graphics.FillEllipse(br2, Width - 90, -30, 140, 140);
            };

            var pnlIcon = new Panel { Size = new Size(38, 38), Location = new Point(20, 14), BackColor = Color.Transparent };
            pnlIcon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(50, Color.White)))
                    e.Graphics.FillEllipse(br, 0, 0, 37, 37);
                using (var p = new Pen(Color.White, 1.8f))
                {
                    e.Graphics.DrawRectangle(p, 8, 10, 16, 16);
                    e.Graphics.DrawLine(p, 8, 10, 18, 4); e.Graphics.DrawLine(p, 24, 10, 18, 4);
                    e.Graphics.DrawLine(p, 28, 18, 33, 18); e.Graphics.DrawLine(p, 30, 15, 33, 18); e.Graphics.DrawLine(p, 30, 21, 33, 18);
                }
            };

            lblTitle = new Label { Text = "THÊM KHUYẾN MÃI MỚI", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = C_WHITE, BackColor = Color.Transparent, AutoSize = true, Location = new Point(66, 14) };
            var lblSub = new Label { Text = "Tạo chương trình khuyến mãi cho nhà hàng", Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(210, 255, 255, 255), BackColor = Color.Transparent, AutoSize = true, Location = new Point(68, 40) };

            dragControl = new Guna2DragControl { TargetControl = pnlHeader };
            pnlHeader.Controls.AddRange(new Control[] { pnlIcon, lblTitle, lblSub });

            // ── 2. Footer (Nút Lưu, Hủy) ──
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 72, BackColor = C_WHITE };
            pnlFooter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER), 0, 0, Width, 0);

            btnCancel = new Guna2Button { Text = "Hủy bỏ", Size = new Size(110, 42), Location = new Point(230, 15), BorderRadius = 10, FillColor = Color.FromArgb(240, 239, 252), ForeColor = C_MUTED, BorderColor = C_BORDER, BorderThickness = 1, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCancel.HoverState.FillColor = Color.FromArgb(228, 226, 248);
            btnCancel.Click += (s, e) => Close();

            btnSave = new Guna2Button { Text = "  💾  TẠO MỚI", Size = new Size(148, 42), Location = new Point(352, 15), BorderRadius = 10, FillColor = C_GREEN, ForeColor = C_WHITE, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.HoverState.FillColor = C_GREEN_D;
            btnSave.Click += BtnSave_Click; // Gọi hàm bên file .cs

            pnlFooter.Controls.AddRange(new Control[] { btnCancel, btnSave });

            // ── 3. Body (Lưới FlowLayoutPanel cuộn tự động) ──
            flpForm = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(24, 16, 24, 8), BackColor = C_BG };

            int fw = 432; // Chiều rộng các ô input

            // Tên chương trình
            flpForm.Controls.Add(MakeLabel("Tên chương trình *"));
            txtPromoName = MakeTextBox("VD: Lễ hội bia, Tri ân khách hàng...", fw);
            flpForm.Controls.Add(txtPromoName);

            // Hình thức áp dụng
            flpForm.Controls.Add(MakeLabel("Hình thức áp dụng *"));
            cboApplyType = MakeCombo(fw);
            cboApplyType.Items.AddRange(new object[] { "Giảm trên tổng hóa đơn", "Giảm theo món ăn" });
            cboApplyType.SelectedIndex = 0;
            cboApplyType.SelectedIndexChanged += CboApplyType_Changed; // Gọi hàm bên file .cs
            flpForm.Controls.Add(cboApplyType);

            // Panel chọn món (ẩn/hiện tự động)
            pnlItemPicker = new Panel { Width = fw, Height = 0, BackColor = Color.Transparent, Margin = new Padding(0) };
            var lblPicker = new Label { Text = "Chọn món áp dụng *", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = C_LABEL, Location = new Point(0, 0), AutoSize = true };
            clbItems = new CheckedListBox { Location = new Point(0, 22), Size = new Size(fw, 130), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10F), BackColor = C_WHITE, ForeColor = C_TEXT, CheckOnClick = true, IntegralHeight = false };
            var lblHint = new Label { Text = "ℹ  Tích chọn một hoặc nhiều món ăn muốn áp dụng.", Font = new Font("Segoe UI", 8F, FontStyle.Italic), ForeColor = C_MUTED, Location = new Point(0, 158), Size = new Size(fw, 20) };

            pnlItemPicker.Controls.AddRange(new Control[] { lblPicker, clbItems, lblHint });
            flpForm.Controls.Add(pnlItemPicker);

            // % Giảm
            flpForm.Controls.Add(MakeLabel("Phần trăm giảm (%) *"));
            txtDiscount = MakeTextBox("VD: 10, 20, 50...", fw);
            flpForm.Controls.Add(txtDiscount);

            // Ngày bắt đầu / kết thúc (Nằm song song 50/50)
            var pnlDates = new Panel { Width = fw, Height = 68, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 10) };
            int hw = (fw - 12) / 2;
            var lblS = new Label { Text = "Từ ngày *", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = C_LABEL, Location = new Point(0, 0), AutoSize = true };
            var lblE = new Label { Text = "Đến ngày *", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = C_LABEL, Location = new Point(hw + 12, 0), AutoSize = true };

            dtpStart = MakeDatePicker(hw); dtpStart.Location = new Point(0, 22);
            dtpEnd = MakeDatePicker(hw); dtpEnd.Location = new Point(hw + 12, 22);
            pnlDates.Controls.AddRange(new Control[] { lblS, lblE, dtpStart, dtpEnd });
            flpForm.Controls.Add(pnlDates);

            // Trạng thái
            flpForm.Controls.Add(MakeLabel("Trạng thái"));
            cboStatus = MakeCombo(fw);
            cboStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            cboStatus.SelectedIndex = 0;
            flpForm.Controls.Add(cboStatus);

            // Ráp vào Form
            this.Controls.Add(flpForm);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);

            this.ResumeLayout(false);
        }

        // ── Helper UI (Sinh Control động) ─────────────────────────
        private Label MakeLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = C_LABEL,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 4),
            BackColor = Color.Transparent
        };

        private Guna2TextBox MakeTextBox(string placeholder, int width) => new Guna2TextBox
        {
            Width = width,
            Height = 42,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10.5F),
            FillColor = C_FIELD,
            PlaceholderText = placeholder,
            ForeColor = C_TEXT,
            Margin = new Padding(0, 0, 0, 8),
            Padding = new Padding(6, 0, 0, 0)
        };

        private Guna2ComboBox MakeCombo(int width) => new Guna2ComboBox
        {
            Width = width,
            Height = 42,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10.5F),
            FillColor = C_FIELD,
            ForeColor = C_TEXT,
            Margin = new Padding(0, 0, 0, 8)
        };

        private Guna2DateTimePicker MakeDatePicker(int width) => new Guna2DateTimePicker
        {
            Width = width,
            Height = 42,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = C_FIELD,
            Format = DateTimePickerFormat.Short
        };
    }
}