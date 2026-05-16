using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class NewPromotionAddForm : Form
    {
        // ── Palette ──────────────────────────────────────────────
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_BG = Color.FromArgb(246, 247, 252);
        private static readonly Color C_PURPLE = Color.FromArgb(88, 28, 230);
        private static readonly Color C_PURPLE_S = Color.FromArgb(237, 233, 255);
        private static readonly Color C_GREEN = Color.FromArgb(16, 185, 129);
        private static readonly Color C_GREEN_D = Color.FromArgb(5, 150, 105);
        private static readonly Color C_TEXT = Color.FromArgb(31, 41, 55);
        private static readonly Color C_LABEL = Color.FromArgb(75, 85, 99);
        private static readonly Color C_MUTED = Color.FromArgb(107, 114, 128);
        private static readonly Color C_BORDER = Color.FromArgb(220, 218, 240);
        private static readonly Color C_FIELD = Color.FromArgb(249, 250, 251);

        // ── Controls ─────────────────────────────────────────────
        private Guna2TextBox txtPromoName, txtDiscount;
        private Guna2DateTimePicker dtpStart, dtpEnd;
        private Guna2ComboBox cboStatus, cboApplyType;
        private Guna2Button btnSave, btnCancel;
        private Label lblTitle;
        private Panel pnlItemPicker;   // hiện/ẩn theo ApplyType
        private CheckedListBox clbItems;        // multi-select món ăn
        private Guna2DragControl dragControl;
        private FlowLayoutPanel flpForm;

        private int _promoID = -1;

        public NewPromotionAddForm(int promoID = -1)
        {
            InitializeComponent();
            _promoID = promoID;
            Controls.Clear();

            Size = new Size(480, 660);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = C_WHITE;
            Font = new Font("Segoe UI", 9.5F);

            new Guna2Elipse { TargetControl = this, BorderRadius = 14 };

            BuildUI();
            LoadMenuItems();

            if (_promoID != -1)
            {
                LoadDataForEdit();
                lblTitle.Text = "CẬP NHẬT KHUYẾN MÃI";
                btnSave.Text = "  💾  LƯU THAY ĐỔI";
            }
        }

        // ─────────────────────────────────────────────────────────
        private void BuildUI()
        {
            SuspendLayout();

            // ── Header ───────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 66, BackColor = C_PURPLE };
            pnlHeader.Paint += (s, e) =>
            {
                using (var br = new LinearGradientBrush(pnlHeader.ClientRectangle,
                    Color.FromArgb(35, Color.White), Color.Transparent, 40f))
                    e.Graphics.FillRectangle(br, pnlHeader.ClientRectangle);
                // decorative circle
                using (var br2 = new SolidBrush(Color.FromArgb(16, Color.White)))
                    e.Graphics.FillEllipse(br2, Width - 90, -30, 140, 140);
            };

            var pnlIcon = new Panel { Size = new Size(38, 38), Location = new Point(20, 14), BackColor = Color.Transparent };
            pnlIcon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(50, Color.White)))
                    e.Graphics.FillEllipse(br, 0, 0, 37, 37);
                using (var p = new Pen(Color.White, 1.8f))
                {
                    e.Graphics.DrawRectangle(p, 8, 10, 16, 16);
                    e.Graphics.DrawLine(p, 8, 10, 18, 4); e.Graphics.DrawLine(p, 24, 10, 18, 4);
                    e.Graphics.DrawLine(p, 28, 18, 33, 18); e.Graphics.DrawLine(p, 30, 15, 33, 18); e.Graphics.DrawLine(p, 30, 21, 33, 18);
                }
            };

            lblTitle = new Label
            {
                Text = "THÊM KHUYẾN MÃI MỚI",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_WHITE,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(66, 14)
            };
            var lblSub = new Label
            {
                Text = "Tạo chương trình khuyến mãi cho nhà hàng",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(210, 255, 255, 255),
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(68, 40)
            };

            dragControl = new Guna2DragControl { TargetControl = pnlHeader };
            pnlHeader.Controls.AddRange(new Control[] { pnlIcon, lblTitle, lblSub });

            // ── Footer ───────────────────────────────────────────
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 72, BackColor = C_WHITE };
            pnlFooter.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(C_BORDER), 0, 0, Width, 0);

            btnCancel = new Guna2Button
            {
                Text = "Hủy bỏ",
                Size = new Size(110, 42),
                Location = new Point(230, 15),
                BorderRadius = 10,
                FillColor = Color.FromArgb(240, 239, 252),
                ForeColor = C_MUTED,
                BorderColor = C_BORDER,
                BorderThickness = 1,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.HoverState.FillColor = Color.FromArgb(228, 226, 248);
            btnCancel.Click += (s, e) => Close();

            btnSave = new Guna2Button
            {
                Text = "  💾  TẠO MỚI",
                Size = new Size(148, 42),
                Location = new Point(352, 15),
                BorderRadius = 10,
                FillColor = C_GREEN,
                ForeColor = C_WHITE,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.HoverState.FillColor = C_GREEN_D;
            btnSave.Click += BtnSave_Click;

            pnlFooter.Controls.AddRange(new Control[] { btnCancel, btnSave });

            // ── Body (scrollable) ────────────────────────────────
            flpForm = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(24, 16, 24, 8),
                BackColor = C_BG
            };

            int fw = 432; // field width = form width - padding*2

            // 1. Tên chương trình
            flpForm.Controls.Add(MakeLabel("Tên chương trình *"));
            txtPromoName = MakeTextBox("VD: Lễ hội bia, Tri ân khách hàng...", fw);
            flpForm.Controls.Add(txtPromoName);

            // 2. Hình thức áp dụng
            flpForm.Controls.Add(MakeLabel("Hình thức áp dụng *"));
            cboApplyType = MakeCombo(fw);
            cboApplyType.Items.AddRange(new object[] { "Giảm trên tổng hóa đơn", "Giảm theo món ăn" });
            cboApplyType.SelectedIndex = 0;
            cboApplyType.SelectedIndexChanged += CboApplyType_Changed;
            flpForm.Controls.Add(cboApplyType);

            // 3. Panel chọn món (ẩn mặc định)
            pnlItemPicker = new Panel
            {
                Width = fw,
                Height = 0,          // collapsed by default
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            var lblPicker = new Label
            {
                Text = "Chọn món áp dụng *",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = C_LABEL,
                Location = new Point(0, 0),
                AutoSize = true
            };

            clbItems = new CheckedListBox
            {
                Location = new Point(0, 22),
                Size = new Size(fw, 130),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F),
                BackColor = C_WHITE,
                ForeColor = C_TEXT,
                CheckOnClick = true,
                IntegralHeight = false
            };

            // Info label
            var lblHint = new Label
            {
                Text = "ℹ  Tích chọn một hoặc nhiều món ăn muốn áp dụng.",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Location = new Point(0, 158),
                Size = new Size(fw, 20)
            };

            pnlItemPicker.Controls.AddRange(new Control[] { lblPicker, clbItems, lblHint });
            flpForm.Controls.Add(pnlItemPicker);

            // 4. % Giảm
            flpForm.Controls.Add(MakeLabel("Phần trăm giảm (%) *"));
            txtDiscount = MakeTextBox("VD: 10, 20, 50...", fw);
            flpForm.Controls.Add(txtDiscount);

            // 5. Ngày bắt đầu / kết thúc (side by side)
            var pnlDates = new Panel { Width = fw, Height = 68, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 10) };
            int hw = (fw - 12) / 2;

            var lblS = new Label { Text = "Từ ngày *", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = C_LABEL, Location = new Point(0, 0), AutoSize = true };
            var lblE = new Label { Text = "Đến ngày *", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = C_LABEL, Location = new Point(hw + 12, 0), AutoSize = true };

            dtpStart = MakeDatePicker(hw);
            dtpStart.Location = new Point(0, 22);
            dtpEnd = MakeDatePicker(hw);
            dtpEnd.Location = new Point(hw + 12, 22);

            pnlDates.Controls.AddRange(new Control[] { lblS, lblE, dtpStart, dtpEnd });
            flpForm.Controls.Add(pnlDates);

            // 6. Trạng thái
            flpForm.Controls.Add(MakeLabel("Trạng thái"));
            cboStatus = MakeCombo(fw);
            cboStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            cboStatus.SelectedIndex = 0;
            flpForm.Controls.Add(cboStatus);

            // Assemble
            Controls.Add(flpForm);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);

            ResumeLayout(false);
        }

        // ── ApplyType toggle ─────────────────────────────────────
        private void CboApplyType_Changed(object sender, EventArgs e)
        {
            bool showPicker = cboApplyType.SelectedIndex == 1;
            pnlItemPicker.Height = showPicker ? 184 : 0;   // expand / collapse
            flpForm.PerformLayout();
        }

        // ── Load món ăn vào CheckedListBox ───────────────────────
        private void LoadMenuItems()
        {
            try
            {
                string query = @"
            SELECT 
                MenuItemID,
                ItemName
            FROM MenuItem
            ORDER BY ItemName";

                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);

                clbItems.DataSource = null;
                clbItems.Items.Clear();

                clbItems.DisplayMember = "ItemName";
                clbItems.ValueMember = "MenuItemID";
                clbItems.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ── Load dữ liệu khi sửa ────────────────────────────────
        private void LoadDataForEdit()
        {
            try
            {
                string query = $"SELECT * FROM Promotion WHERE PromotionID = {_promoID}";
                DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(query);
                if (dt.Rows.Count == 0) return;

                DataRow r = dt.Rows[0];
                txtPromoName.Text = r["PromotionName"].ToString();
                txtDiscount.Text = r["DiscountPercent"].ToString();
                dtpStart.Value = Convert.ToDateTime(r["StartDate"]);
                dtpEnd.Value = Convert.ToDateTime(r["EndDate"]);
                cboStatus.Text = r["Status"].ToString();
                int applyType = Convert.ToInt32(r["ApplyType"]);
                cboApplyType.SelectedIndex = applyType; // 0 hoặc 1

                // Nếu theo món → load các món đã chọn
                if (applyType == 1)
                {
                    string detailQ = $"SELECT MenuItemID FROM PromotionDetail WHERE PromotionID = {_promoID}";
                    DataTable dtd = my_own_project.DAL.DataHelper.ExecuteQuery(detailQ);
                    var selectedIDs = new HashSet<int>();
                    foreach (DataRow dr in dtd.Rows)
                        selectedIDs.Add(Convert.ToInt32(dr["MenuItemID"]));

                    for (int i = 0; i < clbItems.Items.Count; i++)
                    {
                        DataRowView drv = (DataRowView)clbItems.Items[i];
                        int id = Convert.ToInt32(drv["MenuItemID"]);
                        clbItems.SetItemChecked(i, selectedIDs.Contains(id));
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        // ── Save ─────────────────────────────────────────────────
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtPromoName.Text))
            { MessageBox.Show("Vui lòng nhập Tên chương trình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!decimal.TryParse(txtDiscount.Text.Trim(), out decimal discount) || discount <= 0 || discount > 100)
            { MessageBox.Show("Phần trăm giảm phải là số từ 1–100!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            if (dtpStart.Value >= dtpEnd.Value)
            { MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            int applyType = cboApplyType.SelectedIndex; // 0 hoặc 1

            // Validate chọn món nếu ApplyType = 1
            List<int> selectedMenuItemIDs = new List<int>();
            if (applyType == 1)
            {
                foreach (DataRowView drv in clbItems.CheckedItems)
                    selectedMenuItemIDs.Add(Convert.ToInt32(drv["MenuItemID"]));

                if (selectedMenuItemIDs.Count == 0)
                { MessageBox.Show("Vui lòng chọn ít nhất một món ăn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            }

            try
            {
                string start = dtpStart.Value.ToString("yyyy-MM-dd");
                string end = dtpEnd.Value.ToString("yyyy-MM-dd");
                string name = txtPromoName.Text.Trim().Replace("'", "''");
                string status = cboStatus.Text;
                int promoID = _promoID;

                if (_promoID == -1)
                {
                    // INSERT Promotion
                    string ins = $@"INSERT INTO Promotion (PromotionName, DiscountPercent, StartDate, EndDate, Status, ApplyType)
                                    VALUES (N'{name}', {discount}, '{start}', '{end}', N'{status}', {applyType});
                                    SELECT SCOPE_IDENTITY();";
                    DataTable dt = my_own_project.DAL.DataHelper.ExecuteQuery(ins);
                    promoID = Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                {
                    // UPDATE Promotion
                    string upd = $@"UPDATE Promotion SET
                                    PromotionName   = N'{name}',
                                    DiscountPercent = {discount},
                                    StartDate       = '{start}',
                                    EndDate         = '{end}',
                                    Status          = N'{status}',
                                    ApplyType       = {applyType}
                                    WHERE PromotionID = {_promoID}";
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(upd);

                    // Xóa PromotionDetail cũ trước khi ghi lại
                    my_own_project.DAL.DataHelper.ExecuteNonQuery(
                        $"DELETE FROM PromotionDetail WHERE PromotionID = {_promoID}");
                }

                // INSERT PromotionDetail nếu theo món
                if (applyType == 1)
                {
                    foreach (int mid in selectedMenuItemIDs)
                    {
                        my_own_project.DAL.DataHelper.ExecuteNonQuery(
                            $"INSERT INTO PromotionDetail (PromotionID, MenuItemID) VALUES ({promoID}, {mid})");
                    }
                }

                MessageBox.Show(_promoID == -1 ? "Tạo khuyến mãi thành công!" : "Cập nhật thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message); }
        }

        // ── Helpers ──────────────────────────────────────────────
        private Label MakeLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = C_LABEL,
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 4),
            BackColor = Color.Transparent
        };

        private Guna2TextBox MakeTextBox(string placeholder, int width) => new Guna2TextBox
        {
            Width = width,
            Height = 42,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10.5F),
            FillColor = C_FIELD,
            PlaceholderText = placeholder,
            ForeColor = C_TEXT,
            Margin = new Padding(0, 0, 0, 8),
            Padding = new Padding(6, 0, 0, 0)
        };

        private Guna2ComboBox MakeCombo(int width)
        {
            var cbo = new Guna2ComboBox
            {
                Width = width,
                Height = 42,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10.5F),
                FillColor = C_FIELD,
                ForeColor = C_TEXT,
                Margin = new Padding(0, 0, 0, 8)
            };
            return cbo;
        }

        private Guna2DateTimePicker MakeDatePicker(int width) => new Guna2DateTimePicker
        {
            Width = width,
            Height = 42,
            BorderRadius = 8,
            Font = new Font("Segoe UI", 10F),
            FillColor = C_FIELD,
            Format = DateTimePickerFormat.Short
        };
    }
}