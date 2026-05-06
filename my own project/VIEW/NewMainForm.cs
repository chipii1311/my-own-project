using Guna.UI2.WinForms;
using my_own_project.DesignForms; // chứa ProductForm, AccountForm, StaffForm,... (nếu cần)
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewMainForm : Form
    {
        // ========================================================
        // BIẾN TOÀN CỤC
        // ========================================================
        private Guna2Panel pnlSidebar;
        private Guna2Panel pnlBody;
        private Guna2DragControl dragSidebar;
        private Form activeForm = null;

        // Màu sắc
        private Color colorMainBG = Color.FromArgb(245, 246, 250);
        private Color colorPurple = Color.FromArgb(88, 28, 230);

        // Thông tin người dùng (truyền từ Login)
        public string UserRole { get; set; } = "Quản lý";
        public string LoggedInUserName { get; set; } = "";
        public int LoggedInUserID { get; set; } = 0;

        // Các nút menu
        private Guna2Button btnPOS, btnHistory, btnProduct, btnDashboard,
                            btnSettings, btnStaff, btnInventory, btnAccount, btnExit;

        public NewMainForm()
        {
            InitializeModernUI();
            this.Load += NewMainForm_Load;
            this.Resize += (s, e) => PositionBottomButtons();
        }

        // ========================================================
        #region 1. VẼ GIAO DIỆN
        // ========================================================
        private void InitializeModernUI()
        {
            this.Size = new Size(1366, 768);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = colorMainBG;

            // Drag control gán vào sidebar
            dragSidebar = new Guna2DragControl();

            // ── Vùng chứa form con (bên phải) ──
            pnlBody = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlBody);

            // ── SIDEBAR ──
            pnlSidebar = new Guna2Panel
            {
                Dock = DockStyle.Left,
                Width = 90,
                FillColor = Color.White,
                CustomBorderThickness = new Padding(0, 0, 1, 0),
                CustomBorderColor = Color.FromArgb(235, 235, 235)
            };
            dragSidebar.TargetControl = pnlSidebar;

            // Logo
            Label lblLogo = new Label
            {
                Text = "🍩",
                Font = new Font("Segoe UI", 24F),
                AutoSize = true,
                Location = new Point(25, 20)
            };
            pnlSidebar.Controls.Add(lblLogo);

            // ── CÁC NÚT CHỨC NĂNG ──
            btnPOS = AddSidebarButton("🛒", 120);
            btnHistory = AddSidebarButton("🧾", 190);
            btnProduct = AddSidebarButton("🍔", 260);
            btnDashboard = AddSidebarButton("📊", 330);
            btnSettings = AddSidebarButton("⚙️", 400);
            btnStaff = AddSidebarButton("👥", 470);
            btnInventory = AddSidebarButton("📦", 540);

            // Gắn sự kiện
            btnPOS.Click += (s, e) => OpenChildForm(new POSForm());
            btnHistory.Click += (s, e) => OpenChildForm(new HistoryForm());
            btnProduct.Click += (s, e) => OpenChildForm(new ProductForm());
            btnDashboard.Click += (s, e) => OpenChildForm(new NewDashboardForm());
            btnSettings.Click += (s, e) => OpenChildForm(new SettingForm());
            btnStaff.Click += (s, e) => OpenChildForm(new StaffForm());
            btnInventory.Click += (s, e) => OpenChildForm(new InventoryForm());

            // Nút Tài khoản và Thoát (tạm vị trí 0, sẽ chỉnh lại sau)
            btnAccount = AddSidebarButton("👤", 0);
            btnExit = AddSidebarButton("🛑", 0);

            btnAccount.Click += BtnAccount_Click;
            btnExit.Click += (s, e) => Application.Exit();

            // Thêm tất cả vào sidebar
            pnlSidebar.Controls.Add(btnPOS);
            pnlSidebar.Controls.Add(btnHistory);
            pnlSidebar.Controls.Add(btnProduct);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(btnSettings);
            pnlSidebar.Controls.Add(btnStaff);
            pnlSidebar.Controls.Add(btnInventory);
            pnlSidebar.Controls.Add(btnAccount);
            pnlSidebar.Controls.Add(btnExit);

            this.Controls.Add(pnlSidebar);

            // Mở form POS mặc định
            btnPOS.Checked = true;
            OpenChildForm(new POSForm());
        }

        // Tạo nút icon trong sidebar
        private Guna2Button AddSidebarButton(string icon, int y)
        {
            var btn = new Guna2Button
            {
                Size = new Size(50, 50),
                Location = new Point(20, y),
                BorderRadius = 15,
                Text = icon,
                Font = new Font("Segoe UI", 16F),
                ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                Cursor = Cursors.Hand,
                Animated = true,
                FillColor = Color.Transparent,
                ForeColor = Color.Gray,
                CheckedState = { FillColor = Color.FromArgb(240, 235, 255), ForeColor = colorPurple }
            };
            return btn;
        }

        // Đặt vị trí nút Account và Exit xuống đáy sidebar
        private void PositionBottomButtons()
        {
            int sidebarH = pnlSidebar.Height;
            int spacing = 6;
            btnExit.Top = sidebarH - btnExit.Height - 10;
            btnExit.Left = 20;
            btnAccount.Top = btnExit.Top - btnAccount.Height - spacing;
            btnAccount.Left = 20;
        }
        #endregion

        // ========================================================
        #region 2. MỞ FORM CON
        // ========================================================
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlBody.Controls.Add(childForm);
            childForm.Show();
        }
        #endregion

        // ========================================================
        #region 3. SỰ KIỆN & PHÂN QUYỀN
        // ========================================================
        private void NewMainForm_Load(object sender, EventArgs e)
        {
            // Tên hiển thị trên nút Account (có thể để tooltip nếu thích)
            if (!string.IsNullOrEmpty(LoggedInUserName))
            {
                // Gợi ý: có thể hiển thị tên viết tắt cạnh icon, nhưng giữ nguyên icon đơn giản
                string[] parts = LoggedInUserName.Trim().Split(' ');
                string shortName = parts[parts.Length - 1];
                btnAccount.Text = "👤"; // hoặc "👤 " + shortName nếu muốn
            }

            // Phân quyền
            if (UserRole == "Nhân viên")
            {
                btnHistory.Visible = false;
                btnDashboard.Visible = false;
                btnSettings.Visible = false;
                btnStaff.Visible = false;
                btnInventory.Visible = false;

                // Đôn nút Product lên sát POS (y=190)
                btnProduct.Location = new Point(20, 190);
            }
            else // Quản lý
            {
                btnHistory.Visible = true;
                btnDashboard.Visible = true;
                btnSettings.Visible = true;
                btnStaff.Visible = true;
                btnInventory.Visible = true;
            }

            // Căn chỉnh nút Account và Exit
            PositionBottomButtons();
        }

        private void BtnAccount_Click(object sender, EventArgs e)
        {
            // Bỏ trạng thái checked của các nút khác
            btnPOS.Checked = btnHistory.Checked = btnProduct.Checked =
            btnDashboard.Checked = btnSettings.Checked = btnStaff.Checked = false;

            OpenChildForm(new AccountForm(this.LoggedInUserID));
        }
        #endregion
    }
}