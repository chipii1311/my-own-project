using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class PaymentForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== CONTROLS GIAO DIỆN =====================
        private Guna2ComboBox cboPromotion;
        private Guna2ComboBox cboPaymentMethod;
        private Label lblSubTotal;
        private Label lblDiscount;
        private Label lblTotalAmount;
        private Guna2Button btnPrint;
        private Guna2Button btnConfirm;
        private Guna2Button btnCancel;
        private Guna2ShadowForm shadowForm;
        private Guna2BorderlessForm borderlessForm;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 560);
            this.Name = "PaymentForm";
            this.Text = "PaymentForm";
            this.ResumeLayout(false);
        }

        #endregion

        // ========================================================
        // VẼ GIAO DIỆN TĨNH (UI BUILDER)
        // ========================================================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.Size = new Size(480, 560);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            shadowForm = new Guna2ShadowForm(this);
            shadowForm.ShadowColor = Color.Black;

            // --- HEADER MÀU TÍM ---
            Guna2Panel pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 60, FillColor = Color.FromArgb(88, 28, 230) };
            pnlHeader.Controls.Add(new Label { Text = "XÁC NHẬN THANH TOÁN", Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 15), AutoSize = true, BackColor = Color.Transparent });
            this.Controls.Add(pnlHeader);

            int currentY = 80;

            // --- KHU VỰC KHUYẾN MÃI ---
            this.Controls.Add(new Label { Text = "Chương trình khuyến mãi áp dụng:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(25, currentY), AutoSize = true });
            currentY += 25;

            cboPromotion = new Guna2ComboBox { Location = new Point(25, currentY), Size = new Size(430, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F) };
            this.Controls.Add(cboPromotion);
            currentY += 55;

            // --- KHU VỰC PHƯƠNG THỨC THANH TOÁN ---
            this.Controls.Add(new Label { Text = "Phương thức thanh toán:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.Gray, Location = new Point(25, currentY), AutoSize = true });
            currentY += 25;

            cboPaymentMethod = new Guna2ComboBox { Location = new Point(25, currentY), Size = new Size(430, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F) };
            cboPaymentMethod.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản" });
            cboPaymentMethod.SelectedIndex = 0; // Mặc định
            this.Controls.Add(cboPaymentMethod);
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

            // --- 3 NÚT BẤM (Hủy - In - Xác Nhận) ---
            btnCancel = new Guna2Button { Text = "Hủy bỏ", Size = new Size(100, 55), Location = new Point(25, currentY), BorderRadius = 8, FillColor = Color.FromArgb(235, 235, 235), ForeColor = Color.Black, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCancel.Click += BtnCancel_Click; // Gọi hàm bên file .cs
            this.Controls.Add(btnCancel);

            btnPrint = new Guna2Button { Text = "In Hóa Đơn", Size = new Size(130, 55), Location = new Point(135, currentY), BorderRadius = 8, FillColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPrint.Click += BtnPrint_Click;
            this.Controls.Add(btnPrint);

            btnConfirm = new Guna2Button { Text = "Xác nhận Thu tiền", Size = new Size(180, 55), Location = new Point(275, currentY), BorderRadius = 8, FillColor = Color.FromArgb(88, 28, 230), ForeColor = Color.White, Font = new Font("Segoe UI", 11F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            // Viền bo góc bằng Guna2BorderlessForm
            borderlessForm = new Guna2BorderlessForm { ContainerControl = this, BorderRadius = 15 };
        }
    }
}