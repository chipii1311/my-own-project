using Guna.UI2.WinForms;
using my_own_project.BLL;
// [ĐÃ XÓA]: // using my_own_project.DAL; đã bị comment nhưng vẫn còn gọi trực tiếp ở BtnDelete
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewPromotionForm : Form
    {
        private int currentSelectedID = -1;

        public NewPromotionForm()
        {
            InitializeComponent();
            BuildUI();
            this.Load += NewPromotionForm_Load;
        }

        private void NewPromotionForm_Load(object sender, EventArgs e) => LoadPromotions();

        private void LoadPromotions(string keyword = "", string status = "Tất cả")
        {
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL thay vì DataHelper.ExecuteQuery(rawSQL)
                DataTable dt = PromotionBLL.GetAllPromotionsFiltered(keyword, status);
                dgvPromotions.DataSource = dt;

                if (dgvPromotions.Columns.Contains("Mã KM"))
                    dgvPromotions.Columns["Mã KM"].Visible = false;
                if (dgvPromotions.Columns.Contains("Ngày BĐ"))
                    dgvPromotions.Columns["Ngày BĐ"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgvPromotions.Columns.Contains("Ngày KT"))
                    dgvPromotions.Columns["Ngày KT"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void TxtSearch_TextChanged(object sender, EventArgs e) => LoadPromotions(txtSearch.Text, cboFilterStatus.Text);
        public void CboFilterStatus_SelectedIndexChanged(object sender, EventArgs e) => LoadPromotions(txtSearch.Text, cboFilterStatus.Text);

        public void BtnAdd_Click(object sender, EventArgs e)
        {
            NewPromotionAddForm frm = new NewPromotionAddForm(-1);
            if (frm.ShowDialog() == DialogResult.OK)
                LoadPromotions(txtSearch.Text, cboFilterStatus.Text);
        }

        public void BtnEdit_Click(object sender, EventArgs e)
        {
            if (currentSelectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NewPromotionAddForm frm = new NewPromotionAddForm(currentSelectedID);
            if (frm.ShowDialog() == DialogResult.OK)
                LoadPromotions(txtSearch.Text, cboFilterStatus.Text);
        }

        public void BtnDelete_Click(object sender, EventArgs e)
        {
            if (currentSelectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn một chương trình khuyến mãi để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khuyến mãi này?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // [ĐÃ SỬA]: Gọi qua BLL thay vì DataHelper.ExecuteNonQuery("DELETE FROM ...")
                    PromotionBLL.DeletePromotion(currentSelectedID);

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    currentSelectedID = -1;
                    LoadPromotions(txtSearch.Text, cboFilterStatus.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvPromotions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                currentSelectedID = Convert.ToInt32(dgvPromotions.Rows[e.RowIndex].Cells["Mã KM"].Value);
        }

        private void DgvPromotions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvPromotions.Columns[e.ColumnIndex].Name;

            if (colName == "Trạng thái" && e.Value != null)
            {
                string status = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                e.CellStyle.ForeColor = (status == "Active")
                    ? Color.FromArgb(16, 185, 129)
                    : Color.FromArgb(239, 68, 68);
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }

            if (colName == "Giảm (%)" && e.Value != null)
            {
                e.Value = e.Value.ToString() + "%";
                e.CellStyle.ForeColor = Color.FromArgb(88, 28, 230);
                e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
        }
    }
}
