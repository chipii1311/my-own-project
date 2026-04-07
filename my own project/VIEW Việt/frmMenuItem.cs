using my_own_project.BLL;
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

namespace my_own_project.VIEW_Việt
{
    public partial class frmMenuItem : Form
    {
        public frmMenuItem()
        {
            InitializeComponent();
        }

        private void frmMenuItem_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsManager && !CurrentUser.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            LoadMenuItems();
            SetupDataGridView();
        }
        private void LoadMenuItems()
        {
            try
            {
                DataTable dtMenuItems = MenuItemBLL.GetAllMenuItems();
                dgvMenuItem.DataSource = dtMenuItems;
                lblStatus.Text = $"✅ Tải {dtMenuItems.Rows.Count} menu items";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thiết lập DataGridView
        /// </summary>
        private void SetupDataGridView()
        {
            dgvMenuItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvMenuItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenuItem.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Mở form thêm menu item mới
            // frmMenuItemEdit frm = new frmMenuItemEdit();
            // if (frm.ShowDialog() == DialogResult.OK)
            // {
            //     LoadMenuItems();
            // }
            MessageBox.Show("✨ Form thêm menu sẽ được cập nhật sớm!", "Thông báo");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMenuItem.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn menu item để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn chắc muốn xóa menu item này?", "Xác Nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int menuItemID = (int)dgvMenuItem.SelectedRows[0].Cells["MenuItemID"].Value;
                    MenuItemBLL.DeleteMenuItem(menuItemID);
                    LoadMenuItems();
                    MessageBox.Show("✅ Xóa menu item thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMenuItem.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn menu item để sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form sửa
            int menuItemID = (int)dgvMenuItem.SelectedRows[0].Cells["MenuItemID"].Value;
            // frmMenuItemEdit frm = new frmMenuItemEdit(menuItemID);
            // if (frm.ShowDialog() == DialogResult.OK)
            // {
            //     LoadMenuItems();
            // }
            MessageBox.Show("✨ Form sửa menu sẽ được cập nhật sớm!", "Thông báo");
        
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
            lblStatus.Text = "✅ Làm mới dữ liệu";
        }

       
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                DataTable dtMenuItems = MenuItemBLL.GetAllMenuItems();

                if (string.IsNullOrEmpty(searchText))
                {
                    dgvMenuItem.DataSource = dtMenuItems;
                }
                else
                {
                    var filteredData = dtMenuItems.AsEnumerable()
                        .Where(row => row["ItemName"].ToString().Contains(searchText) ||
                                      row["CategoryName"].ToString().Contains(searchText))
                        .CopyToDataTable();

                    dgvMenuItem.DataSource = filteredData;
                }

                lblStatus.Text = $"✅ Tìm {dgvMenuItem.Rows.Count} kết quả";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
    
