using my_own_project.BLL;
using my_own_project.DAL;
using my_own_project.DAL.DTO;
using my_own_project.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW_Việt
{
    public partial class frmLogin : Form
    {
        private bool isRegisterMode = false;

        public frmLogin()
        {
            InitializeComponent();
            StyleForm();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Load saved email nếu có
            if (Properties.Settings.Default.RememberEmail)
            {
                txtEmail.Text = Properties.Settings.Default.SavedEmail;
                chkRemember.Checked = true;
            }

            txtEmail.Focus();

            // Test connection
            if (!DataHelper.TestConnection())
            {
                MessageBox.Show("❌ Không thể kết nối đến Database!\n\nVui lòng kiểm tra cấu hình App.config",
                    "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void StyleForm()
        {
            this.Text = "Hệ Thống Quản Lý Nhà Hàng";
            this.Size = new System.Drawing.Size(600, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.White;

            // Thiết lập gradient cho panel chính
            pnlMain.FillColor = System.Drawing.Color.FromArgb(52, 152, 219); // Blue
            pnlMain.FillColor2 = System.Drawing.Color.FromArgb(41, 128, 185);
            pnlMain.Dock = DockStyle.Fill;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                // Regex pattern cho email hợp lệ
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, pattern))
                    return false;

                // Kiểm tra thêm bằng MailAddress
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate input
        /// </summary>
        private bool ValidateInput(out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                errorMessage = "Vui lòng nhập email!";
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorMessage = "Vui lòng nhập mật khẩu!";
                return false;
            }

            return true;
        }
        private bool ValidateRegisterInput(out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrEmpty(txtRegFullName.Text.Trim()))
            {
                errorMessage = "Vui lòng nhập tên đầy đủ!";
                return false;
            }

            if (string.IsNullOrEmpty(txtRegEmail.Text.Trim()))
            {
                errorMessage = "Vui lòng nhập email!";
                return false;
            }

            // Validate email format
            if (!IsValidEmail(txtRegEmail.Text.Trim()))
            {
                errorMessage = "Email không hợp lệ! (Ví dụ: user@example.com)";
                return false;
            }

            if (string.IsNullOrEmpty(txtRegPhone.Text.Trim()))
            {
                errorMessage = "Vui lòng nhập số điện thoại!";
                return false;
            }

            if (string.IsNullOrEmpty(txtRegPassword.Text) || txtRegPassword.Text.Length < 6)
            {
                errorMessage = "Mật khẩu phải >= 6 ký tự!";
                return false;
            }

            if (txtRegPassword.Text != txtRegConfirmPassword.Text)
            {
                errorMessage = "Xác nhận mật khẩu không khớp!";
                return false;
            }

            return true;
        }



        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(message, "❌ Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            prbLoading.Visible = false;
            btnLogin.Enabled = true;
        }

        /// <summary>
        /// Hiển thị thông báo thành công
        /// </summary>
        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "✅ Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (!ValidateInput(out string errorMsg))
                {
                    ShowError(errorMsg);
                    return;
                }

                btnLogin.Enabled = false;
                prbLoading.Visible = true;
                Application.DoEvents();

                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                // Gọi BLL để login
                UserDTO user = UserBLL.Login(email, password);

                if (user != null)
                {
                    // Lưu thông tin user
                    CurrentUser.Login(user);

                    // Lưu email nếu chọn "Ghi nhớ tôi"
                    if (chkRemember.Checked)
                    {
                        Properties.Settings.Default.RememberEmail = true;
                        Properties.Settings.Default.SavedEmail = email;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.RememberEmail = false;
                        Properties.Settings.Default.Save();
                    }

                    ShowSuccess($"✅ Chào mừng {user.FullName}!");

                    // Mở form chính
                    frmMain mainForm = new frmMain();
                    this.Hide();
                    mainForm.Show();
                    mainForm.FormClosed += (s, args) => this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowError($"❌ Lỗi: {ex.Message}");
            }
            finally
            {
                btnLogin.Enabled = true;
                prbLoading.Visible = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.Handled = true;
            }
        }

        private void lblToggleRegister_Click(object sender, EventArgs e)
        {
            pnlLoginBox.Visible = false;
            pnlRegisterBox.Visible = true;
            isRegisterMode = true;

            // Clear form đăng ký
            ClearRegisterForm();
        }

        private void lblBackToLogin_Click(object sender, EventArgs e)
        {
            pnlRegisterBox.Visible = false;
            pnlLoginBox.Visible = true;
            isRegisterMode = false;

            // Clear form login
            ClearLoginForm();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateRegisterInput(out string errorMsg))
                {
                    MessageBox.Show(errorMsg, "❌ Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnRegister.Enabled = false;
                Application.DoEvents();

                // Tạo UserDTO mới
                UserDTO newUser = new UserDTO
                {
                    FullName = txtRegFullName.Text.Trim(),
                    Email = txtRegEmail.Text.Trim(),
                    Phone = txtRegPhone.Text.Trim(),
                    PasswordHash = txtRegPassword.Text,
                    Role = "Staff" // Default role cho user mới
                };

                // Gọi BLL để thêm user
                int userID = UserBLL.AddUser(newUser);

                MessageBox.Show($"✅ Đăng ký thành công!\n\nID tài khoản: {userID}\n\nVui lòng đăng nhập với email: {newUser.Email}",
                    "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Quay lại form login
                lblBackToLogin_Click(null, null);

                // Fill email vào form login
                txtEmail.Text = newUser.Email;
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }
        private void ClearLoginForm()
        {
            txtEmail.Text = "";
            txtPassword.Text = "";
            chkRemember.Checked = false;
            txtEmail.Focus();
        }

        private void ClearRegisterForm()
        {
            txtRegFullName.Text = "";
            txtRegEmail.Text = "";
            txtRegPhone.Text = "";
            txtRegPassword.Text = "";
            txtRegConfirmPassword.Text = "";
            txtRegFullName.Focus();
        }
    }

}
