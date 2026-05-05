using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace my_own_project.DesignForms
{
    public partial class PaymentForm : Form
    {
        private int currentOrderID;
        private decimal totalAmount = 0;
        private int tableID;

        // Các control giao diện
        private Label lblTotalAmount;
        private Guna2Button btnPrint;   // Nút In tạm tính
        private Guna2Button btnConfirm; // Nút Xác nhận thu tiền
        private Guna2Button btnCancel;
        private Guna2ShadowForm shadowForm;

        // Đồ nghề in ấn
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;

        public PaymentForm(int orderID, int tableID = -1)
        {
            this.currentOrderID = orderID;
            this.tableID = tableID;

            // Khởi tạo máy in
            printDoc = new PrintDocument();
            // Thiết lập khổ giấy in nhiệt 80mm (Khoảng 320 pixel)
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 320, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;

            printPreview = new PrintPreviewDialog();
            printPreview.Document = printDoc;
            printPreview.StartPosition = FormStartPosition.CenterScreen;
            printPreview.Size = new Size(400, 600);

            InitializeModernUI();
            LoadTotalAmount();
        }

        private void InitializeModernUI()
        {
            this.Size = new Size(480, 300);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            shadowForm = new Guna2ShadowForm(this);
            shadowForm.ShadowColor = Color.Black;

            // --- HEADER ---
            Guna2Panel pnlHeader = new Guna2Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.FillColor = Color.FromArgb(88, 28, 230);

            Label lblTitle = new Label();
            lblTitle.Text = "XÁC NHẬN THANH TOÁN";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            pnlHeader.Controls.Add(lblTitle);

            // --- NỘI DUNG CHÍNH ---
            Label lblTextTotal = new Label { Text = "Tổng tiền cần thanh toán:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, Location = new Point(30, 90), AutoSize = true };
            this.Controls.Add(lblTextTotal);

            lblTotalAmount = new Label { Text = "0 đ", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 71, 87), Location = new Point(30, 120), AutoSize = true };
            this.Controls.Add(lblTotalAmount);

            // --- 3 NÚT BẤM (Thiết kế lại logic) ---
            int btnY = 210;

            btnCancel = new Guna2Button();
            btnCancel.Text = "Hủy";
            btnCancel.Size = new Size(90, 50);
            btnCancel.Location = new Point(20, btnY);
            btnCancel.BorderRadius = 10;
            btnCancel.FillColor = Color.FromArgb(240, 240, 240);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) => { this.Close(); };
            this.Controls.Add(btnCancel);

            btnPrint = new Guna2Button();
            btnPrint.Text = "In Hóa Đơn Tạm Tính";
            btnPrint.Size = new Size(130, 50);
            btnPrint.Location = new Point(125, btnY);
            btnPrint.BorderRadius = 10;
            btnPrint.FillColor = Color.FromArgb(46, 204, 113); // Màu xanh lá cho nút In
            btnPrint.ForeColor = Color.White;
            btnPrint.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnPrint.Cursor = Cursors.Hand;
            btnPrint.Click += (s, e) => { printPreview.ShowDialog(); }; // Chỉ in, không cập nhật DB
            this.Controls.Add(btnPrint);

            btnConfirm = new Guna2Button();
            btnConfirm.Text = "Xác nhận & Thu tiền";
            btnConfirm.Size = new Size(180, 50);
            btnConfirm.Location = new Point(270, btnY);
            btnConfirm.BorderRadius = 10;
            btnConfirm.FillColor = Color.FromArgb(88, 28, 230); // Màu tím chủ đạo
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            Guna2BorderlessForm borderlessForm = new Guna2BorderlessForm();
            borderlessForm.ContainerControl = this;
            borderlessForm.BorderRadius = 20;
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
            if (MessageBox.Show("Khách đã thanh toán đủ tiền?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // 1. Cập nhật Order thành 'Completed'
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE Orders SET Status = 'Completed' WHERE OrderID = {currentOrderID}");

                    // 2. Trả bàn về trạng thái 'Trống'
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
        // BÍ QUYẾT VẼ BILL CHUẨN MÁY IN NHIỆT (THERMAL 80MM)
        // ===============================================
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontTitle = new Font("Courier New", 16, FontStyle.Bold);
            Font fontHeader = new Font("Courier New", 10, FontStyle.Italic);
            Font fontRegular = new Font("Courier New", 10, FontStyle.Regular);
            Font fontBold = new Font("Courier New", 10, FontStyle.Bold);

            int yPos = 10;
            int width = 300; // Cố định chiều rộng để căn giữa
            int margin = 10; // Căn lề trái

            // Công cụ để căn giữa và căn phải
            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;

            StringFormat rightFormat = new StringFormat();
            rightFormat.Alignment = StringAlignment.Far;

            // --- HEADER QUÁN (Căn Giữa) ---
            g.DrawString("PBL3 RESTAURANT", fontTitle, Brushes.Black, new RectangleF(0, yPos, width, 25), centerFormat);
            yPos += 25;
            g.DrawString("Đ/c: Đại học Bách Khoa Đà Nẵng", fontRegular, Brushes.Black, new RectangleF(0, yPos, width, 20), centerFormat);
            yPos += 20;
            g.DrawString("Hotline: 0123.456.789", fontRegular, Brushes.Black, new RectangleF(0, yPos, width, 20), centerFormat);
            yPos += 30;

            // --- THÔNG TIN BILL ---
            g.DrawString("PHIẾU TẠM TÍNH", new Font("Courier New", 14, FontStyle.Bold), Brushes.Black, new RectangleF(0, yPos, width, 25), centerFormat);
            yPos += 30;

            g.DrawString("Mã HD: " + currentOrderID, fontRegular, Brushes.Black, margin, yPos);
            yPos += 20;
            g.DrawString("Ngày: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontRegular, Brushes.Black, margin, yPos);
            yPos += 25;

            string line = new string('-', 35);
            g.DrawString(line, fontRegular, Brushes.Black, margin, yPos);
            yPos += 20;

            // --- TIÊU ĐỀ CÁC CỘT ---
            g.DrawString("Tên món", fontBold, Brushes.Black, margin, yPos);
            g.DrawString("SL", fontBold, Brushes.Black, 150, yPos);
            // Gióng phải cho đẹp
            g.DrawString("Đ.Giá", fontBold, Brushes.Black, new RectangleF(170, yPos, 50, 20), rightFormat);
            g.DrawString("T.Tiền", fontBold, Brushes.Black, new RectangleF(230, yPos, 60, 20), rightFormat);
            yPos += 20;
            g.DrawString(line, fontRegular, Brushes.Black, margin, yPos);
            yPos += 20;

            // --- CHI TIẾT CÁC MÓN ĂN ---
            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
            foreach (DataRow row in dtDetails.Rows)
            {
                string name = row["ItemName"].ToString();
                if (name.Length > 15) name = name.Substring(0, 15) + "..."; // Cắt tên dài

                string qty = row["Quantity"].ToString();
                string price = (Convert.ToDecimal(row["UnitPrice"]) / 1000).ToString("0") + "k";
                string sub = (Convert.ToDecimal(row["SubTotal"]) / 1000).ToString("0") + "k";

                g.DrawString(name, fontRegular, Brushes.Black, margin, yPos);
                g.DrawString(qty, fontRegular, Brushes.Black, 150, yPos);
                g.DrawString(price, fontRegular, Brushes.Black, new RectangleF(170, yPos, 50, 20), rightFormat);
                g.DrawString(sub, fontRegular, Brushes.Black, new RectangleF(230, yPos, 60, 20), rightFormat);
                yPos += 25;
            }

            g.DrawString(line, fontRegular, Brushes.Black, margin, yPos);
            yPos += 20;

            // --- TỔNG CỘNG ---
            g.DrawString("TỔNG CỘNG:", new Font("Courier New", 12, FontStyle.Bold), Brushes.Black, margin, yPos);
            g.DrawString(totalAmount.ToString("N0") + " đ", new Font("Courier New", 12, FontStyle.Bold), Brushes.Black, new RectangleF(130, yPos, 160, 25), rightFormat);
            yPos += 40;

            // --- LỜI CẢM ƠN ---
            g.DrawString("Cảm ơn & Hẹn gặp lại!", fontHeader, Brushes.Black, new RectangleF(0, yPos, width, 20), centerFormat);
        }
    }
}