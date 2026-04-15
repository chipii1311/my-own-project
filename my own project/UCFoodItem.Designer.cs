namespace my_own_project
{
    partial class UCFoodItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picImage = new System.Windows.Forms.PictureBox();
            this.lblFoodName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.nudQuantity = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.btnBuy = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.picImage.Location = new System.Drawing.Point(0, 0);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(170, 134);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // lblFoodName
            // 
            this.lblFoodName.BackColor = System.Drawing.SystemColors.Control;
            this.lblFoodName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFoodName.Location = new System.Drawing.Point(0, 134);
            this.lblFoodName.Name = "lblFoodName";
            this.lblFoodName.Size = new System.Drawing.Size(170, 20);
            this.lblFoodName.TabIndex = 3;
            this.lblFoodName.Text = "label1";
            this.lblFoodName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPrice
            // 
            this.lblPrice.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPrice.ForeColor = System.Drawing.Color.Blue;
            this.lblPrice.Location = new System.Drawing.Point(0, 154);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(170, 20);
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "label1";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudQuantity
            // 
            this.nudQuantity.BackColor = System.Drawing.Color.Transparent;
            this.nudQuantity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nudQuantity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nudQuantity.Location = new System.Drawing.Point(4, 176);
            this.nudQuantity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(63, 33);
            this.nudQuantity.TabIndex = 6;
            // 
            // btnBuy
            // 
            this.btnBuy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBuy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBuy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBuy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBuy.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBuy.ForeColor = System.Drawing.Color.White;
            this.btnBuy.Location = new System.Drawing.Point(74, 176);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(96, 33);
            this.btnBuy.TabIndex = 7;
            this.btnBuy.Text = "Thêm";
            // 
            // UCFoodItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBuy);
            this.Controls.Add(this.nudQuantity);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblFoodName);
            this.Controls.Add(this.picImage);
            this.Name = "UCFoodItem";
            this.Size = new System.Drawing.Size(170, 212);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Label lblFoodName;
        private System.Windows.Forms.Label lblPrice;
        private Guna.UI2.WinForms.Guna2NumericUpDown nudQuantity;
        private Guna.UI2.WinForms.Guna2Button btnBuy;
    }
}
