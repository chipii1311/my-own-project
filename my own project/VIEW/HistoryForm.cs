using Guna.UI2.WinForms;
using my_own_project.BLL; // Nhớ có dòng này để gọi chi tiết đơn hàng
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing; // Thư viện vẽ máy in
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class HistoryForm : Form
    {
        private Guna2DateTimePicker dtpFrom;
        private Guna2DateTimePicker dtpTo;
        private Guna2Button btnFilter;
        private Label lblTotalRevenue;
        private DataGridView dgvHistory;

        // --- BỘ ĐỒ NGHỀ XEM LẠI HÓA ĐƠN ---
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;
        private int selectedOrderID = -1;
        private decimal selectedTotal = 0;
        private string selectedDate = "";

        public HistoryForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            InitializeModernUI();

            // Cấu hình máy in ảo khổ 80mm
            printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Thermal80mm", 315, 600);
            printDoc.PrintPage += PrintDoc_PrintPage;

            printPreview = new PrintPreviewDialog();
            printPreview.Document = printDoc;
            printPreview.StartPosition = FormStartPosition.CenterScreen;
            printPreview.Size = new Size(450, 650);
            printPreview.PrintPreviewControl.Zoom = 1.0;

            this.Load += (s, e) => { LoadHistoryData(); };
        }

        private void InitializeModernUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;

            // ==========================================
            // 1. THANH CÔNG CỤ NẰM TRÊN CÙNG
            // ==========================================
            Guna2Panel pnlTop = new Guna2Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 100;
            pnlTop.FillColor = Color.White;
            pnlTop.CustomBorderThickness = new Padding(0, 0, 0, 1);
            pnlTop.CustomBorderColor = Color.LightGray;
            this.Controls.Add(pnlTop);

            // Ép thanh Top nằm đè lên trên để không bị Grid che
            pnlTop.BringToFront();

            Label lblTitle = new Label { Text = "LỊCH SỬ DOANH THU", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(20, 25), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblTitle);

            // Dòng hướng dẫn nhỏ
            Label lblHint = new Label { Text = "(Nhấp đúp chuột vào một dòng để xem chi tiết Bill)", Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(23, 60), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblHint);

            Label lblFrom = new Label { Text = "Từ ngày:", Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(350, 42), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblFrom);

            dtpFrom = new Guna2DateTimePicker { Location = new Point(420, 32), Size = new Size(130, 40), BorderRadius = 8, Format = DateTimePickerFormat.Short, FillColor = Color.FromArgb(240, 240, 240), Value = DateTime.Today };
            pnlTop.Controls.Add(dtpFrom);

            Label lblTo = new Label { Text = "Đến ngày:", Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(570, 42), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblTo);

            dtpTo = new Guna2DateTimePicker { Location = new Point(650, 32), Size = new Size(130, 40), BorderRadius = 8, Format = DateTimePickerFormat.Short, FillColor = Color.FromArgb(240, 240, 240), Value = DateTime.Today };
            pnlTop.Controls.Add(dtpTo);

            btnFilter = new Guna2Button { Text = "Lọc dữ liệu", Location = new Point(800, 32), Size = new Size(110, 40), BorderRadius = 8, FillColor = Color.FromArgb(88, 28, 230), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnFilter.Click += (s, e) => { LoadHistoryData(); };
            pnlTop.Controls.Add(btnFilter);

            Label lblTotalText = new Label { Text = "Tổng doanh thu:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, AutoSize = true, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblTotalText.Location = new Point(pnlTop.Width - 200, 20);
            pnlTop.Controls.Add(lblTotalText);

            lblTotalRevenue = new Label { Text = "0 đ", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Size = new Size(180, 35), AutoSize = false, TextAlign = ContentAlignment.MiddleRight, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblTotalRevenue.Location = new Point(pnlTop.Width - 200, 45);
            pnlTop.Controls.Add(lblTotalRevenue);

            // ==========================================
            // 2. TẠO BẢNG DỮ LIỆU
            // ==========================================
            dgvHistory = new DataGridView();
            dgvHistory.Location = new Point(20, 120);
            dgvHistory.Size = new Size(this.Width - 40, this.Height - 140);
            dgvHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.ReadOnly = true;
            dgvHistory.BackgroundColor = Color.White;
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistory.GridColor = Color.FromArgb(230, 230, 230);
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.ColumnHeadersHeight = 50;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(88, 28, 230);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            dgvHistory.DefaultCellStyle.BackColor = Color.White;
            dgvHistory.DefaultCellStyle.ForeColor = Color.Black;
            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 220, 255);
            dgvHistory.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            dgvHistory.Cursor = Cursors.Hand;

            // Gắn sự kiện nhấp đúp chuột
            dgvHistory.CellDoubleClick += DgvHistory_CellDoubleClick;

            this.Controls.Add(dgvHistory);
            dgvHistory.BringToFront();
        }

        private void LoadHistoryData()
        {
            try
            {
                string fromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
                string toDate = dtpTo.Value.ToString("yyyy-MM-dd");

                string query = $@"
                    SELECT 
                        o.OrderID AS [Mã HĐ],
                        o.OrderDate AS [Ngày giờ],
                        ISNULL(CAST(t.TableNumber AS VARCHAR), N'Mang đi') AS [Bàn],
                        SUM(od.Quantity * od.UnitPrice) AS [Tổng tiền]
                    FROM Orders o
                    LEFT JOIN DiningTable t ON o.TableID = t.TableID
                    INNER JOIN OrderDetail od ON o.OrderID = od.OrderID
                    WHERE o.Status = 'Completed' 
                      AND CAST(o.OrderDate AS DATE) >= '{fromDate}' 
                      AND CAST(o.OrderDate AS DATE) <= '{toDate}'
                    GROUP BY o.OrderID, o.OrderDate, t.TableNumber
                    ORDER BY o.OrderDate DESC";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvHistory.DataSource = dt;

                decimal totalRevenue = 0;
                foreach (DataRow row in dt.Rows)
                {
                    totalRevenue += Convert.ToDecimal(row["Tổng tiền"]);
                }
                lblTotalRevenue.Text = totalRevenue.ToString("N0") + " đ";

                if (dgvHistory.Columns.Contains("Tổng tiền"))
                {
                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.Format = "N0";
                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.ForeColor = Color.Red;
                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    dgvHistory.Columns["Tổng tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        // ===============================================
        // BẮT SỰ KIỆN CLICK ĐÚP CHUỘT VÀO HÓA ĐƠN
        // ===============================================
        private void DgvHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ cái dòng đang được click
                selectedOrderID = Convert.ToInt32(dgvHistory.Rows[e.RowIndex].Cells["Mã HĐ"].Value);
                selectedTotal = Convert.ToDecimal(dgvHistory.Rows[e.RowIndex].Cells["Tổng tiền"].Value);
                selectedDate = Convert.ToDateTime(dgvHistory.Rows[e.RowIndex].Cells["Ngày giờ"].Value).ToString("dd/MM/yyyy HH:mm");

                // Mở bản in xem trước
                printPreview.ShowDialog();
            }
        }

        // ===============================================
        // VẼ "BẢN SAO HÓA ĐƠN" CHI TIẾT
        // ===============================================
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font fontTitle = new Font("Courier New", 18, FontStyle.Bold);
            Font fontSub = new Font("Courier New", 11, FontStyle.Regular);
            Font fontHeader = new Font("Courier New", 13, FontStyle.Bold);
            Font fontItem = new Font("Courier New", 11, FontStyle.Regular);
            Font fontBold = new Font("Courier New", 11, FontStyle.Bold);

            StringFormat centerAlign = new StringFormat() { Alignment = StringAlignment.Center };
            StringFormat rightAlign = new StringFormat() { Alignment = StringAlignment.Far };

            int yPos = 10;
            int leftMargin = 5;
            int centerPoint = 157;
            int rightMargin = 300;

            g.DrawString("PBL3 RESTAURANT", fontTitle, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 30;
            g.DrawString("Đ/c: ĐH Bách Khoa Đà Nẵng", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 20;
            g.DrawString("Hotline: 0123.456.789", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 35;

            // Đổi tiêu đề thành Bản Sao
            g.DrawString("BẢN SAO HÓA ĐƠN", fontHeader, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
            yPos += 35;

            g.DrawString("Mã HD: " + selectedOrderID, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            g.DrawString("Ngày : " + selectedDate, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            string line = new string('-', 33);
            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            g.DrawString("Tên món", fontBold, Brushes.Black, leftMargin, yPos);
            g.DrawString("SL", fontBold, Brushes.Black, 170, yPos);
            g.DrawString("T.Tiền", fontBold, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 25;
            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 20;

            // Truy vấn lấy chi tiết các món ăn của Hóa đơn này
            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(selectedOrderID);
            foreach (DataRow row in dtDetails.Rows)
            {
                string name = row["ItemName"].ToString();
                if (name.Length > 15) name = name.Substring(0, 15) + "..";

                string qty = row["Quantity"].ToString();
                string sub = Convert.ToDecimal(row["SubTotal"]).ToString("N0");

                g.DrawString(name, fontItem, Brushes.Black, leftMargin, yPos);
                g.DrawString(qty, fontItem, Brushes.Black, 170, yPos);
                g.DrawString(sub, fontItem, Brushes.Black, rightMargin, yPos, rightAlign);
                yPos += 25;
            }

            g.DrawString(line, fontItem, Brushes.Black, leftMargin, yPos);
            yPos += 25;

            g.DrawString("TỔNG CỘNG:", fontHeader, Brushes.Black, leftMargin, yPos);
            g.DrawString(selectedTotal.ToString("N0") + " đ", fontHeader, Brushes.Black, rightMargin, yPos, rightAlign);
            yPos += 45;

            g.DrawString("*** BẢN SAO (REPRINT) ***", fontSub, Brushes.Black, new PointF(centerPoint, yPos), centerAlign);
        }
    }
}