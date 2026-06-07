using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Data;
using System.Windows.Forms;

// [ĐÃ SỬA]: Bỏ using my_own_project.DAL — Form không được gọi trực tiếp xuống DAL

namespace my_own_project.VIEW
{
    public partial class SettingForm : Form
    {
        private int _selectedTableID = -1;
        private int _selectedCategoryID = -1;

        public SettingForm()
        {
            InitializeComponent();
            this.Load += (s, e) => { LoadTableData(); LoadCategoryData(); };
        }

        // ===================== LOAD DATA =====================
        private void LoadTableData()
        {
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL thay vì DataHelper.ExecuteQuery(rawSQL)
                DataTable dt = DiningTableBLL.GetAllTables();

                dgvTables.DataSource = dt;

                if (dgvTables.Columns.Contains("TableID"))
                    dgvTables.Columns["TableID"].Visible = false;

                if (lblTableCount != null)
                    lblTableCount.Text = $"{dt.Rows.Count} bàn ăn";
            }
            catch (Exception ex)
            {
                ShowError("Lỗi tải danh sách bàn: " + ex.Message);
            }
        }

        private void LoadCategoryData()
        {
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL thay vì DataHelper.ExecuteQuery(rawSQL)
                DataTable dt = CategoryBLL.GetAllCategories();

                dgvCategories.DataSource = dt;

                if (dgvCategories.Columns.Contains("CategoryID"))
                    dgvCategories.Columns["CategoryID"].Visible = false;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi tải danh mục: " + ex.Message);
            }
        }

        // ===================== CELL FORMATTING =====================
        private void DgvTables_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvTables.Columns[e.ColumnIndex].Name != "Status") return;

            string v = e.Value?.ToString() ?? "";
            e.CellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);

            switch (v)
            {
                case "Trống":
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(22, 163, 74);
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(220, 252, 231);
                    break;
                case "Có khách":
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(109, 60, 240);
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(237, 233, 254);
                    break;
                case "Đặt trước":
                    e.CellStyle.ForeColor = System.Drawing.Color.FromArgb(217, 119, 6);
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(254, 243, 199);
                    break;
            }
        }

        // ===================== EVENTS — TABLES =====================
        private void DgvTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvTables.Rows[e.RowIndex];
            _selectedTableID = Convert.ToInt32(row.Cells["TableID"].Value);
            txtTableNumber.Text = row.Cells["TableNumber"].Value?.ToString() ?? "";
            txtTableCapacity.Text = row.Cells["Capacity"].Value?.ToString() ?? "";
            cboTableStatus.Text = row.Cells["Status"].Value?.ToString() ?? "Trống";

            lblTableHint.Text = $"✏️ Đang chỉnh sửa bàn số {txtTableNumber.Text}";
            lblTableHint.ForeColor = System.Drawing.Color.FromArgb(88, 28, 230);

            btnSaveTable.Enabled = true;
            btnDeleteTable.Enabled = true;
        }

        private void BtnAddTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableNumber.Text))
            {
                ShowWarn("Vui lòng nhập số bàn!");
                return;
            }

            if (!int.TryParse(txtTableNumber.Text, out int num))
            {
                ShowWarn("Số bàn chỉ được nhập số (VD: 1, 2, 3...)");
                return;
            }

            int cap = 4;
            if (!string.IsNullOrWhiteSpace(txtTableCapacity.Text))
                int.TryParse(txtTableCapacity.Text, out cap);

            try
            {
                // [ĐÃ SỬA]: Tạo DTO và gọi BLL thay vì INSERT SQL thuần
                var table = new DiningTableDTO
                {
                    TableNumber = num,
                    Capacity = cap,
                    Status = cboTableStatus.Text,
                    Notes = ""
                };

                DiningTableBLL.AddTable(table);
                ShowInfo("✔️ Thêm bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex)
            {
                ShowError("Lỗi thêm bàn: " + ex.Message);
            }
        }

        private void BtnSaveTable_Click(object sender, EventArgs e)
        {
            if (_selectedTableID == -1)
            {
                ShowWarn("Vui lòng chọn bàn cần sửa!");
                return;
            }

            if (!int.TryParse(txtTableNumber.Text, out int num))
            {
                ShowWarn("Số bàn chỉ được nhập số!");
                return;
            }

            int cap = 4;
            if (!string.IsNullOrWhiteSpace(txtTableCapacity.Text))
                int.TryParse(txtTableCapacity.Text, out cap);

            try
            {
                // [ĐÃ SỬA]: Tạo DTO và gọi BLL thay vì UPDATE SQL thuần
                var table = new DiningTableDTO
                {
                    TableID = _selectedTableID,
                    TableNumber = num,
                    Capacity = cap,
                    Status = cboTableStatus.Text,
                    Notes = ""
                };

                DiningTableBLL.UpdateTable(table);
                ShowInfo("✔️ Cập nhật bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex)
            {
                ShowError("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            if (_selectedTableID == -1)
            {
                ShowWarn("Vui lòng chọn bàn cần xóa!");
                return;
            }

            if (MessageBox.Show(
                    $"Xóa bàn số {txtTableNumber.Text}? Hành động không thể hoàn tác.",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                // [ĐÃ SỬA]: Gọi BLL thay vì DataHelper.ExecuteNonQuery("DELETE FROM ...")
                DiningTableBLL.DeleteTable(_selectedTableID);
                ShowInfo("✔️ Xóa bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex)
            {
                ShowError("Lỗi xóa bàn: " + ex.Message);
            }
        }

        private void ClearTableForm()
        {
            _selectedTableID = -1;
            txtTableNumber.Clear();
            txtTableCapacity.Clear();
            cboTableStatus.SelectedIndex = 0;

            lblTableHint.Text = "✦️ Nhấp vào bàn ở danh sách để chỉnh sửa";
            lblTableHint.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);

            btnSaveTable.Enabled = false;
            btnDeleteTable.Enabled = false;
            dgvTables.ClearSelection();
        }

        // ===================== EVENTS — CATEGORIES =====================
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvCategories.Rows[e.RowIndex];
            _selectedCategoryID = Convert.ToInt32(row.Cells["CategoryID"].Value);
            txtCategoryName.Text = row.Cells["CategoryName"].Value?.ToString() ?? "";

            lblCatHint.Text = $"✏️ Đang chỉnh sửa: {txtCategoryName.Text}";
            lblCatHint.ForeColor = System.Drawing.Color.FromArgb(88, 28, 230);

            btnSaveCat.Enabled = true;
            btnDeleteCat.Enabled = true;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                ShowWarn("Vui lòng nhập tên danh mục!");
                return;
            }

            try
            {
                // [ĐÃ SỬA]: Tạo DTO và gọi BLL thay vì INSERT SQL thuần
                var category = new CategoryDTO
                {
                    CategoryName = txtCategoryName.Text.Trim(),
                    IsActive = true
                };

                CategoryBLL.AddCategory(category);
                ShowInfo("✔️ Thêm danh mục thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex)
            {
                ShowError("Lỗi thêm danh mục: " + ex.Message);
            }
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryID == -1)
            {
                ShowWarn("Vui lòng chọn danh mục cần sửa!");
                return;
            }

            try
            {
                // [ĐÃ SỬA]: Tạo DTO và gọi BLL thay vì UPDATE SQL thuần
                var category = new CategoryDTO
                {
                    CategoryID = _selectedCategoryID,
                    CategoryName = txtCategoryName.Text.Trim(),
                    IsActive = true
                };

                CategoryBLL.UpdateCategory(category);
                ShowInfo("✔️ Cập nhật thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex)
            {
                ShowError("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryID == -1)
            {
                ShowWarn("Vui lòng chọn danh mục cần xóa!");
                return;
            }

            if (MessageBox.Show(
                    $"Ẩn danh mục \"{txtCategoryName.Text}\"?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                // [ĐÃ SỬA]: Gọi BLL thay vì UPDATE Category SET IsActive=0 SQL thuần
                CategoryBLL.DeleteCategory(_selectedCategoryID);
                ShowInfo("✔️ Xóa danh mục thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex)
            {
                ShowError("Lỗi xóa danh mục: " + ex.Message);
            }
        }

        private void ClearCatForm()
        {
            _selectedCategoryID = -1;
            txtCategoryName.Clear();

            lblCatHint.Text = "✦️ Nhấp vào danh mục ở danh sách để chỉnh sửa";
            lblCatHint.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);

            btnSaveCat.Enabled = false;
            btnDeleteCat.Enabled = false;
            dgvCategories.ClearSelection();
        }

        // ===================== HELPER FUNCTIONS =====================
        private void ShowInfo(string msg)
            => MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void ShowWarn(string msg)
            => MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void ShowError(string msg)
            => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}