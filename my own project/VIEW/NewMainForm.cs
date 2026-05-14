using Guna.UI2.WinForms;
using my_own_project.DesignForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewMainForm : Form
    {
        private Guna2Panel pnlSidebar, pnlTopBar, pnlBody;
        private Guna2DragControl dragSidebar, dragTopBar;
        private Form activeForm = null;

        private Color colorMainBG = Color.FromArgb(245, 246, 250);
        private Color colorPurple = Color.FromArgb(88, 28, 230);

        public string UserRole { get; set; } = "Quản lý";
        public string LoggedInUserName { get; set; } = "";
        public int LoggedInUserID { get; set; } = 0;

        private Guna2Button btnPOS, btnHistory, btnProduct, btnDashboard,
            btnSettings, btnStaff, btnInventory, btnRecipe, btnPromotion, btnAccount, btnExit;

        public NewMainForm()
        {
            InitializeModernUI();

            this.Load += NewMainForm_Load;
            this.Resize += (s, e) => PositionBottomButtons();
        }

        private void InitializeModernUI()
        {
            this.Size = new Size(1366, 768);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = colorMainBG;

            dragSidebar = new Guna2DragControl();
            dragTopBar = new Guna2DragControl();

            pnlSidebar = new Guna2Panel
            {
                Dock = DockStyle.Left,
                Width = 90,
                FillColor = Color.White,
                CustomBorderThickness = new Padding(0, 0, 1, 0),
                CustomBorderColor = Color.FromArgb(235, 235, 235)
            };

            dragSidebar.TargetControl = pnlSidebar;

            pnlSidebar.Controls.Add(new Label
            {
                Text = "🍩",
                Font = new Font("Segoe UI", 24F),
                AutoSize = true,
                Location = new Point(25, 20)
            });

            pnlTopBar = new Guna2Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                FillColor = Color.White,
                CustomBorderThickness = new Padding(0, 0, 0, 1),
                CustomBorderColor = Color.FromArgb(235, 235, 235)
            };

            dragTopBar.TargetControl = pnlTopBar;

            Guna2ControlBox btnClose = new Guna2ControlBox
            {
                Dock = DockStyle.Right,
                Width = 55,
                FillColor = Color.Transparent,
                IconColor = Color.Gray,
                HoverState = { FillColor = Color.Red, IconColor = Color.White },
                Cursor = Cursors.Hand
            };

            Guna2ControlBox btnMax = new Guna2ControlBox
            {
                Dock = DockStyle.Right,
                ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox,
                Width = 55,
                FillColor = Color.Transparent,
                IconColor = Color.Gray,
                Cursor = Cursors.Hand
            };

            Guna2ControlBox btnMin = new Guna2ControlBox
            {
                Dock = DockStyle.Right,
                ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox,
                Width = 55,
                FillColor = Color.Transparent,
                IconColor = Color.Gray,
                Cursor = Cursors.Hand
            };

            pnlTopBar.Controls.AddRange(new Control[] { btnMin, btnMax, btnClose });

            pnlBody = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(pnlSidebar);

            pnlSidebar.SendToBack();
            pnlBody.BringToFront();

            btnPOS = AddSidebarButton("🛒", 120);
            btnHistory = AddSidebarButton("🧾", 190);
            btnProduct = AddSidebarButton("🍔", 260);
            btnDashboard = AddSidebarButton("📊", 330);
            btnSettings = AddSidebarButton("⚙️", 400);
            btnStaff = AddSidebarButton("👥", 470);
            btnInventory = AddSidebarButton("📦", 535);
            btnRecipe = AddSidebarButton("🧪", 595);
            btnPromotion = AddSidebarButton("🎁", 655);
            btnAccount = AddSidebarButton("👤", 0);
            btnExit = AddSidebarButton("🛑", 0);

            btnPOS.Click += (s, e) => OpenChildForm(new POSForm(this.LoggedInUserID, this.LoggedInUserName));
            btnHistory.Click += (s, e) => OpenChildForm(new HistoryForm());
            btnProduct.Click += (s, e) => OpenChildForm(new ProductForm(this.UserRole));
            btnDashboard.Click += (s, e) => OpenChildForm(new NewDashboardForm());
            btnSettings.Click += (s, e) => OpenChildForm(new SettingForm());
            btnStaff.Click += (s, e) => OpenChildForm(new StaffForm());
            btnInventory.Click += (s, e) => OpenChildForm(new InventoryForm());
            btnRecipe.Click += (s, e) => OpenChildForm(new RecipeManagementForm());
            btnPromotion.Click += (s, e) => OpenChildForm(new NewPromotionForm());
            btnAccount.Click += BtnAccount_Click;
            btnExit.Click += (s, e) => Application.Exit();

            pnlSidebar.Controls.AddRange(new Control[]
            {
                btnPOS,
                btnHistory,
                btnProduct,
                btnDashboard,
                btnSettings,
                btnStaff,
                btnInventory,
                btnRecipe,
                btnPromotion,
                btnAccount,
                btnExit
            });
        }

        private Guna2Button AddSidebarButton(string icon, int y)
        {
            return new Guna2Button
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
                CheckedState =
                {
                    FillColor = Color.FromArgb(240, 235, 255),
                    ForeColor = colorPurple
                }
            };
        }

        private void PositionBottomButtons()
        {
            btnExit.Top = pnlSidebar.Height - btnExit.Height - 10;
            btnExit.Left = 20;

            btnAccount.Top = btnExit.Top - btnAccount.Height - 6;
            btnAccount.Left = 20;
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlBody.Controls.Clear();
            pnlBody.Controls.Add(childForm);

            childForm.Show();
        }

        private void NewMainForm_Load(object sender, EventArgs e)
        {
            if (UserRole == "Nhân viên")
            {
                btnHistory.Visible =
                btnDashboard.Visible =
                btnSettings.Visible =
                btnStaff.Visible =
                btnInventory.Visible =
                btnRecipe.Visible =
                btnPromotion.Visible = false;

                btnProduct.Location = new Point(20, 190);
            }

            PositionBottomButtons();

            btnPOS.Checked = true;
            OpenChildForm(new POSForm(this.LoggedInUserID, this.LoggedInUserName));
        }

        private void BtnAccount_Click(object sender, EventArgs e)
        {
            btnPOS.Checked =
            btnHistory.Checked =
            btnProduct.Checked =
            btnDashboard.Checked =
            btnSettings.Checked =
            btnStaff.Checked =
            btnInventory.Checked =
            btnRecipe.Checked =
            btnPromotion.Checked = false;

            OpenChildForm(new AccountForm(this.LoggedInUserID));
        }
    }
}