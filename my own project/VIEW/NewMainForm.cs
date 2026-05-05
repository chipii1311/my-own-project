using Guna.UI2.WinForms;
using my_own_project.DesignForms; // Hoặc thư mục chứa các form của bạn
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewMainForm : Form
    {
        private Guna2Panel pnlSidebar;
        private Guna2Panel pnlMainContent;
        private Guna2Panel pnlBody;
        private Guna2DragControl dragControl;
        private Guna2ShadowForm shadowForm;

        // Bảng màu từ thiết kế mới
        private Color colorMainBG = Color.FromArgb(245, 246, 250);
        private Color colorPurple = Color.FromArgb(88, 28, 230);

        public NewMainForm()
        {
            InitializeModernUI();
        }

        private void InitializeModernUI()
        {
            this.Size = new Size(1366, 768);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = colorMainBG;

            shadowForm = new Guna2ShadowForm(this);

            // ==========================================
            // VÙNG NỘI DUNG CHÍNH (Bao gồm TopBar và Body)
            // ==========================================
            pnlMainContent = new Guna2Panel();
            pnlMainContent.Dock = DockStyle.Fill;
            pnlMainContent.BackColor = Color.Transparent;

            // --- THANH TIÊU ĐỀ (TOPBAR) KÉO THẢ ---
            Guna2Panel pnlTopBar = new Guna2Panel();
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 40;
            pnlTopBar.Width = this.Width;
            pnlTopBar.BackColor = colorMainBG;

            // === CÁCH TRỊ BỆNH LỘN NÚT: TỌA ĐỘ VẬT LÝ TUYỆT ĐỐI ===

            // 1. NÚT TẮT (Đóng đinh sát mép phải: Width - 50)
            Guna2ControlBox btnClose = new Guna2ControlBox();
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Size = new Size(50, 40);
            btnClose.Location = new Point(pnlTopBar.Width - 50, 0);
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = Color.Gray;
            btnClose.HoverState.FillColor = Color.FromArgb(255, 71, 87);
            btnClose.HoverState.IconColor = Color.White;
            btnClose.CustomClick = true;
            btnClose.Click += (s, e) => { Application.Exit(); };

            // 2. NÚT PHÓNG TO (Đóng đinh cách mép phải 100px)
            Guna2ControlBox btnMax = new Guna2ControlBox();
            btnMax.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            btnMax.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMax.Size = new Size(50, 40);
            btnMax.Location = new Point(pnlTopBar.Width - 100, 0);
            btnMax.FillColor = Color.Transparent;
            btnMax.IconColor = Color.Gray;

            // 3. NÚT THU NHỎ (Đóng đinh cách mép phải 150px)
            Guna2ControlBox btnMin = new Guna2ControlBox();
            btnMin.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            btnMin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMin.Size = new Size(50, 40);
            btnMin.Location = new Point(pnlTopBar.Width - 150, 0);
            btnMin.FillColor = Color.Transparent;
            btnMin.IconColor = Color.Gray;

            pnlTopBar.Controls.Add(btnMin);
            pnlTopBar.Controls.Add(btnMax);
            pnlTopBar.Controls.Add(btnClose);

            pnlMainContent.Controls.Add(pnlTopBar);

            dragControl = new Guna2DragControl();
            dragControl.TargetControl = pnlTopBar;

            // --- VÙNG CHỨA CÁC FORM CON ---
            pnlBody = new Guna2Panel();
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.BackColor = Color.Transparent;
            pnlMainContent.Controls.Add(pnlBody);

            pnlTopBar.BringToFront();

            // ==========================================
            // SIDEBAR SIÊU MỎNG (MINI SIDEBAR)
            // ==========================================
            pnlSidebar = new Guna2Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 90;
            pnlSidebar.FillColor = Color.White;
            pnlSidebar.CustomBorderThickness = new Padding(0, 0, 1, 0);
            pnlSidebar.CustomBorderColor = Color.FromArgb(235, 235, 235);

            // Logo
            Label lblLogo = new Label();
            lblLogo.Text = "🍩";
            lblLogo.Font = new Font("Segoe UI", 24F);
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(25, 20);
            pnlSidebar.Controls.Add(lblLogo);

            // CÁC NÚT MENU (Khoảng cách tăng thêm 70px cho mỗi nút)
            Guna2Button btnPOS = CreateIconButton("🛒", 120);
            btnPOS.Checked = true;
            btnPOS.Click += (s, e) => { OpenChildForm(new POSForm()); }; // Tùy thuộc vào namespace của POSForm của bạn

            Guna2Button btnHistory = CreateIconButton("🧾", 190);
            btnHistory.Click += (s, e) => { OpenChildForm(new HistoryForm()); };

            Guna2Button btnProduct = CreateIconButton("🍔", 260);
            btnProduct.Click += (s, e) => { OpenChildForm(new ProductForm()); };

            // === ĐÂY LÀ NÚT DASHBOARD MỚI ĐƯỢC THÊM VÀO ===
            Guna2Button btnDashboard = CreateIconButton("📊", 330);
            btnDashboard.Click += (s, e) => { OpenChildForm(new NewDashboardForm()); }; // Gọi form NewDashboardForm ra

            // Dời nút Cài đặt xuống 1 nấc (từ 330 -> 400)
            Guna2Button btnSettings = CreateIconButton("⚙️", 400);
            btnSettings.Click += (s, e) => { OpenChildForm(new SettingForm()); };

            // Nút Tắt app
            Guna2Button btnExit = CreateIconButton("🛑", this.Height - 80);
            btnExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExit.Location = new Point(20, this.Height - 80);
            btnExit.Click += (s, e) => { Application.Exit(); };

            pnlSidebar.Controls.Add(btnPOS);
            pnlSidebar.Controls.Add(btnHistory);
            pnlSidebar.Controls.Add(btnProduct);
            pnlSidebar.Controls.Add(btnDashboard); // Ném nút Dashboard vào Sidebar
            pnlSidebar.Controls.Add(btnSettings);
            pnlSidebar.Controls.Add(btnExit);

            Guna2DragControl dragSidebar = new Guna2DragControl();
            dragSidebar.TargetControl = pnlSidebar;

            this.Controls.Add(pnlMainContent);
            this.Controls.Add(pnlSidebar);

            // TỰ ĐỘNG MỞ POSFORM VÀO PNLBODY KHI KHỞI ĐỘNG
            OpenChildForm(new POSForm());
        }

        private Guna2Button CreateIconButton(string iconText, int yPos)
        {
            Guna2Button btn = new Guna2Button();
            btn.Size = new Size(50, 50);
            btn.Location = new Point(20, yPos);
            btn.BorderRadius = 15;
            btn.Text = iconText;
            btn.Font = new Font("Segoe UI", 16F);
            btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btn.Cursor = Cursors.Hand;
            btn.Animated = true;
            btn.FillColor = Color.Transparent;
            btn.ForeColor = Color.Gray;
            btn.CheckedState.FillColor = Color.FromArgb(240, 235, 255);
            btn.CheckedState.ForeColor = colorPurple;
            return btn;
        }

        private Form activeForm = null;
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null) activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlBody.Controls.Add(childForm);
            childForm.Show();
        }
    }
}