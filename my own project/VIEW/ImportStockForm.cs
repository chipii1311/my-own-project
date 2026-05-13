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
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtPrice;
        private Guna2TextBox txtNote;
        private Guna2Button btnSave;

        public ImportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            InitializeComponent();

            Controls.Clear();
            Text = "Nhập kho nguyên liệu";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(420, 340);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            BuildUI();
            LoadIngredients();

            Shown += (s, e) =>
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

            var lblIngredient = new Label
            {
                Text = "Nguyên liệu",
                Location = new Point(24, 60),
                AutoSize = true
            };

            cboIngredient = new ComboBox
            {
                Location = new Point(24, 80),
                Size = new Size(326, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            if (_ingredientID.HasValue)
                cboIngredient.Enabled = false;

            var lblQty = new Label
            {
                Text = "Số lượng nhập",
                Location = new Point(24, 120),
                AutoSize = true
            };

            txtQuantity = new Guna2TextBox
            {
                Location = new Point(24, 140),
                Size = new Size(150, 36),
                BorderRadius = 6,
                PlaceholderText = "Số lượng..."
            };

            var lblPrice = new Label
            {
                Text = "Giá nhập",
                Location = new Point(200, 120),
                AutoSize = true
            };

            txtPrice = new Guna2TextBox
            {
                Location = new Point(200, 140),
                Size = new Size(150, 36),
                BorderRadius = 6,
                PlaceholderText = "VNĐ..."
            };

            var lblNote = new Label
            {
                Text = "Ghi chú",
                Location = new Point(24, 190),
                AutoSize = true
            };

            txtNote = new Guna2TextBox
            {
                Location = new Point(24, 210),
                Size = new Size(326, 36),
                BorderRadius = 6,
                PlaceholderText = "Ghi chú nếu có..."
            };

            btnSave = new Guna2Button
            {
                Text = "Xác nhận nhập",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(108, 99, 255),
                ForeColor = Color.White,
                BorderRadius = 8,
                Size = new Size(180, 40),
                Location = new Point(170, 270),
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
                lblPrice,
                txtPrice,
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

        private void SetIngredient(int ingredientID)
        {
            if (cboIngredient.Items.Count > 0)
                cboIngredient.SelectedValue = ingredientID;
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
                    MessageBox.Show("Số lượng nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
                {
                    MessageBox.Show("Giá nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
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

                InventoryTransactionBLL.ImportIngredient(ingredientID, qty, staffID, note);

                IngredientDTO ingredient = IngredientBLL.GetIngredientByID(ingredientID);

                if (ingredient != null)
                {
                    ingredient.PurchasePrice = price;
                    IngredientBLL.UpdateIngredient(ingredient);
                }

                MessageBox.Show("Nhập kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}