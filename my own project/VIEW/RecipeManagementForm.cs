using Guna.UI2.WinForms;
using my_own_project.DAL;

using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class RecipeManagementForm : Form
    {
        // ============================================================
        // DESIGN TOKENS
        // ============================================================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color C_BORDER = Color.FromArgb(232, 232, 240);

        // ============================================================
        // CONTROLS
        // ============================================================
        private ComboBox cboMenuItem;
        private ComboBox cboIngredient;

        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtSearchIngredient;

        private Guna2Button btnAdd;
        private Guna2Button btnUpdate;
        private Guna2Button btnDelete;
        private Guna2Button btnClear;
        private Guna2Button btnRefresh;

        private Guna2DataGridView dgvRecipe;
        private Guna2DataGridView dgvIngredients;

        private Label lblSelectedFood;
        private Label lblSummary;

        private int selectedRecipeID = 0;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================
        public RecipeManagementForm()
        {
            InitializeComponent();

            Controls.Clear();

            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;

            BuildUI();

            LoadMenuItems();
            LoadIngredients();

            LoadRecipeBySelectedMenu();
        }

        // ============================================================
        // UI
        // ============================================================
        private void BuildUI()
        {
            SuspendLayout();

            Panel header = BuildHeader();
            Panel main = BuildMainLayout();

            Controls.Add(main);
            Controls.Add(header);

            ResumeLayout(false);
        }

        // ============================================================
        // HEADER
        // ============================================================
        private Panel BuildHeader()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 66,
                BackColor = C_WHITE,
                Padding = new Padding(24, 0, 24, 0)
            };

            Label title = new Label
            {
                Text = "QUẢN LÝ CÔNG THỨC MÓN ĂN",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(24, 19)
            };

            btnRefresh = CreateButton("↻ Làm mới", C_PURPLE);

            btnRefresh.Size = new Size(118, 38);

            btnRefresh.Click += (s, e) =>
            {
                LoadMenuItems();
                LoadIngredients();
                LoadRecipeBySelectedMenu();
            };

            header.Controls.Add(title);
            header.Controls.Add(btnRefresh);

            header.Resize += (s, e) =>
            {
                btnRefresh.Location = new Point(header.Width - 145, 14);
            };

            return header;
        }

        // ============================================================
        // MAIN LAYOUT
        // ============================================================
        private Panel BuildMainLayout()
        {
            Panel wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG,
                Padding = new Padding(16)
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));

            layout.Controls.Add(BuildLeftCard(), 0, 0);
            layout.Controls.Add(BuildRightCard(), 1, 0);

            wrapper.Controls.Add(layout);

            return wrapper;
        }

        // ============================================================
        // CARD
        // ============================================================
        private Guna2Panel CreateCard()
        {
            return new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Margin = new Padding(6)
            };
        }

        private Guna2Button CreateButton(string text, Color color)
        {
            return new Guna2Button
            {
                Text = text,
                FillColor = color,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private Guna2DataGridView CreateGrid()
        {
            Guna2DataGridView grid = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = C_BORDER,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            return grid;
        }

        // ============================================================
        // LEFT CARD
        // ============================================================
        private Control BuildLeftCard()
        {
            Guna2Panel card = CreateCard();

            dgvRecipe = CreateGrid();

            card.Controls.Add(dgvRecipe);

            return card;
        }

        // ============================================================
        // RIGHT CARD
        // ============================================================
        private Control BuildRightCard()
        {
            Guna2Panel card = CreateCard();

            Panel formPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 250,
                BackColor = C_WHITE
            };

            Label lblIngredient = new Label
            {
                Text = "Nguyên liệu",
                Location = new Point(16, 20),
                AutoSize = true
            };

            cboIngredient = new ComboBox
            {
                Location = new Point(16, 45),
                Size = new Size(320, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            txtQuantity = new Guna2TextBox
            {
                Location = new Point(16, 90),
                Size = new Size(180, 38),
                PlaceholderText = "VD: 0.25"
            };

            btnAdd = CreateButton("+ Thêm", C_GREEN);
            btnUpdate = CreateButton("Cập nhật", C_PURPLE);
            btnDelete = CreateButton("Xóa", C_RED);
            btnClear = CreateButton("Làm trống", C_AMBER);

            btnAdd.Location = new Point(16, 150);
            btnUpdate.Location = new Point(120, 150);
            btnDelete.Location = new Point(240, 150);
            btnClear.Location = new Point(340, 150);

            formPanel.Controls.Add(lblIngredient);
            formPanel.Controls.Add(cboIngredient);
            formPanel.Controls.Add(txtQuantity);
            formPanel.Controls.Add(btnAdd);
            formPanel.Controls.Add(btnUpdate);
            formPanel.Controls.Add(btnDelete);
            formPanel.Controls.Add(btnClear);

            card.Controls.Add(formPanel);

            return card;
        }

        // ============================================================
        // DATA
        // ============================================================
        private void LoadMenuItems()
        {

        }

        private void LoadIngredients()
        {

        }

        private void LoadRecipeBySelectedMenu()
        {

        }
    }
}