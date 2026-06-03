using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace my_own_project.VIEW
{
    public partial class NewPromotionAddForm : Form
    {
        private int _promoID = -1;

        public NewPromotionAddForm(int promoID = -1)
        {
            InitializeComponent();
            _promoID = promoID;

            // Khởi tạo các thông số Form cơ bản
            this.Size = new Size(800, 850);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;

            new Guna2Elipse { TargetControl = this, BorderRadius = 14 };

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            // Tải dữ liệu ban đầu
            LoadMenuItems();

            // Nếu truyền ID vào tức là đang ở chế độ SỬA
            if (_promoID != -1)
            {
                LoadDataForEdit();
                lblTitle.Text = "CẬP NHẬT KHUYẾN MÃI";
                btnSave.Text = "  💾  LƯU THAY ĐỔI";
            }
        }

        // ========================================================
        // 1. DATA BINDING & XỬ LÝ SỰ KIỆN GIAO DIỆN ĐỘNG
        // ========================================================

        // ── Đóng/mở khung chọn món ăn tùy theo hình thức ──
        public void CboApplyType_Changed(object sender, EventArgs e)
        {
            bool showPicker = cboApplyType.SelectedIndex == 1; // 1 = Giảm theo món ăn
            pnlItemPicker.Height = showPicker ? 280 : 0;       // expand / collapse
            flpForm.PerformLayout();
        }

        private void LoadMenuItems()
        {
            try
            {
                string query = @"SELECT MenuItemID, ItemName FROM MenuItem ORDER BY ItemName";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                clbItems.Items.Clear();

                // Thêm từng item với đối tượng MenuItem_Item thay vì DataRowView
                foreach (DataRow row in dt.Rows)
                {
                    int menuItemID = Convert.ToInt32(row["MenuItemID"]);
                    string itemName = row["ItemName"].ToString();

                    clbItems.Items.Add(new MenuItem_Item
                    {
                        ID = menuItemID,
                        Name = itemName
                    }, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách món: " + ex.Message);
            }
        }

        private void LoadDataForEdit()
        {
            try
            {
                string query = $"SELECT * FROM Promotion WHERE PromotionID = {_promoID}";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                if (dt.Rows.Count == 0) return;

                DataRow r = dt.Rows[0];
                txtPromoName.Text = r["PromotionName"].ToString();
                txtDiscount.Text = r["DiscountPercent"].ToString();
                dtpStart.Value = Convert.ToDateTime(r["StartDate"]);
                dtpEnd.Value = Convert.ToDateTime(r["EndDate"]);
                cboStatus.Text = r["Status"].ToString();

                int applyType = Convert.ToInt32(r["ApplyType"]);
                cboApplyType.SelectedIndex = applyType; // 0 hoặc 1

                // Nếu giảm theo món → Load lại các check box
                if (applyType == 1)
                {
                    string detailQ = $"SELECT MenuItemID FROM PromotionDetail WHERE PromotionID = {_promoID}";
                    DataTable dtd = my_own_project.DAL.DataHelper.ExecuteQuery(detailQ);
                    var selectedIDs = new HashSet<int>();
                    foreach (DataRow dr in dtd.Rows)
                        selectedIDs.Add(Convert.ToInt32(dr["MenuItemID"]));

                    for (int i = 0; i < clbItems.Items.Count; i++)
                    {
                        MenuItem_Item item = (MenuItem_Item)clbItems.Items[i];
                        clbItems.SetItemChecked(i, selectedIDs.Contains(item.ID));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // ========================================================
        // 2. LƯU DỮ LIỆU
        // ========================================================
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate (Xác thực dữ liệu)
            if (string.IsNullOrWhiteSpace(txtPromoName.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên chương trình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtDiscount.Text.Trim(), out decimal discount) || discount <= 0 || discount > 100)
            {
                MessageBox.Show("Phần trăm giảm phải là số từ 1–100!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtpStart.Value >= dtpEnd.Value)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int applyType = cboApplyType.SelectedIndex; // 0 hoặc 1

            // Validate chọn món nếu ApplyType = 1
            List<int> selectedMenuItemIDs = new List<int>();
            if (applyType == 1)
            {
                foreach (MenuItem_Item item in clbItems.CheckedItems)
                    selectedMenuItemIDs.Add(item.ID);

                if (selectedMenuItemIDs.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một món ăn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Thực thi DB
            try
            {
                string start = dtpStart.Value.ToString("yyyy-MM-dd");
                string end = dtpEnd.Value.ToString("yyyy-MM-dd");
                string name = txtPromoName.Text.Trim().Replace("'", "''");
                string status = cboStatus.Text;
                int promoID = _promoID;

                if (_promoID == -1) // THÊM MỚI
                {
                    string ins = $@"INSERT INTO Promotion (PromotionName, DiscountPercent, StartDate, EndDate, Status, ApplyType)
                                    VALUES (N'{name}', {discount}, '{start}', '{end}', N'{status}', {applyType});
                                    SELECT SCOPE_IDENTITY();";
                    DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(ins);
                    promoID = Convert.ToInt32(dt.Rows[0][0]);
                }
                else // CẬP NHẬT
                {
                    string upd = $@"UPDATE Promotion SET
                                    PromotionName   = N'{name}',
                                    DiscountPercent = {discount},
                                    StartDate       = '{start}',
                                    EndDate         = '{end}',
                                    Status          = N'{status}',
                                    ApplyType       = {applyType}
                                    WHERE PromotionID = {_promoID}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(upd);

                    // Xóa PromotionDetail cũ trước khi ghi đè lại
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM PromotionDetail WHERE PromotionID = {_promoID}");
                }

                // LƯU PromotionDetail nếu chọn Khuyến mãi theo món
                if (applyType == 1)
                {
                    foreach (int mid in selectedMenuItemIDs)
                    {
                        my_own_project.DAL.DataHelper.ExecuteNonQuery($"INSERT INTO PromotionDetail (PromotionID, MenuItemID) VALUES ({promoID}, {mid})");
                    }
                }

                MessageBox.Show(_promoID == -1 ? "Tạo khuyến mãi thành công!" : "Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message);
            }
        }
    }

    // ========================================================
    // CLASS HỖ TRỢ: Để hiển thị tên món thay vì DataRowView
    // ========================================================
    public class MenuItem_Item
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name; // Hiển thị tên món trong CheckedListBox
        }
    }
}
