using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DAL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class HistoryForm : Form
    {
        // ===================== DESIGN TOKENS =====================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_MID = Color.FromArgb(109, 60, 240);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(237, 233, 254);
        private static readonly Color C_GREEN = Color.FromArgb(22, 163, 74);
        private static readonly Color C_GREEN_SOFT = Color.FromArgb(220, 252, 231);
        private static readonly Color C_BLUE = Color.FromArgb(37, 99, 235);
        private static readonly Color C_BLUE_SOFT = Color.FromArgb(219, 234, 254);
        private static readonly Color C_AMBER = Color.FromArgb(217, 119, 6);
        private static readonly Color C_AMBER_SOFT = Color.FromArgb(254, 243, 199);
        private static readonly Color C_RED = Color.FromArgb(220, 38, 38);
        private static readonly Color C_RED_SOFT = Color.FromArgb(254, 226, 226);
        private static readonly Color C_TEXT = Color.FromArgb(17, 24, 39);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);

        // ===================== CONTROLS =====================
        private Guna2DateTimePicker dtpFrom, dtpTo;
        private Guna2Button btnFilter, btnExport;
        private Guna2Button btnToday, btn7Days, btn30Days, btnThisMonth;
        private Label lblTotalRevenue, lblTotalOrders, lblAvgOrder;
        private Label lblLastUpdated, lblRowCount, lblHint;
        private DataGridView dgvHistory;

        // Print
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;
        private int selectedOrderID = -1;
        private decimal selectedTotal = 0;
        private string selectedDate = "";

        public HistoryForm()
        {
            InitializeComponent();
            Controls.Clear();
            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;

            BuildUI();

            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 315, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;
            printPreview = new PrintPreviewDialog
            {
                Document = printDoc,
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(450, 650)
            };
            printPreview.PrintPreviewControl.Zoom = 1.0;

            Load += (s, e) => LoadData();
        }

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            SuspendLayout();

            Panel header = BuildHeader();
            Panel filterBar = BuildFilterBar();
            Panel statCards = BuildStatCards();
            Panel gridHeader = BuildGridHeader();

            // DataGridView
            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(243, 244, 246),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Cursor = Cursors.Hand,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 44,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            // Header style
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHistory.ColumnHeadersDefaultCellStyle.Padding = new Padding(14, 0, 0, 0);

            // Row style
            dgvHistory.DefaultCellStyle.BackColor = C_WHITE;
            dgvHistory.DefaultCellStyle.ForeColor = C_TEXT;
            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dgvHistory.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            dgvHistory.DefaultCellStyle.SelectionForeColor = C_TEXT;
            dgvHistory.DefaultCellStyle.Padding = new Padding(14, 0, 0, 0);
            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 253);
            dgvHistory.RowTemplate.Height = 48;

            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;
            dgvHistory.CellFormatting += DgvHistory_CellFormatting;

            // Assemble (Fill first, then Top panels)
            Controls.Add(dgvHistory);
            Controls.Add(gridHeader);
            Controls.Add(statCards);
            Controls.Add(filterBar);
            Controls.Add(header);

            ResumeLayout(false);
        }

        // ── Header ─────────────────────────────────────────────────────────
        private Panel BuildHeader()
        {
            Panel h = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = C_WHITE
            };
            h.Paint += PaintBottomBorder;

            // Accent strip on the left
            Panel accent = new Panel
            {
                Size = new Size(4, 68),
                Location = new Point(0, 0),
                BackColor = C_PURPLE
            };

            
            Label title = new Label
            {
                Text = "Lịch sử doanh thu",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(52, 14)
            };

            lblLastUpdated = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(53, 44)
            };

            h.Controls.AddRange(new Control[] { accent, title, lblLastUpdated });
            return h;
        }

        // ── Filter bar ──────────────────────────────────────────────────────
        private Panel BuildFilterBar()
        {
            Panel bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 62,
                BackColor = C_WHITE,
                Padding = new Padding(16, 12, 16, 0)
            };
            bar.Paint += PaintBottomBorder;

            // Quick-range pill buttons
            btnToday = QuickBtn("Hôm nay", 16, true);
            btn7Days = QuickBtn("7 ngày", 106, false);
            btn30Days = QuickBtn("30 ngày", 196, false);
            btnThisMonth = QuickBtn("Tháng này", 286, false);

            btnToday.Click += (s, e) => { dtpFrom.Value = DateTime.Today; dtpTo.Value = DateTime.Today; SetQuickActive(btnToday); LoadData(); };
            btn7Days.Click += (s, e) => { dtpFrom.Value = DateTime.Today.AddDays(-6); dtpTo.Value = DateTime.Today; SetQuickActive(btn7Days); LoadData(); };
            btn30Days.Click += (s, e) => { dtpFrom.Value = DateTime.Today.AddDays(-29); dtpTo.Value = DateTime.Today; SetQuickActive(btn30Days); LoadData(); };
            btnThisMonth.Click += (s, e) => { dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); dtpTo.Value = DateTime.Today; SetQuickActive(btnThisMonth); LoadData(); };

            // Separator label
            Label lblFrom = new Label { Text = "Từ:", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, AutoSize = true, Location = new Point(392, 21) };

            dtpFrom = new Guna2DateTimePicker
            {
                Size = new Size(126, 36),
                Location = new Point(416, 12),
                BorderRadius = 8,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(-29),
                BorderColor = C_BORDER,
                FillColor = C_WHITE
            };

            Label lblTo = new Label { Text = "đến:", Font = new Font("Segoe UI", 9.5F), ForeColor = C_MUTED, AutoSize = true, Location = new Point(548, 21) };

            dtpTo = new Guna2DateTimePicker
            {
                Size = new Size(126, 36),
                Location = new Point(578, 12),
                BorderRadius = 8,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                BorderColor = C_BORDER,
                FillColor = C_WHITE
            };

            btnFilter = new Guna2Button
            {
                Text = "🔍 Lọc",
                Size = new Size(88, 36),
                Location = new Point(710, 12),
                BorderRadius = 8,
                BorderThickness = 0,
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFilter.Click += (s, e) => { SetQuickActive(null); LoadData(); };

            btnExport = new Guna2Button
            {
                Text = "⬇ Xuất CSV",
                Size = new Size(110, 36),
                Location = new Point(806, 12),
                BorderRadius = 8,
                BorderThickness = 0,
                FillColor = C_GREEN_SOFT,
                ForeColor = C_GREEN,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExport.Click += BtnExport_Click;

            bar.Controls.AddRange(new Control[]
            {
                btnToday, btn7Days, btn30Days, btnThisMonth,
                lblFrom, dtpFrom, lblTo, dtpTo, btnFilter, btnExport
            });

            return bar;
        }

        private Guna2Button QuickBtn(string text, int x, bool active)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(84, 36),
                Location = new Point(x, 12),
                BorderRadius = 18,
                BorderThickness = 0,
                FillColor = active ? C_PURPLE : C_PURPLE_SOFT,
                ForeColor = active ? Color.White : C_PURPLE,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void SetQuickActive(Guna2Button active)
        {
            foreach (var b in new[] { btnToday, btn7Days, btn30Days, btnThisMonth })
            {
                if (b == null) continue;
                b.FillColor = (b == active) ? C_PURPLE : C_PURPLE_SOFT;
                b.ForeColor = (b == active) ? Color.White : C_PURPLE;
            }
        }

        // ── Stat cards ───────────────────────────────────────────────────────
        private Panel BuildStatCards()
        {
            Panel row = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = C_BG,
                Padding = new Padding(16, 12, 16, 0)
            };

            Panel c1 = StatCard("TỔNG DOANH THU", "💰", C_GREEN, C_GREEN_SOFT, out lblTotalRevenue);
            Panel c2 = StatCard("SỐ ĐƠN HÀNG", "🧾", C_PURPLE, C_PURPLE_SOFT, out lblTotalOrders);
            Panel c3 = StatCard("GIÁ TRỊ TB/ĐƠN", "📈", C_BLUE, C_BLUE_SOFT, out lblAvgOrder);

            c1.Location = new Point(16, 12);
            c2.Location = new Point(246, 12);
            c3.Location = new Point(476, 12);

            row.Controls.AddRange(new Control[] { c1, c2, c3 });
            return row;
        }

        private Panel StatCard(string title, string icon, Color accent, Color softBg, out Label lblVal)
        {
            Panel card = new Panel
            {
                Size = new Size(220, 76),
                BackColor = C_WHITE
            };

            // Rounded corners via Paint
            card.Paint += (s, e) =>
            {
                var p = s as Panel;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int r = 12, w = p.Width - 1, h = p.Height - 1;
                    path.AddArc(0, 0, r * 2, r * 2, 180, 90);
                    path.AddArc(w - r * 2, 0, r * 2, r * 2, 270, 90);
                    path.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
                    path.AddArc(0, h - r * 2, r * 2, r * 2, 90, 90);
                    path.CloseFigure();
                    p.Region = new Region(path);
                }
            };

            // Left accent bar
            Panel bar = new Panel { Size = new Size(4, 76), Location = new Point(0, 0), BackColor = accent };

            Label lTitle = new Label
            {
                Text = icon + "  " + title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = C_MUTED,
                Location = new Point(16, 10),
                AutoSize = true
            };

            lblVal = new Label
            {
                Text = "—",
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(16, 32),
                AutoSize = true
            };

            card.Controls.AddRange(new Control[] { bar, lTitle, lblVal });
            return card;
        }

        // ── Grid header bar ─────────────────────────────────────────────────
        private Panel BuildGridHeader()
        {
            Panel bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = C_WHITE,
                Padding = new Padding(20, 0, 20, 0)
            };
            bar.Paint += PaintBottomBorder;

            Label lblTitle = new Label
            {
                Text = "Danh sách hóa đơn",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(20, 14)
            };

            lblRowCount = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            lblHint = new Label
            {
                Text = "✦  Nhấp đúp vào hóa đơn để in lại",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = C_MUTED,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            bar.Controls.AddRange(new Control[] { lblTitle, lblRowCount, lblHint });
            bar.Resize += (s, e) =>
            {
                lblHint.Location = new Point(bar.Width - lblHint.Width - 20, 16);
                lblRowCount.Location = new Point(lblHint.Left - lblRowCount.Width - 24, 16);
            };

            return bar;
        }

        // ===================== LOAD DATA =====================
        private void LoadData()
        {
            try
            {
                string fromDate = dtpFrom.Value.ToString("yyyyMMdd");
                string toDate = dtpTo.Value.ToString("yyyyMMdd");

                string query = $@"
                    SELECT
                        o.OrderID                                            AS [Mã HĐ],
                        o.OrderDate                                          AS [Ngày giờ],
                        ISNULL(CAST(t.TableNumber AS NVARCHAR), N'Mang đi') AS [Bàn],
                        o.OrderType                                          AS [Loại],
                        o.TotalAmount                                        AS [Tổng tiền]
                    FROM   Orders o
                    LEFT  JOIN DiningTable t ON o.TableID = t.TableID
                    WHERE  o.Status = 'Completed'
                      AND  CAST(o.OrderDate AS DATE) >= '{fromDate}'
                      AND  CAST(o.OrderDate AS DATE) <= '{toDate}'
                    ORDER  BY o.OrderDate DESC";

                DataTable dt = DataHelper.ExecuteQuery(query);
                dgvHistory.DataSource = dt;

                // Format columns
                if (dgvHistory.Columns.Contains("Tổng tiền"))
                {
                    var col = dgvHistory.Columns["Tổng tiền"];
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Padding = new Padding(0, 0, 18, 0);
                }

                // Stat cards
                decimal totalRev = 0;
                int count = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                    totalRev += row["Tổng tiền"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Tổng tiền"]);

                lblTotalRevenue.Text = totalRev.ToString("N0") + " đ";
                lblTotalOrders.Text = count + " đơn";
                lblAvgOrder.Text = count > 0 ? (totalRev / count).ToString("N0") + " đ" : "—";

                // Row count + timestamp
                if (lblRowCount != null) lblRowCount.Text = count + " hóa đơn";
                if (lblLastUpdated != null)
                    lblLastUpdated.Text = $"Cập nhật lúc {DateTime.Now:HH:mm:ss}  ·  "
                        + $"{dtpFrom.Value:dd/MM/yyyy} — {dtpTo.Value:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== CELL FORMATTING =====================
        private void DgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string col = dgvHistory.Columns[e.ColumnIndex].Name;

            if (col == "Tổng tiền" && e.Value != null)
            {
                e.CellStyle.ForeColor = C_RED;
                e.CellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                e.CellStyle.SelectionForeColor = C_RED;
            }

            if (col == "Loại" && e.Value != null)
            {
                string v = e.Value.ToString();
                switch (v)
                {
                    case "DineIn":
                        e.CellStyle.ForeColor = C_BLUE;
                        e.CellStyle.BackColor = C_BLUE_SOFT;
                        e.CellStyle.SelectionForeColor = C_BLUE;
                        e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        break;
                    case "TakeAway":
                        e.CellStyle.ForeColor = C_AMBER;
                        e.CellStyle.BackColor = C_AMBER_SOFT;
                        e.CellStyle.SelectionForeColor = C_AMBER;
                        e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        break;
                    default:
                        e.CellStyle.ForeColor = C_GREEN;
                        e.CellStyle.BackColor = C_GREEN_SOFT;
                        e.CellStyle.SelectionForeColor = C_GREEN;
                        e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        break;
                }
            }
        }

        // ===================== EXPORT CSV =====================
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvHistory.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV files (*.csv)|*.csv";
                dlg.FileName = $"DoanhThu_{dtpFrom.Value:yyyyMMdd}_{dtpTo.Value:yyyyMMdd}.csv";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var sb = new System.Text.StringBuilder();

                // Header row
                for (int i = 0; i < dgvHistory.Columns.Count; i++)
                {
                    sb.Append(dgvHistory.Columns[i].HeaderText);
                    if (i < dgvHistory.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();

                // Data rows
                foreach (DataGridViewRow row in dgvHistory.Rows)
                {
                    for (int i = 0; i < dgvHistory.Columns.Count; i++)
                    {
                        string val = row.Cells[i].Value?.ToString()?.Replace(",", ";") ?? "";
                        sb.Append(val);
                        if (i < dgvHistory.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();
                }

                System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                MessageBox.Show($"✔ Xuất thành công!\n{dlg.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ===================== DOUBLE-CLICK → PRINT PREVIEW =====================
        private void DgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                selectedOrderID = Convert.ToInt32(dgvHistory.Rows[e.RowIndex].Cells["Mã HĐ"].Value);
                selectedTotal = Convert.ToDecimal(dgvHistory.Rows[e.RowIndex].Cells["Tổng tiền"].Value);
                selectedDate = Convert.ToDateTime(dgvHistory.Rows[e.RowIndex].Cells["Ngày giờ"].Value).ToString("dd/MM/yyyy HH:mm");
                printPreview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===================== PRINT PAGE =====================
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var fTitle = new Font("Courier New", 18, FontStyle.Bold);
            var fSub = new Font("Courier New", 11, FontStyle.Regular);
            var fHeader = new Font("Courier New", 13, FontStyle.Bold);
            var fItem = new Font("Courier New", 11, FontStyle.Regular);
            var fBold = new Font("Courier New", 11, FontStyle.Bold);
            var center = new StringFormat { Alignment = StringAlignment.Center };
            var right = new StringFormat { Alignment = StringAlignment.Far };
            int y = 10, lx = 5, cx = 157, rx = 300;
            string line = new string('-', 33);

            g.DrawString("PBL3 RESTAURANT", fTitle, Brushes.Black, new PointF(cx, y), center); y += 30;
            g.DrawString("Địa chỉ: Đà Nẵng", fSub, Brushes.Black, new PointF(cx, y), center); y += 20;
            g.DrawString("Hotline: 0123.456.789", fSub, Brushes.Black, new PointF(cx, y), center); y += 35;
            g.DrawString("BẢN SAO HÓA ĐƠN", fHeader, Brushes.Black, new PointF(cx, y), center); y += 35;
            g.DrawString("Mã HD: " + selectedOrderID, fItem, Brushes.Black, lx, y); y += 20;
            g.DrawString("Ngày : " + selectedDate, fItem, Brushes.Black, lx, y); y += 25;
            g.DrawString(line, fItem, Brushes.Black, lx, y); y += 20;
            g.DrawString("Tên", fBold, Brushes.Black, lx, y);
            g.DrawString("SL", fBold, Brushes.Black, 170, y);
            g.DrawString("T.Tiền", fBold, Brushes.Black, rx, y, right); y += 25;
            g.DrawString(line, fItem, Brushes.Black, lx, y); y += 20;

            var dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(selectedOrderID);
            decimal subTotal = 0;
            foreach (DataRow row in dtDetails.Rows)
            {
                string name = row["ItemName"].ToString();
                if (name.Length > 15) name = name.Substring(0, 15) + "..";
                string qty = row["Quantity"].ToString();
                decimal rowSub = Convert.ToDecimal(row["SubTotal"]);
                subTotal += rowSub;

                g.DrawString(name, fItem, Brushes.Black, lx, y);
                g.DrawString(qty, fItem, Brushes.Black, 170, y);
                g.DrawString(rowSub.ToString("N0"), fItem, Brushes.Black, rx, y, right);
                y += 25;
            }

            g.DrawString(line, fItem, Brushes.Black, lx, y); y += 25;

            decimal discount = subTotal - selectedTotal;
            if (discount > 0)
            {
                g.DrawString("Tạm tính:", fBold, Brushes.Black, lx, y);
                g.DrawString(subTotal.ToString("N0") + " đ", fBold, Brushes.Black, rx, y, right); y += 25;
                g.DrawString("Khuyến mãi:", fBold, Brushes.Black, lx, y);
                g.DrawString("-" + discount.ToString("N0") + " đ", fBold, Brushes.Black, rx, y, right); y += 25;
            }

            g.DrawString("TỔNG CỘNG:", fHeader, Brushes.Black, lx, y);
            g.DrawString(selectedTotal.ToString("N0") + " đ", fHeader, Brushes.Black, rx, y, right); y += 45;
            g.DrawString("*** BẢN SAO (REPRINT) ***", fSub, Brushes.Black, new PointF(cx, y), center);
        }

        // ===================== PAINT HELPER =====================
        private void PaintBottomBorder(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (Pen pen = new Pen(C_BORDER, 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }
    }
}