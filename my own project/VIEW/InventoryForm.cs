using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class InventoryForm : Form
    {
        // ==================== COLORS ====================
        private static readonly Color C_BG = Color.FromArgb(245, 246, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(83, 74, 183);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_GREEN_BG = Color.FromArgb(234, 243, 222);
        private static readonly Color C_GREEN_TEXT = Color.FromArgb(39, 80, 10);
        private static readonly Color C_AMBER = Color.FromArgb(186, 117, 23);
        private static readonly Color C_AMBER_BG = Color.FromArgb(250, 238, 218);
        private static readonly Color C_AMBER_TEXT = Color.FromArgb(99, 56, 6);
        private static readonly Color C_RED = Color.FromArgb(226, 75, 74);
        private static readonly Color C_RED_BG = Color.FromArgb(252, 235, 235);
        private static readonly Color C_RED_TEXT = Color.FromArgb(121, 31, 31);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(120, 120, 140);
        private static readonly Color C_BORDER = Color.FromArgb(228, 228, 238);
        private static readonly Color C_STAT_BG = Color.FromArgb(237, 238, 245);

        // ==================== CONTROLS ====================
        private Guna2TextBox txtSearch;
        private Guna2Button btnAdd, btnEdit, btnDelete, btnImport, btnExport, btnRefresh;
        private Guna2DataGridView dgvInventory, dgvTransactions;

        // Stat labels
        private Label lblStatTotal, lblStatOk, lblStatLow, lblStatOut;

        public InventoryForm()
        {
            InitializeComponent();
            Controls.Clear();
            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BuildUI();
            LoadData();
        }

        // ==================== BUILD UI ====================
        private void BuildUI()
        {
            SuspendLayout();

            // Header
            Panel header = BuildHeader();

            // Search + stat row
            Panel subbar = BuildSubBar();

            // Split: top = ingredient list, bottom = history
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 390,
                BackColor = C_BG,
                BorderStyle = BorderStyle.None,
                SplitterWidth = 6
            };

            dgvInventory = CreateGrid();
            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
            dgvInventory.DataBindingComplete += DgvInventory_DataBindingComplete;
            dgvInventory.CellDoubleClick += DgvInventory_CellDoubleClick;
            dgvInventory.SelectionChanged += (s, e) => RefreshStats();

            dgvTransactions = CreateGrid();
            dgvTransactions.DataBindingComplete += DgvTransactions_DataBindingComplete;

            split.Panel1.Controls.Add(BuildCard("  Danh sách nguyên liệu", dgvInventory));
            split.Panel2.Controls.Add(BuildCard("  Lịch sử nhập / xuất gần đây", dgvTransactions));

            Controls.Add(split);
            Controls.Add(subbar);
            Controls.Add(header);

            ResumeLayout(false);
        }

        // ── Header ──────────────────────────────────────────────────────────
        private Panel BuildHeader()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE,
                Padding = new Padding(24, 0, 24, 0)
            };

            // Bottom border
            header.Paint += (s, e) =>
            {
                using (Pen p = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            // Icon + Title
            

            Label title = new Label
            {
                Text = "QUẢN LÝ KHO NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(58, 22)
            };

            // Buttons
            btnAdd = MakeBtn("+ Thêm", C_GREEN, 90);
            btnEdit = MakeBtn("✎ Sửa", C_PURPLE, 84);
            btnDelete = MakeBtn("✕ Xóa", C_RED, 84);
            btnImport = MakeBtn("↓ Nhập kho", Color.FromArgb(24, 95, 165), 110);
            btnExport = MakeBtn("↑ Xuất kho", Color.FromArgb(186, 117, 23), 110);
            btnRefresh = MakeBtn("↻", Color.FromArgb(90, 90, 110), 42);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnRefresh.Click += (s, e) => LoadData();

            header.Controls.AddRange(new Control[] { title, btnAdd, btnEdit, btnDelete, btnImport, btnExport, btnRefresh });

            header.Resize += (s, e) => LayoutHeaderButtons(header);
            header.Width = header.Width; // trigger once

            return header;
        }

        private void LayoutHeaderButtons(Panel header)
        {
            int right = header.Width - 24;
            int y = 14;
            int gap = 8;

            foreach (var btn in new[] { btnRefresh, btnExport, btnImport, btnDelete, btnEdit, btnAdd })
            {
                btn.Location = new Point(right - btn.Width, y);
                right -= btn.Width + gap;
            }
        }

        // ── Sub-bar: search + stat cards ────────────────────────────────────
        private Panel BuildSubBar()
        {
            Panel bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = C_BG,
                Padding = new Padding(16, 10, 16, 10)
            };

            // Search box
            txtSearch = new Guna2TextBox
            {
                PlaceholderText = "🔍  Tìm kiếm nguyên liệu...",
                Font = new Font("Segoe UI", 10.5F),
                FillColor = C_WHITE,
                BorderColor = C_BORDER,
                BorderRadius = 8,
                Size = new Size(300, 38),
                Location = new Point(16, 14)
            };
            txtSearch.FocusedState.BorderColor = C_PURPLE;
            txtSearch.HoverState.BorderColor = C_PURPLE;
            txtSearch.TextChanged += (s, e) => ApplyFilter();

            // 4 stat cards
            Panel statRow = new Panel { BackColor = C_BG };

            var statCards = new[]
            {
                ("Tổng NL",     lblStatTotal, C_PURPLE,     C_PURPLE_SOFT),
                ("Ổn định",    lblStatOk,    C_GREEN_TEXT, C_GREEN_BG),
                ("Sắp hết",   lblStatLow,   C_AMBER_TEXT, C_AMBER_BG),
                ("Hết hàng",  lblStatOut,   C_RED_TEXT,   C_RED_BG),
            };

            // Build 4 stat cards dynamically
            Panel[] cards = new Panel[4];
            string[] captions = { "Tổng NL", "Ổn định", "Sắp hết", "Hết hàng" };
            Color[] valColors = { C_PURPLE, C_GREEN_TEXT, C_AMBER_TEXT, C_RED_TEXT };
            Color[] bgColors = { C_PURPLE_SOFT, C_GREEN_BG, C_AMBER_BG, C_RED_BG };
            Label[] valLabels = new Label[4];

            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                Panel card = new Panel
                {
                    BackColor = bgColors[i],
                    BorderStyle = BorderStyle.None,
                    Size = new Size(110, 54)
                };
                RoundPanel(card, 10);

                Label cap = new Label
                {
                    Text = captions[i],
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = valColors[i],
                    AutoSize = false,
                    Size = new Size(106, 18),
                    Location = new Point(8, 6),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label val = new Label
                {
                    Text = "—",
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = valColors[i],
                    AutoSize = false,
                    Size = new Size(106, 26),
                    Location = new Point(8, 24),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                card.Controls.Add(cap);
                card.Controls.Add(val);
                cards[i] = card;
                valLabels[i] = val;
            }

            lblStatTotal = valLabels[0];
            lblStatOk = valLabels[1];
            lblStatLow = valLabels[2];
            lblStatOut = valLabels[3];

            bar.Controls.Add(txtSearch);

            // Position stat cards on resize
            bar.Resize += (s, e) =>
            {
                int right = bar.Width - 16;
                int y = 14;
                int gap = 10;
                int w = 110;

                for (int i = 3; i >= 0; i--)
                {
                    cards[i].Location = new Point(right - w, y);
                    cards[i].Size = new Size(w, 54);
                    right -= w + gap;
                    if (!bar.Controls.Contains(cards[i]))
                        bar.Controls.Add(cards[i]);
                }
            };

            return bar;
        }

        // ── Card wrapper for DataGridView ────────────────────────────────────
        private Panel BuildCard(string titleText, Control content)
        {
            Panel outer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 8, 12, 8), BackColor = C_BG };

            Guna2Panel card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Padding = new Padding(0)
            };

            // Card title bar
            Panel titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 42,
                BackColor = C_WHITE
            };
            titleBar.Paint += (s, e) =>
            {
                using (Pen p = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, titleBar.Height - 1, titleBar.Width, titleBar.Height - 1);
            };

            Label lbl = new Label
            {
                Text = titleText,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(14, 12)
            };
            titleBar.Controls.Add(lbl);

            card.Controls.Add(content);
            card.Controls.Add(titleBar);
            content.BringToFront();

            outer.Controls.Add(card);
            return outer;
        }

        // ── DataGridView factory ─────────────────────────────────────────────
        private Guna2DataGridView CreateGrid()
        {
            var grid = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = C_BORDER,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Cursor = Cursors.Hand
            };

            // Header style
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersHeight = 38;
            grid.EnableHeadersVisualStyles = false;

            // Row style
            grid.DefaultCellStyle.BackColor = C_WHITE;
            grid.DefaultCellStyle.ForeColor = C_TEXT;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            grid.DefaultCellStyle.SelectionForeColor = C_TEXT;
            grid.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            grid.RowTemplate.Height = 40;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 253);

            return grid;
        }

        // ── Button factory ───────────────────────────────────────────────────
        private Guna2Button MakeBtn(string text, Color color, int width)
        {
            return new Guna2Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                FillColor = color,
                ForeColor = Color.White,
                BorderRadius = 8,
                Size = new Size(width, 36),
                Cursor = Cursors.Hand
            };
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

        // ==================== FILTER ====================
        private void ApplyFilter()
        {
            if (!(dgvInventory.DataSource is DataTable dt)) return;

            string kw = txtSearch.Text.Trim().Replace("'", "''");
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(kw)
                ? ""
                : $"IngredientName LIKE '%{kw}%' OR Unit LIKE '%{kw}%'";

            RefreshStats();
        }

        // ==================== STAT CARDS ====================
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

            // Column widths
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

                switch (status)
                {
                    case "Hết hàng":
                        e.CellStyle.ForeColor = C_RED_TEXT;
                        e.CellStyle.BackColor = C_RED_BG;
                        e.CellStyle.SelectionForeColor = C_RED_TEXT;
                        e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        break;
                    case "Sắp hết":
                        e.CellStyle.ForeColor = C_AMBER_TEXT;
                        e.CellStyle.BackColor = C_AMBER_BG;
                        e.CellStyle.SelectionForeColor = C_AMBER_TEXT;
                        e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        break;
                    default:
                        e.CellStyle.ForeColor = C_GREEN_TEXT;
                        e.CellStyle.BackColor = C_GREEN_BG;
                        e.CellStyle.SelectionForeColor = C_GREEN_TEXT;
                        e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                        break;
                }
            }

            // Highlight low / out-of-stock rows
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
                if (type == "IMPORT")
                {
                    row.Cells["TransactionType"].Value = "↓ Nhập kho";
                    row.Cells["TransactionType"].Style.ForeColor = C_GREEN_TEXT;
                    row.Cells["TransactionType"].Style.BackColor = C_GREEN_BG;
                    row.Cells["TransactionType"].Style.SelectionForeColor = C_GREEN_TEXT;
                    row.Cells["TransactionType"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (type == "EXPORT")
                {
                    row.Cells["TransactionType"].Value = "↑ Xuất kho";
                    row.Cells["TransactionType"].Style.ForeColor = C_AMBER_TEXT;
                    row.Cells["TransactionType"].Style.BackColor = C_AMBER_BG;
                    row.Cells["TransactionType"].Style.SelectionForeColor = C_AMBER_TEXT;
                    row.Cells["TransactionType"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
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
            if (!id.HasValue)
            {
                ShowInfo("Vui lòng chọn nguyên liệu cần sửa.");
                return;
            }
            using (var form = new IngredientEditForm(id.Value))
                if (form.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedID();
            if (!id.HasValue) { ShowInfo("Vui lòng chọn nguyên liệu cần xóa."); return; }

            if (MessageBox.Show("Bạn có chắc muốn xóa nguyên liệu này?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

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

        private void HideCol(DataGridView grid, string col)
        {
            if (grid.Columns.Contains(col)) grid.Columns[col].Visible = false;
        }

        private void SetHeader(DataGridView grid, string col, string header)
        {
            if (grid.Columns.Contains(col)) grid.Columns[col].HeaderText = header;
        }

        private void SetColWidth(DataGridView grid, string col, float fill)
        {
            if (!grid.Columns.Contains(col)) return;
            grid.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns[col].FillWeight = fill * 100f;
        }

        private void ShowInfo(string msg)
        {
            MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Adds rounded corners to a Panel via Region (no Guna dependency).</summary>
        private static void RoundPanel(Panel panel, int radius)
        {
            panel.Paint += (s, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int d = radius * 2;
                Rectangle r = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                panel.Region = new Region(path);
            };
        }
    }
}