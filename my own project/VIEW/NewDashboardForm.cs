using my_own_project.BLL;
using my_own_project.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace my_own_project.VIEW
{
    public partial class NewDashboardForm : Form
    {
        public NewDashboardForm()
        {
            InitializeComponent();

            // Gọi hàm dựng giao diện (được định nghĩa bên Designer.cs)
            BuildUI();

            this.Load += (s, e) => RefreshAll();
        }

        // ═══════════════════════════════════════════════════════════════
        //  DATA HELPERS
        // ═══════════════════════════════════════════════════════════════
        private string Short(string msg, int max) =>
            string.IsNullOrEmpty(msg) ? "" :
            msg.Length <= max ? msg : msg.Substring(0, max) + "...";

        private string FindCol(DataTable dt, params string[] names)
        {
            foreach (var n in names) if (dt.Columns.Contains(n)) return n;
            return dt.Columns.Count > 0 ? dt.Columns[0].ColumnName : "";
        }



        

        // ═══════════════════════════════════════════════════════════════
        //  DATA LOADING
        // ═══════════════════════════════════════════════════════════════
        public void RefreshAll()
        {
            lblUpdated.Text = "Cập nhật lúc " + DateTime.Now.ToString("HH:mm:ss");
            LoadKPIs();
            LoadRevChart();
            LoadCatChart();
            LoadTop5();
            LoadOrders();
        }


        private void LoadKPIs()
        {
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL
                var dt = DashboardBLL.GetSummary(DateTime.Today, DateTime.Today.AddDays(1).AddSeconds(-1));

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
            catch { lblInv.Text = "--"; lblInv.ForeColor = MUTED; lblInvSub.Text = "Không thể kiểm tra kho"; }
        }

        private void LoadRevChart()
        {
            lblRevMsg.Text = "Đang tải..."; lblRevMsg.Visible = true;
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL
                var dt = DashboardBLL.GetRevenueChart(
                    DateTime.Today.AddDays(-6), DateTime.Today.AddDays(1));

                chartRev.Series["rev"].Points.Clear();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string dc = FindCol(dt, "OrderDate", "Date", "CreatedAt");
                    string rc = FindCol(dt, "Revenue", "TotalRevenue", "TotalAmount");
                    foreach (DataRow row in dt.Rows)
                    {
                        string lbl = DateTime.TryParse(row[dc].ToString(), out var d) ? d.ToString("dd/MM") : row[dc].ToString();
                        decimal rev = row[rc] == DBNull.Value ? 0 : Convert.ToDecimal(row[rc]);
                        chartRev.Series["rev"].Points.AddXY(lbl, rev);
                    }
                    lblRevMsg.Visible = false; chartRev.Visible = true;
                    chartRev.Invalidate(); chartRev.Update();
                }
                else
                {
                    chartRev.Visible = false;
                    lblRevMsg.Text = "Không có dữ liệu 7 ngày qua"; lblRevMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                chartRev.Visible = false;
                lblRevMsg.Text = "Lỗi: " + Short(ex.Message, 60); lblRevMsg.Visible = true;
            }
        }

        private void LoadCatChart()
        {
            lblCatMsg.Text = "Đang tải..."; lblCatMsg.Visible = true;
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL (method mới thêm)
                var dt = DashboardBLL.GetCategoryRevenueShare(
                    DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1));

                chartCat.Series["cat"].Points.Clear();
                if (dt != null && dt.Rows.Count > 0)
                {
                    string nc = FindCol(dt, "CategoryName", "Name", "Category");
                    string vc = FindCol(dt, "Revenue", "TotalRevenue", "TotalAmount", "Value");
                    foreach (DataRow row in dt.Rows)
                    {
                        string lbl = Short(row[nc].ToString(), 18);
                        decimal val = row[vc] == DBNull.Value ? 0 : Convert.ToDecimal(row[vc]);
                        chartCat.Series["cat"].Points.AddXY(lbl, val);
                    }
                    lblCatMsg.Visible = false; chartCat.Visible = true;
                }
                else
                {
                    chartCat.Visible = false;
                    lblCatMsg.Text = "Không có dữ liệu"; lblCatMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                chartCat.Visible = false;
                lblCatMsg.Text = "Lỗi: " + Short(ex.Message, 60); lblCatMsg.Visible = true;
            }
        }

        private void LoadTop5()
        {
            try
            {
                var dt = DashboardBLL.GetTop5Products(
                    DateTime.Today.AddDays(-30), DateTime.Today.AddDays(1));

                flowTop5.Controls.Clear();

                if (dt == null || dt.Rows.Count == 0)
                {
                    flowTop5.Controls.Add(new Label
                    {
                        Text = "Không có dữ liệu",
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = MUTED,
                        AutoSize = true
                    });
                    return;
                }

                int rank = 1;

                foreach (DataRow row in dt.Rows)
                {
                    string nameCol = FindCol(dt, "ProductName", "Name", "Product");
                    string qtyCol = FindCol(dt, "Quantity", "TotalQuantity", "SoldQty", "Qty");

                    string productName = row[nameCol]?.ToString() ?? "";
                    string quantity = row[qtyCol]?.ToString() ?? "0";

                    var item = new Panel
                    {
                        Width = flowTop5.ClientSize.Width - 30,
                        Height = 52,
                        BackColor = WHITE,
                        Margin = new Padding(0, 0, 0, 8)
                    };

                    var lblRank = new Label
                    {
                        Text = "#" + rank,
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        ForeColor = PURPLE,
                        Location = new Point(4, 14),
                        AutoSize = true
                    };

                    var lblName = new Label
                    {
                        Text = Short(productName, 25),
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        ForeColor = TEXT,
                        Location = new Point(48, 8),
                        AutoSize = true
                    };

                    var lblQty = new Label
                    {
                        Text = quantity + " đã bán",
                        Font = new Font("Segoe UI", 9F),
                        ForeColor = MUTED,
                        Location = new Point(48, 29),
                        AutoSize = true
                    };

                    item.Controls.Add(lblRank);
                    item.Controls.Add(lblName);
                    item.Controls.Add(lblQty);

                    flowTop5.Controls.Add(item);
                    rank++;
                }
            }
            catch
            {
                flowTop5.Controls.Clear();
                flowTop5.Controls.Add(new Label
                {
                    Text = "Không thể tải Top 5",
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = RED,
                    AutoSize = true
                });
            }
        }

        private void LoadOrders()
        {
            try
            {
                var dt = DashboardBLL.GetRecentOrders(
                    DateTime.Today, DateTime.Today.AddDays(1).AddSeconds(-1));

                dgv.DataSource = dt;
            }
            catch
            {
                dgv.DataSource = null;
            }
        }
    }
}
