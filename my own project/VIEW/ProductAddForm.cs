using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
// using my_own_project.DAL;

namespace my_own_project.VIEW
{
    public partial class ProductAddForm : Form
    {
        private string selectedImagePath = "";

        public ProductAddForm()
        {
            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            // Gắn sự kiện Load form
            this.Load += ProductAddForm_Load;
        }

        // ========================================================
        // 1. DATA BINDING (TẢI DỮ LIỆU)
        // ========================================================
        private void ProductAddForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT CategoryID, CategoryName FROM Category WHERE IsActive = 1";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                cboCategory.DataSource = dt;
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========================================================
        // 2. SỰ KIỆN NÚT BẤM (CHỌN ẢNH & LƯU DB)
        // ========================================================
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnChooseImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picItem.Image = Image.FromFile(ofd.FileName);
                    // Lưu lại TOÀN BỘ đường dẫn gốc của ảnh trên máy bạn
                    selectedImagePath = ofd.FileName;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Tên món và Giá bán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Giá bán chỉ được nhập số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int catId = Convert.ToInt32(cboCategory.SelectedValue);
                string finalImageName = "";

                // --- LOGIC COPY ẢNH VÀO THƯ MỤC CỦA APP ---
                if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
                {
                    // Lấy đường dẫn thư mục MenuImages của app
                    string imageFolder = Path.Combine(Application.StartupPath, "MenuImages");
                    if (!Directory.Exists(imageFolder)) Directory.CreateDirectory(imageFolder);

                    // Đổi tên ảnh tránh bị trùng lặp
                    finalImageName = "ITEM_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(selectedImagePath);
                    string destPath = Path.Combine(imageFolder, finalImageName);

                    // Thực hiện copy file
                    File.Copy(selectedImagePath, destPath, true);
                }

                // Lưu tên ảnh mới vào Database
                string query = $"INSERT INTO MenuItem (CategoryID, ItemName, Price, Status, ImageUrl, ItemStatus, CreatedAt) " +
                               $"VALUES ({catId}, N'{txtItemName.Text.Replace("'", "''")}', {price}, N'Còn', N'{finalImageName}', 1, GETDATE())";

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);

                MessageBox.Show("Đã thêm món mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}