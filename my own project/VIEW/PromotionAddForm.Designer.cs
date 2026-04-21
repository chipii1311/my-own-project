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
        private void InitializeComponent()
        {
            this.txtPromotionName = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numDiscountPercent = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.dtpStartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.cbbApplyType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.clbMenuItems = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscountPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(205, 38);
            this.label1.Text = "Add Promotion";
            // 
            // panel1
            // 
            this.panel1.Size = new System.Drawing.Size(1178, 100);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 686);
            this.panel2.Size = new System.Drawing.Size(1178, 100);
            // 
            // btnClose
            // 
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // txtPromotionName
            // 
            this.txtPromotionName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionName.DefaultText = "";
            this.txtPromotionName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPromotionName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPromotionName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPromotionName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPromotionName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPromotionName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPromotionName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPromotionName.Location = new System.Drawing.Point(64, 182);
            this.txtPromotionName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPromotionName.Name = "txtPromotionName";
            this.txtPromotionName.PlaceholderText = "";
            this.txtPromotionName.SelectedText = "";
            this.txtPromotionName.Size = new System.Drawing.Size(236, 48);
            this.txtPromotionName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tên ưu đãi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 28);
            this.label3.TabIndex = 5;
            this.label3.Text = "Phần trăm ưu đãi";
            // 
            // numDiscountPercent
            // 
            this.numDiscountPercent.BackColor = System.Drawing.Color.Transparent;
            this.numDiscountPercent.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numDiscountPercent.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numDiscountPercent.Location = new System.Drawing.Point(348, 182);
            this.numDiscountPercent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numDiscountPercent.Name = "numDiscountPercent";
            this.numDiscountPercent.Size = new System.Drawing.Size(168, 48);
            this.numDiscountPercent.TabIndex = 6;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Checked = true;
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpStartDate.Location = new System.Drawing.Point(69, 596);
            this.dtpStartDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpStartDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(227, 60);
            this.dtpStartDate.TabIndex = 7;
            this.dtpStartDate.Value = new System.DateTime(2026, 4, 20, 22, 44, 31, 148);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(73, 565);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 28);
            this.label4.TabIndex = 8;
            this.label4.Text = "Ngày bắt đầu";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(374, 565);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 28);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ngày kết thúc";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Checked = true;
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpEndDate.Location = new System.Drawing.Point(370, 596);
            this.dtpEndDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpEndDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(227, 60);
            this.dtpEndDate.TabIndex = 9;
            this.dtpEndDate.Value = new System.DateTime(2026, 4, 20, 22, 44, 31, 148);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(576, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 28);
            this.label6.TabIndex = 12;
            this.label6.Text = "Loại áp dụng";
            // 
            // cbbApplyType
            // 
            this.cbbApplyType.BackColor = System.Drawing.Color.Transparent;
            this.cbbApplyType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbbApplyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbApplyType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbbApplyType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbbApplyType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbApplyType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbbApplyType.ItemHeight = 30;
            this.cbbApplyType.Location = new System.Drawing.Point(571, 194);
            this.cbbApplyType.Name = "cbbApplyType";
            this.cbbApplyType.Size = new System.Drawing.Size(246, 36);
            this.cbbApplyType.TabIndex = 14;
            this.cbbApplyType.SelectedIndexChanged += new System.EventHandler(this.cbbApplyType_SelectedIndexChanged);
            // 
            // clbMenuItems
            // 
            this.clbMenuItems.FormattingEnabled = true;
            this.clbMenuItems.Location = new System.Drawing.Point(78, 327);
            this.clbMenuItems.Name = "clbMenuItems";
            this.clbMenuItems.Size = new System.Drawing.Size(402, 159);
            this.clbMenuItems.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(88, 287);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 28);
            this.label7.TabIndex = 16;
            this.label7.Text = "Danh sách món ăn áp dụng";
            // 
            // cbbStatus
            // 
            this.cbbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cbbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbbStatus.ItemHeight = 30;
            this.cbbStatus.Location = new System.Drawing.Point(581, 450);
            this.cbbStatus.Name = "cbbStatus";
            this.cbbStatus.Size = new System.Drawing.Size(140, 36);
            this.cbbStatus.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(586, 410);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 28);
            this.label8.TabIndex = 18;
            this.label8.Text = "Trạng thái";
            // 
            // PromotionAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 786);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbbStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.clbMenuItems);
            this.Controls.Add(this.cbbApplyType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.numDiscountPercent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPromotionName);
            this.Name = "PromotionAddForm";
            this.Text = "PromotionAddForm";
            this.Load += new System.EventHandler(this.PromotionAddForm_Load);
            this.Controls.SetChildIndex(this.txtPromotionName, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.numDiscountPercent, 0);
            this.Controls.SetChildIndex(this.dtpStartDate, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dtpEndDate, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.cbbApplyType, 0);
            this.Controls.SetChildIndex(this.clbMenuItems, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.cbbStatus, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numDiscountPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtPromotionName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2NumericUpDown numDiscountPercent;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2ComboBox cbbApplyType;
        private System.Windows.Forms.CheckedListBox clbMenuItems;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2ComboBox cbbStatus;
        private System.Windows.Forms.Label label8;
    }
}