using my_own_project.BLL;
using my_own_project.DAL.DTO;
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

namespace my_own_project.DesignForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                UserDTO user = UserBLL.Login(email, password);
                CurrentUser.Login(user);

                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                diaError.Show(ex.Message, "Login Failed");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerform = new RegisterForm();
            registerform.Show();
            this.Hide();
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
