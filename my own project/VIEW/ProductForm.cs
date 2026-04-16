using my_own_project.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.DesignForms
{
    public partial class ProductForm : SampleView
    {

        private DataTable dtProducts;
        public ProductForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            ProductAddForm f = new ProductAddForm();

            // Dòng 2: Lệnh hiển thị form lên màn hình
            f.ShowDialog();
        }

        private void LoadData()
        {
            try
            {
                // Gán dữ liệu lôi từ SQL vào biến toàn cục
                dtProducts = MenuItemBLL.GetAllMenuItems();

                // Đổ dữ liệu vào bảng
                dataGridView1.DataSource = dtProducts;

                // Tinh chỉnh cho bảng đẹp hơn
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;

                // Ẩn 2 cột dữ liệu thô đi (ID Danh mục và Tên file ảnh)
                if (dataGridView1.Columns["CategoryID"] != null)
                    dataGridView1.Columns["CategoryID"].Visible = false;
                if (dataGridView1.Columns["ImageUrl"] != null)
                    dataGridView1.Columns["ImageUrl"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách món: " + ex.Message);
            }
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadData(); 
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            // Nếu dữ liệu chưa tải xong thì không làm gì cả
            if (dtProducts == null) return;

            // Lấy chữ người dùng vừa gõ
            string keyword = txtSearch.Text.Trim();

            // Tạo bộ lọc trên RAM
            DataView dv = dtProducts.DefaultView;

            // Cú pháp lọc: Cột [Name] chứa từ khóa. (Lệnh Replace để chống lỗi gõ dấu nháy đơn)
            dv.RowFilter = string.Format("Name LIKE '%{0}%'", keyword.Replace("'", "''"));

            // Đổ lại kết quả vừa lọc vào bảng
            dataGridView1.DataSource = dv;
        }

        // Sự kiện khi Form vừa mở lên

    }
}
