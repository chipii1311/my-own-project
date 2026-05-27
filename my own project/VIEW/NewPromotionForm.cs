using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
// using my_own_project.DAL;

namespace my_own_project.VIEW
{
    public partial class NewPromotionForm : Form
    {
        private int currentSelectedID = -1;

        public NewPromotionForm()
        {
            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            this.Load += NewPromotionForm_Load;
        }

        private void NewPromotionForm_Load(object sender, EventArgs e)
        {
            LoadPromotions();
        }

        // ========================================================
        // 1. DATA BINDING (TẢI DỮ LIỆU)
        // ========================================================
        private void LoadPromotions(string keyword = "", string status = "Tất cả")
        {
            try
            {
                // Lấy dữ liệu từ DB (SQL Server)
                string query = @"SELECT PromotionID AS [Mã KM], 
                                        PromotionName AS [Tên chương trình], 
                                        DiscountPercent AS [Giảm (%)], 
                                        StartDate AS [Ngày BĐ], 
                                        EndDate AS [Ngày KT], 
                                        Status AS [Trạng thái],
                                        CASE WHEN ApplyType = 0 THEN N'Tổng hóa đơn' ELSE N'Theo món ăn' END AS [Áp dụng]
                                 FROM Promotion WHERE 1=1";

                if (!string.IsNullOrEmpty(keyword))
                    query += $" AND PromotionName LIKE N'%{keyword}%'";

                if (status != "Tất cả")
                    query += $" AND Status = N'{status}'";

                query += " ORDER BY PromotionID DESC";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvPromotions.DataSource = dt;

                // Ẩn cột ID
                if (dgvPromotions.Columns.Contains("Mã KM"))
                    dgvPromotions.Columns["Mã KM"].Visible = false;

                // Định dạng ngày tháng
                if (dgvPromotions.Columns.Contains("Ngày BĐ"))
                    dgvPromotions.Columns["Ngày BĐ"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgvPromotions.Columns.Contains("Ngày KT"))
                    dgvPromotions.Columns["Ngày KT"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                // MOCK DATA: Dự phòng nếu chưa có CSDL hoặc lỗi kết nối để kiểm tra UI
                DataTable dt = new DataTable();
                dt.Columns.Add("Mã KM", typeof(int));
                dt.Columns.Add("Tên chương trình", typeof(string));
                dt.Columns.Add("Giảm (%)", typeof(int));
                dt.Columns.Add("Ngày BĐ", typeof(string));
                dt.Columns.Add("Ngày KT", typeof(string));
                dt.Columns.Add("Trạng thái", typeof(string));
                dt.Columns.Add("Áp dụng", typeof(string));

                dt.Rows.Add(1, "Siêu sale cuối tuần", 20, "01/10/2026", "31/10/2026", "Active", "Tổng hóa đơn");
                dt.Rows.Add(2, "Khuyến mãi món mới", 15, "15/10/2026", "17/10/2026", "Active", "Theo món ăn");
                dt.Rows.Add(3, "Tri ân khách hàng cũ", 10, "01/01/2026", "31/12/2026", "Inactive", "Tổng hóa đơn");
                dgvPromotions.DataSource = dt;

                if (dgvPromotions.Columns.Contains("Mã KM")) dgvPromotions.Columns["Mã KM"].Visible = false;
            }
        }

        // ========================================================
        // 2. SỰ KIỆN TÌM KIẾM & LỌC
        // ========================================================
        public void TxtSearch_TextChanged(object sender, EventArgs e) => LoadPromotions(txtSearch.Text, cboFilterStatus.Text);
        public void CboFilterStatus_SelectedIndexChanged(object sender, EventArgs e) => LoadPromotions(txtSearch.Text, cboFilterStatus.Text);

        // ========================================================
        // 3. SỰ KIỆN NÚT BẤM (THÊM / SỬA / XÓA)
        // ========================================================
        public void BtnAdd_Click(object sender, EventArgs e)
        {
            // Truyền -1 để nhận biết đây là lệnh Thêm Mới
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
            // Truyền ID sang form Add để nó load dữ liệu cũ lên
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

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khuyến mãi này? Thao tác này không thể hoàn tác.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Xóa chi tiết món ăn (Khóa ngoại) trước rồi mới xóa Khuyến mãi
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM PromotionDetail WHERE PromotionID = {currentSelectedID}");
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM Promotion WHERE PromotionID = {currentSelectedID}");

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

        // ========================================================
        // 4. SỰ KIỆN CLICK GRID & ĐỊNH DẠNG MÀU SẮC
        // ========================================================
        private void DgvPromotions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                currentSelectedID = Convert.ToInt32(dgvPromotions.Rows[e.RowIndex].Cells["Mã KM"].Value);
            }
        }

        private void DgvPromotions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvPromotions.Columns[e.ColumnIndex].Name;

            // Đổi màu Trạng thái
            if (colName == "Trạng thái" && e.Value != null)
            {
                string status = e.Value.ToString();
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                if (status == "Active")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129); // Xanh lá
                    e.CellStyle.SelectionForeColor = Color.FromArgb(16, 185, 129);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68); // Đỏ
                    e.CellStyle.SelectionForeColor = Color.FromArgb(239, 68, 68);
                }
            }

            // Định dạng thêm dấu % cho cột giảm giá
            if (colName == "Giảm (%)" && e.Value != null)
            {
                e.Value = e.Value.ToString() + "%";
                e.CellStyle.ForeColor = Color.FromArgb(88, 28, 230); // Tím đậm
                e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
        }
    }
}