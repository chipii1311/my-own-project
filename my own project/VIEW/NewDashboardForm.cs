using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace my_own_project.VIEW
{
    // ĐÃ ĐỔI TÊN THÀNH NewDashboardForm
    public partial class NewDashboardForm : Form
    {
        private Label lblRevenue, lblOrderCount, lblTopItem;
        private Chart chartRevenue, chartCategory;
        private Guna2DataGridView dgvRecentOrders;

        // ĐÃ ĐỔI TÊN HÀM KHỞI TẠO
        public NewDashboardForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildDashboardUI();

            this.Load += (s, e) => {
                LoadKPIs();
                LoadRevenueChart();
                LoadCategoryChart();
                LoadRecentOrders();
            };
        }

        private void BuildDashboardUI()
        {
            TableLayoutPanel tlpMain = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            tlpMain.RowCount = 3;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            this.Controls.Add(tlpMain);

            // ==========================================
            // HÀNG 1: 3 THẺ KPI
            // ==========================================
            TableLayoutPanel tlpKPI = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, Margin = new Padding(0, 0, 0, 20) };
            tlpKPI.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpKPI.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpKPI.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpMain.Controls.Add(tlpKPI, 0, 0);

            lblRevenue = new Label();
            tlpKPI.Controls.Add(CreateKPICard("DOANH THU HÔM NAY", lblRevenue, Color.FromArgb(46, 204, 113), " đ"), 0, 0);

            lblOrderCount = new Label();
            tlpKPI.Controls.Add(CreateKPICard("TỔNG ĐƠN HÔM NAY", lblOrderCount, Color.FromArgb(52, 152, 219), " đơn"), 1, 0);

            lblTopItem = new Label();
            tlpKPI.Controls.Add(CreateKPICard("MÓN BÁN CHẠY NHẤT", lblTopItem, Color.FromArgb(255, 159, 67), ""), 2, 0);

            // ==========================================
            // HÀNG 2: 2 BIỂU ĐỒ (CỘT & TRÒN)
            // ==========================================
            TableLayoutPanel tlpCharts = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Margin = new Padding(0, 0, 0, 20) };
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpMain.Controls.Add(tlpCharts, 0, 1);

            Guna2Panel cardChart1 = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(0, 0, 10, 0), Padding = new Padding(15) };
            chartRevenue = new Chart { Dock = DockStyle.Fill };
            ChartArea ca1 = new ChartArea { Name = "MainArea" };
            ca1.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca1.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartRevenue.ChartAreas.Add(ca1);
            Series s1 = new Series { Name = "DoanhThu", ChartType = SeriesChartType.Column, Color = Color.FromArgb(88, 28, 230), IsValueShownAsLabel = true };
            chartRevenue.Series.Add(s1);
            chartRevenue.Titles.Add(new Title("Doanh thu 7 ngày gần nhất", Docking.Top, new Font("Segoe UI", 12F, FontStyle.Bold), Color.FromArgb(64, 64, 64)));
            cardChart1.Controls.Add(chartRevenue);
            tlpCharts.Controls.Add(cardChart1, 0, 0);

            Guna2Panel cardChart2 = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(10, 0, 0, 0), Padding = new Padding(15) };
            chartCategory = new Chart { Dock = DockStyle.Fill };
            chartCategory.ChartAreas.Add(new ChartArea { Name = "MainArea" });
            Series s2 = new Series { Name = "DanhMuc", ChartType = SeriesChartType.Doughnut, IsValueShownAsLabel = true };
            s2["DoughnutRadius"] = "50";
            chartCategory.Series.Add(s2);
            chartCategory.Legends.Add(new Legend { Docking = Docking.Bottom, Font = new Font("Segoe UI", 9F) });
            chartCategory.Titles.Add(new Title("Tỷ trọng bán hàng 30 ngày", Docking.Top, new Font("Segoe UI", 12F, FontStyle.Bold), Color.FromArgb(64, 64, 64)));
            cardChart2.Controls.Add(chartCategory);
            tlpCharts.Controls.Add(cardChart2, 1, 0);

            // ==========================================
            // HÀNG 3: BẢNG LỊCH SỬ ĐƠN MỚI NHẤT
            // ==========================================
            Guna2Panel cardGrid = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Padding = new Padding(10) };
            tlpMain.Controls.Add(cardGrid, 0, 2);

            Label lblGridTitle = new Label { Text = "GIAO DỊCH MỚI NHẤT", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), Dock = DockStyle.Top, Height = 30 };
            cardGrid.Controls.Add(lblGridTitle);

            dgvRecentOrders = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            dgvRecentOrders.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvRecentOrders.ThemeStyle.HeaderStyle.ForeColor = Color.Black;
            dgvRecentOrders.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvRecentOrders.RowTemplate.Height = 40;
            dgvRecentOrders.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(240, 235, 255);
            dgvRecentOrders.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            cardGrid.Controls.Add(dgvRecentOrders);
            dgvRecentOrders.BringToFront();
        }

        private Guna2Panel CreateKPICard(string title, Label lblValue, Color color, string suffix)
        {
            Guna2Panel pnl = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(10) };

            Panel colorStrip = new Panel { Dock = DockStyle.Left, Width = 8, BackColor = color };
            pnl.Controls.Add(colorStrip);

            Label lbl = new Label { Text = title, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, AutoSize = true, Location = new Point(20, 20) };
            pnl.Controls.Add(lbl);

            lblValue.Text = "0" + suffix;
            lblValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblValue.ForeColor = color;
            lblValue.AutoSize = true;
            lblValue.Location = new Point(20, 50);
            pnl.Controls.Add(lblValue);

            return pnl;
        }

        private void LoadKPIs()
        {
            try
            {
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery("EXEC sp_Dashboard_GetKPIs");
                if (dt.Rows.Count > 0)
                {
                    decimal rev = Convert.ToDecimal(dt.Rows[0]["TodayRevenue"]);
                    lblRevenue.Text = rev.ToString("N0") + " đ";
                    lblOrderCount.Text = dt.Rows[0]["TodayOrders"].ToString() + " đơn";
                    lblTopItem.Text = dt.Rows[0]["TopItem"].ToString();
                }
            }
            catch { }
        }

        private void LoadRevenueChart()
        {
            try
            {
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery("EXEC sp_Dashboard_GetRevenue7Days");
                chartRevenue.Series["DoanhThu"].Points.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string date = row["OrderDate"].ToString();
                    decimal rev = Convert.ToDecimal(row["Revenue"]);
                    chartRevenue.Series["DoanhThu"].Points.AddXY(date, rev);
                }
            }
            catch { }
        }

        private void LoadCategoryChart()
        {
            try
            {
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery("EXEC sp_Dashboard_GetCategoryRevenue");
                chartCategory.Series["DanhMuc"].Points.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string cat = row["CategoryName"].ToString();
                    decimal rev = Convert.ToDecimal(row["Revenue"]);
                    chartCategory.Series["DanhMuc"].Points.AddXY(cat, rev);
                }
            }
            catch { }
        }

        private void LoadRecentOrders()
        {
            try
            {
                string query = $"EXEC sp_Dashboard_GetRecentOrders '{DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd")}', '{DateTime.Now.ToString("yyyy-MM-dd")}'";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                if (dt.Columns.Contains("Product")) dt.Columns.Remove("Product");

                dgvRecentOrders.DataSource = dt;

                // --- THÊM ĐOẠN NÀY ĐỂ TRANG TRÍ CÁC CỘT ---
                if (dgvRecentOrders.Columns.Count > 0)
                {
                    dgvRecentOrders.Columns["OrderID"].HeaderText = "Mã HĐ";
                    dgvRecentOrders.Columns["Customer"].HeaderText = "Khách hàng";

                    dgvRecentOrders.Columns["Total"].HeaderText = "Tổng tiền (VNĐ)";
                    dgvRecentOrders.Columns["Total"].DefaultCellStyle.Format = "N0"; // Thêm dấu phẩy hàng nghìn

                    dgvRecentOrders.Columns["Status"].HeaderText = "Trạng thái";
                }
            }
            catch { }
        }
    }
}
