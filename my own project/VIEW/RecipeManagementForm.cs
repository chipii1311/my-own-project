using Guna.UI2.WinForms;
using my_own_project.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class RecipeManagementForm : Form
    {
        private int selectedRecipeID = 0;

        public RecipeManagementForm()
        {
            InitializeComponent();
            BuildUI(); // Nếu bạn có hàm này trong Designer
            this.Load += RecipeManagementForm_Load;
        }

        private void RecipeManagementForm_Load(object sender, EventArgs e)
        {
            LoadMenuItems();
            LoadIngredients();
            LoadRecipeBySelectedMenu();
        }

        // ===================== DATA LOADING =====================
        private void LoadMenuItems()
        {
            try
            {
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllLite");
                cboMenuItem.DataSource = dt;
                cboMenuItem.DisplayMember = "ItemName";
                cboMenuItem.ValueMember = "MenuItemID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadIngredients()
        {
            try
            {
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Ingredient_GetAll");
                cboIngredient.DataSource = dt.Copy();
                cboIngredient.DisplayMember = "IngredientName";
                cboIngredient.ValueMember = "IngredientID";

                dgvIngredients.DataSource = dt;
                FormatIngredientGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRecipeBySelectedMenu()
        {
            int menuItemID = GetSelectedMenuItemID();
            if (menuItemID <= 0)
            {
                dgvRecipe.DataSource = null;
                UpdateSelectedFoodText(0);
                return;
            }

            try
            {
                SqlParameter[] parameters = { new SqlParameter("@MenuItemID", menuItemID) };
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Recipe_GetByMenuItem", parameters);

                dgvRecipe.DataSource = dt;
                FormatRecipeGrid();
                UpdateSelectedFoodText(dt.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải công thức: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== EVENTS =====================
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
            LoadIngredients();
            LoadRecipeBySelectedMenu();
        }

        private void CboMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearInput(false);
            LoadRecipeBySelectedMenu();
        }

        private void TxtSearchIngredient_TextChanged(object sender, EventArgs e)
        {
            FilterIngredientGrid();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInput(true);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int menuItemID = GetSelectedMenuItemID();
                int ingredientID = GetSelectedIngredientID();
                decimal quantity = ReadQuantity();

                if (menuItemID <= 0) throw new Exception("Vui lòng chọn món ăn.");
                if (ingredientID <= 0) throw new Exception("Vui lòng chọn nguyên liệu.");
                if (quantity <= 0) throw new Exception("Định lượng phải lớn hơn 0.");

                if (RecipeExists(menuItemID, ingredientID))
                {
                    if (MessageBox.Show("Nguyên liệu này đã tồn tại. Cập nhật định lượng?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@MenuItemID", menuItemID),
                        new SqlParameter("@IngredientID", ingredientID),
                        new SqlParameter("@Quantity", quantity)
                    };
                    DataHelper.ExecuteSP("sp_Recipe_Update", param);
                }
                else
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@MenuItemID", menuItemID),
                        new SqlParameter("@IngredientID", ingredientID),
                        new SqlParameter("@Quantity", quantity)
                    };
                    DataHelper.ExecuteSP("sp_Recipe_Insert", param);
                }

                MessageBox.Show("Lưu công thức thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRecipeID <= 0) throw new Exception("Vui lòng chọn dòng cần cập nhật.");

            try
            {
                int ingredientID = GetSelectedIngredientID();
                decimal quantity = ReadQuantity();

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@RecipeID", selectedRecipeID),
                    new SqlParameter("@IngredientID", ingredientID),
                    new SqlParameter("@Quantity", quantity)
                };

                DataHelper.ExecuteSP("sp_Recipe_Update", parameters);

                MessageBox.Show("Cập nhật thành công.", "Thành công");
                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedRecipeID <= 0) return;

            if (MessageBox.Show("Xóa nguyên liệu này khỏi công thức?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                SqlParameter[] parameters = { new SqlParameter("@RecipeID", selectedRecipeID) };
                DataHelper.ExecuteSP("sp_Recipe_Delete", parameters);

                MessageBox.Show("Xóa thành công.");
                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // ── Grid row click ────────────────────────────────────────────────────
        private void DgvRecipe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) FillInputFromRecipeRow(e.RowIndex);
        }

        private void DgvRecipe_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) FillInputFromRecipeRow(e.RowIndex);
        }

        private void DgvIngredients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvIngredients.Rows[e.RowIndex];
            if (row.Cells["IngredientID"].Value != DBNull.Value)
                cboIngredient.SelectedValue = Convert.ToInt32(row.Cells["IngredientID"].Value);
        }

        private void DgvRecipe_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatRecipeGrid();
        }

        private void DgvIngredients_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatIngredientGrid();
        }

        // ===================== GRID FORMATTING =====================
        private void FormatRecipeGrid()
        {
            if (dgvRecipe == null || dgvRecipe.Columns.Count == 0) return;

            HideCol(dgvRecipe, "RecipeID");
            HideCol(dgvRecipe, "MenuItemID");
            HideCol(dgvRecipe, "IngredientID");

            SetHeader(dgvRecipe, "ItemName", "Món ăn");
            SetHeader(dgvRecipe, "IngredientName", "Nguyên liệu");
            SetHeader(dgvRecipe, "Unit", "Đơn vị");
            SetHeader(dgvRecipe, "Quantity", "Định lượng");
            SetHeader(dgvRecipe, "StockQuantity", "Tồn kho");
            SetHeader(dgvRecipe, "MinStock", "Tối thiểu");

            AlignRight(dgvRecipe, "Quantity");
            AlignRight(dgvRecipe, "StockQuantity");
            AlignRight(dgvRecipe, "MinStock");
        }

        private void FormatIngredientGrid()
        {
            if (dgvIngredients == null || dgvIngredients.Columns.Count == 0) return;

            HideCol(dgvIngredients, "IngredientID");
            HideCol(dgvIngredients, "IsActive");
            HideCol(dgvIngredients, "PurchasePrice");

            SetHeader(dgvIngredients, "IngredientName", "Nguyên liệu");
            SetHeader(dgvIngredients, "Unit", "Đơn vị");
            SetHeader(dgvIngredients, "StockQuantity", "Tồn kho");
            SetHeader(dgvIngredients, "MinStock", "Tối thiểu");
            SetHeader(dgvIngredients, "StockStatus", "Trạng thái");

            AlignRight(dgvIngredients, "StockQuantity");
            AlignRight(dgvIngredients, "MinStock");

            // Color status column
            foreach (DataGridViewRow row in dgvIngredients.Rows)
            {
                if (!dgvIngredients.Columns.Contains("StockStatus")) break;

                string status = row.Cells["StockStatus"].Value?.ToString() ?? "";
                switch (status)
                {
                    case "Hết hàng":
                        row.Cells["StockStatus"].Style.ForeColor = Color.FromArgb(121, 31, 31);
                        row.Cells["StockStatus"].Style.BackColor = Color.FromArgb(252, 235, 235);
                        row.Cells["StockStatus"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    case "Sắp hết":
                        row.Cells["StockStatus"].Style.ForeColor = Color.FromArgb(99, 56, 6);
                        row.Cells["StockStatus"].Style.BackColor = Color.FromArgb(250, 238, 218);
                        row.Cells["StockStatus"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                    default:
                        row.Cells["StockStatus"].Style.ForeColor = Color.FromArgb(39, 80, 10);
                        row.Cells["StockStatus"].Style.BackColor = Color.FromArgb(234, 243, 222);
                        row.Cells["StockStatus"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                        break;
                }
            }
        }

        // ===================== CRUD HELPERS =====================
        private bool RecipeExists(int menuItemID, int ingredientID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@MenuItemID", menuItemID),
                new SqlParameter("@IngredientID", ingredientID)
            };

            DataTable dt = DataHelper.ExecuteSPGetTable("sp_Recipe_CheckExists", parameters);
            return dt.Rows.Count > 0;
        }

        private void FillInputFromRecipeRow(int rowIndex)
        {
            DataGridViewRow row = dgvRecipe.Rows[rowIndex];

            selectedRecipeID = Convert.ToInt32(row.Cells["RecipeID"].Value);

            if (row.Cells["IngredientID"].Value != DBNull.Value)
                cboIngredient.SelectedValue = Convert.ToInt32(row.Cells["IngredientID"].Value);

            if (row.Cells["Quantity"].Value != DBNull.Value)
                txtQuantity.Text = Convert.ToDecimal(row.Cells["Quantity"].Value)
                                    .ToString("0.###", CultureInfo.InvariantCulture);
        }

        // ===================== GENERAL HELPERS =====================
        private int GetSelectedMenuItemID()
        {
            if (cboMenuItem?.SelectedValue == null) return 0;
            return Convert.ToInt32(cboMenuItem.SelectedValue);
        }

        private int GetSelectedIngredientID()
        {
            if (cboIngredient?.SelectedValue == null) return 0;
            return Convert.ToInt32(cboIngredient.SelectedValue);
        }

        private decimal ReadQuantity()
        {
            string text = txtQuantity.Text.Trim().Replace(",", ".");
            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal qty))
                throw new Exception("Định lượng không hợp lệ. Ví dụ: 0.25 hoặc 1");
            return qty;
        }

        private string SqlDecimal(decimal value) => value.ToString(CultureInfo.InvariantCulture);

        private void ClearInput(bool clearIngredient)
        {
            selectedRecipeID = 0;
            txtQuantity.Clear();
            if (clearIngredient && cboIngredient != null && cboIngredient.Items.Count > 0)
                cboIngredient.SelectedIndex = 0;
        }

        private void UpdateSelectedFoodText(int recipeCount)
        {
            string foodName = cboMenuItem?.Text ?? "";
            lblSelectedFood.Text = string.IsNullOrWhiteSpace(foodName) ? "Chưa chọn món" : "Công thức: " + foodName;
            lblRecipeCount.Text = recipeCount > 0 ? recipeCount + " nguyên liệu" : "";
        }

        private void FilterIngredientGrid()
        {
            if (dgvIngredients == null) return;
            if (!(dgvIngredients.DataSource is DataTable dt)) return;

            string kw = txtSearchIngredient.Text.Trim().Replace("'", "''");
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(kw)
                ? ""
                : $"IngredientName LIKE '%{kw}%' OR Unit LIKE '%{kw}%'";
        }

        // ── Grid helper methods ───────────────────────────────────────────────
        private void HideCol(DataGridView grid, string col)
        {
            if (grid.Columns.Contains(col)) grid.Columns[col].Visible = false;
        }

        private void SetHeader(DataGridView grid, string col, string header)
        {
            if (grid.Columns.Contains(col)) grid.Columns[col].HeaderText = header;
        }

        private void AlignRight(DataGridView grid, string col)
        {
            if (grid.Columns.Contains(col))
                grid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}