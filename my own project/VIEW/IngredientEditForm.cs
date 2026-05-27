using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace my_own_project.VIEW
{
    public partial class IngredientEditForm : Form
    {
        private readonly int? _ingredientID;

        public IngredientEditForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            InitializeComponent();

            bool isEdit = _ingredientID.HasValue;

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI(isEdit);

            // Nếu là chế độ Sửa, tải dữ liệu cũ lên
            if (isEdit)
            {
                LoadIngredient(_ingredientID.Value);
            }
        }

        // ════════════════════════════════════════════════════════
        // DATA BINDING (TẢI DỮ LIỆU)
        // ════════════════════════════════════════════════════════
        private void LoadIngredient(int id)
        {
            try
            {
                var ing = IngredientBLL.GetIngredientByID(id);
                if (ing == null)
                {
                    MessageBox.Show("Không tìm thấy nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }

                txtName.Text = ing.IngredientName;
                txtUnit.Text = ing.Unit;
                txtStock.Text = ing.StockQuantity.ToString();
                txtMinStock.Text = ing.MinStock.ToString();
                txtPurchasePrice.Text = ing.PurchasePrice.ToString("0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        // ════════════════════════════════════════════════════════
        // SỰ KIỆN LƯU (XÁC THỰC & DB)
        // ════════════════════════════════════════════════════════
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string unit = txtUnit.Text.Trim();

                // Xác thực dữ liệu (Validation)
                if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên nguyên liệu không được để trống.");
                if (string.IsNullOrWhiteSpace(unit)) throw new Exception("Đơn vị tính không được để trống.");
                if (!float.TryParse(txtStock.Text.Trim(), out float stock)) throw new Exception("Số lượng tồn không hợp lệ.");
                if (!float.TryParse(txtMinStock.Text.Trim(), out float minStock)) throw new Exception("Mức tồn tối thiểu không hợp lệ.");
                if (!decimal.TryParse(txtPurchasePrice.Text.Trim(), out decimal price)) throw new Exception("Giá nhập không hợp lệ.");
                if (stock < 0) throw new Exception("Số lượng tồn không được âm.");
                if (minStock < 0) throw new Exception("Mức tồn tối thiểu không được âm.");
                if (price < 0) throw new Exception("Giá nhập không được âm.");

                var ing = new IngredientDTO
                {
                    IngredientName = name,
                    Unit = unit,
                    StockQuantity = stock,
                    MinStock = minStock,
                    PurchasePrice = price,
                    IsActive = true
                };

                // Lưu dữ liệu
                if (_ingredientID.HasValue)
                {
                    ing.IngredientID = _ingredientID.Value;
                    IngredientBLL.UpdateIngredient(ing);
                    MessageBox.Show("Cập nhật nguyên liệu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    IngredientBLL.AddIngredient(ing);
                    MessageBox.Show("Thêm nguyên liệu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK; // Báo cho frmInventory biết để Refresh Grid
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}