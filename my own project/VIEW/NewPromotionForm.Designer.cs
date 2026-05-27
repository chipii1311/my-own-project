using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class NewPromotionForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== DESIGN TOKENS =====================
        private static readonly Color C_BG = Color.FromArgb(245, 246, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE_LIGHT = Color.FromArgb(240, 235, 255);
        private static readonly Color C_GREEN = Color.FromArgb(16, 185, 129);
        private static readonly Color C_BLUE = Color.FromArgb(59, 130, 246);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_TEXT = Color.FromArgb(31, 41, 55);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(226, 232, 240);

        // ===================== CONTROLS =====================
        private Guna2TextBox txtSearch;
        private Guna2ComboBox cboFilterStatus;
        private Guna2Button btnAdd, btnEdit, btnDelete;
        private Guna2DataGridView dgvPromotions;

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
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "NewPromotionForm";
            this.Text = "Khuyến Mãi";
            this.ResumeLayout(false);
        }
        #endregion

        // ===================== BUILD UI =====================
        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(24);

            // ── 1. HEADER ──
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.Transparent };
            Label lblTitle = new Label
            {
                Text = "Quản lý Khuyến mãi",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Location = new Point(0, 10),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── 2. TOOLBAR (Tìm kiếm, Lọc, CRUD) ──
            Panel pnlToolbar = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.Transparent };

            txtSearch = new Guna2TextBox
            {
                PlaceholderText = "🔍 Tìm kiếm khuyến mãi...",
                Font = new Font("Segoe UI", 10F),
                Size = new Size(300, 42),
                Location = new Point(0, 10),
                BorderRadius = 8,
                BorderColor = C_BORDER
            };
            txtSearch.TextChanged += TxtSearch_TextChanged; // Map Event

            cboFilterStatus = new Guna2ComboBox
            {
                Size = new Size(160, 42),
                Location = new Point(315, 10),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                Font = new Font("Segoe UI", 10F)
            };
            cboFilterStatus.Items.AddRange(new object[] { "Tất cả", "Active", "Inactive" });
            cboFilterStatus.SelectedIndex = 0;
            cboFilterStatus.SelectedIndexChanged += CboFilterStatus_SelectedIndexChanged; // Map Event

            // Nút Xóa
            btnDelete = new Guna2Button
            {
                Text = "✕ Xóa",
                Size = new Size(100, 42),
                BorderRadius = 8,
                FillColor = C_RED,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDelete.Click += BtnDelete_Click;

            // Nút Sửa
            btnEdit = new Guna2Button
            {
                Text = "✎ Sửa",
                Size = new Size(100, 42),
                BorderRadius = 8,
                FillColor = C_BLUE,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnEdit.Click += BtnEdit_Click;

            // Nút Thêm
            btnAdd = new Guna2Button
            {
                Text = "+ Thêm mới",
                Size = new Size(130, 42),
                BorderRadius = 8,
                FillColor = C_GREEN,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAdd.Click += BtnAdd_Click;

            pnlToolbar.Controls.AddRange(new Control[] { txtSearch, cboFilterStatus, btnDelete, btnEdit, btnAdd });

            // Xử lý Resize tự động neo các nút sang phải
            pnlToolbar.Resize += (s, e) =>
            {
                btnAdd.Left = pnlToolbar.Width - btnAdd.Width;
                btnEdit.Left = btnAdd.Left - btnEdit.Width - 10;
                btnDelete.Left = btnEdit.Left - btnDelete.Width - 10;
            };

            // ── 3. DATA GRID VIEW (Khung lưới hiển thị) ──
            Panel pnlGridWrap = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE,
                Padding = new Padding(1)
            };
            // Bo viền mảnh 1px cho Panel bọc ngoài Grid
            pnlGridWrap.Paint += (s, e) => { e.Graphics.DrawRectangle(new Pen(C_BORDER, 1), 0, 0, pnlGridWrap.Width - 1, pnlGridWrap.Height - 1); };

            dgvPromotions = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = C_BORDER,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Cursor = Cursors.Hand
            };

            // Tùy chỉnh CSS Header
            dgvPromotions.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvPromotions.ThemeStyle.HeaderStyle.ForeColor = C_MUTED;
            dgvPromotions.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvPromotions.ThemeStyle.HeaderStyle.Height = 45;

            // Tùy chỉnh CSS Cells
            dgvPromotions.DefaultCellStyle.BackColor = C_WHITE;
            dgvPromotions.DefaultCellStyle.ForeColor = C_TEXT;
            dgvPromotions.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dgvPromotions.DefaultCellStyle.SelectionBackColor = C_PURPLE_LIGHT;
            dgvPromotions.DefaultCellStyle.SelectionForeColor = C_TEXT;
            dgvPromotions.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvPromotions.RowTemplate.Height = 45;

            // Map các sự kiện
            dgvPromotions.CellClick += DgvPromotions_CellClick;
            dgvPromotions.CellFormatting += DgvPromotions_CellFormatting;

            pnlGridWrap.Controls.Add(dgvPromotions);

            // ── RÁP CÁC KHỐI LÊN FORM CHÍNH ──
            this.Controls.Add(pnlGridWrap);
            this.Controls.Add(pnlToolbar);
            this.Controls.Add(pnlHeader);
        }
    }
}