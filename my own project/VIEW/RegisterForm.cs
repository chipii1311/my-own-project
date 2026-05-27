using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            BuildUI(); // Hàm dựng giao diện
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra sơ bộ trên UI
                if (string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Gom dữ liệu vào DTO
                UserDTO newUser = new UserDTO
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    Role = "Nhân viên", // Mặc định là nhân viên
                    IsActive = true
                };

                // 3. Gọi BLL để lưu vào DB
                int newUserId = UserBLL.AddUser(newUser);

                if (newUserId > 0)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 4. Quay lại trang Login
                    var loginForm = Application.OpenForms["LoginForm"];
                    if (loginForm != null) loginForm.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            var loginForm = Application.OpenForms["LoginForm"];
            if (loginForm != null) loginForm.Show();
            this.Close();
        }
    }
}