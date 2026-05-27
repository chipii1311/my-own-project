using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class StaffForm : Form
    {
        public StaffForm()
        {
            InitializeComponent();
            BuildUI(); // Hàm dựng giao diện từ Designer
            this.Load += StaffForm_Load;
        }

        private void StaffForm_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        // ==========================================
        // 1. DATA LOADING
        // ==========================================
        private void LoadStaffData()
        {
            try
            {
                string query = "SELECT UserID, FullName AS [Họ tên], Email, Phone AS [SĐT], Role AS [Vai trò], CASE WHEN IsActive = 1 THEN N'Đang hoạt động' ELSE N'Đã nghỉ' END AS [Trạng thái] FROM Users";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvStaff.DataSource = dt;

                if (dgvStaff.Columns.Contains("UserID"))
                    dgvStaff.Columns["UserID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }

        // ==========================================
        // 2. SỰ KIỆN XỬ LÝ (CRUD)
        // ==========================================
        private void DgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];
                txtUserID.Text = row.Cells["UserID"].Value.ToString();
                txtFullName.Text = row.Cells["Họ tên"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtPhone.Text = row.Cells["SĐT"].Value.ToString();
                cboRole.Text = row.Cells["Vai trò"].Value.ToString();
                cboStatus.Text = row.Cells["Trạng thái"].Value.ToString();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int isActive = (cboStatus.Text == "Đang hoạt động") ? 1 : 0;
                string query = $"UPDATE Users SET FullName = N'{txtFullName.Text}', Email = N'{txtEmail.Text}', Phone = N'{txtPhone.Text}', Role = N'{cboRole.Text}', IsActive = {isActive} WHERE UserID = {txtUserID.Text}";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadStaffData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e) => ClearInputs();

        private void ClearInputs()
        {
            txtUserID.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            cboRole.SelectedIndex = -1;
            cboStatus.SelectedIndex = -1;
        }

        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            using (var frm = new NewAccountAddForm())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadStaffData();
            }
        }
    }
}