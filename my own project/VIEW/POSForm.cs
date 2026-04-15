using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.DesignForms
{
    public partial class POSForm : Form
    {

        private int currentTableID = -1;
        private int currentOrderID = -1;
        public POSForm()
        {
            InitializeComponent();

            // Lệnh này bắt buộc phải nằm dưới InitializeComponent
            // Nó sẽ tự động vẽ bàn ra ngay khi form vừa hiện lên
            LoadTables();
            LoadMenuItems();
        }

        #region Xử lý hiển thị Sơ đồ Bàn ăn (Cột Trái)

        private void LoadTables()
        {
            flpTables.Controls.Clear();

            try
            {
                DataTable dtTables = DiningTableBLL.GetAllTables();

                foreach (DataRow row in dtTables.Rows)
                {
                    Button btn = new Button();
                    btn.Width = 90;
                    btn.Height = 90;

                    int tableID = Convert.ToInt32(row["TableID"]);
                    string tableNum = row["TableNumber"].ToString();

                    // Dữ liệu DB của bạn đang là tiếng Việt, ta lấy thẳng ra luôn
                    // Dùng Trim() để đề phòng trong DB bạn lỡ gõ dư dấu cách
                    string status = row["Status"] != DBNull.Value ? row["Status"].ToString().Trim() : "Trống";

                    btn.Text = "Bàn " + tableNum + Environment.NewLine + "(" + status + ")";
                    btn.Tag = tableID;

                    // Kiểm tra chính xác chữ tiếng Việt trong DB của bạn
                    switch (status)
                    {
                        case "Trống":
                            btn.BackColor = Color.LightGreen;
                            break;
                        case "Đang dùng":
                        case "Có khách": // Tôi thấy Bàn 4 của bạn ghi chữ "Có khách"
                            btn.BackColor = Color.LightCoral;
                            break;
                        case "Đã đặt":
                            btn.BackColor = Color.LightSalmon;
                            break;
                        default:
                            btn.BackColor = Color.LightGray;
                            break;
                    }

                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.Gray;

                    btn.Click += BtnTable_Click;
                    flpTables.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị bàn: " + ex.Message);
            }
        }

        // Hàm xử lý khi Thu ngân click vào 1 Bàn
       private void BtnTable_Click(object sender, EventArgs e)
{
    Button clickedBtn = sender as Button;
    if (clickedBtn != null)
    {
        currentTableID = Convert.ToInt32(clickedBtn.Tag);
        ShowBill(currentTableID); // Tải hóa đơn (nếu có)
    }
}
        #endregion

        // ==========================================
        // 3. KHU VỰC CỘT PHẢI (HÓA ĐƠN)
        // ==========================================
        #region Xử lý hiển thị Hóa Đơn




        private void ShowBill(int tableID)
        {
            lsvBill.Items.Clear();
            decimal totalAmount = 0;
            currentOrderID = -1; // Reset OrderID

            try
            {
                DataTable dtOrders = OrderBLL.GetOrdersByTable(tableID);
                DataRow activeOrder = null;

                foreach (DataRow row in dtOrders.Rows)
                {
                    string status = row["Status"].ToString().Trim();
                    if (!status.Equals("Completed", StringComparison.OrdinalIgnoreCase) &&
                        !status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                    {
                        activeOrder = row;
                        break;
                    }
                }

                // Nếu tìm thấy hóa đơn, mới bắt đầu lôi món ăn ra
                if (activeOrder != null)
                {
                    currentOrderID = Convert.ToInt32(activeOrder["OrderID"]);
                    DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);

                    foreach (DataRow row in dtDetails.Rows)
                    {
                        ListViewItem lvi = new ListViewItem(row["ItemName"].ToString());
                        lvi.SubItems.Add(row["Quantity"].ToString());
                        lvi.SubItems.Add(Convert.ToDecimal(row["UnitPrice"]).ToString("N0"));
                        lvi.SubItems.Add(Convert.ToDecimal(row["SubTotal"]).ToString("N0"));

                        lsvBill.Items.Add(lvi);
                        totalAmount += Convert.ToDecimal(row["SubTotal"]);
                    }
                }

                txtTotalAmount.Text = totalAmount.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị hóa đơn: " + ex.Message);
            }
        }


        #endregion

        // ==========================================
        // 4. KHU VỰC CỘT GIỮA (THỰC ĐƠN MÓN ĂN)
        // ==========================================
        #region Xử lý hiển thị Thực Đơn

        private void LoadMenuItems()
        {
            flpMenu.Controls.Clear();
            DataTable dt = MenuItemBLL.GetAllAvailableItems(); // Giả sử bạn có hàm này trong BLL

            foreach (DataRow row in dt.Rows)
            {
                // 1. Khởi tạo một "Thẻ món ăn" từ bản thiết kế UC
                UCFoodItem uc = new UCFoodItem();

                // 2. Đổ dữ liệu vào thẻ
                uc.SetData(
                    Convert.ToInt32(row["MenuItemID"]),
                    row["ItemName"].ToString(),
                    Convert.ToDecimal(row["Price"]),
                    row["ImageUrl"].ToString()
                );

                // 3. Đăng ký sự kiện: Khi bấm "Thêm" trên thẻ này thì chạy hàm xử lý bên dưới
                uc.OnSelect += Uc_OnSelect;

                // 4. Ném thẻ vào flowLayoutPanel
                flpMenu.Controls.Add(uc);
            }
        }


        private void Uc_OnSelect(object sender, EventArgs e)
        {
            if (currentTableID == -1)
            {
                MessageBox.Show("Chưa chọn bàn!");
                return;
            }

            UCFoodItem uc = (UCFoodItem)sender;
            int quantity = uc.GetQuantity();

            // Thực hiện logic thêm món vào Database y như cũ...
            // Sau khi thêm xong, gọi:
            uc.ResetQuantity();
            ShowBill(currentTableID);
        }

        // Hàm xử lý khi Thu ngân click chọn 1 Món ăn
        private void BtnAddDynamic_Click(object sender, EventArgs e)
        {
            if (currentTableID == -1)
            {
                MessageBox.Show("Vui lòng chọn một Bàn ở cột trái trước khi gọi món!", "Nhắc nhở");
                return;
            }

            Button btn = sender as Button;
            if (btn != null)
            {
                // Khui dữ liệu từ Tag ra
                var itemData = btn.Tag as Tuple<int, decimal, NumericUpDown>;
                int menuItemID = itemData.Item1;
                decimal price = itemData.Item2;
                NumericUpDown nud = itemData.Item3;

                // Lấy chính xác số lượng mà thu ngân vừa gõ/bấm
                int quantity = (int)nud.Value;

                if (quantity == 0) return; // Nếu số lượng = 0 thì không làm gì cả

                try
                {
                    // 1. Tạo Hóa đơn nếu bàn đang trống
                    if (currentOrderID == -1)
                    {
                        OrderDTO newOrder = new OrderDTO();
                        newOrder.TableID = currentTableID;
                        newOrder.RestaurantID = 1;
                        newOrder.OrderType = "DineIn";
                        newOrder.Status = "Pending";
                        newOrder.OrderDate = DateTime.Now;

                        currentOrderID = OrderBLL.CreateOrder(newOrder);

                        DiningTableDTO table = DiningTableBLL.GetTableByID(currentTableID);
                        if (table != null)
                        {
                            table.Status = "Đang dùng";
                            DiningTableBLL.UpdateTable(table);
                            LoadTables();
                        }
                    }

                    // 2. Thêm món vào hóa đơn với Số Lượng tùy chọn
                    OrderDetailDTO detail = new OrderDetailDTO();
                    detail.OrderID = currentOrderID;
                    detail.MenuItemID = menuItemID;
                    detail.Quantity = quantity; // Bắn 10 lon bia hoặc -2 lon bia xuống SQL
                    detail.UnitPrice = price;

                    OrderDetailBLL.AddOrderDetail(detail);

                    // 3. Tải lại bảng Hóa đơn
                    ShowBill(currentTableID);

                    // 4. Trả ô số lượng về lại số 1 để chuẩn bị cho lần bấm tiếp theo
                    nud.Value = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm món: " + ex.Message);
                }
            }
        }
        #endregion
    }
}