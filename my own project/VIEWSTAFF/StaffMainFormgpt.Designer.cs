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
            this.btnTrangChu = new Guna.UI2.WinForms.Guna2Button();
            this.btnBan = new Guna.UI2.WinForms.Guna2Button();
            this.btnOrder = new Guna.UI2.WinForms.Guna2Button();
            this.btnHoaDon = new Guna.UI2.WinForms.Guna2Button();

            this.header = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();

            this.mainPanel = new Guna.UI2.WinForms.Guna2Panel();

            this.cardTotal = new Guna.UI2.WinForms.Guna2Panel();
            this.cardEmpty = new Guna.UI2.WinForms.Guna2Panel();
            this.cardUsing = new Guna.UI2.WinForms.Guna2Panel();

            this.lblTotal = new System.Windows.Forms.Label();
            this.lblEmpty = new System.Windows.Forms.Label();
            this.lblUsing = new System.Windows.Forms.Label();

            this.timer = new System.Windows.Forms.Timer(this.components);

            // Form
            this.Text = "Staff Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 45);

            // SIDEBAR
            sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            sidebar.Width = 200;
            sidebar.FillColor = System.Drawing.Color.FromArgb(20, 20, 35);

            // Buttons
            btnTrangChu.Text = "Trang chủ";
            btnTrangChu.Dock = System.Windows.Forms.DockStyle.Top;
            btnTrangChu.Height = 50;
            btnTrangChu.Click += BtnTrangChu_Click;

            btnBan.Text = "Quản lý bàn";
            btnBan.Dock = System.Windows.Forms.DockStyle.Top;
            btnBan.Height = 50;
            btnBan.Click += BtnBan_Click;

            btnOrder.Text = "Gọi món";
            btnOrder.Dock = System.Windows.Forms.DockStyle.Top;
            btnOrder.Height = 50;
            btnOrder.Click += BtnOrder_Click;

            btnHoaDon.Text = "Hóa đơn";
            btnHoaDon.Dock = System.Windows.Forms.DockStyle.Top;
            btnHoaDon.Height = 50;
            btnHoaDon.Click += BtnHoaDon_Click;

            sidebar.Controls.Add(btnHoaDon);
            sidebar.Controls.Add(btnOrder);
            sidebar.Controls.Add(btnBan);
            sidebar.Controls.Add(btnTrangChu);

            // HEADER
            header.Dock = System.Windows.Forms.DockStyle.Top;
            header.Height = 60;
            header.FillColor = System.Drawing.Color.FromArgb(40, 40, 60);

            lblUser.Text = "Staff";
            lblUser.ForeColor = System.Drawing.Color.White;
            lblUser.Location = new System.Drawing.Point(20, 20);

            lblTime.ForeColor = System.Drawing.Color.White;
            lblTime.Location = new System.Drawing.Point(300, 20);

            header.Controls.Add(lblUser);
            header.Controls.Add(lblTime);

            // MAIN PANEL
            mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            // CARDS
            cardTotal.Size = new System.Drawing.Size(200, 100);
            cardTotal.Location = new System.Drawing.Point(50, 50);
            cardTotal.FillColor = System.Drawing.Color.MediumPurple;
            cardTotal.BorderRadius = 15;

            cardEmpty.Size = new System.Drawing.Size(200, 100);
            cardEmpty.Location = new System.Drawing.Point(300, 50);
            cardEmpty.FillColor = System.Drawing.Color.Green;
            cardEmpty.BorderRadius = 15;

            cardUsing.Size = new System.Drawing.Size(200, 100);
            cardUsing.Location = new System.Drawing.Point(550, 50);
            cardUsing.FillColor = System.Drawing.Color.Red;
            cardUsing.BorderRadius = 15;

            // LABELS
            lblTotal.Text = "Tổng bàn: 20";
            lblTotal.ForeColor = System.Drawing.Color.White;
            lblTotal.Location = new System.Drawing.Point(20, 40);

            lblEmpty.Text = "Trống: 10";
            lblEmpty.ForeColor = System.Drawing.Color.White;
            lblEmpty.Location = new System.Drawing.Point(20, 40);

            lblUsing.Text = "Đang dùng: 10";
            lblUsing.ForeColor = System.Drawing.Color.White;
            lblUsing.Location = new System.Drawing.Point(20, 40);

            cardTotal.Controls.Add(lblTotal);
            cardEmpty.Controls.Add(lblEmpty);
            cardUsing.Controls.Add(lblUsing);

            mainPanel.Controls.Add(cardTotal);
            mainPanel.Controls.Add(cardEmpty);
            mainPanel.Controls.Add(cardUsing);

            // TIMER
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            // ADD
            this.Controls.Add(mainPanel);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);
        }
    }
}
