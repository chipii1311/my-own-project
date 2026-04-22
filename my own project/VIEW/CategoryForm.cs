using my_own_project.BLL;
using my_own_project.DAL;
using my_own_project.DTO;
using my_own_project.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using my_own_project.VIEW;

namespace my_own_project.DesignForms
{
    public partial class CategoryForm : Form
    {
        
        private DataTable currentData;
        public CategoryForm()
        {
            InitializeComponent();
            
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        { 
            LoadData();

        }
        public void LoadData()
        {
            try
            {
                currentData = CategoryDAL.GetAll();

                if (currentData != null && currentData.Rows.Count > 0)
                {
                    dgvCategory.DataSource = currentData;
                    SetupColumns();
                    UpdateRecordCount();
                    ShowEmptyState(false);
                }
                else
                {
                    dgvCategory.DataSource = null;
                    ShowEmptyState(true);
                    lblRecordCount.Text = "📊 Tổng cộng: 0 danh mục";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải danh sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // SETUP COLUMNS
        // ============================================
        private void SetupColumns()
        {
            try
            {
                // Hide ID
                if (dgvCategory.Columns.Contains("CategoryID"))
                    dgvCategory.Columns["CategoryID"].Visible = false;

                // Configure columns
                ConfigureColumn("CategoryName", "Tên danh mục", 300, DataGridViewContentAlignment.MiddleLeft);
                ConfigureColumn("IsActive", "Trạng thái", 150, DataGridViewContentAlignment.MiddleCenter);

                // Format boolean column
                if (dgvCategory.Columns.Contains("IsActive"))
                {
                    dgvCategory.Columns["IsActive"].DefaultCellStyle.Format = "Yes/No";
                }

                // Add action columns
                AddActionColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi setup columns: " + ex.Message);
            }
        }

        // ============================================
        // CONFIGURE COLUMN
        // ============================================
        private void ConfigureColumn(string columnName, string headerText, int width,
            DataGridViewContentAlignment alignment)
        {
            if (dgvCategory.Columns.Contains(columnName))
            {
                dgvCategory.Columns[columnName].HeaderText = headerText;
                dgvCategory.Columns[columnName].Width = width;
                dgvCategory.Columns[columnName].DefaultCellStyle.Alignment = alignment;
            }
        }

        // ============================================
        // ADD ACTION COLUMNS
        // ============================================
        private void AddActionColumns()
        {
            // Remove old columns
            if (dgvCategory.Columns.Contains("btnEdit"))
                dgvCategory.Columns.Remove("btnEdit");
            if (dgvCategory.Columns.Contains("btnDelete"))
                dgvCategory.Columns.Remove("btnDelete");

            // Edit button
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "Thao tác";
            btnEdit.Text = "✏️ Sửa";
            btnEdit.UseColumnTextForButtonValue = true;
            btnEdit.Width = 80;
            btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            btnEdit.DefaultCellStyle.ForeColor = Color.White;
            btnEdit.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCategory.Columns.Add(btnEdit);

            // Delete button
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "🗑️ Xóa";
            btnDelete.UseColumnTextForButtonValue = true;
            btnDelete.Width = 80;
            btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(244, 67, 54);
            btnDelete.DefaultCellStyle.ForeColor = Color.White;
            btnDelete.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCategory.Columns.Add(btnDelete);
        }

        // ============================================
        // SEARCH TEXT CHANGED
        // ============================================
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentData == null)
            {
                ShowEmptyState(true);
                return;
            }

            string searchText = txtSearch.Text.ToLower();
            DataView dv = new DataView(currentData);
            dv.RowFilter = $"CategoryName LIKE '%{searchText}%'";
            dgvCategory.DataSource = dv;

            ShowEmptyState(dv.Count == 0);
            lblRecordCount.Text = $"📊 Tổng cộng: {dv.Count} danh mục";
        }

        // ============================================
        // SHOW/HIDE EMPTY STATE
        // ============================================
        private void ShowEmptyState(bool isEmpty)
        {
            if (pnlEmpty != null)
                pnlEmpty.Visible = isEmpty;
            if (dgvCategory != null)
                dgvCategory.Visible = !isEmpty;
        }

        // ============================================
        // BUTTON ADD CLICK
        // ============================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryAddEditForm frm = new CategoryAddEditForm(0); // 0 = Add mode
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        // ============================================
        // DATAGRIDVIEW CELL CLICK
        // ============================================
        private void DgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvCategory.Columns[e.ColumnIndex].Name;
            int categoryID = Convert.ToInt32(dgvCategory.Rows[e.RowIndex].Cells["CategoryID"].Value);

            if (columnName == "btnEdit")
            {
                OpenEditForm(categoryID);
            }
            else if (columnName == "btnDelete")
            {
                DeleteCategory(categoryID);
            }
        }

        // ============================================
        // OPEN EDIT FORM
        // ============================================
        private void OpenEditForm(int categoryID)
        {
            try
            {
                CategoryAddEditForm frm = new CategoryAddEditForm(categoryID); // categoryID > 0 = Edit mode
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // DELETE CATEGORY
        // ============================================
        private void DeleteCategory(int categoryID)
        {
            try
            {
                if (MessageBox.Show("⚠️ Bạn có chắc chắn muốn xóa danh mục này?\n\nHành động này không thể hoàn tác!",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CategoryDAL.Delete(categoryID);
                    MessageBox.Show("✓ Xóa danh mục thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // UPDATE RECORD COUNT
        // ============================================
        private void UpdateRecordCount()
        {
            if (currentData != null)
            {
                lblRecordCount.Text = $"📊 Tổng cộng: {currentData.Rows.Count} danh mục";
            }
            else
            {
                lblRecordCount.Text = "📊 Tổng cộng: 0 danh mục";
            }
        }


    }
}
