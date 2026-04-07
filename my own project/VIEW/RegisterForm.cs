using my_own_project.BLL;
using my_own_project.DAL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy dữ liệu từ giao diện
                string fullName = txtFullName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string password = txtPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                // 2. Kiểm tra sơ bộ trên UI (Tránh gọi xuống BLL nếu thiếu dữ liệu cơ bản)
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    // Giả sử bạn đang dùng Guna2MessageDialog (diaError) giống form Login
                    diaSth.Show("Please fill in all required information!", "Incomplete information");
                    return;
                }

                if (password != confirmPassword)
                {
                    diaSth.Show("Password entry does not match!", "Verification error");
                    return;
                }

                // 3. Đóng gói dữ liệu vào UserDTO
                // Lưu ý: Đưa luôn raw password vào thuộc tính PasswordHash vì UserBLL.AddUser của bạn đã có hàm tự động băm (hash) nó rồi.
                UserDTO newUser = new UserDTO
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    PasswordHash = password,
                    Role = "User", // Phân quyền mặc định cho tài khoản đăng ký mới
                    IsActive = true
                };

                // 4. Gọi BLL để lưu vào DB
                int newUserId = UserBLL.AddUser(newUser);

                if (newUserId > 0)
                {
                    diaSth.Show("Account created successfully! Please log in.", "Success");

                    // 5. Quay lại trang Login
                    // Tìm form Login đang bị ẩn và hiển thị nó lên
                    var loginForm = Application.OpenForms["LoginForm"];
                    if (loginForm != null)
                    {
                        loginForm.Show();
                    }

                    // Đóng form đăng ký này lại
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                diaSth.Show(ex.Message, "Registration error");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Xoá Data còn trống trong txt
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();

            // Tìm form Login đang bị ẩn và hiển thị nó lên
            var loginForm = Application.OpenForms["LoginForm"];
            if (loginForm != null)
            {
                loginForm.Show();
            }

            // Tắt form đăng ký
            this.Close();
        }
    }
}
