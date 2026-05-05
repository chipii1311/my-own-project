//using Guna.UI2.WinForms;
//using my_own_project.BLL; // Nhớ có dòng này để gọi chi tiết đơn hàng
//using System;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Printing; // Thư viện vẽ máy in
//using System.Windows.Forms;

//namespace my_own_project.VIEW
//{
//    public partial class HistoryForm : Form
//    {
//        // ========================================================
//        // KHAI BÁO BIẾN TOÀN CỤC
//        // ========================================================
//        private Guna2DateTimePicker dtpFrom;
//        private Guna2DateTimePicker dtpTo;
//        private Guna2Button btnFilter;
//        private Label lblTotalRevenue;
//        private DataGridView dgvHistory;

//        // BỘ ĐỒ NGHỀ XEM LẠI HÓA ĐƠN
//        private PrintDocument printDoc;
//        private PrintPreviewDialog printPreview;
//        private int selectedOrderID = -1;
//        private decimal selectedTotal = 0;
//        private string selectedDate = "";

//        public HistoryForm()
//        {
//            InitializeComponent();
//            this.Controls.Clear();

//            InitializeModernUI(); // Vẽ giao diện

//            // Cấu hình máy in ảo khổ 80mm
//            printDoc = new PrintDocument();
//            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 315, 600);
//            printDoc.PrintPage += PrintDoc_PrintPage;

//            printPreview = new PrintPreviewDialog();
//            printPreview.Document = printDoc;
//            printPreview.StartPosition = FormStartPosition.CenterScreen;
//            printPreview.Size = new Size(450, 650);
//            printPreview.PrintPreviewControl.Zoom = 1.0;

//            // Gắn sự kiện Load form
//            this.Load += HistoryForm_Load;
//        }

//        // ========================================================
//        #region 1. KHU VỰC VẼ GIAO DIỆN (UI BUILDER)
//        // ========================================================

//        private void InitializeModernUI()
//        {
//            this.BackColor = Color.FromArgb(245, 246, 250);
//            this.FormBorderStyle = FormBorderStyle.None;

//            // --- 1. THANH CÔNG CỤ NẰM TRÊN CÙNG ---
//            Guna2Panel pnlTop = new Guna2Panel();
//            pnlTop.Dock = DockStyle.Top;
//            pnlTop.Height = 100;
//            pnlTop.FillColor = Color.White;
//            pnlTop.CustomBorderThickness = new Padding(0, 0, 0, 1);
//            pnlTop.CustomBorderColor = Color.LightGray;
//            this.Controls.Add(pnlTop);

//            // Ép thanh Top nằm đè lên trên để không bị Grid che
//            pnlTop.BringToFront();

//            Label lblTitle = new Label { Text = "LỊCH SỬ DOANH THU", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(20, 25), AutoSize = true, BackColor = Color.White };
//            pnlTop.Controls.Add(lblTitle);

//            // Dòng hướng dẫn nhỏ
//            Label lblHint = new Label { Text = "(Nhấp đúp chuột vào một dòng để xem chi tiết Bill)", Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(23, 60), AutoSize = true, BackColor = Color.White };
//            pnlTop.Controls.Add(lblHint);

//            Label lblFrom = new Label { Text = "Từ ngày:", Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(350, 42), AutoSize = true, BackColor = Color.White };
//            pnlTop.Controls.Add(lblFrom);

//            dtpFrom = new Guna2DateTimePicker { Location = new Point(420, 32), Size = new Size(130, 40), BorderRadius = 8, Format = DateTimePickerFormat.Short, FillColor = Color.FromArgb(240, 240, 240), Value = DateTime.Today };
//            pnlTop.Controls.Add(dtpFrom);

//            Label lblTo = new Label { Text = "Đến ngày:", Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(570, 42), AutoSize = true, BackColor = Color.White };
//            pnlTop.Controls.Add(lblTo);

//            dtpTo = new Guna2DateTimePicker { Location = new Point(650, 32), Size = new Size(130, 40), BorderRadius = 8, Format = DateTimePickerFormat.Short, FillColor = Color.FromArgb(240, 240, 240), Value = DateTime.Today };
//            pnlTop.Controls.Add(dtpTo);

//            btnFilter = new Guna2Button { Text = "Lọc dữ liệu", Location = new Point(800, 32), Size = new Size(110, 40), BorderRadius = 8, FillColor = Color.FromArgb(88, 28, 230), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
//            btnFilter.Click += BtnFilter_Click; // Đã tách xuống Khu vực 3
//            pnlTop.Controls.Add(btnFilter);

//            Label lblTotalText = new Label { Text = "Tổng doanh thu:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, AutoSize = true, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right };
//            lblTotalText.Location = new Point(pnlTop.Width - 200, 20);
//            pnlTop.Controls.Add(lblTotalText);

//            lblTotalRevenue = new Label { Text = "0 đ", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Size = new Size(180, 35), AutoSize = false, TextAlign = ContentAlignment.MiddleRight, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right };
//            lblTotalRevenue.Location = new Point(pnlTop.Width - 200, 45);
//            pnlTop.Controls.Add(lblTotalRevenue);

//            // --- 2. TẠO BẢNG DỮ LIỆU ---
//            dgvHistory = new DataGridView();
//            dgvHistory.Location = new Point(20, 120);
//            dgvHistory.Size = new Size(this.Width - 40, this.Height - 140);
//            dgvHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

//            dgvHistory.AllowUserToAddRows = false;
//            dgvHistory.ReadOnly = true;
//            dgvHistory.BackgroundColor = Color.White;
//            dgvHistory.BorderStyle = BorderStyle.None;
//            dgvHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
//            dgvHistory.GridColor = Color.FromArgb(230, 230, 230);
//            dgvHistory.RowHeadersVisible = false;
//            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

//            dgvHistory.EnableHeadersVisualStyles = false;
//            dgvHistory.ColumnHeadersHeight = 50;
//            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
//            dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
//            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(88, 28, 230);
//            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

//            dgvHistory.DefaultCellStyle.BackColor = Color.White;
//            dgvHistory.DefaultCellStyle.ForeColor = Color.Black;
//            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
//            dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 220, 255);
//            dgvHistory.DefaultCellStyle.SelectionForeColor = Color.Black;
//            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
//            dgvHistory.Cursor = Cursors.Hand;

//            // Gắn sự kiện nhấp đúp chuột
//            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;

//            this.Controls.Add(dgvHistory);
//            dgvHistory.BringToFront();
//        }

//        #endregion


//        // ========================================================
//        #region 2. KHU VỰC CHỨC NĂNG & LOGIC DATABASE
//        // ========================================================

//        private void LoadHistoryData()
//        {
//            try
//            {
//                string fromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
//                string toDate = dtpTo.Value.ToString("yyyy-MM-dd");

//                string query = $@"
//                    SELECT 
//                        o.OrderID AS [Mã HĐ],
//                        o.OrderDate AS [Ngày giờ],
//                        ISNULL(CAST(t.TableNumber AS VARCHAR), N'Mang đi') AS [Bàn],
//                        SUM(od.Quantity * od.UnitPrice) AS [Tổng tiền]
//                    FROM Orders o
//                    LEFT JOIN DiningTable t ON o.TableID = t.TableID
//                    INNER JOIN OrderDetail od ON o.OrderID = od.OrderID
//                    WHERE o.Status = 'Completed' 
//                      AND CAST(o.OrderDate AS DATE) >= '{fromDate}' 
//                      AND CAST(o.OrderDate AS DATE) <= '{toDate}'
//                    GROUP BY o.OrderID, o.OrderDate, t.TableNumber
//                    ORDER BY o.OrderDate DESC";

//                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
//                dgvHistory.DataSource = dt;

//                decimal totalRevenue = 0;
//                foreach (DataRow row in dt.Rows)
//                {
//                    totalRevenue += Convert.ToDecimal(row["Tổng tiền"]);
//                }
//                lblTotalRevenue.Text = totalRevenue.ToString("N0") + " đ";

//                if (dgvHistory.Columns.Contains("Tổng tiền"))
//                {
//                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.Format = "N0";
//                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.ForeColor = Color.Red;
//                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Có lỗi khi tải dữ liệu: " + ex.Message);
//            }
//        }

//        #endregion


//        // ========================================================
//        #region 3. KHU VỰC SỰ KIỆN (EVENTS)
//        // ========================================================

//        private void HistoryForm_Load(object sender, EventArgs e)
//        {
//            LoadHistoryData();
//        }

//        private void BtnFilter_Click(object sender, EventArgs e)
//        {
//            LoadHistoryData();
//        }

//        private void DgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0)
//            {
//                // Lấy thông tin từ cái dòng đang được click
//                selectedOrderID = Convert.ToInt32(dgvHistory.Rows[e.RowIndex].Cells["Mã HĐ"].Value);
//                selectedTotal = Convert.ToDecimal(dgvHistory.Rows[e.RowIndex].Cells["Tổng tiền"].Value);
//                selectedDate = Convert.ToDateTime(dgvHistory.Rows[e.RowIndex].Cells["Ngày giờ"].Value).ToString("dd/MM/yyyy HH:mm");

//                // Mở bản in xem trước
//                printPreview.ShowDialog();
//            }
//        }

//        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
//        {
//            Graphics g = e.Graphics;

//            Font fontTitle = new Font("Courier New", 18, FontStyle.Bold);
//            Font fontSub = new Font("Courier New", 11, FontStyle.Regular);
//            Font fontHeader = new Font("Courier New", 13, FontStyle.Bold);
//            Font fontItem = new Font("Courier New", 11, FontStyle.Regular);
//            Font fontBold = new Font("Courier New", 11, FontStyle.Bold);

//            StringFormat centerAlign = new StringFormat() { Alignment = StringAlignment.Center };
//            StringFormat rightAlign = new StringFormat() { Alignment = StringAlignment.Far };

//            int yPos = 10;
//            int leftMargin = 5;
//            int centerPoint = 157;
//            int rightMargin = 300;

//            g.DrawString("PBL3 RESTAURANT", fontTitle, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
//            yPos += 30;
//            g.DrawString("Đ/c: ĐH Bách Khoa Đà Nẵng", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
//            yPos += 20;
//            g.DrawString("Hotline: 0123.456.789", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
//            yPos += 35;

//            // Đổi tiêu đề thành Bản Sao
//            g.DrawString("BẢN SAO HÓA ĐƠN", fontHeader, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
//            yPos += 35;

//            g.DrawString("Mã HD: " + selectedOrderID, fontItem, Brushes.Black, leftMargin, yPos);
//            yPos += 20;
//            g.DrawString("Ngày : " + selectedDate, fontItem, Brushes.Black, leftMargin, yPos);
//            yPos += 25;

//            string line = new string('-', 33);
//            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
//            yPos += 20;

//            g.DrawString("Tên món", fontBold, Brushes.Black, leftMargin, yPos);
//            g.DrawString("SL", fontBold, Brushes.Black, 170, yPos);
//            g.DrawString("T.Tiền", fontBold, Brushes.Black, rightMargin, yPos, rightAlign);
//            yPos += 25;
//            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
//            yPos += 20;

//            // Truy vấn lấy chi tiết các món ăn của Hóa đơn này
//            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(selectedOrderID);
//            foreach (DataRow row in dtDetails.Rows)
//            {
//                string name = row["ItemName"].ToString();
//                if (name.Length > 15) name = name.Substring(0, 15) + "..";

//                string qty = row["Quantity"].ToString();
//                string sub = Convert.ToDecimal(row["SubTotal"]).ToString("N0");

//                g.DrawString(name, fontItem, Brushes.Black, leftMargin, yPos);
//                g.DrawString(qty, fontItem, Brushes.Black, 170, yPos);
//                g.DrawString(sub, fontItem, Brushes.Black, rightMargin, yPos, rightAlign);
//                yPos += 25;
//            }

//            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
//            yPos += 25;

//            g.DrawString("TỔNG CỘNG:", fontHeader, Brushes.Black, leftMargin, yPos);
//            g.DrawString(selectedTotal.ToString("N0") + " đ", fontHeader, Brushes.Black, rightMargin, yPos, rightAlign);
//            yPos += 45;

//            g.DrawString("*** BẢN SAO (REPRINT) ***", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
//        }

//        #endregion
//    }
//}
using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class HistoryForm : Form
    {
        // ════════════════════════════════════════════════════════
        // DESIGN TOKENS
        // ════════════════════════════════════════════════════════
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_S = Color.FromArgb(237, 233, 254);
        private static readonly Color C_GREEN = Color.FromArgb(22, 163, 74);
        private static readonly Color C_GREEN_S = Color.FromArgb(220, 252, 231);
        private static readonly Color C_BLUE = Color.FromArgb(37, 99, 235);
        private static readonly Color C_BLUE_S = Color.FromArgb(219, 234, 254);
        private static readonly Color C_AMBER = Color.FromArgb(217, 119, 6);
        private static readonly Color C_AMBER_S = Color.FromArgb(254, 243, 199);
        private static readonly Color C_TEXT = Color.FromArgb(17, 24, 39);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);
        private static readonly Color C_RED = Color.FromArgb(220, 38, 38);

        // ════════════════════════════════════════════════════════
        // CONTROLS
        // ════════════════════════════════════════════════════════
        private Guna2DateTimePicker dtpFrom, dtpTo;
        private Guna2Button btnFilter, btnExport;
        private Label lblTotalRevenue, lblTotalOrders,
                                    lblAvgOrder, lblLastUpdated;
        private DataGridView dgvHistory;
        private Label lblRowCount;

        // Print
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;
        private int selectedOrderID = -1;
        private decimal selectedTotal = 0;
        private string selectedDate = "";

        public HistoryForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildUI();

            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize =
                new PaperSize("Thermal80mm", 315, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;

            printPreview = new PrintPreviewDialog
            {
                Document = printDoc,
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(450, 650)
            };
            printPreview.PrintPreviewControl.Zoom = 1.0;

            this.Load += (s, e) => LoadData();
        }

        // ════════════════════════════════════════════════════════
        // UI BUILDER
        // ════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.SuspendLayout();

            // ── 1. HEADER BAR ──────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE
            };
            pnlHeader.Paint += (s, e) =>
            {
                using (var p = new System.Drawing.Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1,
                                           pnlHeader.Width, pnlHeader.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "Lịch sử doanh thu",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(24, 18)
            };

            lblLastUpdated = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(24, 44)
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblLastUpdated);

            // ── 2. FILTER BAR ──────────────────────────────────
            var pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = C_WHITE,
                Padding = new Padding(16, 10, 16, 10)
            };
            pnlFilter.Paint += (s, e) =>
            {
                using (var p = new System.Drawing.Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, pnlFilter.Height - 1,
                                           pnlFilter.Width, pnlFilter.Height - 1);
            };

            // Quick filter buttons
            void QBtn(Guna2Button b, string t, int x, bool active = false)
            {
                b.Text = t;
                b.Size = new Size(80, 34);
                b.Location = new Point(x, 13);
                b.BorderRadius = 17;
                b.BorderThickness = 0;
                b.FillColor = active ? C_PURPLE : C_PURPLE_S;
                b.ForeColor = active ? C_WHITE : C_PURPLE;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
            }

            var btnToday = new Guna2Button(); QBtn(btnToday, "Hôm nay", 16, true);
            var btn7Days = new Guna2Button(); QBtn(btn7Days, "7 ngày", 106);
            var btn30Days = new Guna2Button(); QBtn(btn30Days, "30 ngày", 196);
            var btnThisMonth = new Guna2Button(); QBtn(btnThisMonth, "Tháng này", 286);

            void SetQuickActive(Guna2Button active)
            {
                foreach (var b in new[] { btnToday, btn7Days, btn30Days, btnThisMonth })
                {
                    b.FillColor = (b == active) ? C_PURPLE : C_PURPLE_S;
                    b.ForeColor = (b == active) ? C_WHITE : C_PURPLE;
                }
            }

            btnToday.Click += (s, e) =>
            {
                dtpFrom.Value = DateTime.Today;
                dtpTo.Value = DateTime.Today;
                SetQuickActive(btnToday); LoadData();
            };
            btn7Days.Click += (s, e) =>
            {
                dtpFrom.Value = DateTime.Today.AddDays(-6);
                dtpTo.Value = DateTime.Today;
                SetQuickActive(btn7Days); LoadData();
            };
            btn30Days.Click += (s, e) =>
            {
                dtpFrom.Value = DateTime.Today.AddDays(-29);
                dtpTo.Value = DateTime.Today;
                SetQuickActive(btn30Days); LoadData();
            };
            btnThisMonth.Click += (s, e) =>
            {
                dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                dtpTo.Value = DateTime.Today;
                SetQuickActive(btnThisMonth); LoadData();
            };

            // Date range pickers
            var lblSep = new Label
            {
                Text = "Từ:",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(390, 21)
            };

            dtpFrom = new Guna2DateTimePicker
            {
                Size = new Size(118, 34),
                Location = new Point(412, 13),
                BorderRadius = 6,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            var lblSep2 = new Label
            {
                Text = "đến:",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(536, 21)
            };

            dtpTo = new Guna2DateTimePicker
            {
                Size = new Size(118, 34),
                Location = new Point(562, 13),
                BorderRadius = 6,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            btnFilter = new Guna2Button
            {
                Text = "Lọc",
                Size = new Size(70, 34),
                Location = new Point(686, 13),
                BorderRadius = 6,
                BorderThickness = 0,
                FillColor = C_PURPLE,
                ForeColor = C_WHITE,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFilter.Click += (s, e) => { SetQuickActive(null); LoadData(); };

            btnExport = new Guna2Button
            {
                Text = "📥 Xuất CSV",
                Size = new Size(100, 34),
                Location = new Point(762, 13),
                BorderRadius = 6,
                BorderThickness = 0,
                FillColor = C_GREEN_S,
                ForeColor = C_GREEN,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExport.Click += BtnExport_Click;

            pnlFilter.Controls.AddRange(new Control[]
            {
                btnToday, btn7Days, btn30Days, btnThisMonth,
                lblSep, dtpFrom, lblSep2, dtpTo, btnFilter, btnExport
            });

            // ── 3. STAT CARDS ROW ──────────────────────────────
            var pnlCards = new Panel
            {
                Dock = DockStyle.Top,
                Height = 96,
                BackColor = C_BG,
                Padding = new Padding(16, 12, 16, 12)
            };

            Panel MakeCard(string title, Color accent, Color softBg,
                           out Label lblVal, string icon = "")
            {
                var card = new Panel
                {
                    Width = 210,
                    Height = 72,
                    BackColor = C_WHITE
                };
                // Rounded via Region
                card.Region = System.Drawing.Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, 210, 72, 10, 10));

                // Accent bar
                var bar = new Panel
                {
                    Size = new Size(4, 72),
                    Location = new Point(0, 0),
                    BackColor = accent
                };

                var lTitle = new Label
                {
                    Text = icon + " " + title,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = C_MUTED,
                    Location = new Point(14, 10),
                    AutoSize = true
                };

                lblVal = new Label
                {
                    Text = "—",
                    Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                    ForeColor = accent,
                    Location = new Point(14, 30),
                    AutoSize = true
                };

                card.Controls.Add(bar);
                card.Controls.Add(lTitle);
                card.Controls.Add(lblVal);
                return card;
            }

            var card1 = MakeCard("TỔNG DOANH THU", C_GREEN, C_GREEN_S,
                                  out lblTotalRevenue, "💰");
            var card2 = MakeCard("SỐ ĐƠN HÀNG", C_PURPLE, C_PURPLE_S,
                                  out lblTotalOrders, "🧾");
            var card3 = MakeCard("GIÁ TRỊ TB/ĐƠN", C_BLUE, C_BLUE_S,
                                  out lblAvgOrder, "📊");

            card1.Location = new Point(16, 12);
            card2.Location = new Point(242, 12);
            card3.Location = new Point(468, 12);

            pnlCards.Controls.Add(card1);
            pnlCards.Controls.Add(card2);
            pnlCards.Controls.Add(card3);

            // ── 4. GRID HEADER BAR ─────────────────────────────
            var pnlGridHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = C_WHITE,
                Padding = new Padding(20, 8, 20, 0)
            };

            var lblGridTitle = new Label
            {
                Text = "Danh sách hóa đơn",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(20, 10)
            };

            lblRowCount = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            var lblHint = new Label
            {
                Text = "💡 Nhấp đúp vào hóa đơn để in lại",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = C_MUTED,
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            pnlGridHeader.Controls.Add(lblGridTitle);
            pnlGridHeader.Controls.Add(lblHint);
            pnlGridHeader.Controls.Add(lblRowCount);

            pnlGridHeader.Resize += (s, e) =>
            {
                lblHint.Location = new Point(pnlGridHeader.Width - lblHint.Width - 20, 12);
                lblRowCount.Location = new Point(pnlGridHeader.Width - lblRowCount.Width - 20, 12);
            };

            // ── 5. DATAGRIDVIEW ────────────────────────────────
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
                ColumnHeadersHeight = 42,
                ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            // Column header style
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Row style
            dgvHistory.DefaultCellStyle.BackColor = C_WHITE;
            dgvHistory.DefaultCellStyle.ForeColor = C_TEXT;
            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvHistory.DefaultCellStyle.SelectionBackColor = C_PURPLE_S;
            dgvHistory.DefaultCellStyle.SelectionForeColor = C_TEXT;
            dgvHistory.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(249, 250, 251);
            dgvHistory.RowTemplate.Height = 44;

            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;
            dgvHistory.CellFormatting += DgvHistory_CellFormatting;

            // ── ASSEMBLE (DockStyle.Fill TRƯỚC, DockStyle.Top SAU) ──
            this.Controls.Add(dgvHistory);       // Fill — add trước
            this.Controls.Add(pnlGridHeader);    // Top  — add sau = nằm trên grid
            this.Controls.Add(pnlCards);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlHeader);

            this.ResumeLayout(false);
        }

        // ════════════════════════════════════════════════════════
        // LOAD DATA
        // ════════════════════════════════════════════════════════
        private void LoadData()
        {
            try
            {
                string fromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
                string toDate = dtpTo.Value.ToString("yyyy-MM-dd");

                string query = $@"
                    SELECT
                        o.OrderID                                            AS [Mã HĐ],
                        o.OrderDate                                          AS [Ngày giờ],
                        ISNULL(CAST(t.TableNumber AS NVARCHAR), N'Mang đi') AS [Bàn],
                        o.OrderType                                          AS [Loại],
                        SUM(od.Quantity * od.UnitPrice)                      AS [Tổng tiền]
                    FROM Orders o
                    LEFT JOIN DiningTable t  ON o.TableID  = t.TableID
                    INNER JOIN OrderDetail od ON o.OrderID  = od.OrderID
                    WHERE o.Status = 'Completed'
                      AND CAST(o.OrderDate AS DATE) >= '{fromDate}'
                      AND CAST(o.OrderDate AS DATE) <= '{toDate}'
                    GROUP BY o.OrderID, o.OrderDate, t.TableNumber, o.OrderType
                    ORDER BY o.OrderDate DESC";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvHistory.DataSource = dt;

                // Format cột tiền
                if (dgvHistory.Columns.Contains("Tổng tiền"))
                {
                    var col = dgvHistory.Columns["Tổng tiền"];
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Padding = new Padding(0, 0, 16, 0);
                }

                // Tính stat cards
                decimal totalRev = 0;
                int count = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                    totalRev += Convert.ToDecimal(row["Tổng tiền"]);

                lblTotalRevenue.Text = totalRev.ToString("N0") + " đ";
                lblTotalOrders.Text = count.ToString("N0") + " đơn";
                lblAvgOrder.Text = count > 0
                    ? (totalRev / count).ToString("N0") + " đ" : "—";

                // Row count + timestamp
                lblRowCount.Text = $"{count} hóa đơn";
                lblLastUpdated.Text = $"Cập nhật lúc {DateTime.Now:HH:mm:ss}  ·  " +
                                     $"{dtpFrom.Value:dd/MM/yyyy} → {dtpTo.Value:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        // CELL FORMATTING — màu trạng thái + loại đơn
        // ════════════════════════════════════════════════════════
        private void DgvHistory_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string col = dgvHistory.Columns[e.ColumnIndex].Name;

            // Cột Tổng tiền → đỏ đậm
            if (col == "Tổng tiền" && e.Value != null)
            {
                e.CellStyle.ForeColor = C_RED;
                e.CellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            }

            // Cột Loại → pill badge bằng màu text
            if (col == "Loại" && e.Value != null)
            {
                string v = e.Value.ToString();
                e.CellStyle.ForeColor = v == "DineIn" ? C_BLUE :
                                        v == "TakeAway" ? C_AMBER : C_GREEN;
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
        }

        // ════════════════════════════════════════════════════════
        // EXPORT CSV
        // ════════════════════════════════════════════════════════
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvHistory.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV files (*.csv)|*.csv";
                dlg.FileName = $"DoanhThu_{dtpFrom.Value:yyyyMMdd}_{dtpTo.Value:yyyyMMdd}.csv";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var sb = new System.Text.StringBuilder();

                // Header
                for (int i = 0; i < dgvHistory.Columns.Count; i++)
                {
                    sb.Append(dgvHistory.Columns[i].HeaderText);
                    if (i < dgvHistory.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();

                // Rows
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

                System.IO.File.WriteAllText(dlg.FileName, sb.ToString(),
                    System.Text.Encoding.UTF8);
                MessageBox.Show($"✅ Xuất thành công!\n{dlg.FileName}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ════════════════════════════════════════════════════════
        // DOUBLE-CLICK → PRINT PREVIEW
        // ════════════════════════════════════════════════════════
        private void DgvHistory_CellDoubleClick(object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                selectedOrderID = Convert.ToInt32(
                    dgvHistory.Rows[e.RowIndex].Cells["Mã HĐ"].Value);
                selectedTotal = Convert.ToDecimal(
                    dgvHistory.Rows[e.RowIndex].Cells["Tổng tiền"].Value);
                selectedDate = Convert.ToDateTime(
                    dgvHistory.Rows[e.RowIndex].Cells["Ngày giờ"].Value)
                    .ToString("dd/MM/yyyy HH:mm");

                printPreview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mở bản in: " + ex.Message);
            }
        }

        // ════════════════════════════════════════════════════════
        // PRINT PAGE
        // ════════════════════════════════════════════════════════
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
            g.DrawString("Đ/c: ĐH Bách Khoa Đà Nẵng", fSub, Brushes.Black, new PointF(cx, y), center); y += 20;
            g.DrawString("Hotline: 0123.456.789", fSub, Brushes.Black, new PointF(cx, y), center); y += 35;
            g.DrawString("BẢN SAO HÓA ĐƠN", fHeader, Brushes.Black, new PointF(cx, y), center); y += 35;
            g.DrawString("Mã HD: " + selectedOrderID, fItem, Brushes.Black, lx, y); y += 20;
            g.DrawString("Ngày : " + selectedDate, fItem, Brushes.Black, lx, y); y += 25;

            g.DrawString(line, fItem, Brushes.Black, lx, y); y += 20;
            g.DrawString("Tên món", fBold, Brushes.Black, lx, y);
            g.DrawString("SL", fBold, Brushes.Black, 170, y);
            g.DrawString("T.Tiền", fBold, Brushes.Black, rx, y, right); y += 25;
            g.DrawString(line, fItem, Brushes.Black, lx, y); y += 20;

            var dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(selectedOrderID);
            foreach (DataRow row in dtDetails.Rows)
            {
                string name = row["ItemName"].ToString();
                if (name.Length > 15) name = name.Substring(0, 15) + "..";
                string qty = row["Quantity"].ToString();
                string sub = Convert.ToDecimal(row["SubTotal"]).ToString("N0");

                g.DrawString(name, fItem, Brushes.Black, lx, y);
                g.DrawString(qty, fItem, Brushes.Black, 170, y);
                g.DrawString(sub, fItem, Brushes.Black, rx, y, right);
                y += 25;
            }

            g.DrawString(line, fItem, Brushes.Black, lx, y); y += 25;
            g.DrawString("TỔNG CỘNG:", fHeader, Brushes.Black, lx, y);
            g.DrawString(selectedTotal.ToString("N0") + " đ", fHeader,
                         Brushes.Black, rx, y, right); y += 45;
            g.DrawString("*** BẢN SAO (REPRINT) ***", fSub,
                         Brushes.Black, new PointF(cx, y), center);
        }

        // P/Invoke cho rounded card corners
        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern System.IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
    }
}