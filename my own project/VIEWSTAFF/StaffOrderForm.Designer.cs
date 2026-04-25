namespace my_own_project.VIEWSTAFF
{
    partial class StaffOrderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaffOrderForm));

            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatus = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTime = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblUser = new Guna.UI2.WinForms.Guna2HtmlLabel();

            this.pnlMain = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.lblMenuTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlCategories = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlMenuItems = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlRight = new Guna.UI2.WinForms.Guna2Panel();
            this.lblBillTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnCustomer = new Guna.UI2.WinForms.Guna2Button();
            this.pnlBillItems = new System.Windows.Forms.DataGridView();
            this.pnlBillTotal = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDiscountLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDiscount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblGrandTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblGrandTotalAmount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlFooter = new Guna.UI2.WinForms.Guna2Panel();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnHold = new Guna.UI2.WinForms.Guna2Button();
            this.btnPayment = new Guna.UI2.WinForms.Guna2Button();

            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBillItems)).BeginInit();
            this.pnlBillTotal.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // ============================================
            // HEADER PANEL
            // ============================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.pnlHeader.Controls.Add(this.lblUser);
            this.pnlHeader.Controls.Add(this.lblTime);
            this.pnlHeader.Controls.Add(this.lblStatus);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.btnBack);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.pnlHeader.Size = new System.Drawing.Size(1400, 80);
            this.pnlHeader.TabIndex = 0;

            this.btnBack.AutoRoundedCorners = true;
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BorderRadius = 18;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.btnBack.Location = new System.Drawing.Point(20, 20);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 40);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "⬅️ Quay lại";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(140, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 35);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "🍽️ GỌI MÓN - BÀN 02";
            this.lblTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblStatus.AutoSize = false;
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(140, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblStatus.Size = new System.Drawing.Size(120, 25);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "🔴 Đang sử dụng";
            this.lblStatus.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblTime.AutoSize = false;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(1100, 20);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(280, 25);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "⏰ 14:30:25 | 20/05/2025";
            this.lblTime.TextAlignment = System.Drawing.ContentAlignment.TopRight;

            this.lblUser.AutoSize = false;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(1100, 50);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(280, 25);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "👤 Nguyễn Văn An";
            this.lblUser.TextAlignment = System.Drawing.ContentAlignment.BottomRight;

            // ============================================
            // MAIN PANEL (LEFT + RIGHT)
            // ============================================
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.pnlRight);
            this.pnlMain.Controls.Add(this.pnlLeft);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 80);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1400, 570);
            this.pnlMain.TabIndex = 1;

            // ============================================
            // LEFT PANEL - MENU
            // ============================================
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Controls.Add(this.pnlMenuItems);
            this.pnlLeft.Controls.Add(this.pnlCategories);
            this.pnlLeft.Controls.Add(this.txtSearch);
            this.pnlLeft.Controls.Add(this.lblMenuTitle);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(20);
            this.pnlLeft.Size = new System.Drawing.Size(440, 570);
            this.pnlLeft.TabIndex = 0;

            this.lblMenuTitle.AutoSize = false;
            this.lblMenuTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMenuTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblMenuTitle.Location = new System.Drawing.Point(20, 20);
            this.lblMenuTitle.Name = "lblMenuTitle";
            this.lblMenuTitle.Size = new System.Drawing.Size(400, 25);
            this.lblMenuTitle.TabIndex = 0;
            this.lblMenuTitle.Text = "📋 DANH SÁCH MÓN";

            this.txtSearch.AutoRoundedCorners = true;
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtSearch.BorderRadius = 8;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.txtSearch.Location = new System.Drawing.Point(20, 55);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "🔍 Tìm món ăn...";
            this.txtSearch.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(400, 35);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);

            this.pnlCategories.AutoScroll = true;
            this.pnlCategories.BackColor = System.Drawing.Color.White;
            this.pnlCategories.Location = new System.Drawing.Point(20, 100);
            this.pnlCategories.Name = "pnlCategories";
            this.pnlCategories.Size = new System.Drawing.Size(400, 50);
            this.pnlCategories.TabIndex = 2;

            this.pnlMenuItems.AutoScroll = true;
            this.pnlMenuItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlMenuItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlMenuItems.Location = new System.Drawing.Point(20, 160);
            this.pnlMenuItems.Name = "pnlMenuItems";
            this.pnlMenuItems.Size = new System.Drawing.Size(400, 390);
            this.pnlMenuItems.TabIndex = 3;
            this.pnlMenuItems.WrapContents = false;

            // ============================================
            // RIGHT PANEL - BILL
            // ============================================
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlRight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlRight.BorderThickness = 1;
            this.pnlRight.Controls.Add(this.pnlFooter);
            this.pnlRight.Controls.Add(this.pnlBillTotal);
            this.pnlRight.Controls.Add(this.pnlBillItems);
            this.pnlRight.Controls.Add(this.btnCustomer);
            this.pnlRight.Controls.Add(this.lblBillTitle);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(440, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(20);
            this.pnlRight.Size = new System.Drawing.Size(960, 570);
            this.pnlRight.TabIndex = 1;

            this.lblBillTitle.AutoSize = false;
            this.lblBillTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblBillTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBillTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblBillTitle.Location = new System.Drawing.Point(20, 20);
            this.lblBillTitle.Name = "lblBillTitle";
            this.lblBillTitle.Size = new System.Drawing.Size(500, 25);
            this.lblBillTitle.TabIndex = 0;
            this.lblBillTitle.Text = "📝 CHI TIẾT HÓA ĐƠN";

            this.btnCustomer.AutoRoundedCorners = true;
            this.btnCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomer.BorderRadius = 15;
            this.btnCustomer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCustomer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.btnCustomer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCustomer.ForeColor = System.Drawing.Color.White;
            this.btnCustomer.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnCustomer.Location = new System.Drawing.Point(800, 15);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(140, 35);
            this.btnCustomer.TabIndex = 1;
            this.btnCustomer.Text = "👥 Khách lẻ";
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click);

            // ============================================
            // BILL ITEMS DATAGRIDVIEW
            // ============================================
            this.pnlBillItems.AllowUserToAddRows = false;
            this.pnlBillItems.AllowUserToDeleteRows = false;
            this.pnlBillItems.BackgroundColor = System.Drawing.Color.White;
            this.pnlBillItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlBillItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pnlBillItems.Location = new System.Drawing.Point(20, 60);
            this.pnlBillItems.Name = "pnlBillItems";
            this.pnlBillItems.ReadOnly = true;
            this.pnlBillItems.RowHeadersVisible = false;
            this.pnlBillItems.Size = new System.Drawing.Size(920, 280);
            this.pnlBillItems.TabIndex = 2;

            // ============================================
            // BILL TOTAL PANEL
            // ============================================
            this.pnlBillTotal.BackColor = System.Drawing.Color.White;
            this.pnlBillTotal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.pnlBillTotal.BorderThickness = 1;
            this.pnlBillTotal.Controls.Add(this.lblGrandTotalAmount);
            this.pnlBillTotal.Controls.Add(this.lblGrandTotal);
            this.pnlBillTotal.Controls.Add(this.lblDiscount);
            this.pnlBillTotal.Controls.Add(this.lblDiscountLabel);
            this.pnlBillTotal.Controls.Add(this.lblTotal);
            this.pnlBillTotal.Controls.Add(this.lblTotalLabel);
            this.pnlBillTotal.Location = new System.Drawing.Point(20, 350);
            this.pnlBillTotal.Name = "pnlBillTotal";
            this.pnlBillTotal.Padding = new System.Windows.Forms.Padding(15);
            this.pnlBillTotal.Size = new System.Drawing.Size(920, 100);
            this.pnlBillTotal.TabIndex = 3;

            this.lblTotalLabel.AutoSize = false;
            this.lblTotalLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblTotalLabel.Location = new System.Drawing.Point(15, 15);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(400, 20);
            this.lblTotalLabel.TabIndex = 0;
            this.lblTotalLabel.Text = "Tạm tính";

            this.lblTotal.AutoSize = false;
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblTotal.Location = new System.Drawing.Point(750, 15);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(155, 20);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "425,000 đ";
            this.lblTotal.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

            this.lblDiscountLabel.AutoSize = false;
            this.lblDiscountLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscountLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDiscountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblDiscountLabel.Location = new System.Drawing.Point(15, 40);
            this.lblDiscountLabel.Name = "lblDiscountLabel";
            this.lblDiscountLabel.Size = new System.Drawing.Size(400, 20);
            this.lblDiscountLabel.TabIndex = 2;
            this.lblDiscountLabel.Text = "Giảm giá";

            this.lblDiscount.AutoSize = false;
            this.lblDiscount.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDiscount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblDiscount.Location = new System.Drawing.Point(750, 40);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(155, 20);
            this.lblDiscount.TabIndex = 3;
            this.lblDiscount.Text = "0 đ";
            this.lblDiscount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

            this.lblGrandTotal.AutoSize = false;
            this.lblGrandTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblGrandTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblGrandTotal.Location = new System.Drawing.Point(15, 65);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(400, 25);
            this.lblGrandTotal.TabIndex = 4;
            this.lblGrandTotal.Text = "Tổng cộng";

            this.lblGrandTotalAmount.AutoSize = false;
            this.lblGrandTotalAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblGrandTotalAmount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.lblGrandTotalAmount.Location = new System.Drawing.Point(750, 60);
            this.lblGrandTotalAmount.Name = "lblGrandTotalAmount";
            this.lblGrandTotalAmount.Size = new System.Drawing.Size(155, 30);
            this.lblGrandTotalAmount.TabIndex = 5;
            this.lblGrandTotalAmount.Text = "425,000 đ";
            this.lblGrandTotalAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

            // ============================================
            // FOOTER PANEL - BUTTONS
            // ============================================
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlFooter.Controls.Add(this.btnPayment);
            this.pnlFooter.Controls.Add(this.btnHold);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlFooter.Location = new System.Drawing.Point(20, 470);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(15);
            this.pnlFooter.Size = new System.Drawing.Size(920, 80);
            this.pnlFooter.TabIndex = 4;

            this.btnCancel.AutoRoundedCorners = true;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 20;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btnCancel.Location = new System.Drawing.Point(15, 20);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "❌ Xóa tất cả";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.btnHold.AutoRoundedCorners = true;
            this.btnHold.BackColor = System.Drawing.Color.Transparent;
            this.btnHold.BorderRadius = 20;
            this.btnHold.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHold.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.btnHold.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnHold.ForeColor = System.Drawing.Color.White;
            this.btnHold.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.btnHold.Location = new System.Drawing.Point(165, 20);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(140, 40);
            this.btnHold.TabIndex = 1;
            this.btnHold.Text = "⏸️ Lưu tạm";
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);

            this.btnPayment.AutoRoundedCorners = true;
            this.btnPayment.BackColor = System.Drawing.Color.Transparent;
            this.btnPayment.BorderRadius = 20;
            this.btnPayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPayment.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.btnPayment.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnPayment.Location = new System.Drawing.Point(640, 15);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(265, 50);
            this.btnPayment.TabIndex = 2;
            this.btnPayment.Text = "🛒 Thanh toán";
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);

            // ============================================
            // STAFF ORDER FORM
            // ============================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 650);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StaffOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Staff Order - Gọi món";
            this.Load += new System.EventHandler(this.StaffOrderForm_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBillItems)).EndInit();
            this.pnlBillTotal.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatus;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTime;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblUser;
        private Guna.UI2.WinForms.Guna2Panel pnlMain;
        private Guna.UI2.WinForms.Guna2Panel pnlLeft;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblMenuTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Panel pnlCategories;
        private System.Windows.Forms.FlowLayoutPanel pnlMenuItems;
        private Guna.UI2.WinForms.Guna2Panel pnlRight;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBillTitle;
        private Guna.UI2.WinForms.Guna2Button btnCustomer;
        private System.Windows.Forms.DataGridView pnlBillItems;
        private Guna.UI2.WinForms.Guna2Panel pnlBillTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalLabel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDiscountLabel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDiscount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGrandTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGrandTotalAmount;
        private Guna.UI2.WinForms.Guna2Panel pnlFooter;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2Button btnHold;
        private Guna.UI2.WinForms.Guna2Button btnPayment;
    }
}