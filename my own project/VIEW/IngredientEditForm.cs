using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class IngredientEditForm : Form
    {
        private readonly int? _ingredientID;

        private Label lblTitle;

        private Guna2TextBox txtName;
        private Guna2TextBox txtUnit;
        private Guna2TextBox txtStock;
        private Guna2TextBox txtMinStock;
        private Guna2TextBox txtPurchasePrice;

        private Guna2Button btnSave;
        private Guna2Button btnCancel;

        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_DARK = Color.FromArgb(90, 80, 230);
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color C_BORDER = Color.FromArgb(232, 232, 240);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);

        public IngredientEditForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            InitializeComponent();
            Controls.Clear();

            Text = _ingredientID.HasValue ? "Cập nhật nguyên liệu" : "Thêm nguyên liệu";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(460, 430);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = C_WHITE;

            BuildUI();

            if (_ingredientID.HasValue)
                LoadIngredient(_ingredientID.Value);
        }

        private void BuildUI()
        {
            lblTitle = new Label
            {
                Text = _ingredientID.HasValue ? "CẬP NHẬT NGUYÊN LIỆU" : "THÊM NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                Location = new Point(28, 24),
                AutoSize = true
            };

            txtName = CreateInput("Tên nguyên liệu", "Ví dụ: Thịt bò", 28, 78);
            txtUnit = CreateInput("Đơn vị tính", "kg, lon, hộp...", 28, 144);
            txtStock = CreateInput("Số lượng tồn", "0", 28, 210);
            txtMinStock = CreateInput("Mức tồn tối thiểu", "0", 238, 210);
            txtPurchasePrice = CreateInput("Giá nhập", "0", 28, 276);

            btnSave = new Guna2Button
            {
                Text = _ingredientID.HasValue ? "Lưu thay đổi" : "Thêm mới",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                BorderRadius = 8,
                Size = new Size(150, 40),
                Location = new Point(266, 342),
                Cursor = Cursors.Hand
            };

            btnSave.HoverState.FillColor = C_PURPLE_DARK;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Guna2Button
            {
                Text = "Hủy",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_BG,
                ForeColor = C_TEXT,
                BorderColor = C_BORDER,
                BorderThickness = 1,
                BorderRadius = 8,
                Size = new Size(100, 40),
                Location = new Point(154, 342),
                Cursor = Cursors.Hand
            };

            btnCancel.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
        }

        private Guna2TextBox CreateInput(string label, string placeholder, int x, int y)
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                Location = new Point(x, y - 22),
                AutoSize = true
            };

            Guna2TextBox txt = new Guna2TextBox
            {
                Location = new Point(x, y),
                Size = label == "Mức tồn tối thiểu" ? new Size(178, 38) : new Size(388, 38),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                FillColor = C_BG,
                PlaceholderText = placeholder,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                PlaceholderForeColor = C_MUTED
            };

            txt.FocusedState.BorderColor = C_PURPLE;
            txt.HoverState.BorderColor = C_PURPLE;

            Controls.Add(lbl);
            Controls.Add(txt);

            return txt;
        }

        private void LoadIngredient(int ingredientID)
        {
            try
            {
                IngredientDTO ingredient = IngredientBLL.GetIngredientByID(ingredientID);

                if (ingredient == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy nguyên liệu.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    Close();
                    return;
                }

                txtName.Text = ingredient.IngredientName;
                txtUnit.Text = ingredient.Unit;
                txtStock.Text = ingredient.StockQuantity.ToString();
                txtMinStock.Text = ingredient.MinStock.ToString();
                txtPurchasePrice.Text = ingredient.PurchasePrice.ToString("0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải nguyên liệu: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IngredientDTO ingredient = ReadInput();

                if (_ingredientID.HasValue)
                {
                    ingredient.IngredientID = _ingredientID.Value;
                    IngredientBLL.UpdateIngredient(ingredient);

                    MessageBox.Show(
                        "Cập nhật nguyên liệu thành công.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    IngredientBLL.AddIngredient(ingredient);

                    MessageBox.Show(
                        "Thêm nguyên liệu thành công.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private IngredientDTO ReadInput()
        {
            string name = txtName.Text.Trim();
            string unit = txtUnit.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Tên nguyên liệu không được để trống.");

            if (string.IsNullOrWhiteSpace(unit))
                throw new Exception("Đơn vị tính không được để trống.");

            if (!float.TryParse(txtStock.Text.Trim(), out float stock))
                throw new Exception("Số lượng tồn không hợp lệ.");

            if (!float.TryParse(txtMinStock.Text.Trim(), out float minStock))
                throw new Exception("Mức tồn tối thiểu không hợp lệ.");

            if (!decimal.TryParse(txtPurchasePrice.Text.Trim(), out decimal price))
                throw new Exception("Giá nhập không hợp lệ.");

            if (stock < 0)
                throw new Exception("Số lượng tồn không được âm.");

            if (minStock < 0)
                throw new Exception("Mức tồn tối thiểu không được âm.");

            if (price < 0)
                throw new Exception("Giá nhập không được âm.");

            return new IngredientDTO
            {
                IngredientName = name,
                Unit = unit,
                StockQuantity = stock,
                MinStock = minStock,
                PurchasePrice = price,
                IsActive = true
            };
        }
    }
}
