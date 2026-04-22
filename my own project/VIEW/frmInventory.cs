using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class frmInventory : Form
    {
        private readonly InventoryBLL _bll = new InventoryBLL();
        private int _selectedIngredientID = -1;
        public frmInventory()
        {
            InitializeComponent();
            WireEvents();
        }
        private void frmInventory_Load(object sender, EventArgs e)
        {
            LoadIngredients();
            LoadTransactions();
            LoadLowStock();
            LoadStatCards();
        }

        private void WireEvents()
        {
            this.Load += frmInventory_Load;
            btnSearch.Click += (s, e) => SearchIngredient();
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchIngredient(); };
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            btnImport.Click += btnImport_Click;
            btnExport.Click += btnExport_Click;
            dgvIngredient.SelectionChanged += DgvIngredient_SelectionChanged;
            dgvIngredient.CellFormatting += DgvIngredient_CellFormatting;
            btnTxnFilter.Click += (s, e) => LoadTransactions();
            btnTxnRefresh.Click += (s, e) => { cmbTxnType.SelectedIndex = 0; dtpTxnFrom.Value = DateTime.Now.AddDays(-30); dtpTxnTo.Value = DateTime.Now; LoadTransactions(); };
            btnRefreshLow.Click += (s, e) => LoadLowStock();
            btnQuickImport.Click += btnImport_Click;
            dgvLowStock.CellFormatting += DgvLowStock_CellFormatting;
        }

        // ════════════════════════════════════════════════════════════════
        // LOAD DATA
        // ════════════════════════════════════════════════════════════════
        private void LoadIngredients(string keyword = "")
        {
            try
            {
                DataTable dt = _bll.GetAllIngredients(keyword);
                dgvIngredient.DataSource = dt;
                lblStatus.Text = $"✅  Hiển thị {dt.Rows.Count} nguyên liệu";
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void LoadTransactions()
        {
            try
            {
                string type = cmbTxnType.SelectedItem?.ToString() == "Tất cả" ? null : cmbTxnType.SelectedItem?.ToString();
                DataTable dt = _bll.GetTransactions(dtpTxnFrom.Value, dtpTxnTo.Value, type);
                dgvTransaction.DataSource = dt;
                lblStatus.Text = $"✅  {dt.Rows.Count} giao dịch";
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void LoadLowStock()
        {
            try
            {
                DataTable dt = _bll.GetLowStockIngredients();
                dgvLowStock.DataSource = dt;
                lblStatLowVal.Text = dt.Rows.Count.ToString();
                if (dt.Rows.Count > 0)
                    lblStatus.Text = $"⚠️  Có {dt.Rows.Count} nguyên liệu cần nhập thêm!";
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void LoadStatCards()
        {
            try
            {
                var stats = _bll.GetInventoryStats();
                lblStatTotalVal.Text = stats.TotalIngredients.ToString();
                lblStatLowVal.Text = stats.LowStockCount.ToString();
                lblStatOutVal.Text = stats.OutOfStockCount.ToString();
                lblStatTxnVal.Text = stats.TodayTransactions.ToString();
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════════
        // SEARCH
        // ════════════════════════════════════════════════════════════════
        private void SearchIngredient() => LoadIngredients(txtSearch.Text.Trim());

        // ════════════════════════════════════════════════════════════════
        // CRUD BUTTONS
        // ════════════════════════════════════════════════════════════════
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new frmIngredientDetail())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadIngredients();
                    LoadStatCards();
                    lblStatus.Text = "✅  Thêm nguyên liệu thành công!";
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedIngredientID < 0) { ShowWarning("Vui lòng chọn nguyên liệu cần sửa!"); return; }
            using (var dlg = new frmIngredientDetail(_selectedIngredientID))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadIngredients(txtSearch.Text.Trim());
                    LoadStatCards();
                    lblStatus.Text = "✅  Cập nhật thành công!";
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedIngredientID < 0) { ShowWarning("Vui lòng chọn nguyên liệu cần xóa!"); return; }

            string name = dgvIngredient.CurrentRow?.Cells["colName"].Value?.ToString();
            if (MessageBox.Show($"Bạn có chắc muốn xóa nguyên liệu\n\"{name}\"?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _bll.DeleteIngredient(_selectedIngredientID);
                    LoadIngredients(txtSearch.Text.Trim());
                    LoadStatCards();
                    _selectedIngredientID = -1;
                    lblStatus.Text = "✅  Đã xóa nguyên liệu!";
                }
                catch (Exception ex) { ShowError(ex.Message); }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            int preselect = tabMain.SelectedIndex == 2 && dgvLowStock.CurrentRow != null
                ? Convert.ToInt32(dgvLowStock.CurrentRow.Cells["lColName"].Value ?? -1)
                : _selectedIngredientID;

            using (var dlg = new frmImportExport("Import", preselect))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadIngredients(txtSearch.Text.Trim());
                    LoadTransactions();
                    LoadLowStock();
                    LoadStatCards();
                    lblStatus.Text = "✅  Nhập kho thành công!";
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_selectedIngredientID < 0) { ShowWarning("Vui lòng chọn nguyên liệu cần xuất!"); return; }
            using (var dlg = new frmImportExport("Export", _selectedIngredientID))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadIngredients(txtSearch.Text.Trim());
                    LoadTransactions();
                    LoadStatCards();
                    lblStatus.Text = "✅  Xuất kho thành công!";
                }
            }
        }

        // ════════════════════════════════════════════════════════════════
        // GRID EVENTS
        // ════════════════════════════════════════════════════════════════
        private void DgvIngredient_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIngredient.CurrentRow == null) return;
            _selectedIngredientID = Convert.ToInt32(dgvIngredient.CurrentRow.Cells["colID"].Value);
        }

        // Tô màu dòng theo trạng thái tồn kho
        private void DgvIngredient_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvIngredient.Rows[e.RowIndex];
            string status = row.Cells["colStatus"].Value?.ToString();

            if (status == "Hết hàng")
            {
                row.DefaultCellStyle.ForeColor = Color.FromArgb(229, 57, 53);
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
            }
            else if (status == "Sắp hết")
            {
                row.DefaultCellStyle.ForeColor = Color.FromArgb(230, 81, 0);
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224);
            }
            else
            {
                row.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 70);
                row.DefaultCellStyle.BackColor = e.RowIndex % 2 == 0
                    ? Color.White : Color.FromArgb(249, 249, 253);
            }
        }

        // Tô màu mức độ khẩn cấp trong tab LowStock
        private void DgvLowStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvLowStock.Rows[e.RowIndex];
            string level = row.Cells["lColUrgent"].Value?.ToString();
            if (level == "Khẩn cấp")
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(229, 57, 53);
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(230, 81, 0);
            }
        }

        // ════════════════════════════════════════════════════════════════
        // TIMER CLOCK
        // ════════════════════════════════════════════════════════════════
        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
        }

        // ════════════════════════════════════════════════════════════════
        // HELPERS
        // ════════════════════════════════════════════════════════════════
        private void ShowError(string msg) =>
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowWarning(string msg) =>
            MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
