using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewPromotionForm : Form
    {
        // ════════════════════════════════════════════════════════
        // DESIGN TOKENS (Màu sắc chuẩn phong cách Modern)
        // ════════════════════════════════════════════════════════
        private static readonly Color C_BG = Color.FromArgb(245, 246, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_LIGHT = Color.FromArgb(240, 235, 255);
        private static readonly Color C_GREEN = Color.FromArgb(16, 185, 129);
        private static readonly Color C_BLUE = Color.FromArgb(59, 130, 246);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_RED_LIGHT = Color.FromArgb(254, 226, 226);
        private static readonly Color C_TEXT = Color.FromArgb(31, 41, 55);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(229, 231, 235);
        private static readonly Color C_LABEL = Color.FromArgb(75, 85, 99);

        // ════════════════════════════════════════════════════════
        // CONTROLS
        // ════════════════════════════════════════════════════════
        private Guna2DataGridView dgvPromotions;
        private Guna2TextBox txtPromoID, txtPromoName, txtDiscount;
        private Guna2DateTimePicker dtpStartDate, dtpEndDate;
        private Guna2ComboBox cboStatus, cboApplyType;
        private Label lblHint;
        private Guna2Button btnAdd, btnUpdate, btnDelete;

        public NewPromotionForm()
        {
            InitializeComponent();
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            BuildModernUI();

            this.Load += (s, e) => LoadPromotionData();
        }

        // ════════════════════════════════════════════════════════
        // 1. UI BUILDER
        // ════════════════════════════════════════════════════════
        private void BuildModernUI()
        {
            this.SuspendLayout();

            // ── HEADER ─────────────────────────────────────────
            var pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 70, FillColor = C_WHITE, CustomBorderThickness = new Padding(0, 0, 0, 1), CustomBorderColor = C_BORDER };
            var lblTitle = new Label { Text = "🎁 QUẢN LÝ KHUYẾN MÃI", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = C_PURPLE, AutoSize = true, Location = new Point(24, 20) };
            pnlHeader.Controls.Add(lblTitle);

            // ── MAIN LAYOUT ────────────────────────
            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24, 24, 24, 24) };

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, BackColor = Color.Transparent };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlBody.Controls.Add(tlp);

            // ── LEFT: GRID CARD (BẢNG DANH SÁCH & NÚT THÊM MỚI) ──
            var cardLeft = new Guna2Panel { Dock = DockStyle.Fill, FillColor = C_WHITE, BorderRadius = 12, Margin = new Padding(0, 0, 15, 0), Padding = new Padding(5) };
            var cardHdrL = new Guna2Panel { Dock = DockStyle.Top, Height = 60, CustomBorderThickness = new Padding(0, 0, 0, 1), CustomBorderColor = C_BORDER };

            var lblGridTitle = new Label { Text = "Danh sách chương trình", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(15, 18) };

            // NÚT THÊM MỚI GỌI POPUP
            btnAdd = MakeBtn("➕ THÊM MỚI", C_GREEN, C_WHITE);
            btnAdd.Dock = DockStyle.None;
            btnAdd.Size = new Size(130, 38);
            btnAdd.Location = new Point(500, 11);
            btnAdd.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAdd.Click += BtnAdd_Click; // Gắn sự kiện gọi form Add

            cardHdrL.Controls.AddRange(new Control[] { lblGridTitle, btnAdd });
            cardHdrL.Resize += (s, e) => { btnAdd.Location = new Point(cardHdrL.Width - btnAdd.Width - 15, 11); };

            dgvPromotions = MakeModernGrid();
            dgvPromotions.CellClick += DgvPromotions_CellClick;

            cardLeft.Controls.Add(dgvPromotions);
            cardLeft.Controls.Add(cardHdrL);
            tlp.Controls.Add(cardLeft, 0, 0);

            // ── RIGHT: FORM CARD (CHỈ VIEW & UPDATE) ──────────────────────
            var cardRight = new Guna2Panel { Dock = DockStyle.Fill, FillColor = C_WHITE, BorderRadius = 12, Padding = new Padding(25, 25, 25, 25) };

            var pnlFormHeader = new Panel { Dock = DockStyle.Top, Height = 75 };
            var lblFormTitle = new Label { Text = "CHI TIẾT & CHỈNH SỬA", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = C_TEXT, AutoSize = true, Location = new Point(0, 0) };
            var sep = new Panel { Height = 3, BackColor = C_PURPLE, Width = 50, Location = new Point(0, 28) };
            lblHint = new Label { Text = "👆 Nhấp vào danh sách để xem/chỉnh sửa", Font = new Font("Segoe UI", 9.5F, FontStyle.Italic), ForeColor = C_MUTED, AutoSize = true, Location = new Point(0, 42) };

            pnlFormHeader.Controls.AddRange(new Control[] { lblHint, sep, lblFormTitle });

            // ── KHU VỰC NHẬP LIỆU ──
            var flpForm = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(0, 10, 0, 0) };
            cardRight.Controls.Add(flpForm);
            cardRight.Controls.Add(pnlFormHeader);

            txtPromoID = new Guna2TextBox { Visible = false };

            var lName = MakeFieldLabel("Tên chương trình *");
            txtPromoName = MakeTextBox("VD: Lễ hội bia giảm giá...");

            var lType = MakeFieldLabel("Hình thức áp dụng");
            cboApplyType = MakeComboBox(new object[] { "Giảm trên tổng hóa đơn" });
            cboApplyType.Enabled = false;

            var lDisc = MakeFieldLabel("Phần trăm giảm (%) *");
            txtDiscount = MakeTextBox("VD: 10, 20...");

            // ── GỘP 2 NGÀY VÀO 1 HÀNG ──
            var pnlDates = new TableLayoutPanel { Width = 350, Height = 65, ColumnCount = 2, RowCount = 2, Margin = new Padding(0, 0, 0, 15) };
            pnlDates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlDates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var lStart = MakeFieldLabel("Từ ngày");
            dtpStartDate = MakeDatePicker();
            dtpStartDate.Margin = new Padding(0, 0, 5, 0);

            var lEnd = MakeFieldLabel("Đến ngày");
            dtpEndDate = MakeDatePicker();
            dtpEndDate.Margin = new Padding(5, 0, 0, 0);

            pnlDates.Controls.Add(lStart, 0, 0);
            pnlDates.Controls.Add(lEnd, 1, 0);
            pnlDates.Controls.Add(dtpStartDate, 0, 1);
            pnlDates.Controls.Add(dtpEndDate, 1, 1);

            var lStatus = MakeFieldLabel("Trạng thái");
            cboStatus = MakeComboBox(new object[] { "Active", "Inactive" });

            // ── NÚT BẤM DƯỚI CÙNG (CHỈ CÓ LƯU VÀ XÓA) ─────────
            var tlpBtns = new TableLayoutPanel { Width = 350, Height = 45, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 10, 0, 0) };
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnUpdate = MakeBtn("💾 LƯU CHỈNH SỬA", C_BLUE, C_WHITE);
            btnUpdate.Margin = new Padding(0, 0, 5, 0);
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = MakeBtn("🛑 KẾT THÚC SỚM", C_RED_LIGHT, C_RED);
            btnDelete.Margin = new Padding(5, 0, 0, 0);
            btnDelete.Click += BtnDelete_Click;

            tlpBtns.Controls.Add(btnUpdate, 0, 0);
            tlpBtns.Controls.Add(btnDelete, 1, 0);

            flpForm.Controls.AddRange(new Control[] { txtPromoID, lName, txtPromoName, lType, cboApplyType, lDisc, txtDiscount, pnlDates, lStatus, cboStatus, tlpBtns });

            // Responsive
            cardRight.Resize += (s, e) =>
            {
                int w = flpForm.ClientSize.Width - 10;
                txtPromoName.Width = w;
                cboApplyType.Width = w;
                txtDiscount.Width = w;
                pnlDates.Width = w;
                cboStatus.Width = w;
                tlpBtns.Width = w;
            };

            tlp.Controls.Add(cardRight, 1, 0);
            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlHeader);
            this.ResumeLayout(false);

            ClearForm();
        }

        // ════════════════════════════════════════════════════════
        // 2. LOGIC DATABASE & EVENTS
        // ════════════════════════════════════════════════════════
        private void LoadPromotionData()
        {
            try
            {
                string query = @"SELECT 
                                    PromotionID AS [Mã], 
                                    PromotionName AS [Tên chương trình], 
                                    DiscountPercent AS [% Giảm], 
                                    FORMAT(StartDate, 'dd/MM/yyyy') AS [Từ ngày], 
                                    FORMAT(EndDate, 'dd/MM/yyyy') AS [Đến ngày], 
                                    Status AS [Trạng thái] 
                                 FROM Promotion ORDER BY PromotionID DESC";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                dgvPromotions.DataSource = dt;

                if (dgvPromotions.Columns.Contains("Mã")) dgvPromotions.Columns["Mã"].Visible = false;
                dgvPromotions.Columns["Tên chương trình"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvPromotions.Columns["% Giảm"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvPromotions.Columns["% Giảm"].Width = 80;
                dgvPromotions.Columns["% Giảm"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPromotions.Columns["Từ ngày"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvPromotions.Columns["Từ ngày"].Width = 100;
                dgvPromotions.Columns["Đến ngày"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvPromotions.Columns["Đến ngày"].Width = 100;
                dgvPromotions.Columns["Trạng thái"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvPromotions.Columns["Trạng thái"].Width = 90;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MỞ FORM THÊM MỚI (POPUP)
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Gọi form thêm mới, nếu thêm thành công (OK) thì load lại dữ liệu
            if (new NewPromotionAddForm(-1).ShowDialog() == DialogResult.OK)
            {
                LoadPromotionData();
            }
        }

        private void DgvPromotions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPromotions.Rows[e.RowIndex];

            txtPromoID.Text = row.Cells["Mã"].Value?.ToString();
            txtPromoName.Text = row.Cells["Tên chương trình"].Value?.ToString();
            txtDiscount.Text = row.Cells["% Giảm"].Value?.ToString();

            if (DateTime.TryParseExact(row.Cells["Từ ngày"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime start)) dtpStartDate.Value = start;
            if (DateTime.TryParseExact(row.Cells["Đến ngày"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime end)) dtpEndDate.Value = end;

            cboStatus.Text = row.Cells["Trạng thái"].Value?.ToString() ?? "Active";

            lblHint.Text = "✏️ Đang chỉnh sửa: " + txtPromoName.Text;
            lblHint.ForeColor = C_PURPLE;

            // Mở khóa các nút khi có dữ liệu được chọn
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        // CHỈ LÀM NHIỆM VỤ UPDATE (VÌ THÊM MỚI ĐÃ CÓ FORM RIÊNG)
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPromoID.Text)) return;

            if (string.IsNullOrWhiteSpace(txtPromoName.Text) || string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên chương trình và Phần trăm giảm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtDiscount.Text, out decimal discount))
            {
                MessageBox.Show("Phần trăm giảm phải là con số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string start = dtpStartDate.Value.ToString("yyyy-MM-dd");
                string end = dtpEndDate.Value.ToString("yyyy-MM-dd");

                string query = $@"UPDATE Promotion 
                                  SET PromotionName = N'{txtPromoName.Text.Trim()}', 
                                      DiscountPercent = {discount}, StartDate = '{start}', EndDate = '{end}', Status = N'{cboStatus.Text}' 
                                  WHERE PromotionID = {txtPromoID.Text}";
                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                LoadPromotionData();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPromoID.Text)) return;

            if (MessageBox.Show("Khóa (Kết thúc) mã khuyến mãi này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string query = $"UPDATE Promotion SET Status = 'Inactive' WHERE PromotionID = {txtPromoID.Text}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(query);
                    ClearForm();
                    LoadPromotionData();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        private void ClearForm()
        {
            txtPromoID.Text = "";
            txtPromoName.Text = "";
            txtDiscount.Text = "";
            dtpStartDate.Value = DateTime.Now;
            dtpEndDate.Value = DateTime.Now.AddMonths(1);
            cboStatus.SelectedIndex = 0;

            lblHint.Text = "👆 Nhấp vào danh sách để xem/chỉnh sửa";
            lblHint.ForeColor = C_MUTED;

            // Khóa nút khi chưa chọn dữ liệu
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            dgvPromotions.ClearSelection();
        }

        // ════════════════════════════════════════════════════════
        // 3. FACTORY HELPERS
        // ════════════════════════════════════════════════════════
        private Guna2DataGridView MakeModernGrid()
        {
            var dgv = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(240, 240, 240),
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Cursor = Cursors.Hand,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 50,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            dgv.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.ThemeStyle.HeaderStyle.ForeColor = C_MUTED;
            dgv.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ThemeStyle.RowsStyle.BackColor = C_WHITE;
            dgv.ThemeStyle.RowsStyle.ForeColor = C_TEXT;
            dgv.ThemeStyle.RowsStyle.SelectionBackColor = C_PURPLE_LIGHT;
            dgv.ThemeStyle.RowsStyle.SelectionForeColor = C_PURPLE;
            dgv.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(252, 252, 253);

            dgv.RowTemplate.Height = 50;
            dgv.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 0, 0);

            return dgv;
        }

        private Label MakeFieldLabel(string text) => new Label { Text = text, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = C_LABEL, Dock = DockStyle.Top, Height = 25, BackColor = Color.Transparent };
        private Guna2TextBox MakeTextBox(string placeholder) => new Guna2TextBox { PlaceholderText = placeholder, Dock = DockStyle.Top, Height = 42, BorderRadius = 6, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(249, 250, 251), Margin = new Padding(0, 0, 0, 20) };
        private Guna2ComboBox MakeComboBox(object[] items) { var cbo = new Guna2ComboBox { Dock = DockStyle.Top, Height = 42, BorderRadius = 6, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(249, 250, 251), Margin = new Padding(0, 0, 0, 20) }; cbo.Items.AddRange(items); cbo.SelectedIndex = 0; return cbo; }
        private Guna2DateTimePicker MakeDatePicker() => new Guna2DateTimePicker { Dock = DockStyle.Fill, BorderRadius = 6, Font = new Font("Segoe UI", 10F), FillColor = Color.FromArgb(249, 250, 251), Format = DateTimePickerFormat.Short };
        private Guna2Button MakeBtn(string text, Color fill, Color fore) => new Guna2Button { Text = text, Dock = DockStyle.Fill, BorderRadius = 6, FillColor = fill, ForeColor = fore, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
    }
}