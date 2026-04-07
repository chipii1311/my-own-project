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
        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            
    ((DataGridViewImageColumn)dataGridView1.Columns["dgvEdit"]).DefaultCellStyle.NullValue = null;

            // Gán tấm ảnh của bạn vào (Thay "TenHinhCuaBan" bằng tên tấm ảnh trong Resources)
            ((DataGridViewImageColumn)dataGridView1.Columns["dgvEdit"]).Image = Properties.Resources.icons8_edit_16;

            // Chỉnh cho tấm ảnh vừa vặn với ô (Không bị méo)
            ((DataGridViewImageColumn)dataGridView1.Columns["dgvEdit"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            CategoryAddForm f = new CategoryAddForm();

            // Dòng 2: Lệnh hiển thị form lên màn hình
            f.ShowDialog();
        }
    }
}
