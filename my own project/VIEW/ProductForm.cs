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
    public partial class ProductForm : SampleView
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            ProductAddForm f = new ProductAddForm();

            // Dòng 2: Lệnh hiển thị form lên màn hình
            f.ShowDialog();
        }
    }
}
