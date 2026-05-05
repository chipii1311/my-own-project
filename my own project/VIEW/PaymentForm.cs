using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace my_own_project.VIEW // Đảm bảo đúng namespace này nhé
{
    public partial class PaymentForm : Form
    {
        private int currentOrderID;
        private decimal totalAmount = 0;
        private int tableID;

        // Các control giao diện
        private Label lblTotalAmount;
        private Guna2Button btnPrint;   // Nút In
        private Guna2Button btnConfirm; // Nút Xác nhận
        private Guna2Button btnCancel;  // Nút Hủy
        private Guna2ShadowForm shadowForm;

        // Đồ nghề in ấn
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;

        public PaymentForm(int orderID, int tableID = -1)
        {
            this.currentOrderID = orderID;
            this.tableID = tableID;

            // Khởi tạo máy in khổ 80mm (Khoảng 315 pixel)
            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 315, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;

            printPreview = new PrintPreviewDialog();
            printPreview.Document = printDoc;
            printPreview.StartPosition = FormStartPosition.CenterScreen;
            printPreview.Size = new Size(450, 650);
            // Sửa lỗi zoom nhỏ của PrintPreview mặc định
            printPreview.PrintPreviewControl.Zoom = 1.0;

            InitializeModernUI();
            LoadTotalAmount();
        }

        private void InitializeModernUI()
        {
            this.Size = new Size(480, 320); // Mở rộng form một chút cho thoáng
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            shadowForm = new Guna2ShadowForm(this);
            shadowForm.ShadowColor = Color.Black;

            // --- HEADER MÀU TÍM ---
            Guna2Panel pnlHeader = new Guna2Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.FillColor = Color.FromArgb(88, 28, 230);

            Label lblTitle = new Label();
            lblTitle.Text = "XÁC NHẬN THANH TOÁN";
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            // --- NỘI DUNG CHÍNH ---
            Label lblTextTotal = new Label { Text = "Tổng tiền cần thanh toán:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, Location = new Point(30, 90), AutoSize = true };
            this.Controls.Add(lblTextTotal);

            lblTotalAmount = new Label { Text = "0 đ", Font = new Font("Segoe UI", 28F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 71, 87), Location = new Point(25, 120), AutoSize = true };
            this.Controls.Add(lblTotalAmount);

            // --- 3 NÚT BẤM RÕ RÀNG ---
            int btnY = 220;

            btnCancel = new Guna2Button();
            btnCancel.Text = "Hủy bỏ";
            btnCancel.Size = new Size(100, 55);
            btnCancel.Location = new Point(25, btnY);
            btnCancel.BorderRadius = 8;
            btnCancel.FillColor = Color.FromArgb(235, 235, 235);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) => { this.Close(); };
            this.Controls.Add(btnCancel);

            btnPrint = new Guna2Button();
            btnPrint.Text = "In Hóa Đơn";
            btnPrint.Size = new Size(130, 55);
            btnPrint.Location = new Point(135, btnY);
            btnPrint.BorderRadius = 8;
            btnPrint.FillColor = Color.FromArgb(46, 204, 113); // Màu xanh lá
            btnPrint.ForeColor = Color.White;
            btnPrint.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnPrint.Cursor = Cursors.Hand;
            btnPrint.Click += (s, e) => { printPreview.ShowDialog(); }; // Chỉ bật Preview Bill, không dọn bàn
            this.Controls.Add(btnPrint);

            btnConfirm = new Guna2Button();
            btnConfirm.Text = "Xác nhận Thu tiền";
            btnConfirm.Size = new Size(180, 55);
            btnConfirm.Location = new Point(275, btnY);
            btnConfirm.BorderRadius = 8;
            btnConfirm.FillColor = Color.FromArgb(88, 28, 230); // Màu tím
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            // Viền bo góc
            Guna2BorderlessForm borderlessForm = new Guna2BorderlessForm();
            borderlessForm.ContainerControl = this;
            borderlessForm.BorderRadius = 15;
        }

        private void LoadTotalAmount()
        {
            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
            totalAmount = 0;
            foreach (DataRow row in dtDetails.Rows)
            {
                totalAmount += Convert.ToDecimal(row["SubTotal"]);
            }
            lblTotalAmount.Text = totalAmount.ToString("N0") + " đ";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận khách đã thanh toán đủ tiền và dọn bàn?", "Chốt đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE Orders SET Status = 'Completed' WHERE OrderID = {currentOrderID}");
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE DiningTable SET Status = N'Trống' WHERE TableID = (SELECT TableID FROM Orders WHERE OrderID = {currentOrderID})");

                    MessageBox.Show("Thanh toán thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        // ===============================================
        // BÍ QUYẾT VẼ BILL RÕ NÉT CĂN CHỈNH TUYỆT ĐỐI
        // ===============================================
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Khai báo Font chữ to, rõ ràng
            Font fontTitle = new Font("Courier New", 18, FontStyle.Bold);
            Font fontSub = new Font("Courier New", 11, FontStyle.Regular);
            Font fontHeader = new Font("Courier New", 14, FontStyle.Bold);
            Font fontItem = new Font("Courier New", 11, FontStyle.Regular);
            Font fontBold = new Font("Courier New", 11, FontStyle.Bold);

            // Công cụ căn giữa và căn phải tuyệt đối
            StringFormat centerAlign = new StringFormat() { Alignment = StringAlignment.Center };
            StringFormat rightAlign = new StringFormat() { Alignment = StringAlignment.Far };

            int yPos = 10;
            int leftMargin = 5;
            int centerPoint = 157; // Điểm chính giữa của tờ giấy 315px
            int rightMargin = 300; // Mép phải tờ giấy

            // --- HEADER QUÁN ---
            g.DrawString("PBL3 RESTAURANT", fontTitle, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 30;
            g.DrawString("Đ/c: ĐH Bách Khoa Đà Nẵng", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 20;
            g.DrawString("Hotline: 0123.456.789", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 35;

            // --- TIÊU ĐỀ BILL ---
            g.DrawString("PHIẾU TẠM TÍNH", fontHeader, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 35;

            g.DrawString("Mã HD: " + currentOrderID, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            g.DrawString("Ngày : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            string line = new string('-', 33);
            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            // --- TIÊU ĐỀ CÁC CỘT ---
            g.DrawString("Tên món", fontBold, Brushes.Black, leftMargin, yPos);
            g.DrawString("SL", fontBold, Brushes.Black, 170, yPos);
            g.DrawString("T.Tiền", fontBold, Brushes.Black, rightMargin, yPos, rightAlign); // Ép sát mép phải
            yPos += 25;
            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            // --- CHI TIẾT MÓN ---
            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
            foreach (DataRow row in dtDetails.Rows)
            {
                string name = row["ItemName"].ToString();
                if (name.Length > 15) name = name.Substring(0, 15) + ".."; // Chống tràn dòng

                string qty = row["Quantity"].ToString();
                string sub = Convert.ToDecimal(row["SubTotal"]).ToString("N0");

                g.DrawString(name, fontItem, Brushes.Black, leftMargin, yPos);
                g.DrawString(qty, fontItem, Brushes.Black, 170, yPos);
                g.DrawString(sub, fontItem, Brushes.Black, rightMargin, yPos, rightAlign); // Tiền ép mép phải
                yPos += 25;
            }

            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            // --- TỔNG CỘNG ---
            g.DrawString("TỔNG CỘNG:", fontHeader, Brushes.Black, leftMargin, yPos);
            g.DrawString(totalAmount.ToString("N0") + " đ", fontHeader, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 45;

            // --- FOOTER ---
            g.DrawString("Cảm ơn & Hẹn gặp lại!", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
        }
    }
}