using Guna.UI2.WinForms;
using my_own_project.DAL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class SettingForm : Form
    {
        public static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        public static readonly Color C_WHITE = Color.White;
        public static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        public static readonly Color C_RED = Color.FromArgb(220, 38, 38);
        public SettingForm()
        {
            InitializeComponent();
            BuildUI();
            LoadCategoryData();
        }

        // ========================================================
        // 1. LOAD DATA
        // ========================================================
        private void LoadCategoryData()
        {
            try
            {
                DataTable dt = DataHelper.ExecuteQuery("SELECT CategoryID, CategoryName FROM Category WHERE IsActive = 1");
                dgvCategories.DataSource = dt;
                if (dgvCategories.Columns.Contains("CategoryID"))
                    dgvCategories.Columns["CategoryID"].Visible = false;
            }
            catch (Exception ex) { ShowError("Lỗi tải danh mục: " + ex.Message); }
        }

        // ========================================================
        // 2. LOGIC CRUD
        // ========================================================
        private void BtnSaveCat_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();
            if (string.IsNullOrEmpty(name)) { ShowWarn("Vui lòng nhập tên danh mục!"); return; }

            try
            {
                if (string.IsNullOrEmpty(txtCategoryID.Text))
                {
                    DataHelper.ExecuteNonQuery($"INSERT INTO Category (CategoryName, IsActive) VALUES (N'{name}', 1)");
                    ShowInfo("✔ Thêm mới thành công!");
                }
                else
                {
                    DataHelper.ExecuteNonQuery($"UPDATE Category SET CategoryName = N'{name}' WHERE CategoryID = {txtCategoryID.Text}");
                    ShowInfo("✔ Cập nhật thành công!");
                }
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi lưu: " + ex.Message); }
        }

        private void BtnDeleteCat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                DataHelper.ExecuteNonQuery($"UPDATE Category SET IsActive = 0 WHERE CategoryID = {txtCategoryID.Text}");
                ShowInfo("✔ Đã xóa thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi xóa: " + ex.Message); }
        }

        // ========================================================
        // 3. UI HELPERS
        // ========================================================
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCategories.Rows[e.RowIndex];
                txtCategoryID.Text = row.Cells["CategoryID"].Value.ToString();
                txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();

                btnSaveCat.Enabled = true;
                btnDeleteCat.Enabled = true;
                btnSaveCat.FillColor = C_PURPLE;
                btnDeleteCat.FillColor = C_RED;
            }
        }

        private void ClearCatForm()
        {
            txtCategoryID.Text = "";
            txtCategoryName.Clear();
            btnSaveCat.Enabled = false;
            btnDeleteCat.Enabled = false;
            btnSaveCat.FillColor = Color.FromArgb(210, 210, 218);
            btnDeleteCat.FillColor = Color.FromArgb(210, 210, 218);
        }

        private void ShowInfo(string msg) => MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowWarn(string msg) => MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void ShowError(string msg) => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}