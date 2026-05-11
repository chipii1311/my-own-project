using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class PaymentForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private int currentOrderID;
        private int tableID;

        // Thẻ bài nhân viên (nhận từ POSForm)
        private int currentStaffID;
        private string currentStaffName;

        // Biến phục vụ tính toán tiền nong
        private decimal subTotal = 0;       // Tiền nguyên giá
        private decimal discountAmount = 0; // Tiền được giảm
        private decimal finalAmount = 0;    // Tiền chốt khách trả

        // Các control giao diện
        private Guna2ComboBox cboPromotion;
        private Label lblSubTotal;
        private Label lblDiscount;
        private Label lblTotalAmount;

        private Guna2Button btnPrint;
        private Guna2Button btnConfirm;
        private Guna2Button btnCancel;
        private Guna2ShadowForm shadowForm;
        private Guna2BorderlessForm borderlessForm;

        // Đồ nghề in ấn
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;

        // Cập nhật hàm khởi tạo để nhận thêm ID và Tên thu ngân
        public PaymentForm(int orderID, int tableID = -1, int staffID = 0, string staffName = "Admin")
        {
            InitializeComponent();
            this.Controls.Clear();

            this.currentOrderID = orderID;
            this.tableID = tableID;
            this.currentStaffID = staffID;
            this.currentStaffName = staffName;

            // Khởi tạo máy in khổ 80mm
            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 315, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;

            printPreview = new PrintPreviewDialog();
            printPreview.Document = printDoc;
            printPreview.StartPosition = FormStartPosition.CenterScreen;
            printPreview.Size = new Size(450, 650);
            printPreview.PrintPreviewControl.Zoom = 1.0;

            BuildPaymentUI(); // Vẽ giao diện

            this.Load += PaymentForm_Load;
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void BuildPaymentUI()
        {
            this.Size = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            shadowForm = new Guna2ShadowForm(this);
            shadowForm.ShadowColor = Color.Black;

            // --- HEADER MÀU TÍM ---
            Guna2Panel pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 60, FillColor = Color.FromArgb(88, 28, 230) };
            pnlHeader.Controls.Add(new Label { Text = "XÁC NHẬN THANH TOÁN", Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 15), AutoSize = true, BackColor = Color.Transparent });
            this.Controls.Add(pnlHeader);

            // --- KHU VỰC KHUYẾN MÃI ---
            int currentY = 80;
            this.Controls.Add(new Label { Text = "Chương trình khuyến mãi áp dụng:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(25, currentY), AutoSize = true });
            currentY += 25;

            cboPromotion = new Guna2ComboBox { Location = new Point(25, currentY), Size = new Size(430, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F) };
            this.Controls.Add(cboPromotion);
            currentY += 55;

            // --- KHU VỰC TÍNH TIỀN ---
            lblSubTotal = new Label { Text = "Tạm tính: 0 đ", Font = new Font("Segoe UI", 12F), ForeColor = Color.Black, Location = new Point(25, currentY), AutoSize = true };
            this.Controls.Add(lblSubTotal);
            currentY += 30;

            lblDiscount = new Label { Text = "Giảm giá: 0 đ", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(16, 185, 129), Location = new Point(25, currentY), AutoSize = true };
            this.Controls.Add(lblDiscount);
            currentY += 40;

            this.Controls.Add(new Label { Text = "Khách cần trả:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, Location = new Point(25, currentY), AutoSize = true });
            currentY += 25;

            lblTotalAmount = new Label { Text = "0 đ", Font = new Font("Segoe UI", 28F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 71, 87), Location = new Point(20, currentY), AutoSize = true };
            this.Controls.Add(lblTotalAmount);
            currentY += 70;

            // --- 3 NÚT BẤM ---
            btnCancel = new Guna2Button { Text = "Hủy bỏ", Size = new Size(100, 55), Location = new Point(25, currentY), BorderRadius = 8, FillColor = Color.FromArgb(235, 235, 235), ForeColor = Color.Black, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);

            btnPrint = new Guna2Button { Text = "In Hóa Đơn", Size = new Size(130, 55), Location = new Point(135, currentY), BorderRadius = 8, FillColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPrint.Click += BtnPrint_Click;
            this.Controls.Add(btnPrint);

            btnConfirm = new Guna2Button { Text = "Xác nhận Thu tiền", Size = new Size(180, 55), Location = new Point(275, currentY), BorderRadius = 8, FillColor = Color.FromArgb(88, 28, 230), ForeColor = Color.White, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            // Viền bo góc
            borderlessForm = new Guna2BorderlessForm { ContainerControl = this, BorderRadius = 15 };
        }

        #endregion

        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC
        // ========================================================

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            LoadSubTotalAmount();
            LoadActivePromotions();
            CalculateFinalAmount();
        }

        private void LoadSubTotalAmount()
        {
            try
            {
                DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
                subTotal = 0;
                foreach (DataRow row in dtDetails.Rows)
                {
                    subTotal += Convert.ToDecimal(row["SubTotal"]);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải thông tin hóa đơn: " + ex.Message); }
        }

        private void LoadActivePromotions()
        {
            try
            {
                string query = @"SELECT PromotionID, 
                                        PromotionName + ' (-' + CAST(CAST(DiscountPercent AS int) AS VARCHAR) + '%)' AS PromoDisplay, 
                                        DiscountPercent 
                                 FROM Promotion 
                                 WHERE Status = 'Active' AND CAST(GETDATE() AS DATE) BETWEEN StartDate AND EndDate";
                DataTable dtPromo = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                DataRow dr = dtPromo.NewRow();
                dr["PromotionID"] = -1;
                dr["PromoDisplay"] = "-- Không áp dụng khuyến mãi --";
                dr["DiscountPercent"] = 0;
                dtPromo.Rows.InsertAt(dr, 0);

                cboPromotion.DataSource = dtPromo;
                cboPromotion.DisplayMember = "PromoDisplay";
                cboPromotion.ValueMember = "PromotionID";
                cboPromotion.SelectedIndex = 0;

                cboPromotion.SelectedIndexChanged += (s, e) => CalculateFinalAmount();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải mã khuyến mãi: " + ex.Message); }
        }

        private void CalculateFinalAmount()
        {
            discountAmount = 0;

            if (cboPromotion.SelectedValue != null && Convert.ToInt32(cboPromotion.SelectedValue) != -1)
            {
                DataRowView drv = (DataRowView)cboPromotion.SelectedItem;
                decimal percent = Convert.ToDecimal(drv["DiscountPercent"]);
                discountAmount = subTotal * (percent / 100m);
            }

            finalAmount = subTotal - discountAmount;

            lblSubTotal.Text = $"Tạm tính: {subTotal.ToString("N0")} đ";
            lblDiscount.Text = $"Giảm giá: -{discountAmount.ToString("N0")} đ";
            lblTotalAmount.Text = finalAmount.ToString("N0") + " đ";
        }

        #endregion

        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN NÚT BẤM (EVENTS)
        // ========================================================

        private void BtnCancel_Click(object sender, EventArgs e) => this.Close();

        private void BtnPrint_Click(object sender, EventArgs e) => printPreview.ShowDialog();

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận khách đã thanh toán đủ tiền và dọn bàn?", "Chốt đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int selectedPromoID = Convert.ToInt32(cboPromotion.SelectedValue);
                    string promoSQL = (selectedPromoID == -1) ? "NULL" : selectedPromoID.ToString();

                    // Cập nhật Hóa đơn kèm ID Khuyến mãi và ID Thu ngân (Lưu ý: Bạn nhắc Việt check lại xem cột là StaffID hay EmployeeID nhé)
                    string updateOrderSQL = $@"UPDATE Orders 
                                               SET Status = 'Completed', 
                                                   TotalAmount = {finalAmount}, 
                                                   PromotionID = {promoSQL},
                                                   StaffID = {currentStaffID} 
                                               WHERE OrderID = {currentOrderID}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(updateOrderSQL);

                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE DiningTable SET Status = N'Trống' WHERE TableID = (SELECT TableID FROM Orders WHERE OrderID = {currentOrderID})");

                    MessageBox.Show("Thanh toán thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex) { MessageBox.Show("Có lỗi xảy ra: " + ex.Message); }
            }
        }

        // --- IN HÓA ĐƠN ---
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font fontTitle = new Font("Courier New", 18, FontStyle.Bold);
            Font fontSub = new Font("Courier New", 11, FontStyle.Regular);
            Font fontHeader = new Font("Courier New", 14, FontStyle.Bold);
            Font fontItem = new Font("Courier New", 11, FontStyle.Regular);
            Font fontBold = new Font("Courier New", 11, FontStyle.Bold);

            StringFormat centerAlign = new StringFormat() { Alignment = StringAlignment.Center };
            StringFormat rightAlign = new StringFormat() { Alignment = StringAlignment.Far };

            int yPos = 10;
            int leftMargin = 5;
            int centerPoint = 157;
            int rightMargin = 300;

            g.DrawString("PBL3 RESTAURANT", fontTitle, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 30;
            g.DrawString("Đ/c: ĐH Bách Khoa Đà Nẵng", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 20;
            g.DrawString("Hotline: 0123.456.789", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 35;

            g.DrawString("PHIẾU TẠM TÍNH", fontHeader, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 35;

            g.DrawString("Mã HD: " + currentOrderID, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            g.DrawString("Ngày : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            // In tên Thu ngân
            g.DrawString("Thu ngân: " + currentStaffName, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            string line = new string('-', 33);
            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            g.DrawString("Tên món", fontBold, Brushes.Black, leftMargin, yPos);
            g.DrawString("SL", fontBold, Brushes.Black, 170, yPos);
            g.DrawString("T.Tiền", fontBold, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 25;
            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
            foreach (DataRow row in dtDetails.Rows)
            {
                string name = row["ItemName"].ToString();
                if (name.Length > 15) name = name.Substring(0, 15) + "..";

                string qty = row["Quantity"].ToString();
                string sub = Convert.ToDecimal(row["SubTotal"]).ToString("N0");

                g.DrawString(name, fontItem, Brushes.Black, leftMargin, yPos);
                g.DrawString(qty, fontItem, Brushes.Black, 170, yPos);
                g.DrawString(sub, fontItem, Brushes.Black, rightMargin, yPos, rightAlign);
                yPos += 25;
            }

            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            // IN TỔNG TIỀN + KHUYẾN MÃI
            g.DrawString("Tạm tính:", fontBold, Brushes.Black, leftMargin, yPos);
            g.DrawString(subTotal.ToString("N0") + " đ", fontBold, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 25;

            if (discountAmount > 0)
            {
                g.DrawString("Khuyến mãi:", fontBold, Brushes.Black, leftMargin, yPos);
                g.DrawString("-" + discountAmount.ToString("N0") + " đ", fontBold, Brushes.Black, rightMargin, yPos, rightAlign);
                yPos += 25;
            }

            g.DrawString("TỔNG CỘNG:", fontHeader, Brushes.Black, leftMargin, yPos);
            g.DrawString(finalAmount.ToString("N0") + " đ", fontHeader, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 45;

            g.DrawString("Cảm ơn & Hẹn gặp lại!", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
        }

        #endregion
    }
}