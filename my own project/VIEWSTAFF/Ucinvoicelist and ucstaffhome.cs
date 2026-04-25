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
    public class UcInvoiceList : UserControl
    {
        private readonly UserDTO _user;
        private Color Purple = Color.FromArgb(106, 90, 205);
        private Color Dark = Color.FromArgb(25, 23, 60);

        private Guna.UI2.WinForms.Guna2DateTimePicker dtpDate;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Panel pnlTabs, pnlList;
        private string _currentFilter = "all";

        public UcInvoiceList(UserDTO user)
        {
            _user = user;
            InitUI();
            LoadInvoices();
        }

        private void InitUI()
        {
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 9.5f);

            // Filter row
            var lblDate = new System.Windows.Forms.Label { Text = "Ngày", AutoSize = true, Location = new Point(16, 18), ForeColor = Dark, Font = new Font("Segoe UI", 9.5f) };
            dtpDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            dtpDate.BorderRadius = 8;
            dtpDate.FillColor = Color.FromArgb(245, 246, 250);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Value = DateTime.Today;
            dtpDate.Size = new Size(140, 34);
            dtpDate.Location = new Point(52, 12);
            dtpDate.ValueChanged += (s, e) => LoadInvoices();

            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            txtSearch.PlaceholderText = "Tìm theo mã hóa đơn, bàn...";
            txtSearch.BorderRadius = 8;
            txtSearch.FillColor = Color.FromArgb(245, 246, 250);
            txtSearch.Font = new Font("Segoe UI", 9.5f);
            txtSearch.Size = new Size(280, 34);
            txtSearch.Location = new Point(210, 12);

            btnSearch = new Guna.UI2.WinForms.Guna2Button();
            btnSearch.Text = "🔍";
            btnSearch.BorderRadius = 8;
            btnSearch.FillColor = Color.FromArgb(245, 246, 250);
            btnSearch.ForeColor = Dark;
            btnSearch.Size = new Size(40, 34);
            btnSearch.Location = new Point(496, 12);
            btnSearch.Click += (s, e) => LoadInvoices();

            // Status tabs
            pnlTabs = new Panel();
            pnlTabs.BackColor = Color.White;
            pnlTabs.Location = new Point(0, 58);
            pnlTabs.Size = new Size(1050, 48);
            pnlTabs.BorderStyle = BorderStyle.None;

            AddTabBtn(pnlTabs, "Tất cả", "all", 16);
            AddTabBtn(pnlTabs, "Đang phục vụ", "serving", 100);
            AddTabBtn(pnlTabs, "Đã thanh toán", "paid", 230);
            AddTabBtn(pnlTabs, "Đã hủy", "cancel", 370);

            var sep = new Panel { BackColor = Color.FromArgb(230, 230, 240), Location = new Point(0, 106), Size = new Size(1050, 1) };

            // List panel
            pnlList = new Panel();
            pnlList.BackColor = Color.White;
            pnlList.Location = new Point(0, 108);
            pnlList.Size = new Size(1050, 580);
            pnlList.AutoScroll = true;

            this.Controls.AddRange(new Control[] {
                lblDate, dtpDate, txtSearch, btnSearch,
                pnlTabs, sep, pnlList
            });
        }

        private void LoadInvoices()
        {
            pnlList.Controls.Clear();
            try
            {
                DataTable dt = OrderBLL.GetOrdersByStatus(_currentFilter == "all" ? null : MapFilter(_currentFilter));
                int y = 8;
                foreach (DataRow row in dt.Rows)
                {
                    var card = BuildInvoiceCard(row);
                    card.Location = new Point(8, y);
                    pnlList.Controls.Add(card);
                    y += 74;
                }
                if (dt.Rows.Count == 0)
                {
                    var lbl = new System.Windows.Forms.Label { Text = "Không có hóa đơn nào", AutoSize = true, ForeColor = Color.FromArgb(150, 150, 180), Font = new Font("Segoe UI", 11f), Location = new Point(400, 60) };
                    pnlList.Controls.Add(lbl);
                }
            }
            catch { }
        }

        private Panel BuildInvoiceCard(DataRow row)
        {
            string orderID = $"HD{Convert.ToInt32(row["OrderID"]):D6}";
            string table = $"Bàn {row["TableNumber"]}";
            string status = row["Status"].ToString();
            decimal total = Convert.ToDecimal(row["TotalAmount"]);

            Color statusColor; string statusText;
            switch (status)
            {
                case "Serving": statusColor = Color.FromArgb(255, 152, 0); statusText = "Đang phục vụ"; break;
                case "Paid": statusColor = Color.FromArgb(76, 175, 80); statusText = "Đã thanh toán"; break;
                case "Cancelled": statusColor = Color.FromArgb(229, 57, 53); statusText = "Đã hủy"; break;
                default: statusColor = Color.FromArgb(106, 90, 205); statusText = status; break;
            }

            var card = new Panel();
            card.BackColor = Color.White;
            card.Size = new Size(1030, 66);
            card.Padding = new Padding(8);
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 240)))
                    e.Graphics.DrawLine(pen, 0, card.Height - 1, card.Width, card.Height - 1);
            };

            var lblIcon = new System.Windows.Forms.Label { Text = status == "Paid" ? "📄" : status == "Cancelled" ? "❌" : "⏳", Font = new Font("Segoe UI", 18f), AutoSize = true, Location = new Point(8, 16), BackColor = Color.Transparent };
            var lblCode = new System.Windows.Forms.Label { Text = orderID, Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold), ForeColor = Color.FromArgb(25, 23, 60), AutoSize = true, Location = new Point(48, 10) };
            var lblTable = new System.Windows.Forms.Label { Text = table, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(120, 120, 150), AutoSize = true, Location = new Point(48, 32) };
            var lblTotal2 = new System.Windows.Forms.Label { Text = $"{total:N0} đ", Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(25, 23, 60), AutoSize = true, Location = new Point(500, 18) };

            // Status badge
            var pnlBadge = new Panel { BackColor = Color.FromArgb(statusColor.R, statusColor.G, statusColor.B, 30), Size = new Size(120, 26), Location = new Point(700, 18) };
            var lblBadge = new System.Windows.Forms.Label { Text = statusText, ForeColor = statusColor, Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold), AutoSize = true, Location = new Point(8, 4), BackColor = Color.Transparent };
            pnlBadge.Controls.Add(lblBadge);

            var btnDetail = new Guna.UI2.WinForms.Guna2Button();
            btnDetail.Text = "📋 Chi tiết";
            btnDetail.BorderRadius = 16;
            btnDetail.FillColor = Color.FromArgb(240, 238, 255);
            btnDetail.ForeColor = Purple;
            btnDetail.Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold);
            btnDetail.Size = new Size(90, 30);
            btnDetail.Location = new Point(930, 16);

            card.Controls.AddRange(new Control[] { lblIcon, lblCode, lblTable, lblTotal2, pnlBadge, btnDetail });
            return card;
        }

        private void AddTabBtn(Panel parent, string text, string filter, int x)
        {
            var btn = new Guna.UI2.WinForms.Guna2Button();
            btn.Text = text;
            btn.BorderRadius = 0;
            bool active = filter == _currentFilter;
            btn.FillColor = Color.Transparent;
            btn.ForeColor = active ? Purple : Color.FromArgb(100, 100, 130);
            btn.Font = active
                ? new Font("Segoe UI Semibold", 10f, FontStyle.Bold)
                : new Font("Segoe UI", 10f);
            btn.Size = new Size(120, 44);
            btn.Location = new Point(x, 0);
            btn.Tag = filter;
            btn.Click += (s, e) =>
            {
                _currentFilter = filter;
                foreach (Control c in parent.Controls)
                    if (c is Guna.UI2.WinForms.Guna2Button b2)
                    {
                        b2.ForeColor = (string)b2.Tag == filter ? Purple : Color.FromArgb(100, 100, 130);
                        b2.Font = (string)b2.Tag == filter ? new Font("Segoe UI Semibold", 10f, FontStyle.Bold) : new Font("Segoe UI", 10f);
                    }
                LoadInvoices();
            };
            parent.Controls.Add(btn);
        }

        private string MapFilter(string f)
        {
            switch (f)
            {
                case "serving": return "Serving";
                case "paid": return "Paid";
                case "cancel": return "Cancelled";
                default: return null;
            }
        }
    }

    // ════════════════════════════════════════════════════
    // ucStaffHome — Trang chủ nhân viên
    // ════════════════════════════════════════════════════
    public class ucStaffHome : UserControl
    {
        private readonly UserDTO _user;
        private Color Purple = Color.FromArgb(106, 90, 205);
        private Color Dark = Color.FromArgb(25, 23, 60);

        public ucStaffHome(UserDTO user)
        {
            _user = user;
            InitUI();
        }

        private void InitUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 9.5f);

            int cW = 230, cH = 110, gap = 20;

            var cardFree = BuildCard("🪑", "Bàn trống", "0", Color.FromArgb(76, 175, 80), 16, 16, cW, cH);
            var cardBusy = BuildCard("🔴", "Đang phục vụ", "0", Color.FromArgb(229, 57, 53), 16 + (cW + gap), 16, cW, cH);
            var cardOrders = BuildCard("📋", "Order hôm nay", "0", Purple, 16 + (cW + gap) * 2, 16, cW, cH);
            var cardRevenue = BuildCard("💰", "Doanh thu hôm nay", "0 đ", Color.FromArgb(255, 152, 0), 16 + (cW + gap) * 3, 16, cW, cH);

            var lblHint = new System.Windows.Forms.Label
            {
                Text = $"Xin chào, {_user?.FullName ?? "nhân viên"}! Chúc bạn làm việc hiệu quả hôm nay 🎉",
                Font = new Font("Segoe UI Semibold", 13f, FontStyle.Bold),
                ForeColor = Dark,
                AutoSize = true,
                Location = new Point(16, 148)
            };

            this.Controls.AddRange(new Control[] { cardFree, cardBusy, cardOrders, cardRevenue, lblHint });
        }

        private Panel BuildCard(string icon, string title, string val, Color color, int x, int y, int w, int h)
        {
            var card = new Panel { BackColor = Color.White, Size = new Size(w, h), Location = new Point(x, y) };
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 240)))
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };
            card.Controls.Add(new System.Windows.Forms.Label { Text = icon, Font = new Font("Segoe UI", 22f), AutoSize = true, Location = new Point(12, 12), BackColor = Color.Transparent });
            card.Controls.Add(new System.Windows.Forms.Label { Text = val, Font = new Font("Segoe UI Semibold", 22f, FontStyle.Bold), ForeColor = color, AutoSize = true, Location = new Point(12, 48), BackColor = Color.Transparent });
            card.Controls.Add(new System.Windows.Forms.Label { Text = title, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(120, 120, 150), AutoSize = true, Location = new Point(14, 84), BackColor = Color.Transparent });
            return card;
        }
    }
}
