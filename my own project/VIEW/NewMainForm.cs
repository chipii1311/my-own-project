using Guna.UI2.WinForms;
using my_own_project.DesignForms; // Hoặc thư mục chứa các form của bạn
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewMainForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private Guna2Panel pnlSidebar;
        private Guna2Panel pnlMainContent;
        private Guna2Panel pnlBody;
        private Guna2DragControl dragControl;
        private Guna2ShadowForm shadowForm;
        private Form activeForm = null;

        // Bảng màu từ thiết kế mới
        private Color colorMainBG = Color.FromArgb(245, 246, 250);
        private Color colorPurple = Color.FromArgb(88, 28, 230);

        // ========================================================
        // BIẾN PHÂN QUYỀN VÀ THÔNG TIN ĐĂNG NHẬP (Lấy từ Login)
        // ========================================================
        public string UserRole { get; set; } = "Quản lý";
        public string LoggedInUserName { get; set; } = ""; // ĐÃ THÊM: Để hiện tên lên Avatar
        public int LoggedInUserID { get; set; } = 0;       // ĐÃ THÊM: Để truyền cho AccountForm

        // Các nút Menu cần khai báo toàn cục để dễ ẩn/hiện khi phân quyền
        private Guna2Button btnPOS, btnHistory, btnProduct, btnDashboard, btnSettings, btnStaff, btnExit;

        // --- NÚT TÀI KHOẢN (AVATAR) TRÊN TOPBAR ---
        private Guna2Button btnAccount;

        public NewMainForm()
        {
            InitializeModernUI();

            // Gắn sự kiện Load để phân quyền lúc vừa mở form lên
            this.Load += NewMainForm_Load;
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void InitializeModernUI()
        {
            this.Size = new Size(1366, 768);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = colorMainBG;

            shadowForm = new Guna2ShadowForm(this);

            // --- VÙNG NỘI DUNG CHÍNH ---
            pnlMainContent = new Guna2Panel();
            pnlMainContent.Dock = DockStyle.Fill;
            pnlMainContent.BackColor = Color.Transparent;

            // --- THANH TIÊU ĐỀ (TOPBAR) ---
            Guna2Panel pnlTopBar = new Guna2Panel();
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 40;
            pnlTopBar.Width = this.Width;
            pnlTopBar.BackColor = colorMainBG;

            // 1. NÚT TẮT
            Guna2ControlBox btnClose = new Guna2ControlBox();
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Size = new Size(50, 40);
            btnClose.Location = new Point(pnlTopBar.Width - 50, 0);
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = Color.Gray;
            btnClose.HoverState.FillColor = Color.FromArgb(255, 71, 87);
            btnClose.HoverState.IconColor = Color.White;
            btnClose.CustomClick = true;
            btnClose.Click += BtnClose_Click;

            // 2. NÚT PHÓNG TO
            Guna2ControlBox btnMax = new Guna2ControlBox();
            btnMax.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            btnMax.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMax.Size = new Size(50, 40);
            btnMax.Location = new Point(pnlTopBar.Width - 100, 0);
            btnMax.FillColor = Color.Transparent;
            btnMax.IconColor = Color.Gray;

            // 3. NÚT THU NHỎ
            Guna2ControlBox btnMin = new Guna2ControlBox();
            btnMin.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            btnMin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMin.Size = new Size(50, 40);
            btnMin.Location = new Point(pnlTopBar.Width - 150, 0);
            btnMin.FillColor = Color.Transparent;
            btnMin.IconColor = Color.Gray;

            // 4. NÚT TÀI KHOẢN (AVATAR) TRÊN TOPBAR - BẢN SANG CHẢNH
            btnAccount = new Guna2Button();
            btnAccount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAccount.Size = new Size(150, 32);
            btnAccount.Location = new Point(pnlTopBar.Width - 320, 4);

            btnAccount.FillColor = Color.FromArgb(240, 235, 255);
            btnAccount.ForeColor = Color.FromArgb(88, 28, 230);
            btnAccount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAccount.Text = "🙂 Xin chào";
            btnAccount.BorderRadius = 16;
            btnAccount.Cursor = Cursors.Hand;

            btnAccount.HoverState.FillColor = Color.FromArgb(88, 28, 230);
            btnAccount.HoverState.ForeColor = Color.White;

            btnAccount.Click += BtnAccount_Click;

            pnlTopBar.Controls.Add(btnAccount);
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

            // --- SIDEBAR SIÊU MỎNG ---
            pnlSidebar = new Guna2Panel();
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 90;
            pnlSidebar.FillColor = Color.White;
            pnlSidebar.CustomBorderThickness = new Padding(0, 0, 1, 0);
            pnlSidebar.CustomBorderColor = Color.FromArgb(235, 235, 235);

            Label lblLogo = new Label();
            lblLogo.Text = "🍩";
            lblLogo.Font = new Font("Segoe UI", 24F);
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(25, 20);
            pnlSidebar.Controls.Add(lblLogo);

            // --- CÁC NÚT MENU (Khoảng cách đều đặn 70px) ---
            btnPOS = CreateIconButton("🛒", 120);
            btnPOS.Checked = true;
            btnPOS.Click += BtnPOS_Click;

            btnHistory = CreateIconButton("🧾", 190);
            btnHistory.Click += BtnHistory_Click;

            btnProduct = CreateIconButton("🍔", 260);
            btnProduct.Click += BtnProduct_Click;

            btnDashboard = CreateIconButton("📊", 330);
            btnDashboard.Click += BtnDashboard_Click;

            btnSettings = CreateIconButton("⚙️", 400);
            btnSettings.Click += BtnSettings_Click;

            btnStaff = CreateIconButton("👥", 470);
            btnStaff.Click += BtnStaff_Click;

            btnExit = CreateIconButton("🛑", this.Height - 80);
            btnExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExit.Location = new Point(20, this.Height - 80);
            btnExit.Click += BtnClose_Click;

            pnlSidebar.Controls.Add(btnPOS);
            pnlSidebar.Controls.Add(btnHistory);
            pnlSidebar.Controls.Add(btnProduct);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(btnSettings);
            pnlSidebar.Controls.Add(btnStaff);
            pnlSidebar.Controls.Add(btnExit);

            Guna2DragControl dragSidebar = new Guna2DragControl();
            dragSidebar.TargetControl = pnlSidebar;

            this.Controls.Add(pnlMainContent);
            this.Controls.Add(pnlSidebar);

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

        #endregion

        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC
        // ========================================================

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

        #endregion

        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN (EVENTS) & PHÂN QUYỀN
        // ========================================================

        private void NewMainForm_Load(object sender, EventArgs e)
        {
            // 1. ĐỔI TÊN NÚT AVATAR
            if (!string.IsNullOrEmpty(LoggedInUserName))
            {
                // Cắt lấy tên ngắn nhất (VD: Nguyễn Văn A -> A)
                string[] nameParts = LoggedInUserName.Trim().Split(' ');
                string shortName = nameParts[nameParts.Length - 1];
                btnAccount.Text = "🙂 Hi, " + shortName;
            }

            // 2. TÍNH NĂNG PHÂN QUYỀN (ROLE-BASED ACCESS)
            if (UserRole == "Nhân viên")
            {
                // Giấu các tính năng nhạy cảm không cho nhân viên thấy
                btnProduct.Visible = false;
                btnDashboard.Visible = false;
                btnSettings.Visible = false;
                btnStaff.Visible = false;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnPOS_Click(object sender, EventArgs e)
        {
            OpenChildForm(new POSForm());
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new HistoryForm());
        }

        private void BtnProduct_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ProductForm());
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            OpenChildForm(new NewDashboardForm());
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            OpenChildForm(new SettingForm());
        }

        private void BtnStaff_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StaffForm());
        }

        // --- SỰ KIỆN GỌI FORM TÀI KHOẢN CÁ NHÂN ---
        private void BtnAccount_Click(object sender, EventArgs e)
        {
            btnPOS.Checked = false;
            btnHistory.Checked = false;
            btnProduct.Checked = false;
            btnDashboard.Checked = false;
            btnSettings.Checked = false;
            btnStaff.Checked = false;

            // TRUYỀN ID CỦA NGƯỜI DÙNG VÀO TRONG DẤU NGOẶC LUÔN!
            AccountForm accForm = new AccountForm(this.LoggedInUserID);

            OpenChildForm(accForm);
        }

        #endregion
    }
}