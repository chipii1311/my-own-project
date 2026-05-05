using Guna.UI2.WinForms;
using System;
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
        // ================================================================
        // DESIGN TOKENS — đổi màu toàn form chỉ cần sửa ở đây
        // ================================================================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_GREEN_SOFT = Color.FromArgb(220, 252, 231);
        private static readonly Color C_AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color C_AMBER_SOFT = Color.FromArgb(254, 243, 199);
        private static readonly Color C_BLUE = Color.FromArgb(59, 130, 246);
        private static readonly Color C_BLUE_SOFT = Color.FromArgb(219, 234, 254);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color C_BORDER = Color.FromArgb(232, 232, 240);

        // ================================================================
        // CONTROLS
        // ================================================================
        private Label lblRevenue, lblRevDelta;
        private Label lblOrders, lblOrdDelta;
        private Label lblTopItem, lblTopSub;
        private Label lblTables, lblTableSub;
        private Chart chartRevenue, chartCategory;
        private Guna2DataGridView dgvRecentOrders;
        private Label lblLoadingRevenue, lblLoadingCategory;
        private Guna2Button btnRefresh;
        private Label lblLastUpdated;

        public NewDashboardForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildDashboardUI();
            this.Load += (s, e) => RefreshAll();
        }

        // ================================================================
        #region UI BUILDER
        // ================================================================

        private void BuildDashboardUI()
        {
            this.SuspendLayout();

            // ---- TOP BAR ----
            Panel topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = C_WHITE,
                Padding = new Padding(20, 0, 20, 0)
            };
            // Bottom border của topbar
            topBar.Paint += (s, e) =>
            {
                using (var pen = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(pen, 0, topBar.Height - 1, topBar.Width, topBar.Height - 1);
            };

            Label lblPageTitle = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(20, 14)
            };
            topBar.Controls.Add(lblPageTitle);

            lblLastUpdated = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(180, 19)
            };
            topBar.Controls.Add(lblLastUpdated);

            // FIX #3: Dùng Anchor + Margin thay vì Location cứng.
            // Location = Point(topBar.Width - 130, 12) bị tính lúc khởi tạo
            // khi topBar.Width = 0 → button xuất hiện ở x=-130 (ngoài màn hình).
            btnRefresh = new Guna2Button
            {
                Text = "↻  Làm mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FillColor = C_PURPLE_SOFT,
                ForeColor = C_PURPLE,
                BorderRadius = 8,
                BorderThickness = 0,
                Size = new Size(110, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Margin = new Padding(0, 12, 20, 0),
                Cursor = Cursors.Hand
            };
            btnRefresh.HoverState.FillColor = C_PURPLE;
            btnRefresh.HoverState.ForeColor = Color.White;
            btnRefresh.Click += (s, e) => RefreshAll();
            // Đặt vào FlowLayoutPanel phụ căn phải thay vì add thẳng vào Panel
            var topBarRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 12, 20, 0),
                WrapContents = false
            };
            topBarRight.Controls.Add(btnRefresh);
            topBar.Controls.Add(topBarRight);

            this.Controls.Add(topBar);

            // ---- MAIN SCROLL AREA ----
            Panel scrollArea = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = C_BG,
                Padding = new Padding(20, 16, 20, 20)
            };
            this.Controls.Add(scrollArea);
            scrollArea.BringToFront();

            // Inner layout
            TableLayoutPanel tlpMain = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent
            };
            // FIX #2: tăng KPI row từ 140 → 160 để lblSub không bị cắt
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F)); // KPI row
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));  // spacer
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F)); // Charts
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));  // spacer

            // ROW 0 — KPI CARDS
            tlpMain.Controls.Add(BuildKPIRow(), 0, 0);

            // ROW 2 — CHARTS
            tlpMain.Controls.Add(BuildChartsRow(), 0, 2);

            // FIX #1: Add ordersCard TRƯỚC tlpMain vào scrollArea.
            // Với Dock=Top, control Add CUỐI sẽ nằm TRÊN cùng.
            // Thứ tự Add đúng: ordersCard trước → tlpMain sau → tlpMain nằm trên ordersCard.
            var ordersCard = BuildOrdersTable();
            ordersCard.Dock = DockStyle.Top;
            ordersCard.Height = 360;
            scrollArea.Controls.Add(ordersCard); // add trước = nằm dưới
            scrollArea.Controls.Add(tlpMain);    // add sau  = nằm trên (KPI + Charts)

            this.ResumeLayout(false);
        }

        // ---- KPI ROW ----
        private TableLayoutPanel BuildKPIRow()
        {
            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 4; i++)
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            // Card 1 — Revenue
            var c1 = MakeKPICard("DOANH THU HÔM NAY", C_PURPLE, C_PURPLE_SOFT, "💰",
                out lblRevenue, out lblRevDelta);
            lblRevenue.Text = "---";
            lblRevDelta.Text = "Đang tải...";

            // Card 2 — Orders
            var c2 = MakeKPICard("TỔNG ĐƠN HÔM NAY", C_BLUE, C_BLUE_SOFT, "🧾",
                out lblOrders, out lblOrdDelta);
            lblOrders.Text = "---";
            lblOrdDelta.Text = "Đang tải...";

            // Card 3 — Top item
            var c3 = MakeKPICard("MÓN BÁN CHẠY NHẤT", C_AMBER, C_AMBER_SOFT, "⭐",
                out lblTopItem, out lblTopSub);
            lblTopItem.Text = "---";
            lblTopSub.Text = "Trong 30 ngày";

            // Card 4 — Low stock
            var c4 = MakeKPICard("KHO CẦN NHẬP THÊM", C_RED, Color.FromArgb(254, 226, 226), "⚠️",
                out lblTables, out lblTableSub);
            lblTables.Text = "---";
            lblTableSub.Text = "Mặt hàng sắp hết";

            tlp.Controls.Add(c1, 0, 0);
            tlp.Controls.Add(c2, 1, 0);
            tlp.Controls.Add(c3, 2, 0);
            tlp.Controls.Add(c4, 3, 0);
            return tlp;
        }

        private Guna2Panel MakeKPICard(string title, Color accent, Color softBg, string icon,
            out Label lblValue, out Label lblSub)
        {
            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Margin = new Padding(6),
                Padding = new Padding(16)
            };
            // Đổ bóng nhẹ
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.FromArgb(12, 0, 0, 0)))
                    g.FillRectangle(brush, 4, card.Height - 3, card.Width - 8, 3);
            };

            // Icon badge
            var iconBox = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 18F),
                BackColor = softBg,
                Size = new Size(44, 44),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(16, 16)
            };
            iconBox.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(softBg))
                {
                    var path = RoundPath(new Rectangle(0, 0, iconBox.Width, iconBox.Height), 10);
                    g.FillPath(brush, path);
                }
            };
            card.Controls.Add(iconBox);

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = C_MUTED,
                AutoSize = false,
                Size = new Size(card.Width - 80, 16),
                Location = new Point(68, 18),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(lblTitle);

            lblValue = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = accent,
                AutoSize = true,
                Location = new Point(16, 60)   // FIX #2: giữ nguyên Y=60
            };
            card.Controls.Add(lblValue);

            lblSub = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(18, 96)   // FIX #2: giữ nguyên Y=96
            };
            card.Controls.Add(lblSub);

            // FIX #2: tăng card.Height từ 130 → 148 để lblSub (Y=96+16=112) không bị cắt
            card.Height = 148;
            return card;
        }

        // ---- CHARTS ROW ----
        private TableLayoutPanel BuildChartsRow()
        {
            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));

            // Chart 1 — Bar revenue
            var card1 = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Margin = new Padding(6, 0, 6, 0),
                Padding = new Padding(16)
            };

            var hdr1 = new Label
            {
                Text = "Doanh thu 7 ngày gần nhất",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(16, 12),
                Dock = DockStyle.Top,
                Height = 32
            };
            card1.Controls.Add(hdr1);

            chartRevenue = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                BorderlineColor = Color.Transparent
            };
            var ca1 = new ChartArea("main");
            ca1.BackColor = Color.Transparent;
            ca1.AxisX.LineColor = C_BORDER;
            ca1.AxisY.LineColor = C_BORDER;
            ca1.AxisX.MajorGrid.LineColor = Color.Transparent;
            ca1.AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 248);
            ca1.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            ca1.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            ca1.AxisX.LabelStyle.ForeColor = C_MUTED;
            ca1.AxisY.LabelStyle.ForeColor = C_MUTED;
            ca1.AxisY.LabelStyle.Format = "N0";
            chartRevenue.ChartAreas.Add(ca1);

            var s1 = new Series("rev")
            {
                ChartType = SeriesChartType.Column,
                Color = C_PURPLE,
                IsValueShownAsLabel = false,
                XValueType = ChartValueType.String,
                BackGradientStyle = GradientStyle.TopBottom,
                BackSecondaryColor = Color.FromArgb(150, 90, 80, 230)
            };
            s1["PointWidth"] = "0.6";
            chartRevenue.Series.Add(s1);

            // Label loading thay thế biểu đồ khi chưa có dữ liệu (nằm đè lên, sẽ ẩn khi có dữ liệu)
            lblLoadingRevenue = new Label
            {
                Text = "Đang tải dữ liệu...",
                Font = new Font("Segoe UI", 11F),
                ForeColor = C_MUTED,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                BackColor = Color.Transparent,
                Visible = false   // mặc định ẩn
            };
            card1.Controls.Add(lblLoadingRevenue);
            card1.Controls.Add(chartRevenue);
            // Đưa label loading lên trên cùng (nhưng khi Visible = false sẽ không che)
            lblLoadingRevenue.BringToFront();
            tlp.Controls.Add(card1, 0, 0);

            // Chart 2 — Donut category
            var card2 = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Margin = new Padding(6, 0, 0, 0),
                Padding = new Padding(16)
            };

            var hdr2 = new Label
            {
                Text = "Tỷ trọng theo danh mục",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(16, 12),
                Dock = DockStyle.Top,
                Height = 32
            };
            card2.Controls.Add(hdr2);

            chartCategory = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                BorderlineColor = Color.Transparent
            };
            var ca2 = new ChartArea("main") { BackColor = Color.Transparent };
            chartCategory.ChartAreas.Add(ca2);

            var s2 = new Series("cat")
            {
                ChartType = SeriesChartType.Doughnut,
                IsValueShownAsLabel = true,
                LabelFormat = "#,0"
            };
            s2["DoughnutRadius"] = "55";
            s2["PieLabelStyle"] = "Outside";
            s2["PieDrawingStyle"] = "Default";
            chartCategory.Series.Add(s2);

            var legend = new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 8F),
                ForeColor = C_MUTED,
                BackColor = Color.Transparent
            };
            chartCategory.Legends.Add(legend);

            lblLoadingCategory = new Label
            {
                Text = "Đang tải dữ liệu...",
                Font = new Font("Segoe UI", 11F),
                ForeColor = C_MUTED,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                BackColor = Color.Transparent,
                Visible = false
            };
            card2.Controls.Add(lblLoadingCategory);
            card2.Controls.Add(chartCategory);
            lblLoadingCategory.BringToFront();
            tlp.Controls.Add(card2, 1, 0);

            return tlp;
        }

        // ---- ORDERS TABLE ----
        private Guna2Panel BuildOrdersTable()
        {
            var card = new Guna2Panel
            {
                FillColor = C_WHITE,
                BorderRadius = 12,
                Margin = new Padding(6),
                Padding = new Padding(0),
                Dock = DockStyle.Top,
                Height = 360  // tăng lên để thấy rõ nhiều dòng
            };

            // Header bar
            var hdrBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent
            };
            var lblTitle = new Label
            {
                Text = "Giao dịch mới nhất",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(16, 12)
            };
            hdrBar.Controls.Add(lblTitle);

            // "Xem tất cả" link
            var lnkAll = new Label
            {
                Text = "Xem tất cả →",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(card.Width - 110, 14)
            };
            hdrBar.Controls.Add(lnkAll);
            hdrBar.Resize += (s, e) => lnkAll.Location = new Point(hdrBar.Width - 110, 14);

            // Divider line
            var divider = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = C_BORDER
            };

            dgvRecentOrders = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = C_BORDER,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dgvRecentOrders.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(249, 249, 252);
            dgvRecentOrders.ThemeStyle.HeaderStyle.ForeColor = C_MUTED;
            dgvRecentOrders.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvRecentOrders.ThemeStyle.HeaderStyle.Height = 36;
            dgvRecentOrders.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgvRecentOrders.ThemeStyle.RowsStyle.ForeColor = C_TEXT;
            dgvRecentOrders.ThemeStyle.RowsStyle.BackColor = C_WHITE;
            dgvRecentOrders.ThemeStyle.RowsStyle.SelectionBackColor = C_PURPLE_SOFT;
            dgvRecentOrders.ThemeStyle.RowsStyle.SelectionForeColor = C_TEXT;
            dgvRecentOrders.RowTemplate.Height = 42;
            dgvRecentOrders.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            card.Controls.Add(dgvRecentOrders);
            card.Controls.Add(divider);
            card.Controls.Add(hdrBar);
            dgvRecentOrders.BringToFront();

            return card;
        }

        // ---- Utility: rounded path ----
        private GraphicsPath RoundPath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        #endregion

        // ================================================================
        #region DATA LOADING  (gọi đúng SP có trong database)
        // ================================================================

        private void RefreshAll()
        {
            lblLastUpdated.Text = "Cập nhật lúc " + DateTime.Now.ToString("HH:mm:ss");
            LoadKPIs();
            LoadRevenueChart();
            LoadCategoryChart();
            LoadRecentOrders();
        }

        // ----------------------------------------------------------------
        // KPI — dùng sp_Dashboard_GetSummary (đúng tên trong DB)
        // ----------------------------------------------------------------
        private void LoadKPIs()
        {
            try
            {
                // Summary cho hôm nay
                DataTable dt = DAL.DataHelper.ExecuteSPGetTable(
                    "sp_Dashboard_GetSummary",
                    new SqlParameter[]
                    {
                        new SqlParameter("@StartDate", DateTime.Today),
                        new SqlParameter("@EndDate",   DateTime.Today.AddDays(1).AddSeconds(-1))
                    });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];

                    // Revenue
                    if (dt.Columns.Contains("TotalRevenue"))
                    {
                        decimal rev = Convert.ToDecimal(r["TotalRevenue"]);
                        lblRevenue.Text = rev.ToString("N0") + " đ";
                        lblRevDelta.Text = rev > 0 ? "Hôm nay" : "Chưa có đơn";
                    }
                    // Orders
                    if (dt.Columns.Contains("TotalOrders"))
                    {
                        int ord = Convert.ToInt32(r["TotalOrders"]);
                        lblOrders.Text = ord.ToString() + " đơn";
                        lblOrdDelta.Text = "Tổng hôm nay";
                    }
                }
                else
                {
                    lblRevenue.Text = "0 đ";
                    lblOrders.Text = "0 đơn";
                    lblRevDelta.Text = lblOrdDelta.Text = "Chưa có dữ liệu";
                }

                // Top product — dùng sp_Dashboard_GetTopProducts (30 ngày)
                DataTable dtTop = DAL.DataHelper.ExecuteSPGetTable(
                    "sp_Dashboard_GetTopProducts",
                    new SqlParameter[]
                    {
                        new SqlParameter("@StartDate", DateTime.Today.AddDays(-30)),
                        new SqlParameter("@EndDate",   DateTime.Today.AddDays(1))
                    });

                if (dtTop != null && dtTop.Rows.Count > 0)
                {
                    string nameCol = dtTop.Columns.Contains("MenuItemName") ? "MenuItemName"
                                   : dtTop.Columns.Contains("ItemName") ? "ItemName"
                                   : dtTop.Columns[0].ColumnName;
                    lblTopItem.Text = dtTop.Rows[0][nameCol].ToString();
                    lblTopSub.Text = "Bán chạy nhất 30 ngày";
                }
                else
                {
                    lblTopItem.Text = "Chưa có dữ liệu";
                    lblTopSub.Text = "Trong 30 ngày";
                }

                // Low stock — dùng sp_Ingredient_GetLowStock
                try
                {
                    DataTable dtLow = DAL.DataHelper.ExecuteSPGetTable("sp_Ingredient_GetLowStock", null);
                    int count = dtLow?.Rows.Count ?? 0;
                    lblTables.Text = count > 0 ? count + " mặt hàng" : "Đủ hàng";
                    lblTableSub.Text = count > 0 ? "⚠ Cần nhập thêm" : "✓ Kho ổn định";
                    if (count > 0) lblTables.ForeColor = C_RED;
                    else lblTables.ForeColor = C_GREEN;
                }
                catch { lblTables.Text = "--"; lblTableSub.Text = "Không thể kiểm tra"; }
            }
            catch (Exception ex)
            {
                lblRevenue.Text = "Lỗi";
                lblOrders.Text = "Lỗi";
                lblRevDelta.Text = lblOrdDelta.Text = ex.Message.Length > 40
                    ? ex.Message.Substring(0, 40) + "..." : ex.Message;
            }
        }

        // ----------------------------------------------------------------
        // Revenue Chart — sp_Dashboard_GetRevenueByDate
        // ----------------------------------------------------------------
        private void LoadRevenueChart()
        {
            lblLoadingRevenue.Text = "Đang tải dữ liệu...";
            lblLoadingRevenue.Visible = true;
            try
            {
                DataTable dt = DAL.DataHelper.ExecuteSPGetTable(
                    "sp_Dashboard_GetRevenueByDate",
                    new SqlParameter[]
                    {
                        new SqlParameter("@StartDate", DateTime.Today.AddDays(-6)),
                        new SqlParameter("@EndDate",   DateTime.Today.AddDays(1))
                    });

                chartRevenue.Series["rev"].Points.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    string dateCol = dt.Columns.Contains("OrderDate") ? "OrderDate"
                                   : dt.Columns.Contains("Date") ? "Date"
                                   : dt.Columns[0].ColumnName;
                    string revCol = dt.Columns.Contains("Revenue") ? "Revenue"
                                   : dt.Columns.Contains("TotalRevenue") ? "TotalRevenue"
                                   : dt.Columns[1].ColumnName;

                    foreach (DataRow row in dt.Rows)
                    {
                        string label = "";
                        if (row[dateCol] != DBNull.Value)
                        {
                            if (DateTime.TryParse(row[dateCol].ToString(), out DateTime d))
                                label = d.ToString("dd/MM");
                            else
                                label = row[dateCol].ToString();
                        }
                        decimal rev = row[revCol] == DBNull.Value ? 0 : Convert.ToDecimal(row[revCol]);
                        chartRevenue.Series["rev"].Points.AddXY(label, rev);
                    }
                    lblLoadingRevenue.Visible = false;
                }
                else
                {
                    lblLoadingRevenue.Text = "Không có dữ liệu 7 ngày qua";
                }
            }
            catch (Exception ex)
            {
                lblLoadingRevenue.Text = "Lỗi: " + (ex.Message.Length > 60 ? ex.Message.Substring(0, 60) : ex.Message);
            }
        }

        // ----------------------------------------------------------------
        // Category Chart — sp_Dashboard_GetTopProducts làm proxy
        // ----------------------------------------------------------------
        private void LoadCategoryChart()
        {
            lblLoadingCategory.Text = "Đang tải dữ liệu...";
            lblLoadingCategory.Visible = true;
            try
            {
                DataTable dt = DAL.DataHelper.ExecuteSPGetTable(
                    "sp_Dashboard_GetTopProducts",
                    new SqlParameter[]
                    {
                        new SqlParameter("@StartDate", DateTime.Today.AddDays(-30)),
                        new SqlParameter("@EndDate",   DateTime.Today.AddDays(1))
                    });

                chartCategory.Series["cat"].Points.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    string nameCol = dt.Columns.Contains("MenuItemName") ? "MenuItemName"
                                   : dt.Columns.Contains("ItemName") ? "ItemName"
                                   : dt.Columns.Contains("CategoryName") ? "CategoryName"
                                   : dt.Columns[0].ColumnName;
                    string valCol = dt.Columns.Contains("Revenue") ? "Revenue"
                                   : dt.Columns.Contains("TotalQuantity") ? "TotalQuantity"
                                   : dt.Columns.Contains("Quantity") ? "Quantity"
                                   : dt.Columns[dt.Columns.Count - 1].ColumnName;

                    Color[] palette = new[]
                    {
                        C_PURPLE, C_BLUE, C_GREEN, C_AMBER, C_RED,
                        Color.FromArgb(168, 85, 247)
                    };
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (i >= 6) break;
                        string name = row[nameCol]?.ToString() ?? "?";
                        double val = row[valCol] == DBNull.Value ? 0 : Convert.ToDouble(row[valCol]);
                        if (val <= 0) { i++; continue; }
                        int idx = chartCategory.Series["cat"].Points.AddXY(name, val);
                        chartCategory.Series["cat"].Points[idx].Color = palette[i % palette.Length];
                        i++;
                    }

                    if (chartCategory.Series["cat"].Points.Count > 0)
                        lblLoadingCategory.Visible = false;
                    else
                        lblLoadingCategory.Text = "Không có dữ liệu";
                }
                else
                {
                    lblLoadingCategory.Text = "Không có dữ liệu 30 ngày";
                }
            }
            catch (Exception ex)
            {
                lblLoadingCategory.Text = "Lỗi: " + (ex.Message.Length > 50 ? ex.Message.Substring(0, 50) : ex.Message);
            }
        }

        // ----------------------------------------------------------------
        // Recent Orders — sp_Dashboard_GetRecentOrders
        // ----------------------------------------------------------------
        private void LoadRecentOrders()
        {
            try
            {
                DataTable dt = DAL.DataHelper.ExecuteSPGetTable(
                    "sp_Dashboard_GetRecentOrders",
                    new SqlParameter[]
                    {
                        new SqlParameter("@StartDate", DateTime.Today.AddDays(-15)),
                        new SqlParameter("@EndDate",   DateTime.Today.AddDays(1))
                    });

                if (dt == null) return;

                // Xoá cột không cần
                foreach (string col in new[] { "Product", "Notes", "RestaurantID" })
                    if (dt.Columns.Contains(col)) dt.Columns.Remove(col);

                dgvRecentOrders.DataSource = dt;

                // Đổi tên header cho thân thiện
                var headerMap = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "OrderID",       "Mã HĐ"           },
                    { "Customer",      "Khách hàng"       },
                    { "CustomerName",  "Khách hàng"       },
                    { "Total",         "Tổng tiền (đ)"    },
                    { "TotalAmount",   "Tổng tiền (đ)"    },
                    { "Status",        "Trạng thái"       },
                    { "OrderDate",     "Ngày đặt"         },
                    { "CreatedAt",     "Thời gian"        },
                    { "TableNumber",   "Bàn số"           },
                    { "PaymentMethod", "Thanh toán"       }
                };
                foreach (DataGridViewColumn col in dgvRecentOrders.Columns)
                    if (headerMap.ContainsKey(col.Name)) col.HeaderText = headerMap[col.Name];

                // Format tiền
                foreach (DataGridViewColumn col in dgvRecentOrders.Columns)
                    if (col.Name == "Total" || col.Name == "TotalAmount")
                        col.DefaultCellStyle.Format = "N0";

                // Dịch + tô màu Status
                foreach (DataGridViewRow row in dgvRecentOrders.Rows)
                {
                    DataGridViewCell statusCell = null;
                    if (dgvRecentOrders.Columns.Contains("Status"))
                        statusCell = row.Cells["Status"];

                    if (statusCell?.Value != null)
                    {
                        switch (statusCell.Value.ToString())
                        {
                            case "Completed":
                                statusCell.Value = "✓ Đã thanh toán";
                                statusCell.Style.ForeColor = C_GREEN;
                                statusCell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                                break;
                            case "Pending":
                                statusCell.Value = "⏳ Chờ xử lý";
                                statusCell.Style.ForeColor = C_AMBER;
                                statusCell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                                break;
                            case "Cancelled":
                            case "Canceled":
                                statusCell.Value = "✕ Đã huỷ";
                                statusCell.Style.ForeColor = C_RED;
                                statusCell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dgvRecentOrders.DataSource = null;
                var errTable = new DataTable();
                errTable.Columns.Add("Thông báo");
                errTable.Rows.Add("Lỗi tải dữ liệu: " + ex.Message);
                dgvRecentOrders.DataSource = errTable;
            }
        }

        #endregion
    }
}