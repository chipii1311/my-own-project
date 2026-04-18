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
            ProductAddForm form = new ProductAddForm();
            form.editItemID = -1; // -1 báo hiệu là chế độ THÊM MỚI
            form.ShowDialog();

            LoadData(); // Load lại bảng sau khi tắt form
        }

        private void LoadData()
        {
            try
            {
                dtProducts = MenuItemBLL.GetAllMenuItems();
                dataGridView1.DataSource = dtProducts;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;

                // 1. Ẩn các cột dữ liệu thô
                if (dataGridView1.Columns["CategoryID"] != null)
                    dataGridView1.Columns["CategoryID"].Visible = false;

                if (dataGridView1.Columns["ImageUrl"] != null)
                    dataGridView1.Columns["ImageUrl"].Visible = false;

                // (ĐÃ XÓA dòng ẩn cột IsAvailable cũ ở đây)

                // 2. THÊM MỚI: Đổi tên tiêu đề cột ItemStatus thành tiếng Việt
                if (dataGridView1.Columns["ItemStatus"] != null)
                {
                    dataGridView1.Columns["ItemStatus"].HeaderText = "Trạng thái";
                }


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
            if (e.RowIndex < 0) return;

            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            int itemID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

            // NẾU BẤM NÚT SỬA
            if (colName == "colEdit")
            {
                ProductAddForm editForm = new ProductAddForm();

                // TRUYỀN ID CỦA MÓN ĐANG CHỌN QUA FORM KIA
                editForm.editItemID = itemID;

                editForm.ShowDialog();
                LoadData(); // F5 lại bảng sau khi sửa xong
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra xem có đúng là đang vẽ cột "ItemStatus" không
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ItemStatus" && e.Value != null)
            {
                int status = Convert.ToInt32(e.Value);

                // Ép kiểu chữ in đậm cho dễ nhìn
                e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);

                if (status == 1)
                {
                    e.Value = "Đang phục vụ";
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (status == 2)
                {
                    e.Value = "Tạm hết";
                    e.CellStyle.ForeColor = Color.DarkOrange;
                }
                else if (status == 0)
                {
                    e.Value = "Ngừng bán";
                    e.CellStyle.ForeColor = Color.Red;
                }

                e.FormattingApplied = true; // Báo cho C# biết là "Tô màu xong rồi, đừng tự vẽ số nữa"
            }
        }

        // Sự kiện khi Form vừa mở lên

    }
}
