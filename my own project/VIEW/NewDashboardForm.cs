using Guna.UI2.WinForms;
using my_own_project.DAL;
using my_own_project.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace my_own_project.VIEW
{
    public partial class NewDashboardForm : Form
    {
        // ═══════════════════════════════════════════════════════════════
        //  DESIGN TOKENS
        // ═══════════════════════════════════════════════════════════════
        static readonly Color BG = Color.FromArgb(244, 245, 250);
        static readonly Color WHITE = Color.White;
        static readonly Color TEXT = Color.FromArgb(30, 30, 46);
        static readonly Color MUTED = Color.FromArgb(122, 122, 140);
        static readonly Color BORDER = Color.FromArgb(232, 232, 240);
        static readonly Color PURPLE = Color.FromArgb(108, 99, 255);
        static readonly Color PURPLES = Color.FromArgb(238, 237, 254);
        static readonly Color GREEN = Color.FromArgb(34, 197, 94);
        static readonly Color AMBER = Color.FromArgb(245, 158, 11);
        static readonly Color BLUE = Color.FromArgb(59, 130, 246);
        static readonly Color RED = Color.FromArgb(239, 68, 68);

        // ═══════════════════════════════════════════════════════════════
        //  CONTROLS (data-binding targets)
        // ═══════════════════════════════════════════════════════════════
        Label lblRevenue, lblRevSub;
        Label lblOrders, lblOrdSub;
        Label lblAvgOrd, lblAvgSub;
        Label lblInv, lblInvSub;
        Label lblUpdated;

        Chart chartRev, chartCat;
        Label lblRevMsg, lblCatMsg;

        FlowLayoutPanel flowTop5;
        Guna2DataGridView dgv;

        // ═══════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ═══════════════════════════════════════════════════════════════
        public NewDashboardForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            BuildUI();
            this.Load += (s, e) => RefreshAll();
        }

        // ═══════════════════════════════════════════════════════════════
        //  BUILD UI — một Panel cuộn duy nhất chứa các "row" xếp dọc
        // ═══════════════════════════════════════════════════════════════
        void BuildUI()
        {
            // ── TOP BAR (fixed height 52) ────────────────────────────
            var topBar = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = WHITE };
            topBar.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(BORDER), 0, 51, topBar.Width, 51);

            topBar.Controls.Add(Lbl("Dashboard", new Font("Segoe UI", 14F, FontStyle.Bold),
                TEXT, new Point(20, 13)));

            lblUpdated = Lbl("", new Font("Segoe UI", 9F), MUTED, new Point(170, 18));
            topBar.Controls.Add(lblUpdated);

            var btnRefresh = Btn("↻  Làm mới", PURPLES, PURPLE);
            btnRefresh.Size = new Size(112, 30);
            btnRefresh.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnRefresh.Click += (s, e) => RefreshAll();
            topBar.Controls.Add(btnRefresh);
            topBar.Resize += (s, e) => btnRefresh.Location = new Point(topBar.Width - 128, 11);
            this.Controls.Add(topBar);

            // ── SCROLL AREA (fills rest) ─────────────────────────────
            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BG,
                Padding = new Padding(18, 14, 18, 18)
            };
            this.Controls.Add(scroll);
            scroll.BringToFront();

            // ── INNER WRAPPER (Dock=Top → stacks rows top-down) ──────
            var inner = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent
            };
            scroll.Controls.Add(inner);

            // ROW 1 — KPI CARDS (height 114)
            var rowKPI = Row(inner, 114, 0);
            BuildKPIRow(rowKPI);

            // ROW 2 — CHARTS (height 300)
            var rowCharts = Row(inner, 300, 114 + 14);
            BuildChartsRow(rowCharts);

            // ROW 3 — TOP5 + ORDERS (height 340)
            var rowBottom = Row(inner, 340, 114 + 14 + 300 + 14);
            BuildBottomRow(rowBottom);

            // Set inner height so scroll works
            inner.Height = 114 + 14 + 300 + 14 + 340 + 14;
        }

        // Helpers: create a positioned Panel row inside inner
        Panel Row(Panel parent, int h, int top)
        {
            var p = new Panel
            {
                BackColor = Color.Transparent,
                Bounds = new Rectangle(0, top, parent.Width, h),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            parent.Controls.Add(p);
            parent.Resize += (s, e) => p.Width = parent.Width;
            return p;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ROW 1: KPI CARDS
        // ═══════════════════════════════════════════════════════════════
        void BuildKPIRow(Panel row)
        {
            // 4 cards equally spaced with Anchor so they resize
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
                int idx = i; // capture

                var card = Card(12);
                card.Bounds = CardBounds(row.Width, i, 4, row.Height);
                card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                row.Resize += (s, e) => card.Bounds = CardBounds(row.Width, idx, 4, row.Height);

                // Icon box
                var ico = new Label
                {
                    Text = cfg.Icon,
                    Font = new Font("Segoe UI Emoji", 16F),
                    Size = new Size(44, 44),
                    Location = new Point(14, 14),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = cfg.Soft
                };
                ico.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var b = new SolidBrush(cfg.Soft))
                    using (var gp = RoundPath(new Rectangle(0, 0, ico.Width, ico.Height), 10))
                        e.Graphics.FillPath(b, gp);
                };
                card.Controls.Add(ico);

                // Title
                var lTitle = new Label
                {
                    Text = cfg.Title,
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                    ForeColor = MUTED,
                    AutoSize = false,
                    Size = new Size(card.Width - 72, 14),
                    Location = new Point(66, 18),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                };
                card.Controls.Add(lTitle);

                valLabels[idx] = new Label
                {
                    Text = "---",
                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                    ForeColor = cfg.Accent,
                    AutoSize = true,
                    Location = new Point(14, 58)
                };
                card.Controls.Add(valLabels[idx]);

                subLabels[idx] = new Label
                {
                    Text = "Đang tải...",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = MUTED,
                    AutoSize = true,
                    Location = new Point(16, 90)
                };
                card.Controls.Add(subLabels[idx]);
                row.Controls.Add(card);
            }

            lblRevenue = valLabels[0]; lblRevSub = subLabels[0];
            lblOrders = valLabels[1]; lblOrdSub = subLabels[1];
            lblAvgOrd = valLabels[2]; lblAvgSub = subLabels[2];
            lblInv = valLabels[3]; lblInvSub = subLabels[3];
        }

        Rectangle CardBounds(int rowW, int idx, int count, int rowH)
        {
            int gap = 12;
            int total = rowW - gap * (count - 1);
            int w = total / count;
            int x = idx * (w + gap);
            return new Rectangle(x, 0, w + (idx == count - 1 ? total - w * count : 0), rowH);
        }

        // ═══════════════════════════════════════════════════════════════
        //  ROW 2: CHARTS  (Revenue 62% | Category 38%)
        // ═══════════════════════════════════════════════════════════════
        void BuildChartsRow(Panel row)
        {
            // ── Khai báo CẢ HAI card trước lambda để tránh CS0841 ──
            var cardRev = Card(12);
            var cardCat = Card(12);

            // Shared resize logic
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

            // ── Revenue card (left) ──────────────────────────────────
            row.Controls.Add(cardRev);

            var hRev = new Label
            {
                Text = "Doanh thu 7 ngày gần nhất",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TEXT,
                Dock = DockStyle.Top,
                Height = 36,
                Padding = new Padding(14, 10, 0, 0)
            };
            cardRev.Controls.Add(hRev);

            // Wrapper panel chứa chart + message label
            var wrapRev = new Panel { Dock = DockStyle.Fill, BackColor = WHITE };
            cardRev.Controls.Add(wrapRev);
            wrapRev.BringToFront();

            chartRev = BuildBarChart();
            chartRev.Dock = DockStyle.Fill;
            chartRev.BackColor = WHITE;
            wrapRev.Controls.Add(chartRev);

            // Label phải thêm SAU chart rồi BringToFront để nằm trên chart
            lblRevMsg = new Label
            {
                Text = "Đang tải...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = MUTED,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = true
            };
            wrapRev.Controls.Add(lblRevMsg);
            lblRevMsg.BringToFront();

            // ── Category card (right) ────────────────────────────────
            row.Controls.Add(cardCat);

            var hCat = new Label
            {
                Text = "Tỷ trọng doanh thu theo danh mục",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TEXT,
                Dock = DockStyle.Top,
                Height = 36,
                Padding = new Padding(14, 10, 0, 0)
            };
            cardCat.Controls.Add(hCat);

            var wrapCat = new Panel { Dock = DockStyle.Fill, BackColor = WHITE };
            cardCat.Controls.Add(wrapCat);
            wrapCat.BringToFront();

            chartCat = BuildDonutChart();
            chartCat.Dock = DockStyle.Fill;
            chartCat.BackColor = WHITE;
            wrapCat.Controls.Add(chartCat);

            lblCatMsg = new Label
            {
                Text = "Đang tải...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = MUTED,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = true
            };
            wrapCat.Controls.Add(lblCatMsg);
            lblCatMsg.BringToFront();

            // Trigger initial size
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

        Chart BuildBarChart()
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

        Chart BuildDonutChart()
        {
            var c = new Chart { BackColor = WHITE, BorderlineColor = Color.Transparent };
            var ca = new ChartArea("a") { BackColor = WHITE };
            c.ChartAreas.Add(ca);
            var s = new Series("cat")
            {
                ChartType = SeriesChartType.Doughnut,
                IsValueShownAsLabel = false
            };
            s["DoughnutRadius"] = "50";
            c.Series.Add(s);
            c.Legends.Add(new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 8F),
                ForeColor = MUTED,
                BackColor = WHITE
            });
            return c;
        }

        // ═══════════════════════════════════════════════════════════════
        //  ROW 3: TOP5 (38%) | ORDERS TABLE (62%)
        // ═══════════════════════════════════════════════════════════════
        void BuildBottomRow(Panel row)
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

            // ── Top 5 card ───────────────────────────────────────────
            var hTop = new Label
            {
                Text = "🏆  Top 5 món bán chạy nhất",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TEXT,
                Dock = DockStyle.Top,
                Height = 38,
                Padding = new Padding(14, 10, 0, 0)
            };
            var subTop = new Label
            {
                Text = "Theo số lượng bán trong 30 ngày",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MUTED,
                Dock = DockStyle.Top,
                Height = 22,
                Padding = new Padding(16, 0, 0, 0)
            };
            var divTop = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = BORDER };

            flowTop5 = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = WHITE,
                Padding = new Padding(12, 8, 12, 8)
            };

            cardTop5.Controls.Add(flowTop5);
            cardTop5.Controls.Add(divTop);
            cardTop5.Controls.Add(subTop);
            cardTop5.Controls.Add(hTop);
            flowTop5.BringToFront();

            // ── Orders card ──────────────────────────────────────────
            var hOrd = new Label
            {
                Text = "Giao dịch mới nhất",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TEXT,
                Dock = DockStyle.Top,
                Height = 38,
                Padding = new Padding(14, 10, 0, 0)
            };
            var lnkAll = new Label
            {
                Text = "Xem tất cả →",
                Font = new Font("Segoe UI", 9F),
                ForeColor = PURPLE,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
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
        //  SHARED UI HELPERS
        // ═══════════════════════════════════════════════════════════════
        Guna2Panel Card(int radius)
        {
            return new Guna2Panel
            {
                FillColor = WHITE,
                BorderRadius = radius,
                BackColor = Color.Transparent
            };
        }

        Label Lbl(string text, Font font, Color fore, Point loc)
        {
            return new Label { Text = text, Font = font, ForeColor = fore, Location = loc, AutoSize = true };
        }

        Guna2Button Btn(string text, Color fill, Color fore)
        {
            var b = new Guna2Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FillColor = fill,
                ForeColor = fore,
                BorderRadius = 8,
                BorderThickness = 0,
                Cursor = Cursors.Hand
            };
            b.HoverState.FillColor = PURPLE;
            b.HoverState.ForeColor = WHITE;
            return b;
        }

        GraphicsPath RoundPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad * 2, rad * 2, 180, 90);
            p.AddArc(r.Right - rad * 2, r.Y, rad * 2, rad * 2, 270, 90);
            p.AddArc(r.Right - rad * 2, r.Bottom - rad * 2, rad * 2, rad * 2, 0, 90);
            p.AddArc(r.X, r.Bottom - rad * 2, rad * 2, rad * 2, 90, 90);
            p.CloseAllFigures(); return p;
        }

        string Short(string msg, int max) =>
            string.IsNullOrEmpty(msg) ? "" :
            msg.Length <= max ? msg : msg.Substring(0, max) + "...";

        string FindCol(DataTable dt, params string[] names)
        {
            foreach (var n in names) if (dt.Columns.Contains(n)) return n;
            return dt.Columns.Count > 0 ? dt.Columns[0].ColumnName : "";
        }

        // ═══════════════════════════════════════════════════════════════
        //  DATA LOADING
        // ═══════════════════════════════════════════════════════════════
        void RefreshAll()
        {
            lblUpdated.Text = "Cập nhật lúc " + DateTime.Now.ToString("HH:mm:ss");
            LoadKPIs();
            LoadRevChart();
            LoadCatChart();
            LoadTop5();
            LoadOrders();
        }

        void LoadKPIs()
        {
            try
            {
                var p = Params(DateTime.Today, DateTime.Today.AddDays(1).AddSeconds(-1));
                var dt = DataHelper.ExecuteSPGetTable("sp_Dashboard_GetSummary", p);

                decimal rev = 0; int ord = 0;
                if (dt?.Rows.Count > 0)
                {
                    var r = dt.Rows[0];
                    if (dt.Columns.Contains("TotalRevenue") && r["TotalRevenue"] != DBNull.Value)
                        rev = Convert.ToDecimal(r["TotalRevenue"]);
                    if (dt.Columns.Contains("TotalOrders") && r["TotalOrders"] != DBNull.Value)
                        ord = Convert.ToInt32(r["TotalOrders"]);
                }
                lblRevenue.Text = rev.ToString("N0") + " đ";
                lblRevSub.Text = rev > 0 ? "Doanh thu hôm nay" : "Chưa có đơn";
                lblOrders.Text = ord + " đơn";
                lblOrdSub.Text = "Tổng hôm nay";
                decimal avg = ord > 0 ? rev / ord : 0;
                lblAvgOrd.Text = avg.ToString("N0") + " đ";
                lblAvgSub.Text = ord > 0 ? "Trung bình / hóa đơn" : "Chưa có đơn";
            }
            catch (Exception ex)
            {
                lblRevenue.Text = lblOrders.Text = lblAvgOrd.Text = "Lỗi";
                lblRevSub.Text = Short(ex.Message, 45);
            }
            // Inventory chưa làm
            try
            {
                int lowStockCount = IngredientBLL.GetLowStockCount();

                if (lowStockCount > 0)
                {
                    lblInv.Text = lowStockCount + " mặt hàng";
                    lblInv.ForeColor = RED;
                    lblInvSub.Text = "Cần nhập thêm";
                }
                else
                {
                    lblInv.Text = "Ổn định";
                    lblInv.ForeColor = GREEN;
                    lblInvSub.Text = "Kho đang đủ hàng";
                }
            }
            catch
            {
                lblInv.Text = "--";
                lblInv.ForeColor = MUTED;
                lblInvSub.Text = "Không thể kiểm tra kho";
            }
        }

        void LoadRevChart()
        {
            lblRevMsg.Text = "Đang tải..."; lblRevMsg.Visible = true;
            try
            {
                var dt = DataHelper.ExecuteSPGetTable("sp_Dashboard_GetRevenueByDate",
                    Params(DateTime.Today.AddDays(-6), DateTime.Today.AddDays(1)));

                chartRev.Series["rev"].Points.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    string dc = FindCol(dt, "OrderDate", "Date", "CreatedAt");
                    string rc = FindCol(dt, "Revenue", "TotalRevenue", "TotalAmount");
                    foreach (DataRow row in dt.Rows)
                    {
                        string lbl = DateTime.TryParse(row[dc].ToString(), out var d)
                            ? d.ToString("dd/MM") : row[dc].ToString();
                        decimal rev = row[rc] == DBNull.Value ? 0 : Convert.ToDecimal(row[rc]);
                        chartRev.Series["rev"].Points.AddXY(lbl, rev);
                    }
                    // Ẩn message, hiện chart
                    lblRevMsg.Visible = false;
                    chartRev.Visible = true;
                    chartRev.Invalidate();
                    chartRev.Update();
                }
                else
                {
                    chartRev.Visible = false;
                    lblRevMsg.Text = "Không có dữ liệu 7 ngày qua";
                    lblRevMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                chartRev.Visible = false;
                lblRevMsg.Text = "Lỗi: " + Short(ex.Message, 60);
                lblRevMsg.Visible = true;
            }
        }

        void LoadCatChart()
        {
            lblCatMsg.Text = "Đang tải..."; lblCatMsg.Visible = true;
            try
            {
                var dt = DataHelper.ExecuteSPGetTable("sp_Dashboard_GetCategoryRevenueShare",
                    Params(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1)));

                chartCat.Series["cat"].Points.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    string nc = FindCol(dt, "CategoryName", "Name");
                    string vc = FindCol(dt, "Revenue", "TotalRevenue", "Quantity");
                    Color[] pal = { PURPLE, BLUE, GREEN, AMBER, RED, Color.FromArgb(168, 85, 247) };
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        double v = row[vc] == DBNull.Value ? 0 : Convert.ToDouble(row[vc]);
                        if (v <= 0) continue;
                        int idx = chartCat.Series["cat"].Points.AddXY(row[nc]?.ToString() ?? "?", v);
                        chartCat.Series["cat"].Points[idx].Color = pal[i++ % pal.Length];
                    }
                    if (chartCat.Series["cat"].Points.Count > 0)
                    {
                        lblCatMsg.Visible = false;
                        chartCat.Visible = true;
                        chartCat.Invalidate();
                        chartCat.Update();
                    }
                    else
                    {
                        chartCat.Visible = false;
                        lblCatMsg.Text = "Không có dữ liệu";
                        lblCatMsg.Visible = true;
                    }
                }
                else
                {
                    chartCat.Visible = false;
                    lblCatMsg.Text = "Không có dữ liệu 30 ngày";
                    lblCatMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                chartCat.Visible = false;
                lblCatMsg.Text = "Lỗi: " + Short(ex.Message, 60);
                lblCatMsg.Visible = true;
            }
        }

        void LoadTop5()
        {
            flowTop5?.Controls.Clear();
            try
            {
                DataTable dt;
                try
                {
                    dt = DataHelper.ExecuteSPGetTable("sp_Dashboard_GetTop5Products",
                        Params(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1)));
                }
                catch
                {
                    dt = DataHelper.ExecuteSPGetTable("sp_Dashboard_GetTopProducts",
                        Params(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1)));
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    flowTop5.Controls.Add(Lbl("Chưa có dữ liệu bán hàng 30 ngày.",
                        new Font("Segoe UI", 9.5F), MUTED, new Point(0, 8)));
                    return;
                }

                string nc = FindCol(dt, "ProductName", "MenuItemName", "ItemName", "FoodName", "Name");
                string qc = FindCol(dt, "TotalQuantity", "Quantity", "Count", "SoldQuantity");
                string rc = FindCol(dt, "Revenue", "TotalRevenue", "TotalAmount", "Amount");

                Color[] rankColors = { AMBER, PURPLE, BLUE, GREEN, MUTED };

                int rank = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (rank >= 5) break;
                    string name = row[nc]?.ToString() ?? "?";
                    int qty = row[qc] == DBNull.Value ? 0 : Convert.ToInt32(row[qc]);
                    decimal rev = string.IsNullOrEmpty(rc) || row[rc] == DBNull.Value
                        ? 0 : Convert.ToDecimal(row[rc]);

                    int w = Math.Max(220, flowTop5.ClientSize.Width - 28);
                    var item = new Panel
                    {
                        Size = new Size(w, 54),
                        BackColor = WHITE,
                        Margin = new Padding(0, 0, 0, 6)
                    };
                    flowTop5.Controls.Add(item);
                    flowTop5.Resize += (s, e) =>
                        item.Width = Math.Max(220, flowTop5.ClientSize.Width - 28);

                    // Rank badge
                    var badge = new Label
                    {
                        Text = "#" + (rank + 1),
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = rankColors[rank],
                        BackColor = Color.Transparent,
                        Size = new Size(34, 24),
                        Location = new Point(0, 10),
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    // Name
                    var lName = new Label
                    {
                        Text = name,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        ForeColor = TEXT,
                        AutoEllipsis = true,
                        Size = new Size(w - 150, 20),
                        Location = new Point(36, 4)
                    };
                    // Qty sub
                    var lQty = new Label
                    {
                        Text = qty.ToString("N0") + " đã bán",
                        Font = new Font("Segoe UI", 8.5F),
                        ForeColor = MUTED,
                        AutoSize = true,
                        Location = new Point(36, 26)
                    };
                    // Revenue right
                    var lRev = new Label
                    {
                        Text = rev > 0 ? rev.ToString("N0") + " đ" : "",
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = GREEN,
                        Size = new Size(105, 20),
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(w - 112, 14),
                        Anchor = AnchorStyles.Top | AnchorStyles.Right
                    };
                    // Divider
                    var div = new Panel { BackColor = BORDER, Size = new Size(w - 36, 1), Location = new Point(36, 52) };

                    item.Controls.AddRange(new Control[] { badge, lName, lQty, lRev, div });
                    rank++;
                }
            }
            catch (Exception ex)
            {
                flowTop5?.Controls.Clear();
                flowTop5?.Controls.Add(Lbl("Lỗi: " + Short(ex.Message, 80),
                    new Font("Segoe UI", 9F), RED, new Point(0, 8)));
            }
        }

        void LoadOrders()
        {
            try
            {
                var dt = DataHelper.ExecuteSPGetTable("sp_Dashboard_GetRecentOrders",
                    Params(DateTime.Today.AddDays(-15), DateTime.Today.AddDays(1)));
                if (dt == null) return;

                foreach (var c in new[] { "Product", "Notes", "RestaurantID" })
                    if (dt.Columns.Contains(c)) dt.Columns.Remove(c);

                dgv.DataSource = dt;

                var hmap = new Dictionary<string, string>
                {
                    {"OrderID","Mã HĐ"}, {"Customer","Khách hàng"}, {"CustomerName","Khách hàng"},
                    {"TableNumber","Bàn"}, {"OrderDate","Ngày đặt"}, {"OrderType","Loại"},
                    {"TotalAmount","Tổng tiền (đ)"}, {"Total","Tổng tiền (đ)"}, {"Status","Trạng thái"}
                };
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (hmap.ContainsKey(col.Name)) col.HeaderText = hmap[col.Name];
                    if (col.Name == "Total" || col.Name == "TotalAmount")
                    {
                        col.DefaultCellStyle.Format = "N0";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!dgv.Columns.Contains("Status")) break;
                    var cell = row.Cells["Status"];
                    if (cell?.Value == null) continue;
                    switch (cell.Value.ToString())
                    {
                        case "Completed":
                        case "Paid":
                            cell.Value = "✓ Đã thanh toán";
                            cell.Style.ForeColor = GREEN;
                            cell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); break;
                        case "Pending":
                        case "Processing":
                        case "Open":
                            cell.Value = "⏳ Chờ xử lý";
                            cell.Style.ForeColor = AMBER;
                            cell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); break;
                        case "Cancelled":
                        case "Canceled":
                            cell.Value = "✕ Đã huỷ";
                            cell.Style.ForeColor = RED;
                            cell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold); break;
                    }
                }
            }
            catch (Exception ex)
            {
                dgv.DataSource = null;
                var err = new DataTable(); err.Columns.Add("Lỗi");
                err.Rows.Add(ex.Message); dgv.DataSource = err;
            }
        }

        SqlParameter[] Params(DateTime start, DateTime end) => new SqlParameter[]
        {
            new SqlParameter("@StartDate", start),
            new SqlParameter("@EndDate",   end)
        };
    }
}