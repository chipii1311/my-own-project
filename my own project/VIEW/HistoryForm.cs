using my_own_project.DAL;
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
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        // Sự kiện khi Form vừa được mở lên
        private void HistoryForm_Load(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        // Hàm xử lý việc load và làm đẹp dữ liệu
        private void LoadHistoryData()
        {
            try
            {
                // 1. Gọi hàm GetAll từ OrderDAL cực xịn của bạn
                DataTable dt = OrderDAL.GetAll();
                dgvHistory.DataSource = dt;

                // 2. ẨN CÁC CỘT KHÔNG CẦN THIẾT (Mã ID, ngày cập nhật...)
                string[] hiddenColumns = {
                    "CustomerID", "RestaurantID", "TableID", "StaffID",
                    "PromotionID", "UpdatedAt", "RestaurantName", "Position"
                };

                foreach (string col in hiddenColumns)
                {
                    if (dgvHistory.Columns.Contains(col))
                    {
                        dgvHistory.Columns[col].Visible = false;
                    }
                }

                // 3. VIỆT HÓA VÀ ĐỊNH DẠNG LẠI CÁC CỘT HIỂN THỊ
                if (dgvHistory.Columns.Contains("OrderID"))
                {
                    dgvHistory.Columns["OrderID"].HeaderText = "Mã Hóa Đơn";
                    dgvHistory.Columns["OrderID"].Width = 100;
                }

                if (dgvHistory.Columns.Contains("TableNumber"))
                {
                    dgvHistory.Columns["TableNumber"].HeaderText = "Bàn Số";
                    dgvHistory.Columns["TableNumber"].Width = 80;
                }

                if (dgvHistory.Columns.Contains("CustomerName"))
                {
                    dgvHistory.Columns["CustomerName"].HeaderText = "Tên Khách";
                }

                if (dgvHistory.Columns.Contains("OrderType"))
                {
                    dgvHistory.Columns["OrderType"].HeaderText = "Loại Đơn";
                }

                if (dgvHistory.Columns.Contains("OrderDate"))
                {
                    dgvHistory.Columns["OrderDate"].HeaderText = "Thời Gian Tạo";
                    // Định dạng giờ chuẩn VN: Ngày/Tháng/Năm Giờ:Phút
                    dgvHistory.Columns["OrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                if (dgvHistory.Columns.Contains("TotalAmount"))
                {
                    dgvHistory.Columns["TotalAmount"].HeaderText = "Tổng Tiền (VNĐ)";
                    // Định dạng phân cách hàng nghìn (Ví dụ: 150,000)
                    dgvHistory.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
                    dgvHistory.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Căn phải cho số tiền
                }

                if (dgvHistory.Columns.Contains("Status"))
                {
                    dgvHistory.Columns["Status"].HeaderText = "Trạng Thái";
                }

                // 4. CẤU HÌNH GIAO DIỆN CHUNG CHO DATAGRIDVIEW
                dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động kéo dãn lấp đầy khoảng trống
                dgvHistory.RowHeadersVisible = false;                                  // Ẩn cột mũi tên trống bên mép trái
                dgvHistory.AllowUserToAddRows = false;                                 // Không cho hiển thị dòng thừa dưới cùng
                dgvHistory.ReadOnly = true;                                            // Chỉ xem, không cho sửa trực tiếp trên bảng
                dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;    // Click vào 1 ô là bôi đen cả dòng
                dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray; // Tạo hiệu ứng sọc ngựa vằn cho dễ nhìn
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải lịch sử hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
