using Guna.UI2.WinForms;
using my_own_project.DAL;
using System;
using System.Data;
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

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            // Tải dữ liệu ban đầu
            LoadMenuItems();
            LoadIngredients();
            LoadRecipeBySelectedMenu();
        }

        // ===================== DATA LOADING =====================
        private void LoadMenuItems()
        {
            try
            {
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllLite", null);
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
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Ingredient_GetAll", null);

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
                if (dgvRecipe != null) dgvRecipe.DataSource = null;
                UpdateSelectedFoodText(0);
                return;
            }

            try
            {
                string query = @"
                    SELECT
                        r.RecipeID,
                        r.MenuItemID,
                        m.ItemName,
                        r.IngredientID,
                        i.IngredientName,
                        i.Unit,
                        r.Quantity,
                        ISNULL(i.StockQuantity, 0) AS StockQuantity,
                        ISNULL(i.MinStock, 0)      AS MinStock
                    FROM dbo.Recipe r
                    INNER JOIN dbo.MenuItem m ON r.MenuItemID = m.MenuItemID
                    INNER JOIN dbo.Ingredient i ON r.IngredientID = i.IngredientID
                    WHERE r.MenuItemID = " + menuItemID + @"
                    ORDER BY i.IngredientName;";

                DataTable dt = DataHelper.ExecuteQuery(query);
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
                    DialogResult confirm = MessageBox.Show(
                        "Nguyên liệu này đã có trong công thức. Bạn muốn cập nhật định lượng không?",
                        "Nguyên liệu đã tồn tại",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm != DialogResult.Yes) return;

                    string updateSql = @"
                        UPDATE dbo.Recipe
                        SET Quantity = " + SqlDecimal(quantity) + @"
                        WHERE MenuItemID = " + menuItemID + @"
                          AND IngredientID = " + ingredientID + ";";
                    DataHelper.ExecuteNonQuery(updateSql);
                }
                else
                {
                    string insertSql = @"
                        INSERT INTO dbo.Recipe (MenuItemID, IngredientID, Quantity)
                        VALUES (" + menuItemID + ", " + ingredientID + ", " + SqlDecimal(quantity) + ");";
                    DataHelper.ExecuteNonQuery(insertSql);
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
            try
            {
                if (selectedRecipeID <= 0) throw new Exception("Vui lòng chọn dòng công thức cần cập nhật.");

                int ingredientID = GetSelectedIngredientID();
                decimal quantity = ReadQuantity();

                if (ingredientID <= 0) throw new Exception("Vui lòng chọn nguyên liệu.");
                if (quantity <= 0) throw new Exception("Định lượng phải lớn hơn 0.");

                string query = @"
                    UPDATE dbo.Recipe
                    SET IngredientID = " + ingredientID + @",
                        Quantity     = " + SqlDecimal(quantity) + @"
                    WHERE RecipeID = " + selectedRecipeID + ";";

                DataHelper.ExecuteNonQuery(query);

                MessageBox.Show("Cập nhật công thức thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            try
            {
                if (selectedRecipeID <= 0) throw new Exception("Vui lòng chọn dòng công thức cần xóa.");

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc muốn xóa nguyên liệu này khỏi công thức?",
                    "Xóa nguyên liệu",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                string query = "DELETE FROM dbo.Recipe WHERE RecipeID = " + selectedRecipeID + ";";
                DataHelper.ExecuteNonQuery(query);

                MessageBox.Show("Xóa khỏi công thức thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string query = @"
                SELECT RecipeID FROM dbo.Recipe
                WHERE MenuItemID = " + menuItemID + " AND IngredientID = " + ingredientID + ";";
            DataTable dt = DataHelper.ExecuteQuery(query);
            return dt != null && dt.Rows.Count > 0;
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
            if (cboMenuItem == null || cboMenuItem.SelectedValue == null) return 0;
            if (cboMenuItem.SelectedValue is DataRowView) return 0;
            return Convert.ToInt32(cboMenuItem.SelectedValue);
        }

        private int GetSelectedIngredientID()
        {
            if (cboIngredient == null || cboIngredient.SelectedValue == null) return 0;
            if (cboIngredient.SelectedValue is DataRowView) return 0;
            return Convert.ToInt32(cboIngredient.SelectedValue);
        }

        private decimal ReadQuantity()
        {
            string text = txtQuantity.Text.Trim().Replace(",", ".");
            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal qty))
                throw new Exception("Định lượng không hợp lệ. Ví dụ nhập: 0.25 hoặc 1.");
            return qty;
        }

        private string SqlDecimal(decimal value) => value.ToString(CultureInfo.InvariantCulture);

        private void ClearInput(bool clearIngredient)
        {
            selectedRecipeID = 0;
            if (txtQuantity != null) txtQuantity.Clear();
            if (clearIngredient && cboIngredient != null && cboIngredient.Items.Count > 0)
                cboIngredient.SelectedIndex = 0;
        }

        private void UpdateSelectedFoodText(int recipeCount)
        {
            string foodName = cboMenuItem == null ? "" : cboMenuItem.Text;

            if (lblSelectedFood != null)
                lblSelectedFood.Text = string.IsNullOrWhiteSpace(foodName)
                    ? "Chưa chọn món"
                    : "Công thức: " + foodName;

            if (lblRecipeCount != null)
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