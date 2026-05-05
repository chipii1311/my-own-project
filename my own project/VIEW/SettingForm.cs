using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class SettingForm : Form
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
        private static readonly Color C_RED = Color.FromArgb(220, 38, 38);
        private static readonly Color C_RED_S = Color.FromArgb(254, 226, 226);
        private static readonly Color C_TEXT = Color.FromArgb(17, 24, 39);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);
        private static readonly Color C_LABEL = Color.FromArgb(75, 85, 99);

        // ════════════════════════════════════════════════════════
        // CONTROLS — Tables tab
        // ════════════════════════════════════════════════════════
        private DataGridView dgvTables;
        private Guna2TextBox txtTableNumber, txtTableCapacity, txtTableID;
        private Guna2ComboBox cboTableStatus;
        private Label lblTableHint;
        private Guna2Button btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable;

        // ════════════════════════════════════════════════════════
        // CONTROLS — Categories tab
        // ════════════════════════════════════════════════════════
        private DataGridView dgvCategories;
        private Guna2TextBox txtCategoryName, txtCategoryID;
        private Label lblCatHint;
        private Guna2Button btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat;

        // Tab state
        private Panel pageTable, pageCat;
        private Guna2Button _activeTab;

        public SettingForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildUI();
            this.Load += (s, e) => { LoadTableData(); LoadCategoryData(); };
        }

        // ════════════════════════════════════════════════════════
        // UI BUILDER
        // ════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.SuspendLayout();

            // ── HEADER ─────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE
            };
            pnlHeader.Paint += PaintBottomBorder;

            var lblTitle = new Label
            {
                Text = "Cài đặt hệ thống",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(24, 18)
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── TAB BAR ─────────────────────────────────────────
            var pnlTabs = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = C_WHITE
            };
            pnlTabs.Paint += PaintBottomBorder;

            Label lblTitle = new Label();
            lblTitle.Text = "CÀI ĐẶT";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Margin = new Padding(10, 0, 0, 30);
            flpMenu.Controls.Add(lblTitle);

            btnTabTables.Click += (s, e) => SwitchTab(pageTable, btnTabTables,
                                                       btnTabCats);
            btnTabCats.Click += (s, e) => SwitchTab(pageCat, btnTabCats,
                                                       btnTabTables);
            pnlTabs.Controls.Add(btnTabTables);
            pnlTabs.Controls.Add(btnTabCats);
            _activeTab = btnTabTables;
            SetTabActive(btnTabTables);

            // ── PAGES ───────────────────────────────────────────
            pageTable = BuildPageTables();
            pageCat = BuildPageCategories();
            pageTable.Visible = true;
            pageCat.Visible = false;

            // ── ASSEMBLE ─────────────────────────────────────────
            // Fill first, then Tops
            this.Controls.Add(pageTable);
            this.Controls.Add(pageCat);
            this.Controls.Add(pnlTabs);
            this.Controls.Add(pnlHeader);

            this.ResumeLayout(false);
        }

        // ── TAB BUTTON ──────────────────────────────────────────
        private Guna2Button MakeTabBtn(string text, int x)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(160, 46),
                Location = new Point(x, 2),
                BorderRadius = 0,
                BorderThickness = 0,
                FillColor = Color.Transparent,
                ForeColor = C_MUTED,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void SetTabActive(Guna2Button btn)
        {
            btn.ForeColor = C_PURPLE;
            // Draw purple underline via Paint
            btn.Paint += (s, e) =>
            {
                if (btn.ForeColor == C_PURPLE)
                {
                    using (var b = new SolidBrush(C_PURPLE))
                        e.Graphics.FillRectangle(b, 0, btn.Height - 3, btn.Width, 3);
                }
            };
        }

        private void SwitchTab(Panel show, Guna2Button active, Guna2Button inactive)
        {
            pageTable.Visible = (show == pageTable);
            pageCat.Visible = (show == pageCat);
            show.BringToFront();

            active.ForeColor = C_PURPLE;
            inactive.ForeColor = C_MUTED;
            active.Refresh();
            inactive.Refresh();
        }

        // ════════════════════════════════════════════════════════
        // PAGE: QUẢN LÝ BÀN
        // ════════════════════════════════════════════════════════
        private Panel BuildPageTables()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = C_BG, Padding = new Padding(24, 20, 24, 24) };

            Label lblHeader = new Label { Text = "QUẢN LÝ BÀN ĂN", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.FromArgb(88, 28, 230), AutoSize = true, Dock = DockStyle.Top };
            pnl.Controls.Add(lblHeader);

            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlp.Padding = new Padding(0, 20, 0, 0);
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnl.Controls.Add(tlp);
            tlp.BringToFront();

            // ── LEFT: Grid card ──
            var cardLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Margin = new Padding(0, 0, 12, 0)
            };
            cardLeft.Region = RoundRegion(cardLeft, 12);
            cardLeft.Resize += (s, e) => cardLeft.Region = RoundRegion(cardLeft, 12);

            // Card header
            var cardHdr = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;

            var lblGridTitle = new Label
            {
                Text = "Danh sách bàn ăn",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(18, 15)
            };
            var lblTableCount = new Label
            {
                Name = "lblTableCount",
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(300, 18)
            };
            cardHdr.Controls.Add(lblGridTitle);
            cardHdr.Controls.Add(lblTableCount);
            cardHdr.Resize += (s, e) => lblTableCount.Location =
                new Point(cardHdr.Width - lblTableCount.Width - 18, 18);

            dgvTables = MakeGrid();
            dgvTables.CellClick += DgvTables_CellClick;
            dgvTables.CellFormatting += DgvTables_CellFormatting;

            cardLeft.Controls.Add(dgvTables);
            cardLeft.Controls.Add(cardHdr);
            tlp.Controls.Add(cardLeft, 0, 0);

            // ── RIGHT: Form card ──
            var cardRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Margin = new Padding(12, 0, 0, 0),
                Padding = new Padding(24)
            };
            cardRight.Region = RoundRegion(cardRight, 12);
            cardRight.Resize += (s, e) => cardRight.Region = RoundRegion(cardRight, 12);

            int fw = 0; // field width — set via Resize

            // Title
            var lblFormTitle = new Label
            {
                Text = "Thông tin bàn",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Dock = DockStyle.Top,
                Height = 36
            };

            var sep = new Panel { Dock = DockStyle.Top, Height = 2, BackColor = C_PURPLE, Margin = new Padding(0, 0, 0, 16) };

            // Hint label
            lblTableHint = new Label
            {
                Text = "👆 Nhấp vào bàn để chọn",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.Transparent
            };

            // Số bàn
            var lNum = MakeFieldLabel("Số bàn *");
            txtTableNumber = MakeTextBox("Nhập số bàn  (VD: 6)");
            txtTableID = new Guna2TextBox { Visible = false };

            // Sức chứa
            var lCap = MakeFieldLabel("Sức chứa (người)");
            txtTableCapacity = MakeTextBox("VD: 4");

            // Trạng thái
            var lStt = MakeFieldLabel("Trạng thái");
            cboTableStatus = new Guna2ComboBox
            {
                Dock = DockStyle.Top,
                Height = 38,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10F),
                FillColor = C_WHITE,
                Margin = new Padding(0, 0, 0, 16)
            };
            cboTableStatus.Items.AddRange(new object[] { "Trống", "Đang dùng", "Đã đặt" });
            cboTableStatus.SelectedIndex = 0;

            // Buttons
            var pnlBtns = new Panel { Dock = DockStyle.Top, Height = 100 };

            btnAddTable = MakeBtn("➕  Thêm bàn mới", C_PURPLE, C_WHITE);
            btnAddTable.Size = new Size(200, 40);
            btnAddTable.Location = new Point(0, 0);
            btnAddTable.Click += BtnAddTable_Click;

            btnSaveTable = MakeBtn("💾  Lưu thay đổi", C_BLUE, C_WHITE);
            btnSaveTable.Size = new Size(200, 40);
            btnSaveTable.Location = new Point(0, 50);
            btnSaveTable.Enabled = false;
            btnSaveTable.Click += BtnEditTable_Click;

            btnDeleteTable = MakeBtn("🗑️  Xóa bàn này", C_RED, C_WHITE);
            btnDeleteTable.Size = new Size(130, 40);
            btnDeleteTable.Location = new Point(210, 0);
            btnDeleteTable.Enabled = false;
            btnDeleteTable.Click += BtnDeleteTable_Click;

            btnClearTable = MakeBtn("✕  Hủy", Color.FromArgb(229, 231, 235), C_MUTED);
            btnClearTable.Size = new Size(80, 40);
            btnClearTable.Location = new Point(350, 0);
            btnClearTable.Click += (s, e) => ClearTableForm();

            pnlBtns.Controls.AddRange(new Control[]
            { btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable });

            // Assemble right panel (reverse order for DockStyle.Top)
            foreach (var c in new Control[]
            { pnlBtns, cboTableStatus, lStt, txtTableCapacity, lCap,
              txtTableNumber, lNum, lblTableHint, sep, lblFormTitle, txtTableID })
                cardRight.Controls.Add(c);

            // Resize: update fields width
            cardRight.Resize += (s, e) =>
            {
                int w = cardRight.ClientSize.Width - 48;
                foreach (var c in new Control[]
                { txtTableNumber, txtTableCapacity })
                {
                    if (c is Guna2TextBox tb) tb.Width = w;
                }
                cboTableStatus.Width = w;
                pnlBtns.Width = w;
                btnAddTable.Width = (w - 10) / 2;
                btnSaveTable.Width = (w - 10) / 2;
                btnDeleteTable.Width = (w - 10) / 2;
                btnClearTable.Location = new Point((w - 10) / 2 + 10, 50);
                btnClearTable.Width = (w - 10) / 2;
            };

            tlp.Controls.Add(cardRight, 1, 0);
            return pnl;
        }

        // ════════════════════════════════════════════════════════
        // PAGE: DANH MỤC MÓN
        // ════════════════════════════════════════════════════════
        private Panel BuildPageCategories()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = C_BG, Padding = new Padding(24, 20, 24, 24) };

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnl.Controls.Add(tlp);

            // ── LEFT: Grid card ──
            var cardLeft = new Panel { Dock = DockStyle.Fill, BackColor = C_WHITE, Margin = new Padding(0, 0, 12, 0) };
            cardLeft.Region = RoundRegion(cardLeft, 12);
            cardLeft.Resize += (s, e) => cardLeft.Region = RoundRegion(cardLeft, 12);

            var cardHdr = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;
            var lblCatTitle = new Label { Text = "Danh sách danh mục", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(18, 15) };
            cardHdr.Controls.Add(lblCatTitle);

            dgvCategories = MakeGrid();
            dgvCategories.CellClick += DgvCategories_CellClick;

            cardLeft.Controls.Add(dgvCategories);
            cardLeft.Controls.Add(cardHdr);
            tlp.Controls.Add(cardLeft, 0, 0);

            // ── RIGHT: Form card ──
            var cardRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Margin = new Padding(12, 0, 0, 0),
                Padding = new Padding(24)
            };
            cardRight.Region = RoundRegion(cardRight, 12);
            cardRight.Resize += (s, e) => cardRight.Region = RoundRegion(cardRight, 12);

            var lblFormTitle = new Label { Text = "Thông tin danh mục", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = C_TEXT, Dock = DockStyle.Top, Height = 36 };
            var sep = new Panel { Dock = DockStyle.Top, Height = 2, BackColor = C_PURPLE };

            lblCatHint = new Label
            {
                Text = "👆 Nhấp vào danh mục để chọn",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.Transparent
            };

            var lName = MakeFieldLabel("Tên danh mục *");
            txtCategoryName = MakeTextBox("VD: Đồ uống, Món chính...");
            txtCategoryID = new Guna2TextBox { Visible = false };

            var pnlBtns = new Panel { Dock = DockStyle.Top, Height = 100 };

            btnAddCat = MakeBtn("➕  Thêm danh mục", C_PURPLE, C_WHITE);
            btnAddCat.Size = new Size(200, 40);
            btnAddCat.Location = new Point(0, 0);
            btnAddCat.Click += BtnAddCategory_Click;

            btnSaveCat = MakeBtn("💾  Lưu thay đổi", C_BLUE, C_WHITE);
            btnSaveCat.Size = new Size(200, 40);
            btnSaveCat.Location = new Point(0, 50);
            btnSaveCat.Enabled = false;
            btnSaveCat.Click += BtnEditCategory_Click;

            btnDeleteCat = MakeBtn("🗑️  Xóa", C_RED, C_WHITE);
            btnDeleteCat.Size = new Size(130, 40);
            btnDeleteCat.Location = new Point(210, 0);
            btnDeleteCat.Enabled = false;
            btnDeleteCat.Click += BtnDeleteCategory_Click;

            btnClearCat = MakeBtn("✕  Hủy", Color.FromArgb(229, 231, 235), C_MUTED);
            btnClearCat.Size = new Size(80, 40);
            btnClearCat.Location = new Point(350, 0);
            btnClearCat.Click += (s, e) => ClearCatForm();

            pnlBtns.Controls.AddRange(new Control[]
            { btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat });

            foreach (var c in new Control[]
            { pnlBtns, txtCategoryName, lName, lblCatHint, sep, lblFormTitle, txtCategoryID })
                cardRight.Controls.Add(c);

            cardRight.Resize += (s, e) =>
            {
                int w = cardRight.ClientSize.Width - 48;
                txtCategoryName.Width = w;
                pnlBtns.Width = w;
                btnAddCat.Width = (w - 10) / 2;
                btnSaveCat.Width = (w - 10) / 2;
                btnDeleteCat.Width = (w - 10) / 2;
                btnClearCat.Location = new Point((w - 10) / 2 + 10, 50);
                btnClearCat.Width = (w - 10) / 2;
            };

            tlp.Controls.Add(cardRight, 1, 0);
            return pnl;
        }

        // ════════════════════════════════════════════════════════
        // FACTORY HELPERS
        // ════════════════════════════════════════════════════════
        private DataGridView MakeGrid()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(243, 244, 246),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Cursor = Cursors.Hand,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 44,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgv.DefaultCellStyle.ForeColor = C_TEXT;
            dgv.DefaultCellStyle.SelectionBackColor = C_PURPLE_S;
            dgv.DefaultCellStyle.SelectionForeColor = C_PURPLE;
            dgv.DefaultCellStyle.BackColor = C_WHITE;
            dgv.DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.RowTemplate.Height = 46;
            return dgv;
        }

        private Label MakeFieldLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = C_LABEL,
            Dock = DockStyle.Top,
            Height = 26,
            BackColor = Color.Transparent
        };

        private Guna2TextBox MakeTextBox(string placeholder) => new Guna2TextBox
        {
            PlaceholderText = placeholder,
            Dock = DockStyle.Top,
            Height = 38,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = Color.FromArgb(249, 250, 251),
            //FocusedBorderColor = C_PURPLE,
            Margin = new Padding(0, 0, 0, 16)
        };

        private Guna2Button MakeBtn(string text, Color fill, Color fore) => new Guna2Button
        {
            Text = text,
            Height = 40,
            BorderRadius = 8,
            BorderThickness = 0,
            FillColor = fill,
            ForeColor = fore,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        // ════════════════════════════════════════════════════════
        // PAINT HELPERS
        // ════════════════════════════════════════════════════════
        private void PaintBottomBorder(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (var pen = new System.Drawing.Pen(C_BORDER, 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }
        private void PaintBottomBorderLight(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (var pen = new System.Drawing.Pen(Color.FromArgb(243, 244, 246), 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private System.Drawing.Region RoundRegion(Control c, int r)
        {
            var path = new GraphicsPath();
            int w = c.Width, h = c.Height;
            if (w <= 0 || h <= 0) return null;
            path.AddArc(0, 0, r * 2, r * 2, 180, 90);
            path.AddArc(w - r * 2, 0, r * 2, r * 2, 270, 90);
            path.AddArc(w - r * 2, h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(0, h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseAllFigures();
            return new System.Drawing.Region(path);
        }

        // ════════════════════════════════════════════════════════
        // LOAD DATA
        // ════════════════════════════════════════════════════════
        private void LoadTableData()
        {
            try
            {
                string q = @"SELECT TableID AS [Mã],
                                    TableNumber AS [Số bàn],
                                    Capacity    AS [Sức chứa],
                                    Status      AS [Trạng thái]
                             FROM DiningTable
                             ORDER BY TableNumber";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(q);
                dgvTables.DataSource = dt;

                // Ẩn cột ID
                if (dgvTables.Columns.Contains("Mã"))
                    dgvTables.Columns["Mã"].Visible = false;

                // Update count label
                var lbl = pageTable.Controls.Find("lblTableCount", true);
                if (lbl.Length > 0) lbl[0].Text = $"{dt.Rows.Count} bàn";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bàn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategoryData()
        {
            try
            {
                string q = @"SELECT CategoryID   AS [Mã],
                                    CategoryName AS [Tên danh mục]
                             FROM Category
                             WHERE IsActive = 1
                             ORDER BY CategoryID";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(q);
                dgvCategories.DataSource = dt;

                if (dgvCategories.Columns.Contains("Mã"))
                    dgvCategories.Columns["Mã"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        // CELL FORMATTING — màu trạng thái bàn
        // ════════════════════════════════════════════════════════
        private void DgvTables_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvTables.Columns[e.ColumnIndex].Name != "Trạng thái") return;

            string v = e.Value?.ToString() ?? "";
            e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            switch (v)
            {
                case "Trống":
                    e.CellStyle.ForeColor = C_GREEN;
                    break;
                case "Đang dùng":
                    e.CellStyle.ForeColor = C_RED;
                    break;
                case "Đã đặt":
                    e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6); // amber
                    break;
            }
        }

        // ════════════════════════════════════════════════════════
        // EVENTS — TABLES
        // ════════════════════════════════════════════════════════
        private void DgvTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvTables.Rows[e.RowIndex];
            txtTableID.Text = row.Cells["Mã"].Value?.ToString() ?? "";
            txtTableNumber.Text = row.Cells["Số bàn"].Value?.ToString() ?? "";
            txtTableCapacity.Text = dgvTables.Columns.Contains("Sức chứa")
                ? row.Cells["Sức chứa"].Value?.ToString() ?? "" : "";
            cboTableStatus.Text = row.Cells["Trạng thái"].Value?.ToString() ?? "Trống";

            lblTableHint.Text = "✏️  Đang chỉnh sửa bàn đã chọn";
            lblTableHint.ForeColor = C_PURPLE;
            btnSaveTable.Enabled = true;
            btnDeleteTable.Enabled = true;
        }

        private void BtnAddTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableNumber.Text))
            { ShowWarn("Vui lòng nhập số bàn!"); return; }
            if (!int.TryParse(txtTableNumber.Text, out int num))
            { ShowWarn("Số bàn chỉ được nhập số (VD: 1, 2, 3...)"); return; }

            int cap = 4;
            if (!string.IsNullOrWhiteSpace(txtTableCapacity.Text))
                int.TryParse(txtTableCapacity.Text, out cap);

            try
            {
                string q = $@"INSERT INTO DiningTable (TableNumber, Capacity, Status)
                              VALUES ({num}, {cap}, N'{cboTableStatus.Text}')";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(q);
                ShowInfo("✅  Thêm bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex) { ShowError("Lỗi thêm bàn: " + ex.Message); }
        }

        private void BtnEditTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableID.Text))
            { ShowWarn("Vui lòng chọn bàn cần sửa!"); return; }
            if (!int.TryParse(txtTableNumber.Text, out int num))
            { ShowWarn("Số bàn chỉ được nhập số!"); return; }

            int cap = 4;
            if (!string.IsNullOrWhiteSpace(txtTableCapacity.Text))
                int.TryParse(txtTableCapacity.Text, out cap);

            try
            {
                string q = $@"UPDATE DiningTable
                              SET TableNumber = {num},
                                  Capacity    = {cap},
                                  Status      = N'{cboTableStatus.Text}'
                              WHERE TableID = {txtTableID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(q);
                ShowInfo("✅  Cập nhật bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex) { ShowError("Lỗi cập nhật: " + ex.Message); }
        }

        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableID.Text))
            { ShowWarn("Vui lòng chọn bàn cần xóa!"); return; }

            if (MessageBox.Show($"Xóa bàn {txtTableNumber.Text}?  Hành động không thể hoàn tác.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                string q = $"DELETE FROM DiningTable WHERE TableID = {txtTableID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(q);
                ShowInfo("✅  Đã xóa bàn!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex) { ShowError("Lỗi xóa: " + ex.Message); }
        }

        private void ClearTableForm()
        {
            txtTableID.Text = "";
            txtTableNumber.Text = "";
            txtTableCapacity.Text = "";
            cboTableStatus.SelectedIndex = 0;
            lblTableHint.Text = "👆 Nhấp vào bàn để chọn";
            lblTableHint.ForeColor = C_MUTED;
            btnSaveTable.Enabled = false;
            btnDeleteTable.Enabled = false;
        }

        // ════════════════════════════════════════════════════════
        // EVENTS — CATEGORIES
        // ════════════════════════════════════════════════════════
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCategories.Rows[e.RowIndex];
            txtCategoryID.Text = row.Cells["Mã"].Value?.ToString() ?? "";
            txtCategoryName.Text = row.Cells["Tên danh mục"].Value?.ToString() ?? "";

            lblCatHint.Text = "✏️  Đang chỉnh sửa danh mục đã chọn";
            lblCatHint.ForeColor = C_PURPLE;
            btnSaveCat.Enabled = true;
            btnDeleteCat.Enabled = true;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            { ShowWarn("Vui lòng nhập tên danh mục!"); return; }

            try
            {
                string q = $"INSERT INTO Category (CategoryName, IsActive) VALUES (N'{txtCategoryName.Text.Trim()}', 1)";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(q);
                ShowInfo("✅  Thêm danh mục thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi thêm: " + ex.Message); }
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            { ShowWarn("Vui lòng chọn danh mục cần sửa!"); return; }

            try
            {
                string q = $"UPDATE Category SET CategoryName = N'{txtCategoryName.Text.Trim()}' WHERE CategoryID = {txtCategoryID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(q);
                ShowInfo("✅  Cập nhật thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi cập nhật: " + ex.Message); }
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            { ShowWarn("Vui lòng chọn danh mục cần xóa!"); return; }

            if (MessageBox.Show($"Ẩn danh mục \"{txtCategoryName.Text}\"?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                string q = $"UPDATE Category SET IsActive = 0 WHERE CategoryID = {txtCategoryID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(q);
                ShowInfo("✅  Đã xóa danh mục!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi xóa: " + ex.Message); }
        }

        private void ClearCatForm()
        {
            txtCategoryID.Text = "";
            txtCategoryName.Text = "";
            lblCatHint.Text = "👆 Nhấp vào danh mục để chọn";
            lblCatHint.ForeColor = C_MUTED;
            btnSaveCat.Enabled = false;
            btnDeleteCat.Enabled = false;
        }

        // ════════════════════════════════════════════════════════
        // MESSAGE HELPERS
        // ════════════════════════════════════════════════════════
        private void ShowInfo(string msg)
            => MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowWarn(string msg)
            => MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void ShowError(string msg)
            => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}