// ImportStockForm.cs
using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ImportStockForm : Form
    {
        private int? _ingredientID;
        private ComboBox cboIngredient;
        private Guna2TextBox txtQuantity, txtPrice, txtNote;
        private Guna2Button btnSave;

        public ImportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;
            InitializeComponent();
            this.Text = "Nhập kho nguyên liệu";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(420, 340);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            BuildUI();
            LoadIngredients();

            // Đợi form hiển thị xong mới set nguyên liệu, tránh lỗi binding
            this.Shown += (s, e) =>
            {
                if (_ingredientID.HasValue)
                    SetIngredient(_ingredientID.Value);
            };
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "NHẬP KHO NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(108, 99, 255),
                Location = new Point(24, 20),
                AutoSize = true
            };

            // Nguyên liệu
            var lblIngredient = new Label { Text = "Nguyên liệu", Location = new Point(24, 60), AutoSize = true };
            cboIngredient = new ComboBox
            {
                Location = new Point(24, 80),
                Size = new Size(200, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            if (_ingredientID.HasValue) cboIngredient.Enabled = false; // không cho đổi nếu mở từ danh sách

            // Số lượng
            var lblQty = new Label { Text = "Số lượng nhập", Location = new Point(24, 120), AutoSize = true };
            txtQuantity = new Guna2TextBox
            {
                Location = new Point(24, 140),
                Size = new Size(150, 36),
                BorderRadius = 6,
                PlaceholderText = "Nhập số lượng..."
            };

            // Giá nhập
            var lblPrice = new Label { Text = "Giá nhập (VNĐ)", Location = new Point(200, 120), AutoSize = true };
            txtPrice = new Guna2TextBox
            {
                Location = new Point(200, 140),
                Size = new Size(150, 36),
                BorderRadius = 6,
                PlaceholderText = "Nhập giá..."
            };

            // Ghi chú
            var lblNote = new Label { Text = "Ghi chú", Location = new Point(24, 190), AutoSize = true };
            txtNote = new Guna2TextBox
            {
                Location = new Point(24, 210),
                Size = new Size(326, 36),
                BorderRadius = 6,
                PlaceholderText = "Ghi chú (nếu có)"
            };

            // Nút Lưu
            btnSave = new Guna2Button
            {
                Text = "✅ Xác nhận nhập",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(108, 99, 255),
                ForeColor = Color.White,
                BorderRadius = 8,
                Size = new Size(180, 40),
                Location = new Point(170, 270),
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] {
                lblTitle, lblIngredient, cboIngredient,
                lblQty, txtQuantity, lblPrice, txtPrice,
                lblNote, txtNote, btnSave
            });
        }

        private void LoadIngredients()
        {
            DataTable dt = IngredientBLL.GetAllIngredients();
            cboIngredient.DataSource = dt;
            cboIngredient.DisplayMember = "IngredientName";
            cboIngredient.ValueMember = "IngredientID";
        }

        private void SetIngredient(int ingredientID)
        {
            // Chọn đúng nguyên liệu trong ComboBox (phải gọi sau khi DataSource đã sẵn sàng)
            if (cboIngredient.Items.Count > 0)
                cboIngredient.SelectedValue = ingredientID;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // ---- Validate dữ liệu cơ bản ----
                if (cboIngredient.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!float.TryParse(txtQuantity.Text.Trim(), out float qty) || qty <= 0)
                {
                    MessageBox.Show("Số lượng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return;
                }
                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
                {
                    MessageBox.Show("Giá nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }

                int ingredientID = (int)cboIngredient.SelectedValue;
                string note = txtNote.Text.Trim();

                // ---- Xác định StaffID từ User hiện tại ----
                int userID = Helpers.CurrentUser.UserID; // lấy từ session đăng nhập
                int staffID = GetStaffIDFromUserID(userID);
                if (staffID == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin nhân viên của bạn.\nHãy liên hệ quản lý để được phân công.",
                                    "Lỗi phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ---- Thực hiện nhập kho ----
                InventoryTransactionBLL.ImportIngredient(ingredientID, qty, staffID, note);

                // Cập nhật giá nhập mới vào Ingredient
                var ingredient = IngredientBLL.GetIngredientByID(ingredientID);
                if (ingredient != null)
                {
                    ingredient.PurchasePrice = price;
                    IngredientBLL.UpdateIngredient(ingredient);
                }

                MessageBox.Show("✅ Nhập kho thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm lấy StaffID từ UserID (dùng DAL)
        private int GetStaffIDFromUserID(int userID)
        {
            try
            {
                // Giả sử bạn đã có Stored Procedure sp_Staff_GetByUserID hoặc truy vấn tương tự
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(
                    $"SELECT StaffID FROM Staff WHERE UserID = {userID}");
                if (dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0]["StaffID"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetStaffIDFromUserID error: " + ex.Message);
            }
            return 0; // không tìm thấy
        }
    }
}