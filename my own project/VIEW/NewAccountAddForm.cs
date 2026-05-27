using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewAccountAddForm : Form
    {
        public NewAccountAddForm()
        {
            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();
        }

        // ==========================================
        // LOGIC XỬ LÝ DỮ LIỆU
        // ==========================================
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra mật khẩu khớp nhau ngay tại UI
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp! Vui lòng kiểm tra lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Clear();
                    txtConfirmPassword.Focus(); // Bắt người dùng nhập lại ô xác nhận
                    return;
                }

                // Gom dữ liệu ném vào DTO
                UserDTO newUser = new UserDTO
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    Role = cboRole.Text,
                    IsActive = true
                };

                // Gọi BLL xử lý
                int newID = UserBLL.AddUser(newUser);

                // Nếu DB lưu thành công
                if (newID > 0)
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // Báo cho StaffForm biết để Load lại bảng
                    this.Close(); // Đóng Form popup
                }
            }
            catch (Exception ex)
            {
                // Hứng tất cả lỗi từ BLL (ví dụ: Thiếu email, Pass < 6 ký tự, Trùng Email...)
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}