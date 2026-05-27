using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class OrderDetailView
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== DESIGN TOKENS =====================
        private static readonly Color C_BG = Color.White;
        private static readonly Color C_TEXT = Color.FromArgb(31, 41, 55);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);
        private static readonly Color C_GRAY_BG = Color.FromArgb(249, 250, 251);
        private static readonly Color C_WHITE = Color.White; // <- added

        // ===================== CONTROLS =====================
        private Label lblOrderID, lblOrderDate, lblCashier, lblTable, lblStatus;
        private Label lblSubTotal, lblDiscount, lblFinalTotal;
        private Guna2DataGridView dgvDetails;
        private Guna2Button btnClose, btnPrint;

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            // Setup Form là một Popup nhỏ gọn
            this.ClientSize = new System.Drawing.Size(550, 750);
            this.Name = "OrderDetailView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết Hóa đơn";
            this.ResumeLayout(false);
        }
        #endregion

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // ─── 1. HEADER & ORDER INFO ───
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 180, BackColor = Color.Transparent };

            Label lblTitle = new Label { Text = "CHI TIẾT HÓA ĐƠN", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = C_PURPLE, Location = new Point(25, 20), AutoSize = true };
            lblOrderID = new Label { Text = "# 00000", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = C_TEXT, Location = new Point(410, 22), AutoSize = false, Size = new Size(110, 30), TextAlign = ContentAlignment.TopRight };

            // Vẽ đường gạch ngang đứt khúc (Dashed Line) mô phỏng biên lai
            Panel div1 = new Panel { Location = new Point(25, 60), Size = new Size(495, 1) };
            div1.Paint += (s, e) => { using (Pen p = new Pen(C_BORDER, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }) e.Graphics.DrawLine(p, 0, 0, div1.Width, 0); };

            // Các thông tin cơ bản
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblOrderID);
            pnlHeader.Controls.Add(div1);
            pnlHeader.Controls.Add(MakeInfoRow("Ngày tạo:", out lblOrderDate, 25, 75));
            pnlHeader.Controls.Add(MakeInfoRow("Thu ngân:", out lblCashier, 25, 105));
            pnlHeader.Controls.Add(MakeInfoRow("Khu vực / Bàn:", out lblTable, 25, 135));
            pnlHeader.Controls.Add(MakeInfoRow("Trạng thái:", out lblStatus, 320, 75)); // Nằm góc phải

            this.Controls.Add(pnlHeader);

            // ─── 2. DANH SÁCH MÓN (DATA GRID) ───
            Panel pnlGridWrap = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 0, 25, 10) };

            dgvDetails = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = C_BG,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = C_BORDER,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            };

            // Style Lưới
            dgvDetails.ThemeStyle.HeaderStyle.BackColor = C_GRAY_BG;
            dgvDetails.ThemeStyle.HeaderStyle.ForeColor = C_MUTED;
            dgvDetails.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvDetails.ThemeStyle.HeaderStyle.Height = 40;

            dgvDetails.DefaultCellStyle.BackColor = C_BG;
            dgvDetails.DefaultCellStyle.ForeColor = C_TEXT;
            dgvDetails.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvDetails.DefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
            dgvDetails.DefaultCellStyle.SelectionForeColor = C_TEXT;
            dgvDetails.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0);
            dgvDetails.RowTemplate.Height = 40;

            pnlGridWrap.Controls.Add(dgvDetails);
            this.Controls.Add(pnlGridWrap);
            pnlGridWrap.BringToFront();

            // ─── 3. TỔNG KẾT & FOOTER (BOTTOM) ───
            Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 220, BackColor = Color.Transparent };

            Panel div2 = new Panel { Location = new Point(25, 10), Size = new Size(495, 1) };
            div2.Paint += (s, e) => { using (Pen p = new Pen(C_BORDER, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }) e.Graphics.DrawLine(p, 0, 0, div2.Width, 0); };

            pnlFooter.Controls.Add(div2);
            pnlFooter.Controls.Add(MakeInfoRow("Tạm tính:", out lblSubTotal, 25, 25, true));
            pnlFooter.Controls.Add(MakeInfoRow("Khuyến mãi:", out lblDiscount, 25, 55, true));

            Panel div3 = new Panel { Location = new Point(25, 90), Size = new Size(495, 1), BackColor = C_BORDER };
            pnlFooter.Controls.Add(div3);

            // TỔNG CỘNG LỚN
            Label lblTotalText = new Label { Text = "TỔNG CỘNG:", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = C_TEXT, Location = new Point(25, 105), AutoSize = true };
            lblFinalTotal = new Label { Text = "0 đ", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = C_PURPLE, Location = new Point(270, 100), AutoSize = false, Size = new Size(250, 35), TextAlign = ContentAlignment.TopRight };
            pnlFooter.Controls.Add(lblTotalText);
            pnlFooter.Controls.Add(lblFinalTotal);

            // Nút bấm
            btnClose = new Guna2Button { Text = "Đóng", Size = new Size(120, 45), Location = new Point(260, 155), BorderRadius = 8, FillColor = C_GRAY_BG, ForeColor = C_MUTED, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnClose.Click += BtnClose_Click;

            btnPrint = new Guna2Button { Text = "🖨️ In hóa đơn", Size = new Size(140, 45), Location = new Point(390, 155), BorderRadius = 8, FillColor = C_PURPLE, ForeColor = C_WHITE, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPrint.Click += BtnPrint_Click;

            pnlFooter.Controls.Add(btnClose);
            pnlFooter.Controls.Add(btnPrint);

            this.Controls.Add(pnlFooter);
        }

        // ===================== UI HELPERS =====================
        private Panel MakeInfoRow(string labelText, out Label valueLabel, int x, int y, bool isRightAlign = false)
        {
            Panel p = new Panel { Location = new Point(x, y), Size = new Size(isRightAlign ? 495 : 200, 25), BackColor = Color.Transparent };

            Label lbl = new Label { Text = labelText, Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, Location = new Point(0, 0), AutoSize = true };

            valueLabel = new Label
            {
                Text = "---",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = !isRightAlign
            };

            if (isRightAlign)
            {
                valueLabel.Size = new Size(250, 25);
                valueLabel.Location = new Point(p.Width - 250, 0);
                valueLabel.TextAlign = ContentAlignment.TopRight;
            }
            else
            {
                valueLabel.Location = new Point(90, 0); // Khoảng cách cố định cho phần Info
            }

            p.Controls.Add(lbl);
            p.Controls.Add(valueLabel);
            return p;
        }
    }
}