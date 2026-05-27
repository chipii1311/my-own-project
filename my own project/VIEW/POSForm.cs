using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DAL;
using my_own_project.DTO;
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

        public POSForm(int staffID = 0, string staffName = "Admin")
        {
            this.currentStaffID = staffID <= 0 ? 1 : staffID;
            this.currentStaffName = staffName;

            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            // Tải dữ liệu ban đầu
            LoadDiningTables();
            LoadMenuItems();
        }

        // ─────────────────────────────────────────────────────────
        //  DATA BINDING & ĐỘNG GIAO DIỆN
        // ─────────────────────────────────────────────────────────
        private Guna2Button CreateCatButton(string text, int tag)
        {
            Guna2Button btn = new Guna2Button
            {
                Text = text,
                Size = new Size(110, 40),
                Margin = new Padding(0, 0, 10, 0),
                BorderRadius = 20,
                ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                Tag = tag,
                Cursor = Cursors.Hand,
                FillColor = Color.White,
                ForeColor = Color.Black
            };
            btn.CheckedState.FillColor = Color.FromArgb(30, 30, 30);
            btn.CheckedState.ForeColor = Color.White;
            btn.Click += CategoryButton_Click;
            return btn;
        }

        private void LoadDiningTables()
        {
            try
            {
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
            dtAllMenu = my_own_project.DAL.DataHelper.ExecuteSPGetTable("sp_POS_GetMenuWithStockStatus");
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

            if (lblTotal != null) lblTotal.Text = subTotal.ToString("N0") + " đ";
        }

        // ─────────────────────────────────────────────────────────
        //  EVENTS HỆ THỐNG
        // ─────────────────────────────────────────────────────────
        private void CboTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTable != null)
            {
                object val = cboTable.SelectedValue;
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

        public void CategoryButton_Click(object sender, EventArgs e) => FilterMenu(Convert.ToInt32(((Guna2Button)sender).Tag), txtSearch.Text);
        public void TxtSearch_TextChanged(object sender, EventArgs e) => FilterMenu(0, txtSearch.Text);

        private void Uc_OnSelect(object sender, EventArgs e)
        {
            my_own_project.UCFoodItem uc = (my_own_project.UCFoodItem)sender;
            if (uc.GetQuantity() == 0) return;

            try
            {
                if (currentOrderID == -1)
                {
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

        public void BtnClear_Click(object sender, EventArgs e)
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

        public void BtnContinue_Click(object sender, EventArgs e)
        {
            if (currentOrderID == -1)
            {
                MessageBox.Show("Giỏ hàng đang trống!");
                return;
            }

            try
            {
                // 1. Kiểm tra kho trước khi mở thanh toán
                DataTable dtMissing = InventoryTransactionBLL.CheckStockForOrder(currentOrderID);

                if (dtMissing != null && dtMissing.Rows.Count > 0)
                {
                    string message = "Không đủ nguyên liệu để thanh toán đơn này:\n\n";

                    foreach (DataRow row in dtMissing.Rows)
                    {
                        string name = row["IngredientName"].ToString();
                        string unit = row["Unit"].ToString();
                        decimal required = Convert.ToDecimal(row["RequiredQuantity"]);
                        decimal current = Convert.ToDecimal(row["CurrentStock"]);

                        message += $"- {name}: cần {required:N2} {unit}, hiện có {current:N2} {unit}\n";
                    }

                    MessageBox.Show(
                        message,
                        "Thiếu nguyên liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // 2. Thanh toán như cũ
                // Tùy thuộc namespace của PaymentForm bạn đang sử dụng
                my_own_project.VIEW.PaymentForm frm = new my_own_project.VIEW.PaymentForm(currentOrderID, -1, currentStaffID, currentStaffName);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // 3. Sau thanh toán thành công mới trừ kho
                    InventoryTransactionBLL.ExportByOrderRecipe(
                        currentOrderID,
                        currentStaffID,
                        "Tự động trừ kho từ POS - Order #" + currentOrderID);

                    currentOrderID = -1;
                    ShowBill();
                    LoadDiningTables();
                    LoadMenuItems();

                    MessageBox.Show(
                        "Thanh toán thành công và đã trừ kho nguyên liệu.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi thanh toán / trừ kho: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}