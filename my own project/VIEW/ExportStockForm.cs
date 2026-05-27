using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ExportStockForm : Form
    {
        private readonly int? _ingredientID;

        public ExportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            this.Load += ExportStockForm_Load;
        }

        // ════════════════════════════════════════════════════════
        // DATA BINDING
        // ════════════════════════════════════════════════════════
        private void ExportStockForm_Load(object sender, EventArgs e)
        {
            LoadIngredients();

            // Nếu có truyền ID nguyên liệu vào thì tự động chọn nguyên liệu đó
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
        // SỰ KIỆN NÚT BẤM
        // ════════════════════════════════════════════════════════
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboIngredient.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!float.TryParse(txtQuantity.Text.Trim(), out float qty) || qty <= 0)
                {
                    MessageBox.Show("Số lượng xuất không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ── Bỏ kiểm tra nhân viên, truyền 0 (NULL) vào DB ──
                InventoryTransactionBLL.ExportIngredient(
                    Convert.ToInt32(cboIngredient.SelectedValue),
                    qty,
                    0,
                    txtNote.Text.Trim()
                );

                MessageBox.Show("Xuất kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}