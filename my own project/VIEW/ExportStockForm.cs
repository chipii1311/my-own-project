using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ExportStockForm : Form
    {
        private readonly int? _ingredientID;

        private Guna2ComboBox cboIngredient;
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtNote;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;

        // ── Palette (đồng bộ với ImportStockForm) ────────────────
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_BG = Color.FromArgb(248, 249, 254);
        private static readonly Color C_PURPLE = Color.FromArgb(99, 88, 255);
        private static readonly Color C_PURPLE_DARK = Color.FromArgb(78, 68, 220);
        private static readonly Color C_TEXT = Color.FromArgb(22, 22, 38);
        private static readonly Color C_MUTED = Color.FromArgb(130, 128, 158);
        private static readonly Color C_BORDER = Color.FromArgb(225, 224, 240);
        private static readonly Color C_CANCEL_BG = Color.FromArgb(241, 241, 248);
        private static readonly Color C_CANCEL_HVR = Color.FromArgb(230, 229, 245);
        private static readonly Color C_TAG_BG = Color.FromArgb(237, 235, 255);

        public ExportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;

            Text = "Xuất kho nguyên liệu";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(560, 680);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = C_BG;
            Font = new Font("Segoe UI", 9.5F);
            AutoScaleMode = AutoScaleMode.None;

            BuildUI();
            LoadIngredients();

            Shown += (s, e) =>
            {
                if (_ingredientID.HasValue)
                    cboIngredient.SelectedValue = _ingredientID.Value;
            };
        }

        private void BuildUI()
        {
            SuspendLayout();
            Controls.Clear();

            const int FORM_W = 560;
            const int HEADER_H = 108;
            const int BODY_H = 484;
            const int FOOTER_H = 88;
            const int MARGIN_X = 34;
            const int FIELD_W = FORM_W - (MARGIN_X * 2);

            // ── Header ────────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(FORM_W, HEADER_H),
                BackColor = C_PURPLE
            };

            var pnlIcon = new Panel
            {
                Size = new Size(52, 52),
                Location = new Point(MARGIN_X, 28),
                BackColor = Color.Transparent
            };
            pnlIcon.Paint += DrawHeaderIcon;

            var lblTitle = new Label
            {
                Text = "XUẤT KHO NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(104, 28),
                AutoSize = true
            };

            var lblSub = new Label
            {
                Text = "Ghi nhận phiếu xuất hàng ra khỏi kho",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(200, 255, 255, 255),
                Location = new Point(106, 62),
                AutoSize = true
            };

            pnlHeader.Controls.AddRange(new Control[] { pnlIcon, lblTitle, lblSub });

            // ── Body ──────────────────────────────────────────────────
            var pnlBody = new Panel
            {
                Location = new Point(0, HEADER_H),
                Size = new Size(FORM_W, BODY_H),
                BackColor = C_BG
            };

            AddLabel(pnlBody, "Nguyên liệu", MARGIN_X, 28);

            cboIngredient = new Guna2ComboBox
            {
                Location = new Point(MARGIN_X, 58),
                Size = new Size(FIELD_W, 42),
                BorderRadius = 11,
                BorderColor = C_BORDER,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                FillColor = C_WHITE,
                FocusedState = { BorderColor = C_PURPLE }
            };
            if (_ingredientID.HasValue) cboIngredient.Enabled = false;
            pnlBody.Controls.Add(cboIngredient);

            AddLabel(pnlBody, "Số lượng xuất", MARGIN_X, 140);
            txtQuantity = new Guna2TextBox
            {
                Location = new Point(MARGIN_X, 150),
                Size = new Size(FIELD_W, 54),
                BorderRadius = 11,
                BorderColor = C_BORDER,
                PlaceholderText = "Nhập số lượng cần xuất...",
                PlaceholderForeColor = C_MUTED,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                FillColor = C_WHITE,
                FocusedState = { BorderColor = C_PURPLE },
                HoverState = { BorderColor = Color.FromArgb(180, 160, 255) }
            };
            pnlBody.Controls.Add(txtQuantity);

            AddLabel(pnlBody, "Ghi chú", MARGIN_X, 257);
            txtNote = new Guna2TextBox
            {
                Location = new Point(MARGIN_X, 260),
                Size = new Size(FIELD_W, 64),
                BorderRadius = 11,
                BorderColor = C_BORDER,
                PlaceholderText = "VD: Dùng cho bếp, hủy hàng hỏng...",
                PlaceholderForeColor = C_MUTED,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                FillColor = C_WHITE,
                FocusedState = { BorderColor = C_PURPLE },
                HoverState = { BorderColor = Color.FromArgb(180, 160, 255) }
            };
            pnlBody.Controls.Add(txtNote);

            var pnlWarn = new Panel
            {
                Location = new Point(MARGIN_X, 370),
                Size = new Size(FIELD_W, 52),
                BackColor = Color.Transparent
            };
            pnlWarn.Paint += (s, e) => DrawRoundedPanel(
                e.Graphics, pnlWarn.ClientRectangle, 12, C_TAG_BG, C_BORDER);

            var lblWarn = new Label
            {
                Text = "⚠ Thao tác này sẽ làm giảm tồn kho và không thể hoàn tác.",
                Font = new Font("Segoe UI", 8.8F, FontStyle.Italic),
                ForeColor = C_PURPLE,
                Location = new Point(16, 16),
                AutoSize = true
            };
            pnlWarn.Controls.Add(lblWarn);
            pnlBody.Controls.Add(pnlWarn);

            // ── Footer ────────────────────────────────────────────────
            var pnlFooter = new Panel
            {
                Location = new Point(0, HEADER_H + BODY_H),
                Size = new Size(FORM_W, FOOTER_H),
                BackColor = C_WHITE
            };
            pnlFooter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER, 1), 0, 0, FORM_W, 0);

            btnCancel = new Guna2Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_CANCEL_BG,
                ForeColor = C_MUTED,
                BorderColor = C_BORDER,
                BorderThickness = 1,
                BorderRadius = 11,
                Size = new Size(126, 46),
                Location = new Point(258, 21),
                Cursor = Cursors.Hand
            };
            btnCancel.HoverState.FillColor = C_CANCEL_HVR;
            btnCancel.Click += (s, e) => Close();

            btnSave = new Guna2Button
            {
                Text = "Xác nhận xuất",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                BorderRadius = 11,
                Size = new Size(146, 46),
                Location = new Point(396, 21),
                Cursor = Cursors.Hand
            };
            btnSave.HoverState.FillColor = C_PURPLE_DARK;
            btnSave.Click += BtnSave_Click;

            pnlFooter.Controls.AddRange(new Control[] { btnCancel, btnSave });
            Controls.AddRange(new Control[] { pnlHeader, pnlBody, pnlFooter });
            ResumeLayout(false);
        }

        private void DrawHeaderIcon(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(Color.FromArgb(45, 255, 255, 255)))
                e.Graphics.FillEllipse(brush, 0, 0, 51, 51);

            using (var pen = new Pen(Color.White, 2f))
            {
                e.Graphics.DrawLine(pen, 26, 32, 26, 14);
                e.Graphics.DrawLine(pen, 18, 22, 26, 12);
                e.Graphics.DrawLine(pen, 34, 22, 26, 12);
                e.Graphics.DrawLine(pen, 14, 37, 14, 43);
                e.Graphics.DrawLine(pen, 38, 37, 38, 43);
                e.Graphics.DrawLine(pen, 14, 43, 38, 43);
            }
        }

        private void AddLabel(Panel parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                Location = new Point(x, y),
                AutoSize = true
            });
        }

        private static void DrawRoundedPanel(Graphics g, Rectangle rect, int radius, Color fillColor, Color borderColor)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            rect.Width -= 1;
            rect.Height -= 1;
            using (var path = RoundedRect(rect, radius))
            {
                using (var brush = new SolidBrush(fillColor)) g.FillPath(brush, path);
                using (var pen = new Pen(borderColor)) g.DrawPath(pen, path);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void LoadIngredients()
        {
            DataTable dt = IngredientBLL.GetAllIngredients();
            cboIngredient.DataSource = dt;
            cboIngredient.DisplayMember = "IngredientName";
            cboIngredient.ValueMember = "IngredientID";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboIngredient.SelectedItem == null)
                { MessageBox.Show("Vui lòng chọn nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                if (!float.TryParse(txtQuantity.Text.Trim(), out float qty) || qty <= 0)
                { MessageBox.Show("Số lượng xuất không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                // ── Bỏ kiểm tra nhân viên, truyền 0 (NULL) vào DB ──
                InventoryTransactionBLL.ExportIngredient(
                    Convert.ToInt32(cboIngredient.SelectedValue),
                    qty,
                    0,
                    txtNote.Text.Trim()
                );

                MessageBox.Show("Xuất kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}