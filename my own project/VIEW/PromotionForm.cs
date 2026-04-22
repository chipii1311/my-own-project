using my_own_project.DAL;
using my_own_project.DesignForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class PromotionForm : Form
    {
        private DataTable currentData;
        public PromotionForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            PromotionAddForm frm = new PromotionAddForm();

            // 2. Hiển thị Form lên dưới dạng Pop-up (Cửa sổ nổi)
            frm.ShowDialog();
            LoadData();
        }

        private void PromotionForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                currentData = PromotionDAL.GetAll();

                if (currentData != null && currentData.Rows.Count > 0)
                {
                    // ❌ SAI: Không format ngày thành String rồi gán lại
                    // ✅ ĐÚNG: Chỉ format khi hiển thị trong DataGridView, không sửa dữ liệu gốc

                    dgvPromotions.DataSource = currentData;
                    SetupColumns();
                    UpdateRecordCount();
                    ShowEmptyState(false);
                }
                else
                {
                    dgvPromotions.DataSource = null;
                    ShowEmptyState(true);
                    lblRecordCount.Text = "📊 Tổng cộng: 0 khuyến mãi";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải danh sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // SETUP DATAGRIDVIEW COLUMNS
        // ============================================
        private void SetupColumns()
        {
            try
            {
                // Hide ID column
                if (dgvPromotions.Columns.Contains("PromotionID"))
                    dgvPromotions.Columns["PromotionID"].Visible = false;

                // Configure columns
                ConfigureColumn("PromotionName", "Tên chương trình", 200, DataGridViewContentAlignment.MiddleLeft);
                ConfigureColumn("DiscountPercent", "Mức giảm (%)", 100, DataGridViewContentAlignment.MiddleCenter);
                ConfigureColumn("StartDate", "Ngày bắt đầu", 120, DataGridViewContentAlignment.MiddleCenter);
                ConfigureColumn("EndDate", "Ngày kết thúc", 120, DataGridViewContentAlignment.MiddleCenter);
                ConfigureColumn("Status", "Trạng thái", 100, DataGridViewContentAlignment.MiddleCenter);
                ConfigureColumn("ApplyTypeName", "Phạm vi áp dụng", 150, DataGridViewContentAlignment.MiddleCenter);

                // Format Date columns for display only
                if (dgvPromotions.Columns.Contains("StartDate"))
                {
                    dgvPromotions.Columns["StartDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvPromotions.Columns.Contains("EndDate"))
                {
                    dgvPromotions.Columns["EndDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
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
        // CONFIGURE SINGLE COLUMN
        // ============================================
        private void ConfigureColumn(string columnName, string headerText, int width,
            DataGridViewContentAlignment alignment)
        {
            if (dgvPromotions.Columns.Contains(columnName))
            {
                dgvPromotions.Columns[columnName].HeaderText = headerText;
                dgvPromotions.Columns[columnName].Width = width;
                dgvPromotions.Columns[columnName].DefaultCellStyle.Alignment = alignment;
            }
        }

        // ============================================
        // ADD ACTION BUTTON COLUMNS
        // ============================================
        private void AddActionColumns()
        {
            // Remove old columns if exist
            if (dgvPromotions.Columns.Contains("btnEdit"))
                dgvPromotions.Columns.Remove("btnEdit");
            if (dgvPromotions.Columns.Contains("btnDelete"))
                dgvPromotions.Columns.Remove("btnDelete");

            // Add Edit button
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "Thao tác";
            btnEdit.Text = "✏️ Sửa";
            btnEdit.UseColumnTextForButtonValue = true;
            btnEdit.Width = 80;
            btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            btnEdit.DefaultCellStyle.ForeColor = Color.White;
            btnEdit.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPromotions.Columns.Add(btnEdit);

            // Add Delete button
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "🗑️ Xóa";
            btnDelete.UseColumnTextForButtonValue = true;
            btnDelete.Width = 80;
            btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(244, 67, 54);
            btnDelete.DefaultCellStyle.ForeColor = Color.White;
            btnDelete.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPromotions.Columns.Add(btnDelete);
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

            // Filter by PromotionName or Status
            dv.RowFilter = $"PromotionName LIKE '%{searchText}%' OR Status LIKE '%{searchText}%'";
            dgvPromotions.DataSource = dv;

            // Show/Hide empty state
            ShowEmptyState(dv.Count == 0);

            // Update record count
            lblRecordCount.Text = $"📊 Tổng cộng: {dv.Count} khuyến mãi";
        }

        // ============================================
        // SHOW/HIDE EMPTY STATE
        // ============================================
        private void ShowEmptyState(bool isEmpty)
        {
            if (pnlEmpty != null)
            {
                pnlEmpty.Visible = isEmpty;
            }
            if (dgvPromotions != null)
            {
                dgvPromotions.Visible = !isEmpty;
            }
        }

        // ============================================
        // BUTTON ADD CLICK
        // ============================================
        

        // ============================================
        // DATAGRIDVIEW CELL CLICK (EDIT/DELETE)
        // ============================================
        private void DgvPromotions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dgvPromotions.Columns[e.ColumnIndex].Name;

            if (columnName == "btnEdit")
            {
                int promotionID = Convert.ToInt32(dgvPromotions.Rows[e.RowIndex].Cells["PromotionID"].Value);
                OpenEditForm(promotionID);
            }
            else if (columnName == "btnDelete")
            {
                int promotionID = Convert.ToInt32(dgvPromotions.Rows[e.RowIndex].Cells["PromotionID"].Value);
                DeletePromotion(promotionID);
            }
        }

        // ============================================
        // DATAGRIDVIEW CELL DOUBLE CLICK (VIEW DETAIL)
        // ============================================
        private void DgvPromotions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int promotionID = Convert.ToInt32(dgvPromotions.Rows[e.RowIndex].Cells["PromotionID"].Value);
            OpenViewForm(promotionID);
        }

        // ============================================
        // OPEN EDIT FORM
        // ============================================
        private void OpenEditForm(int promotionID)
        {
            try
            {
                PromotionAddForm frm = new PromotionAddForm(promotionID); // promotionID > 0 = Edit mode
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
        // OPEN VIEW FORM (CHI TIẾT)
        // ============================================
        private void OpenViewForm(int promotionID)
        {
            try
            {
                // TODO: Sẽ implement PromotionDetailForm sau
                MessageBox.Show("📋 Chức năng xem chi tiết sẽ sớm được cập nhật!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // DELETE PROMOTION
        // ============================================
        private void DeletePromotion(int promotionID)
        {
            try
            {
                if (MessageBox.Show("⚠️ Bạn có chắc chắn muốn xóa khuyến mãi này?\n\nHành động này không thể hoàn tác!",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PromotionDAL.Delete(promotionID);
                    MessageBox.Show("✓ Xóa khuyến mãi thành công!", "Thành công",
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
        // UPDATE RECORD COUNT IN FOOTER
        // ============================================
        private void UpdateRecordCount()
        {
            if (currentData != null)
            {
                lblRecordCount.Text = $"📊 Tổng cộng: {currentData.Rows.Count} khuyến mãi";
            }
            else
            {
                lblRecordCount.Text = "📊 Tổng cộng: 0 khuyến mãi";
            }
        }
    }
}
