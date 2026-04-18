using my_own_project.BLL;
using my_own_project.DAL;
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
using System.Xml.Linq;

namespace my_own_project.DesignForms
{


    public partial class ProductAddForm : SampleAdd
    {

        private string selectedImageName = "";
        public int editItemID = -1;
        public ProductAddForm()
        {
            InitializeComponent();
        }

        private void ProductAddForm_Load(object sender, EventArgs e)
        {
            // Giả sử bạn đổi tên ComboBox là cboCategory
            DataTable dtCategories = CategoryBLL.GetAllCategories();
            cbbCategory.DataSource = dtCategories;
            cbbCategory.DisplayMember = "CategoryName"; // Chữ hiện lên cho người ta đọc
            cbbCategory.ValueMember = "CategoryID";     // ID ngầm bên dưới để lưu SQL

            cbbStatus.Items.Clear();
            cbbStatus.Items.Add("Ngừng kinh doanh"); // Index 0
            cbbStatus.Items.Add("Đang phục vụ");     // Index 1
            cbbStatus.Items.Add("Tạm hết hàng");     // Index 2
            cbbStatus.SelectedIndex = 1; // Mặc định mở lên là "Đang phục vụ"

            // 2. NẾU LÀ CHẾ ĐỘ SỬA (editItemID khác -1) -> Lôi dữ liệu cũ lên
            if (editItemID != -1)
            {
                // Đổi tiêu đề Form cho chuyên nghiệp
                this.Text = "Sửa món ăn";

                // Lấy dữ liệu từ DB lên
                MenuItemDTO item = MenuItemDAL.GetByID(editItemID);
                if (item != null)
                {
                    // Điền vào các ô trống
                    txtName.Text = item.ItemName;
                    txtPrice.Text = item.Price.ToString();
                    cbbCategory.SelectedValue = item.CategoryID;

                    // Mẹo cực hay: Vì Index của ComboBox là 0,1,2 khớp y xì đúc với trạng thái trong DB!
                    cbbStatus.SelectedIndex = item.ItemStatus;

                    // Hiển thị ảnh cũ (nếu có)
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
                        selectedImageName = item.ImageUrl; // Giữ lại tên file cũ để lỡ họ không đổi ảnh
                        string imagePath = Application.StartupPath + "\\MenuImages\\" + item.ImageUrl;
                        if (System.IO.File.Exists(imagePath))
                        {
                            pictureBox1.Image = Image.FromFile(imagePath);
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                }
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            // Chỉ cho phép chọn file ảnh
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.webp)|*.jpg; *.jpeg; *.png; *.webp";

            if (open.ShowDialog() == DialogResult.OK)
            {
                // 1. Hiển thị ảnh lên PictureBox cho người dùng xem trước
                pictureBox1.Image = Image.FromFile(open.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                // 2. Tạo một cái tên file mới tinh không bị trùng (dùng ngày giờ)
                string extension = System.IO.Path.GetExtension(open.FileName);
                selectedImageName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension;

                // 3. Đường dẫn đích: Thư mục MenuImages của project
                string targetPath = Application.StartupPath + "\\MenuImages\\" + selectedImageName;

                // 4. Copy cái ảnh người dùng vừa chọn vào thư mục của phần mềm
                System.IO.File.Copy(open.FileName, targetPath, true);
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ Tên và Giá!");
                    return;
                }

                // Gom dữ liệu từ trên giao diện xuống
                MenuItemDTO item = new MenuItemDTO();
                item.ItemName = txtName.Text;
                item.Price = Convert.ToDecimal(txtPrice.Text);
                item.CategoryID = Convert.ToInt32(cbbCategory.SelectedValue);
                item.ItemStatus = cbbStatus.SelectedIndex; // Lấy đúng số 0, 1, 2
                item.ImageUrl = selectedImageName;

                // RẼ NHÁNH: THÊM HAY SỬA?
                if (editItemID == -1)
                {
                    // CHẾ ĐỘ THÊM
                    MenuItemDAL.Insert(item);
                    MessageBox.Show("Đã thêm món ăn thành công!");
                }
                else
                {
                    // CHẾ ĐỘ SỬA
                    item.MenuItemID = editItemID; // Gắn ID vào để DB biết sửa dòng nào
                    MenuItemDAL.Update(item);
                    MessageBox.Show("Đã cập nhật thông tin món ăn!");
                }

                this.Close(); // Đóng form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
