namespace my_own_project.DesignForms
{
    partial class POSForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button13 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dgvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.flpTables = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 914);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2079, 57);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2079, 49);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button13);
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1388, 49);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(691, 865);
            this.panel4.TabIndex = 4;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(240, 730);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(181, 48);
            this.button13.TabIndex = 12;
            this.button13.Text = "Thanh toan";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvName,
            this.dgvPrice,
            this.dgvQuantity,
            this.dgvTotal});
            this.dataGridView1.Location = new System.Drawing.Point(17, 71);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(662, 637);
            this.dataGridView1.TabIndex = 0;
            // 
            // dgvName
            // 
            this.dgvName.HeaderText = "Ten mon";
            this.dgvName.MinimumWidth = 8;
            this.dgvName.Name = "dgvName";
            this.dgvName.ReadOnly = true;
            // 
            // dgvPrice
            // 
            this.dgvPrice.HeaderText = "Gia tien";
            this.dgvPrice.MinimumWidth = 8;
            this.dgvPrice.Name = "dgvPrice";
            this.dgvPrice.ReadOnly = true;
            // 
            // dgvQuantity
            // 
            this.dgvQuantity.HeaderText = "So luong";
            this.dgvQuantity.MinimumWidth = 8;
            this.dgvQuantity.Name = "dgvQuantity";
            // 
            // dgvTotal
            // 
            this.dgvTotal.HeaderText = "Thanh tien";
            this.dgvTotal.MinimumWidth = 8;
            this.dgvTotal.Name = "dgvTotal";
            this.dgvTotal.ReadOnly = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(1386, 49);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(2, 865);
            this.panel6.TabIndex = 6;
            // 
            // flpTables
            // 
            this.flpTables.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpTables.Location = new System.Drawing.Point(0, 49);
            this.flpTables.Name = "flpTables";
            this.flpTables.Size = new System.Drawing.Size(367, 865);
            this.flpTables.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DimGray;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(367, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 865);
            this.panel3.TabIndex = 8;
            // 
            // POSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2079, 971);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.flpTables);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "POSForm";
            this.Text = "POSForm";
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTotal;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.FlowLayoutPanel flpTables;
        private System.Windows.Forms.Panel panel3;
    }
}