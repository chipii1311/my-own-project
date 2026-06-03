using my_own_project.BLL;
using my_own_project.DTO;
using my_own_project.VIEW; // Phải using cái này để gọi NewMainForm
using System;
using System.Windows.Forms;

using System.Data.SqlClient;

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

                // 1. Gọi hàm Login từ BLL (Hàm này đã lo việc băm pass và so sánh với DB)
                UserDTO loggedUser = UserBLL.Login(email, password);

                // Nếu đăng nhập thành công, loggedUser sẽ chứa thông tin người đó
                if (loggedUser != null)
                {
                    // (Nếu bạn có class Helper CurrentUser thì cứ giữ lại dòng này)
                    // CurrentUser.Login(loggedUser); 

                    // 2. KHỞI TẠO FORM GIAO DIỆN MỚI SIÊU ĐẸP
                    NewMainForm mainForm = new NewMainForm();

                    // 3. TRUYỀN PHÂN QUYỀN SANG FORM CHÍNH
                    // Gửi chức vụ (Role) để mainForm biết đường ẩn các nút cấm
                    mainForm.UserRole = loggedUser.Role;

                    // Gửi thêm thông tin để hiển thị tên lên cái nút Avatar "Xin chào"
                    mainForm.LoggedInUserName = loggedUser.FullName;

                    // Gửi UserID để sau này mở AccountForm biết lấy Data của ai
                    mainForm.LoggedInUserID = loggedUser.UserID;

                    // 4. MỞ FORM CHÍNH VÀ ĐÓNG HẲN FORM LOGIN
                    this.Hide();
                    mainForm.ShowDialog(); // ShowDialog chặn không cho nhảy xuống dòng This.Close() bên dưới

                    // Khi người dùng tắt NewMainForm, dòng này mới chạy để kết liễu app hoàn toàn
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Thay thế bằng MessageBox mặc định nếu bạn bị lỗi thư viện diaError
                MessageBox.Show(ex.Message, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Nhớ lại bài học "Zero Trust": Không cho tự đăng ký tài khoản quản lý!
            // Bạn nên bỏ nút này, hoặc show thông báo kêu gọi quản lý cấp tài khoản.
            MessageBox.Show("Vui lòng liên hệ Quản lý hoặc Admin hệ thống để được cấp tài khoản làm việc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}