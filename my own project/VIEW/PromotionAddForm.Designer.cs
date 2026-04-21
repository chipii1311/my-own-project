namespace my_own_project.VIEW
{
    partial class PromotionAddForm
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
        
        #endregion

        private void InitializeComponent()
        {
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2GroupBox4 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.cbbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblStatus = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2GroupBox3 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dtpEndDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblEndDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dtpStartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblStartDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2GroupBox2 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.clbMenuItems = new System.Windows.Forms.CheckedListBox();
            this.lblApplyInfo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.cbbApplyType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblApplyType = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.numDiscountPercent = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.lblDiscountPercent = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtPromotionName = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblPromotionName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlFooter = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.guna2GroupBox4.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.guna2GroupBox2.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscountPercent)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.pnlHeader.Size = new System.Drawing.Size(1457, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🎯 Thêm chương trình khuyến mãi";
            this.lblTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.guna2GroupBox4);
            this.pnlContent.Controls.Add(this.guna2GroupBox3);
            this.pnlContent.Controls.Add(this.guna2GroupBox2);
            this.pnlContent.Controls.Add(this.lblApplyInfo);
            this.pnlContent.Controls.Add(this.guna2GroupBox1);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 80);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(30);
            this.pnlContent.Size = new System.Drawing.Size(1457, 677);
            this.pnlContent.TabIndex = 1;
            // 
            // guna2GroupBox4
            // 
            this.guna2GroupBox4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.guna2GroupBox4.BorderRadius = 10;
            this.guna2GroupBox4.BorderThickness = 2;
            this.guna2GroupBox4.Controls.Add(this.cbbStatus);
            this.guna2GroupBox4.Controls.Add(this.lblStatus);
            this.guna2GroupBox4.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2GroupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox4.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox4.Location = new System.Drawing.Point(30, 560);
            this.guna2GroupBox4.Name = "guna2GroupBox4";
            this.guna2GroupBox4.Padding = new System.Windows.Forms.Padding(20);
            this.guna2GroupBox4.Size = new System.Drawing.Size(1380, 120);
            this.guna2GroupBox4.TabIndex = 4;
            this.guna2GroupBox4.Text = "✅ TRẠNG THÁI";
            // 
            // cbbStatus
            // 
            this.cbbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cbbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbStatus.FocusedColor = System.Drawing.Color.Empty;
            this.cbbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbbStatus.ItemHeight = 30;
            this.cbbStatus.Location = new System.Drawing.Point(20, 75);
            this.cbbStatus.Name = "cbbStatus";
            this.cbbStatus.Size = new System.Drawing.Size(200, 36);
            this.cbbStatus.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblStatus.Location = new System.Drawing.Point(20, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(71, 19);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // guna2GroupBox3
            // 
            this.guna2GroupBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.guna2GroupBox3.BorderRadius = 10;
            this.guna2GroupBox3.BorderThickness = 2;
            this.guna2GroupBox3.Controls.Add(this.dtpEndDate);
            this.guna2GroupBox3.Controls.Add(this.lblEndDate);
            this.guna2GroupBox3.Controls.Add(this.dtpStartDate);
            this.guna2GroupBox3.Controls.Add(this.lblStartDate);
            this.guna2GroupBox3.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2GroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox3.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox3.Location = new System.Drawing.Point(30, 410);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Padding = new System.Windows.Forms.Padding(20);
            this.guna2GroupBox3.Size = new System.Drawing.Size(1380, 150);
            this.guna2GroupBox3.TabIndex = 3;
            this.guna2GroupBox3.Text = "📅 THỜI GIAN ÁP DỤNG";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Checked = true;
            this.dtpEndDate.FillColor = System.Drawing.Color.Gainsboro;
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpEndDate.Location = new System.Drawing.Point(350, 75);
            this.dtpEndDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpEndDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(280, 55);
            this.dtpEndDate.TabIndex = 3;
            this.dtpEndDate.Value = new System.DateTime(2026, 4, 21, 0, 0, 0, 0);
            // 
            // lblEndDate
            // 
            this.lblEndDate.BackColor = System.Drawing.Color.Transparent;
            this.lblEndDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEndDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblEndDate.Location = new System.Drawing.Point(350, 50);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(93, 19);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "Ngày kết thúc:";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Checked = true;
            this.dtpStartDate.FillColor = System.Drawing.Color.Gainsboro;
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpStartDate.Location = new System.Drawing.Point(20, 75);
            this.dtpStartDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpStartDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(280, 55);
            this.dtpStartDate.TabIndex = 1;
            this.dtpStartDate.Value = new System.DateTime(2026, 4, 21, 0, 0, 0, 0);
            // 
            // lblStartDate
            // 
            this.lblStartDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStartDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblStartDate.Location = new System.Drawing.Point(20, 50);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(90, 19);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Ngày bắt đầu:";
            // 
            // guna2GroupBox2
            // 
            this.guna2GroupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.guna2GroupBox2.BorderRadius = 10;
            this.guna2GroupBox2.BorderThickness = 2;
            this.guna2GroupBox2.Controls.Add(this.clbMenuItems);
            this.guna2GroupBox2.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2GroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox2.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox2.Location = new System.Drawing.Point(30, 210);
            this.guna2GroupBox2.Name = "guna2GroupBox2";
            this.guna2GroupBox2.Padding = new System.Windows.Forms.Padding(20);
            this.guna2GroupBox2.Size = new System.Drawing.Size(1380, 200);
            this.guna2GroupBox2.TabIndex = 2;
            this.guna2GroupBox2.Text = "🍽️ DANH SÁCH MÓN ĂN";
            // 
            // clbMenuItems
            // 
            this.clbMenuItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(245)))));
            this.clbMenuItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbMenuItems.CheckOnClick = true;
            this.clbMenuItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbMenuItems.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.clbMenuItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.clbMenuItems.FormattingEnabled = true;
            this.clbMenuItems.Location = new System.Drawing.Point(20, 60);
            this.clbMenuItems.Margin = new System.Windows.Forms.Padding(5);
            this.clbMenuItems.Name = "clbMenuItems";
            this.clbMenuItems.Size = new System.Drawing.Size(1340, 120);
            this.clbMenuItems.TabIndex = 0;
            // 
            // lblApplyInfo
            // 
            this.lblApplyInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblApplyInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblApplyInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.lblApplyInfo.Location = new System.Drawing.Point(30, 230);
            this.lblApplyInfo.Name = "lblApplyInfo";
            this.lblApplyInfo.Size = new System.Drawing.Size(241, 17);
            this.lblApplyInfo.TabIndex = 1;
            this.lblApplyInfo.Text = "✓ Khuyến mãi sẽ áp dụng cho tất cả hóa đơn";
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.guna2GroupBox1.BorderRadius = 10;
            this.guna2GroupBox1.BorderThickness = 2;
            this.guna2GroupBox1.Controls.Add(this.cbbApplyType);
            this.guna2GroupBox1.Controls.Add(this.lblApplyType);
            this.guna2GroupBox1.Controls.Add(this.numDiscountPercent);
            this.guna2GroupBox1.Controls.Add(this.lblDiscountPercent);
            this.guna2GroupBox1.Controls.Add(this.txtPromotionName);
            this.guna2GroupBox1.Controls.Add(this.lblPromotionName);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox1.Location = new System.Drawing.Point(30, 30);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Padding = new System.Windows.Forms.Padding(20);
            this.guna2GroupBox1.Size = new System.Drawing.Size(1380, 180);
            this.guna2GroupBox1.TabIndex = 0;
            this.guna2GroupBox1.Text = "📋 THÔNG TIN CƠ BẢN";
            // 
            // cbbApplyType
            // 
            this.cbbApplyType.BackColor = System.Drawing.Color.Transparent;
            this.cbbApplyType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbbApplyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbApplyType.FocusedColor = System.Drawing.Color.Empty;
            this.cbbApplyType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbApplyType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbbApplyType.ItemHeight = 30;
            this.cbbApplyType.Location = new System.Drawing.Point(500, 75);
            this.cbbApplyType.Name = "cbbApplyType";
            this.cbbApplyType.Size = new System.Drawing.Size(410, 36);
            this.cbbApplyType.TabIndex = 5;
            this.cbbApplyType.SelectedIndexChanged += new System.EventHandler(this.cbbApplyType_SelectedIndexChanged);
            // 
            // lblApplyType
            // 
            this.lblApplyType.BackColor = System.Drawing.Color.Transparent;
            this.lblApplyType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblApplyType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblApplyType.Location = new System.Drawing.Point(500, 50);
            this.lblApplyType.Name = "lblApplyType";
            this.lblApplyType.Size = new System.Drawing.Size(88, 19);
            this.lblApplyType.TabIndex = 4;
            this.lblApplyType.Text = "Loại áp dụng:";
            // 
            // numDiscountPercent
            // 
            this.numDiscountPercent.BackColor = System.Drawing.Color.Transparent;
            this.numDiscountPercent.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numDiscountPercent.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numDiscountPercent.Location = new System.Drawing.Point(330, 75);
            this.numDiscountPercent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDiscountPercent.Name = "numDiscountPercent";
            this.numDiscountPercent.Size = new System.Drawing.Size(120, 35);
            this.numDiscountPercent.TabIndex = 3;
            this.numDiscountPercent.UpDownButtonFillColor = System.Drawing.Color.LightCoral;
            // 
            // lblDiscountPercent
            // 
            this.lblDiscountPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscountPercent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiscountPercent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblDiscountPercent.Location = new System.Drawing.Point(330, 50);
            this.lblDiscountPercent.Name = "lblDiscountPercent";
            this.lblDiscountPercent.Size = new System.Drawing.Size(98, 19);
            this.lblDiscountPercent.TabIndex = 2;
            this.lblDiscountPercent.Text = "Mức giảm (%) :";
            // 
            // txtPromotionName
            // 
            this.txtPromotionName.AutoRoundedCorners = true;
            this.txtPromotionName.BackColor = System.Drawing.Color.White;
            this.txtPromotionName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtPromotionName.BorderRadius = 16;
            this.txtPromotionName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionName.DefaultText = "";
            this.txtPromotionName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPromotionName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtPromotionName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.txtPromotionName.Location = new System.Drawing.Point(20, 75);
            this.txtPromotionName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPromotionName.Name = "txtPromotionName";
            this.txtPromotionName.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.txtPromotionName.PlaceholderText = "Nhập tên chương trình khuyến mãi...";
            this.txtPromotionName.SelectedText = "";
            this.txtPromotionName.Size = new System.Drawing.Size(280, 35);
            this.txtPromotionName.TabIndex = 1;
            // 
            // lblPromotionName
            // 
            this.lblPromotionName.BackColor = System.Drawing.Color.Transparent;
            this.lblPromotionName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPromotionName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblPromotionName.Location = new System.Drawing.Point(20, 50);
            this.lblPromotionName.Name = "lblPromotionName";
            this.lblPromotionName.Size = new System.Drawing.Size(74, 19);
            this.lblPromotionName.TabIndex = 0;
            this.lblPromotionName.Text = "Tên ưu đãi:";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlFooter.Location = new System.Drawing.Point(0, 757);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.pnlFooter.Size = new System.Drawing.Size(1457, 80);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.AutoRoundedCorners = true;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderRadius = 19;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(142)))), ((int)(((byte)(60)))));
            this.btnSave.Location = new System.Drawing.Point(30, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "💾 LƯU";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnClose
            // 
            this.btnClose.AutoRoundedCorners = true;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BorderRadius = 19;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FillColor = System.Drawing.Color.LightCoral;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btnClose.Location = new System.Drawing.Point(160, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "❌ ĐÓNG";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // PromotionAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1457, 837);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromotionAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm khuyến mãi";
            this.Load += new System.EventHandler(this.PromotionAddForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.guna2GroupBox4.ResumeLayout(false);
            this.guna2GroupBox4.PerformLayout();
            this.guna2GroupBox3.ResumeLayout(false);
            this.guna2GroupBox3.PerformLayout();
            this.guna2GroupBox2.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscountPercent)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        // ============================================
        // CONTROL DECLARATIONS - GUNA UI2
        // ============================================
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPromotionName;
        private Guna.UI2.WinForms.Guna2TextBox txtPromotionName;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDiscountPercent;
        private Guna.UI2.WinForms.Guna2NumericUpDown numDiscountPercent;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblApplyType;
        private Guna.UI2.WinForms.Guna2ComboBox cbbApplyType;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblApplyInfo;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox2;
        private System.Windows.Forms.CheckedListBox clbMenuItems;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox3;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStartDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpStartDate;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEndDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpEndDate;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox4;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatus;
        private Guna.UI2.WinForms.Guna2ComboBox cbbStatus;
        private Guna.UI2.WinForms.Guna2Panel pnlFooter;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnClose;
    }
}