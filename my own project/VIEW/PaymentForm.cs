using my_own_project.DAL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class PaymentForm : Form
    {

        private int currentOrderID;
        private decimal subTotal = 0;
        private decimal finalTotal = 0;

        public PaymentForm(int orderID, int tableName)
        {
            InitializeComponent();
            currentOrderID = orderID;

            // Gán thông tin lên Label
            lblOrderID.Text = "Mã Đơn: #" + currentOrderID;
            lblTableName.Text = "Bàn: " + tableName;
        }


        private void PaymentForm_Load(object sender, EventArgs e)
        {
            LoadBillDetails();
        }
        public PaymentForm()
        {
            InitializeComponent();
        }

        private void LoadBillDetails()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@OrderID", currentOrderID) };
                DataTable dtBill = DataHelper.ExecuteSPGetTable("sp_OrderDetail_GetByOrderID", parameters);

                // 1. Gán dữ liệu
                dgvBill.DataSource = dtBill;

                // 2. Việt hóa tiêu đề và chỉnh độ rộng (Chỉ chạy sau khi đã gán DataSource)
                if (dgvBill.Columns.Contains("ItemName")) dgvBill.Columns["ItemName"].HeaderText = "Tên món";
                if (dgvBill.Columns.Contains("Quantity")) dgvBill.Columns["Quantity"].HeaderText = "SL";
                if (dgvBill.Columns.Contains("UnitPrice")) dgvBill.Columns["UnitPrice"].HeaderText = "Đơn giá";
                if (dgvBill.Columns.Contains("SubTotal")) dgvBill.Columns["SubTotal"].HeaderText = "Thành tiền";

                // 3. Xóa cột trống đầu tiên và dòng trống cuối cùng
                dgvBill.RowHeadersVisible = false; // Xóa cột đầu tiên
                dgvBill.AllowUserToAddRows = false; // Xóa dòng trống cuối cùng

                // 4. Ẩn các cột ID
                if (dgvBill.Columns.Contains("OrderDetailID")) dgvBill.Columns["OrderDetailID"].Visible = false;
                if (dgvBill.Columns.Contains("OrderID")) dgvBill.Columns["OrderID"].Visible = false;
                if (dgvBill.Columns.Contains("MenuItemID")) dgvBill.Columns["MenuItemID"].Visible = false;

                // 5. Tự động giãn đều các cột cho đẹp
                dgvBill.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Tính tổng tiền các món
                subTotal = 0;
                foreach (DataRow row in dtBill.Rows)
                {
                    subTotal += Convert.ToDecimal(row["SubTotal"]);
                }

                // Hiển thị lên màn hình (tạm thời finalTotal bằng subTotal vì chưa tính khuyến mãi)
                finalTotal = subTotal;
                lblSubTotal.Text = subTotal.ToString("N0") + " VNĐ";
                lblFinalTotal.Text = finalTotal.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết đơn hàng: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // Lệnh đóng form hiện tại
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {// 1. Thiết lập khổ giấy 80mm
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("ThermalBill", 315, 600);

            printPreviewDialog1.Width = 450;  // Kéo rộng cửa sổ ra
            printPreviewDialog1.Height = 700; // Kéo dài cửa sổ xuống
            printPreviewDialog1.StartPosition = FormStartPosition.CenterScreen; // Hiển thị ngay chính giữa màn hình

            // 2. Phóng to trên màn hình xem trước
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.5;

            // 3. Nối công cụ và hiển thị
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Dòng này bây giờ sẽ không bị lỗi nữa vì biến 'e' ở đây là PrintPageEventArgs
            Graphics graphic = e.Graphics;

            // Cài đặt font chữ
            Font fontTitle = new Font("Courier New", 14, FontStyle.Bold);
            Font fontNormal = new Font("Courier New", 9, FontStyle.Regular);
            Font fontItalic = new Font("Courier New", 9, FontStyle.Italic);
            SolidBrush brush = new SolidBrush(Color.Black);

            int startX = 10;
            int startY = 10;
            int offset = 15;

            // Vẽ Tiêu đề
            graphic.DrawString("NHÀ HÀNG CỦA BẠN", fontTitle, brush, startX + 45, startY);
            offset += 25;
            graphic.DrawString("Địa chỉ: Đà Nẵng, Việt Nam", fontNormal, brush, startX + 25, startY + offset);
            offset += 25;
            graphic.DrawString("HÓA ĐƠN THANH TOÁN", fontTitle, brush, startX + 35, startY + offset);

            // Vẽ thông tin đơn
            offset += 30;
            graphic.DrawString("Mã đơn: #" + currentOrderID, fontNormal, brush, startX, startY + offset);
            graphic.DrawString("Ngày: " + DateTime.Now.ToString("dd/MM/yy HH:mm"), fontNormal, brush, startX + 130, startY + offset);

            offset += 20;
            graphic.DrawString("--------------------------------------", fontNormal, brush, startX, startY + offset);

            // Vẽ tiêu đề cột
            offset += 20;
            graphic.DrawString("Tên món", fontNormal, brush, startX, startY + offset);
            graphic.DrawString("SL", fontNormal, brush, startX + 170, startY + offset);
            graphic.DrawString("Thành tiền", fontNormal, brush, startX + 210, startY + offset);

            offset += 20;
            graphic.DrawString("--------------------------------------", fontNormal, brush, startX, startY + offset);

            // Vẽ danh sách món
            offset += 20;
            foreach (DataGridViewRow row in dgvBill.Rows)
            {
                string tenMon = row.Cells["ItemName"].Value.ToString();
                string sl = row.Cells["Quantity"].Value.ToString();
                string thanhTien = Convert.ToDecimal(row.Cells["SubTotal"].Value).ToString("N0");

                if (tenMon.Length > 18)
                {
                    tenMon = tenMon.Substring(0, 18) + "...";
                }

                graphic.DrawString(tenMon, fontNormal, brush, startX, startY + offset);
                graphic.DrawString(sl, fontNormal, brush, startX + 170, startY + offset);
                graphic.DrawString(thanhTien, fontNormal, brush, startX + 210, startY + offset);
                offset += 20;
            }

            // Vẽ Tổng tiền
            offset += 10;
            graphic.DrawString("--------------------------------------", fontNormal, brush, startX, startY + offset);
            offset += 25;

            Font fontTotal = new Font("Courier New", 11, FontStyle.Bold);
            graphic.DrawString("TỔNG TIỀN: " + lblFinalTotal.Text, fontTotal, brush, startX + 70, startY + offset);

            offset += 35;
            graphic.DrawString("Cảm ơn quý khách và hẹn gặp lại!", fontItalic, brush, startX + 15, startY + offset);    
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            // Xác nhận thu tiền
            DialogResult result = MessageBox.Show($"Xác nhận thu khách số tiền: {finalTotal:N0} VNĐ?",
                                                  "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // 1. Tạo đối tượng PaymentDTO và nhét dữ liệu vào
                    PaymentDTO paymentInfo = new PaymentDTO
                    {
                        OrderID = currentOrderID, // Biến lưu mã Order của bạn
                        Amount = finalTotal,      // Biến lưu tổng tiền cần thanh toán
                        Method = "Cash",          // Hoặc lấy từ ComboBox nếu bạn có cho khách chọn Tiền mặt/Chuyển khoản
                        TransactionID = ""        // Để trống nếu trả tiền mặt
                    };

                    // 2. Gọi hàm Insert siêu xịn của bạn bạn
                    int newPaymentID = PaymentDAL.Insert(paymentInfo);

                    if (newPaymentID > 0)
                    {
                        MessageBox.Show("Thanh toán thành công! Bàn đã được dọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // Đóng form lại
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi trong quá trình thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
