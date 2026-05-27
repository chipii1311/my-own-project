using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class RecipeManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== COLORS =====================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_PURPLE_DARK = Color.FromArgb(72, 63, 210);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color C_TEXT = Color.FromArgb(28, 28, 44);
        private static readonly Color C_MUTED = Color.FromArgb(110, 110, 135);
        private static readonly Color C_BORDER = Color.FromArgb(226, 226, 238);
        private static readonly Color C_SIDEBAR = Color.FromArgb(250, 250, 255);

        // ===================== CONTROLS =====================
        private ComboBox cboMenuItem;
        private ComboBox cboIngredient;
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtSearchIngredient;
        private Guna2Button btnAdd, btnUpdate, btnDelete, btnClear, btnRefresh;
        private Guna2DataGridView dgvRecipe, dgvIngredients;
        private Label lblSelectedFood, lblRecipeCount;

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
            this.Text = "RecipeManagementForm";
        }

        #endregion

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.SuspendLayout();

            // Top header bar
            Panel header = BuildHeader();

            // Body: left card + right sidebar
            TableLayoutPanel body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = C_BG,
                Padding = new Padding(16, 10, 16, 16),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            body.Controls.Add(BuildLeftCard(), 0, 0);
            body.Controls.Add(BuildRightCard(), 1, 0);

            this.Controls.Add(body);
            this.Controls.Add(header);

            this.ResumeLayout(false);
        }

        // ── Header ─────────────────────────────────────────────────────────
        private Panel BuildHeader()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE,
                Padding = new Padding(24, 0, 24, 0)
            };

            header.Paint += (s, e) =>
            {
                using (Pen p = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, header.Height - 1, header.Width, header.Height - 1);
            };

            Label title = new Label
            {
                Text = "QUẢN LÝ CÔNG THỨC MÓN ĂN",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(58, 21)
            };

            btnRefresh = new Guna2Button
            {
                Text = "↻  Làm mới",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                BorderRadius = 10,
                Size = new Size(130, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += BtnRefresh_Click;

            header.Controls.AddRange(new Control[] { title, btnRefresh });
            header.Resize += (s, e) =>
                btnRefresh.Location = new Point(header.Width - 150, 13);

            return header;
        }

        // ── Left card: menu selector + recipe grid ──────────────────────────
        private Panel BuildLeftCard()
        {
            Panel outer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 8, 0),
                BackColor = C_BG
            };

            Guna2Panel card = CreateCard();

            // ── Selector row ───
            Panel selectorRow = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = C_WHITE,
                Padding = new Padding(16, 12, 16, 0)
            };

            Label lbl = new Label
            {
                Text = "Chọn món ăn:",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(16, 14)
            };

            cboMenuItem = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                BackColor = C_SIDEBAR,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(360, 32),
                Location = new Point(16, 30)
            };
            cboMenuItem.SelectedIndexChanged += CboMenuItem_SelectedIndexChanged;

            selectorRow.Controls.Add(lbl);
            selectorRow.Controls.Add(cboMenuItem);

            // ── Info bar ───
            Panel infoBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = C_PURPLE_SOFT,
                Padding = new Padding(16, 0, 16, 0)
            };
            infoBar.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(200, 190, 255), 1))
                {
                    e.Graphics.DrawLine(p, 0, 0, infoBar.Width, 0);
                    e.Graphics.DrawLine(p, 0, infoBar.Height - 1, infoBar.Width, infoBar.Height - 1);
                }
            };

            lblSelectedFood = new Label
            {
                Text = "Chưa chọn món",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = C_PURPLE_DARK,
                AutoSize = true,
                Location = new Point(16, 14)
            };

            lblRecipeCount = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            infoBar.Controls.Add(lblSelectedFood);
            infoBar.Controls.Add(lblRecipeCount);
            infoBar.Resize += (s, e) =>
                lblRecipeCount.Location = new Point(infoBar.Width - lblRecipeCount.Width - 16, 16);

            // ── Recipe DataGridView ───
            dgvRecipe = CreateGrid();
            dgvRecipe.CellClick += DgvRecipe_CellClick;
            dgvRecipe.CellDoubleClick += DgvRecipe_CellDoubleClick;
            dgvRecipe.DataBindingComplete += DgvRecipe_DataBindingComplete;

            card.Controls.Add(dgvRecipe);
            card.Controls.Add(infoBar);
            card.Controls.Add(selectorRow);

            outer.Controls.Add(card);
            return outer;
        }

        // ── Right card: form + ingredient list ─────────────────────────────
        private Panel BuildRightCard()
        {
            Panel outer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(8, 0, 0, 0),
                BackColor = C_BG
            };

            Guna2Panel card = CreateCard();

            // ── Row layout inside card ───
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = C_WHITE
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));   // form
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));    // ingredient header
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));    // grid

            layout.Controls.Add(BuildInputForm(), 0, 0);
            layout.Controls.Add(BuildIngredientHeader(), 0, 1);
            layout.Controls.Add(BuildIngredientGrid(), 0, 2);

            card.Controls.Add(layout);
            outer.Controls.Add(card);
            return outer;
        }

        // ── Input form inside right card ────────────────────────────────────
        private Panel BuildInputForm()
        {
            Panel form = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_SIDEBAR,
                Padding = new Padding(20, 18, 20, 10)
            };

            // Section title
            Label secTitle = new Label
            {
                Text = "Thêm / sửa nguyên liệu trong công thức",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(20, 14)
            };

            // Ingredient label + combo
            Label lblIng = new Label
            {
                Text = "Nguyên liệu",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            cboIngredient = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                BackColor = C_WHITE,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 68),
                Size = new Size(320, 32)
            };

            // Qty label + textbox
            Label lblQty = new Label
            {
                Text = "Định lượng / 1 món",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true,
                Location = new Point(20, 112)
            };

            txtQuantity = new Guna2TextBox
            {
                PlaceholderText = "VD: 0.25",
                Font = new Font("Segoe UI", 10F),
                FillColor = C_WHITE,
                BorderColor = C_BORDER,
                BorderRadius = 8,
                Location = new Point(20, 130),
                Size = new Size(180, 38)
            };
            txtQuantity.FocusedState.BorderColor = C_PURPLE;
            txtQuantity.HoverState.BorderColor = C_PURPLE;

            // Buttons
            btnAdd = MakeBtn("+ Thêm", C_GREEN, 90);
            btnUpdate = MakeBtn("↑ Cập nhật", C_PURPLE, 110);
            btnDelete = MakeBtn("✕ Xóa", C_RED, 80);
            btnClear = MakeBtn("↺ Làm trống", C_AMBER, 110);

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClear.Click += BtnClear_Click;

            form.Controls.AddRange(new Control[] { secTitle, lblIng, cboIngredient, lblQty, txtQuantity, btnAdd, btnUpdate, btnDelete, btnClear });

            form.Resize += (s, e) =>
            {
                int y = 178;
                int gap = 8;
                int x = 20;
                foreach (var b in new[] { btnAdd, btnUpdate, btnDelete, btnClear })
                {
                    b.Location = new Point(x, y);
                    x += b.Width + gap;
                }
            };

            form.Paint += (s, e) =>
            {
                using (Pen p = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, form.Height - 1, form.Width, form.Height - 1);
            };

            return form;
        }

        // ── Ingredient section header ────────────────────────────────────────
        private Panel BuildIngredientHeader()
        {
            Panel bar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Padding = new Padding(14, 10, 14, 0)
            };
            bar.Paint += (s, e) =>
            {
                using (Pen p = new Pen(C_BORDER, 1))
                    e.Graphics.DrawLine(p, 0, bar.Height - 1, bar.Width, bar.Height - 1);
            };

            Label lbl = new Label
            {
                Text = "Danh sách nguyên liệu",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = C_TEXT,
                AutoSize = true,
                Location = new Point(14, 12)
            };

            txtSearchIngredient = new Guna2TextBox
            {
                PlaceholderText = "Tìm nguyên liệu...",
                Font = new Font("Segoe UI", 9F),
                FillColor = C_SIDEBAR,
                BorderColor = C_BORDER,
                BorderRadius = 8,
                Size = new Size(180, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            txtSearchIngredient.FocusedState.BorderColor = C_PURPLE;
            txtSearchIngredient.TextChanged += TxtSearchIngredient_TextChanged;

            bar.Controls.Add(lbl);
            bar.Controls.Add(txtSearchIngredient);
            bar.Resize += (s, e) =>
                txtSearchIngredient.Location = new Point(bar.Width - 194, 8);

            return bar;
        }

        // ── Ingredient DataGridView ──────────────────────────────────────────
        private Panel BuildIngredientGrid()
        {
            Panel p = new Panel { Dock = DockStyle.Fill, BackColor = C_WHITE };

            dgvIngredients = CreateGrid();
            dgvIngredients.CellDoubleClick += DgvIngredients_CellDoubleClick;
            dgvIngredients.DataBindingComplete += DgvIngredients_DataBindingComplete;

            p.Controls.Add(dgvIngredients);
            return p;
        }

        // ── Guna2Panel card factory ─────────────────────────────────────────
        private Guna2Panel CreateCard()
        {
            return new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 14,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
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

        // ── Button factory ────────────────────────────────────────────────────
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
    }
}