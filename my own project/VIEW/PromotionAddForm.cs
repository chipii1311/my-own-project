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
    public partial class PromotionAddForm : SampleAdd
    {
        public PromotionAddForm()
        {
            InitializeComponent();
        }

        private void PromotionAddForm_Load(object sender, EventArgs e)
        {
            // 1. Nạp dữ liệu cho ComboBox Loại áp dụng
            cbbApplyType.Items.Add("Toàn bộ hóa đơn"); // Index = 0
            cbbApplyType.Items.Add("Theo món cụ thể"); // Index = 1
            cbbApplyType.SelectedIndex = 0; // Mặc định chọn cái đầu tiên

            // 2. Nạp dữ liệu cho ComboBox Trạng thái
            cbbStatus.Items.Add("Active");
            cbbStatus.Items.Add("Inactive");
            cbbStatus.SelectedIndex = 0;

            // 3. Đổ danh sách món ăn vào CheckedListBox
            LoadMenuItems();
        }


        private void LoadMenuItems()
        {
            try
            {
                // Gọi thẳng Stored Procedure thông qua DataHelper
                DataTable dtMenu = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllLite");

                // Ép kiểu CheckedListBox về ListBox
                ((ListBox)clbMenuItems).DataSource = dtMenu;
                ((ListBox)clbMenuItems).DisplayMember = "ItemName";
                ((ListBox)clbMenuItems).ValueMember = "MenuItemID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách món: " + ex.Message);
            }
        }

        private void cbbApplyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbApplyType.SelectedIndex == 0) // Chọn Toàn bộ hóa đơn
            {
                clbMenuItems.Enabled = false; // Khóa mờ danh sách món

                // Lệnh này giúp xóa bỏ mọi dấu tick nếu người dùng đổi ý từ "Theo món" sang "Toàn bill"
                for (int i = 0; i < clbMenuItems.Items.Count; i++)
                {
                    clbMenuItems.SetItemChecked(i, false);
                }
            }
            else // Chọn Theo món cụ thể
            {
                clbMenuItems.Enabled = true; // Mở khóa cho phép tick chọn
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // 1. Lấy thông tin từ giao diện
            string promoName = txtPromotionName.Text.Trim();
            decimal discount = numDiscountPercent.Value; // Giả sử bạn dùng NumericUpDown
            DateTime start = dtpStartDate.Value;
            DateTime end = dtpEndDate.Value;
            int applyType = cbbApplyType.SelectedIndex;
            string status = cbbStatus.SelectedItem.ToString();

            // Validate sơ bộ
            if (string.IsNullOrEmpty(promoName))
            {
                MessageBox.Show("Vui lòng nhập tên khuyến mãi!");
                return;
            }

            try
            {
                // ==========================================
                // BƯỚC A: LƯU VÀO BẢNG PROMOTION VÀ LẤY ID MỚI
                // ==========================================
                // Đoạn này bạn gọi hàm Insert từ PromotionDAL. 
                // LƯU Ý: Hàm Insert trong DAL của bạn phải dùng lệnh "SELECT SCOPE_IDENTITY();" ở cuối câu SQL 
                // để trả về cái ID của dòng vừa thêm vào.

                PromotionDTO newPromo = new PromotionDTO
                {
                    PromotionName = promoName,
                    DiscountPercent = discount,
                    StartDate = start,
                    EndDate = end,
                    Status = status,
                    ApplyType = applyType
                };

                int newPromotionID = PromotionDAL.Insert(newPromo);

                // ==========================================
                // BƯỚC B: LƯU VÀO BẢNG PROMOTION_DETAIL (Nếu chọn theo món)
                // ==========================================
                if (applyType == 1 && newPromotionID > 0)
                {
                    // Duyệt qua tất cả các món ăn ĐÃ ĐƯỢC TICK
                    foreach (DataRowView checkedItem in clbMenuItems.CheckedItems)
                    {
                        // Lấy MenuItemID đang nằm ẩn dưới cái tên món
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

                MessageBox.Show("Thêm Khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Đóng pop-up
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
