using my_own_project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEWSTAFF
{
    public partial class StaffOrderForm : Form
    {
        private int tableID;
        private int tableNumber;
        private int currentOrderID;
        private DataTable menuItemsData;
        private DataTable billItems; // Lưu chi tiết hóa đơn
        private string currentUserName = "Nguyễn Văn An";
        private Timer timerClock;
        public StaffOrderForm(int tableId, int tableNum)
        {
            InitializeComponent();
            this.tableID = tableId;
            this.tableNumber = tableNum;
            timerClock = new Timer();
            timerClock.Interval = 1000; // Cập nhật mỗi giây
            timerClock.Tick += (s, e) => UpdateClock();
           
        }
        private void StaffOrderForm_Load(object sender, EventArgs e)
        {
            try
            {
                lblTitle.Text = $"🍽️ GỌI MÓN - BÀN {tableNumber:D2}";
                lblUser.Text = $"👤 {currentUserName}";
                timerClock.Start();

                // Initialize bill
                billItems = new DataTable();
                billItems.Columns.Add("MenuItemID", typeof(int));
                billItems.Columns.Add("ItemName", typeof(string));
                billItems.Columns.Add("Quantity", typeof(int));
                billItems.Columns.Add("UnitPrice", typeof(decimal));
                billItems.Columns.Add("SubTotal", typeof(decimal));

                // Load menu
                LoadMenuItems();
                LoadCategories();
                SetupBillGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khởi tạo: " + ex.Message, "Lỗi");
            }
        }
        private void LoadMenuItems()
        {
            try
            {
                menuItemsData = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllAvailable");

                if (menuItemsData != null && menuItemsData.Rows.Count > 0)
                {
                    DisplayMenuItems(menuItemsData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải menu: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // LOAD CATEGORIES
        // ============================================
        private void LoadCategories()
        {
            try
            {
                DataTable categories = DataHelper.ExecuteSPGetTable("sp_Category_GetAll");

                if (categories != null && categories.Rows.Count > 0)
                {
                    pnlCategories.Controls.Clear();

                    foreach (DataRow row in categories.Rows)
                    {
                        string categoryName = row["CategoryName"].ToString();
                        int categoryID = Convert.ToInt32(row["CategoryID"]);

                        Guna.UI2.WinForms.Guna2Button btn = new Guna.UI2.WinForms.Guna2Button();
                        btn.Text = categoryName;
                        btn.FillColor = Color.FromArgb(63, 81, 181);
                        btn.ForeColor = Color.White;
                        btn.AutoRoundedCorners = true;
                        btn.BorderRadius = 8;
                        btn.Size = new Size(90, 35);
                        btn.Margin = new Padding(5);
                        btn.Cursor = Cursors.Hand;
                        btn.HoverState.FillColor = Color.FromArgb(33, 150, 243);
                        btn.Click += (s, e) => FilterByCategory(categoryID);

                        pnlCategories.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải danh mục: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // DISPLAY MENU ITEMS
        // ============================================
        private void DisplayMenuItems(DataTable items)
        {
            try
            {
                pnlMenuItems.Controls.Clear();

                foreach (DataRow row in items.Rows)
                {
                    int menuItemID = Convert.ToInt32(row["MenuItemID"]);
                    string itemName = row["ItemName"].ToString();
                    decimal price = Convert.ToDecimal(row["Price"]);

                    // Create menu item panel
                    Guna.UI2.WinForms.Guna2Panel itemPanel = new Guna.UI2.WinForms.Guna2Panel();
                    itemPanel.AutoRoundedCorners = true;
                    itemPanel.BackColor = Color.White;
                    itemPanel.BorderColor = Color.FromArgb(200, 200, 200);
                    itemPanel.BorderRadius = 8;
                    itemPanel.BorderThickness = 1;
                    itemPanel.FillColor = Color.White;
                    itemPanel.Size = new Size(360, 80);
                    itemPanel.Margin = new Padding(5);
                    itemPanel.Padding = new Padding(10);
                    itemPanel.Cursor = Cursors.Hand;

                    // Item info
                    Label lblItem = new Label();
                    lblItem.Text = itemName;
                    lblItem.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lblItem.ForeColor = Color.FromArgb(50, 50, 50);
                    lblItem.AutoSize = true;
                    lblItem.Location = new Point(10, 10);
                    itemPanel.Controls.Add(lblItem);

                    // Price
                    Label lblPrice = new Label();
                    lblPrice.Text = $"{price:N0} đ";
                    lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    lblPrice.ForeColor = Color.FromArgb(244, 67, 54);
                    lblPrice.AutoSize = true;
                    lblPrice.Location = new Point(10, 35);
                    itemPanel.Controls.Add(lblPrice);

                    // Add button
                    Guna.UI2.WinForms.Guna2Button btnAdd = new Guna.UI2.WinForms.Guna2Button();
                    btnAdd.Text = "➕";
                    btnAdd.BackColor = Color.Transparent;
                    btnAdd.BorderRadius = 8;
                    btnAdd.FillColor = Color.FromArgb(63, 81, 181);
                    btnAdd.ForeColor = Color.White;
                    btnAdd.Font = new Font("Segoe UI", 14);
                    btnAdd.Size = new Size(40, 40);
                    btnAdd.Location = new Point(310, 20);
                    btnAdd.Cursor = Cursors.Hand;
                    btnAdd.Click += (s, e) => AddToBill(menuItemID, itemName, (int)price);
                    itemPanel.Controls.Add(btnAdd);

                    pnlMenuItems.Controls.Add(itemPanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi hiển thị menu: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // FILTER BY CATEGORY
        // ============================================
        private void FilterByCategory(int categoryID)
        {
            try
            {
                DataView dv = new DataView(menuItemsData);
                dv.RowFilter = $"CategoryID = {categoryID}";
                DataTable filtered = dv.ToTable();
                DisplayMenuItems(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi lọc: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // ADD TO BILL
        // ============================================
        private void AddToBill(int menuItemID, string itemName, int price)
        {
            try
            {
                // Check if item already exists
                DataRow[] existingRows = billItems.Select($"MenuItemID = {menuItemID}");

                if (existingRows.Length > 0)
                {
                    // Increase quantity
                    existingRows[0]["Quantity"] = Convert.ToInt32(existingRows[0]["Quantity"]) + 1;
                    existingRows[0]["SubTotal"] = Convert.ToInt32(existingRows[0]["Quantity"]) * price;
                }
                else
                {
                    // Add new row
                    DataRow newRow = billItems.NewRow();
                    newRow["MenuItemID"] = menuItemID;
                    newRow["ItemName"] = itemName;
                    newRow["Quantity"] = 1;
                    newRow["UnitPrice"] = price;
                    newRow["SubTotal"] = price;
                    billItems.Rows.Add(newRow);
                }

                // Refresh bill display
                RefreshBill();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm món: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // SETUP BILL GRID
        // ============================================
        private void SetupBillGrid()
        {
            try
            {
                pnlBillItems.AutoGenerateColumns = false;
                pnlBillItems.Columns.Clear();

                // STT
                DataGridViewTextBoxColumn colSTT = new DataGridViewTextBoxColumn();
                colSTT.HeaderText = "STT";
                colSTT.DataPropertyName = "MenuItemID";
                colSTT.Width = 40;
                pnlBillItems.Columns.Add(colSTT);

                // Item Name
                DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                colName.HeaderText = "Tên món";
                colName.DataPropertyName = "ItemName";
                colName.Width = 200;
                pnlBillItems.Columns.Add(colName);

                // Quantity +/-
                DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn();
                colQty.HeaderText = "SL";
                colQty.DataPropertyName = "Quantity";
                colQty.Width = 40;
                colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                pnlBillItems.Columns.Add(colQty);

                // Unit Price
                DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn();
                colPrice.HeaderText = "Đơn giá";
                colPrice.DataPropertyName = "UnitPrice";
                colPrice.Width = 100;
                colPrice.DefaultCellStyle.Format = "N0";
                colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                pnlBillItems.Columns.Add(colPrice);

                // Subtotal
                DataGridViewTextBoxColumn colSubtotal = new DataGridViewTextBoxColumn();
                colSubtotal.HeaderText = "Thành tiền";
                colSubtotal.DataPropertyName = "SubTotal";
                colSubtotal.Width = 100;
                colSubtotal.DefaultCellStyle.Format = "N0";
                colSubtotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                pnlBillItems.Columns.Add(colSubtotal);

                // Delete button
                DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn();
                colDelete.HeaderText = "";
                colDelete.Text = "🗑️";
                colDelete.UseColumnTextForButtonValue = true;
                colDelete.Width = 40;
                pnlBillItems.Columns.Add(colDelete);

                pnlBillItems.CellClick += (s, e) =>
                {
                    if (e.ColumnIndex == pnlBillItems.Columns.Count - 1 && e.RowIndex >= 0)
                    {
                        billItems.Rows[e.RowIndex].Delete();
                        RefreshBill();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi setup grid: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // REFRESH BILL
        // ============================================
        private void RefreshBill()
        {
            try
            {
                pnlBillItems.DataSource = billItems;

                // Calculate totals
                decimal total = 0;
                foreach (DataRow row in billItems.Rows)
                {
                    total += Convert.ToDecimal(row["SubTotal"]);
                }

                lblTotal.Text = $"{total:N0} đ";
                lblGrandTotalAmount.Text = $"{total:N0} đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi refresh: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // TEXT SEARCH CHANGED
        // ============================================
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.ToLower();
                DataView dv = new DataView(menuItemsData);
                dv.RowFilter = $"ItemName LIKE '%{searchText}%'";
                DisplayMenuItems(dv.ToTable());
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tìm kiếm: " + ex.Message, "Lỗi");
            }
        }

        // ============================================
        // BUTTON EVENTS
        // ============================================
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("👥 Chọn khách hàng (Chưa implement)", "Thông báo");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("❌ Xóa tất cả các món?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                billItems.Clear();
                RefreshBill();
            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            MessageBox.Show("💾 Lưu tạm đơn hàng (Chưa implement)", "Thông báo");
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (billItems.Rows.Count == 0)
            {
                MessageBox.Show("⚠️ Vui lòng thêm món trước!", "Lỗi");
                return;
            }

            // Open payment form
            //StaffPaymentForm paymentForm = new StaffPaymentForm(tableID, tableNumber, billItems);
            //paymentForm.ShowDialog();
            this.Close();
        }

        private void UpdateClock()
        {
            lblTime.Text = DateTime.Now.ToString("⏰ HH:mm:ss | dd/MM/yyyy");
        }
    }
}
