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
using System.Web.UI;
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
                dataRecent.Columns["OrderID"].DataPropertyName = "OrderID";
                dataRecent.Columns["Customer"].DataPropertyName = "Customer"; // Hoặc "CustomerName" tùy SP của bạn
                dataRecent.Columns["Product"].DataPropertyName = "Product";
                dataRecent.Columns["Total"].DataPropertyName = "Total"; // Nếu SQL trả về là TotalAmount thì sửa chữ "Total" thành "TotalAmount"
                dataRecent.Columns["Status"].DataPropertyName = "Status";
                dataRecent.DataSource = DashboardBLL.GetRecentOrders(start, end);

                // 3. Load Biểu đồ Doanh thu (Area Chart)
                DateTime chartRevenueStart = start;
                DateTime chartRevenueEnd = end;

                // Nếu ngày bắt đầu TRÙNG ngày kết thúc (VD: Chọn Today hoặc Custom cùng 1 ngày)
                if (dateStart.Value.Date == dateEnd.Value.Date)
                {
                    // Tự động lùi ngày bắt đầu của biểu đồ về 6 ngày trước (để lấy đủ 7 ngày vẽ đường xu hướng)
                    chartRevenueStart = dateEnd.Value.Date.AddDays(-6);
                }

                DataTable dtRevenue = DashboardBLL.GetRevenueChart(chartRevenueStart, chartRevenueEnd);
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

        private void ShowOrderDetails(int orderId)
        {
            try
            {
                // 1. Dùng DataHelper của bạn gọi thẳng SP lấy chi tiết món (đã có sẵn trong SQL)
                System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[]
                {
            new System.Data.SqlClient.SqlParameter("@OrderID", orderId)
                };
                DataTable dtDetails = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_OrderDetail_GetByOrderID", parameters);

                if (dtDetails.Rows.Count == 0)
                {
                    MessageBox.Show("Đơn hàng này không có món nào hoặc bị lỗi dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Tự động tạo một Form Popup ẩn hiện
                Form popup = new Form()
                {
                    Text = "Chi tiết món ăn của Đơn hàng #" + orderId,
                    Size = new Size(600, 350),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                // 3. Tự động tạo DataGridView để nhét vào Popup
                DataGridView dgvDetails = new DataGridView()
                {
                    DataSource = dtDetails,
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    BackgroundColor = Color.White,
                    RowHeadersVisible = false // Giấu cột mũi tên trỏ dòng đi cho đẹp
                };

                // 4. Định dạng lại tên cột cho đẹp (ẩn các cột ID thừa)
                dgvDetails.DataBindingComplete += (s, ev) =>
                {
                    dgvDetails.Columns["OrderDetailID"].Visible = false;
                    dgvDetails.Columns["OrderID"].Visible = false;
                    dgvDetails.Columns["MenuItemID"].Visible = false;

                    dgvDetails.Columns["ItemName"].HeaderText = "Tên món";
                    dgvDetails.Columns["Quantity"].HeaderText = "SL";
                    dgvDetails.Columns["UnitPrice"].HeaderText = "Đơn giá";
                    dgvDetails.Columns["UnitPrice"].DefaultCellStyle.Format = "N0"; // Định dạng tiền
                    dgvDetails.Columns["SubTotal"].HeaderText = "Thành tiền";
                    dgvDetails.Columns["SubTotal"].DefaultCellStyle.Format = "N0";
                    dgvDetails.Columns["Note"].HeaderText = "Ghi chú";
                };

                // 5. Thêm bảng vào popup và hiển thị nó lên
                popup.Controls.Add(dgvDetails);
                popup.ShowDialog(); // ShowDialog chặn tương tác với form chính cho đến khi đóng popup
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở chi tiết đơn hàng: " + ex.Message);
            }
        }
        private void dataRecent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có click vào vùng dữ liệu hợp lệ không (bỏ qua tiêu đề cột)
            if (e.RowIndex >= 0)
            {
                // Lấy OrderID của dòng vừa click
                int orderId = Convert.ToInt32(dataRecent.Rows[e.RowIndex].Cells["OrderID"].Value);

                // Gọi hàm hiển thị chi tiết (được định nghĩa bên dưới)
                ShowOrderDetails(orderId);
            }
        }
    }
}
