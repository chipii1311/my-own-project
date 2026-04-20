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
    public partial class PromotionForm : SampleView
    {
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
                DataTable dt = PromotionDAL.GetAll();
                dgvPromotions.DataSource = dt;

                // Định dạng lại các cột cho đẹp và Việt hóa
                if (dgvPromotions.Columns.Contains("PromotionID"))
                    dgvPromotions.Columns["PromotionID"].Visible = false;

                if (dgvPromotions.Columns.Contains("PromotionName"))
                    dgvPromotions.Columns["PromotionName"].HeaderText = "Tên chương trình";

                if (dgvPromotions.Columns.Contains("DiscountPercent"))
                    dgvPromotions.Columns["DiscountPercent"].HeaderText = "Mức giảm (%)";

                if (dgvPromotions.Columns.Contains("StartDate"))
                    dgvPromotions.Columns["StartDate"].HeaderText = "Ngày bắt đầu";

                if (dgvPromotions.Columns.Contains("EndDate"))
                    dgvPromotions.Columns["EndDate"].HeaderText = "Ngày kết thúc";

                if (dgvPromotions.Columns.Contains("Status"))
                    dgvPromotions.Columns["Status"].HeaderText = "Trạng thái";

                if (dgvPromotions.Columns.Contains("ApplyTypeName"))
                    dgvPromotions.Columns["ApplyTypeName"].HeaderText = "Phạm vi áp dụng";

                dgvPromotions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvPromotions.RowHeadersVisible = false;
                dgvPromotions.AllowUserToAddRows = false;
                dgvPromotions.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }
    }
}
