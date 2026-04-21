using Guna.Charts.WinForms;
using my_own_project.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }
        private void ToggleCustomDateControls(bool isVisible)
        {
            dateStart.Visible = isVisible;
            dateEnd.Visible = isVisible;
            btnConfirm.Visible = isVisible;
        }

        private void LoadDashboardData()
        {
            try
            {
                // Lấy đầu ngày bắt đầu và cuối ngày kết thúc
                DateTime start = dateStart.Value.Date;
                DateTime end = dateEnd.Value.Date.AddDays(1).AddTicks(-1);

                // 1. Load các con số tổng (Labels)
                DataTable dtSummary = DashboardBLL.GetSummary(start, end);
                if (dtSummary.Rows.Count > 0)
                {
                    DataRow row = dtSummary.Rows[0];
                    lblRevenue.Text = Convert.ToDecimal(row["TotalRevenue"]).ToString("N0") + " đ";
                    lblOrders.Text = row["TotalOrders"].ToString();
                    lblCustomers.Text = row["TotalCustomers"].ToString();
                }

                // 2. Load DataGridView (Đơn hàng gần đây)
                dataRecent.AutoGenerateColumns = false;
                dataRecent.DataSource = DashboardBLL.GetRecentOrders(start, end);

                // 3. Load Biểu đồ Doanh thu (Area Chart)
                DataTable dtRevenue = DashboardBLL.GetRevenueChart(start, end);
                gunaAreaDataset1.DataPoints.Clear(); // Xóa data cũ
                foreach (DataRow row in dtRevenue.Rows)
                {
                    string dateLabel = Convert.ToDateTime(row["Date"]).ToString("dd/MM");
                    double revenueVal = Convert.ToDouble(row["Revenue"]);
                    gunaAreaDataset1.DataPoints.Add(dateLabel, revenueVal);
                }
                chartRevenue.Update();
                chartRevenue.Refresh();

                // 4. Load Biểu đồ Top Sản phẩm (Doughnut Chart)
                DataTable dtProducts = DashboardBLL.GetTopProducts(start, end);
                gunaDoughnutDataset1.DataPoints.Clear();
                foreach (DataRow row in dtProducts.Rows)
                {
                    string productName = row["ProductName"].ToString();
                    double quantity = Convert.ToDouble(row["Quantity"]);
                    gunaDoughnutDataset1.DataPoints.Add(productName, quantity);
                }
                chartProduct.Update();
                chartProduct.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Dashboard: " + ex.Message);
            }
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            ToggleCustomDateControls(true);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            ToggleCustomDateControls(false);
            dateStart.Value = DateTime.Today;
            dateEnd.Value = DateTime.Now;
            LoadDashboardData();
        }

        private void btn7days_Click(object sender, EventArgs e)
        {
            ToggleCustomDateControls(false);
            dateStart.Value = DateTime.Today.AddDays(-7);
            dateEnd.Value = DateTime.Now;
            LoadDashboardData();
        }

        private void btn30days_Click(object sender, EventArgs e)
        {
            ToggleCustomDateControls(false);
            dateStart.Value = DateTime.Today.AddDays(-30);
            dateEnd.Value = DateTime.Now;
            LoadDashboardData();
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            ToggleCustomDateControls(false);
            dateStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // Ngày đầu tháng
            dateEnd.Value = DateTime.Now;
            LoadDashboardData();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            // Mặc định khi mở form lên là xem dữ liệu 7 ngày qua
            dateStart.Value = DateTime.Today.AddDays(-7);
            dateEnd.Value = DateTime.Now;

            // Ẩn 2 ô chọn ngày và nút OK lúc mới mở form
            ToggleCustomDateControls(false);

            // Load dữ liệu lần đầu
            LoadDashboardData();
        }
    }
}
