using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewPromotionAddForm : Form
    {
        // ════════════════════════════════════════════════════════
        // DESIGN TOKENS
        // ════════════════════════════════════════════════════════
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_GREEN = Color.FromArgb(16, 185, 129);
        private static readonly Color C_TEXT = Color.FromArgb(31, 41, 55);
        private static readonly Color C_LABEL = Color.FromArgb(75, 85, 99);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);

        // ════════════════════════════════════════════════════════
        // CONTROLS & VARIABLES
        // ════════════════════════════════════════════════════════
        private Guna2TextBox txtPromoName, txtDiscount;
        private Guna2DateTimePicker dtpStartDate, dtpEndDate;
        private Guna2ComboBox cboStatus;
        private Guna2Button btnSave, btnCancel;
        private Label lblTitle;
        private Guna2DragControl dragControl;

        private int _promoID = -1; // -1: Thêm mới, Khác -1: Cập nhật

        // Constructor nhận ID (Mặc định là -1 nếu không truyền)
        public NewPromotionAddForm(int promoID = -1)
        {
            InitializeComponent();
            _promoID = promoID;

            this.Controls.Clear();
            this.Size = new Size(450, 600); // Kích thước form Popup
            this.StartPosition = FormStartPosition.CenterParent; // Hiện ở giữa form cha
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = C_WHITE;

            // Bo góc toàn bộ Form
            Guna2Elipse elipse = new Guna2Elipse { TargetControl = this, BorderRadius = 15 };

            BuildUI();

            if (_promoID != -1)
            {
                LoadDataForEdit();
                lblTitle.Text = "CẬP NHẬT KHUYẾN MÃI";
                btnSave.Text = "💾 LƯU THAY ĐỔI";
            }
        }

        // ════════════════════════════════════════════════════════
        // 1. UI BUILDER
        // ════════════════════════════════════════════════════════
        private void BuildUI()
        {
            this.SuspendLayout();

            // ── HEADER (Có thể nắm kéo form) ──
            var pnlHeader = new Guna2Panel { Dock = DockStyle.Top, Height = 60, FillColor = C_PURPLE };

            // ĐÃ SỬA LỖI TÀNG HÌNH: Thêm BackColor = Color.Transparent
            lblTitle = new Label
            {
                Text = "THÊM KHUYẾN MÃI MỚI",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = C_WHITE,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, 18)
            };
            pnlHeader.Controls.Add(lblTitle);

            dragControl = new Guna2DragControl { TargetControl = pnlHeader };

            // ── KHU VỰC NHẬP LIỆU ──
            var flpForm = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(25, 20, 25, 0) };

            var lName = MakeFieldLabel("Tên chương trình *");
            txtPromoName = MakeTextBox("VD: Lễ hội bia giảm giá...");

            var lType = MakeFieldLabel("Hình thức áp dụng");
            var cboApplyType = MakeComboBox(new object[] { "Giảm trên tổng hóa đơn" });
            cboApplyType.Enabled = false;

            var lDisc = MakeFieldLabel("Phần trăm giảm (%) *");
            txtDiscount = MakeTextBox("VD: 10, 20...");

            // Gộp ngày
            var pnlDates = new TableLayoutPanel { Width = 400, Height = 65, ColumnCount = 2, RowCount = 2, Margin = new Padding(0, 0, 0, 15) };
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

            flpForm.Controls.AddRange(new Control[] { lName, txtPromoName, lType, cboApplyType, lDisc, txtDiscount, pnlDates, lStatus, cboStatus });

            // ── KHU VỰC BUTTONS DƯỚI CÙNG ──
            var pnlBottom = new Guna2Panel { Dock = DockStyle.Bottom, Height = 80, CustomBorderThickness = new Padding(0, 1, 0, 0), CustomBorderColor = Color.FromArgb(235, 235, 235) };

            btnSave = MakeBtn("💾 TẠO MỚI", C_GREEN, C_WHITE);
            btnSave.Size = new Size(180, 45);
            btnSave.Location = new Point(245, 17);
            btnSave.Click += BtnSave_Click;

            btnCancel = MakeBtn("HỦY BỎ", Color.FromArgb(229, 231, 235), C_MUTED);
            btnCancel.Size = new Size(120, 45);
            btnCancel.Location = new Point(115, 17);
            btnCancel.Click += (s, e) => this.Close();

            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);

            // Ráp vào Form
            this.Controls.Add(flpForm);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlBottom);

            // Responsive Width
            this.Resize += (s, e) => {
                int w = this.Width - 50;
                txtPromoName.Width = w;
                cboApplyType.Width = w;
                txtDiscount.Width = w;
                pnlDates.Width = w;
                cboStatus.Width = w;
            };

            this.ResumeLayout(false);
        }

        // ════════════════════════════════════════════════════════
        // 2. LOGIC XỬ LÝ (THÊM / SỬA)
        // ════════════════════════════════════════════════════════
        private void LoadDataForEdit()
        {
            try
            {
                string query = $"SELECT * FROM Promotion WHERE PromotionID = {_promoID}";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    txtPromoName.Text = r["PromotionName"].ToString();
                    txtDiscount.Text = r["DiscountPercent"].ToString();
                    dtpStartDate.Value = Convert.ToDateTime(r["StartDate"]);
                    dtpEndDate.Value = Convert.ToDateTime(r["EndDate"]);
                    cboStatus.Text = r["Status"].ToString();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPromoName.Text) || string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên và % Giảm giá!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtDiscount.Text, out decimal discount))
            {
                MessageBox.Show("Phần trăm giảm phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string start = dtpStartDate.Value.ToString("yyyy-MM-dd");
                string end = dtpEndDate.Value.ToString("yyyy-MM-dd");
                string query = "";

                if (_promoID == -1) // THÊM MỚI
                {
                    query = $@"INSERT INTO Promotion (PromotionName, DiscountPercent, StartDate, EndDate, Status, ApplyType) 
                               VALUES (N'{txtPromoName.Text.Trim()}', {discount}, '{start}', '{end}', N'{cboStatus.Text}', 0)";
                }
                else // CẬP NHẬT
                {
                    query = $@"UPDATE Promotion 
                               SET PromotionName = N'{txtPromoName.Text.Trim()}', DiscountPercent = {discount}, 
                                   StartDate = '{start}', EndDate = '{end}', Status = N'{cboStatus.Text}' 
                               WHERE PromotionID = {_promoID}";
                }

                my_own_project.DAL.DataHelper.ExecuteNonQuery(query);

                // Trả về tín hiệu OK để form mẹ biết đường load lại bảng
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message); }
        }

        // ════════════════════════════════════════════════════════
        // 3. FACTORY HELPERS
        // ════════════════════════════════════════════════════════
        private Label MakeFieldLabel(string text) => new Label { Text = text, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = C_LABEL, Dock = DockStyle.Top, Height = 25, BackColor = Color.Transparent };
        private Guna2TextBox MakeTextBox(string placeholder) => new Guna2TextBox { PlaceholderText = placeholder, Dock = DockStyle.Top, Height = 42, BorderRadius = 6, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(249, 250, 251), Margin = new Padding(0, 0, 0, 15) };
        private Guna2ComboBox MakeComboBox(object[] items) { var cbo = new Guna2ComboBox { Dock = DockStyle.Top, Height = 42, BorderRadius = 6, Font = new Font("Segoe UI", 11F), FillColor = Color.FromArgb(249, 250, 251), Margin = new Padding(0, 0, 0, 15) }; cbo.Items.AddRange(items); cbo.SelectedIndex = 0; return cbo; }
        private Guna2DateTimePicker MakeDatePicker() => new Guna2DateTimePicker { Dock = DockStyle.Fill, BorderRadius = 6, Font = new Font("Segoe UI", 10F), FillColor = Color.FromArgb(249, 250, 251), Format = DateTimePickerFormat.Short };
        private Guna2Button MakeBtn(string text, Color fill, Color fore) => new Guna2Button { Text = text, BorderRadius = 6, FillColor = fill, ForeColor = fore, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
    }
}