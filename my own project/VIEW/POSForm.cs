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
                // Gọi BLL của nhóm lấy danh sách bàn
                DataTable dtTables = DiningTableBLL.GetAllTables();

                foreach (DataRow row in dtTables.Rows)
                {
                    Button btn = new Button();
                    btn.Width = 90;
                    btn.Height = 90;

                    int tableID = Convert.ToInt32(row["TableID"]);
                    string tableNum = row["TableNumber"].ToString();
                    string status = row["Status"] != DBNull.Value ? row["Status"].ToString() : "Trống";

                    btn.Text = "Bàn " + tableNum + Environment.NewLine + "(" + status + ")";
                    btn.Tag = tableID;

                    switch (status)
                    {
                        case "Trống":
                            btn.BackColor = Color.LightGreen;
                            break;
                        case "Đang dùng":
                        case "Có khách":
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

                    // Gắn sự kiện click
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
                // Ghi nhớ lại ID của bàn đang chọn
                currentTableID = Convert.ToInt32(clickedBtn.Tag);

                // Hiển thị Hóa đơn của bàn đó
                ShowBill(currentTableID);
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
            currentOrderID = -1;

            try
            {
                // Tìm Order đang mở của bàn này
                DataTable dtOrders = OrderBLL.GetOrdersByTable(tableID);
                DataRow activeOrder = null;

                foreach (DataRow row in dtOrders.Rows)
                {
                    string status = row["Status"].ToString();
                    if (status != "Completed" && status != "Cancelled" && status != "Đã thanh toán")
                    {
                        activeOrder = row;
                        break;
                    }
                }

                // Nếu có Order, lấy chi tiết món ăn hiển thị lên
                if (activeOrder != null)
                {
                    currentOrderID = Convert.ToInt32(activeOrder["OrderID"]);
                    DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);

                    foreach (DataRow row in dtDetails.Rows)
                    {
                        ListViewItem lvi = new ListViewItem(row["ItemName"].ToString());
                        lvi.SubItems.Add(row["Quantity"].ToString());

                        decimal price = Convert.ToDecimal(row["UnitPrice"]);
                        lvi.SubItems.Add(price.ToString("N0"));

                        decimal subTotal = Convert.ToDecimal(row["SubTotal"]);
                        lvi.SubItems.Add(subTotal.ToString("N0"));

                        lsvBill.Items.Add(lvi);
                        totalAmount += subTotal;
                    }
                }

                txtTotalPrice.Text = totalAmount.ToString("N0") + " VNĐ";
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

            try
            {
                // Tạm thời vẫn dùng Sql connection cho nhanh để xem giao diện
                string connString = ConfigurationManager.ConnectionStrings["RestaurantDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT MenuItemID, ItemName, Price, ImageUrl FROM MenuItem WHERE IsAvailable = 1";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Button btn = new Button();
                        btn.Width = 140;
                        btn.Height = 160;
                        btn.BackColor = Color.White;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderColor = Color.LightGray;

                        string name = reader["ItemName"].ToString();
                        string price = Convert.ToDecimal(reader["Price"]).ToString("N0");
                        btn.Text = name + Environment.NewLine + price + "đ";
                        btn.TextAlign = ContentAlignment.BottomCenter;
                        btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                        string imgPath = reader["ImageUrl"].ToString();
                        try
                        {
                            if (!string.IsNullOrEmpty(imgPath) && System.IO.File.Exists(imgPath))
                            {
                                btn.Image = Image.FromFile(imgPath);
                            }
                        }
                        catch { }

                        btn.ImageAlign = ContentAlignment.TopCenter;
                        btn.TextImageRelation = TextImageRelation.ImageAboveText;

                        // Giấu ID món
                        btn.Tag = reader["MenuItemID"];

                        // Gắn sự kiện click
                        btn.Click += BtnMenuItem_Click;

                        flpMenu.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thực đơn: " + ex.Message);
            }
        }

        // Hàm xử lý khi Thu ngân click chọn 1 Món ăn
        private void BtnMenuItem_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int menuItemID = Convert.ToInt32(btn.Tag);
                string itemName = btn.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0];

                // Tạm thời báo thông báo để chương trình không bị lỗi
                MessageBox.Show("Bạn vừa chọn món: " + itemName + ". Tính năng thêm vào Hóa đơn sẽ được làm tiếp theo!");
            }
        }
        #endregion

    }
}