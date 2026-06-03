using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class StaffForm : Form
    {
        private int _selectedUserID = -1;

        public StaffForm()
        {
            InitializeComponent();
            this.Load += StaffForm_Load;
        }

        // ========================================================
        #region 1. LOAD DỮ LIỆU
        // ========================================================

        private void StaffForm_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void LoadStaffData()
        {
            try
            {
                string query = "SELECT UserID AS [Mã NV], FullName AS [Họ Tên], Email, Phone AS [SĐT], Role AS [Vai trò], IsActive FROM Users ORDER BY UserID DESC";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                DataTable dtDisplay = dt.Clone();
                dtDisplay.Columns["IsActive"].DataType = typeof(string);

                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = dtDisplay.NewRow();
                    newRow["Mã NV"] = row["Mã NV"];
                    newRow["Họ Tên"] = row["Họ Tên"];
                    newRow["Email"] = row["Email"];
                    newRow["SĐT"] = row["SĐT"];
                    newRow["Vai trò"] = row["Vai trò"];
                    newRow["IsActive"] = Convert.ToBoolean(row["IsActive"]) ? "Hoạt động" : "Đã khóa";
                    dtDisplay.Rows.Add(newRow);
                }

                dgvStaff.DataSource = dtDisplay;

                if (dgvStaff.Columns.Contains("Mã NV")) dgvStaff.Columns["Mã NV"].Width = 80;
                if (dgvStaff.Columns.Contains("Vai trò")) dgvStaff.Columns["Vai trò"].Width = 100;
                if (dgvStaff.Columns.Contains("IsActive")) dgvStaff.Columns["IsActive"].Width = 110;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message);
            }
        }

        #endregion

        // ========================================================
        #region 2. SỰ KIỆN GRID
        // ========================================================

        private void DgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];

                _selectedUserID = Convert.ToInt32(row.Cells["Mã NV"].Value);
                txtFullName.Text = row.Cells["Họ Tên"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtPhone.Text = row.Cells["SĐT"].Value?.ToString();

                string role = row.Cells["Vai trò"].Value?.ToString();
                if (cboRole.Items.Contains(role)) cboRole.Text = role;

                string status = row.Cells["IsActive"].Value?.ToString();
                if (cboStatus.Items.Contains(status)) cboStatus.Text = status;
            }
        }

        #endregion

        // ========================================================
        #region 3. NÚT CHỨC NĂNG
        // ========================================================

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedUserID == -1)
            {
                MessageBox.Show("Vui lòng click chọn 1 nhân viên từ bảng bên trái để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs())
                return;

            try
            {
                int isActive = (cboStatus.Text == "Hoạt động") ? 1 : 0;

                string query = $"UPDATE Users SET FullName = N'{EscapeSQL(txtFullName.Text)}', Email = N'{EscapeSQL(txtEmail.Text)}', Phone = N'{EscapeSQL(txtPhone.Text)}', " +
                               $"Role = N'{cboRole.Text}', IsActive = {isActive} " +
                               $"WHERE UserID = {_selectedUserID}";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputs();
                LoadStaffData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedUserID == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhân viên: {txtFullName.Text}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = $"DELETE FROM Users WHERE UserID = {_selectedUserID}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadStaffData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            NewAccountAddForm frmAdd = new NewAccountAddForm();
            frmAdd.ShowDialog();
            LoadStaffData();
        }

        #endregion

        // ========================================================
        #region 4. HỖ TRỢ / VALIDATION
        // ========================================================

        private void ClearInputs()
        {
            _selectedUserID = -1;
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            if (cboRole.Items.Count > 0) cboRole.SelectedIndex = 0;
            if (cboStatus.Items.Count > 0) cboStatus.SelectedIndex = 0;
            dgvStaff.ClearSelection();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!IsValidPhone(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (10-11 chữ số)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            if (cboRole.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10,11}$");
        }

        private string EscapeSQL(string input)
        {
            return input.Replace("'", "''");
        }

        #endregion
    }
}
