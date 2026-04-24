using my_own_project.DesignForms;
using my_own_project.Helpers;
using my_own_project.VIEW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private Button currentButton;


        public void AddControls(Form f)
        {
            ControlsPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            ControlsPanel.Controls.Add(f);
            f.Show();

        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            AddControls(new CategoryForm());

        }
        private void btnTable_Click(object sender, EventArgs e)
        {
            AddControls(new TableForm());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            AddControls(new ProductForm());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            AddControls(new POSForm());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (CurrentUser.IsLoggedIn)
            {
                lblUserName.Text = "Welcome, " + CurrentUser.FullName;
            }
            else
            {
                lblUserName.Text = "Welcome, Guest";
            }
            AddControls(new DashboardForm());
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (logoutDialog.Show() == DialogResult.Yes)
            {
                this.Hide();
                LoginForm login = new LoginForm();
                login.Show();
            }

        }

        private void btnSettings_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            MenuForm menu = new MenuForm();
            menu.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AddControls(new DashboardForm());
        }

        private void btnCategor_Click(object sender, EventArgs e)
        {
            AddControls(new POSForm());
        }

        private void btnPromotion_Click(object sender, EventArgs e)
        {
            AddControls(new PromotionForm());
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            AddControls(new HistoryForm());
        }

        private void btnCategory_Click_1(object sender, EventArgs e)
        {
            AddControls(new CategoryForm());
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            AddControls(new frmTable());
        }
    }
}
