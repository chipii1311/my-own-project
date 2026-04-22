namespace my_own_project.VIEW
{
    partial class frmInventory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Top Panel ──────────────────────────────────────────────
            this.pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblSubTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.picIcon = new System.Windows.Forms.PictureBox();

            // ── Stat Cards ─────────────────────────────────────────────
            this.pnlStatTotal = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatTotalVal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatTotalLbl = new Guna.UI2.WinForms.Guna2HtmlLabel();

            this.pnlStatLow = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatLowVal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatLowLbl = new Guna.UI2.WinForms.Guna2HtmlLabel();

            this.pnlStatOut = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatOutVal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatOutLbl = new Guna.UI2.WinForms.Guna2HtmlLabel();

            this.pnlStatTxn = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatTxnVal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblStatTxnLbl = new Guna.UI2.WinForms.Guna2HtmlLabel();

            // ── Tab Control ────────────────────────────────────────────
            this.tabMain = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabIngredient = new System.Windows.Forms.TabPage();
            this.tabTransaction = new System.Windows.Forms.TabPage();
            this.tabLowStock = new System.Windows.Forms.TabPage();

            // ── Tab 1: Ingredient ──────────────────────────────────────
            this.pnlIngredientTop = new Guna.UI2.WinForms.Guna2Panel();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.btnAdd = new Guna.UI2.WinForms.Guna2Button();
            this.btnEdit = new Guna.UI2.WinForms.Guna2Button();
            this.btnDelete = new Guna.UI2.WinForms.Guna2Button();
            this.btnImport = new Guna.UI2.WinForms.Guna2Button();
            this.btnExport = new Guna.UI2.WinForms.Guna2Button();
            this.dgvIngredient = new Guna.UI2.WinForms.Guna2DataGridView();

            // ── Tab 2: Transaction ─────────────────────────────────────
            this.pnlTxnFilter = new Guna.UI2.WinForms.Guna2Panel();
            this.cmbTxnType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpTxnFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtpTxnTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnTxnFilter = new Guna.UI2.WinForms.Guna2Button();
            this.btnTxnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.dgvTransaction = new Guna.UI2.WinForms.Guna2DataGridView();

            // ── Tab 3: Low Stock ───────────────────────────────────────
            this.pnlLowTop = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLowWarning = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnRefreshLow = new Guna.UI2.WinForms.Guna2Button();
            this.btnQuickImport = new Guna.UI2.WinForms.Guna2Button();
            this.dgvLowStock = new Guna.UI2.WinForms.Guna2DataGridView();

            // ── Bottom Status Bar ──────────────────────────────────────
            this.pnlBottom = new Guna.UI2.WinForms.Guna2Panel();
            this.lblStatus = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDateTime = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);

            // ── Guna2 Shadows ──────────────────────────────────────────
            this.shadowTop = new Guna.UI2.WinForms.Guna2ShadowForm();
            this.shadowCards = new Guna.UI2.WinForms.Guna2ShadowForm();

            ((System.ComponentModel.ISupportInitialize)(this.dgvIngredient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabIngredient.SuspendLayout();
            this.tabTransaction.SuspendLayout();
            this.tabLowStock.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════
            // FORM
            // ════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.ClientSize = new System.Drawing.Size(1200, 780);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInventory";
            this.Text = "Inventory Management";
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlStatTotal);
            this.Controls.Add(this.pnlStatLow);
            this.Controls.Add(this.pnlStatOut);
            this.Controls.Add(this.pnlStatTxn);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlBottom);

            // ════════════════════════════════════════════════════════════
            // TOP PANEL
            // ════════════════════════════════════════════════════════════
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(36, 34, 72);
            this.pnlTop.BorderRadius = 0;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Size = new System.Drawing.Size(1200, 80);
            this.pnlTop.Controls.Add(this.picIcon);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.lblSubTitle);

            this.picIcon.BackColor = System.Drawing.Color.Transparent;
            this.picIcon.Location = new System.Drawing.Point(24, 18);
            this.picIcon.Size = new System.Drawing.Size(44, 44);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18f, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(78, 12);
            this.lblTitle.Text = "Inventory Management";

            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(180, 180, 210);
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.lblSubTitle.Location = new System.Drawing.Point(80, 46);
            this.lblSubTitle.Text = "Quản lý nguyên liệu · Nhập/Xuất kho · Cảnh báo tồn kho";

            // ════════════════════════════════════════════════════════════
            // STAT CARDS
            // ════════════════════════════════════════════════════════════
            int cardY = 96; int cardW = 270; int cardH = 90; int cardGap = 18;

            // Card 1 — Tổng nguyên liệu
            this.pnlStatTotal.BackColor = System.Drawing.Color.White;
            this.pnlStatTotal.BorderRadius = 14;
            this.pnlStatTotal.Location = new System.Drawing.Point(20, cardY);
            this.pnlStatTotal.Size = new System.Drawing.Size(cardW, cardH);
            this.pnlStatTotal.Controls.Add(this.lblStatTotalVal);
            this.pnlStatTotal.Controls.Add(this.lblStatTotalLbl);
            this.shadowCards.SetShadow(this.pnlStatTotal, true);

            this.lblStatTotalVal.AutoSize = true;
            this.lblStatTotalVal.Font = new System.Drawing.Font("Segoe UI Semibold", 26f, System.Drawing.FontStyle.Bold);
            this.lblStatTotalVal.ForeColor = System.Drawing.Color.FromArgb(36, 34, 72);
            this.lblStatTotalVal.Location = new System.Drawing.Point(20, 14);
            this.lblStatTotalVal.Text = "0";

            this.lblStatTotalLbl.AutoSize = true;
            this.lblStatTotalLbl.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblStatTotalLbl.ForeColor = System.Drawing.Color.FromArgb(120, 120, 150);
            this.lblStatTotalLbl.Location = new System.Drawing.Point(20, 56);
            this.lblStatTotalLbl.Text = "🧂  Tổng nguyên liệu";

            // Card 2 — Sắp hết
            this.pnlStatLow.BackColor = System.Drawing.Color.White;
            this.pnlStatLow.BorderRadius = 14;
            this.pnlStatLow.Location = new System.Drawing.Point(20 + (cardW + cardGap), cardY);
            this.pnlStatLow.Size = new System.Drawing.Size(cardW, cardH);
            this.pnlStatLow.Controls.Add(this.lblStatLowVal);
            this.pnlStatLow.Controls.Add(this.lblStatLowLbl);
            this.shadowCards.SetShadow(this.pnlStatLow, true);

            this.lblStatLowVal.AutoSize = true;
            this.lblStatLowVal.Font = new System.Drawing.Font("Segoe UI Semibold", 26f, System.Drawing.FontStyle.Bold);
            this.lblStatLowVal.ForeColor = System.Drawing.Color.FromArgb(255, 152, 0);
            this.lblStatLowVal.Location = new System.Drawing.Point(20, 14);
            this.lblStatLowVal.Text = "0";

            this.lblStatLowLbl.AutoSize = true;
            this.lblStatLowLbl.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblStatLowLbl.ForeColor = System.Drawing.Color.FromArgb(120, 120, 150);
            this.lblStatLowLbl.Location = new System.Drawing.Point(20, 56);
            this.lblStatLowLbl.Text = "⚠️  Sắp hết hàng";

            // Card 3 — Hết hàng
            this.pnlStatOut.BackColor = System.Drawing.Color.White;
            this.pnlStatOut.BorderRadius = 14;
            this.pnlStatOut.Location = new System.Drawing.Point(20 + (cardW + cardGap) * 2, cardY);
            this.pnlStatOut.Size = new System.Drawing.Size(cardW, cardH);
            this.pnlStatOut.Controls.Add(this.lblStatOutVal);
            this.pnlStatOut.Controls.Add(this.lblStatOutLbl);
            this.shadowCards.SetShadow(this.pnlStatOut, true);

            this.lblStatOutVal.AutoSize = true;
            this.lblStatOutVal.Font = new System.Drawing.Font("Segoe UI Semibold", 26f, System.Drawing.FontStyle.Bold);
            this.lblStatOutVal.ForeColor = System.Drawing.Color.FromArgb(229, 57, 53);
            this.lblStatOutVal.Location = new System.Drawing.Point(20, 14);
            this.lblStatOutVal.Text = "0";

            this.lblStatOutLbl.AutoSize = true;
            this.lblStatOutLbl.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblStatOutLbl.ForeColor = System.Drawing.Color.FromArgb(120, 120, 150);
            this.lblStatOutLbl.Location = new System.Drawing.Point(20, 56);
            this.lblStatOutLbl.Text = "❌  Hết hàng";

            // Card 4 — Giao dịch hôm nay
            this.pnlStatTxn.BackColor = System.Drawing.Color.White;
            this.pnlStatTxn.BorderRadius = 14;
            this.pnlStatTxn.Location = new System.Drawing.Point(20 + (cardW + cardGap) * 3, cardY);
            this.pnlStatTxn.Size = new System.Drawing.Size(cardW, cardH);
            this.pnlStatTxn.Controls.Add(this.lblStatTxnVal);
            this.pnlStatTxn.Controls.Add(this.lblStatTxnLbl);
            this.shadowCards.SetShadow(this.pnlStatTxn, true);

            this.lblStatTxnVal.AutoSize = true;
            this.lblStatTxnVal.Font = new System.Drawing.Font("Segoe UI Semibold", 26f, System.Drawing.FontStyle.Bold);
            this.lblStatTxnVal.ForeColor = System.Drawing.Color.FromArgb(236, 64, 122);
            this.lblStatTxnVal.Location = new System.Drawing.Point(20, 14);
            this.lblStatTxnVal.Text = "0";

            this.lblStatTxnLbl.AutoSize = true;
            this.lblStatTxnLbl.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblStatTxnLbl.ForeColor = System.Drawing.Color.FromArgb(120, 120, 150);
            this.lblStatTxnLbl.Location = new System.Drawing.Point(20, 56);
            this.lblStatTxnLbl.Text = "📋  Giao dịch hôm nay";

            // ════════════════════════════════════════════════════════════
            // TAB CONTROL
            // ════════════════════════════════════════════════════════════
            this.tabMain.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(236, 64, 122);
            this.tabMain.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
            this.tabMain.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabMain.Font = new System.Drawing.Font("Segoe UI", 10f);
            this.tabMain.ItemSize = new System.Drawing.Size(160, 40);
            this.tabMain.Location = new System.Drawing.Point(20, cardY + cardH + 16);
            this.tabMain.Size = new System.Drawing.Size(1160, 570);
            this.tabMain.TabPages.Add(this.tabIngredient);
            this.tabMain.TabPages.Add(this.tabTransaction);
            this.tabMain.TabPages.Add(this.tabLowStock);

            // ── Tab 1 ──────────────────────────────────────────────────
            this.tabIngredient.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabIngredient.Text = "  Nguyên Liệu  ";
            this.tabIngredient.Controls.Add(this.pnlIngredientTop);
            this.tabIngredient.Controls.Add(this.dgvIngredient);

            // Toolbar
            this.pnlIngredientTop.BackColor = System.Drawing.Color.White;
            this.pnlIngredientTop.BorderRadius = 10;
            this.pnlIngredientTop.Location = new System.Drawing.Point(8, 8);
            this.pnlIngredientTop.Size = new System.Drawing.Size(1140, 60);
            this.pnlIngredientTop.Controls.Add(this.txtSearch);
            this.pnlIngredientTop.Controls.Add(this.btnSearch);
            this.pnlIngredientTop.Controls.Add(this.btnAdd);
            this.pnlIngredientTop.Controls.Add(this.btnEdit);
            this.pnlIngredientTop.Controls.Add(this.btnDelete);
            this.pnlIngredientTop.Controls.Add(this.btnImport);
            this.pnlIngredientTop.Controls.Add(this.btnExport);

            // Search box
            this.txtSearch.BorderRadius = 20;
            this.txtSearch.FillColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.txtSearch.BorderColor = System.Drawing.Color.FromArgb(220, 220, 235);
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.txtSearch.PlaceholderText = "🔍  Tìm nguyên liệu...";
            this.txtSearch.Location = new System.Drawing.Point(14, 12);
            this.txtSearch.Size = new System.Drawing.Size(260, 36);

            // Search btn
            this.btnSearch.BorderRadius = 20;
            this.btnSearch.FillColor = System.Drawing.Color.FromArgb(36, 34, 72);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Location = new System.Drawing.Point(282, 12);
            this.btnSearch.Size = new System.Drawing.Size(100, 36);

            // Add btn
            this.btnAdd.BorderRadius = 20;
            this.btnAdd.FillColor = System.Drawing.Color.FromArgb(236, 64, 122);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnAdd.Text = "+ Thêm mới";
            this.btnAdd.Location = new System.Drawing.Point(400, 12);
            this.btnAdd.Size = new System.Drawing.Size(120, 36);

            // Edit btn
            this.btnEdit.BorderRadius = 20;
            this.btnEdit.FillColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnEdit.Text = "✏ Sửa";
            this.btnEdit.Location = new System.Drawing.Point(530, 12);
            this.btnEdit.Size = new System.Drawing.Size(100, 36);

            // Delete btn
            this.btnDelete.BorderRadius = 20;
            this.btnDelete.FillColor = System.Drawing.Color.FromArgb(229, 57, 53);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnDelete.Text = "🗑 Xóa";
            this.btnDelete.Location = new System.Drawing.Point(640, 12);
            this.btnDelete.Size = new System.Drawing.Size(100, 36);

            // Import btn
            this.btnImport.BorderRadius = 20;
            this.btnImport.FillColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnImport.Text = "📦 Nhập kho";
            this.btnImport.Location = new System.Drawing.Point(754, 12);
            this.btnImport.Size = new System.Drawing.Size(120, 36);

            // Export btn
            this.btnExport.BorderRadius = 20;
            this.btnExport.FillColor = System.Drawing.Color.FromArgb(255, 152, 0);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnExport.Text = "📤 Xuất kho";
            this.btnExport.Location = new System.Drawing.Point(884, 12);
            this.btnExport.Size = new System.Drawing.Size(120, 36);

            // DataGridView Ingredient
            this.dgvIngredient.AllowUserToAddRows = false;
            this.dgvIngredient.AllowUserToDeleteRows = false;
            this.dgvIngredient.ReadOnly = true;
            this.dgvIngredient.ColumnHeadersHeight = 44;
            this.dgvIngredient.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvIngredient.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(249, 249, 253);
            this.dgvIngredient.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(36, 34, 72);
            this.dgvIngredient.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvIngredient.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
            this.dgvIngredient.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.dgvIngredient.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 70);
            this.dgvIngredient.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(236, 64, 122);
            this.dgvIngredient.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvIngredient.RowTemplate.Height = 42;
            this.dgvIngredient.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIngredient.MultiSelect = false;
            this.dgvIngredient.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIngredient.Location = new System.Drawing.Point(8, 76);
            this.dgvIngredient.Size = new System.Drawing.Size(1140, 450);

            // Columns
            var colID = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colID", HeaderText = "ID", DataPropertyName = "IngredientID", FillWeight = 40 };
            var colName = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colName", HeaderText = "Tên nguyên liệu", DataPropertyName = "IngredientName", FillWeight = 200 };
            var colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colUnit", HeaderText = "Đơn vị", DataPropertyName = "Unit", FillWeight = 70 };
            var colStock = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colStock", HeaderText = "Tồn kho", DataPropertyName = "StockQuantity", FillWeight = 90 };
            var colMin = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colMin", HeaderText = "Mức tối thiểu", DataPropertyName = "MinStock", FillWeight = 90 };
            var colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Trạng thái", DataPropertyName = "StockStatus", FillWeight = 100 };
            var colActive = new System.Windows.Forms.DataGridViewCheckBoxColumn { Name = "colActive", HeaderText = "Kích hoạt", DataPropertyName = "IsActive", FillWeight = 70 };
            this.dgvIngredient.Columns.AddRange(colID, colName, colUnit, colStock, colMin, colStatus, colActive);

            // ── Tab 2: Transaction ─────────────────────────────────────
            this.tabTransaction.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabTransaction.Text = "  Lịch Sử Giao Dịch  ";
            this.tabTransaction.Controls.Add(this.pnlTxnFilter);
            this.tabTransaction.Controls.Add(this.dgvTransaction);

            this.pnlTxnFilter.BackColor = System.Drawing.Color.White;
            this.pnlTxnFilter.BorderRadius = 10;
            this.pnlTxnFilter.Location = new System.Drawing.Point(8, 8);
            this.pnlTxnFilter.Size = new System.Drawing.Size(1140, 60);
            this.pnlTxnFilter.Controls.Add(this.cmbTxnType);
            this.pnlTxnFilter.Controls.Add(this.dtpTxnFrom);
            this.pnlTxnFilter.Controls.Add(this.dtpTxnTo);
            this.pnlTxnFilter.Controls.Add(this.btnTxnFilter);
            this.pnlTxnFilter.Controls.Add(this.btnTxnRefresh);

            this.cmbTxnType.BorderRadius = 20;
            this.cmbTxnType.FillColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.cmbTxnType.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.cmbTxnType.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.cmbTxnType.Location = new System.Drawing.Point(14, 12);
            this.cmbTxnType.Size = new System.Drawing.Size(150, 36);
            this.cmbTxnType.Items.AddRange(new object[] { "Tất cả", "Import", "Export", "Adjust" });
            this.cmbTxnType.SelectedIndex = 0;

            this.dtpTxnFrom.BorderRadius = 20;
            this.dtpTxnFrom.FillColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.dtpTxnFrom.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.dtpTxnFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTxnFrom.Location = new System.Drawing.Point(174, 12);
            this.dtpTxnFrom.Size = new System.Drawing.Size(140, 36);
            this.dtpTxnFrom.Value = System.DateTime.Now.AddDays(-30);

            this.dtpTxnTo.BorderRadius = 20;
            this.dtpTxnTo.FillColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.dtpTxnTo.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.dtpTxnTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTxnTo.Location = new System.Drawing.Point(324, 12);
            this.dtpTxnTo.Size = new System.Drawing.Size(140, 36);
            this.dtpTxnTo.Value = System.DateTime.Now;

            this.btnTxnFilter.BorderRadius = 20;
            this.btnTxnFilter.FillColor = System.Drawing.Color.FromArgb(36, 34, 72);
            this.btnTxnFilter.ForeColor = System.Drawing.Color.White;
            this.btnTxnFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnTxnFilter.Text = "🔍 Lọc";
            this.btnTxnFilter.Location = new System.Drawing.Point(474, 12);
            this.btnTxnFilter.Size = new System.Drawing.Size(100, 36);

            this.btnTxnRefresh.BorderRadius = 20;
            this.btnTxnRefresh.FillColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.btnTxnRefresh.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.btnTxnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnTxnRefresh.Text = "↺ Làm mới";
            this.btnTxnRefresh.Location = new System.Drawing.Point(584, 12);
            this.btnTxnRefresh.Size = new System.Drawing.Size(110, 36);

            this.dgvTransaction.AllowUserToAddRows = false;
            this.dgvTransaction.AllowUserToDeleteRows = false;
            this.dgvTransaction.ReadOnly = true;
            this.dgvTransaction.ColumnHeadersHeight = 44;
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(36, 34, 72);
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
            this.dgvTransaction.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.dgvTransaction.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(236, 64, 122);
            this.dgvTransaction.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTransaction.RowTemplate.Height = 42;
            this.dgvTransaction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransaction.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransaction.Location = new System.Drawing.Point(8, 76);
            this.dgvTransaction.Size = new System.Drawing.Size(1140, 450);

            var tColID = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColID", HeaderText = "ID", DataPropertyName = "TransactionID", FillWeight = 50 };
            var tColIng = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColIng", HeaderText = "Nguyên liệu", DataPropertyName = "IngredientName", FillWeight = 200 };
            var tColType = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColType", HeaderText = "Loại", DataPropertyName = "TransactionType", FillWeight = 80 };
            var tColQty = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColQty", HeaderText = "Số lượng", DataPropertyName = "QuantityChanged", FillWeight = 80 };
            var tColDate = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColDate", HeaderText = "Ngày giờ", DataPropertyName = "TransactionDate", FillWeight = 130 };
            var tColStaff = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColStaff", HeaderText = "Nhân viên", DataPropertyName = "StaffName", FillWeight = 120 };
            var tColNote = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "tColNote", HeaderText = "Ghi chú", DataPropertyName = "Note", FillWeight = 200 };
            this.dgvTransaction.Columns.AddRange(tColID, tColIng, tColType, tColQty, tColDate, tColStaff, tColNote);

            // ── Tab 3: Low Stock ───────────────────────────────────────
            this.tabLowStock.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabLowStock.Text = "  ⚠ Cảnh Báo Kho  ";
            this.tabLowStock.Controls.Add(this.pnlLowTop);
            this.tabLowStock.Controls.Add(this.dgvLowStock);

            this.pnlLowTop.BackColor = System.Drawing.Color.FromArgb(255, 243, 224);
            this.pnlLowTop.BorderRadius = 10;
            this.pnlLowTop.Location = new System.Drawing.Point(8, 8);
            this.pnlLowTop.Size = new System.Drawing.Size(1140, 60);
            this.pnlLowTop.Controls.Add(this.lblLowWarning);
            this.pnlLowTop.Controls.Add(this.btnRefreshLow);
            this.pnlLowTop.Controls.Add(this.btnQuickImport);

            this.lblLowWarning.AutoSize = true;
            this.lblLowWarning.BackColor = System.Drawing.Color.Transparent;
            this.lblLowWarning.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
            this.lblLowWarning.ForeColor = System.Drawing.Color.FromArgb(230, 81, 0);
            this.lblLowWarning.Location = new System.Drawing.Point(16, 18);
            this.lblLowWarning.Text = "⚠️  Danh sách nguyên liệu cần nhập thêm — tồn kho đang ở mức thấp";

            this.btnRefreshLow.BorderRadius = 20;
            this.btnRefreshLow.FillColor = System.Drawing.Color.FromArgb(255, 152, 0);
            this.btnRefreshLow.ForeColor = System.Drawing.Color.White;
            this.btnRefreshLow.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnRefreshLow.Text = "↺ Làm mới";
            this.btnRefreshLow.Location = new System.Drawing.Point(900, 12);
            this.btnRefreshLow.Size = new System.Drawing.Size(110, 36);

            this.btnQuickImport.BorderRadius = 20;
            this.btnQuickImport.FillColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.btnQuickImport.ForeColor = System.Drawing.Color.White;
            this.btnQuickImport.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
            this.btnQuickImport.Text = "📦 Nhập kho nhanh";
            this.btnQuickImport.Location = new System.Drawing.Point(1020, 12);
            this.btnQuickImport.Size = new System.Drawing.Size(110, 36);

            this.dgvLowStock.AllowUserToAddRows = false;
            this.dgvLowStock.AllowUserToDeleteRows = false;
            this.dgvLowStock.ReadOnly = true;
            this.dgvLowStock.ColumnHeadersHeight = 44;
            this.dgvLowStock.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 81, 0);
            this.dgvLowStock.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvLowStock.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
            this.dgvLowStock.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            this.dgvLowStock.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 152, 0);
            this.dgvLowStock.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvLowStock.RowTemplate.Height = 42;
            this.dgvLowStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLowStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLowStock.Location = new System.Drawing.Point(8, 76);
            this.dgvLowStock.Size = new System.Drawing.Size(1140, 450);

            var lColName = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "lColName", HeaderText = "Tên nguyên liệu", DataPropertyName = "IngredientName", FillWeight = 250 };
            var lColUnit = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "lColUnit", HeaderText = "Đơn vị", DataPropertyName = "Unit", FillWeight = 80 };
            var lColStock = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "lColStock", HeaderText = "Tồn kho hiện tại", DataPropertyName = "StockQuantity", FillWeight = 120 };
            var lColMin = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "lColMin", HeaderText = "Mức tối thiểu", DataPropertyName = "MinStock", FillWeight = 120 };
            var lColDiff = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "lColDiff", HeaderText = "Cần nhập thêm", DataPropertyName = "NeedToImport", FillWeight = 120 };
            var lColUrgent = new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "lColUrgent", HeaderText = "Mức độ", DataPropertyName = "UrgentLevel", FillWeight = 100 };
            this.dgvLowStock.Columns.AddRange(lColName, lColUnit, lColStock, lColMin, lColDiff, lColUrgent);

            // ════════════════════════════════════════════════════════════
            // BOTTOM STATUS BAR
            // ════════════════════════════════════════════════════════════
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.BorderRadius = 0;
            this.pnlBottom.Location = new System.Drawing.Point(0, 740);
            this.pnlBottom.Size = new System.Drawing.Size(1200, 40);
            this.pnlBottom.Controls.Add(this.lblStatus);
            this.pnlBottom.Controls.Add(this.lblDateTime);

            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(100, 100, 130);
            this.lblStatus.Location = new System.Drawing.Point(16, 10);
            this.lblStatus.Text = "✅  Sẵn sàng";

            this.lblDateTime.AutoSize = true;
            this.lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9f);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(100, 100, 130);
            this.lblDateTime.Location = new System.Drawing.Point(1050, 10);
            this.lblDateTime.Text = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // Timer
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            this.timerClock.Start();

            ((System.ComponentModel.ISupportInitialize)(this.dgvIngredient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabIngredient.ResumeLayout(false);
            this.tabTransaction.ResumeLayout(false);
            this.tabLowStock.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        // ── Controls Declaration ────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubTitle;
        private System.Windows.Forms.PictureBox picIcon;

        private Guna.UI2.WinForms.Guna2Panel pnlStatTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatTotalVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatTotalLbl;

        private Guna.UI2.WinForms.Guna2Panel pnlStatLow;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatLowVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatLowLbl;

        private Guna.UI2.WinForms.Guna2Panel pnlStatOut;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatOutVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatOutLbl;

        private Guna.UI2.WinForms.Guna2Panel pnlStatTxn;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatTxnVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatTxnLbl;

        private Guna.UI2.WinForms.Guna2TabControl tabMain;
        private System.Windows.Forms.TabPage tabIngredient;
        private System.Windows.Forms.TabPage tabTransaction;
        private System.Windows.Forms.TabPage tabLowStock;

        private Guna.UI2.WinForms.Guna2Panel pnlIngredientTop;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private Guna.UI2.WinForms.Guna2Button btnImport;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2DataGridView dgvIngredient;

        private Guna.UI2.WinForms.Guna2Panel pnlTxnFilter;
        private Guna.UI2.WinForms.Guna2ComboBox cmbTxnType;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTxnFrom;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTxnTo;
        private Guna.UI2.WinForms.Guna2Button btnTxnFilter;
        private Guna.UI2.WinForms.Guna2Button btnTxnRefresh;
        private Guna.UI2.WinForms.Guna2DataGridView dgvTransaction;

        private Guna.UI2.WinForms.Guna2Panel pnlLowTop;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblLowWarning;
        private Guna.UI2.WinForms.Guna2Button btnRefreshLow;
        private Guna.UI2.WinForms.Guna2Button btnQuickImport;
        private Guna.UI2.WinForms.Guna2DataGridView dgvLowStock;

        private Guna.UI2.WinForms.Guna2Panel pnlBottom;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblStatus;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDateTime;
        private System.Windows.Forms.Timer timerClock;

        private Guna.UI2.WinForms.Guna2ShadowForm shadowTop;
        private Guna.UI2.WinForms.Guna2ShadowForm shadowCards;
    }
}