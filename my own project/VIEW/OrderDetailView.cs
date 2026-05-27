using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
// using my_own_project.BLL; 
// using my_own_project.DAL;

namespace my_own_project.VIEW
{
    public partial class OrderDetailView : Form
    {
        private readonly int _orderID;

        // Bắt buộc phải truyền OrderID vào khi mở form này
        public OrderDetailView(int orderID)
        {
            _orderID = orderID;

            InitializeComponent();

            // Gọi hàm dựng UI từ file Designer
            BuildUI();

            this.Load += OrderDetailView_Load;
        }

        // ========================================================
        // 1. DATA BINDING (TẢI DỮ LIỆU TỪ DB)
        // ========================================================
        private void OrderDetailView_Load(object sender, EventArgs e)
        {
            LoadOrderHeader();
            LoadOrderDetails();
        }

        private void LoadOrderHeader()
        {
            try
            {
                // TODO: Dùng BLL hoặc DAL thực tế của bạn
                // string query = $"SELECT * FROM Orders WHERE OrderID = {_orderID}";
                // DataTable dt = DataHelper.ExecuteQuery(query);

                // MOCK DATA (Dữ liệu mẫu để hiển thị UI nếu chưa nối DB)
                lblOrderID.Text = $"# {_orderID:D5}";
                lblOrderDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                lblCashier.Text = "Nguyễn Văn Admin";
                lblTable.Text = "Bàn 05 (Dine-In)";
                lblStatus.Text = "Đã thanh toán";
                lblStatus.ForeColor = Color.FromArgb(16, 185, 129); // Màu xanh lá
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin hóa đơn: " + ex.Message);
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                // TODO: Dùng BLL hoặc DAL thực tế của bạn
                // string query = $"SELECT ItemName, Quantity, UnitPrice, SubTotal FROM OrderDetail WHERE OrderID = {_orderID}";
                // DataTable dt = DataHelper.ExecuteQuery(query);

                // MOCK DATA
                DataTable dt = new DataTable();
                dt.Columns.Add("Tên món", typeof(string));
                dt.Columns.Add("SL", typeof(int));
                dt.Columns.Add("Đơn giá", typeof(decimal));
                dt.Columns.Add("Thành tiền", typeof(decimal));

                dt.Rows.Add("Hamburger Bò nướng", 2, 45000, 90000);
                dt.Rows.Add("Khoai tây chiên cỡ L", 1, 30000, 30000);
                dt.Rows.Add("Coca Cola", 2, 15000, 30000);

                dgvDetails.DataSource = dt;

                // Format lại các cột số tiền
                if (dgvDetails.Columns.Contains("Đơn giá"))
                {
                    dgvDetails.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    dgvDetails.Columns["Đơn giá"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvDetails.Columns.Contains("Thành tiền"))
                {
                    dgvDetails.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                    dgvDetails.Columns["Thành tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dgvDetails.Columns.Contains("SL"))
                    dgvDetails.Columns["SL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Căn chỉnh độ rộng cột
                dgvDetails.Columns["Tên món"].FillWeight = 200;
                dgvDetails.Columns["SL"].FillWeight = 50;
                dgvDetails.Columns["Đơn giá"].FillWeight = 100;
                dgvDetails.Columns["Thành tiền"].FillWeight = 120;

                CalculateTotals(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết món ăn: " + ex.Message);
            }
        }

        private void CalculateTotals(DataTable dt)
        {
            decimal subTotal = 0;
            foreach (DataRow row in dt.Rows)
            {
                subTotal += Convert.ToDecimal(row["Thành tiền"]);
            }

            decimal discount = 0; // Thay bằng dữ liệu giảm giá thật nếu có
            decimal finalTotal = subTotal - discount;

            lblSubTotal.Text = subTotal.ToString("N0") + " đ";
            lblDiscount.Text = "- " + discount.ToString("N0") + " đ";
            lblFinalTotal.Text = finalTotal.ToString("N0") + " đ";
        }

        // ========================================================
        // 2. SỰ KIỆN NÚT BẤM
        // ========================================================
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            // TODO: Gọi hàm in hóa đơn (như trong HistoryForm bạn đã làm)
            MessageBox.Show("Đang kết nối máy in...", "Thông báo in", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}