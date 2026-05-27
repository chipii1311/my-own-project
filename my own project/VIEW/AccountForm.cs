using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class AccountForm : Form
    {
        public int LoggedInUserID { get; set; }
        private UserDTO currentUser;

        public AccountForm(int userID)
        {
            InitializeComponent();

            // Khởi tạo các giá trị cơ bản
            LoggedInUserID = userID;

            // Gọi hàm xây dựng giao diện (được định nghĩa bên file Designer.cs)
            BuildUI();

            // Lấy dữ liệu tài khoản
            LoadAccountData();
        }

        // ─────────────────────────────────────────────────────────
        //  DATA & EVENTS
        // ─────────────────────────────────────────────────────────
        private void LoadAccountData()
        {
            try
            {
                currentUser = UserBLL.GetUserByID(LoggedInUserID);
                if (currentUser != null)
                {
                    txtFullName.Text = currentUser.FullName;
                    txtEmail.Text = currentUser.Email;
                    txtPhone.Text = currentUser.Phone;
                    txtRole.Text = currentUser.Role;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser == null) return;
                currentUser.FullName = txtFullName.Text.Trim();
                currentUser.Phone = txtPhone.Text.Trim();

                bool success = UserBLL.UpdateUser(currentUser);
                if (success)
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPass.Text != txtConfirmPass.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success = UserBLL.ChangePassword(LoggedInUserID, txtOldPass.Text, txtNewPass.Text);
                if (success)
                {
                    MessageBox.Show("Đổi mật khẩu thành công! Hãy ghi nhớ mật khẩu mới.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOldPass.Clear();
                    txtNewPass.Clear();
                    txtConfirmPass.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}