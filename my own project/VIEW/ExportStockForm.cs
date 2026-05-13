using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ExportStockForm : Form
    {
        private int? _ingredientID;

        private ComboBox cboIngredient;
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtNote;
        private Guna2Button btnSave;

        public ExportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            Text = "Xuất kho nguyên liệu";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(420, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            BuildUI();
            LoadIngredients();

            Shown += (s, e) =>
            {
                if (_ingredientID.HasValue)
                    cboIngredient.SelectedValue = _ingredientID.Value;
            };
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "XUẤT KHO NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(24, 20),
                AutoSize = true
            };

            var lblIngredient = new Label
            {
                Text = "Nguyên liệu",
                Location = new Point(24, 60),
                AutoSize = true
            };

            cboIngredient = new ComboBox
            {
                Location = new Point(24, 82),
                Size = new Size(326, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            if (_ingredientID.HasValue)
                cboIngredient.Enabled = false;

            var lblQty = new Label
            {
                Text = "Số lượng xuất",
                Location = new Point(24, 122),
                AutoSize = true
            };

            txtQuantity = new Guna2TextBox
            {
                Location = new Point(24, 144),
                Size = new Size(326, 36),
                BorderRadius = 6,
                PlaceholderText = "Nhập số lượng..."
            };

            var lblNote = new Label
            {
                Text = "Ghi chú",
                Location = new Point(24, 190),
                AutoSize = true
            };

            txtNote = new Guna2TextBox
            {
                Location = new Point(24, 212),
                Size = new Size(326, 36),
                BorderRadius = 6,
                PlaceholderText = "Ví dụ: dùng cho bếp, hủy hàng..."
            };

            btnSave = new Guna2Button
            {
                Text = "Xác nhận xuất",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                BorderRadius = 8,
                Size = new Size(160, 38),
                Location = new Point(190, 255),
                Cursor = Cursors.Hand
            };

            btnSave.Click += BtnSave_Click;

            Controls.AddRange(new Control[]
            {
                lblTitle,
                lblIngredient,
                cboIngredient,
                lblQty,
                txtQuantity,
                lblNote,
                txtNote,
                btnSave
            });
        }

        private void LoadIngredients()
        {
            DataTable dt = IngredientBLL.GetAllIngredients();

            cboIngredient.DataSource = dt;
            cboIngredient.DisplayMember = "IngredientName";
            cboIngredient.ValueMember = "IngredientID";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboIngredient.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!float.TryParse(txtQuantity.Text.Trim(), out float qty) || qty <= 0)
                {
                    MessageBox.Show("Số lượng xuất không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return;
                }

                int ingredientID = Convert.ToInt32(cboIngredient.SelectedValue);
                string note = txtNote.Text.Trim();

                int userID = Helpers.CurrentUser.UserID;
                int staffID = StaffBLL.GetStaffIDByUserID(userID);

                if (staffID <= 0)
                {
                    MessageBox.Show(
                        "Không tìm thấy thông tin nhân viên của tài khoản hiện tại.",
                        "Lỗi phân quyền",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                InventoryTransactionBLL.ExportIngredient(ingredientID, qty, staffID, note);

                MessageBox.Show("Xuất kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}