using Guna.UI2.WinForms;
using my_own_project.DAL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class SettingForm : Form
    {
        // ===================== DESIGN TOKENS =====================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_MID = Color.FromArgb(109, 60, 240);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(237, 233, 254);
        private static readonly Color C_GREEN = Color.FromArgb(22, 163, 74);
        private static readonly Color C_GREEN_BG = Color.FromArgb(220, 252, 231);
        private static readonly Color C_RED = Color.FromArgb(220, 38, 38);
        private static readonly Color C_RED_BG = Color.FromArgb(254, 226, 226);
        private static readonly Color C_AMBER = Color.FromArgb(217, 119, 6);
        private static readonly Color C_AMBER_BG = Color.FromArgb(254, 243, 199);
        private static readonly Color C_BLUE = Color.FromArgb(37, 99, 235);
        private static readonly Color C_TEXT = Color.FromArgb(17, 24, 39);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);
        private static readonly Color C_FIELD_BG = Color.FromArgb(249, 250, 251);
        private static readonly Color C_LABEL = Color.FromArgb(55, 65, 81);

        // ===================== CONTROLS — Tables =====================
        private DataGridView dgvTables;
        private Guna2TextBox txtTableNumber, txtTableCapacity, txtTableID;
        private Guna2ComboBox cboTableStatus;
        private Label lblTableHint, lblTableCount;
        private Guna2Button btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable;

        // ===================== CONTROLS — Categories =====================
        private DataGridView dgvCategories;
        private Guna2TextBox txtCategoryName, txtCategoryID;
        private Label lblCatHint;
        private Guna2Button btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat;

        // ===================== TAB STATE =====================
        private Panel pageTable, pageCat;
        private Guna2Button _tabTables, _tabCats;

        public SettingForm()
        {
            InitializeComponent();
            Controls.Clear();
            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BuildUI();
            Load += (s, e) => { LoadTableData(); LoadCategoryData(); };
        }

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            SuspendLayout();

            Panel header = BuildHeader();
            Panel tabBar = BuildTabBar();

            pageTable = BuildPageTables();
            pageCat = BuildPageCategories();

            pageTable.Visible = true;
            pageCat.Visible = false;

            Controls.Add(pageTable);
            Controls.Add(pageCat);
            Controls.Add(tabBar);
            Controls.Add(header);

            ResumeLayout(false);
        }

        // ── Header ─────────────────────────────────────────────────────────
        private Panel BuildHeader()
        {
            Panel h = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE
            };
            h.Paint += PaintBottomBorder;

            

            Label title = new Label
            {
                Text = "Cài đặt hệ thống",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(52, 21)
            };

            h.Controls.AddRange(new Control[] {  title });
            return h;
        }

        // ── Tab bar ─────────────────────────────────────────────────────────
        private Panel BuildTabBar()
        {
            Panel bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = C_WHITE
            };
            bar.Paint += PaintBottomBorder;

            _tabTables = MakeTabBtn("🪑  Quản lý bàn ăn", 24);
            _tabCats = MakeTabBtn("📋  Danh mục món ăn", 210);

            _tabTables.Click += (s, e) => SwitchTab(pageTable, _tabTables, _tabCats);
            _tabCats.Click += (s, e) => SwitchTab(pageCat, _tabCats, _tabTables);

            SetTabActive(_tabTables, true);
            SetTabActive(_tabCats, false);

            bar.Controls.AddRange(new Control[] { _tabTables, _tabCats });
            return bar;
        }

        private Guna2Button MakeTabBtn(string text, int x)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(180, 46),
                Location = new Point(x, 2),
                BorderRadius = 0,
                BorderThickness = 0,
                FillColor = Color.Transparent,
                ForeColor = C_MUTED,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void SetTabActive(Guna2Button btn, bool active)
        {
            btn.ForeColor = active ? C_PURPLE : C_MUTED;
            btn.Paint -= TabBtn_Paint; // remove previous handler
            if (active) btn.Paint += TabBtn_Paint;
            btn.Refresh();
        }

        private void TabBtn_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Guna2Button;
            if (btn == null) return;
            using (SolidBrush b = new SolidBrush(C_PURPLE))
                e.Graphics.FillRectangle(b, 0, btn.Height - 3, btn.Width, 3);
        }

        private void SwitchTab(Panel show, Guna2Button active, Guna2Button inactive)
        {
            pageTable.Visible = (show == pageTable);
            pageCat.Visible = (show == pageCat);
            show.BringToFront();
            SetTabActive(active, true);
            SetTabActive(inactive, false);
        }

        // ===================== PAGE: QUẢN LÝ BÀN ĂN =====================
        private Panel BuildPageTables()
        {
            Panel page = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG,
                Padding = new Padding(24, 16, 24, 24)
            };

            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ── Left card: table grid ──
            Panel leftCard = CreateCard(new Padding(0, 0, 10, 0));

            // Card header
            Panel cardHdr = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;

            Label lblTitle = new Label
            {
                Text = "Danh sách bàn ăn",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(18, 16)
            };

            lblTableCount = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            cardHdr.Controls.AddRange(new Control[] { lblTitle, lblTableCount });
            cardHdr.Resize += (s, e) =>
                lblTableCount.Location = new Point(cardHdr.Width - lblTableCount.Width - 18, 18);

            dgvTables = MakeGrid();
            dgvTables.CellClick += DgvTables_CellClick;
            dgvTables.CellFormatting += DgvTables_CellFormatting;

            leftCard.Controls.Add(dgvTables);
            leftCard.Controls.Add(cardHdr);

            // ── Right card: form ──
            Panel rightCard = CreateCard(new Padding(10, 0, 0, 0));
            rightCard.Padding = new Padding(26, 22, 26, 22);

            BuildTableForm(rightCard);

            tlp.Controls.Add(leftCard, 0, 0);
            tlp.Controls.Add(rightCard, 1, 0);
            page.Controls.Add(tlp);
            return page;
        }

        private void BuildTableForm(Panel card)
        {
            // Title + separator
            Label lblTitle = new Label
            {
                Text = "Thông tin bàn",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Dock = DockStyle.Top,
                Height = 36
            };

            Panel sep = new Panel { Dock = DockStyle.Top, Height = 3, BackColor = C_PURPLE, Margin = new Padding(0, 0, 0, 6) };

            lblTableHint = new Label
            {
                Text = "✦  Nhấp vào bàn để chọn",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.Transparent
            };

            // Hidden ID
            txtTableID = new Guna2TextBox { Visible = false };

            // Fields
            Label lNum = FieldLabel("Số bàn *");
            txtTableNumber = FieldTextBox("Nhập số bàn  (VD: 6)");

            Label lCap = FieldLabel("Sức chứa (người)");
            txtTableCapacity = FieldTextBox("VD: 4");

            Label lStt = FieldLabel("Trạng thái");

            cboTableStatus = new Guna2ComboBox
            {
                Dock = DockStyle.Top,
                Height = 40,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10F),
                FillColor = C_FIELD_BG,
                Margin = new Padding(0, 0, 0, 18)
            };
            cboTableStatus.Items.AddRange(new object[] { "Trống", "Có khách", "Đặt trước" });
            cboTableStatus.SelectedIndex = 0;

            // Buttons: 2 x 2 grid in a panel
            Panel pnlBtns = new Panel { Dock = DockStyle.Top, Height = 100 };

            btnAddTable = BtnPrimary("+ Thêm bàn mới", C_PURPLE);
            btnDeleteTable = BtnPrimary("🗑  Xóa bàn này", Color.FromArgb(210, 210, 218));
            btnDeleteTable.ForeColor = C_MUTED;
            btnDeleteTable.Enabled = false;

            btnSaveTable = BtnPrimary("💾  Lưu thay đổi", Color.FromArgb(210, 210, 218));
            btnSaveTable.ForeColor = C_MUTED;
            btnSaveTable.Enabled = false;

            btnClearTable = BtnPrimary("✕  Hủy", Color.FromArgb(235, 235, 240));
            btnClearTable.ForeColor = C_MUTED;

            btnAddTable.Click += BtnAddTable_Click;
            btnSaveTable.Click += BtnSaveTable_Click;
            btnDeleteTable.Click += BtnDeleteTable_Click;
            btnClearTable.Click += (s, e) => ClearTableForm();

            pnlBtns.Controls.AddRange(new Control[] { btnAddTable, btnDeleteTable, btnSaveTable, btnClearTable });

            // Layout buttons on resize
            pnlBtns.Resize += (s, e) => LayoutBtns2x2(pnlBtns,
                btnAddTable, btnDeleteTable, btnSaveTable, btnClearTable);

            // Assemble — DockStyle.Top reads in reverse
            foreach (Control c in new Control[]
                { pnlBtns, cboTableStatus, lStt, txtTableCapacity, lCap,
                  txtTableNumber, lNum, lblTableHint, sep, lblTitle, txtTableID })
                card.Controls.Add(c);

            card.Resize += (s, e) =>
            {
                int w = card.ClientSize.Width - card.Padding.Horizontal;
                txtTableNumber.Width = w;
                txtTableCapacity.Width = w;
                cboTableStatus.Width = w;
                pnlBtns.Width = w;
                LayoutBtns2x2(pnlBtns, btnAddTable, btnDeleteTable, btnSaveTable, btnClearTable);
            };
        }

        // ===================== PAGE: DANH MỤC MÓN ĂN =====================
        private Panel BuildPageCategories()
        {
            Panel page = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG,
                Padding = new Padding(24, 16, 24, 24)
            };

            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ── Left card: category grid ──
            Panel leftCard = CreateCard(new Padding(0, 0, 10, 0));

            Panel cardHdr = new Panel { Dock = DockStyle.Top, Height = 52, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;

            Label lblCatTitle = new Label
            {
                Text = "Danh sách danh mục",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(18, 16)
            };
            cardHdr.Controls.Add(lblCatTitle);

            dgvCategories = MakeGrid();
            dgvCategories.CellClick += DgvCategories_CellClick;

            leftCard.Controls.Add(dgvCategories);
            leftCard.Controls.Add(cardHdr);

            // ── Right card: form ──
            Panel rightCard = CreateCard(new Padding(10, 0, 0, 0));
            rightCard.Padding = new Padding(26, 22, 26, 22);
            BuildCategoryForm(rightCard);

            tlp.Controls.Add(leftCard, 0, 0);
            tlp.Controls.Add(rightCard, 1, 0);
            page.Controls.Add(tlp);
            return page;
        }

        private void BuildCategoryForm(Panel card)
        {
            Label lblTitle = new Label
            {
                Text = "Thông tin danh mục",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Dock = DockStyle.Top,
                Height = 36
            };

            Panel sep = new Panel { Dock = DockStyle.Top, Height = 3, BackColor = C_PURPLE };

            lblCatHint = new Label
            {
                Text = "✦  Nhấp vào danh mục để chọn",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.Transparent
            };

            txtCategoryID = new Guna2TextBox { Visible = false };

            Label lName = FieldLabel("Tên danh mục *");
            txtCategoryName = FieldTextBox("VD: Đồ uống, Món chính...");

            Panel pnlBtns = new Panel { Dock = DockStyle.Top, Height = 100 };

            btnAddCat = BtnPrimary("+ Thêm danh mục", C_PURPLE);
            btnDeleteCat = BtnPrimary("🗑  Xóa", Color.FromArgb(210, 210, 218));
            btnDeleteCat.ForeColor = C_MUTED;
            btnDeleteCat.Enabled = false;

            btnSaveCat = BtnPrimary("💾  Lưu thay đổi", Color.FromArgb(210, 210, 218));
            btnSaveCat.ForeColor = C_MUTED;
            btnSaveCat.Enabled = false;

            btnClearCat = BtnPrimary("✕  Hủy", Color.FromArgb(235, 235, 240));
            btnClearCat.ForeColor = C_MUTED;

            btnAddCat.Click += BtnAddCategory_Click;
            btnSaveCat.Click += BtnEditCategory_Click;
            btnDeleteCat.Click += BtnDeleteCategory_Click;
            btnClearCat.Click += (s, e) => ClearCatForm();

            pnlBtns.Controls.AddRange(new Control[] { btnAddCat, btnDeleteCat, btnSaveCat, btnClearCat });
            pnlBtns.Resize += (s, e) => LayoutBtns2x2(pnlBtns, btnAddCat, btnDeleteCat, btnSaveCat, btnClearCat);

            foreach (Control c in new Control[]
                { pnlBtns, txtCategoryName, lName, lblCatHint, sep, lblTitle, txtCategoryID })
                card.Controls.Add(c);

            card.Resize += (s, e) =>
            {
                int w = card.ClientSize.Width - card.Padding.Horizontal;
                txtCategoryName.Width = w;
                pnlBtns.Width = w;
                LayoutBtns2x2(pnlBtns, btnAddCat, btnDeleteCat, btnSaveCat, btnClearCat);
            };
        }

        // ===================== FACTORY HELPERS =====================
        private Panel CreateCard(Padding margin)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Margin = margin
            };
            ApplyRoundCorners(card, 12);
            return card;
        }

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
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(14, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dgv.DefaultCellStyle.ForeColor = C_TEXT;
            dgv.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            dgv.DefaultCellStyle.SelectionForeColor = C_PURPLE;
            dgv.DefaultCellStyle.BackColor = C_WHITE;
            dgv.DefaultCellStyle.Padding = new Padding(14, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 253);
            dgv.RowTemplate.Height = 48;

            return dgv;
        }

        private Label FieldLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = C_LABEL,
            Dock = DockStyle.Top,
            Height = 28,
            BackColor = Color.Transparent
        };

        private Guna2TextBox FieldTextBox(string placeholder) => new Guna2TextBox
        {
            PlaceholderText = placeholder,
            Dock = DockStyle.Top,
            Height = 40,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = C_FIELD_BG,
            BorderColor = C_BORDER,
            Margin = new Padding(0, 0, 0, 16)
        };

        private Guna2Button BtnPrimary(string text, Color fillColor) => new Guna2Button
        {
            Text = text,
            Height = 42,
            BorderRadius = 9,
            BorderThickness = 0,
            FillColor = fillColor,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        /// <summary>Lays out 4 buttons in a 2×2 grid within pnl.</summary>
        private static void LayoutBtns2x2(Panel pnl, Guna2Button b1, Guna2Button b2, Guna2Button b3, Guna2Button b4)
        {
            int gap = 8;
            int half = (pnl.Width - gap) / 2;
            if (half < 20) return;

            b1.Location = new Point(0, 0); b1.Size = new Size(half, 42);
            b2.Location = new Point(half + gap, 0); b2.Size = new Size(half, 42);
            b3.Location = new Point(0, 50); b3.Size = new Size(half, 42);
            b4.Location = new Point(half + gap, 50); b4.Size = new Size(half, 42);
        }

        // ===================== ROUND CORNERS =====================
        private static void ApplyRoundCorners(Panel panel, int radius)
        {
            panel.Paint += (s, e) =>
            {
                var p = s as Panel;
                if (p == null || p.Width <= 0 || p.Height <= 0) return;
                var path = new GraphicsPath();
                int w = p.Width - 1, h = p.Height - 1, d = radius * 2;
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(w - d, 0, d, d, 270, 90);
                path.AddArc(w - d, h - d, d, d, 0, 90);
                path.AddArc(0, h - d, d, d, 90, 90);
                path.CloseFigure();
                p.Region = new Region(path);
            };
        }

        // ===================== PAINT HELPERS =====================
        private void PaintBottomBorder(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (Pen pen = new Pen(C_BORDER, 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void PaintBottomBorderLight(object s, PaintEventArgs e)
        {
            var p = s as Panel;
            using (Pen pen = new Pen(Color.FromArgb(243, 244, 246), 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }

        // ===================== LOAD DATA =====================
        private void LoadTableData()
        {
            try
            {
                string q = @"
                    SELECT TableID     AS [TableID],
                           TableNumber AS [Số bàn],
                           Capacity    AS [Sức chứa],
                           Status      AS [Trạng thái]
                    FROM   DiningTable
                    ORDER  BY TableNumber";

                DataTable dt = DataHelper.ExecuteQuery(q);
                dgvTables.DataSource = dt;

                if (dgvTables.Columns.Contains("TableID"))
                    dgvTables.Columns["TableID"].Visible = false;

                if (lblTableCount != null)
                    lblTableCount.Text = dt.Rows.Count + " bàn";
            }
            catch (Exception ex)
            {
                ShowError("Lỗi tải danh sách bàn: " + ex.Message);
            }
        }

        private void LoadCategoryData()
        {
            try
            {
                string q = @"
                    SELECT CategoryID   AS [CategoryID],
                           CategoryName AS [Tên danh mục]
                    FROM   Category
                    WHERE  IsActive = 1
                    ORDER  BY CategoryID";

                DataTable dt = DataHelper.ExecuteQuery(q);
                dgvCategories.DataSource = dt;

                if (dgvCategories.Columns.Contains("CategoryID"))
                    dgvCategories.Columns["CategoryID"].Visible = false;
            }
            catch (Exception ex)
            {
                ShowError("Lỗi tải danh mục: " + ex.Message);
            }
        }

        // ===================== CELL FORMATTING =====================
        private void DgvTables_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvTables.Columns[e.ColumnIndex].Name != "Trạng thái") return;

            string v = e.Value?.ToString() ?? "";
            e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            switch (v)
            {
                case "Trống":
                    e.CellStyle.ForeColor = C_GREEN;
                    e.CellStyle.BackColor = C_GREEN_BG;
                    e.CellStyle.SelectionForeColor = C_GREEN;
                    break;
                case "Có khách":
                    e.CellStyle.ForeColor = C_PURPLE_MID;
                    e.CellStyle.BackColor = C_PURPLE_SOFT;
                    e.CellStyle.SelectionForeColor = C_PURPLE_MID;
                    break;
                case "Đặt trước":
                    e.CellStyle.ForeColor = C_AMBER;
                    e.CellStyle.BackColor = C_AMBER_BG;
                    e.CellStyle.SelectionForeColor = C_AMBER;
                    break;
            }
        }

        // ===================== EVENTS — TABLES =====================
        private void DgvTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvTables.Rows[e.RowIndex];
            txtTableID.Text = row.Cells["TableID"].Value?.ToString() ?? "";
            txtTableNumber.Text = row.Cells["Số bàn"].Value?.ToString() ?? "";
            txtTableCapacity.Text = dgvTables.Columns.Contains("Sức chứa")
                                    ? row.Cells["Sức chứa"].Value?.ToString() ?? "" : "";
            cboTableStatus.Text = row.Cells["Trạng thái"].Value?.ToString() ?? "Trống";

            lblTableHint.Text = "✏  Đang chỉnh sửa bàn " + txtTableNumber.Text;
            lblTableHint.ForeColor = C_PURPLE;

            btnSaveTable.FillColor = C_BLUE;
            btnSaveTable.ForeColor = Color.White;
            btnSaveTable.Enabled = true;

            btnDeleteTable.FillColor = C_RED;
            btnDeleteTable.ForeColor = Color.White;
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
                DataHelper.ExecuteNonQuery(q);
                ShowInfo("✔  Thêm bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex) { ShowError("Lỗi thêm bàn: " + ex.Message); }
        }

        private void BtnSaveTable_Click(object sender, EventArgs e)
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
                DataHelper.ExecuteNonQuery(q);
                ShowInfo("✔  Cập nhật bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex) { ShowError("Lỗi cập nhật: " + ex.Message); }
        }

        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTableID.Text))
            { ShowWarn("Vui lòng chọn bàn cần xóa!"); return; }

            if (MessageBox.Show(
                    $"Xóa bàn số {txtTableNumber.Text}? Hành động không thể hoàn tác.",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                DataHelper.ExecuteNonQuery($"DELETE FROM DiningTable WHERE TableID = {txtTableID.Text}");
                ShowInfo("✔  Xóa bàn thành công!");
                ClearTableForm();
                LoadTableData();
            }
            catch (Exception ex) { ShowError("Lỗi xóa bàn: " + ex.Message); }
        }

        private void ClearTableForm()
        {
            txtTableID.Text = "";
            txtTableNumber.Clear();
            txtTableCapacity.Clear();
            cboTableStatus.SelectedIndex = 0;

            lblTableHint.Text = "✦  Nhấp vào bàn để chọn";
            lblTableHint.ForeColor = C_MUTED;

            btnSaveTable.FillColor = Color.FromArgb(210, 210, 218);
            btnSaveTable.ForeColor = C_MUTED;
            btnSaveTable.Enabled = false;

            btnDeleteTable.FillColor = Color.FromArgb(210, 210, 218);
            btnDeleteTable.ForeColor = C_MUTED;
            btnDeleteTable.Enabled = false;
        }

        // ===================== EVENTS — CATEGORIES =====================
        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvCategories.Rows[e.RowIndex];
            txtCategoryID.Text = row.Cells["CategoryID"].Value?.ToString() ?? "";
            txtCategoryName.Text = row.Cells["Tên danh mục"].Value?.ToString() ?? "";

            lblCatHint.Text = "✏  Đang chỉnh sửa: " + txtCategoryName.Text;
            lblCatHint.ForeColor = C_PURPLE;

            btnSaveCat.FillColor = C_BLUE;
            btnSaveCat.ForeColor = Color.White;
            btnSaveCat.Enabled = true;

            btnDeleteCat.FillColor = C_RED;
            btnDeleteCat.ForeColor = Color.White;
            btnDeleteCat.Enabled = true;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            { ShowWarn("Vui lòng nhập tên danh mục!"); return; }

            try
            {
                string q = $"INSERT INTO Category (CategoryName, IsActive) VALUES (N'{txtCategoryName.Text.Trim()}', 1)";
                DataHelper.ExecuteNonQuery(q);
                ShowInfo("✔  Thêm danh mục thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi thêm danh mục: " + ex.Message); }
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            { ShowWarn("Vui lòng chọn danh mục cần sửa!"); return; }

            try
            {
                string q = $"UPDATE Category SET CategoryName = N'{txtCategoryName.Text.Trim()}' WHERE CategoryID = {txtCategoryID.Text}";
                DataHelper.ExecuteNonQuery(q);
                ShowInfo("✔  Cập nhật thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi cập nhật: " + ex.Message); }
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            { ShowWarn("Vui lòng chọn danh mục cần xóa!"); return; }

            if (MessageBox.Show(
                    $"Ẩn danh mục \"{txtCategoryName.Text}\"?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                DataHelper.ExecuteNonQuery($"UPDATE Category SET IsActive = 0 WHERE CategoryID = {txtCategoryID.Text}");
                ShowInfo("✔  Xóa danh mục thành công!");
                ClearCatForm();
                LoadCategoryData();
            }
            catch (Exception ex) { ShowError("Lỗi xóa danh mục: " + ex.Message); }
        }

        private void ClearCatForm()
        {
            txtCategoryID.Text = "";
            txtCategoryName.Clear();

            lblCatHint.Text = "✦  Nhấp vào danh mục để chọn";
            lblCatHint.ForeColor = C_MUTED;

            btnSaveCat.FillColor = Color.FromArgb(210, 210, 218);
            btnSaveCat.ForeColor = C_MUTED;
            btnSaveCat.Enabled = false;

            btnDeleteCat.FillColor = Color.FromArgb(210, 210, 218);
            btnDeleteCat.ForeColor = C_MUTED;
            btnDeleteCat.Enabled = false;
        }

        // ===================== MESSAGE HELPERS =====================
        private void ShowInfo(string msg)
            => MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowWarn(string msg)
            => MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void ShowError(string msg)
            => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}