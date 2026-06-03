using my_own_project.DesignForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewMainForm : Form
    {
        private Form activeForm = null;

        public string UserRole { get; set; } = "Quản lý";
        public string LoggedInUserName { get; set; } = "";
        public int LoggedInUserID { get; set; } = 0;
        

        public NewMainForm()
        {
            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            this.Load += NewMainForm_Load;
            this.Resize += (s, e) => PositionBottomButtons();
        }

        // ─────────────────────────────────────────────────────────
        //  LOGIC & EVENTS
        // ─────────────────────────────────────────────────────────

        private void PositionBottomButtons()
        {
            if (btnAccount != null && pnlSidebar != null)
            {
                btnAccount.Top = pnlSidebar.Height - btnAccount.Height - 10;
                btnAccount.Left = 20;
            }
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

        // ── XỬ LÝ SỰ KIỆN CLICK MENU ──────────────────────────────

        public void BtnPOS_Click(object sender, EventArgs e) => OpenChildForm(new POSForm(this.LoggedInUserID, this.LoggedInUserName));
        public void BtnHistory_Click(object sender, EventArgs e) => OpenChildForm(new HistoryForm());
        public void BtnProduct_Click(object sender, EventArgs e) => OpenChildForm(new ProductForm(this.UserRole));
        public void BtnDashboard_Click(object sender, EventArgs e) => OpenChildForm(new NewDashboardForm());
        public void BtnSettings_Click(object sender, EventArgs e) => OpenChildForm(new SettingForm());
        public void BtnStaff_Click(object sender, EventArgs e) => OpenChildForm(new StaffForm());
        public void BtnInventory_Click(object sender, EventArgs e) => OpenChildForm(new InventoryForm());
        public void BtnRecipe_Click(object sender, EventArgs e) => OpenChildForm(new RecipeManagementForm());
        public void BtnPromotion_Click(object sender, EventArgs e) => OpenChildForm(new NewPromotionForm());

        public void BtnAccount_Click(object sender, EventArgs e)
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