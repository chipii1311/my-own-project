namespace my_own_project.VIEWSTAFF
{
    partial class StaffDashboard
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
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblSubtitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTime = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblUser = new Guna.UI2.WinForms.Guna2HtmlLabel();
            
            this.pnlStats = new Guna.UI2.WinForms.Guna2Panel();
            this.statTables = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.statEmpty = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.statOccupied = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.statReserved = new Guna.UI2.WinForms.Guna2HtmlLabel();
            
            this.pnlFilter = new Guna.UI2.WinForms.Guna2Panel();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.lblFilter = new Guna.UI2.WinForms.Guna2HtmlLabel();
            
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlTables = new System.Windows.Forms.FlowLayoutPanel();
            
            this.timerClock = new System.Windows.Forms.Timer(this.components);

            this.pnlHeader.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();

            // ============================================
            // HEADER PANEL
            // ============================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.pnlHeader.Controls.Add(this.lblUser);
            this.pnlHeader.Controls.Add(this.lblTime);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.pnlHeader.Size = new System.Drawing.Size(1200, 100);
            this.pnlHeader.TabIndex = 0;

            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🍽️ QUẢN LÝ BÀN";
            this.lblTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(255)))));
            this.lblSubtitle.Location = new System.Drawing.Point(30, 50);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(400, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Chọn bàn để gọi món";
            this.lblSubtitle.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblTime.AutoSize = false;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(900, 20);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(250, 25);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "⏰ 14:30:25 | 20/05/2025";
            this.lblTime.TextAlignment = System.Drawing.ContentAlignment.TopRight;

            this.lblUser.AutoSize = false;
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(900, 50);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(250, 25);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "👤 Nguyễn Văn An (Nhân viên)";
            this.lblUser.TextAlignment = System.Drawing.ContentAlignment.BottomRight;

            // ============================================
            // STATS PANEL
            // ============================================
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlStats.Controls.Add(this.statReserved);
            this.pnlStats.Controls.Add(this.statOccupied);
            this.pnlStats.Controls.Add(this.statEmpty);
            this.pnlStats.Controls.Add(this.statTables);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlStats.Location = new System.Drawing.Point(0, 100);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(30);
            this.pnlStats.Size = new System.Drawing.Size(1200, 90);
            this.pnlStats.TabIndex = 1;

            this.statTables.AutoSize = false;
            this.statTables.BackColor = System.Drawing.Color.White;
            this.statTables.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.statTables.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.statTables.Location = new System.Drawing.Point(30, 30);
            this.statTables.Name = "statTables";
            this.statTables.Padding = new System.Windows.Forms.Padding(20);
            this.statTables.Size = new System.Drawing.Size(220, 60);
            this.statTables.TabIndex = 0;
            this.statTables.Text = "🪑 Tổng số bàn\n20";
            this.statTables.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;

            this.statEmpty.AutoSize = false;
            this.statEmpty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(201)))));
            this.statEmpty.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.statEmpty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.statEmpty.Location = new System.Drawing.Point(280, 30);
            this.statEmpty.Name = "statEmpty";
            this.statEmpty.Padding = new System.Windows.Forms.Padding(20);
            this.statEmpty.Size = new System.Drawing.Size(220, 60);
            this.statEmpty.TabIndex = 1;
            this.statEmpty.Text = "● Trống\n12";
            this.statEmpty.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;

            this.statOccupied.AutoSize = false;
            this.statOccupied.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(205)))), ((int)(((byte)(210)))));
            this.statOccupied.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.statOccupied.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.statOccupied.Location = new System.Drawing.Point(530, 30);
            this.statOccupied.Name = "statOccupied";
            this.statOccupied.Padding = new System.Windows.Forms.Padding(20);
            this.statOccupied.Size = new System.Drawing.Size(220, 60);
            this.statOccupied.TabIndex = 2;
            this.statOccupied.Text = "● Đang sử dụng\n6";
            this.statOccupied.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;

            this.statReserved.AutoSize = false;
            this.statReserved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(224)))));
            this.statReserved.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.statReserved.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.statReserved.Location = new System.Drawing.Point(780, 30);
            this.statReserved.Name = "statReserved";
            this.statReserved.Padding = new System.Windows.Forms.Padding(20);
            this.statReserved.Size = new System.Drawing.Size(220, 60);
            this.statReserved.TabIndex = 3;
            this.statReserved.Text = "● Đã đặt trước\n2";
            this.statReserved.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;

            // ============================================
            // FILTER PANEL
            // ============================================
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlFilter.Controls.Add(this.btnRefresh);
            this.pnlFilter.Controls.Add(this.lblFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.pnlFilter.Location = new System.Drawing.Point(0, 190);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(30);
            this.pnlFilter.Size = new System.Drawing.Size(1200, 60);
            this.pnlFilter.TabIndex = 2;

            this.lblFilter.AutoSize = false;
            this.lblFilter.BackColor = System.Drawing.Color.Transparent;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblFilter.Location = new System.Drawing.Point(30, 20);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(300, 25);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "🏘️ KHU VỰC TRONG NHÀ";
            this.lblFilter.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

            this.btnRefresh.AutoRoundedCorners = true;
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BorderRadius = 18;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnRefresh.Location = new System.Drawing.Point(1050, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 40);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "🔄 Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // ============================================
            // CONTENT PANEL
            // ============================================
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.pnlTables);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 250);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(30);
            this.pnlContent.Size = new System.Drawing.Size(1200, 450);
            this.pnlContent.TabIndex = 3;

            this.pnlTables.AutoSize = true;
            this.pnlTables.BackColor = System.Drawing.Color.White;
            this.pnlTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTables.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlTables.Location = new System.Drawing.Point(30, 30);
            this.pnlTables.Name = "pnlTables";
            this.pnlTables.Size = new System.Drawing.Size(1140, 390);
            this.pnlTables.TabIndex = 0;
            this.pnlTables.WrapContents = true;

            // ============================================
            // TIMER
            // ============================================
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.TimerClock_Tick);

            // ============================================
            // STAFF DASHBOARD
            // ============================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlHeader);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "StaffDashboard";
            this.Text = "Staff Dashboard - Quản lý bàn";
            this.Load += new System.EventHandler(this.StaffDashboard_Load);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.pnlHeader.ResumeLayout(false);
            this.pnlStats.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubtitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTime;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblUser;
        private Guna.UI2.WinForms.Guna2Panel pnlStats;
        private Guna.UI2.WinForms.Guna2HtmlLabel statTables;
        private Guna.UI2.WinForms.Guna2HtmlLabel statEmpty;
        private Guna.UI2.WinForms.Guna2HtmlLabel statOccupied;
        private Guna.UI2.WinForms.Guna2HtmlLabel statReserved;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private Guna.UI2.WinForms.Guna2Panel pnlFilter;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFilter;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private System.Windows.Forms.FlowLayoutPanel pnlTables;
        private System.Windows.Forms.Timer timerClock;

       

       
    }
}