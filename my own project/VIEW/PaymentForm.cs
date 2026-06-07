using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
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
        // KHAI BÁO BIẾN DỮ LIỆU
        // ========================================================
        private int currentOrderID;
        private int tableID;
        private int currentStaffID;
        private string currentStaffName;

        private decimal subTotal = 0;
        private decimal discountAmount = 0;
        private decimal finalAmount = 0;

        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;

        public PaymentForm(int orderID, int tableID = -1, int staffID = 0, string staffName = "Admin")
        {
            InitializeComponent();

            this.currentOrderID = orderID;
            this.tableID = tableID;
            this.currentStaffID = staffID;
            this.currentStaffName = staffName;

            BuildUI();

            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 315, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;

            printPreview = new PrintPreviewDialog();
            printPreview.Document = printDoc;
            printPreview.StartPosition = FormStartPosition.CenterScreen;
            printPreview.Size = new Size(450, 650);
            printPreview.PrintPreviewControl.Zoom = 1.0;

            this.Load += PaymentForm_Load;
        }

        // ========================================================
        // 1. KHU VỰC DATA BINDING & TÍNH TOÁN
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
                    subTotal += Convert.ToDecimal(row["SubTotal"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin hóa đơn: " + ex.Message);
            }
        }

        private void LoadActivePromotions()
        {
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL thay vì viết SQL thuần trực tiếp
                DataTable dtPromo = PromotionBLL.GetActivePromotionsForOrder(currentOrderID);

                // Thêm dòng "Không áp dụng" ở đầu
                DataRow dr = dtPromo.NewRow();
                dr["PromotionID"] = -1;
                dr["PromoDisplay"] = "-- Không áp dụng khuyến mãi --";
                dr["DiscountPercent"] = 0;
                dr["ApplyType"] = 0;
                dtPromo.Rows.InsertAt(dr, 0);

                cboPromotion.SelectedIndexChanged -= CboPromotion_SelectedIndexChanged;

                cboPromotion.DataSource = dtPromo;
                cboPromotion.DisplayMember = "PromoDisplay";
                cboPromotion.ValueMember = "PromotionID";
                cboPromotion.SelectedIndex = 0;

                cboPromotion.SelectedIndexChanged += CboPromotion_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải mã khuyến mãi: " + ex.Message);
            }
        }

        private void CboPromotion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateFinalAmount();
        }

        private void CalculateFinalAmount()
        {
            discountAmount = 0;

            if (cboPromotion.SelectedValue != null && Convert.ToInt32(cboPromotion.SelectedValue) != -1)
            {
                DataRowView drv = (DataRowView)cboPromotion.SelectedItem;
                decimal percent = Convert.ToDecimal(drv["DiscountPercent"]);
                int applyType = Convert.ToInt32(drv["ApplyType"]);
                int promoID = Convert.ToInt32(drv["PromotionID"]);

                // [ĐÃ SỬA]: Gọi qua BLL thay vì DataHelper.ExecuteScalar() trực tiếp
                discountAmount = PromotionBLL.CalculateDiscountForOrder(
                    currentOrderID, promoID, applyType, subTotal, percent);
            }

            finalAmount = subTotal - discountAmount;

            lblSubTotal.Text = $"Tạm tính: {subTotal.ToString("N0")} đ";
            lblDiscount.Text = $"Giảm giá: -{discountAmount.ToString("N0")} đ";
            lblTotalAmount.Text = finalAmount.ToString("N0") + " đ";
        }

        // ========================================================
        // 2. KHU VỰC SỰ KIỆN NÚT BẤM
        // ========================================================
        public void BtnCancel_Click(object sender, EventArgs e) => this.Close();

        public void BtnPrint_Click(object sender, EventArgs e) => printPreview.ShowDialog();

        public void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Xác nhận khách đã thanh toán đủ tiền và dọn bàn?",
                    "Chốt đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int selectedPromoID = Convert.ToInt32(cboPromotion.SelectedValue);
                    int? promoID = (selectedPromoID == -1) ? (int?)null : selectedPromoID;

                    // 1. Hoàn tất đơn hàng qua BLL
                    OrderBLL.CompleteOrder(currentOrderID, finalAmount, promoID, currentStaffID);

                    // 2. Lưu thanh toán qua BLL
                    PaymentDTO newPayment = new PaymentDTO
                    {
                        OrderID = currentOrderID,
                        Method = cboPaymentMethod.Text,
                        Amount = finalAmount
                    };
                    PaymentBLL.CreatePayment(newPayment);

                    // 3. Giải phóng bàn qua BLL
                    if (tableID > 0)
                        DiningTableBLL.UpdateStatus(tableID, "Trống");

                    MessageBox.Show("Thanh toán thành công!", "Hoàn tất",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi Thanh Toán",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ========================================================
        // 3. KHU VỰC IN HÓA ĐƠN GDI+
        // ========================================================
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

            // Gọi qua BLL (đã đúng từ trước)
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
            yPos += 30;

            g.DrawString("Hình thức TT:", fontItem, Brushes.Black, leftMargin, yPos);
            g.DrawString(cboPaymentMethod.Text, fontItem, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 45;

            g.DrawString("Cảm ơn & Hẹn gặp lại!", fontSub, Brushes.Black,
                          new PointF(centerPoint, yPos), centerAlign);
        }
    }
}