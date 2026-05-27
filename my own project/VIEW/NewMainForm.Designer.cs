using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class NewMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Khai báo các Control UI
        private Guna2Panel pnlSidebar, pnlTopBar, pnlBody;
        private Guna2DragControl dragSidebar, dragTopBar;

        private Guna2Button btnPOS, btnHistory, btnProduct, btnDashboard,
            btnSettings, btnStaff, btnInventory, btnRecipe, btnPromotion, btnAccount;

        // Bảng màu
        private Color colorMainBG = Color.FromArgb(245, 246, 250);
        private Color colorPurple = Color.FromArgb(88, 28, 230);

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "NewMainForm";
        }

        #endregion

        // ─────────────────────────────────────────────────────────
        //  BUILD UI
        // ─────────────────────────────────────────────────────────
        private void BuildUI()
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
                Cursor = Cursors.Hand
            };
            btnClose.HoverState.FillColor = Color.Red;
            btnClose.HoverState.IconColor = Color.White;

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

            // Khởi tạo các nút
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

            // Gán sự kiện cho các nút gọi sang các hàm bên file .cs
            btnPOS.Click += BtnPOS_Click;
            btnHistory.Click += BtnHistory_Click;
            btnProduct.Click += BtnProduct_Click;
            btnDashboard.Click += BtnDashboard_Click;
            btnSettings.Click += BtnSettings_Click;
            btnStaff.Click += BtnStaff_Click;
            btnInventory.Click += BtnInventory_Click;
            btnRecipe.Click += BtnRecipe_Click;
            btnPromotion.Click += BtnPromotion_Click;
            btnAccount.Click += BtnAccount_Click;

            pnlSidebar.Controls.AddRange(new Control[]
            {
                btnPOS, btnHistory, btnProduct, btnDashboard,
                btnSettings, btnStaff, btnInventory, btnRecipe,
                btnPromotion, btnAccount
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
    }
}