using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DAL;
using my_own_project.DTO;
using my_own_project.VIEW;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.DesignForms
{
    public partial class POSForm : Form
    {
        // ========================================================
        // KHAI BÁO BIẾN TOÀN CỤC
        // ========================================================
        private DataTable dtAllMenu;
        private int currentOrderID = -1;

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
            InitializeModernPOS(); // Vẽ giao diện

            // Xử lý dữ liệu ban đầu
            LoadDiningTables();
            LoadMenuItems();
        }

        // ========================================================
        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
        // ========================================================

        private void InitializeModernPOS()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Padding = new Padding(20);

            // --- 1. GIỎ HÀNG ---
            pnlCart = new Guna2Panel();
            pnlCart.Dock = DockStyle.Right;
            pnlCart.Width = 500;
            pnlCart.FillColor = Color.White;
            pnlCart.CustomBorderThickness = new Padding(1, 0, 0, 0);
            pnlCart.CustomBorderColor = Color.FromArgb(235, 235, 235);

            Label lblCartTitle = new Label();
            lblCartTitle.Text = "CHI TIẾT HÓA ĐƠN";
            lblCartTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblCartTitle.Location = new Point(20, 20);
            lblCartTitle.AutoSize = true;
            lblCartTitle.BackColor = Color.White;
            pnlCart.Controls.Add(lblCartTitle);

            Guna2ComboBox cboTable = new Guna2ComboBox();
            cboTable.Name = "cboTable";
            cboTable.Location = new Point(280, 15);
            cboTable.Size = new Size(200, 36);
            cboTable.BorderRadius = 5;
            cboTable.Font = new Font("Segoe UI", 10F);
            pnlCart.Controls.Add(cboTable);

            Guna2Panel pnlColHeader = new Guna2Panel();
            pnlColHeader.Location = new Point(20, 70);
            pnlColHeader.Size = new Size(460, 30);
            pnlColHeader.CustomBorderThickness = new Padding(0, 0, 0, 1);
            pnlColHeader.CustomBorderColor = Color.LightGray;
            pnlColHeader.BackColor = Color.White;

            pnlColHeader.Controls.Add(new Label { Text = "STT", Location = new Point(0, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Tên món", Location = new Point(40, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "SL", Location = new Point(210, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Đơn giá", Location = new Point(280, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Thành tiền", Location = new Point(370, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlCart.Controls.Add(pnlColHeader);

            flpCart = new FlowLayoutPanel();
            flpCart.Location = new Point(20, 105);
            flpCart.Size = new Size(470, 380);
            flpCart.AutoScroll = true;
            flpCart.FlowDirection = FlowDirection.TopDown;
            flpCart.WrapContents = false;
            pnlCart.Controls.Add(flpCart);

            Label lblSubTotalTitle = new Label { Text = "Tạm tính", Font = new Font("Segoe UI", 10F), Location = new Point(20, 500), AutoSize = true, BackColor = Color.White };
            Label lblTotalTitle = new Label { Text = "Tổng cộng", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(20, 540), AutoSize = true, BackColor = Color.White };
            pnlCart.Controls.Add(lblSubTotalTitle);
            pnlCart.Controls.Add(lblTotalTitle);

            lblTotal = new Label { Name = "lblTotalAmount", Text = "0 đ", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(310, 540), Size = new Size(170, 30), TextAlign = ContentAlignment.MiddleRight, BackColor = Color.White };
            pnlCart.Controls.Add(lblTotal);

            // Gắn sự kiện cho các nút
            Guna2Button btnClear = new Guna2Button { Text = "Xóa tất cả", BorderRadius = 5, Size = new Size(100, 45), Location = new Point(20, 590), FillColor = Color.White, ForeColor = Color.Red, CustomBorderThickness = new Padding(1), CustomBorderColor = Color.Red, Font = new Font("Segoe UI", 10F), Cursor = Cursors.Hand };
            btnClear.Click += BtnClear_Click;

            Guna2Button btnSave = new Guna2Button { Text = "Lưu tạm", BorderRadius = 5, Size = new Size(100, 45), Location = new Point(130, 590), FillColor = Color.FromArgb(240, 240, 240), ForeColor = Color.Black, Font = new Font("Segoe UI", 10F), Cursor = Cursors.Hand };

            btnContinue = new Guna2Button { Text = "Thanh toán", BorderRadius = 5, Size = new Size(240, 45), Location = new Point(240, 590), FillColor = Color.FromArgb(88, 28, 230), Font = new Font("Segoe UI", 12F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnContinue.Click += BtnContinue_Click;

            pnlCart.Controls.Add(btnClear);
            pnlCart.Controls.Add(btnSave);
            pnlCart.Controls.Add(btnContinue);

            // --- 2. HEADER ---
            pnlHeader = new Guna2Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 120;
            pnlHeader.BackColor = Color.Transparent;

            Label lblPageTitle = new Label();
            lblPageTitle.Text = "Menu Items";
            lblPageTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(88, 28, 230);
            lblPageTitle.Location = new Point(10, 15);
            lblPageTitle.AutoSize = true;
            pnlHeader.Controls.Add(lblPageTitle);

            txtSearch = new Guna2TextBox();
            txtSearch.Size = new Size(350, 45);
            txtSearch.Location = new Point(10, 50);
            txtSearch.BorderRadius = 20;
            txtSearch.PlaceholderText = "Search items...";
            txtSearch.TextChanged += TxtSearch_TextChanged;
            pnlHeader.Controls.Add(txtSearch);

            flpCategories = new FlowLayoutPanel();
            flpCategories.Location = new Point(380, 50);
            flpCategories.Size = new Size(600, 60);
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = true;
            pnlHeader.Controls.Add(flpCategories);

            // --- 3. MENU ---
            flpMenu = new FlowLayoutPanel();
            flpMenu.Dock = DockStyle.Fill;
            flpMenu.AutoScroll = true;
            flpMenu.Padding = new Padding(10, 20, 10, 10);

            this.Controls.Add(flpMenu);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlCart);
        }

        private Guna2Button CreateCatButton(string text, int tag)
        {
            Guna2Button btn = new Guna2Button();
            btn.Text = text;
            btn.Size = new Size(110, 40);
            btn.Margin = new Padding(0, 0, 10, 0);
            btn.BorderRadius = 20;
            btn.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            btn.Tag = tag;
            btn.Cursor = Cursors.Hand;
            btn.FillColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.CheckedState.FillColor = Color.FromArgb(30, 30, 30);
            btn.CheckedState.ForeColor = Color.White;
            btn.Click += CategoryButton_Click;
            return btn;
        }

        #endregion

        // ========================================================
        #region 2. KHU VỰC CHỨC NĂNG & LOGIC DATABASE
        // ========================================================

        private void LoadDiningTables()
        {
            try
            {
                Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls["cboTable"];
                object currentSelectedValue = null;

                if (cboTable != null) currentSelectedValue = cboTable.SelectedValue;

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_DiningTable_GetAll");
                dt.Columns.Add("TableDisplay", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string status = row["Status"].ToString();
                    row["TableDisplay"] = $"Bàn {row["TableNumber"]} - {status}";
                }

                DataRow dr = dt.NewRow();
                dr["TableID"] = DBNull.Value;
                dr["TableDisplay"] = "Mang đi (Take Away)";
                dt.Rows.InsertAt(dr, 0);

                if (cboTable != null)
                {
                    cboTable.SelectedIndexChanged -= CboTable_SelectedIndexChanged;

                    cboTable.DataSource = dt;
                    cboTable.DisplayMember = "TableDisplay";
                    cboTable.ValueMember = "TableID";

                    if (currentSelectedValue != null && currentSelectedValue != DBNull.Value)
                        cboTable.SelectedValue = currentSelectedValue;
                    else
                        cboTable.SelectedIndex = 0;

                    cboTable.SelectedIndexChanged += CboTable_SelectedIndexChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bàn: " + ex.Message);
            }
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

        private void LoadMenuItems()
        {
            string query = "SELECT MenuItemID, CategoryID, ItemName, Price, ISNULL(ImageUrl, '') AS ImageUrl, ISNULL(Status, N'Còn') AS Status FROM MenuItem WHERE ItemStatus = 1";
            dtAllMenu = my_own_project.DAL.DataHelper.ExecuteQuery(query);

            LoadCategories();
            FilterMenu(0, "");
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

                if (row["Status"].ToString() == "Hết")
                {
                    uc.Enabled = false;

                    Label lblOut = new Label();
                    lblOut.Text = "HẾT HÀNG";
                    lblOut.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    lblOut.ForeColor = Color.White;
                    lblOut.BackColor = Color.FromArgb(255, 71, 87);
                    lblOut.AutoSize = true;
                    lblOut.Location = new Point(10, 10);
                    lblOut.Padding = new Padding(3);

                    uc.Controls.Add(lblOut);
                    lblOut.BringToFront();
                }
                else
                {
                    uc.OnSelect += Uc_OnSelect;
                }

                flpMenu.Controls.Add(uc);
            }
        }

        private void ShowBill()
        {
            flpCart.Controls.Clear();
            decimal subTotal = 0;
            int stt = 1;

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

                    Guna2Panel pnlItem = new Guna2Panel();
                    pnlItem.Size = new Size(460, 50);
                    pnlItem.CustomBorderThickness = new Padding(0, 0, 0, 1);
                    pnlItem.CustomBorderColor = Color.FromArgb(240, 240, 240);

                    Label lblSTT = new Label { Text = stt.ToString(), Location = new Point(0, 15), Font = new Font("Segoe UI", 10F), AutoSize = true };
                    Label lblName = new Label { Text = name, Location = new Point(40, 15), Font = new Font("Segoe UI Semibold", 10F), AutoSize = false, Size = new Size(130, 25), AutoEllipsis = true };

                    // ==========================================
                    // 1. NÚT TRỪ (Đã nắn lại giữa chuẩn chỉ)
                    // ==========================================
                    Button btnMinus = new Button
                    {
                        Size = new Size(24, 24),
                        Location = new Point(172, 13),
                        BackColor = Color.FromArgb(230, 230, 230),
                        ForeColor = Color.Black,
                        Text = "-",
                        Font = new Font("Arial", 14F, FontStyle.Bold), // Đổi sang Arial và tăng size chữ
                        TextAlign = ContentAlignment.MiddleCenter, // Ép ra giữa
                        Padding = new Padding(0), // Xóa mọi viền đệm ẩn
                        UseCompatibleTextRendering = true, // Phép thuật cân bằng chữ của WinForms
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    btnMinus.FlatAppearance.BorderSize = 0;
                    btnMinus.Click += (s, ev) => {
                        if (qty > 1)
                        {
                            my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE OrderDetail SET Quantity = Quantity - 1 WHERE OrderDetailID = {detailID}");
                        }
                        else
                        {
                            my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM OrderDetail WHERE OrderDetailID = {detailID}");
                        }
                        ShowBill();
                    };

                    Label lblQty = new Label { Text = qty.ToString(), Location = new Point(200, 13), Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = false, Size = new Size(30, 24), TextAlign = ContentAlignment.MiddleCenter };

                    // ==========================================
                    // 2. NÚT CỘNG (Đã nắn lại giữa chuẩn chỉ)
                    // ==========================================
                    Button btnPlus = new Button
                    {
                        Size = new Size(24, 24),
                        Location = new Point(234, 13),
                        BackColor = Color.FromArgb(230, 230, 230),
                        ForeColor = Color.Black,
                        Text = "+",
                        Font = new Font("Arial", 14F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Padding = new Padding(0),
                        UseCompatibleTextRendering = true,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    btnPlus.FlatAppearance.BorderSize = 0;
                    btnPlus.Click += (s, ev) => {
                        my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE OrderDetail SET Quantity = Quantity + 1 WHERE OrderDetailID = {detailID}");
                        ShowBill();
                    };

                    Label lblPrice = new Label { Text = price.ToString("N0"), Location = new Point(275, 15), Font = new Font("Segoe UI", 10F), AutoSize = true };
                    Label lblRowTotal = new Label { Text = rowTotal.ToString("N0"), Location = new Point(355, 15), Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true };

                    // ==========================================
                    // 3. NÚT XÓA NHANH (Nắn lại chữ X)
                    // ==========================================
                    Button btnDelete = new Button
                    {
                        Size = new Size(24, 24),
                        Location = new Point(425, 13),
                        BackColor = Color.FromArgb(255, 200, 200),
                        ForeColor = Color.Red,
                        Text = "X",
                        Font = new Font("Arial", 9F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Padding = new Padding(0),
                        UseCompatibleTextRendering = true,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    btnDelete.FlatAppearance.BorderSize = 0;
                    btnDelete.Click += (s, ev) => {
                        my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM OrderDetail WHERE OrderDetailID = {detailID}");
                        ShowBill();
                    };

                    pnlItem.Controls.Add(lblSTT);
                    pnlItem.Controls.Add(lblName);
                    pnlItem.Controls.Add(btnMinus);
                    pnlItem.Controls.Add(lblQty);
                    pnlItem.Controls.Add(btnPlus);
                    pnlItem.Controls.Add(lblPrice);
                    pnlItem.Controls.Add(lblRowTotal);
                    pnlItem.Controls.Add(btnDelete);

                    flpCart.Controls.Add(pnlItem);
                    subTotal += rowTotal;
                    stt++;
                }
            }

            Control ctl = pnlCart.Controls["lblTotalAmount"];
            if (ctl != null) ctl.Text = subTotal.ToString("N0") + " đ";
        }

        #endregion

        // ========================================================
        #region 3. KHU VỰC SỰ KIỆN (EVENTS)
        // ========================================================

        private void CboTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guna2ComboBox cbo = sender as Guna2ComboBox;
            if (cbo != null)
            {
                object val = cbo.SelectedValue;

                if (val == DBNull.Value || val == null)
                {
                    currentOrderID = -1;
                }
                else
                {
                    int tableID = Convert.ToInt32(val);
                    System.Data.SqlClient.SqlParameter[] p = new System.Data.SqlClient.SqlParameter[] {
                        new System.Data.SqlClient.SqlParameter("@TableID", tableID)
                    };
                    DataTable dtOrder = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_Orders_GetByTable", p);

                    if (dtOrder != null && dtOrder.Rows.Count > 0)
                    {
                        currentOrderID = Convert.ToInt32(dtOrder.Rows[0]["OrderID"]);
                    }
                    else
                    {
                        currentOrderID = -1;
                    }
                }
                ShowBill();
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            int catID = Convert.ToInt32(((Guna2Button)sender).Tag);
            FilterMenu(catID, txtSearch.Text);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterMenu(0, txtSearch.Text);
        }

        private void Uc_OnSelect(object sender, EventArgs e)
        {
            my_own_project.UCFoodItem uc = (my_own_project.UCFoodItem)sender;
            int menuItemID = uc.FoodID;
            decimal price = uc.Price;
            int quantity = uc.GetQuantity();

            if (quantity == 0) return;

            try
            {
                if (currentOrderID == -1)
                {
                    Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls["cboTable"];
                    object selectedTableID = cboTable.SelectedValue;

                    OrderDTO newOrder = new OrderDTO();
                    newOrder.TableID = (selectedTableID == DBNull.Value) ? (int?)null : Convert.ToInt32(selectedTableID);
                    newOrder.OrderType = (newOrder.TableID == null) ? "TakeAway" : "DineIn";
                    newOrder.Status = "Pending";
                    newOrder.OrderDate = DateTime.Now;

                    currentOrderID = OrderBLL.CreateOrder(newOrder);

                    if (newOrder.TableID != null)
                    {
                        my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE DiningTable SET Status = N'Có khách' WHERE TableID = {newOrder.TableID}");
                    }

                    LoadDiningTables();
                }

                OrderDetailDTO detail = new OrderDetailDTO();
                detail.OrderID = currentOrderID;
                detail.MenuItemID = menuItemID;
                detail.Quantity = quantity;
                detail.UnitPrice = price;

                OrderDetailBLL.AddOrderDetail(detail);

                ShowBill();
                uc.ResetQuantity();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món: " + ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (currentOrderID != -1 && MessageBox.Show("Xóa toàn bộ giỏ hàng và Hủy bàn này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE DiningTable SET Status = N'Trống' WHERE TableID = (SELECT TableID FROM Orders WHERE OrderID = {currentOrderID})");
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM OrderDetail WHERE OrderID = {currentOrderID}");
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM OrderHistory WHERE OrderID = {currentOrderID}");
                    my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM Orders WHERE OrderID = {currentOrderID}");

                    currentOrderID = -1;
                    ShowBill();
                    LoadDiningTables();
                    MessageBox.Show("Đã xóa sạch sẽ dưới Database!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa dưới Database:\n" + ex.Message, "Phát hiện Bug", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (currentOrderID == -1)
            {
                MessageBox.Show("Giỏ hàng đang trống!");
                return;
            }

            PaymentForm frm = new PaymentForm(currentOrderID, -1);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                currentOrderID = -1;
                ShowBill();
                LoadDiningTables();
            }
        }

        #endregion
    }
}