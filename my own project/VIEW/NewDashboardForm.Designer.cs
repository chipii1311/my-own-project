using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace my_own_project.VIEW
{
    partial class NewDashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        // ═══════════════════════════════════════════════════════════════
        //  DESIGN TOKENS
        // ═══════════════════════════════════════════════════════════════
        private static readonly Color BG = Color.FromArgb(244, 245, 250);
        private static readonly Color WHITE = Color.White;
        private static readonly Color TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color BORDER = Color.FromArgb(232, 232, 240);
        private static readonly Color PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color PURPLES = Color.FromArgb(238, 237, 254);
        private static readonly Color GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color BLUE = Color.FromArgb(59, 130, 246);
        private static readonly Color RED = Color.FromArgb(239, 68, 68);

        // ═══════════════════════════════════════════════════════════════
        //  CONTROLS
        // ═══════════════════════════════════════════════════════════════
        private Label lblRevenue, lblRevSub;
        private Label lblOrders, lblOrdSub;
        private Label lblAvgOrd, lblAvgSub;
        private Label lblInv, lblInvSub;
        private Label lblUpdated;

        private Chart chartRev, chartCat;
        private Label lblRevMsg, lblCatMsg;

        private FlowLayoutPanel flowTop5;
        private Guna2DataGridView dgv;

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
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "NewDashboardForm";
        }

        #endregion

        // ═══════════════════════════════════════════════════════════════
        //  BUILD UI TĨNH 
        // ═══════════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.Controls.Clear();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            var topBar = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = WHITE };
            topBar.Paint += (s, e) => e.Graphics.DrawLine(new Pen(BORDER), 0, 51, topBar.Width, 51);

            topBar.Controls.Add(Lbl("Dashboard", new Font("Segoe UI", 14F, FontStyle.Bold), TEXT, new Point(20, 13)));

            lblUpdated = Lbl("", new Font("Segoe UI", 9F), MUTED, new Point(170, 18));
            topBar.Controls.Add(lblUpdated);

            var btnRefresh = Btn("↻  Làm mới", PURPLES, PURPLE);
            btnRefresh.Size = new Size(112, 30);
            btnRefresh.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnRefresh.Click += (s, e) => RefreshAll();
            topBar.Controls.Add(btnRefresh);
            topBar.Resize += (s, e) => btnRefresh.Location = new Point(topBar.Width - 128, 11);
            this.Controls.Add(topBar);

            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = BG, Padding = new Padding(18, 14, 18, 18) };
            this.Controls.Add(scroll);
            scroll.BringToFront();

            var inner = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, BackColor = Color.Transparent };
            scroll.Controls.Add(inner);

            var rowKPI = Row(inner, 114, 0);
            BuildKPIRow(rowKPI);

            var rowCharts = Row(inner, 300, 114 + 14);
            BuildChartsRow(rowCharts);

            var rowBottom = Row(inner, 340, 114 + 14 + 300 + 14);
            BuildBottomRow(rowBottom);

            inner.Height = 114 + 14 + 300 + 14 + 340 + 14;
        }

        private Panel Row(Panel parent, int h, int top)
        {
            var p = new Panel { BackColor = Color.Transparent, Bounds = new Rectangle(0, top, parent.Width, h), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            parent.Controls.Add(p);
            parent.Resize += (s, e) => p.Width = parent.Width;
            return p;
        }

        private void BuildKPIRow(Panel row)
        {
            var configs = new[]
            {
                new { Icon="💰", Title="DOANH THU HÔM NAY",  Accent=PURPLE, Soft=PURPLES },
                new { Icon="🧾", Title="TỔNG ĐƠN HÔM NAY",   Accent=BLUE,   Soft=Color.FromArgb(219,234,254) },
                new { Icon="📊", Title="DOANH THU TB/ĐƠN",   Accent=GREEN,  Soft=Color.FromArgb(220,252,231) },
                new { Icon="⚠️", Title="KHO CẦN NHẬP THÊM",  Accent=AMBER,  Soft=Color.FromArgb(254,243,199) },
            };

            Label[] valLabels = new Label[4];
            Label[] subLabels = new Label[4];

            for (int i = 0; i < 4; i++)
            {
                var cfg = configs[i];
                int idx = i;

                var card = Card(12);
                card.Bounds = CardBounds(row.Width, i, 4, row.Height);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                row.Resize += (s, e) => card.Bounds = CardBounds(row.Width, idx, 4, row.Height);

                var ico = new Label { Text = cfg.Icon, Font = new Font("Segoe UI Emoji", 16F), Size = new Size(44, 44), Location = new Point(14, 14), TextAlign = ContentAlignment.MiddleCenter, BackColor = cfg.Soft };
                ico.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var b = new SolidBrush(cfg.Soft))
                    using (var gp = RoundPath(new Rectangle(0, 0, ico.Width, ico.Height), 10))
                        e.Graphics.FillPath(b, gp);
                };
                card.Controls.Add(ico);

                var lTitle = new Label { Text = cfg.Title, Font = new Font("Segoe UI", 7.5F, FontStyle.Bold), ForeColor = MUTED, AutoSize = false, Size = new Size(card.Width - 72, 14), Location = new Point(66, 18), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
                card.Controls.Add(lTitle);

                valLabels[idx] = new Label { Text = "---", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = cfg.Accent, AutoSize = true, Location = new Point(14, 58) };
                card.Controls.Add(valLabels[idx]);

                subLabels[idx] = new Label { Text = "Đang tải...", Font = new Font("Segoe UI", 9F), ForeColor = MUTED, AutoSize = true, Location = new Point(16, 90) };
                card.Controls.Add(subLabels[idx]);
                row.Controls.Add(card);
            }

            lblRevenue = valLabels[0]; lblRevSub = subLabels[0];
            lblOrders = valLabels[1]; lblOrdSub = subLabels[1];
            lblAvgOrd = valLabels[2]; lblAvgSub = subLabels[2];
            lblInv = valLabels[3]; lblInvSub = subLabels[3];
        }

        private Rectangle CardBounds(int rowW, int idx, int count, int rowH)
        {
            int gap = 12;
            int total = rowW - gap * (count - 1);
            int w = total / count;
            int x = idx * (w + gap);
            return new Rectangle(x, 0, w + (idx == count - 1 ? total - w * count : 0), rowH);
        }

        private void BuildChartsRow(Panel row)
        {
            var cardRev = Card(12);
            var cardCat = Card(12);

            Action doResize = () =>
            {
                if (row.Width <= 0) return;
                int gap = 12;
                int revW = (int)((row.Width - gap) * 0.62f);
                int catW = row.Width - gap - revW;
                cardRev.Bounds = new Rectangle(0, 0, revW, row.Height);
                cardCat.Bounds = new Rectangle(revW + gap, 0, catW, row.Height);
            };
            row.Resize += (s, e) => doResize();

            row.Controls.Add(cardRev);
            var hRev = new Label { Text = "Doanh thu 7 ngày gần nhất", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = TEXT, Dock = DockStyle.Top, Height = 36, Padding = new Padding(14, 10, 0, 0) };
            cardRev.Controls.Add(hRev);

            var wrapRev = new Panel { Dock = DockStyle.Fill, BackColor = WHITE };
            cardRev.Controls.Add(wrapRev);
            wrapRev.BringToFront();

            chartRev = BuildBarChart();
            chartRev.Dock = DockStyle.Fill;
            chartRev.BackColor = WHITE;
            wrapRev.Controls.Add(chartRev);

            lblRevMsg = new Label { Text = "Đang tải...", Font = new Font("Segoe UI", 10F), ForeColor = MUTED, BackColor = Color.Transparent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Visible = true };
            wrapRev.Controls.Add(lblRevMsg);
            lblRevMsg.BringToFront();

            row.Controls.Add(cardCat);
            var hCat = new Label { Text = "Tỷ trọng doanh thu theo danh mục", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = TEXT, Dock = DockStyle.Top, Height = 36, Padding = new Padding(14, 10, 0, 0) };
            cardCat.Controls.Add(hCat);

            var wrapCat = new Panel { Dock = DockStyle.Fill, BackColor = WHITE };
            cardCat.Controls.Add(wrapCat);
            wrapCat.BringToFront();

            chartCat = BuildDonutChart();
            chartCat.Dock = DockStyle.Fill;
            chartCat.BackColor = WHITE;
            wrapCat.Controls.Add(chartCat);

            lblCatMsg = new Label { Text = "Đang tải...", Font = new Font("Segoe UI", 10F), ForeColor = MUTED, BackColor = Color.Transparent, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Visible = true };
            wrapCat.Controls.Add(lblCatMsg);
            lblCatMsg.BringToFront();

            row.PerformLayout();
            if (row.Width > 0)
            {
                int gap = 12;
                int revW = (int)((row.Width - gap) * 0.62f);
                int catW = row.Width - gap - revW;
                cardRev.Bounds = new Rectangle(0, 0, revW, row.Height);
                cardCat.Bounds = new Rectangle(revW + gap, 0, catW, row.Height);
            }
        }

        private Chart BuildBarChart()
        {
            var c = new Chart { BackColor = WHITE, BorderlineColor = Color.Transparent };
            var ca = new ChartArea("a") { BackColor = WHITE };
            ca.AxisX.LineColor = BORDER; ca.AxisY.LineColor = BORDER;
            ca.AxisX.MajorGrid.LineColor = Color.Transparent;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(235, 235, 245);
            ca.AxisX.LabelStyle.Font = ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            ca.AxisX.LabelStyle.ForeColor = ca.AxisY.LabelStyle.ForeColor = MUTED;
            ca.AxisY.LabelStyle.Format = "N0";
            ca.InnerPlotPosition = new ElementPosition(5, 5, 90, 85);
            c.ChartAreas.Add(ca);
            var s = new Series("rev")
            {
                ChartType = SeriesChartType.Column,
                Color = PURPLE,
                XValueType = ChartValueType.String,
                BackGradientStyle = GradientStyle.TopBottom,
                BackSecondaryColor = Color.FromArgb(160, 80, 70, 220),
                IsValueShownAsLabel = false
            };
            s["PointWidth"] = "0.55";
            c.Series.Add(s);
            return c;
        }

        private Chart BuildDonutChart()
        {
            var c = new Chart { BackColor = WHITE, BorderlineColor = Color.Transparent };
            var ca = new ChartArea("a") { BackColor = WHITE };
            c.ChartAreas.Add(ca);
            var s = new Series("cat") { ChartType = SeriesChartType.Doughnut, IsValueShownAsLabel = false };
            s["DoughnutRadius"] = "50";
            c.Series.Add(s);
            c.Legends.Add(new Legend { Docking = Docking.Bottom, ForeColor = MUTED, BackColor = WHITE });
            return c;
        }

        private void BuildBottomRow(Panel row)
        {
            var cardTop5 = Card(12);
            var cardOrders = Card(12);

            Action resize = () =>
            {
                if (row.Width == 0) return;
                int gap = 12;
                int leftW = (int)((row.Width - gap) * 0.38f);
                int rightW = row.Width - gap - leftW;
                cardTop5.Bounds = new Rectangle(0, 0, leftW, row.Height);
                cardOrders.Bounds = new Rectangle(leftW + gap, 0, rightW, row.Height);
            };
            row.Resize += (s, e) => resize();
            row.Controls.Add(cardTop5);
            row.Controls.Add(cardOrders);
            resize();

            var hTop = new Label { Text = "🏆  Top 5 món bán chạy nhất", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = TEXT, Dock = DockStyle.Top, Height = 38, Padding = new Padding(14, 10, 0, 0) };
            var subTop = new Label { Text = "Theo số lượng bán trong 30 ngày", Font = new Font("Segoe UI", 8.5F), ForeColor = MUTED, Dock = DockStyle.Top, Height = 22, Padding = new Padding(16, 0, 0, 0) };
            var divTop = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = BORDER };

            flowTop5 = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = WHITE, Padding = new Padding(12, 8, 12, 8) };

            cardTop5.Controls.Add(flowTop5);
            cardTop5.Controls.Add(divTop);
            cardTop5.Controls.Add(subTop);
            cardTop5.Controls.Add(hTop);
            flowTop5.BringToFront();

            var hOrd = new Label { Text = "Giao dịch mới nhất", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = TEXT, Dock = DockStyle.Top, Height = 38, Padding = new Padding(14, 10, 0, 0) };
            var lnkAll = new Label { Text = "Xem tất cả →", Font = new Font("Segoe UI", 9F), ForeColor = PURPLE, AutoSize = true, Cursor = Cursors.Hand, Anchor = AnchorStyles.Right | AnchorStyles.Top };
            hOrd.Controls.Add(lnkAll);
            hOrd.Resize += (s, e) => lnkAll.Location = new Point(hOrd.Width - 110, 11);

            var divOrd = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = BORDER };

            dgv = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = BORDER,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgv.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(249, 249, 252);
            dgv.ThemeStyle.HeaderStyle.ForeColor = MUTED;
            dgv.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ThemeStyle.HeaderStyle.Height = 36;
            dgv.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgv.ThemeStyle.RowsStyle.ForeColor = TEXT;
            dgv.ThemeStyle.RowsStyle.BackColor = WHITE;
            dgv.ThemeStyle.RowsStyle.SelectionBackColor = PURPLES;
            dgv.ThemeStyle.RowsStyle.SelectionForeColor = TEXT;
            dgv.RowTemplate.Height = 40;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            cardOrders.Controls.Add(dgv);
            cardOrders.Controls.Add(divOrd);
            cardOrders.Controls.Add(hOrd);
            dgv.BringToFront();
        }

        // ═══════════════════════════════════════════════════════════════
        //  UI HELPERS
        // ═══════════════════════════════════════════════════════════════
        private Guna2Panel Card(int radius)
        {
            return new Guna2Panel { FillColor = WHITE, BorderRadius = radius, BackColor = Color.Transparent };
        }

        private Label Lbl(string text, Font font, Color fore, Point loc)
        {
            return new Label { Text = text, Font = font, ForeColor = fore, Location = loc, AutoSize = true };
        }

        private Guna2Button Btn(string text, Color fill, Color fore)
        {
            var b = new Guna2Button { Text = text, Font = new Font("Segoe UI", 9F, FontStyle.Bold), FillColor = fill, ForeColor = fore, BorderRadius = 8, BorderThickness = 0, Cursor = Cursors.Hand };
            b.HoverState.FillColor = PURPLE;
            b.HoverState.ForeColor = WHITE;
            return b;
        }

        private GraphicsPath RoundPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad * 2, rad * 2, 180, 90);
            p.AddArc(r.Right - rad * 2, r.Y, rad * 2, rad * 2, 270, 90);
            p.AddArc(r.Right - rad * 2, r.Bottom - rad * 2, rad * 2, rad * 2, 0, 90);
            p.AddArc(r.X, r.Bottom - rad * 2, rad * 2, rad * 2, 90, 90);
            p.CloseAllFigures(); return p;
        }
    }
}