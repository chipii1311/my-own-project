using my_own_project.BLL;
using my_own_project.DTO;
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
    public class UcOrderPOS : UserControl
    {
        private readonly UserDTO _user;
        private int _tableID;
        private string _tableName;
        private int _currentOrderID = -1;
        private DataTable _orderItems = new DataTable();

        // ── Left panel controls ─────────────────────
        private Panel pnlLeft, pnlRight;
        private Panel pnlCategoryList;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchMenu;
        private Panel pnlMenuItems;

        // ── Right panel controls ────────────────────
        private System.Windows.Forms.Label lblTableTitle;
        private Guna.UI2.WinForms.Guna2Button btnGuestInfo;
        private Guna.UI2.WinForms.Guna2DataGridView dgvOrder;
        private System.Windows.Forms.Label lblSubTotal, lblSubTotalVal;
        private System.Windows.Forms.Label lblDiscount, lblDiscountVal;
        private Guna.UI2.WinForms.Guna2TextBox txtDiscount;
        private System.Windows.Forms.Label lblTotal, lblTotalVal;
        private Guna.UI2.WinForms.Guna2Button btnClear, btnSave, btnCheckout;

        private Color Purple = Color.FromArgb(106, 90, 205);
        private Color Dark = Color.FromArgb(25, 23, 60);
        private DataTable _menuData;

        public UcOrderPOS(UserDTO user)
        {
            _user = user;
            InitUI();
            InitOrderTable();
        }

        public void LoadTable(int tableID, string tableName)
        {
            _tableID = tableID;
            _tableName = tableName;
            lblTableTitle.Text = $"GỌI MÓN - {tableName}";
            LoadMenu();
            LoadCategories();
        }

        private void InitUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 9.5f);

            // ── LEFT: Menu list ────────────────────────
            pnlLeft = new Panel();
            pnlLeft.BackColor = Color.White;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Size = new Size(430, 720);

            var lblMenuTitle = MakeLabel("DANH SÁCH MÓN", 16, 14, Dark, new Font("Segoe UI Semibold", 11f, FontStyle.Bold));

            txtSearchMenu = new Guna.UI2.WinForms.Guna2TextBox();
            txtSearchMenu.BorderRadius = 20;
            txtSearchMenu.FillColor = Color.FromArgb(245, 246, 250);
            txtSearchMenu.PlaceholderText = "Tìm món ăn...";
            txtSearchMenu.Font = new Font("Segoe UI", 9.5f);
            txtSearchMenu.Location = new Point(16, 44);
            txtSearchMenu.Size = new Size(320, 36);
            txtSearchMenu.TextChanged += (s, e) => FilterMenu(txtSearchMenu.Text);

            var btnFilter = new Guna.UI2.WinForms.Guna2Button();
            btnFilter.BorderRadius = 20;
            btnFilter.FillColor = Color.FromArgb(245, 246, 250);
            btnFilter.ForeColor = Dark;
            btnFilter.Text = "⚙";
            btnFilter.Font = new Font("Segoe UI", 12f);
            btnFilter.Location = new Point(344, 44);
            btnFilter.Size = new Size(40, 36);

            pnlCategoryList = new Panel();
            pnlCategoryList.BackColor = Color.White;
            pnlCategoryList.Location = new Point(0, 92);
            pnlCategoryList.Size = new Size(120, 600);
            pnlCategoryList.AutoScroll = true;

            pnlMenuItems = new Panel();
            pnlMenuItems.BackColor = Color.White;
            pnlMenuItems.Location = new Point(124, 92);
            pnlMenuItems.Size = new Size(306, 620);
            pnlMenuItems.AutoScroll = true;

            pnlLeft.Controls.AddRange(new Control[] {
                lblMenuTitle, txtSearchMenu, btnFilter,
                pnlCategoryList, pnlMenuItems
            });

            // ── RIGHT: Invoice ─────────────────────────
            pnlRight = new Panel();
            pnlRight.BackColor = Color.White;
            pnlRight.Location = new Point(442, 0);
            pnlRight.Size = new Size(610, 720);

            lblTableTitle = MakeLabel("GỌI MÓN", 16, 14, Dark, new Font("Segoe UI Semibold", 13f, FontStyle.Bold));

            var lblInvoiceTitle = MakeLabel("CHI TIẾT HÓA ĐƠN", 16, 50, Color.FromArgb(80, 80, 110), new Font("Segoe UI Semibold", 10f, FontStyle.Bold));

            btnGuestInfo = new Guna.UI2.WinForms.Guna2Button();
            btnGuestInfo.Text = "👤  Khách lẻ";
            btnGuestInfo.BorderRadius = 20;
            btnGuestInfo.FillColor = Color.FromArgb(245, 246, 250);
            btnGuestInfo.ForeColor = Purple;
            btnGuestInfo.Font = new Font("Segoe UI", 9.5f);
            btnGuestInfo.Size = new Size(110, 32);
            btnGuestInfo.Location = new Point(482, 44);

            // Order DataGridView
            dgvOrder = new Guna.UI2.WinForms.Guna2DataGridView();
            dgvOrder.Location = new Point(8, 86);
            dgvOrder.Size = new Size(592, 400);
            dgvOrder.AllowUserToAddRows = false;
            dgvOrder.AllowUserToDeleteRows = false;
            dgvOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrder.RowTemplate.Height = 44;
            dgvOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrder.BackgroundColor = Color.White;
            dgvOrder.BorderStyle = BorderStyle.None;
            dgvOrder.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgvOrder.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 80, 110);
            dgvOrder.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            dgvOrder.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvOrder.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 238, 255);
            dgvOrder.DefaultCellStyle.SelectionForeColor = Dark;
            dgvOrder.ColumnHeadersHeight = 40;
            dgvOrder.GridColor = Color.FromArgb(240, 240, 248);
            dgvOrder.CellClick += DgvOrder_CellClick;

            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { Name = "STT", HeaderText = "STT", FillWeight = 30, ReadOnly = true });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Tên món", FillWeight = 180, ReadOnly = true });
            dgvOrder.Columns.Add(new DataGridViewButtonColumn { Name = "Minus", HeaderText = "", FillWeight = 26, Text = "−", UseColumnTextForButtonValue = true });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { Name = "Qty", HeaderText = "SL", FillWeight = 40, ReadOnly = true });
            dgvOrder.Columns.Add(new DataGridViewButtonColumn { Name = "Plus", HeaderText = "", FillWeight = 26, Text = "+", UseColumnTextForButtonValue = true });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Đơn giá", FillWeight = 80, ReadOnly = true });
            dgvOrder.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sub", HeaderText = "Thành tiền", FillWeight = 90, ReadOnly = true });
            dgvOrder.Columns.Add(new DataGridViewButtonColumn { Name = "Del", HeaderText = "", FillWeight = 30, Text = "🗑", UseColumnTextForButtonValue = true });

            // Totals
            int ty = 498;
            lblSubTotal = MakeLabel("Tạm tính", 16, ty, Color.FromArgb(100, 100, 130), new Font("Segoe UI", 10f));
            lblSubTotalVal = MakeLabel("0 đ", 460, ty, Dark, new Font("Segoe UI Semibold", 10f, FontStyle.Bold));
            lblSubTotalVal.AutoSize = true;

            lblDiscount = MakeLabel("Giảm giá", 16, ty + 36, Color.FromArgb(100, 100, 130), new Font("Segoe UI", 10f));
            txtDiscount = new Guna.UI2.WinForms.Guna2TextBox();
            txtDiscount.Text = "0";
            txtDiscount.BorderRadius = 8;
            txtDiscount.FillColor = Color.FromArgb(245, 246, 250);
            txtDiscount.Font = new Font("Segoe UI", 9.5f);
            txtDiscount.Size = new Size(60, 30);
            txtDiscount.Location = new Point(400, ty + 32);
            txtDiscount.TextChanged += (s, e) => RecalcTotal();
            var lblPct = MakeLabel("%", 468, ty + 40, Color.FromArgb(120, 120, 150), new Font("Segoe UI", 9f));
            lblDiscountVal = MakeLabel("0 đ", 500, ty + 40, Dark, new Font("Segoe UI Semibold", 10f, FontStyle.Bold));

            var pnlTotalLine = new Panel { BackColor = Color.FromArgb(230, 230, 240), Location = new Point(8, ty + 80), Size = new Size(592, 1) };

            lblTotal = MakeLabel("Tổng cộng", 16, ty + 92, Dark, new Font("Segoe UI Semibold", 12f, FontStyle.Bold));
            lblTotalVal = MakeLabel("0 đ", 400, ty + 90, Color.FromArgb(229, 57, 53), new Font("Segoe UI Semibold", 16f, FontStyle.Bold));
            lblTotalVal.AutoSize = true;

            // Action buttons
            btnClear = MakeActionBtn("Xóa tất cả", 16, 660, 120, 44, Color.FromArgb(255, 235, 235), ColorBusy: Color.FromArgb(229, 57, 53));
            btnSave = MakeActionBtn("Lưu tạm", 146, 660, 120, 44, Color.FromArgb(245, 246, 250), ColorBusy: Color.FromArgb(80, 80, 110));
            btnCheckout = MakeActionBtn("🛒  Thanh toán", 276, 660, 316, 44, Purple);
            btnCheckout.ForeColor = Color.White;
            btnClear.Click += (s, e) => ClearOrder();
            btnCheckout.Click += (s, e) => OpenPayment();

            pnlRight.Controls.AddRange(new Control[] {
                lblTableTitle, lblInvoiceTitle, btnGuestInfo,
                dgvOrder,
                lblSubTotal, lblSubTotalVal,
                lblDiscount, txtDiscount, lblPct, lblDiscountVal,
                pnlTotalLine,
                lblTotal, lblTotalVal,
                btnClear, btnSave, btnCheckout
            });

            this.Controls.AddRange(new Control[] { pnlLeft, pnlRight });
        }

        private void InitOrderTable()
        {
            _orderItems.Columns.Add("MenuItemID", typeof(int));
            _orderItems.Columns.Add("Name", typeof(string));
            _orderItems.Columns.Add("Price", typeof(decimal));
            _orderItems.Columns.Add("Qty", typeof(int));
        }

        // ════════════════════════════════════════════
        // LOAD MENU
        // ════════════════════════════════════════════
        private void LoadMenu(int categoryID = 0)
        {
            try
            {
                _menuData = MenuItemBLL.GetMenuItemsByCategory(1);
                RenderMenuItems(_menuData, categoryID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải menu: " + ex.Message);
            }
        }

        private void LoadCategories()
        {
            pnlCategoryList.Controls.Clear();
            try
            {
                DataTable dt = CategoryBLL.GetAllCategories();
                int y = 0;
                var btnAll = MakeCategoryBtn("Tất cả", 0, y, true);
                btnAll.Click += (s, e) => { RenderMenuItems(_menuData, 0); SetActiveCatBtn((Guna.UI2.WinForms.Guna2Button)s); };
                pnlCategoryList.Controls.Add(btnAll);
                y += 48;

                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["CategoryID"]);
                    string name = row["CategoryName"].ToString();
                    var btn = MakeCategoryBtn(name, id, y, false);
                    btn.Click += (s, e) => { RenderMenuItems(_menuData, id); SetActiveCatBtn((Guna.UI2.WinForms.Guna2Button)s); };
                    pnlCategoryList.Controls.Add(btn);
                    y += 48;
                }
            }
            catch { }
        }

        private void RenderMenuItems(DataTable dt, int catFilter)
        {
            pnlMenuItems.Controls.Clear();
            if (dt == null) return;
            int y = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (catFilter != 0 && Convert.ToInt32(row["CategoryID"]) != catFilter) continue;
                var card = BuildMenuCard(row);
                card.Location = new Point(0, y);
                pnlMenuItems.Controls.Add(card);
                y += 80;
            }
        }

        private void FilterMenu(string keyword)
        {
            if (_menuData == null) return;
            pnlMenuItems.Controls.Clear();
            int y = 0;
            foreach (DataRow row in _menuData.Rows)
            {
                if (!string.IsNullOrEmpty(keyword) &&
                    !row["ItemName"].ToString().ToLower().Contains(keyword.ToLower())) continue;
                var card = BuildMenuCard(row);
                card.Location = new Point(0, y);
                pnlMenuItems.Controls.Add(card);
                y += 80;
            }
        }

        private Panel BuildMenuCard(DataRow row)
        {
            int id = Convert.ToInt32(row["MenuItemID"]);
            string name = row["ItemName"].ToString();
            decimal price = Convert.ToDecimal(row["Price"]);

            var card = new Panel();
            card.BackColor = Color.White;
            card.Size = new Size(290, 72);
            card.Cursor = Cursors.Hand;
            card.Tag = id;
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 240), 1))
                e.Graphics.DrawLine(pen, 0, card.Height - 1, card.Width, card.Height - 1);
            };

            var picItem = new PictureBox();
            picItem.BackColor = Color.FromArgb(240, 238, 255);
            picItem.Size = new Size(56, 56);
            picItem.Location = new Point(8, 8);
            picItem.SizeMode = PictureBoxSizeMode.Zoom;

            var lblName = new System.Windows.Forms.Label();
            lblName.Text = name;
            lblName.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            lblName.ForeColor = Dark;
            lblName.AutoSize = false;
            lblName.Size = new Size(160, 24);
            lblName.Location = new Point(72, 10);

            var lblPrice = new System.Windows.Forms.Label();
            lblPrice.Text = $"{price:N0} đ";
            lblPrice.Font = new Font("Segoe UI", 9f);
            lblPrice.ForeColor = Color.FromArgb(120, 120, 150);
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(72, 34);

            var btnPlus = new Guna.UI2.WinForms.Guna2Button();
            btnPlus.Text = "+";
            btnPlus.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold);
            btnPlus.BorderRadius = 20;
            btnPlus.FillColor = Purple;
            btnPlus.ForeColor = Color.White;
            btnPlus.Size = new Size(30, 30);
            btnPlus.Location = new Point(250, 20);
            btnPlus.Click += (s, e) => AddToOrder(id, name, price);

            card.Controls.AddRange(new Control[] { picItem, lblName, lblPrice, btnPlus });
            return card;
        }

        // ════════════════════════════════════════════
        // ORDER LOGIC
        // ════════════════════════════════════════════
        private void AddToOrder(int menuItemID, string name, decimal price)
        {
            foreach (DataRow r in _orderItems.Rows)
            {
                if (Convert.ToInt32(r["MenuItemID"]) == menuItemID)
                {
                    r["Qty"] = Convert.ToInt32(r["Qty"]) + 1;
                    RefreshOrderGrid();
                    return;
                }
            }
            _orderItems.Rows.Add(menuItemID, name, price, 1);
            RefreshOrderGrid();
        }

        private void RefreshOrderGrid()
        {
            dgvOrder.Rows.Clear();
            int stt = 1;
            foreach (DataRow r in _orderItems.Rows)
            {
                int qty = Convert.ToInt32(r["Qty"]);
                decimal price = Convert.ToDecimal(r["Price"]);
                decimal sub = price * qty;
                dgvOrder.Rows.Add(stt++, r["Name"], "−", qty, "+",
                    $"{price:N0}", $"{sub:N0}", "🗑");
            }
            RecalcTotal();
        }

        private void DgvOrder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _orderItems.Rows.Count) return;
            DataRow r = _orderItems.Rows[e.RowIndex];

            if (e.ColumnIndex == dgvOrder.Columns["Plus"].Index)
            { r["Qty"] = Convert.ToInt32(r["Qty"]) + 1; RefreshOrderGrid(); }
            else if (e.ColumnIndex == dgvOrder.Columns["Minus"].Index)
            {
                int q = Convert.ToInt32(r["Qty"]) - 1;
                if (q <= 0) _orderItems.Rows.Remove(r);
                else r["Qty"] = q;
                RefreshOrderGrid();
            }
            else if (e.ColumnIndex == dgvOrder.Columns["Del"].Index)
            { _orderItems.Rows.Remove(r); RefreshOrderGrid(); }
        }

        private void RecalcTotal()
        {
            decimal sub = 0;
            foreach (DataRow r in _orderItems.Rows)
                sub += Convert.ToDecimal(r["Price"]) * Convert.ToInt32(r["Qty"]);

            decimal pct = 0;
            decimal.TryParse(txtDiscount.Text, out pct);
            decimal disc = sub * pct / 100;
            decimal total = sub - disc;

            lblSubTotalVal.Text = $"{sub:N0} đ";
            lblDiscountVal.Text = $"{disc:N0} đ";
            lblTotalVal.Text = $"{total:N0} đ";
        }

        private void ClearOrder()
        {
            if (MessageBox.Show("Xóa tất cả món?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _orderItems.Rows.Clear();
                RefreshOrderGrid();
            }
        }

        private void OpenPayment()
        {
            if (_orderItems.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có món nào!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            decimal.TryParse(txtDiscount.Text, out decimal disc);
            var payForm = new frmPayment(_tableID, _tableName, _orderItems, disc, _user);
            payForm.ShowDialog();
        }

        // ── Helpers ───────────────────────────────────
        private System.Windows.Forms.Label MakeLabel(string text, int x, int y,
            Color fore, Font font)
        {
            var lbl = new System.Windows.Forms.Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.ForeColor = fore;
            lbl.Font = font;
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;
            return lbl;
        }

        private Guna.UI2.WinForms.Guna2Button MakeActionBtn(string text, int x, int y,
            int w, int h, Color fill, Color ColorBusy = default)
        {
            var btn = new Guna.UI2.WinForms.Guna2Button();
            btn.Text = text;
            btn.BorderRadius = 10;
            btn.FillColor = fill;
            btn.ForeColor = ColorBusy == default ? Dark : ColorBusy;
            btn.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            btn.Size = new Size(w, h);
            btn.Location = new Point(x, y);
            return btn;
        }

        private Guna.UI2.WinForms.Guna2Button MakeCategoryBtn(string text, int id, int y, bool active)
        {
            var btn = new Guna.UI2.WinForms.Guna2Button();
            btn.Text = text;
            btn.BorderRadius = 0;
            btn.FillColor = active ? Purple : Color.Transparent;
            btn.ForeColor = active ? Color.White : Color.FromArgb(80, 80, 110);
            btn.Font = new Font("Segoe UI", 9.5f);
            btn.TextAlign = HorizontalAlignment.Left;
            btn.Size = new Size(120, 44);
            btn.Location = new Point(0, y);
            btn.Tag = id;
            return btn;
        }

        private void SetActiveCatBtn(Guna.UI2.WinForms.Guna2Button active)
        {
            foreach (Control c in pnlCategoryList.Controls)
            {
                if (c is Guna.UI2.WinForms.Guna2Button b)
                {
                    b.FillColor = b == active ? Purple : Color.Transparent;
                    b.ForeColor = b == active ? Color.White : Color.FromArgb(80, 80, 110);
                }
            }
        }
    }
}
