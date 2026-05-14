using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms; // Bắt buộc phải có thư viện này
using my_own_project.DTO;
using my_own_project.BLL;

namespace my_own_project.VIEW
{
    public partial class NewAccountAddForm : Form
    {
        // Khai báo bằng Guna2 Control cho sang xịn mịn
        private Guna2TextBox txtFullName, txtEmail, txtPhone, txtPassword;
        private Guna2ComboBox cboRole;
        private Guna2Button btnSave, btnCancel;

        public NewAccountAddForm()
        {
            SetupCustomUI();
        }

        private void SetupCustomUI()
        {
            // --- 1. Cấu hình Form nền ---
            this.Text = "Thêm Tài Khoản Mới";
            this.Size = new Size(420, 630); // Form cao lên một chút cho thoáng
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // --- 2. Tiêu đề ---
            Label lblTitle = new Label
            {
                Text = "✨ THÊM TÀI KHOẢN ✨",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(88, 28, 230),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top, // Ép dính lên sát mép trên
                Height = 80
            };
            this.Controls.Add(lblTitle);

            // --- 3. Bố cục CHỐNG ĐÈ 100% (FlowLayoutPanel) ---
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(40, 0, 40, 20), // Canh lề 2 bên 40px
                AutoScroll = true
            };

            int ctrlWidth = 320; // Chiều rộng chuẩn cho các ô nhập liệu

            // Hàm tạo Label nhanh với khoảng cách Margin chống dính
            Label MakeLbl(string text) => new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5) // Cách bên trên 15px, cách bên dưới 5px
            };

            // --- CÁC TRƯỜNG NHẬP LIỆU ---
            flp.Controls.Add(MakeLbl("HỌ VÀ TÊN:"));
            txtFullName = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtFullName);

            flp.Controls.Add(MakeLbl("EMAIL (TÊN ĐĂNG NHẬP):"));
            txtEmail = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtEmail);

            flp.Controls.Add(MakeLbl("SỐ ĐIỆN THOẠI:"));
            txtPhone = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtPhone);

            flp.Controls.Add(MakeLbl("MẬT KHẨU:"));
            txtPassword = new Guna2TextBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), PasswordChar = '●', Margin = new Padding(0, 0, 0, 5) };
            flp.Controls.Add(txtPassword);

            flp.Controls.Add(MakeLbl("VAI TRÒ:"));
            cboRole = new Guna2ComboBox { Width = ctrlWidth, Height = 42, BorderRadius = 5, Font = new Font("Segoe UI", 10F), Margin = new Padding(0, 0, 0, 30) }; // Cách xa nút bấm 30px
            cboRole.Items.AddRange(new object[] { "Quản lý", "Nhân viên" });
            cboRole.SelectedIndex = 1; // Mặc định là Nhân viên
            flp.Controls.Add(cboRole);

            // --- 4. HÀNG NÚT BẤM (Chia 50/50 đều tăm tắp) ---
            TableLayoutPanel tlpBtns = new TableLayoutPanel
            {
                Width = ctrlWidth,
                Height = 45,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnCancel = new Guna2Button { Text = "HỦY BỎ", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(149, 165, 166), Cursor = Cursors.Hand };
            btnCancel.Click += (s, e) => this.Close();
            tlpBtns.Controls.Add(btnCancel, 0, 0);

            btnSave = new Guna2Button { Text = "LƯU TÀI KHOẢN", Dock = DockStyle.Fill, Margin = new Padding(10, 0, 0, 0), BorderRadius = 5, Font = new Font("Segoe UI", 10F, FontStyle.Bold), FillColor = Color.FromArgb(46, 204, 113), Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;
            tlpBtns.Controls.Add(btnSave, 1, 0);

            flp.Controls.Add(tlpBtns);

            // Gắn panel vào form và căn chỉnh lớp
            this.Controls.Add(flp);
            lblTitle.SendToBack(); // Đẩy tiêu đề ra sau để Dock.Top chiếm chỗ đúng
            flp.BringToFront();    // Đưa lưới nhập liệu lên trước
        }   

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UserDTO newUser = new UserDTO
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    Role = cboRole.Text,
                    IsActive = true
                };

                int newID = UserBLL.AddUser(newUser);

                if (newID > 0)
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Báo hiệu cho Form gốc biết là đã Thêm thành công (để nó Load lại bảng)
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}