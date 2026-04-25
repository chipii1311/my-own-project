using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DAL;
using my_own_project.DTO;
using my_own_project.VIEW;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.DesignForms // Đổi lại namespace nếu bạn để ở thư mục khác
{
    public partial class POSForm : Form
    {
        private DataTable dtAllMenu;
        private int currentOrderID = -1; // -1 nghĩa là chưa có hóa đơn nào đang tạo

        // Control khai báo bằng tay để không cần Designer
        private Guna2Panel pnlCart;
        private Guna2Panel pnlHeader;
        private FlowLayoutPanel flpMenu;
        private FlowLayoutPanel flpCategories;
        private Guna2TextBox txtSearch;
        private FlowLayoutPanel flpCart;
        private Label lblTotal;
        private Guna2Button btnContinue;

        public POSForm()
        {
            InitializeModernPOS();
            LoadDiningTables(); // Gọi ở đây để lúc mở lên là có bàn luôn
            LoadMenuItems();
        }

        private void InitializeModernPOS()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Padding = new Padding(20);

            // ==========================================
            // 1. GIỎ HÀNG (PANEL BÊN PHẢI)
            // ==========================================
            pnlCart = new Guna2Panel();
            pnlCart.Dock = DockStyle.Right;
            pnlCart.Width = 350;
            pnlCart.BorderRadius = 25;
            pnlCart.FillColor = Color.White;
            pnlCart.Padding = new Padding(20);

            Label lblCartTitle = new Label();
            lblCartTitle.Text = "Current Order";
            lblCartTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCartTitle.Location = new Point(20, 20);
            lblCartTitle.AutoSize = true;
            lblCartTitle.BackColor = Color.White;
            pnlCart.Controls.Add(lblCartTitle);




            Guna2ComboBox cboTable = new Guna2ComboBox();
            cboTable.Name = "cboTable"; // Đặt tên để lát gọi lấy dữ liệu
            cboTable.Location = new Point(20, 60);
            cboTable.Size = new Size(310, 36);
            cboTable.BorderRadius = 10;
            cboTable.Font = new Font("Segoe UI", 10F);
            cboTable.ForeColor = Color.Black;
            // Tạm thời add tay, lát load từ DB lên sau
            cboTable.Items.Add("Mang đi (Take Away)");
            cboTable.Items.Add("Bàn 1");
            cboTable.Items.Add("Bàn 2");
            cboTable.SelectedIndex = 0; // Mặc định là mang đi
            pnlCart.Controls.Add(cboTable);

            // ListView Hóa đơn
            // Dùng FlowLayoutPanel thay cho ListView cũ
            flpCart = new FlowLayoutPanel();
            flpCart.Location = new Point(20, 110);
            flpCart.Size = new Size(310, 360); // Bóp chiều cao lại một xíu
            flpCart.AutoScroll = true;
            flpCart.FlowDirection = FlowDirection.TopDown;
            flpCart.WrapContents = false;
            pnlCart.Controls.Add(flpCart);
            // Tổng tiền
            lblTotal = new Label();
            lblTotal.Text = "Total: 0 đ";
            lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotal.Location = new Point(20, 490);
            lblTotal.AutoSize = true;
            lblTotal.BackColor = Color.White;
            pnlCart.Controls.Add(lblTotal);

            // Nút Thanh Toán (Continue)
            btnContinue = new Guna2Button();
            btnContinue.Text = "Continue";
            btnContinue.BorderRadius = 20;
            btnContinue.Size = new Size(310, 55);
            btnContinue.Location = new Point(20, 550);
            btnContinue.FillColor = Color.FromArgb(88, 28, 230); // Màu tím chuẩn thiết kế
            btnContinue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnContinue.Click += BtnContinue_Click;
            pnlCart.Controls.Add(btnContinue);

            // ==========================================
            // 2. HEADER (TÌM KIẾM & DANH MỤC)
            // ==========================================
            pnlHeader = new Guna2Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 120;
            pnlHeader.BackColor = Color.Transparent;

            Label lblPageTitle = new Label();
            lblPageTitle.Text = "Items";
            lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(88, 28, 230);
            lblPageTitle.Location = new Point(10, 10);
            pnlHeader.Controls.Add(lblPageTitle);

            txtSearch = new Guna2TextBox();
            txtSearch.Size = new Size(400, 45);
            txtSearch.Location = new Point(10, 50);
            txtSearch.BorderRadius = 20;
            txtSearch.PlaceholderText = "Search items...";
            txtSearch.TextChanged += TxtSearch_TextChanged;
            pnlHeader.Controls.Add(txtSearch);

            flpCategories = new FlowLayoutPanel();
            flpCategories.Location = new Point(430, 50);
            flpCategories.Size = new Size(600, 50);
            pnlHeader.Controls.Add(flpCategories);

            // ==========================================
            // 3. MENU (DANH SÁCH MÓN ĂN)
            // ==========================================
            flpMenu = new FlowLayoutPanel();
            flpMenu.Dock = DockStyle.Fill;
            flpMenu.AutoScroll = true;
            flpMenu.Padding = new Padding(10, 20, 10, 10);

            // RÁP VÀO FORM
            this.Controls.Add(flpMenu);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlCart); // Nằm bên phải
        }

        // ==========================================
        // CÁC HÀM XỬ LÝ LOGIC (ĐÃ BẢO TỒN)
        // ==========================================
        private void LoadMenuItems()
        {
            dtAllMenu = MenuItemBLL.GetAllAvailableItems();
            LoadCategories();
            FilterMenu(0, "");
        }

        private void LoadCategories()
        {
            flpCategories.Controls.Clear();
            Guna2Button btnAll = CreateCatButton("All", 0);
            btnAll.Checked = true;
            flpCategories.Controls.Add(btnAll);

            try
            {
                DataTable dtCategories = CategoryDAL.GetAll();
                foreach (DataRow row in dtCategories.Rows)
                {
                    flpCategories.Controls.Add(CreateCatButton(row["CategoryName"].ToString(), Convert.ToInt32(row["CategoryID"])));
                }
            }
            catch { }
        }

        private Guna2Button CreateCatButton(string text, int tag)
        {
            Guna2Button btn = new Guna2Button();
            btn.Text = text;
            btn.Size = new Size(100, 40);
            btn.BorderRadius = 20; // Hình viên thuốc
            btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btn.Tag = tag;
            btn.Cursor = Cursors.Hand;
            btn.FillColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.CheckedState.FillColor = Color.FromArgb(30, 30, 30); // Màu đen tuyền khi chọn giống ảnh
            btn.CheckedState.ForeColor = Color.White;
            btn.Click += CategoryButton_Click;
            return btn;
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            int catID = Convert.ToInt32(((Guna2Button)sender).Tag);
            FilterMenu(catID, txtSearch.Text);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterMenu(0, txtSearch.Text); // Bỏ qua lọc category khi search để tìm nhanh
        }

        private void FilterMenu(int categoryID, string keyword)
        {
            flpMenu.Controls.Clear();
            if (dtAllMenu == null) return;

            DataView dv = new DataView(dtAllMenu);
            string filterStr = "";

            if (categoryID > 0) filterStr = $"CategoryID = {categoryID}";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (filterStr != "") filterStr += " AND ";
                filterStr += $"ItemName LIKE '%{keyword}%'";
            }

            dv.RowFilter = filterStr;

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                my_own_project.UCFoodItem uc = new my_own_project.UCFoodItem();
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

        // ==========================================
        // LOGIC THÊM MÓN & TẠO HÓA ĐƠN TRỰC TIẾP
        // ==========================================
        private void Uc_OnSelect(object sender, EventArgs e)
        {
            my_own_project.UCFoodItem uc = (my_own_project.UCFoodItem)sender;
            int menuItemID = uc.FoodID;
            decimal price = uc.Price;
            int quantity = uc.GetQuantity();

            if (quantity == 0) return;

            try
            {
                // Tự động tạo hóa đơn mới nếu chưa có
                if (currentOrderID == -1)
                {
                    // === ĐOẠN CODE MỚI: LẤY BÀN TỪ COMBOBOX ===
                    Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls["cboTable"];
                    object selectedTableID = cboTable.SelectedValue;

                    OrderDTO newOrder = new OrderDTO();

                    // Nếu value là DBNull (tức là chọn "Mang đi"), thì set TableID = null
                    newOrder.TableID = (selectedTableID == DBNull.Value) ? (int?)null : Convert.ToInt32(selectedTableID);

                    // Mang đi thì OrderType = TakeAway, có bàn thì là DineIn
                    newOrder.OrderType = (newOrder.TableID == null) ? "TakeAway" : "DineIn";
                    newOrder.Status = "Pending";
                    newOrder.OrderDate = DateTime.Now;

                    currentOrderID = OrderBLL.CreateOrder(newOrder);

                    // Khóa ComboBox lại, cấm đổi bàn khi đã bắt đầu order món
                    cboTable.Enabled = false;
                }

                // Thêm chi tiết món
                OrderDetailDTO detail = new OrderDetailDTO();
                detail.OrderID = currentOrderID;
                detail.MenuItemID = menuItemID;
                detail.Quantity = quantity;
                detail.UnitPrice = price;

                OrderDetailBLL.AddOrderDetail(detail);

                // Cập nhật lại giỏ hàng
                ShowBill();
                uc.ResetQuantity();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món: " + ex.Message);
            }
        }

        private void ShowBill()
        {
            flpCart.Controls.Clear();
            decimal subTotal = 0;

            if (currentOrderID != -1)
            {
                DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
                foreach (DataRow row in dtDetails.Rows)
                {
                    int detailID = Convert.ToInt32(row["OrderDetailID"]);
                    string name = row["ItemName"].ToString();
                    int qty = Convert.ToInt32(row["Quantity"]);
                    decimal price = Convert.ToDecimal(row["UnitPrice"]);
                    decimal rowTotal = Convert.ToDecimal(row["SubTotal"]);

                    // 1. Bảng chứa 1 dòng món ăn (Cao lên một chút để chứa ảnh)
                    Guna2Panel pnlItem = new Guna2Panel();
                    pnlItem.Size = new Size(290, 70);
                    pnlItem.CustomBorderThickness = new Padding(0, 0, 0, 1);
                    pnlItem.CustomBorderColor = Color.FromArgb(240, 240, 240);

                    // 2. ẢNH THUMBNAIL MÓN ĂN (Đã fix lỗi dấu X đỏ)
                    Guna2PictureBox picThumb = new Guna2PictureBox();
                    picThumb.Size = new Size(50, 50);
                    picThumb.Location = new Point(0, 10);
                    picThumb.BorderRadius = 10; // Bo góc ảnh xịn xò
                    picThumb.SizeMode = PictureBoxSizeMode.Zoom;
                    picThumb.ErrorImage = null;   // BÙA TRỊ DẤU X ĐỎ 
                    picThumb.InitialImage = null; // BÙA TRỊ DẤU X ĐỎ 

                    // Lấy ảnh hiển thị lên (Nếu database có lưu tên ảnh)
                    if (row.Table.Columns.Contains("ImageUrl") && row["ImageUrl"] != DBNull.Value)
                    {
                        string imgUrl = row["ImageUrl"].ToString();
                        string imagePath = System.IO.Path.Combine(Application.StartupPath, "MenuImages", imgUrl);
                        if (System.IO.File.Exists(imagePath)) picThumb.ImageLocation = imagePath;
                    }
                    pnlItem.Controls.Add(picThumb);

                    // 3. TÊN VÀ GIÁ TIỀN 
                    Label lblName = new Label { Text = name, Location = new Point(60, 10), Font = new Font("Segoe UI Semibold", 10F), AutoSize = true };
                    Label lblPrice = new Label { Text = price.ToString("N0") + "đ", Location = new Point(60, 35), Font = new Font("Segoe UI", 9.5F), ForeColor = Color.DimGray, AutoSize = true };

                    // 4. NÚT TRỪ [-] (Dùng Guna2Button ép góc thành hình tròn, fix nuốt chữ)
                    Guna2Button btnMinus = new Guna2Button();
                    btnMinus.Size = new Size(30, 30);
                    btnMinus.BorderRadius = 13; // Bí quyết: 26 chia 2 = 13 -> Hình tròn tuyệt đối
                    btnMinus.Location = new Point(190, 22);
                    btnMinus.FillColor = Color.Gray;
                    btnMinus.ForeColor = Color.White;
                    btnMinus.Text = "-";
                    btnMinus.Font = new Font("Consolas", 11F, FontStyle.Bold); // Consolas hiển thị dấu toán học chuẩn nhất
                    btnMinus.Padding = new Padding(0);
                    btnMinus.Cursor = Cursors.Hand;

                    // SỰ KIỆN BẤM NÚT TRỪ
                    btnMinus.Click += (s, e) => {
                        if (qty > 1)
                        {
                            string query = $"UPDATE OrderDetail SET Quantity = Quantity - 1 WHERE OrderDetailID = {detailID}";
                            my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                        }
                        else
                        {
                            string query = $"DELETE FROM OrderDetail WHERE OrderDetailID = {detailID}";
                            my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                        }
                        ShowBill();
                    };



                    // SỐ LƯỢNG
                    Label lblQty = new Label { Text = qty.ToString(), Location = new Point(223, 24), Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true };

                    // 5. NÚT CỘNG [+] (Dùng Guna2Button ép góc)
                    Guna2Button btnPlus = new Guna2Button();
                    btnPlus.Size = new Size(30, 30);
                    btnPlus.BorderRadius = 13;
                    btnPlus.Location = new Point(250, 22);
                    btnPlus.FillColor = Color.Black;
                    btnPlus.ForeColor = Color.White;
                    btnPlus.Text = "+";
                    btnPlus.Font = new Font("Consolas", 11F, FontStyle.Bold);
                    btnPlus.Padding = new Padding(0); // Tắt lề tàng hình
                    btnPlus.Cursor = Cursors.Hand;

                    btnPlus.Click += (s, e) => {
                        string query = $"UPDATE OrderDetail SET Quantity = Quantity + 1 WHERE OrderDetailID = {detailID}";
                        my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                        ShowBill();
                    };

                    // Gắn tất cả vào Panel
                    pnlItem.Controls.Add(picThumb);
                    pnlItem.Controls.Add(lblName);
                    pnlItem.Controls.Add(lblPrice);
                    pnlItem.Controls.Add(btnMinus);
                    pnlItem.Controls.Add(lblQty);
                    pnlItem.Controls.Add(btnPlus);

                    flpCart.Controls.Add(pnlItem);
                    subTotal += rowTotal;
                }
            }
            lblTotal.Text = "Total: " + subTotal.ToString("N0") + " đ";
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (currentOrderID == -1)
            {
                MessageBox.Show("Giỏ hàng đang trống!");
                return;
            }

            PaymentForm frm = new PaymentForm(currentOrderID, -1);
            frm.ShowDialog();

            // Thanh toán xong thì Reset giỏ hàng
            currentOrderID = -1;
            ShowBill();

            // === THÊM 2 DÒNG NÀY ĐỂ MỞ KHÓA LẠI COMBOBOX ===
            Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls["cboTable"];
            if (cboTable != null) cboTable.Enabled = true;
        }


        private void LoadDiningTables()
        {
            try
            {
                // 1. Gọi SP lấy toàn bộ danh sách bàn
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_DiningTable_GetAll");

                // 2. Tạo thêm một cột ảo để hiển thị tên bàn cho thân thiện (VD: "Bàn 1", "Bàn 2")
                dt.Columns.Add("TableDisplay", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["TableDisplay"] = "Bàn " + row["TableNumber"].ToString();
                }

                // 3. Chèn lựa chọn "Mang đi" vào dòng đầu tiên (ID = null)
                DataRow dr = dt.NewRow();
                dr["TableID"] = DBNull.Value;
                dr["TableDisplay"] = "Mang đi (Take Away)";
                dt.Rows.InsertAt(dr, 0);

                // 4. Đổ dữ liệu vào ComboBox (Giả sử bạn đã đặt tên cboTable trong InitializeModernPOS)
                Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls["cboTable"];
                if (cboTable != null)
                {
                    cboTable.DataSource = dt;
                    cboTable.DisplayMember = "TableDisplay"; // Cái hiện ra cho người ta đọc
                    cboTable.ValueMember = "TableID";       // Cái ID ngầm để lưu xuống DB
                    cboTable.SelectedIndex = 0;             // Mặc định chọn dòng đầu (Mang đi)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bàn: " + ex.Message);
            }
        }
    }
}