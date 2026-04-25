using Guna.UI2.WinForms;
using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using my_own_project.BLL;


namespace my_own_project.VIEWSTAFF
{
    public class UcTableManagement : UserControl    
    {
        // Event báo lên frmStaffMain để chuyển sang Order
        public event Action<int, string> OnOpenOrder;

        private readonly UserDTO _user;

        // ── Controls ──────────────────────────────────
        private Guna2Panel pnlTop;
        private Guna2Panel pnlStats;
        private Guna2Panel pnlLegend;
        private Guna2Panel pnlTables;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotal, lblTotalVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFree, lblFreeVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBusy, lblBusyVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRes, lblResVal;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFooter;

        private Color ColorFree = Color.FromArgb(76, 175, 80);
        private Color ColorBusy = Color.FromArgb(229, 57, 53);
        private Color ColorReserv = Color.FromArgb(255, 152, 0);
        private Color ColorDark = Color.FromArgb(25, 23, 60);

        public UcTableManagement(UserDTO user)
        {
            _user = user;
            InitUI();
            LoadTables();
        }

        private void InitUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 9.5f);

            // ── Stats row ──────────────────────────────
            pnlStats = new Guna2Panel();
            pnlStats.BackColor = Color.White;
            pnlStats.Location = new Point(16, 12);
            pnlStats.Size = new Size(1020, 80);
            BuildStatCard(pnlStats, out lblTotal, out lblTotalVal, 16, "Tổng số bàn", "🪑", ColorDark);
            BuildStatCard(pnlStats, out lblFree, out lblFreeVal, 276, "Trống", "✅", ColorFree);
            BuildStatCard(pnlStats, out lblBusy, out lblBusyVal, 536, "Đang sử dụng", "🔴", ColorBusy);
            BuildStatCard(pnlStats, out lblRes, out lblResVal, 796, "Đặt trước", "🟡", ColorReserv);

            // ── Legend + Refresh ───────────────────────
            pnlLegend = new Guna2Panel();
            pnlLegend.BackColor = Color.White;
            pnlLegend.Location = new Point(16, 102);
            pnlLegend.Size = new Size(1020, 46);
            AddLegend(pnlLegend, "● Trống", ColorFree, 16);
            AddLegend(pnlLegend, "● Đang sử dụng", ColorBusy, 110);
            AddLegend(pnlLegend, "● Đặt trước", ColorReserv, 240);

            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            btnRefresh.Text = "↺  Làm mới";
            btnRefresh.BorderRadius = 20;
            btnRefresh.FillColor = Color.FromArgb(25, 23, 60);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            btnRefresh.Size = new Size(110, 32);
            btnRefresh.Location = new Point(896, 7);
            btnRefresh.Click += (s, e) => LoadTables();
            pnlLegend.Controls.Add(btnRefresh);

            // ── Tables panel (scrollable) ──────────────
            pnlTables = new Guna2Panel();
            pnlTables.BackColor = Color.FromArgb(245, 246, 250);
            pnlTables.Location = new Point(16, 158);
            pnlTables.Size = new Size(1020, 510);
            pnlTables.AutoScroll = true;

            // ── Footer hint ────────────────────────────
            lblFooter = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblFooter.Text = "ℹ  Nhấp vào bàn để xem chi tiết và gọi món";
            lblFooter.ForeColor = Color.FromArgb(120, 120, 160);
            lblFooter.Font = new Font("Segoe UI", 9f);
            lblFooter.AutoSize = true;
            lblFooter.Location = new Point(16, 680);

            this.Controls.AddRange(new Control[] { pnlStats, pnlLegend, pnlTables, lblFooter });
        }

        private void LoadTables()
        {
            pnlTables.Controls.Clear();
            try
            {
                DataTable dt = DiningTableBLL.GetAllTables(); // RestaurantID = 1

                int total = dt.Rows.Count;
                int free = 0, busy = 0, res = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string s = row["Status"].ToString();
                    if (s == "Available") free++;
                    else if (s == "Occupied") busy++;
                    else if (s == "Reserved") res++;
                }
                lblTotalVal.Text = total.ToString();
                lblFreeVal.Text = free.ToString();
                lblBusyVal.Text = busy.ToString();
                lblResVal.Text = res.ToString();

                // Group by area (Notes field used as area label)
                string lastArea = "";
                int x = 0, y = 0, cardW = 185, cardH = 130, gap = 14;
                int maxCols = 5;
                int col = 0;

                foreach (DataRow row in dt.Rows)
                {
                    // Area header
                    string area = row["Notes"]?.ToString() ?? "";
                    if (area != lastArea && !string.IsNullOrEmpty(area))
                    {
                        var areaLbl = new System.Windows.Forms.Label
                        {
                            Text = area.ToUpper(),
                            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                            ForeColor = Color.FromArgb(100, 100, 130),
                            AutoSize = true,
                            Location = new Point(0, y)
                        };
                        pnlTables.Controls.Add(areaLbl);
                        y += 28;
                        x = 0;
                        col = 0;
                        lastArea = area;
                    }

                    // Table card
                    var card = BuildTableCard(row, cardW, cardH);
                    card.Location = new Point(x, y);
                    pnlTables.Controls.Add(card);

                    col++;
                    if (col >= maxCols) { col = 0; x = 0; y += cardH + gap; }
                    else x += cardW + gap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bàn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Guna.UI2.WinForms.Guna2Panel BuildTableCard(DataRow row, int w, int h)
        {
            string status = row["Status"].ToString();
            string tableNum = row["TableNumber"].ToString();
            string cap = row["Capacity"].ToString();
            int tableID = Convert.ToInt32(row["TableID"]);
            string tableName = $"Bàn {tableNum}";

            Color bgColor, borderColor, statusColor;
            string statusText, iconText;
            switch (status)
            {
                case "Occupied":
                    bgColor = Color.FromArgb(255, 235, 235);
                    borderColor = ColorBusy;
                    statusColor = ColorBusy;
                    statusText = "Đang sử dụng";
                    iconText = "🔴";
                    break;
                case "Reserved":
                    bgColor = Color.FromArgb(255, 243, 220);
                    borderColor = ColorReserv;
                    statusColor = ColorReserv;
                    statusText = "Đặt trước";
                    iconText = "🟡";
                    break;
                default:
                    bgColor = Color.White;
                    borderColor = Color.FromArgb(220, 220, 235);
                    statusColor = ColorFree;
                    statusText = "Trống";
                    iconText = "🟢";
                    break;
            }

            var card = new Guna.UI2.WinForms.Guna2Panel();
            card.BackColor = bgColor;
            card.Size = new Size(w, h);
            card.Cursor = Cursors.Hand;
            card.Tag = tableID;

            // Border via Paint
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(borderColor, 1.5f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            var lblIcon = new System.Windows.Forms.Label { Text = "🪑", Font = new Font("Segoe UI", 20f), AutoSize = true, BackColor = Color.Transparent, Location = new Point(12, 12), ForeColor = statusColor };
            var lblName = new System.Windows.Forms.Label { Text = tableName, Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold), AutoSize = true, BackColor = Color.Transparent, Location = new Point(52, 12), ForeColor = ColorDark };
            var lblCap = new System.Windows.Forms.Label { Text = $"{cap} chỗ", Font = new Font("Segoe UI", 9f), AutoSize = true, BackColor = Color.Transparent, Location = new Point(54, 36), ForeColor = Color.FromArgb(120, 120, 150) };
            var lblStat = new System.Windows.Forms.Label { Text = $"{iconText}  {statusText}", Font = new Font("Segoe UI", 9.5f), AutoSize = true, BackColor = Color.Transparent, Location = new Point(12, 72), ForeColor = statusColor };

            card.Controls.AddRange(new Control[] { lblIcon, lblName, lblCap, lblStat });

            // Click → open order
            void OpenCard(object s, EventArgs e) => OnOpenOrder?.Invoke(tableID, tableName);
            card.Click += OpenCard;
            lblIcon.Click += OpenCard;
            lblName.Click += OpenCard;
            lblCap.Click += OpenCard;
            lblStat.Click += OpenCard;

            return card;
        }

        // ── Helpers ───────────────────────────────────
        private void BuildStatCard(Guna.UI2.WinForms.Guna2Panel parent,
            out Guna.UI2.WinForms.Guna2HtmlLabel lbl,
            out Guna.UI2.WinForms.Guna2HtmlLabel val,
            int x, string title, string icon, Color color)
        {
            lbl = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbl.AutoSize = true;
            lbl.Text = $"{icon}  {title}";
            lbl.Font = new Font("Segoe UI", 9f);
            lbl.ForeColor = Color.FromArgb(120, 120, 150);
            lbl.Location = new Point(x, 14);
            parent.Controls.Add(lbl);

            val = new Guna.UI2.WinForms.Guna2HtmlLabel();
            val.AutoSize = true;
            val.Text = "0";
            val.Font = new Font("Segoe UI Semibold", 24f, FontStyle.Bold);
            val.ForeColor = color;
            val.Location = new Point(x, 34);
            parent.Controls.Add(val);
        }

        private void AddLegend(Guna.UI2.WinForms.Guna2Panel parent, string text, Color color, int x)
        {
            var lbl = new System.Windows.Forms.Label();
            lbl.Text = text;
            lbl.ForeColor = color;
            lbl.Font = new Font("Segoe UI", 9.5f);
            lbl.AutoSize = true;
            lbl.Location = new Point(x, 14);
            parent.Controls.Add(lbl);
        }
    }
}
