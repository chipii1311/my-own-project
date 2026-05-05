namespace my_own_project.VIEWSTAFF
{
    partial class StaffMainFormgpt
    {
        private System.ComponentModel.IContainer components = null;

        private Guna.UI2.WinForms.Guna2Panel sidebar;
        private Guna.UI2.WinForms.Guna2Button btnTrangChu;
        private Guna.UI2.WinForms.Guna2Button btnBan;
        private Guna.UI2.WinForms.Guna2Button btnOrder;
        private Guna.UI2.WinForms.Guna2Button btnHoaDon;

        private Guna.UI2.WinForms.Guna2Panel header;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblUser;

        private Guna.UI2.WinForms.Guna2Panel mainPanel;

        private Guna.UI2.WinForms.Guna2Panel cardTotal;
        private Guna.UI2.WinForms.Guna2Panel cardEmpty;
        private Guna.UI2.WinForms.Guna2Panel cardUsing;

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblEmpty;
        private System.Windows.Forms.Label lblUsing;

        private System.Windows.Forms.Timer timer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnHoaDon = new Guna.UI2.WinForms.Guna2Button();
            this.btnOrder = new Guna.UI2.WinForms.Guna2Button();
            this.btnBan = new Guna.UI2.WinForms.Guna2Button();
            this.btnTrangChu = new Guna.UI2.WinForms.Guna2Button();
            this.header = new Guna.UI2.WinForms.Guna2Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.mainPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.cardTotal = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.cardEmpty = new Guna.UI2.WinForms.Guna2Panel();
            this.lblEmpty = new System.Windows.Forms.Label();
            this.cardUsing = new Guna.UI2.WinForms.Guna2Panel();
            this.lblUsing = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.sidebar.SuspendLayout();
            this.header.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.cardTotal.SuspendLayout();
            this.cardEmpty.SuspendLayout();
            this.cardUsing.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebar
            // 
            this.sidebar.Controls.Add(this.btnHoaDon);
            this.sidebar.Controls.Add(this.btnOrder);
            this.sidebar.Controls.Add(this.btnBan);
            this.sidebar.Controls.Add(this.btnTrangChu);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(35)))));
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(200, 493);
            this.sidebar.TabIndex = 2;
            // 
            // btnHoaDon
            // 
            this.btnHoaDon.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHoaDon.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnHoaDon.ForeColor = System.Drawing.Color.White;
            this.btnHoaDon.Location = new System.Drawing.Point(0, 150);
            this.btnHoaDon.Name = "btnHoaDon";
            this.btnHoaDon.Size = new System.Drawing.Size(200, 50);
            this.btnHoaDon.TabIndex = 0;
            this.btnHoaDon.Text = "Hóa đơn";
            // 
            // btnOrder
            // 
            this.btnOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOrder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOrder.ForeColor = System.Drawing.Color.White;
            this.btnOrder.Location = new System.Drawing.Point(0, 100);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(200, 50);
            this.btnOrder.TabIndex = 1;
            this.btnOrder.Text = "Gọi món";
            // 
            // btnBan
            // 
            this.btnBan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBan.ForeColor = System.Drawing.Color.White;
            this.btnBan.Location = new System.Drawing.Point(0, 50);
            this.btnBan.Name = "btnBan";
            this.btnBan.Size = new System.Drawing.Size(200, 50);
            this.btnBan.TabIndex = 2;
            this.btnBan.Text = "Quản lý bàn";
            // 
            // btnTrangChu
            // 
            this.btnTrangChu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTrangChu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTrangChu.ForeColor = System.Drawing.Color.White;
            this.btnTrangChu.Location = new System.Drawing.Point(0, 0);
            this.btnTrangChu.Name = "btnTrangChu";
            this.btnTrangChu.Size = new System.Drawing.Size(200, 50);
            this.btnTrangChu.TabIndex = 3;
            this.btnTrangChu.Text = "Trang chủ";
            // 
            // header
            // 
            this.header.Controls.Add(this.lblUser);
            this.header.Controls.Add(this.lblTime);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60)))));
            this.header.Location = new System.Drawing.Point(200, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(773, 60);
            this.header.TabIndex = 1;
            // 
            // lblUser
            // 
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(20, 20);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(100, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "Staff";
            // 
            // lblTime
            // 
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(300, 20);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 1;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.cardTotal);
            this.mainPanel.Controls.Add(this.cardEmpty);
            this.mainPanel.Controls.Add(this.cardUsing);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(200, 60);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(773, 433);
            this.mainPanel.TabIndex = 0;
            // 
            // cardTotal
            // 
            this.cardTotal.BorderRadius = 15;
            this.cardTotal.Controls.Add(this.lblTotal);
            this.cardTotal.FillColor = System.Drawing.Color.MediumPurple;
            this.cardTotal.Location = new System.Drawing.Point(50, 50);
            this.cardTotal.Name = "cardTotal";
            this.cardTotal.Size = new System.Drawing.Size(200, 100);
            this.cardTotal.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.Location = new System.Drawing.Point(20, 40);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(100, 23);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Tổng bàn: 20";
            // 
            // cardEmpty
            // 
            this.cardEmpty.BorderRadius = 15;
            this.cardEmpty.Controls.Add(this.lblEmpty);
            this.cardEmpty.FillColor = System.Drawing.Color.Green;
            this.cardEmpty.Location = new System.Drawing.Point(300, 50);
            this.cardEmpty.Name = "cardEmpty";
            this.cardEmpty.Size = new System.Drawing.Size(200, 100);
            this.cardEmpty.TabIndex = 1;
            // 
            // lblEmpty
            // 
            this.lblEmpty.ForeColor = System.Drawing.Color.White;
            this.lblEmpty.Location = new System.Drawing.Point(20, 40);
            this.lblEmpty.Name = "lblEmpty";
            this.lblEmpty.Size = new System.Drawing.Size(100, 23);
            this.lblEmpty.TabIndex = 0;
            this.lblEmpty.Text = "Trống: 10";
            // 
            // cardUsing
            // 
            this.cardUsing.BorderRadius = 15;
            this.cardUsing.Controls.Add(this.lblUsing);
            this.cardUsing.FillColor = System.Drawing.Color.Red;
            this.cardUsing.Location = new System.Drawing.Point(550, 50);
            this.cardUsing.Name = "cardUsing";
            this.cardUsing.Size = new System.Drawing.Size(200, 100);
            this.cardUsing.TabIndex = 2;
            // 
            // lblUsing
            // 
            this.lblUsing.ForeColor = System.Drawing.Color.White;
            this.lblUsing.Location = new System.Drawing.Point(20, 40);
            this.lblUsing.Name = "lblUsing";
            this.lblUsing.Size = new System.Drawing.Size(100, 23);
            this.lblUsing.TabIndex = 0;
            this.lblUsing.Text = "Đang dùng: 10";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            // 
            // StaffMainFormgpt
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(973, 493);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.header);
            this.Controls.Add(this.sidebar);
            this.Name = "StaffMainFormgpt";
            this.Text = "Staff Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.sidebar.ResumeLayout(false);
            this.header.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.cardTotal.ResumeLayout(false);
            this.cardEmpty.ResumeLayout(false);
            this.cardUsing.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
