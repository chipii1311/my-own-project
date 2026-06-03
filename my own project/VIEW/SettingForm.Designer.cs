using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class SettingForm
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
        protected DataGridView dgvTables;
        protected Guna2TextBox txtTableNumber, txtTableCapacity;
        protected Guna2ComboBox cboTableStatus;
        protected Label lblTableHint, lblTableCount;
        protected Guna2Button btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable;

        // ===================== CONTROLS — Categories =====================
        protected DataGridView dgvCategories;
        protected Guna2TextBox txtCategoryName;
        protected Label lblCatHint;
        protected Guna2Button btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat;

        // ===================== TAB STATE =====================
        protected Panel pageTable, pageCat;
        protected Guna2Button _tabTables, _tabCats;

        private System.ComponentModel.IContainer components = null;

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
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Name = "SettingForm";
            this.Text = "Cài đặt hệ thống";
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildUI();
        }

        #endregion

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
                Height = 80,
                BackColor = C_WHITE
            };
            h.Paint += PaintBottomBorder;

            Label title = new Label
            {
                Text = "⚙️ Cài đặt hệ thống",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(40, 28)
            };

            h.Controls.Add(title);
            return h;
        }

        // ── Tab bar ─────────────────────────────────────────────────────────
        private Panel BuildTabBar()
        {
            Panel bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = C_WHITE
            };
            bar.Paint += PaintBottomBorder;

            _tabTables = MakeTabBtn("🪑  Quản lý bàn ăn", 30);
            _tabCats = MakeTabBtn("📋  Danh mục món ăn", 280);

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
                Size = new Size(220, 56),
                Location = new Point(x, 0),
                BorderRadius = 0,
                BorderThickness = 0,
                FillColor = Color.Transparent,
                ForeColor = C_MUTED,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void SetTabActive(Guna2Button btn, bool active)
        {
            btn.ForeColor = active ? C_PURPLE : C_MUTED;
            btn.Paint -= TabBtn_Paint;
            if (active) btn.Paint += TabBtn_Paint;
            btn.Refresh();
        }

        private void TabBtn_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Guna2Button;
            if (btn == null) return;
            using (SolidBrush b = new SolidBrush(C_PURPLE))
                e.Graphics.FillRectangle(b, 0, btn.Height - 4, btn.Width, 4);
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
                Padding = new Padding(30, 20, 30, 30)
            };

            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ── Left card: table grid ──
            Panel leftCard = CreateCard(new Padding(0, 0, 12, 0));

            Panel cardHdr = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;

            Label lblTitle = new Label
            {
                Text = "📊 Danh sách bàn ăn",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(22, 18)
            };

            lblTableCount = new Label
            {
                Text = "0 bàn",
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            cardHdr.Controls.AddRange(new Control[] { lblTitle, lblTableCount });
            cardHdr.Resize += (s, e) =>
                lblTableCount.Location = new Point(cardHdr.Width - lblTableCount.Width - 22, 20);

            dgvTables = MakeGrid();
            dgvTables.CellClick += DgvTables_CellClick;
            dgvTables.CellFormatting += DgvTables_CellFormatting;

            leftCard.Controls.Add(dgvTables);
            leftCard.Controls.Add(cardHdr);

            // ── Right card: form ──
            Panel rightCard = CreateCard(new Padding(12, 0, 0, 0));
            rightCard.Padding = new Padding(28, 24, 28, 24);

            BuildTableForm(rightCard);

            tlp.Controls.Add(leftCard, 0, 0);
            tlp.Controls.Add(rightCard, 1, 0);
            page.Controls.Add(tlp);
            return page;
        }

        private void BuildTableForm(Panel card)
        {
            Label lblTitle = new Label
            {
                Text = "✏️ Thông tin bàn ăn",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Dock = DockStyle.Top,
                Height = 40
            };

            Panel sep = new Panel
            {
                Dock = DockStyle.Top,
                Height = 3,
                BackColor = C_PURPLE,
                Margin = new Padding(0, 0, 0, 8)
            };

            lblTableHint = new Label
            {
                Text = "✦️ Nhấp vào bàn ở danh sách để chỉnh sửa",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.Transparent
            };

            // Fields
            Label lNum = FieldLabel("Số bàn *");
            txtTableNumber = FieldTextBox("Nhập số bàn (VD: 1, 2, 3...)");

            Label lCap = FieldLabel("Sức chứa (người)");
            txtTableCapacity = FieldTextBox("VD: 4 người");

            Label lStt = FieldLabel("Trạng thái");
            cboTableStatus = new Guna2ComboBox
            {
                Dock = DockStyle.Top,
                Height = 44,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10F),
                FillColor = C_FIELD_BG,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 0, 20)
            };
            cboTableStatus.Items.AddRange(new object[] { "Trống", "Có khách", "Đặt trước" });
            cboTableStatus.SelectedIndex = 0;

            // Buttons
            Panel pnlBtns = new Panel { Dock = DockStyle.Top, Height = 110 };

            btnAddTable = BtnPrimary("➕ Thêm bàn mới", C_PURPLE);
            btnSaveTable = BtnPrimary("💾 Lưu thay đổi", C_BLUE);
            btnSaveTable.Enabled = false;

            btnDeleteTable = BtnPrimary("🗑️ Xóa bàn", C_RED);
            btnDeleteTable.Enabled = false;

            btnClearTable = BtnPrimary("✕ Hủy", Color.FromArgb(200, 200, 210));
            btnClearTable.ForeColor = C_MUTED;

            btnAddTable.Click += BtnAddTable_Click;
            btnSaveTable.Click += BtnSaveTable_Click;
            btnDeleteTable.Click += BtnDeleteTable_Click;
            btnClearTable.Click += (s, e) => ClearTableForm();

            pnlBtns.Controls.AddRange(new Control[] { btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable });
            pnlBtns.Resize += (s, e) => LayoutBtns2x2(pnlBtns, btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable);

            foreach (Control c in new Control[]
                { pnlBtns, cboTableStatus, lStt, txtTableCapacity, lCap,
                  txtTableNumber, lNum, lblTableHint, sep, lblTitle })
                card.Controls.Add(c);

            card.Resize += (s, e) =>
            {
                int w = card.ClientSize.Width - card.Padding.Horizontal;
                txtTableNumber.Width = w;
                txtTableCapacity.Width = w;
                cboTableStatus.Width = w;
                pnlBtns.Width = w;
                LayoutBtns2x2(pnlBtns, btnAddTable, btnSaveTable, btnDeleteTable, btnClearTable);
            };
        }

        // ===================== PAGE: DANH MỤC MÓN ĂN =====================
        private Panel BuildPageCategories()
        {
            Panel page = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG,
                Padding = new Padding(30, 20, 30, 30)
            };

            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ── Left card: category grid ──
            Panel leftCard = CreateCard(new Padding(0, 0, 12, 0));

            Panel cardHdr = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = C_WHITE };
            cardHdr.Paint += PaintBottomBorderLight;

            Label lblCatTitle = new Label
            {
                Text = "📊 Danh sách danh mục",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(22, 18)
            };
            cardHdr.Controls.Add(lblCatTitle);

            dgvCategories = MakeGrid();
            dgvCategories.CellClick += DgvCategories_CellClick;

            leftCard.Controls.Add(dgvCategories);
            leftCard.Controls.Add(cardHdr);

            // ── Right card: form ──
            Panel rightCard = CreateCard(new Padding(12, 0, 0, 0));
            rightCard.Padding = new Padding(28, 24, 28, 24);
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
                Text = "✏️ Thông tin danh mục",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Dock = DockStyle.Top,
                Height = 40
            };

            Panel sep = new Panel
            {
                Dock = DockStyle.Top,
                Height = 3,
                BackColor = C_PURPLE,
                Margin = new Padding(0, 0, 0, 8)
            };

            lblCatHint = new Label
            {
                Text = "✦️ Nhấp vào danh mục ở danh sách để chỉnh sửa",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.Transparent
            };

            Label lName = FieldLabel("Tên danh mục *");
            txtCategoryName = FieldTextBox("VD: Đồ uống, Món chính, Tráng miệng...");

            Panel pnlBtns = new Panel { Dock = DockStyle.Top, Height = 110 };

            btnAddCat = BtnPrimary("➕ Thêm danh mục", C_PURPLE);
            btnSaveCat = BtnPrimary("💾 Lưu thay đổi", C_BLUE);
            btnSaveCat.Enabled = false;

            btnDeleteCat = BtnPrimary("🗑️ Xóa danh mục", C_RED);
            btnDeleteCat.Enabled = false;

            btnClearCat = BtnPrimary("✕ Hủy", Color.FromArgb(200, 200, 210));
            btnClearCat.ForeColor = C_MUTED;

            btnAddCat.Click += BtnAddCategory_Click;
            btnSaveCat.Click += BtnEditCategory_Click;
            btnDeleteCat.Click += BtnDeleteCategory_Click;
            btnClearCat.Click += (s, e) => ClearCatForm();

            pnlBtns.Controls.AddRange(new Control[] { btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat });
            pnlBtns.Resize += (s, e) => LayoutBtns2x2(pnlBtns, btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat);

            foreach (Control c in new Control[]
                { pnlBtns, txtCategoryName, lName, lblCatHint, sep, lblTitle })
                card.Controls.Add(c);

            card.Resize += (s, e) =>
            {
                int w = card.ClientSize.Width - card.Padding.Horizontal;
                txtCategoryName.Width = w;
                pnlBtns.Width = w;
                LayoutBtns2x2(pnlBtns, btnAddCat, btnSaveCat, btnDeleteCat, btnClearCat);
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
            ApplyRoundCorners(card, 14);
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
                ColumnHeadersHeight = 50,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(16, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 11F);
            dgv.DefaultCellStyle.ForeColor = C_TEXT;
            dgv.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            dgv.DefaultCellStyle.SelectionForeColor = C_PURPLE;
            dgv.DefaultCellStyle.BackColor = C_WHITE;
            dgv.DefaultCellStyle.Padding = new Padding(16, 0, 0, 0);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 253);
            dgv.RowTemplate.Height = 50;

            return dgv;
        }

        private Label FieldLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = C_LABEL,
            Dock = DockStyle.Top,
            Height = 30,
            BackColor = Color.Transparent
        };

        private Guna2TextBox FieldTextBox(string placeholder) => new Guna2TextBox
        {
            PlaceholderText = placeholder,
            Dock = DockStyle.Top,
            Height = 44,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = C_FIELD_BG,
            BorderColor = C_BORDER,
            Margin = new Padding(0, 0, 0, 18)
        };

        private Guna2Button BtnPrimary(string text, Color fillColor) => new Guna2Button
        {
            Text = text,
            Height = 44,
            BorderRadius = 8,
            BorderThickness = 0,
            FillColor = fillColor,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };

        private static void LayoutBtns2x2(Panel pnl, Guna2Button b1, Guna2Button b2, Guna2Button b3, Guna2Button b4)
        {
            int gap = 10;
            int half = (pnl.Width - gap) / 2;
            if (half < 20) return;

            b1.Location = new Point(0, 0); b1.Size = new Size(half, 44);
            b2.Location = new Point(half + gap, 0); b2.Size = new Size(half, 44);
            b3.Location = new Point(0, 52); b3.Size = new Size(half, 44);
            b4.Location = new Point(half + gap, 52); b4.Size = new Size(half, 44);
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
    }
}
