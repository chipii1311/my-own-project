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

        private SqlParameter[] Params(DateTime start, DateTime end) => new SqlParameter[]
        {
            new SqlParameter("@StartDate", start),
            new SqlParameter("@EndDate",   end)
        };

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

        private void LoadRevChart()
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

        private void LoadCatChart()
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

        private void LoadTop5()
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
                    var item = new Panel { Size = new Size(w, 54), BackColor = WHITE, Margin = new Padding(0, 0, 0, 6) };
                    flowTop5.Controls.Add(item);
                    flowTop5.Resize += (s, e) => item.Width = Math.Max(220, flowTop5.ClientSize.Width - 28);

                    var badge = new Label { Text = "#" + (rank + 1), Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = rankColors[rank], BackColor = Color.Transparent, Size = new Size(34, 24), Location = new Point(0, 10), TextAlign = ContentAlignment.MiddleLeft };
                    var lName = new Label { Text = name, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = TEXT, AutoEllipsis = true, Size = new Size(w - 150, 20), Location = new Point(36, 4) };
                    var lQty = new Label { Text = qty.ToString("N0") + " đã bán", Font = new Font("Segoe UI", 8.5F), ForeColor = MUTED, AutoSize = true, Location = new Point(36, 26) };
                    var lRev = new Label { Text = rev > 0 ? rev.ToString("N0") + " đ" : "", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = GREEN, Size = new Size(105, 20), TextAlign = ContentAlignment.MiddleRight, Location = new Point(w - 112, 14), Anchor = AnchorStyles.Top | AnchorStyles.Right };
                    var div = new Panel { BackColor = BORDER, Size = new Size(w - 36, 1), Location = new Point(36, 52) };

                    item.Controls.AddRange(new Control[] { badge, lName, lQty, lRev, div });
                    rank++;
                }
            }
            catch (Exception ex)
            {
                flowTop5?.Controls.Clear();
                flowTop5?.Controls.Add(Lbl("Lỗi: " + Short(ex.Message, 80), new Font("Segoe UI", 9F), RED, new Point(0, 8)));
            }
        }

        private void LoadOrders()
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
    }
}