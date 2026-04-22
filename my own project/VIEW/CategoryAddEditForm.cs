using my_own_project.DAL;
using my_own_project.DTO;
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
    public partial class CategoryAddEditForm : Form
    {
        private int categoryID; // 0 = Add mode, > 0 = Edit mode
        private bool isAddMode;
        public CategoryAddEditForm(int id)
        {
            InitializeComponent();
            categoryID = id;
            isAddMode = (id == 0);
        }
        private void CategoryAddEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (isAddMode)
                {
                    // ADD MODE
                    lblTitle.Text = "🏷️ Thêm danh mục mới";
                    this.Text = "Thêm danh mục";
                    chkIsActive.Checked = true; // Default active
                    txtCategoryName.Focus();
                }
                else
                {
                    // EDIT MODE
                    lblTitle.Text = "✏️ Chỉnh sửa danh mục";
                    this.Text = "Chỉnh sửa danh mục";
                    LoadCategoryData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khởi tạo form: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // LOAD CATEGORY DATA (FOR EDIT MODE)
        // ============================================
        private void LoadCategoryData()
        {
            try
            {
                // TODO: Implement CategoryDAL.GetByID() if not exist
                // For now, assume we have the method
                DataTable dt = CategoryDAL.GetByID(categoryID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtCategoryName.Text = row["CategoryName"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(row["IsActive"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // ============================================
        // BUTTON SAVE CLICK
        // ============================================
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // VALIDATION
                if (!ValidateForm())
                {
                    return;
                }

                // GET VALUES
                string categoryName = txtCategoryName.Text.Trim();
                bool isActive = chkIsActive.Checked;

                if (isAddMode)
                {
                    // ADD NEW CATEGORY
                    CategoryDTO newCategory = new CategoryDTO
                    {
                        CategoryName = categoryName,
                        IsActive = isActive
                    };

                    int newID = CategoryDAL.Insert(newCategory);

                    if (newID > 0)
                    {
                        MessageBox.Show("✅ Thêm danh mục thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ Lỗi: Không thể lưu danh mục!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // UPDATE CATEGORY
                    CategoryDTO category = new CategoryDTO
                    {
                        CategoryID = categoryID,
                        CategoryName = categoryName,
                        IsActive = isActive
                    };

                    CategoryDAL.Update(category);

                    MessageBox.Show("✅ Cập nhật danh mục thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi lưu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // VALIDATE FORM
        // ============================================
        private bool ValidateForm()
        {
            // Check category name
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên danh mục!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return false;
            }

            if (txtCategoryName.Text.Length < 2)
            {
                MessageBox.Show("⚠️ Tên danh mục phải ít nhất 2 ký tự!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return false;
            }

            if (txtCategoryName.Text.Length > 100)
            {
                MessageBox.Show("⚠️ Tên danh mục không được vượt quá 100 ký tự!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ============================================
        // BUTTON CLOSE CLICK
        // ============================================
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
