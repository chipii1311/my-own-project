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


                // TẠO CỘT NÚT "SỬA" (Nếu chưa có)
                if (dataGridView1.Columns["colEdit"] == null)
                {
                    DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                    btnEdit.Name = "colEdit";
                    btnEdit.HeaderText = "Sửa";
                    btnEdit.Text = "✏️ Sửa"; // Dùng icon emoji cho sang!
                    btnEdit.UseColumnTextForButtonValue = true;
                    btnEdit.Width = 60;
                    dataGridView1.Columns.Add(btnEdit);
                }

                // TẠO CỘT NÚT "KHÓA/XÓA" (Nếu chưa có)
                if (dataGridView1.Columns["colDelete"] == null)
                {
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                    btnDelete.Name = "colDelete";
                    btnDelete.HeaderText = "Trạng thái";
                    btnDelete.Text = "🗑️ Khóa";
                    btnDelete.UseColumnTextForButtonValue = true;
                    btnDelete.Width = 80;
                    dataGridView1.Columns.Add(btnDelete);
                }

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu bấm vào tiêu đề cột (RowIndex = -1) thì bỏ qua, chống lỗi văng app
            if (e.RowIndex < 0) return;

            // Lấy tên của cái cột mà người dùng vừa bấm vào
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;

            // Lấy ID của món ăn ở cái dòng vừa bấm (Để biết đang thao tác với món nào)
            int itemID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

            // XỬ LÝ KHI BẤM NÚT SỬA
            if (colName == "colEdit")
            {
                // TODO: Chút nữa chúng ta sẽ gọi ProductAddForm lên và truyền cái itemID này qua
                MessageBox.Show("Bạn muốn Sửa món có ID: " + itemID);
            }
            // XỬ LÝ KHI BẤM NÚT KHÓA (NGỪNG BÁN)
            else if (colName == "colDelete")
            {
                DialogResult dialog = MessageBox.Show("Bạn có chắc chắn muốn ngừng bán món này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        // Giả sử bạn có hàm UpdateStatus trong BLL để đổi IsAvailable thành false
                        // MenuItemBLL.ChangeStatus(itemID, false);

                        MessageBox.Show("Đã ngừng bán món này!");
                        LoadData(); // Load lại bảng để cập nhật
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        // Sự kiện khi Form vừa mở lên

    }
}
