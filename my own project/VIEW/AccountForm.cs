using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class AccountForm : Form
    {
        // 1. Đã bỏ gán = 1 ở đây
        public int LoggedInUserID { get; set; }

        private UserDTO currentUser;

        private Guna2TextBox txtFullName, txtEmail, txtPhone, txtRole;
        private Guna2TextBox txtOldPass, txtNewPass, txtConfirmPass;

        // 2. Bắt buộc phải truyền userID vào khi mở Form
        public AccountForm(int userID)
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            // 3. Gán ID ngay lập tức
            this.LoggedInUserID = userID;

            BuildUI();

            // 4. Load dữ liệu (Lúc này chắc chắn ID đã là của người đang đăng nhập)
            LoadAccountData();
        }

        #region 1. GIAO DIỆN (UI)
        private void BuildUI()
        {
            // --- HEADER ---
            Guna2Panel pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 90, FillColor = Color.Transparent };
            Label lblTitle = new Label
            {
                Text = "TÀI KHOẢN CỦA TÔI",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 28, 230),
                AutoSize = true,
                Padding = new Padding(0, 15, 0, 0),
                Location = new Point(30, 20)
            };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            // --- LAYOUT CHÍNH ---
            TableLayoutPanel tlpMain = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(30, 0, 30, 30) };
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.Controls.Add(tlpMain);
            pnlHeader.BringToFront();

            int inputWidth = 450;

            // ==========================================
            // CỘT TRÁI: THÔNG TIN CÁ NHÂN
            // ==========================================
            Guna2Panel cardInfo = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(0, 0, 15, 0), Padding = new Padding(40) };
            tlpMain.Controls.Add(cardInfo, 0, 0);

            FlowLayoutPanel flpInfo = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent };
            cardInfo.Controls.Add(flpInfo);

            Label lblInfoTitle = new Label { Text = "👤 THÔNG TIN CÁ NHÂN", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), AutoSize = true, Margin = new Padding(0, 0, 0, 35) };
            flpInfo.Controls.Add(lblInfoTitle);

            txtFullName = CreateInput(flpInfo, "HỌ VÀ TÊN:", inputWidth);
            txtPhone = CreateInput(flpInfo, "SỐ ĐIỆN THOẠI:", inputWidth);

            txtEmail = CreateInput(flpInfo, "EMAIL (DÙNG ĐỂ ĐĂNG NHẬP):", inputWidth);
            txtEmail.ReadOnly = true;
            txtEmail.FillColor = Color.FromArgb(240, 243, 244);

            txtRole = CreateInput(flpInfo, "CHỨC VỤ CỦA BẠN:", inputWidth);
            txtRole.ReadOnly = true;
            txtRole.FillColor = Color.FromArgb(240, 243, 244);

            Guna2Button btnSaveInfo = new Guna2Button { Text = "CẬP NHẬT THÔNG TIN", Width = inputWidth, Height = 48, BorderRadius = 6, Font = new Font("Segoe UI", 11F, FontStyle.Bold), FillColor = Color.FromArgb(52, 152, 219), Cursor = Cursors.Hand, Margin = new Padding(0, 15, 0, 0) };
            btnSaveInfo.Click += BtnSaveInfo_Click;
            flpInfo.Controls.Add(btnSaveInfo);

            // ==========================================
            // CỘT PHẢI: ĐỔI MẬT KHẨU
            // ==========================================
            Guna2Panel cardPass = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.White, BorderRadius = 10, Margin = new Padding(15, 0, 0, 0), Padding = new Padding(40) };
            tlpMain.Controls.Add(cardPass, 1, 0);

            FlowLayoutPanel flpPass = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent };
            cardPass.Controls.Add(flpPass);

            Label lblPassTitle = new Label { Text = "🔒 ĐỔI MẬT KHẨU BẢO MẬT", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), AutoSize = true, Margin = new Padding(0, 0, 0, 35) };
            flpPass.Controls.Add(lblPassTitle);

            txtOldPass = CreateInput(flpPass, "MẬT KHẨU HIỆN TẠI:", inputWidth);
            txtOldPass.PasswordChar = '●';

            txtNewPass = CreateInput(flpPass, "MẬT KHẨU MỚI:", inputWidth);
            txtNewPass.PasswordChar = '●';

            txtConfirmPass = CreateInput(flpPass, "XÁC NHẬN MẬT KHẨU MỚI:", inputWidth);
            txtConfirmPass.PasswordChar = '●';

            Guna2Button btnChangePass = new Guna2Button { Text = "LƯU MẬT KHẨU MỚI", Width = inputWidth, Height = 48, BorderRadius = 6, Font = new Font("Segoe UI", 11F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand, Margin = new Padding(0, 15, 0, 0) };
            btnChangePass.Click += BtnChangePass_Click;
            flpPass.Controls.Add(btnChangePass);
        }

        private Guna2TextBox CreateInput(FlowLayoutPanel parent, string labelText, int width)
        {
            parent.Controls.Add(new Label { Text = labelText, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(127, 140, 141), AutoSize = true, Margin = new Padding(0, 0, 0, 8) });
            Guna2TextBox txt = new Guna2TextBox { Width = width, Height = 45, BorderRadius = 5, Font = new Font("Segoe UI", 11F), Margin = new Padding(0, 0, 0, 25), TextOffset = new Point(5, 0) };
            parent.Controls.Add(txt);
            return txt;
        }
        #endregion

        #region 2. XỬ LÝ SỰ KIỆN & LOGIC (EVENTS)

        // HÀM LẤY DỮ LIỆU ĐỔ LÊN FORM
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

        // SỰ KIỆN: CẬP NHẬT THÔNG TIN (TRÁI)
        private void BtnSaveInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser == null) return;

                // Cập nhật lại thuộc tính vào object currentUser
                currentUser.FullName = txtFullName.Text.Trim();
                currentUser.Phone = txtPhone.Text.Trim();

                // Gọi BLL để lưu
                bool success = UserBLL.UpdateUser(currentUser);
                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ Validate bên BLL và show lên
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // SỰ KIỆN: ĐỔI MẬT KHẨU (PHẢI)
        private void BtnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate xác nhận mật khẩu ngay tại UI trước khi vứt xuống BLL
                if (txtNewPass.Text != txtConfirmPass.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi hàm BLL
                bool success = UserBLL.ChangePassword(LoggedInUserID, txtOldPass.Text, txtNewPass.Text);

                if (success)
                {
                    MessageBox.Show("Đổi mật khẩu thành công! Hãy ghi nhớ mật khẩu mới của bạn.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset các ô nhập pass
                    txtOldPass.Clear();
                    txtNewPass.Clear();
                    txtConfirmPass.Clear();
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi sai pass cũ, pass quá ngắn... từ BLL
                MessageBox.Show(ex.Message, "Không thể đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}