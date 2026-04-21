using my_own_project.DAL;
using my_own_project.DesignForms;
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
    public partial class PromotionAddForm : Form
    {
        private DataTable menuItemsData;
        

        public PromotionAddForm()
        {
            InitializeComponent();
        }

        private void PromotionAddForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Nạp dữ liệu cho ComboBox Loại áp dụng
                cbbApplyType.Items.Add("🏪 Toàn bộ hóa đơn"); // Index = 0
                cbbApplyType.Items.Add("🍽️ Theo món cụ thể"); // Index = 1
                cbbApplyType.SelectedIndex = 0;

                // 2. Nạp dữ liệu cho ComboBox Trạng thái
                cbbStatus.Items.Add("Active");
                cbbStatus.Items.Add("Inactive");
                cbbStatus.SelectedIndex = 0;

                // 3. Đổ danh sách món ăn vào CheckedListBox
                LoadMenuItems();

                // 4. Set mặc định
                dtpStartDate.Value = DateTime.Now;
                dtpEndDate.Value = DateTime.Now.AddMonths(1);
                numDiscountPercent.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khởi tạo form: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // LOAD MENU ITEMS
        // ============================================
        private void LoadMenuItems()
        {
            try
            {
                // Gọi Stored Procedure để lấy danh sách món ăn
                menuItemsData = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllLite");

                if (menuItemsData == null || menuItemsData.Rows.Count == 0)
                {
                    MessageBox.Show("⚠️ Không có món ăn nào trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Bind dữ liệu vào CheckedListBox
                clbMenuItems.DataSource = menuItemsData;
                clbMenuItems.DisplayMember = "ItemName";
                clbMenuItems.ValueMember = "MenuItemID";

                // Uncheck tất cả mặc định
                for (int i = 0; i < clbMenuItems.Items.Count; i++)
                {
                    clbMenuItems.SetItemChecked(i, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải danh sách món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // COMBOBOX APPLY TYPE CHANGED
        // ============================================
        private void cbbApplyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbApplyType.SelectedIndex == 0) // Toàn bộ hóa đơn
            {
                clbMenuItems.Enabled = false;

                // Uncheck tất cả
                for (int i = 0; i < clbMenuItems.Items.Count; i++)
                {
                    clbMenuItems.SetItemChecked(i, false);
                }

                // Cập nhật label thông tin
                lblApplyInfo.Text = "✓ Khuyến mãi sẽ áp dụng cho tất cả hóa đơn";
                lblApplyInfo.ForeColor = Color.FromArgb(76, 175, 80);
            }
            else // Theo món cụ thể (Index = 1)
            {
                clbMenuItems.Enabled = true;

                // Cập nhật label thông tin
                lblApplyInfo.Text = "Chọn các món ăn cần áp dụng khuyến mãi";
                lblApplyInfo.ForeColor = Color.FromArgb(100, 100, 100);
            }
        }

        // ============================================
        // BUTTON SAVE CLICK
        // ============================================
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy thông tin từ giao diện
                string promoName = txtPromotionName.Text.Trim();
                decimal discount = numDiscountPercent.Value;
                DateTime startDate = dtpStartDate.Value;
                DateTime endDate = dtpEndDate.Value;
                int applyType = cbbApplyType.SelectedIndex;
                string status = cbbStatus.SelectedItem.ToString();

                // 2. Validate dữ liệu
                if (string.IsNullOrEmpty(promoName))
                {
                    MessageBox.Show("⚠️ Vui lòng nhập tên khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPromotionName.Focus();
                    return;
                }

                if (discount <= 0)
                {
                    MessageBox.Show("⚠️ Mức giảm phải lớn hơn 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numDiscountPercent.Focus();
                    return;
                }

                if (discount > 100)
                {
                    MessageBox.Show("⚠️ Mức giảm không được vượt quá 100%!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numDiscountPercent.Focus();
                    return;
                }

                if (endDate <= startDate)
                {
                    MessageBox.Show("⚠️ Ngày kết thúc phải sau ngày bắt đầu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpEndDate.Focus();
                    return;
                }

                if (applyType == 1) // Theo món cụ thể
                {
                    bool hasCheckedItem = false;
                    for (int i = 0; i < clbMenuItems.Items.Count; i++)
                    {
                        if (clbMenuItems.GetItemChecked(i))
                        {
                            hasCheckedItem = true;
                            break;
                        }
                    }

                    if (!hasCheckedItem)
                    {
                        MessageBox.Show("⚠️ Vui lòng chọn ít nhất một món ăn!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        clbMenuItems.Focus();
                        return;
                    }
                }

                // ==========================================
                // BƯỚC A: LƯU VÀO BẢNG PROMOTION VÀ LẤY ID MỚI
                // ==========================================
                PromotionDTO newPromo = new PromotionDTO
                {
                    PromotionName = promoName,
                    DiscountPercent = discount,
                    StartDate = startDate,
                    EndDate = endDate,
                    Status = status,
                    ApplyType = applyType
                };

                int newPromotionID = PromotionDAL.Insert(newPromo);

                if (newPromotionID <= 0)
                {
                    MessageBox.Show("❌ Lỗi: Không thể lưu khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ==========================================
                // BƯỚC B: LƯU VÀO BẢNG PROMOTION_DETAIL (Nếu chọn theo món)
                // ==========================================
                if (applyType == 1 && newPromotionID > 0)
                {
                    // Duyệt qua tất cả các món ăn ĐÃ ĐƯỢC TICK
                    for (int i = 0; i < clbMenuItems.Items.Count; i++)
                    {
                        if (clbMenuItems.GetItemChecked(i))
                        {
                            // Lấy MenuItemID
                            DataRowView checkedItem = (DataRowView)clbMenuItems.Items[i];
                            int menuID = Convert.ToInt32(checkedItem["MenuItemID"]);

                            // Lưu từng món vào CSDL
                            PromotionDetailDTO detail = new PromotionDetailDTO
                            {
                                PromotionID = newPromotionID,
                                MenuItemID = menuID
                            };
                            PromotionDAL.InsertPromotionDetail(detail);
                        }
                    }
                }

                // ==========================================
                // THÀNH CÔNG
                // ==========================================
                MessageBox.Show("✅ Thêm khuyến mãi thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi lưu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // BUTTON CLOSE CLICK
        // ============================================
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
