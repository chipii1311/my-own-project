using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.DesignForms
{
    partial class POSForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Khai báo các Controls động tĩnh
        private Guna2Panel pnlCart, pnlHeader;
        private FlowLayoutPanel flpMenu, flpCategories, flpCart;
        private Guna2TextBox txtSearch;
        private Label lblTotal;
        private Guna2Button btnContinue, btnClear;
        private Guna2ComboBox cboTable;

        // Khai báo biên panel viền 
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel3;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(2077, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(2, 971);
            this.panel6.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DimGray;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 971);
            this.panel3.TabIndex = 8;
            // 
            // POSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(2079, 971);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "POSForm";
            this.Text = "POSForm";
            this.ResumeLayout(false);
        }

        #endregion

        // ─────────────────────────────────────────────────────────
        //  BUILD UI TĨNH (Chuyển từ InitializeModernPOS)
        // ─────────────────────────────────────────────────────────
        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);

            // Ép Form lấp đầy 100% không gian
            this.Padding = new Padding(0);

            // ─── 1. GIỎ HÀNG (PANEL BÊN PHẢI) ───
            pnlCart = new Guna2Panel { Dock = DockStyle.Right, Width = 500, FillColor = Color.White, CustomBorderThickness = new Padding(1, 0, 0, 0), CustomBorderColor = Color.FromArgb(235, 235, 235) };

            Guna2Panel pnlCartTop = new Guna2Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.White };
            pnlCartTop.Controls.Add(new Label { Text = "CHI TIẾT HÓA ĐƠN", Font = new Font("Segoe UI", 13F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true });

            cboTable = new Guna2ComboBox { Name = "cboTable", Location = new Point(270, 15), Size = new Size(210, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F) };
            pnlCartTop.Controls.Add(cboTable);

            Guna2Panel pnlColHeader = new Guna2Panel { Location = new Point(20, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Width = pnlCart.Width - 40, Height = 30, CustomBorderThickness = new Padding(0, 0, 0, 1), CustomBorderColor = Color.LightGray };
            pnlColHeader.Controls.Add(new Label { Text = "STT", Location = new Point(5, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Tên món", Location = new Point(35, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Thành tiền", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlColHeader.Width - 110, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Đơn giá", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlColHeader.Width - 175, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "SL", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlColHeader.Width - 235, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlCartTop.Controls.Add(pnlColHeader);

            Guna2Panel pnlCartBottom = new Guna2Panel { Dock = DockStyle.Bottom, Height = 160, BackColor = Color.White, CustomBorderThickness = new Padding(0, 1, 0, 0), CustomBorderColor = Color.FromArgb(240, 240, 240) };
            pnlCartBottom.Controls.Add(new Label { Text = "Tạm tính", Font = new Font("Segoe UI", 10F), Location = new Point(20, 15), AutoSize = true });
            pnlCartBottom.Controls.Add(new Label { Text = "Tổng cộng", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(20, 50), AutoSize = true });

            lblTotal = new Label { Name = "lblTotalAmount", Text = "0 đ", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(310, 50), Size = new Size(170, 30), TextAlign = ContentAlignment.MiddleRight };

            btnClear = new Guna2Button { Text = "Xóa tất cả", BorderRadius = 5, Size = new Size(100, 45), Location = new Point(20, 95), FillColor = Color.White, ForeColor = Color.Red, CustomBorderThickness = new Padding(1), CustomBorderColor = Color.Red, Font = new Font("Segoe UI", 10F), Cursor = Cursors.Hand };
            btnClear.Click += BtnClear_Click;

            btnContinue = new Guna2Button { Text = "Thanh toán", BorderRadius = 5, Size = new Size(350, 45), Location = new Point(130, 95), FillColor = Color.FromArgb(88, 28, 230), Font = new Font("Segoe UI", 12F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnContinue.Click += BtnContinue_Click;

            pnlCartBottom.Controls.AddRange(new Control[] { lblTotal, btnClear, btnContinue });

            flpCart = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(20, 5, 0, 0), BackColor = Color.White };

            pnlCart.Controls.Add(flpCart);
            pnlCart.Controls.Add(pnlCartTop);
            pnlCart.Controls.Add(pnlCartBottom);

            // ─── 2. HEADER TÌM KIẾM ───
            pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.Transparent };
            pnlHeader.Controls.Add(new Label { Text = "Menu Items", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(20, 15), AutoSize = true });

            txtSearch = new Guna2TextBox { Size = new Size(350, 45), Location = new Point(20, 50), BorderRadius = 20, PlaceholderText = "Search items..." };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            flpCategories = new FlowLayoutPanel { Location = new Point(390, 50), Size = new Size(600, 60), WrapContents = false, AutoScroll = true };
            pnlHeader.Controls.AddRange(new Control[] { txtSearch, flpCategories });

            // ─── 3. MENU LIST ───
            flpMenu = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20, 10, 10, 10) };

            this.Controls.Add(flpMenu);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlCart);
        }
    }
}