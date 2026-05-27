using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class InventoryForm : Form
    {
        public InventoryForm()
        {
            InitializeComponent();

            // Gọi hàm dựng giao diện từ file Designer
            BuildUI();

            // Tải dữ liệu lên Grid
            LoadData();
        }

        // ==================== DATA LOADING ====================
        private void LoadData()
        {
            LoadIngredients();
            LoadTransactions();
        }

        private void LoadIngredients()
        {
            try
            {
                DataTable dt = IngredientBLL.GetAllIngredients();
                dgvInventory.DataSource = dt;
                FormatInventoryGrid();
                ApplyFilter();
                RefreshStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTransactions()
        {
            try
            {
                DataTable dt = InventoryTransactionBLL.GetRecentTransactions();
                dgvTransactions.DataSource = dt;
                FormatTransactionGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== FILTER & STATS ====================
        private void ApplyFilter()
        {
            if (!(dgvInventory.DataSource is DataTable dt)) return;

            string kw = txtSearch.Text.Trim().Replace("'", "''");
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(kw)
                ? ""
                : $"IngredientName LIKE '%{kw}%' OR Unit LIKE '%{kw}%'";

            RefreshStats();
        }

        private void RefreshStats()
        {
            if (!(dgvInventory.DataSource is DataTable dt)) return;

            DataView view = dt.DefaultView;
            int total = view.Count;
            int ok = 0, low = 0, outStock = 0;

            foreach (DataRowView rv in view)
            {
                string status = rv["StockStatus"]?.ToString() ?? "";
                if (status == "Hết hàng") outStock++;
                else if (status == "Sắp hết") low++;
                else ok++;
            }

            lblStatTotal.Text = total.ToString();
            lblStatOk.Text = ok.ToString();
            lblStatLow.Text = low.ToString();
            lblStatOut.Text = outStock.ToString();
        }

        // ==================== GRID FORMATTING ====================
        private void FormatInventoryGrid()
        {
            if (dgvInventory.Columns.Count == 0) return;

            HideCol(dgvInventory, "IngredientID");
            HideCol(dgvInventory, "IsActive");

            SetHeader(dgvInventory, "IngredientName", "Tên nguyên liệu");
            SetHeader(dgvInventory, "Unit", "Đơn vị");
            SetHeader(dgvInventory, "StockQuantity", "Tồn kho");
            SetHeader(dgvInventory, "MinStock", "Tồn tối thiểu");
            SetHeader(dgvInventory, "PurchasePrice", "Giá nhập");
            SetHeader(dgvInventory, "StockStatus", "Trạng thái");

            if (dgvInventory.Columns.Contains("PurchasePrice"))
            {
                dgvInventory.Columns["PurchasePrice"].DefaultCellStyle.Format = "N0";
                dgvInventory.Columns["PurchasePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInventory.Columns["PurchasePrice"].DefaultCellStyle.Padding = new Padding(0, 0, 12, 0);
            }

            if (dgvInventory.Columns.Contains("StockQuantity"))
                dgvInventory.Columns["StockQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            if (dgvInventory.Columns.Contains("MinStock"))
                dgvInventory.Columns["MinStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            SetColWidth(dgvInventory, "IngredientName", 2.0f);
            SetColWidth(dgvInventory, "Unit", 0.7f);
            SetColWidth(dgvInventory, "StockQuantity", 1.0f);
            SetColWidth(dgvInventory, "MinStock", 1.0f);
            SetColWidth(dgvInventory, "PurchasePrice", 1.1f);
            SetColWidth(dgvInventory, "StockStatus", 1.2f);
        }

        private void FormatTransactionGrid()
        {
            if (dgvTransactions.Columns.Count == 0) return;

            HideCol(dgvTransactions, "TransactionID");

            SetHeader(dgvTransactions, "IngredientName", "Nguyên liệu");
            SetHeader(dgvTransactions, "Unit", "Đơn vị");
            SetHeader(dgvTransactions, "QuantityChanged", "Số lượng");
            SetHeader(dgvTransactions, "TransactionType", "Loại");
            SetHeader(dgvTransactions, "TransactionDate", "Thời gian");
            SetHeader(dgvTransactions, "StaffName", "Nhân viên");
            SetHeader(dgvTransactions, "Note", "Ghi chú");

            if (dgvTransactions.Columns.Contains("QuantityChanged"))
                dgvTransactions.Columns["QuantityChanged"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            if (dgvTransactions.Columns.Contains("TransactionDate"))
                dgvTransactions.Columns["TransactionDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        // ==================== CELL FORMATTING ====================
        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string col = dgvInventory.Columns[e.ColumnIndex].Name;

            if (col == "StockStatus")
            {
                string status = e.Value?.ToString() ?? "";
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

                switch (status)
                {
                    case "Hết hàng":
                        e.CellStyle.ForeColor = C_RED_TEXT;
                        e.CellStyle.BackColor = C_RED_BG;
                        e.CellStyle.SelectionForeColor = C_RED_TEXT;
                        break;
                    case "Sắp hết":
                        e.CellStyle.ForeColor = C_AMBER_TEXT;
                        e.CellStyle.BackColor = C_AMBER_BG;
                        e.CellStyle.SelectionForeColor = C_AMBER_TEXT;
                        break;
                    default:
                        e.CellStyle.ForeColor = C_GREEN_TEXT;
                        e.CellStyle.BackColor = C_GREEN_BG;
                        e.CellStyle.SelectionForeColor = C_GREEN_TEXT;
                        break;
                }
            }

            if (dgvInventory.Columns.Contains("StockStatus") && col != "StockStatus")
            {
                DataGridViewRow row = dgvInventory.Rows[e.RowIndex];
                string status = row.Cells["StockStatus"].Value?.ToString() ?? "";
                if (status == "Hết hàng")
                    e.CellStyle.ForeColor = Color.FromArgb(160, 50, 50);
            }
        }

        private void DgvInventory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatInventoryGrid();
            RefreshStats();
        }

        private void DgvTransactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatTransactionGrid();
            if (!dgvTransactions.Columns.Contains("TransactionType")) return;

            foreach (DataGridViewRow row in dgvTransactions.Rows)
            {
                object val = row.Cells["TransactionType"].Value;
                if (val == null) continue;

                string type = val.ToString();
                row.Cells["TransactionType"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

                if (type == "IMPORT")
                {
                    row.Cells["TransactionType"].Value = "↓ Nhập kho";
                    row.Cells["TransactionType"].Style.ForeColor = C_GREEN_TEXT;
                    row.Cells["TransactionType"].Style.BackColor = C_GREEN_BG;
                    row.Cells["TransactionType"].Style.SelectionForeColor = C_GREEN_TEXT;
                }
                else if (type == "EXPORT")
                {
                    row.Cells["TransactionType"].Value = "↑ Xuất kho";
                    row.Cells["TransactionType"].Style.ForeColor = C_AMBER_TEXT;
                    row.Cells["TransactionType"].Style.BackColor = C_AMBER_BG;
                    row.Cells["TransactionType"].Style.SelectionForeColor = C_AMBER_TEXT;
                }
            }
        }

        // ==================== BUTTON EVENTS ====================
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new IngredientEditForm())
                if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedID();
            if (!id.HasValue) { ShowInfo("Vui lòng chọn nguyên liệu cần sửa."); return; }
            using (var form = new IngredientEditForm(id.Value))
                if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedID();
            if (!id.HasValue) { ShowInfo("Vui lòng chọn nguyên liệu cần xóa."); return; }

            if (MessageBox.Show("Bạn có chắc muốn xóa nguyên liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                IngredientBLL.DeleteIngredient(id.Value);
                ShowInfo("Xóa nguyên liệu thành công.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (var form = new ImportStockForm(GetSelectedID()))
                form.ShowDialog();
            LoadData();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (var form = new ExportStockForm(GetSelectedID()))
                form.ShowDialog();
            LoadData();
        }

        private void DgvInventory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) BtnEdit_Click(sender, EventArgs.Empty);
        }

        // ==================== HELPERS ====================
        private int? GetSelectedID()
        {
            if (dgvInventory.CurrentRow == null) return null;
            if (!dgvInventory.Columns.Contains("IngredientID")) return null;
            object v = dgvInventory.CurrentRow.Cells["IngredientID"].Value;
            if (v == null || v == DBNull.Value) return null;
            return Convert.ToInt32(v);
        }

        private void HideCol(DataGridView grid, string col) { if (grid.Columns.Contains(col)) grid.Columns[col].Visible = false; }
        private void SetHeader(DataGridView grid, string col, string header) { if (grid.Columns.Contains(col)) grid.Columns[col].HeaderText = header; }
        private void SetColWidth(DataGridView grid, string col, float fill) { if (!grid.Columns.Contains(col)) return; grid.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; grid.Columns[col].FillWeight = fill * 100f; }
        private void ShowInfo(string msg) { MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); }
    }
}