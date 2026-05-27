using Guna.UI2.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class InventoryForm
    {
        private System.ComponentModel.IContainer components = null;

        // ==================== COLORS (DESIGN TOKENS) ====================
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

        // ==================== CONTROLS ====================
        private Guna2TextBox txtSearch;
        private Guna2Button btnAdd, btnEdit, btnDelete, btnImport, btnExport, btnRefresh;
        private Guna2DataGridView dgvInventory, dgvTransactions;
        private Label lblStatTotal, lblStatOk, lblStatLow, lblStatOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Name = "InventoryForm";
            this.Text = "InventoryForm";
        }
        #endregion

        // ==================== BUILD UI ====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.SuspendLayout();

            Panel header = BuildHeader();
            Panel subbar = BuildSubBar();

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

            this.Controls.Add(split);
            this.Controls.Add(subbar);
            this.Controls.Add(header);

            this.ResumeLayout(false);
        }

        // ── Header ──────────────────────────────────────────────────────────
        private Panel BuildHeader()
        {
            Panel header = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = C_WHITE, Padding = new Padding(24, 0, 24, 0) };
            header.Paint += (s, e) => { using (Pen p = new Pen(C_BORDER, 1)) e.Graphics.DrawLine(p, 0, header.Height - 1, header.Width, header.Height - 1); };

            Label title = new Label { Text = "QUẢN LÝ KHO NGUYÊN LIỆU", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = C_PURPLE, AutoSize = true, Location = new Point(24, 22) };

            btnAdd = MakeBtn("+ Thêm", C_GREEN, 90);
            btnAdd.Click += BtnAdd_Click;
            btnEdit = MakeBtn("✎ Sửa", C_PURPLE, 84);
            btnEdit.Click += BtnEdit_Click;
            btnDelete = MakeBtn("✕ Xóa", C_RED, 84);
            btnDelete.Click += BtnDelete_Click;
            btnImport = MakeBtn("↓ Nhập kho", Color.FromArgb(24, 95, 165), 110);
            btnImport.Click += BtnImport_Click;
            btnExport = MakeBtn("↑ Xuất kho", Color.FromArgb(186, 117, 23), 110);
            btnExport.Click += BtnExport_Click;
            btnRefresh = MakeBtn("↻", Color.FromArgb(90, 90, 110), 42);
            // btnRefresh.Click mapped in .cs LoadData

            header.Controls.AddRange(new Control[] { title, btnAdd, btnEdit, btnDelete, btnImport, btnExport, btnRefresh });
            header.Resize += (s, e) => LayoutHeaderButtons(header);
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
            Panel bar = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = C_BG, Padding = new Padding(16, 10, 16, 10) };

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
            // txtSearch.TextChanged mapped in .cs ApplyFilter

            Panel[] cards = new Panel[4];
            string[] captions = { "Tổng NL", "Ổn định", "Sắp hết", "Hết hàng" };
            Color[] valColors = { C_PURPLE, C_GREEN_TEXT, C_AMBER_TEXT, C_RED_TEXT };
            Color[] bgColors = { C_PURPLE_SOFT, C_GREEN_BG, C_AMBER_BG, C_RED_BG };
            Label[] valLabels = new Label[4];

            for (int i = 0; i < 4; i++)
            {
                Panel card = new Panel { BackColor = bgColors[i], BorderStyle = BorderStyle.None, Size = new Size(110, 54) };
                RoundPanel(card, 10);

                Label cap = new Label { Text = captions[i], Font = new Font("Segoe UI", 9F), ForeColor = valColors[i], AutoSize = false, Size = new Size(106, 18), Location = new Point(8, 6), TextAlign = ContentAlignment.MiddleLeft };
                Label val = new Label { Text = "—", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = valColors[i], AutoSize = false, Size = new Size(106, 26), Location = new Point(8, 24), TextAlign = ContentAlignment.MiddleLeft };

                card.Controls.Add(cap); card.Controls.Add(val);
                cards[i] = card; valLabels[i] = val;
            }

            lblStatTotal = valLabels[0]; lblStatOk = valLabels[1]; lblStatLow = valLabels[2]; lblStatOut = valLabels[3];

            bar.Controls.Add(txtSearch);
            bar.Resize += (s, e) =>
            {
                int right = bar.Width - 16, y = 14, gap = 10, w = 110;
                for (int i = 3; i >= 0; i--)
                {
                    cards[i].Location = new Point(right - w, y);
                    cards[i].Size = new Size(w, 54);
                    right -= w + gap;
                    if (!bar.Controls.Contains(cards[i])) bar.Controls.Add(cards[i]);
                }
            };
            return bar;
        }

        // ── Card wrapper ───────────────────────────────────────────────────
        private Panel BuildCard(string titleText, Control content)
        {
            Panel outer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 8, 12, 8), BackColor = C_BG };
            Guna2Panel card = new Guna2Panel { Dock = DockStyle.Fill, FillColor = C_WHITE, BorderRadius = 12, Padding = new Padding(0) };

            Panel titleBar = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = C_WHITE };
            titleBar.Paint += (s, e) => { using (Pen p = new Pen(C_BORDER, 1)) e.Graphics.DrawLine(p, 0, titleBar.Height - 1, titleBar.Width, titleBar.Height - 1); };

            Label lbl = new Label { Text = titleText, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(14, 12) };
            titleBar.Controls.Add(lbl);

            card.Controls.Add(content);
            card.Controls.Add(titleBar);
            content.BringToFront();
            outer.Controls.Add(card);
            return outer;
        }

        // ── Grid factory ───────────────────────────────────────────────────
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

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersHeight = 38;
            grid.EnableHeadersVisualStyles = false;

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

        // ── Helper UI ──────────────────────────────────────────────────────
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