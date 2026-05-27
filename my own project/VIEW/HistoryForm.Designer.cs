using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class HistoryForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== DESIGN TOKENS =====================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_MID = Color.FromArgb(109, 60, 240);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(237, 233, 254);
        private static readonly Color C_GREEN = Color.FromArgb(22, 163, 74);
        private static readonly Color C_GREEN_SOFT = Color.FromArgb(220, 252, 231);
        private static readonly Color C_BLUE = Color.FromArgb(37, 99, 235);
        private static readonly Color C_BLUE_SOFT = Color.FromArgb(219, 234, 254);
        private static readonly Color C_AMBER = Color.FromArgb(217, 119, 6);
        private static readonly Color C_AMBER_SOFT = Color.FromArgb(254, 243, 199);
        private static readonly Color C_RED = Color.FromArgb(220, 38, 38);
        private static readonly Color C_RED_SOFT = Color.FromArgb(254, 226, 226);
        private static readonly Color C_TEXT = Color.FromArgb(17, 24, 39);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);

        // ===================== CONTROLS =====================
        private Guna2DateTimePicker dtpFrom, dtpTo;
        private Guna2Button btnFilter, btnExport;
        private Guna2Button btnToday, btn7Days, btn30Days, btnThisMonth;
        private Label lblTotalRevenue, lblTotalOrders, lblAvgOrder;
        private Label lblLastUpdated, lblRowCount, lblHint;
        private DataGridView dgvHistory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1114, 666);
            this.Name = "HistoryForm";
            this.Text = "History";
            this.ResumeLayout(false);
        }

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            this.SuspendLayout();

            Panel header = BuildHeader();
            Panel filterBar = BuildFilterBar();
            Panel statCards = BuildStatCards();
            Panel gridHeader = BuildGridHeader();

            // DataGridView
            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(243, 244, 246),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Cursor = Cursors.Hand,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 44,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            // Header style
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHistory.ColumnHeadersDefaultCellStyle.Padding = new Padding(14, 0, 0, 0);

            // Row style
            dgvHistory.DefaultCellStyle.BackColor = C_WHITE;
            dgvHistory.DefaultCellStyle.ForeColor = C_TEXT;
            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dgvHistory.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            dgvHistory.DefaultCellStyle.SelectionForeColor = C_TEXT;
            dgvHistory.DefaultCellStyle.Padding = new Padding(14, 0, 0, 0);
            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 253);
            dgvHistory.RowTemplate.Height = 48;

            // Events mapping cho Grid
            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;
            dgvHistory.CellFormatting += DgvHistory_CellFormatting;

            this.Controls.Add(dgvHistory);
            this.Controls.Add(gridHeader);
            this.Controls.Add(statCards);
            this.Controls.Add(filterBar);
            this.Controls.Add(header);

            this.ResumeLayout(false);
        }

        // ── Header ─────────────────────────────────────────────────────────
        private Panel BuildHeader()
        {
            Panel h = new Panel { Dock = DockStyle.Top, Height = 68, BackColor = C_WHITE };
            h.Paint += PaintBottomBorder;

            Panel accent = new Panel { Size = new Size(4, 68), Location = new Point(0, 0), BackColor = C_PURPLE };

            Label title = new Label { Text = "Lịch sử doanh thu", Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(52, 14) };
            lblLastUpdated = new Label { Text = "", Font = new Font("Segoe UI", 8.5F), ForeColor = C_MUTED, AutoSize = true, Location = new Point(53, 44) };

            h.Controls.AddRange(new Control[] { accent, title, lblLastUpdated });
            return h;
        }

        // ── Filter bar ──────────────────────────────────────────────────────
        private Panel BuildFilterBar()
        {
            Panel bar = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = C_WHITE, Padding = new Padding(16, 12, 16, 0) };
            bar.Paint += PaintBottomBorder;

            btnToday = QuickBtn("Hôm nay", 16, true);
            btn7Days = QuickBtn("7 ngày", 106, false);
            btn30Days = QuickBtn("30 ngày", 196, false);
            btnThisMonth = QuickBtn("Tháng này", 286, false);

            // Gán sự kiện Click cho nút
            btnToday.Click += BtnToday_Click;
            btn7Days.Click += Btn7Days_Click;
            btn30Days.Click += Btn30Days_Click;
            btnThisMonth.Click += BtnThisMonth_Click;

            Label lblFrom = new Label { Text = "Từ:", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, AutoSize = true, Location = new Point(392, 21) };

            dtpFrom = new Guna2DateTimePicker
            {
                Size = new Size(126, 36),
                Location = new Point(416, 12),
                BorderRadius = 8,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(-29),
                BorderColor = C_BORDER,
                FillColor = C_WHITE
            };

            Label lblTo = new Label { Text = "đến:", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, AutoSize = true, Location = new Point(548, 21) };

            dtpTo = new Guna2DateTimePicker
            {
                Size = new Size(126, 36),
                Location = new Point(578, 12),
                BorderRadius = 8,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                BorderColor = C_BORDER,
                FillColor = C_WHITE
            };

            btnFilter = new Guna2Button { Text = "🔍 Lọc", Size = new Size(88, 36), Location = new Point(710, 12), BorderRadius = 8, BorderThickness = 0, FillColor = C_PURPLE, ForeColor = Color.White, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnFilter.Click += BtnFilter_Click;

            btnExport = new Guna2Button { Text = "⬇ Xuất CSV", Size = new Size(110, 36), Location = new Point(806, 12), BorderRadius = 8, BorderThickness = 0, FillColor = C_GREEN_SOFT, ForeColor = C_GREEN, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnExport.Click += BtnExport_Click;

            bar.Controls.AddRange(new Control[] { btnToday, btn7Days, btn30Days, btnThisMonth, lblFrom, dtpFrom, lblTo, dtpTo, btnFilter, btnExport });
            return bar;
        }

        private Guna2Button QuickBtn(string text, int x, bool active)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(84, 36),
                Location = new Point(x, 12),
                BorderRadius = 18,
                BorderThickness = 0,
                FillColor = active ? C_PURPLE : C_PURPLE_SOFT,
                ForeColor = active ? Color.White : C_PURPLE,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // ── Stat cards ───────────────────────────────────────────────────────
        private Panel BuildStatCards()
        {
            Panel row = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = C_BG, Padding = new Padding(16, 12, 16, 0) };

            Panel c1 = StatCard("TỔNG DOANH THU", "💰", C_GREEN, C_GREEN_SOFT, out lblTotalRevenue);
            Panel c2 = StatCard("SỐ ĐƠN HÀNG", "🧾", C_PURPLE, C_PURPLE_SOFT, out lblTotalOrders);
            Panel c3 = StatCard("GIÁ TRỊ TB/ĐƠN", "📈", C_BLUE, C_BLUE_SOFT, out lblAvgOrder);

            c1.Location = new Point(16, 12);
            c2.Location = new Point(246, 12);
            c3.Location = new Point(476, 12);

            row.Controls.AddRange(new Control[] { c1, c2, c3 });
            return row;
        }

        private Panel StatCard(string title, string icon, Color accent, Color softBg, out Label lblVal)
        {
            Panel card = new Panel { Size = new Size(220, 76), BackColor = C_WHITE };
            card.Paint += (s, e) =>
            {
                var p = s as Panel;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int r = 12, w = p.Width - 1, h = p.Height - 1;
                    path.AddArc(0, 0, r * 2, r * 2, 180, 90);
                    path.AddArc(w - r * 2, 0, r * 2, r * 2, 270, 90);
                    path.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
                    path.AddArc(0, h - r * 2, r * 2, r * 2, 90, 90);
                    path.CloseFigure();
                    p.Region = new Region(path);
                }
            };

            Panel bar = new Panel { Size = new Size(4, 76), Location = new Point(0, 0), BackColor = accent };
            Label lTitle = new Label { Text = icon + "  " + title, Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = C_MUTED, Location = new Point(16, 10), AutoSize = true };
            lblVal = new Label { Text = "—", Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = accent, Location = new Point(16, 32), AutoSize = true };

            card.Controls.AddRange(new Control[] { bar, lTitle, lblVal });
            return card;
        }

        // ── Grid header bar ─────────────────────────────────────────────────
        private Panel BuildGridHeader()
        {
            Panel bar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = C_WHITE, Padding = new Padding(20, 0, 20, 0) };
            bar.Paint += PaintBottomBorder;

            Label lblTitle = new Label { Text = "Danh sách hóa đơn", Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(20, 14) };
            lblRowCount = new Label { Text = "", Font = new Font("Segoe UI", 9F), ForeColor = C_PURPLE, AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblHint = new Label { Text = "✦  Nhấp đúp vào hóa đơn để in lại", Font = new Font("Segoe UI", 8.5F, FontStyle.Italic), ForeColor = C_MUTED, AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right };

            bar.Controls.AddRange(new Control[] { lblTitle, lblRowCount, lblHint });
            bar.Resize += (s, e) =>
            {
                lblHint.Location = new Point(bar.Width - lblHint.Width - 20, 16);
                lblRowCount.Location = new Point(lblHint.Left - lblRowCount.Width - 24, 16);
            };

            return bar;
        }

        // ===================== PAINT HELPER =====================
        private void PaintBottomBorder(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (Pen pen = new Pen(C_BORDER, 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }
    }
}