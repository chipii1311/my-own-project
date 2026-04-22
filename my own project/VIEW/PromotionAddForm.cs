using my_own_project.DAL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class PromotionAddForm : Form
    {
        private DataTable menuItemsData;
        private int _promotionID = 0;
        private bool _isAddMode = true;

        // 1. CHẾ ĐỘ THÊM
        public PromotionAddForm()
        {
            InitializeComponent();
            _isAddMode = true;
            _promotionID = 0;
        }

        // 2. CHẾ ĐỘ SỬA
        public PromotionAddForm(int id)
        {
            InitializeComponent();
            _promotionID = id;
            _isAddMode = (id <= 0);
        }

        private void PromotionAddForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Set Data ComboBox
                cbbApplyType.Items.Clear();
                cbbApplyType.Items.Add("🏪 Toàn bộ hóa đơn");
                cbbApplyType.Items.Add("🍽️ Theo món cụ thể");

                cbbStatus.Items.Clear();
                cbbStatus.Items.Add("Active");
                cbbStatus.Items.Add("Inactive");

                // Load list món ăn
                LoadMenuItems();

                // PHÂN LUỒNG
                if (_isAddMode)
                {
                    cbbApplyType.SelectedIndex = 0;
                    cbbStatus.SelectedIndex = 0;
                    dtpStartDate.Value = DateTime.Now;
                    dtpEndDate.Value = DateTime.Now.AddMonths(1);
                    numDiscountPercent.Value = 0;
                }
                else
                {
                    // Gọi hàm Sửa
                    LoadOldPromotionData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi Load Form: " + ex.Message, "Lỗi Debug");
            }
        }

        private void LoadMenuItems()
        {
            menuItemsData = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllLite");
            if (menuItemsData == null || menuItemsData.Rows.Count == 0) return;

            clbMenuItems.DataSource = menuItemsData;
            clbMenuItems.DisplayMember = "ItemName";
            clbMenuItems.ValueMember = "MenuItemID";

            for (int i = 0; i < clbMenuItems.Items.Count; i++)
                clbMenuItems.SetItemChecked(i, false);
        }

        private void LoadOldPromotionData()
        {
            try
            {
                PromotionDTO promo = PromotionDAL.GetByID(_promotionID);

                // Nếu Database trả về null (lỗi truy vấn)
                if (promo == null)
                {
                    MessageBox.Show($"⚠️ CSDL không tìm thấy khuyến mãi mang ID = {_promotionID}!", "Lỗi Debug");
                    return;
                }

                txtPromotionName.Text = promo.PromotionName;
                numDiscountPercent.Value = promo.DiscountPercent;
                dtpStartDate.Value = promo.StartDate;
                dtpEndDate.Value = promo.EndDate;

                int statusIndex = cbbStatus.Items.IndexOf(promo.Status);
                cbbStatus.SelectedIndex = statusIndex >= 0 ? statusIndex : 0;

                // Nạp chế độ áp dụng
                cbbApplyType.SelectedIndex = promo.ApplyType;

                // Tick lại các món ăn
                if (promo.ApplyType == 1)
                {
                    DataTable dtDetails = PromotionDAL.GetPromotionDetails(_promotionID);
                    if (dtDetails != null && dtDetails.Rows.Count > 0)
                    {
                        List<int> checkedIDs = new List<int>();
                        foreach (DataRow row in dtDetails.Rows)
                        {
                            checkedIDs.Add(Convert.ToInt32(row["MenuItemID"]));
                        }

                        for (int i = 0; i < clbMenuItems.Items.Count; i++)
                        {
                            DataRowView item = (DataRowView)clbMenuItems.Items[i];
                            int menuID = Convert.ToInt32(item["MenuItemID"]);
                            if (checkedIDs.Contains(menuID))
                            {
                                clbMenuItems.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi lấy dữ liệu cũ: " + ex.Message, "Lỗi Debug");
            }
        }

        private void cbbApplyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbApplyType.SelectedIndex == 0) // Toàn bill
            {
                clbMenuItems.Enabled = false;
                for (int i = 0; i < clbMenuItems.Items.Count; i++)
                    clbMenuItems.SetItemChecked(i, false);

                lblApplyInfo.Text = "✓ Khuyến mãi sẽ áp dụng cho tất cả hóa đơn";
                lblApplyInfo.ForeColor = Color.FromArgb(76, 175, 80);
            }
            else // Theo món cụ thể
            {
                clbMenuItems.Enabled = true;
                lblApplyInfo.Text = "Chọn các món ăn cần áp dụng khuyến mãi";
                lblApplyInfo.ForeColor = Color.FromArgb(100, 100, 100);
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPromotionName.Text.Trim()))
                {
                    MessageBox.Show("⚠️ Vui lòng nhập tên khuyến mãi!", "Lỗi");
                    return;
                }

                PromotionDTO newPromo = new PromotionDTO
                {
                    PromotionID = _promotionID,
                    PromotionName = txtPromotionName.Text.Trim(),
                    DiscountPercent = numDiscountPercent.Value,
                    StartDate = dtpStartDate.Value,
                    EndDate = dtpEndDate.Value,
                    Status = cbbStatus.SelectedItem != null ? cbbStatus.SelectedItem.ToString() : "Active",
                    ApplyType = cbbApplyType.SelectedIndex
                };

                int targetPromotionID = 0;

                if (_isAddMode)
                {
                    targetPromotionID = PromotionDAL.Insert(newPromo);
                }
                else
                {
                    PromotionDAL.Update(newPromo);
                    targetPromotionID = _promotionID;
                }

                if (targetPromotionID > 0 && newPromo.ApplyType == 1)
                {
                    for (int i = 0; i < clbMenuItems.Items.Count; i++)
                    {
                        if (clbMenuItems.GetItemChecked(i))
                        {
                            DataRowView checkedItem = (DataRowView)clbMenuItems.Items[i];
                            PromotionDetailDTO detail = new PromotionDetailDTO
                            {
                                PromotionID = targetPromotionID,
                                MenuItemID = Convert.ToInt32(checkedItem["MenuItemID"])
                            };
                            PromotionDAL.InsertPromotionDetail(detail);
                        }
                    }
                }

                MessageBox.Show("✅ Lưu thành công!", "Thành công");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi lưu: " + ex.Message, "Lỗi Debug");
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}