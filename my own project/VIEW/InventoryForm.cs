// InventoryForm.cs
using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class InventoryForm : Form
    {
        // ── Design tokens ──
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color C_BORDER = Color.FromArgb(232, 232, 240);

        private Guna2TextBox txtSearch;
        private Guna2Button btnImport;
        private Guna2DataGridView dgvInventory;

        public InventoryForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            this.SuspendLayout();

            // ── HEADER ──────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE,
                Padding = new Padding(24, 0, 24, 0)
            };
            pnlHeader.Paint += (s, e) =>
            {
                using (var pen = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "QUẢN LÝ KHO HÀNG",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(24, 18)
            };

            btnImport = new Guna2Button
            {
                Text = "+ Nhập kho",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                BorderRadius = 8,
                Size = new Size(130, 38),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(pnlHeader.Width - 154, 13),
                Cursor = Cursors.Hand
            };
            btnImport.Click += (s, e) => OpenImportForm(null);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnImport);
            pnlHeader.Resize += (s, e) => btnImport.Location = new Point(pnlHeader.Width - 154, 13);

            // ── SEARCH BAR ──────────────────────────
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = C_WHITE,
                Padding = new Padding(24, 10, 24, 10)
            };
            txtSearch = new Guna2TextBox
            {
                PlaceholderText = "🔍  Tìm kiếm nguyên liệu...",
                Font = new Font("Segoe UI", 11F),
                FillColor = C_BG,
                BorderColor = C_BORDER,
                BorderRadius = 8,
                Size = new Size(360, 36),
                Location = new Point(24, 10)
            };
            txtSearch.TextChanged += (s, e) => LoadData();
            pnlSearch.Controls.Add(txtSearch);

            // ── GRID ─────────────────────────────────
            dgvInventory = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = C_BORDER,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Cursor = Cursors.Hand
            };
            // Header style
            dgvInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvInventory.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgvInventory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvInventory.ColumnHeadersHeight = 40;
            dgvInventory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            // Row style
            dgvInventory.DefaultCellStyle.BackColor = C_WHITE;
            dgvInventory.DefaultCellStyle.ForeColor = C_TEXT;
            dgvInventory.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvInventory.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            dgvInventory.DefaultCellStyle.SelectionForeColor = C_TEXT;
            dgvInventory.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvInventory.RowTemplate.Height = 42;
            dgvInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
            dgvInventory.CellContentClick += DgvInventory_CellContentClick;

            // ── Lắp ráp ─────────────────────────────
            this.Controls.Add(dgvInventory);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlHeader);

            this.ResumeLayout(false);
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = IngredientBLL.GetAllIngredients();
                DataView dv = dt.DefaultView;
                string keyword = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                    dv.RowFilter = $"IngredientName LIKE '%{keyword.Replace("'", "''")}%'";
                dgvInventory.DataSource = dv;

                // Tùy chỉnh cột
                if (dgvInventory.Columns.Count > 0)
                {
                    // Ẩn các cột không cần
                    string[] hideCols = { "IngredientID", "IsActive", "MinStock" };
                    foreach (string col in hideCols)
                        if (dgvInventory.Columns.Contains(col))
                            dgvInventory.Columns[col].Visible = false;

                    // Đổi tên header
                    var headers = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "IngredientName", "Tên nguyên liệu" },
                        { "Unit", "Đơn vị" },
                        { "StockQuantity", "Số lượng tồn" },
                        { "PurchasePrice", "Giá nhập (VNĐ)" },
                        { "MinStock", "Mức tối thiểu" }
                    };
                    foreach (DataGridViewColumn col in dgvInventory.Columns)
                        if (headers.ContainsKey(col.Name)) col.HeaderText = headers[col.Name];

                    // Thêm cột trạng thái nếu chưa có
                    if (!dgvInventory.Columns.Contains("StatusCol"))
                    {
                        var statusCol = new DataGridViewTextBoxColumn
                        {
                            Name = "StatusCol",
                            HeaderText = "Trạng thái",
                            ReadOnly = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            Width = 120
                        };
                        dgvInventory.Columns.Insert(dgvInventory.Columns.Count, statusCol);
                    }

                    // Thêm cột nút thao tác (Nhập hàng)
                    if (!dgvInventory.Columns.Contains("btnAction"))
                    {
                        var btnCol = new DataGridViewButtonColumn
                        {
                            Name = "btnAction",
                            HeaderText = "",
                            Text = "⚙️ Nhập hàng",
                            UseColumnTextForButtonValue = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            Width = 110,
                            FlatStyle = FlatStyle.Flat
                        };
                        dgvInventory.Columns.Add(btnCol);
                    }

                    // Định dạng cột tiền
                    if (dgvInventory.Columns.Contains("PurchasePrice"))
                        dgvInventory.Columns["PurchasePrice"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nguyên liệu: " + ex.Message);
            }
        }

        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Cột trạng thái
            if (dgvInventory.Columns[e.ColumnIndex].Name == "StatusCol")
            {
                // Lấy giá trị StockQuantity và MinStock từ dòng hiện tại
                var row = dgvInventory.Rows[e.RowIndex];
                float stock = 0, min = 0;
                if (dgvInventory.Columns.Contains("StockQuantity") &&
                    row.Cells["StockQuantity"].Value != DBNull.Value)
                    stock = Convert.ToSingle(row.Cells["StockQuantity"].Value);
                if (dgvInventory.Columns.Contains("MinStock") &&
                    row.Cells["MinStock"].Value != DBNull.Value)
                    min = Convert.ToSingle(row.Cells["MinStock"].Value);

                string status;
                Color foreColor;
                if (stock <= 0)
                {
                    status = "🔴 Hết hàng";
                    foreColor = C_RED;
                }
                else if (stock < min)
                {
                    status = "🟠 Sắp hết";
                    foreColor = C_AMBER;
                }
                else
                {
                    status = "🟢 Còn hàng";
                    foreColor = C_GREEN;
                }
                e.Value = status;
                e.CellStyle.ForeColor = foreColor;
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                e.FormattingApplied = true;
            }
        }

        private void DgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvInventory.Columns[e.ColumnIndex].Name == "btnAction")
            {
                int ingredientID = Convert.ToInt32(dgvInventory.Rows[e.RowIndex].Cells["IngredientID"].Value);
                string name = dgvInventory.Rows[e.RowIndex].Cells["IngredientName"].Value.ToString();
                OpenImportForm(ingredientID);
            }
        }

        private void OpenImportForm(int? ingredientID)
        {
            // Mở form ImportStockForm (bạn tự tạo, xem bên dưới)
            var importForm = new ImportStockForm(ingredientID);
            importForm.FormClosed += (s, e) => LoadData(); // refresh sau khi nhập
            importForm.ShowDialog();
        }
    }
}