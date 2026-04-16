using my_own_project.BLL;
using my_own_project.DAL;
using my_own_project.DTO;
using my_own_project.Helpers;
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

        // Khai báo 2 biến toàn cục để lưu dữ liệu và trạng thái lọc
        private DataTable dtAllMenu;
        private int selectedCategoryID = 0; // 0 nghĩa là đang chọn "Tất cả"

        private int currentTableID = -1;
        private int currentOrderID = -1;
        private ContextMenuStrip tableMenu;

        public POSForm()
        {
            InitializeComponent();

            // 1. Phải tạo cái Menu chuột phải RA TRƯỚC
            InitializeTableMenu();

            // 2. Sau đó mới gọi hàm xây Bàn (để bàn có cái mà gán vào)
            LoadTables();

            // 3. Cuối cùng tải món ăn
            LoadMenuItems();
            flpMenu.Enabled = false;
        }

        private void InitializeTableMenu()
        {
            tableMenu = new ContextMenuStrip();

            // Thêm các tuỳ chọn trạng thái (Tên hiển thị, Icon, Hàm xử lý sự kiện)
            tableMenu.Items.Add("Trống", null, ChangeTableStatus_Click);
            tableMenu.Items.Add("Đã đặt", null, ChangeTableStatus_Click);
            tableMenu.Items.Add("Bảo trì", null, ChangeTableStatus_Click);
        }

        // Hàm xử lý khi Thu ngân bấm chọn 1 trạng thái trong Menu
        private void ChangeTableStatus_Click(object sender, EventArgs e)
        {
            // Tìm xem thu ngân vừa click vào trạng thái nào
            ToolStripItem clickedItem = sender as ToolStripItem;

            // Tìm xem cái Menu đó đang được mở ra từ cái Bàn (Button) nào
            ContextMenuStrip menu = clickedItem.Owner as ContextMenuStrip;
            Button btnTable = menu.SourceControl as Button;

            if (btnTable != null)
            {
                int tableID = Convert.ToInt32(btnTable.Tag);
                string newStatus = clickedItem.Text; // Chữ "Đã đặt", "Trống",...

                try
                {
                    // Lôi bàn đó từ Database lên và đổi trạng thái
                    DiningTableDTO table = DiningTableBLL.GetTableByID(tableID);
                    if (table != null)
                    {
                        // Ràng buộc nhỏ: Nếu bàn đang có người ăn thì không cho đổi lung tung
                        if (table.Status.Trim() == "Đang dùng" || table.Status.Trim() == "Có khách")
                        {
                            MessageBox.Show("Bàn đang có khách, không thể đổi trạng thái thủ công! Vui lòng thanh toán để trống bàn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        table.Status = newStatus;
                        DiningTableBLL.UpdateTable(table);

                        LoadTables(); // Load lại để cập nhật màu sắc ngay lập tức
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đổi trạng thái bàn: " + ex.Message);
                }
            }
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
                    btn.ContextMenuStrip = tableMenu;

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

                // --- 1. TẠO HIỆU ỨNG HIGHLIGHT CHO BÀN ---
                // Quét qua tất cả các bàn, trả viền về bình thường (1px màu xám mờ)
                foreach (Control ctrl in flpTables.Controls)
                {
                    if (ctrl is Button)
                    {
                        Button btn = (Button)ctrl;
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.Gray;
                    }
                }

                // Trét viền thật đậm cho cái Bàn vừa bị click (3px màu Xanh dương)
                clickedBtn.FlatAppearance.BorderSize = 3;
                clickedBtn.FlatAppearance.BorderColor = Color.Blue;

                // --- 2. CẬP NHẬT TÊN BÀN LÊN HÓA ĐƠN ---
                // Cắt lấy đoạn "Bàn 1", "Bàn 2" (bỏ đi chữ "Trống" hay "Đang dùng" ở dòng dưới)
                string tableName = clickedBtn.Text.Replace("\r", "").Split('\n')[0];
                lblTableName.Text = "Đang phục vụ: " + tableName;

                // --- 3. MỞ KHÓA MENU ĐỂ BẮT ĐẦU GỌI MÓN ---
                flpMenu.Enabled = true;

                // --- 4. TẢI HÓA ĐƠN TỪ SQL LÊN ---
                ShowBill(currentTableID);

                // --- 5. LOGIC ẨN HIỆN NÚT VÀ KHÓA MENU ---
                string status = clickedBtn.Text; // Lấy text của nút để xét

                if (status.Contains("Trống"))
                {
                    // Bàn trống thì khóa Menu lại, bắt phải bấm Mở bàn mới được gọi món
                    flpMenu.Enabled = false;
                    btnMoBan.Visible = true;
                    btnThanhToan.Visible = false;
                }
                else if (status.Contains("Đang dùng") || status.Contains("Có khách"))
                {
                    // Bàn đang có khách thì mở Menu ra cho gọi thêm món
                    flpMenu.Enabled = true;
                    btnMoBan.Visible = false;
                    btnThanhToan.Visible = true;
                }
                else // Các trạng thái khác như Đã đặt, Bảo trì...
                {
                    flpMenu.Enabled = false;
                    btnMoBan.Visible = false;
                    btnThanhToan.Visible = false;
                }
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
            decimal subTotal = 0; // Tạm tính (Tiền hàng)
            currentOrderID = -1;

            // 1. Reset trắng các thông tin trên giao diện
            lblOrderID.Text = "Mã HĐ: ---";
            lblCheckInTime.Text = "Giờ vào: ---";     

            // Tự động lấy tên người dùng đã đăng nhập (Giống bên MainForm của bạn)
            if (CurrentUser.IsLoggedIn)
            {
                lblStaff.Text = "Thu ngân: " + CurrentUser.FullName;
            }
            else
            {
                lblStaff.Text = "Thu ngân: Guest";
            }

            lblSubTotal.Text = "0 đ";
            lblVAT.Text = "0 đ";
            lblFinalTotal.Text = "0 đ";

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

                // Nếu bàn này đang có khách và có Hóa đơn
                if (activeOrder != null)
                {
                    currentOrderID = Convert.ToInt32(activeOrder["OrderID"]);

                    // --- ĐỔ DỮ LIỆU LÊN HEADER ---
                    lblOrderID.Text = "Mã HĐ: HĐ-" + currentOrderID.ToString("D5");

                    if (activeOrder["OrderDate"] != DBNull.Value)
                    {
                        DateTime checkIn = Convert.ToDateTime(activeOrder["OrderDate"]);
                        lblCheckInTime.Text = "Giờ vào: " + checkIn.ToString("HH:mm - dd/MM");
                    }

                    // --- LẤY DANH SÁCH MÓN ĂN ---
                    DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);

                    foreach (DataRow row in dtDetails.Rows)
                    {
                        ListViewItem lvi = new ListViewItem(row["ItemName"].ToString());
                        lvi.SubItems.Add(row["Quantity"].ToString());
                        lvi.SubItems.Add(Convert.ToDecimal(row["UnitPrice"]).ToString("N0"));

                        decimal rowTotal = Convert.ToDecimal(row["SubTotal"]);
                        lvi.SubItems.Add(rowTotal.ToString("N0"));

                        lsvBill.Items.Add(lvi);
                        subTotal += rowTotal; // Cộng dồn tiền hàng
                    }

                    // --- TÍNH TOÁN FOOTER (VAT & TỔNG TIỀN) ---
                    decimal vat = subTotal * 0.08m; // VAT 8%
                    decimal finalTotal = subTotal + vat;

                    lblSubTotal.Text = subTotal.ToString("N0") + " đ";
                    lblVAT.Text = vat.ToString("N0") + " đ (8%)";
                    lblFinalTotal.Text = finalTotal.ToString("N0") + " đ";
                }
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
            // 1. Chỉ gọi Database đúng 1 lần duy nhất để lấy toàn bộ món ăn
            dtAllMenu = MenuItemBLL.GetAllAvailableItems();

            // 2. Tải các nút Danh mục (Category)
            LoadCategories();

            // 3. Hiển thị toàn bộ món ăn lên màn hình (Mặc định ID = 0, Từ khóa = rỗng)
            FilterMenu(0, "");
        }


        // ==========================================================
        // HÀM XỬ LÝ KHI BẤM NÚT "THÊM" TRÊN TỪNG THẺ MÓN ĂN
        // ==========================================================
        private void Uc_OnSelect(object sender, EventArgs e)
        {
            if (currentTableID == -1)
            {
                MessageBox.Show("Vui lòng chọn một Bàn ở cột trái trước khi gọi món!", "Nhắc nhở");
                return;
            }

            // Lấy cái thẻ món ăn (UCFoodItem) vừa bị bấm
            UCFoodItem uc = (UCFoodItem)sender;

            // Lấy thông tin ID, Giá và Số lượng từ thẻ đó ra
            int menuItemID = uc.FoodID;
            decimal price = uc.Price;
            int quantity = uc.GetQuantity();

            // Nếu thu ngân gõ số 0 rồi bấm Thêm thì bỏ qua
            if (quantity == 0) return;

            try
            {
                // 1. TẠO HÓA ĐƠN MỚI (Nếu bàn đang trống)
                if (currentOrderID == -1)
                {
                    OrderDTO newOrder = new OrderDTO();
                    newOrder.TableID = currentTableID;
                    newOrder.RestaurantID = 1;
                    newOrder.OrderType = "DineIn";
                    newOrder.Status = "Pending";
                    newOrder.OrderDate = DateTime.Now;

                    currentOrderID = OrderBLL.CreateOrder(newOrder);

                    // Đổi màu bàn sang đỏ (Đang dùng)
                    DiningTableDTO table = DiningTableBLL.GetTableByID(currentTableID);
                    if (table != null)
                    {
                        table.Status = "Đang dùng";
                        DiningTableBLL.UpdateTable(table);
                        LoadTables();
                    }
                }

                // 2. THÊM MÓN VÀO DATABASE
                OrderDetailDTO detail = new OrderDetailDTO();
                detail.OrderID = currentOrderID;
                detail.MenuItemID = menuItemID;
                detail.Quantity = quantity; // Bắn số dương (cộng) hoặc âm (trừ) xuống SQL
                detail.UnitPrice = price;

                OrderDetailBLL.AddOrderDetail(detail);

                // 3. CẬP NHẬT LẠI GIAO DIỆN
                ShowBill(currentTableID); // Tải lại bảng hóa đơn bên phải
                uc.ResetQuantity();       // Trả ô chọn số lượng trên thẻ về lại số 1
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món: " + ex.Message);
            }
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
        #region Xử lý hiển thị Thực Đơn

        // Hàm này giờ chỉ đóng vai trò "khởi động" lúc mới mở Form
        

        // ==========================================
        // TẠO CÁC NÚT DANH MỤC ĐỘNG
        // ==========================================
        private void LoadCategories()
        {
            flpCategories.Controls.Clear();

            // Tạo nút "Tất cả" mặc định nằm ở đầu tiên
            Button btnAll = new Button();
            btnAll.Text = "Tất cả";
            btnAll.Width = 80; btnAll.Height = 35;
            btnAll.Tag = 0;
            btnAll.BackColor = Color.LightSkyBlue; // Nút đang chọn có màu xanh
            btnAll.FlatStyle = FlatStyle.Flat;
            btnAll.Click += CategoryButton_Click;
            flpCategories.Controls.Add(btnAll);

            try
            {
                // Gọi BLL/DAL lấy danh sách Category (Giả sử bạn có hàm CategoryDAL.GetAll())
                DataTable dtCategories = CategoryDAL.GetAll();

                foreach (DataRow row in dtCategories.Rows)
                {
                    Button btn = new Button();
                    btn.Text = row["CategoryName"].ToString();
                    btn.Width = 80; btnAll.Height = 35;
                    btn.Tag = Convert.ToInt32(row["CategoryID"]);
                    btn.BackColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Click += CategoryButton_Click;

                    flpCategories.Controls.Add(btn);
                }
            }
            catch { /* Bỏ qua nếu chưa có bảng Category */ }
        }

        // Sự kiện khi thu ngân bấm vào một Nút Danh Mục
        private void CategoryButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                selectedCategoryID = Convert.ToInt32(btn.Tag);

                // Đổi màu: Nút được bấm thành Xanh, các nút khác về Trắng
                foreach (Control ctrl in flpCategories.Controls)
                {
                    ctrl.BackColor = Color.White;
                }
                btn.BackColor = Color.LightSkyBlue;

                // Tiến hành lọc lại món ăn bên dưới
                FilterMenu(selectedCategoryID, txtSearch.Text);
            }
        }

        // ==========================================
        // SỰ KIỆN TÌM KIẾM THEO TÊN (LIVE SEARCH)
        // ==========================================
        // (Nhớ click đúp vào txtSearch trên Form Design để VS tự tạo hàm này)
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterMenu(selectedCategoryID, txtSearch.Text);
        }

        // ==========================================
        // CỖ MÁY LỌC MÓN ĂN SIÊU TỐC
        // ==========================================
        private void FilterMenu(int categoryID, string keyword)
        {
            flpMenu.Controls.Clear();

            // Nếu dữ liệu chưa tải xong thì không làm gì cả
            if (dtAllMenu == null) return;

            // Sử dụng DataView để lọc trực tiếp trên RAM, không cần gọi SQL
            DataView dv = new DataView(dtAllMenu);
            string filterStr = "";

            // Nếu đang chọn một danh mục cụ thể (khác "Tất cả")
            if (categoryID > 0)
            {
                filterStr = $"CategoryID = {categoryID}";
            }

            // Nếu có gõ chữ vào ô tìm kiếm
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (filterStr != "") filterStr += " AND ";
                // Lọc gần đúng, chứa chữ gõ vào là hiện lên
                filterStr += $"ItemName LIKE '%{keyword}%'";
            }

            dv.RowFilter = filterStr;

            // Vẽ lại các thẻ món ăn từ kết quả vừa lọc được
            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                UCFoodItem uc = new UCFoodItem();

                uc.SetData(
                    Convert.ToInt32(row["MenuItemID"]),
                    row["ItemName"].ToString(),
                    Convert.ToDecimal(row["Price"]),
                    row["ImageUrl"].ToString()
                );

                uc.OnSelect += Uc_OnSelect;
                flpMenu.Controls.Add(uc);
            }
        }


        #endregion

        private void lsvBill_SizeChanged(object sender, EventArgs e)
        {
            // Đảm bảo bảng phải có ít nhất 4 cột thì mới chạy
            if (lsvBill.Columns.Count >= 4)
            {
                // Tính tổng chiều rộng của 3 cột cố định (Số lượng, Đơn giá, Thành tiền)
                int fixedWidth = lsvBill.Columns[1].Width + lsvBill.Columns[2].Width + lsvBill.Columns[3].Width;

                // Trừ hao khoảng 25px cho thanh cuộn dọc (Scrollbar) để không bị xuất hiện thanh cuộn ngang xấu xí
                int scrollBarWidth = 25;

                // Ép cột "Tên món" (cột số 0) giãn ra bằng phần đất còn lại
                lsvBill.Columns[0].Width = lsvBill.Width - fixedWidth - scrollBarWidth;
            }
        }

        private void btnMoBan_Click(object sender, EventArgs e)
        {
            if (currentTableID == -1) return;

            try
            {
                // 1. Tạo mới một Hóa đơn (Order) rỗng, chưa có món ăn
                OrderDTO newOrder = new OrderDTO();
                newOrder.TableID = currentTableID;
                newOrder.RestaurantID = 1;
                newOrder.OrderType = "DineIn";
                newOrder.Status = "Pending";
                newOrder.OrderDate = DateTime.Now;

                currentOrderID = OrderBLL.CreateOrder(newOrder);

                // 2. Cập nhật trạng thái Bàn sang "Đang dùng"
                DiningTableDTO table = DiningTableBLL.GetTableByID(currentTableID);
                if (table != null)
                {
                    table.Status = "Đang dùng";
                    DiningTableBLL.UpdateTable(table);
                }

                // 3. Tải lại giao diện
                LoadTables(); // Đổi bàn thành màu Đỏ
                ShowBill(currentTableID); // Hiển thị mã HĐ và Giờ vào
                flpMenu.Enabled = true;   // Mở khóa Menu để bắt đầu chọn món

                // 4. Tráo đổi 2 nút bấm
                btnMoBan.Visible = false;
                btnThanhToan.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở bàn: " + ex.Message);
            }
        }
    }
}
    