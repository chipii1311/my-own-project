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
        // Print
        private PrintDocument printDoc;
        private PrintPreviewDialog printPreview;
        private int selectedOrderID = -1;
        private decimal selectedTotal = 0;
        private string selectedDate = "";
        private string selectedPaymentMethod = ""; // 🌟 Lưu hình thức TT để in

        public HistoryForm()
        {
            InitializeComponent();
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

            this.Load += (s, e) => LoadData();
        }

        public void BtnToday_Click(object sender, EventArgs e) { dtpFrom.Value = DateTime.Today; dtpTo.Value = DateTime.Today; SetQuickActive(btnToday); LoadData(); }
        public void Btn7Days_Click(object sender, EventArgs e) { dtpFrom.Value = DateTime.Today.AddDays(-6); dtpTo.Value = DateTime.Today; SetQuickActive(btn7Days); LoadData(); }
        public void Btn30Days_Click(object sender, EventArgs e) { dtpFrom.Value = DateTime.Today.AddDays(-29); dtpTo.Value = DateTime.Today; SetQuickActive(btn30Days); LoadData(); }
        public void BtnThisMonth_Click(object sender, EventArgs e) { dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); dtpTo.Value = DateTime.Today; SetQuickActive(btnThisMonth); LoadData(); }
        public void BtnFilter_Click(object sender, EventArgs e) { SetQuickActive(null); LoadData(); }


        private void SetQuickActive(Guna2Button active)
        {
            foreach (var b in new[] { btnToday, btn7Days, btn30Days, btnThisMonth })
            {
                if (b == null) continue;
                b.FillColor = (b == active) ? C_PURPLE : C_PURPLE_SOFT;
                b.ForeColor = (b == active) ? Color.White : C_PURPLE;
            }
        }

        private void LoadData()
        {
            try
            {
                // [ĐÃ SỬA]: Gọi qua BLL thay vì DataHelper.ExecuteQuery(rawSQL)
                DataTable dt = OrderBLL.GetCompletedOrdersByDateRange(dtpFrom.Value, dtpTo.Value);
                dgvHistory.DataSource = dt;

                if (dgvHistory.Columns.Contains("Tổng tiền"))
                {
                    var col = dgvHistory.Columns["Tổng tiền"];
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Padding = new Padding(0, 0, 18, 0);
                }

                decimal totalRev = 0;
                int count = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                    totalRev += row["Tổng tiền"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Tổng tiền"]);

                lblTotalRevenue.Text = totalRev.ToString("N0") + " đ";
                lblTotalOrders.Text = count + " đơn";
                lblAvgOrder.Text = count > 0 ? (totalRev / count).ToString("N0") + " đ" : "—";

                if (lblRowCount != null) lblRowCount.Text = count + " hóa đơn";
                if (lblLastUpdated != null) lblLastUpdated.Text =
                    $"Cập nhật lúc {DateTime.Now:HH:mm:ss}  ·  {dtpFrom.Value:dd/MM/yyyy} — {dtpTo.Value:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ════════════════════════════════════════════════════════
        // CELL FORMATTING
        // ════════════════════════════════════════════════════════
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
        public void BtnExport_Click(object sender, EventArgs e)
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

                for (int i = 0; i < dgvHistory.Columns.Count; i++)
                {
                    sb.Append(dgvHistory.Columns[i].HeaderText);
                    if (i < dgvHistory.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();

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
            g.DrawString(selectedTotal.ToString("N0") + " đ", fHeader, Brushes.Black, rx, y, right);
            y += 30;

            g.DrawString("Hình thức TT:", fItem, Brushes.Black, lx, y);
            g.DrawString(selectedPaymentMethod, fItem, Brushes.Black, rx, y, right);
            y += 45;

            g.DrawString("*** BẢN SAO (REPRINT) ***", fSub, Brushes.Black, new PointF(cx, y), center);
        }
    }
}