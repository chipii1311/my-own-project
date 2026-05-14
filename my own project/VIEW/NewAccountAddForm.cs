using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using my_own_project.DTO;
using my_own_project.BLL;

namespace my_own_project.VIEW
{
    public partial class NewAccountAddForm : Form
    {
        // ==========================================
        // 1. KHAI BÁO CÁC CONTROL GUNA2
        // ==========================================
        private Guna2TextBox txtFullName, txtEmail, txtPhone, txtPassword, txtConfirmPassword;
        private Guna2ComboBox cboRole;
        private Guna2Button btnSave, btnCancel;

        public NewAccountAddForm()
        {
            // Bỏ qua InitializeComponent() mặc định, dùng code tay 100%
            SetupCustomUI();
        }

        // ==========================================
        // 2. KHU VỰC VẼ GIAO DIỆN CHỐNG ĐÈ
        // ==========================================
        private void SetupCustomUI()
        {
            // --- Cấu hình Form nền ---
            this.Text = "Thêm Tài Khoản Mới";
            this.Size = new Size(420, 710); // Chiều cao đủ để chứa thêm ô Xác nhận Pass
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // --- Tiêu đề ---
            Label lblTitle = new Label
            {
                Text = "✨ THÊM TÀI KHOẢN ✨",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 28, 230),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };
            this.Controls.Add(lblTitle);

            // --- Lưới chính (FlowLayoutPanel) giúp tự động nối đuôi nhau, chống đè pixel ---
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(40, 0, 40, 20), // Canh lề trái phải 40px
                AutoScroll = true
            };

            int ctrlWidth = 320; // Kích thước chuẩn cho toàn bộ ô nhập

            // Hàm con sinh Label tự động
            Label MakeLbl(string text) => new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };

            // --- HỌ VÀ TÊN ---
            flp.Controls.Add(MakeLbl("HỌ VÀ TÊN:"));
            txtFullName = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtFullName);

            // --- EMAIL ---
            flp.Controls.Add(MakeLbl("EMAIL (TÊN ĐĂNG NHẬP):"));
            txtEmail = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtEmail);

            // --- SỐ ĐIỆN THOẠI ---
            flp.Controls.Add(MakeLbl("SỐ ĐIỆN THOẠI:"));
            txtPhone = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtPhone);

            // --- MẬT KHẨU ---
            flp.Controls.Add(MakeLbl("MẬT KHẨU:"));
            txtPassword = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), PasswordChar = '●', Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtPassword);

            // --- XÁC NHẬN MẬT KHẨU ---
            flp.Controls.Add(MakeLbl("XÁC NHẬN MẬT KHẨU:"));
            txtConfirmPassword = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), PasswordChar = '●', Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtConfirmPassword);

            // --- VAI TRÒ ---
            flp.Controls.Add(MakeLbl("VAI TRÒ:"));
            cboRole = new Guna2ComboBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 30) }; // Cách xa nút bấm 30px
            cboRole.Items.AddRange(new object[] { "Quản lý", "Nhân viên" });
            cboRole.SelectedIndex = 1; // Mặc định là Nhân viên
            flp.Controls.Add(cboRole);

            // --- NÚT BẤM (Chia cột 50/50 bằng TableLayoutPanel) ---
            TableLayoutPanel tlpBtns = new TableLayoutPanel { Width = ctrlWidth, Height = 45, ColumnCount = 2, RowCount = 1, Margin = new Padding(0) };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnCancel = new Guna2Button { Text = "HỦY BỎ", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(149, 165, 166), Cursor = Cursors.Hand };
            btnCancel.Click += (s, e) => this.Close(); // Bấm Hủy là tự đóng Form
            tlpBtns.Controls.Add(btnCancel, 0, 0);

            btnSave = new Guna2Button { Text = "LƯU TÀI KHOẢN", Dock = DockStyle.Fill, Margin = new Padding(10, 0, 0, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;
            tlpBtns.Controls.Add(btnSave, 1, 0);

            // Ráp vào Lưới
            flp.Controls.Add(tlpBtns);

            // Đẩy tất cả lên Form
            this.Controls.Add(flp);
            lblTitle.SendToBack(); // Đẩy Title ra sau để Lưới đẩy lên sát mép Title
            flp.BringToFront();
        }

        // ==========================================
        // 3. LOGIC XỬ LÝ DỮ LIỆU
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