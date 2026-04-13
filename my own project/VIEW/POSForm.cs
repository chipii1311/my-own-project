using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using my_own_project.BLL;
using my_own_project.DTO;

namespace my_own_project.DesignForms
{
    public partial class POSForm : Form
    {
        public POSForm()
        {
            InitializeComponent();

            // Lệnh này bắt buộc phải nằm dưới InitializeComponent
            // Nó sẽ tự động vẽ bàn ra ngay khi form vừa hiện lên
            LoadTableList();
        }

        #region Xử lý hiển thị Sơ đồ Bàn ăn (Cột Trái)

        void LoadTableList()
        {
            // Tên flpTables này phải khớp y hệt tên cái FlowLayoutPanel bạn đã đặt bên tab [Design] nhé
            flpTables.Controls.Clear();

            try
            {
                // 1. Gọi BLL lấy dữ liệu (Giả sử nhà hàng ID = 1)
                DataTable dtTables = DiningTableBLL.GetTablesByRestaurant(1);

                foreach (DataRow row in dtTables.Rows)
                {
                    // 2. Ép kiểu dữ liệu thành DTO
                    DiningTableDTO table = new DiningTableDTO()
                    {
                        TableID = (int)row["TableID"],
                        RestaurantID = (int)row["RestaurantID"],
                        TableNumber = (int)row["TableNumber"],
                        Capacity = (int)row["Capacity"],
                        Status = row["Status"].ToString()
                    };

                    // 3. Tạo nút (Button)
                    Button btn = new Button() { Width = 100, Height = 100 };

                    // 4. Hiển thị chữ lên nút nhờ hàm ToString() của bạn
                    btn.Text = table.ToString();

                    // 5. Giấu nguyên đối tượng DTO vào túi áo (Tag)
                    btn.Tag = table;

                    // 6. Tô màu theo trạng thái
                    if (table.Status == "Available" || table.Status == "Trống")
                        btn.BackColor = Color.LightGreen;
                    else
                        btn.BackColor = Color.LightPink;

                    // 7. Khai báo sự kiện click
                    btn.Click += btnTable_Click;

                    // 8. Đẩy nút vào giao diện
                    flpTables.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bàn: " + ex.Message);
            }
        }

        // Sự kiện khi thu ngân click vào 1 bàn
        private void btnTable_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            DiningTableDTO selectedTable = clickedButton.Tag as DiningTableDTO;

            MessageBox.Show($"Bạn vừa chọn: Bàn số {selectedTable.TableNumber} \nSức chứa: {selectedTable.Capacity} người \nTrạng thái: {selectedTable.Status}", "Test Nút Bấm");

            // TODO: Hiển thị Hóa Đơn ở đây
        }

        #endregion
    }
}
