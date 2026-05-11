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
        private DataTable dtAllMenu;
        private int currentOrderID = -1;

        // Biến lưu thông tin thu ngân
        private int currentStaffID;
        private string currentStaffName;

        private Guna2Panel pnlCart, pnlHeader;
        private FlowLayoutPanel flpMenu, flpCategories, flpCart;
        private Guna2TextBox txtSearch;
        private Label lblTotal;
        private Guna2Button btnContinue;

        // Cập nhật Constructor để nhận thẻ bài nhân viên
        public POSForm(int staffID = 0, string staffName = "Admin")
        {
            this.currentStaffID = staffID;
            this.currentStaffName = staffName;

            InitializeModernPOS();
            LoadDiningTables();
            LoadMenuItems();
        }

        #region 1. UI BUILDER (CHỐNG LẸM KHUNG & TỐI ƯU NÚT)
        private void InitializeModernPOS()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);

            // Ép Form lấp đầy 100% không gian
            this.Padding = new Padding(0);

            // ─── 1. GIỎ HÀNG (PANEL BÊN PHẢI) ───
            pnlCart = new Guna2Panel { Dock = DockStyle.Right, Width = 500, FillColor = Color.White, CustomBorderThickness = new Padding(1, 0, 0, 0), CustomBorderColor = Color.FromArgb(235, 235, 235) };

            Guna2Panel pnlCartTop = new Guna2Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.White };
            pnlCartTop.Controls.Add(new Label { Text = "CHI TIẾT HÓA ĐƠN", Font = new Font("Segoe UI", 13F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true });
            Guna2ComboBox cboTable = new Guna2ComboBox { Name = "cboTable", Location = new Point(270, 15), Size = new Size(210, 36), BorderRadius = 5, Font = new Font("Segoe UI", 10F) };
            pnlCartTop.Controls.Add(cboTable);

            Guna2Panel pnlColHeader = new Guna2Panel { Location = new Point(20, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, Width = pnlCart.Width - 40, Height = 30, CustomBorderThickness = new Padding(0, 0, 0, 1), CustomBorderColor = Color.LightGray };
            pnlColHeader.Controls.Add(new Label { Text = "STT", Location = new Point(5, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Tên món", Location = new Point(35, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Thành tiền", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlColHeader.Width - 110, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "Đơn giá", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlColHeader.Width - 175, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlColHeader.Controls.Add(new Label { Text = "SL", Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlColHeader.Width - 235, 5), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true });
            pnlCartTop.Controls.Add(pnlColHeader);

            Guna2Panel pnlCartBottom = new Guna2Panel { Dock = DockStyle.Bottom, Height = 160, BackColor = Color.White, CustomBorderThickness = new Padding(0, 1, 0, 0), CustomBorderColor = Color.FromArgb(240, 240, 240) };
            pnlCartBottom.Controls.Add(new Label { Text = "Tạm tính", Font = new Font("Segoe UI", 10F), Location = new Point(20, 15), AutoSize = true });
            pnlCartBottom.Controls.Add(new Label { Text = "Tổng cộng", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(20, 50), AutoSize = true });
            lblTotal = new Label { Name = "lblTotalAmount", Text = "0 đ", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Color.Red, Location = new Point(310, 50), Size = new Size(170, 30), TextAlign = ContentAlignment.MiddleRight };

            // ĐÃ XÓA NÚT LƯU TẠM & KÉO DÀI NÚT THANH TOÁN
            Guna2Button btnClear = new Guna2Button { Text = "Xóa tất cả", BorderRadius = 5, Size = new Size(100, 45), Location = new Point(20, 95), FillColor = Color.White, ForeColor = Color.Red, CustomBorderThickness = new Padding(1), CustomBorderColor = Color.Red, Font = new Font("Segoe UI", 10F), Cursor = Cursors.Hand };
            btnClear.Click += BtnClear_Click;

            btnContinue = new Guna2Button { Text = "Thanh toán", BorderRadius = 5, Size = new Size(350, 45), Location = new Point(130, 95), FillColor = Color.FromArgb(88, 28, 230), Font = new Font("Segoe UI", 12F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnContinue.Click += BtnContinue_Click;

            pnlCartBottom.Controls.AddRange(new Control[] { lblTotal, btnClear, btnContinue });

            flpCart = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(20, 5, 0, 0), BackColor = Color.White };

            pnlCart.Controls.Add(flpCart);
            pnlCart.Controls.Add(pnlCartTop);
            pnlCart.Controls.Add(pnlCartBottom);

            // ─── 2. HEADER TÌM KIẾM ───
            pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.Transparent };
            pnlHeader.Controls.Add(new Label { Text = "Menu Items", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(20, 15), AutoSize = true });
            txtSearch = new Guna2TextBox { Size = new Size(350, 45), Location = new Point(20, 50), BorderRadius = 20, PlaceholderText = "Search items..." };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            flpCategories = new FlowLayoutPanel { Location = new Point(390, 50), Size = new Size(600, 60), WrapContents = false, AutoScroll = true };
            pnlHeader.Controls.AddRange(new Control[] { txtSearch, flpCategories });

            // ─── 3. MENU LIST ───
            flpMenu = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(20, 10, 10, 10) };

            this.Controls.Add(flpMenu);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlCart);
        }

        private Guna2Button CreateCatButton(string text, int tag)
        {
            Guna2Button btn = new Guna2Button { Text = text, Size = new Size(110, 40), Margin = new Padding(0, 0, 10, 0), BorderRadius = 20, ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton, Tag = tag, Cursor = Cursors.Hand, FillColor = Color.White, ForeColor = Color.Black };
            btn.CheckedState.FillColor = Color.FromArgb(30, 30, 30);
            btn.CheckedState.ForeColor = Color.White;
            btn.Click += CategoryButton_Click;
            return btn;
        }
        #endregion

        #region 2. LOGIC DATABASE & EVENTS
        private void LoadDiningTables()
        {
            try
            {
                Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls.Find("cboTable", true)[0];
                object currentSelectedValue = cboTable?.SelectedValue;

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_DiningTable_GetAll");
                dt.Columns.Add("TableDisplay", typeof(string));
                foreach (DataRow row in dt.Rows) row["TableDisplay"] = $"Bàn {row["TableNumber"]} - {row["Status"]}";

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
                    if (currentSelectedValue != null && currentSelectedValue != DBNull.Value) cboTable.SelectedValue = currentSelectedValue;
                    else cboTable.SelectedIndex = 0;
                    cboTable.SelectedIndexChanged += CboTable_SelectedIndexChanged;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách bàn: " + ex.Message); }
        }

        private void LoadCategories()
        {
            flpCategories.Controls.Clear();
            Guna2Button btnAll = CreateCatButton("All", 0);
            btnAll.Checked = true;
            flpCategories.Controls.Add(btnAll);
            try { foreach (DataRow row in CategoryDAL.GetAll().Rows) flpCategories.Controls.Add(CreateCatButton(row["CategoryName"].ToString(), Convert.ToInt32(row["CategoryID"]))); } catch { }
        }

        private void LoadMenuItems()
        {
            dtAllMenu = my_own_project.DAL.DataHelper.ExecuteQuery("SELECT MenuItemID, CategoryID, ItemName, Price, ISNULL(ImageUrl, '') AS ImageUrl, ISNULL(Status, N'Còn') AS Status FROM MenuItem WHERE ItemStatus = 1");
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
            if (!string.IsNullOrWhiteSpace(keyword)) filterStr += (filterStr != "" ? " AND " : "") + $"ItemName LIKE '%{keyword}%'";
            dv.RowFilter = filterStr;

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                my_own_project.UCFoodItem uc = new my_own_project.UCFoodItem();
                uc.SetData(Convert.ToInt32(row["MenuItemID"]), row["ItemName"].ToString(), Convert.ToDecimal(row["Price"]), row["ImageUrl"].ToString());

                if (row["Status"].ToString() == "Hết")
                {
                    uc.Enabled = false;
                    uc.Controls.Add(new Label { Text = "HẾT HÀNG", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(255, 71, 87), AutoSize = true, Location = new Point(10, 10), Padding = new Padding(3) });
                    uc.Controls[uc.Controls.Count - 1].BringToFront();
                }
                else uc.OnSelect += Uc_OnSelect;

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
                int itemW = flpCart.ClientSize.Width > 0 ? flpCart.ClientSize.Width - 25 : 435;

                foreach (DataRow row in dtDetails.Rows)
                {
                    int detailID = Convert.ToInt32(row["OrderDetailID"]);
                    string name = row["ItemName"].ToString();
                    int qty = Convert.ToInt32(row["Quantity"]);
                    decimal price = Convert.ToDecimal(row["UnitPrice"]);
                    decimal rowTotal = Convert.ToDecimal(row["SubTotal"]);

                    Guna2Panel pnlItem = new Guna2Panel { Size = new Size(itemW, 50), CustomBorderThickness = new Padding(0, 0, 0, 1), CustomBorderColor = Color.FromArgb(240, 240, 240) };

                    Label lblSTT = new Label { Text = stt.ToString(), Location = new Point(5, 15), Font = new Font("Segoe UI", 10F), AutoSize = true };
                    Label lblName = new Label { Text = name, Location = new Point(35, 15), Font = new Font("Segoe UI Semibold", 10F), AutoSize = false, Size = new Size(110, 25), AutoEllipsis = true };

                    Button btnDelete = new Button { Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(24, 24), Location = new Point(itemW - 28, 13), BackColor = Color.FromArgb(255, 200, 200), ForeColor = Color.Red, Text = "X", Font = new Font("Arial", 9F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Padding = new Padding(0), UseCompatibleTextRendering = true, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                    btnDelete.FlatAppearance.BorderSize = 0;
                    btnDelete.Click += (s, ev) => { my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM OrderDetail WHERE OrderDetailID = {detailID}"); ShowBill(); };

                    Label lblRowTotal = new Label { Anchor = AnchorStyles.Top | AnchorStyles.Right, Text = rowTotal.ToString("N0"), Location = new Point(itemW - 110, 15), Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true };
                    Label lblPrice = new Label { Anchor = AnchorStyles.Top | AnchorStyles.Right, Text = price.ToString("N0"), Location = new Point(itemW - 175, 15), Font = new Font("Segoe UI", 10F), AutoSize = true };

                    Button btnPlus = new Button { Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(24, 24), Location = new Point(itemW - 215, 13), BackColor = Color.FromArgb(230, 230, 230), ForeColor = Color.Black, Text = "+", Font = new Font("Arial", 14F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Padding = new Padding(0), UseCompatibleTextRendering = true, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                    btnPlus.FlatAppearance.BorderSize = 0;
                    btnPlus.Click += (s, ev) => { my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE OrderDetail SET Quantity = Quantity + 1, SubTotal = (Quantity + 1) * UnitPrice WHERE OrderDetailID = {detailID}"); ShowBill(); };

                    Label lblQty = new Label { Anchor = AnchorStyles.Top | AnchorStyles.Right, Text = qty.ToString(), Location = new Point(itemW - 245, 13), Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = false, Size = new Size(30, 24), TextAlign = ContentAlignment.MiddleCenter };

                    Button btnMinus = new Button { Anchor = AnchorStyles.Top | AnchorStyles.Right, Size = new Size(24, 24), Location = new Point(itemW - 275, 13), BackColor = Color.FromArgb(230, 230, 230), ForeColor = Color.Black, Text = "-", Font = new Font("Arial", 14F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Padding = new Padding(0), UseCompatibleTextRendering = true, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                    btnMinus.FlatAppearance.BorderSize = 0;
                    btnMinus.Click += (s, ev) => {
                        if (qty > 1) my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE OrderDetail SET Quantity = Quantity - 1, SubTotal = (Quantity - 1) * UnitPrice WHERE OrderDetailID = {detailID}");
                        else my_own_project.DAL.DataHelper.ExecuteNonQuery($"DELETE FROM OrderDetail WHERE OrderDetailID = {detailID}");
                        ShowBill();
                    };

                    pnlItem.Controls.AddRange(new Control[] { lblSTT, lblName, btnMinus, lblQty, btnPlus, lblPrice, lblRowTotal, btnDelete });
                    flpCart.Controls.Add(pnlItem);

                    subTotal += rowTotal;
                    stt++;
                }
            }

            Control[] ctls = pnlCart.Controls.Find("lblTotalAmount", true);
            if (ctls.Length > 0) ctls[0].Text = subTotal.ToString("N0") + " đ";
        }

        private void CboTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            Guna2ComboBox cbo = sender as Guna2ComboBox;
            if (cbo != null)
            {
                object val = cbo.SelectedValue;
                if (val == DBNull.Value || val == null) currentOrderID = -1;
                else
                {
                    int tableID = Convert.ToInt32(val);
                    System.Data.SqlClient.SqlParameter[] p = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@TableID", tableID) };
                    DataTable dtOrder = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_Orders_GetByTable", p);
                    if (dtOrder != null && dtOrder.Rows.Count > 0) currentOrderID = Convert.ToInt32(dtOrder.Rows[0]["OrderID"]);
                    else currentOrderID = -1;
                }
                ShowBill();
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e) => FilterMenu(Convert.ToInt32(((Guna2Button)sender).Tag), txtSearch.Text);
        private void TxtSearch_TextChanged(object sender, EventArgs e) => FilterMenu(0, txtSearch.Text);

        private void Uc_OnSelect(object sender, EventArgs e)
        {
            my_own_project.UCFoodItem uc = (my_own_project.UCFoodItem)sender;
            if (uc.GetQuantity() == 0) return;

            try
            {
                if (currentOrderID == -1)
                {
                    Guna2ComboBox cboTable = (Guna2ComboBox)pnlCart.Controls.Find("cboTable", true)[0];
                    object selectedTableID = cboTable.SelectedValue;

                    OrderDTO newOrder = new OrderDTO();
                    newOrder.TableID = (selectedTableID == DBNull.Value) ? (int?)null : Convert.ToInt32(selectedTableID);
                    newOrder.OrderType = (newOrder.TableID == null) ? "TakeAway" : "DineIn";
                    newOrder.Status = "Pending";
                    newOrder.OrderDate = DateTime.Now;

                    currentOrderID = OrderBLL.CreateOrder(newOrder);
                    if (newOrder.TableID != null)
                        my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE DiningTable SET Status = N'Có khách' WHERE TableID = {newOrder.TableID}");
                    LoadDiningTables();
                }

                OrderDetailDTO detail = new OrderDetailDTO();
                detail.OrderID = currentOrderID;
                detail.MenuItemID = uc.FoodID;
                detail.Quantity = uc.GetQuantity();
                detail.UnitPrice = uc.Price;

                OrderDetailBLL.AddOrderDetail(detail);
                ShowBill();
                uc.ResetQuantity();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi thêm món: " + ex.Message); }
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
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (currentOrderID == -1) { MessageBox.Show("Giỏ hàng đang trống!"); return; }

            // 👉 ĐÃ FIX: TRUYỀN ID VÀ TÊN NHÂN VIÊN SANG PAYMENT FORM ĐỂ LƯU VẾT VÀ IN BILL
            PaymentForm frm = new PaymentForm(currentOrderID, -1, currentStaffID, currentStaffName);

            if (frm.ShowDialog() == DialogResult.OK) { currentOrderID = -1; ShowBill(); LoadDiningTables(); }
        }
        #endregion
    }
}