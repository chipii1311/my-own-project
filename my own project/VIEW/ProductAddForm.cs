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
                // 1. Kiểm tra nhập liệu
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Vui lòng nhập đủ Tên món và Giá tiền!");
                    return;
                }

                // 2. Gom dữ liệu vào DTO
                MenuItemDTO newItem = new MenuItemDTO();
                newItem.ItemName = txtName.Text;
                newItem.Price = Convert.ToDecimal(txtPrice.Text);
                newItem.CategoryID = Convert.ToInt32(cbbCategory.SelectedValue);
                newItem.ImageUrl = selectedImageName; // Cái tên file ảnh vừa tạo ở trên
                newItem.IsAvailable = true; // (Hoặc lấy từ CheckBox nếu bạn có thêm vào)

                // 3. Gọi BLL lưu xuống DB
                MenuItemBLL.AddMenuItem(newItem);

                MessageBox.Show("Thêm món ăn thành công!");
                this.Close(); // Đóng form lại
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }
    }
}
