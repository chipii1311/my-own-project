using my_own_project.DAL;
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
        {
            // Nối công cụ xem trước với công cụ in
            printPreviewDialog1.Document = printDocument1;
            // Mở màn hình xem trước hóa đơn
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            // Cài đặt các loại font chữ
            Font fontTitle = new Font("Courier New", 18, FontStyle.Bold);
            Font fontNormal = new Font("Courier New", 12, FontStyle.Regular);
            Font fontItalic = new Font("Courier New", 12, FontStyle.Italic);
            SolidBrush brush = new SolidBrush(Color.Black);

            // Tọa độ bắt đầu vẽ
            int startX = 10;
            int startY = 10;
            int offset = 40; // Khoảng cách giữa các dòng

            // 1. VẼ TIÊU ĐỀ QUÁN
            graphic.DrawString("NHÀ HÀNG CỦA BẠN", fontTitle, brush, startX + 40, startY);
            offset += 30;
            graphic.DrawString("Địa chỉ: Đà Nẵng, Việt Nam", fontNormal, brush, startX, startY + offset);
            offset += 30;
            graphic.DrawString("HÓA ĐƠN THANH TOÁN", fontTitle, brush, startX + 30, startY + offset);

            // 2. VẼ THÔNG TIN ĐƠN HÀNG
            offset += 40;
            graphic.DrawString("Mã đơn: #" + currentOrderID, fontNormal, brush, startX, startY + offset);
            graphic.DrawString("Ngày: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontNormal, brush, startX + 150, startY + offset);
            offset += 20;
            graphic.DrawString("---------------------------------------", fontNormal, brush, startX, startY + offset);

            // 3. VẼ TIÊU ĐỀ CỘT
            offset += 20;
            graphic.DrawString("Tên món", fontNormal, brush, startX, startY + offset);
            graphic.DrawString("SL", fontNormal, brush, startX + 160, startY + offset);
            graphic.DrawString("Thành tiền", fontNormal, brush, startX + 220, startY + offset);
            offset += 20;
            graphic.DrawString("---------------------------------------", fontNormal, brush, startX, startY + offset);

            // 4. VẼ DANH SÁCH MÓN ĂN (Lấy từ dgvBill)
            offset += 20;
            foreach (DataGridViewRow row in dgvBill.Rows)
            {
                string tenMon = row.Cells["ItemName"].Value.ToString();
                string sl = row.Cells["Quantity"].Value.ToString();
                string thanhTien = Convert.ToDecimal(row.Cells["SubTotal"].Value).ToString("N0");

                // Cắt bớt tên món nếu nó quá dài để khỏi bị tràn chữ
                if (tenMon.Length > 15)
                {
                    tenMon = tenMon.Substring(0, 15) + "...";
                }

                graphic.DrawString(tenMon, fontNormal, brush, startX, startY + offset);
                graphic.DrawString(sl, fontNormal, brush, startX + 160, startY + offset);
                graphic.DrawString(thanhTien, fontNormal, brush, startX + 220, startY + offset);
                offset += 25;
            }

            // 5. VẼ TỔNG TIỀN VÀ LỜI CẢM ƠN
            offset += 10;
            graphic.DrawString("---------------------------------------", fontNormal, brush, startX, startY + offset);
            offset += 25;

            Font fontTotal = new Font("Courier New", 14, FontStyle.Bold);
            // Lấy số tiền từ label có sẵn trên form của bạn
            graphic.DrawString("TỔNG TIỀN: " + lblFinalTotal.Text, fontTotal, brush, startX + 20, startY + offset);

            offset += 40;
            graphic.DrawString("Cảm ơn quý khách và hẹn gặp lại!", fontItalic, brush, startX + 10, startY + offset);
        }
    }
}
