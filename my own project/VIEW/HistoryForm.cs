using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
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

        public HistoryForm()
        {
            InitializeComponent();
            this.Controls.Clear(); // Quét sạch tàn dư
            InitializeModernUI();

            // Đảm bảo khung vẽ xong xuôi hết rồi mới đổ Data vào
            this.Load += (s, e) => { LoadHistoryData(); };
        }

        private void InitializeModernUI()
        {
            this.BackColor = Color.FromArgb(245, 246, 250);
            this.FormBorderStyle = FormBorderStyle.None;

            // ==========================================
            // 1. THANH CÔNG CỤ (Vẫn giữ nguyên, nó đang xịn)
            // ==========================================
            Guna2Panel pnlTop = new Guna2Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 100;
            pnlTop.FillColor = Color.White;
            pnlTop.CustomBorderThickness = new Padding(0, 0, 0, 1);
            pnlTop.CustomBorderColor = Color.LightGray;
            this.Controls.Add(pnlTop);

            Label lblTitle = new Label { Text = "LỊCH SỬ DOANH THU", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), Location = new Point(20, 35), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblTitle);

            Label lblFrom = new Label { Text = "Từ ngày:", Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(270, 42), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblFrom);

            dtpFrom = new Guna2DateTimePicker { Location = new Point(340, 32), Size = new Size(150, 40), BorderRadius = 8, Format = DateTimePickerFormat.Short, FillColor = Color.FromArgb(240, 240, 240), Value = DateTime.Today };
            pnlTop.Controls.Add(dtpFrom);

            Label lblTo = new Label { Text = "Đến ngày:", Font = new Font("Segoe UI", 10F), ForeColor = Color.Gray, Location = new Point(510, 42), AutoSize = true, BackColor = Color.White };
            pnlTop.Controls.Add(lblTo);

            dtpTo = new Guna2DateTimePicker { Location = new Point(590, 32), Size = new Size(150, 40), BorderRadius = 8, Format = DateTimePickerFormat.Short, FillColor = Color.FromArgb(240, 240, 240), Value = DateTime.Today };
            pnlTop.Controls.Add(dtpTo);

            btnFilter = new Guna2Button { Text = "Lọc dữ liệu", Location = new Point(760, 32), Size = new Size(120, 40), BorderRadius = 8, FillColor = Color.FromArgb(88, 28, 230), Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnFilter.Click += (s, e) => { LoadHistoryData(); };
            pnlTop.Controls.Add(btnFilter);

            Label lblTotalText = new Label { Text = "Tổng doanh thu:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, AutoSize = true, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblTotalText.Location = new Point(pnlTop.Width - 200, 20);
            pnlTop.Controls.Add(lblTotalText);

            lblTotalRevenue = new Label { Text = "0 đ", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Size = new Size(180, 35), AutoSize = false, TextAlign = ContentAlignment.MiddleRight, BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblTotalRevenue.Location = new Point(pnlTop.Width - 200, 45);
            pnlTop.Controls.Add(lblTotalRevenue);

            // ==========================================
            // 2. BẢNG DỮ LIỆU (FIX TỌA ĐỘ TUYỆT ĐỐI)
            // ==========================================
            dgvHistory = new DataGridView();

            // BÍ QUYẾT: Gắn chết tọa độ, không xài Dock nữa!
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

            this.Controls.Add(dgvHistory);
            dgvHistory.BringToFront(); // Phép thuật: Ép nổi lên trên mọi mặt trận
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

                // Căn chỉnh giao diện cột
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
                MessageBox.Show("Có lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}