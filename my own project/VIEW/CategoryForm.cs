using my_own_project.BLL;
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

namespace my_own_project.DesignForms
{
    public partial class CategoryForm : SampleView
    {
        private DataTable dtCategoriesCache;
        private int selectedCategoryID = 0;
        public CategoryForm()
        {
            InitializeComponent();
            StyleForm();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            //if (!CurrentUser.IsManager && !CurrentUser.IsAdmin)
            //{
            //    MessageBox.Show("❌ Bạn không có quyền truy cập!", "Lỗi",
            //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    this.Close();
            //    return;
            //}
                ((DataGridViewImageColumn)dgvCategory.Columns["dgvEdit"]).DefaultCellStyle.NullValue = null;

            // Gán tấm ảnh của bạn vào (Thay "TenHinhCuaBan" bằng tên tấm ảnh trong Resources)
            ((DataGridViewImageColumn)dgvCategory.Columns["dgvEdit"]).Image = Properties.Resources.icons8_edit_16;

            // Chỉnh cho tấm ảnh vừa vặn với ô (Không bị méo)
            ((DataGridViewImageColumn)dgvCategory.Columns["dgvEdit"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            LoadCategories();
            SetupEvents();
            
        }
        private void StyleForm()
        {
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
        }
        private void LoadCategories()
        {
            try
            {
                dtCategoriesCache = CategoryBLL.GetAllCategories();

                // Format DataGridView
                dgvCategory.DataSource = dtCategoriesCache;
                FormatDataGridView();

                // Cập nhật record count
                
                lblUpdateTime.Text = $"Cập nhật: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }
        private void FormatDataGridView()
        {
            dgvCategory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCategory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategory.ReadOnly = true;

            // Header style
            dgvCategory.ColumnHeadersDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(52, 152, 219);
            dgvCategory.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvCategory.ColumnHeadersDefaultCellStyle.Font =
                new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            // Row style
            dgvCategory.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvCategory.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgvCategory.AlternatingRowsDefaultCellStyle.BackColor =
                System.Drawing.Color.FromArgb(245, 248, 250);

            // Ẩn CategoryID
            if (dgvCategory.Columns.Contains("CategoryID"))
                dgvCategory.Columns["CategoryID"].Visible = false;

            // Thêm STT column nếu chưa có
            if (!dgvCategory.Columns.Contains("STT"))
            {
                DataGridViewTextBoxColumn colSTT = new DataGridViewTextBoxColumn();
                colSTT.Name = "STT";
                colSTT.HeaderText = "STT";
                colSTT.Width = 50;
                dgvCategory.Columns.Insert(0, colSTT);
            }

            // Cập nhật STT
            for (int i = 0; i < dgvCategory.Rows.Count; i++)
            {
                dgvCategory.Rows[i].Cells["STT"].Value = i + 1;
            }
        }
        private void SetupEvents()
        {
            btnAdd.Click += btnAdd_Click;
           
                    
            //btnRefresh.Click += BtnRefresh_Click;  // ← Giữ lại cho trường hợp user click thủ công
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvCategory.SelectionChanged += DgvCategory_SelectionChanged; ;
        }

        private void DgvCategory_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategory.SelectedRows.Count > 0)
            {
                selectedCategoryID = (int)dgvCategory.SelectedRows[0].Cells["CategoryID"].Value;
            }
        }

        

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string categoryName = PromptForCategoryName("Thêm Danh Mục", "");

            if (!string.IsNullOrEmpty(categoryName))
            {
                try
                {
                    CategoryDTO category = new CategoryDTO
                    {
                        CategoryName = categoryName
                    };

                    int categoryID = CategoryBLL.AddCategory(category);

                    // ✅ TỰ ĐỘNG LOAD LẠI
                    LoadCategories();
                    

                    MessageBox.Show("✅ Thêm danh mục thành công!", "Thành Công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi");
                   
                }
            }
        }

        private string PromptForCategoryName(string title, string defaultValue)
        {
            Form prompt = new Form();
            prompt.Text = title;
            prompt.Width = 400;
            prompt.Height = 150;
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            prompt.BackColor = System.Drawing.Color.White;

            Label label = new Label();
            label.Left = 20;
            label.Top = 20;
            label.Text = "Tên danh mục:";
            label.Width = 350;
            label.Height = 25;
            label.Font = new System.Drawing.Font("Segoe UI", 10);

            TextBox textBox = new TextBox();
            textBox.Left = 20;
            textBox.Top = 50;
            textBox.Width = 350;
            textBox.Height = 30;
            textBox.Text = defaultValue;
            textBox.Font = new System.Drawing.Font("Segoe UI", 10);
            textBox.SelectAll();  // Select tất cả text để dễ replace

            Button okButton = new Button();
            okButton.Text = "✅ OK";
            okButton.Left = 220;
            okButton.Top = 90;
            okButton.Width = 70;
            okButton.Height = 30;
            okButton.DialogResult = DialogResult.OK;
            okButton.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            okButton.ForeColor = System.Drawing.Color.White;
            okButton.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            Button cancelButton = new Button();
            cancelButton.Text = "❌ Hủy";
            cancelButton.Left = 300;
            cancelButton.Top = 90;
            cancelButton.Width = 70;
            cancelButton.Height = 30;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            cancelButton.ForeColor = System.Drawing.Color.White;
            cancelButton.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            prompt.Controls.Add(label);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(okButton);
            prompt.Controls.Add(cancelButton);
            prompt.AcceptButton = okButton;
            prompt.CancelButton = cancelButton;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim().ToLower();

                if (dtCategoriesCache == null)
                    return;

                if (string.IsNullOrEmpty(searchText))
                {
                    dgvCategory.DataSource = dtCategoriesCache;
                }
                else
                {
                    var filteredData = dtCategoriesCache.AsEnumerable()
                        .Where(row => row["CategoryName"].ToString().ToLower().Contains(searchText))
                        .CopyToDataTable();

                    dgvCategory.DataSource = filteredData;
                }

                FormatDataGridView();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi tìm kiếm: {ex.Message}");
            }
        }

        private void CategoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
