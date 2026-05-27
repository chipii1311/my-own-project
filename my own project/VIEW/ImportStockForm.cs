using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ImportStockForm : Form
    {
        private int? _ingredientID;

        public ImportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            InitializeComponent();
            
            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            this.Load += ImportStockForm_Load;
        }

        // ════════════════════════════════════════════════════════
        // DATA BINDING
        // ════════════════════════════════════════════════════════
        private void ImportStockForm_Load(object sender, EventArgs e)
        {
            LoadIngredients();

            // Nếu truyền ID từ form cha sang, tự động chọn đúng nguyên liệu đó
            if (_ingredientID.HasValue)
            {
                cboIngredient.SelectedValue = _ingredientID.Value;
            }
        }

        private void LoadIngredients()
        {
            try
            {
                DataTable dt = IngredientBLL.GetAllIngredients();
                cboIngredient.DataSource = dt;
                cboIngredient.DisplayMember = "IngredientName";
                cboIngredient.ValueMember = "IngredientID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        // SỰ KIỆN NÚT BẤM (VALIDATION & SAVE)
        // ════════════════════════════════════════════════════════
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

                // 1. Lưu giao dịch nhập kho
                InventoryTransactionBLL.ImportIngredient(ingredientID, qty, 0, note);

                // 2. Cập nhật lại giá nhập (PurchasePrice) cho nguyên liệu nếu có sự thay đổi giá
                IngredientDTO ing = IngredientBLL.GetIngredientByID(ingredientID);
                if (ing != null) 
                { 
                    ing.PurchasePrice = price; 
                    IngredientBLL.UpdateIngredient(ing); 
                }

                MessageBox.Show("Nhập kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Báo hiệu cho form cha biết đã lưu thành công
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}