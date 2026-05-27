using Guna.UI2.WinForms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class IngredientEditForm
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== CONTROLS =====================
        private Guna2TextBox txtName, txtUnit, txtStock, txtMinStock, txtPurchasePrice;
        private Guna2Button btnSave, btnCancel;

        // ===================== DESIGN TOKENS =====================
        private static readonly Color C_BG = Color.FromArgb(246, 247, 252);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(99, 88, 255);
        private static readonly Color C_PURPLE_DARK = Color.FromArgb(78, 68, 220);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(237, 235, 255);
        private static readonly Color C_TEXT = Color.FromArgb(22, 22, 38);
        private static readonly Color C_MUTED = Color.FromArgb(140, 136, 168);
        private static readonly Color C_BORDER = Color.FromArgb(220, 218, 240);
        private static readonly Color C_FIELD_BG = Color.FromArgb(252, 252, 255);
        private static readonly Color C_FOOTER = Color.FromArgb(250, 250, 254);

        // ── Fixed dimensions ─────────────────────────────────────
        private const int W = 520;
        private const int PX = 32;

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
            // Basic Form Setup
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(W, 458);
            this.Name = "IngredientEditForm";
            this.ResumeLayout(false);
        }
        #endregion

        // ===================== BUILD UI =====================
        private void BuildUI(bool isEdit)
        {
            this.Controls.Clear();
            this.Text = isEdit ? "Cập nhật nguyên liệu" : "Thêm nguyên liệu mới";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = C_BG;
            this.Font = new Font("Segoe UI", 9.5F);

            int inner = W - PX * 2;          // 456
            int half = (inner - 16) / 2;     // 220

            /* ── HEADER (Dock Top, h=90) ─────────────────────── */
            var header = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = C_PURPLE };
            header.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Shimmer gradient effect
                using (var br = new LinearGradientBrush(header.ClientRectangle, Color.FromArgb(35, Color.White), Color.Transparent, 40f))
                    g.FillRectangle(br, header.ClientRectangle);

                // Decorative right circle
                using (var br2 = new SolidBrush(Color.FromArgb(18, Color.White)))
                    g.FillEllipse(br2, W - 110, -40, 160, 160);
            };

            var badge = new Panel { Size = new Size(46, 46), Location = new Point(PX, 22), BackColor = Color.Transparent };
            badge.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(52, Color.White)))
                    g.FillEllipse(br, 0, 0, 45, 45);
                using (var p = new Pen(Color.White, 2f) { LineJoin = LineJoin.Round })
                {
                    g.DrawRectangle(p, 10, 16, 25, 19);
                    g.DrawLine(p, 10, 16, 22, 8);
                    g.DrawLine(p, 35, 16, 23, 8);
                    g.DrawLine(p, 10, 23, 35, 23);
                }
            };

            var lblTitle = new Label
            {
                Text = isEdit ? "CẬP NHẬT NGUYÊN LIỆU" : "THÊM NGUYÊN LIỆU MỚI",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(PX + 56, 18),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblSub = new Label
            {
                Text = isEdit ? "Chỉnh sửa thông tin nguyên liệu trong kho" : "Điền đầy đủ thông tin để thêm nguyên liệu mới",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(210, 255, 255, 255),
                Location = new Point(PX + 58, 46),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            header.Controls.AddRange(new Control[] { badge, lblTitle, lblSub });

            /* ── FOOTER (Dock Bottom, h=68) ──────────────────── */
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 68, BackColor = C_FOOTER };
            footer.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER), 0, 0, W, 0);

            btnCancel = new Guna2Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = Color.FromArgb(240, 239, 252),
                ForeColor = C_MUTED,
                BorderColor = C_BORDER,
                BorderThickness = 1,
                BorderRadius = 10,
                Size = new Size(112, 40),
                Location = new Point(W - PX - 112 - 10 - 148, 14),
                Cursor = Cursors.Hand
            };
            btnCancel.HoverState.FillColor = Color.FromArgb(228, 226, 248);
            btnCancel.Click += (s, e) => Close();

            btnSave = new Guna2Button
            {
                Text = isEdit ? "  ✓  Lưu thay đổi" : "  +  Thêm mới",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                BorderRadius = 10,
                Size = new Size(148, 40),
                Location = new Point(W - PX - 148, 14),
                Cursor = Cursors.Hand
            };
            btnSave.HoverState.FillColor = C_PURPLE_DARK;
            btnSave.Click += BtnSave_Click;

            footer.Controls.AddRange(new Control[] { btnCancel, btnSave });

            /* ── BODY (Form Content) ─────────────────────────── */
            var body = new Panel { Dock = DockStyle.Fill, BackColor = C_BG };

            // Gọi Helper dựng Layout TextBox
            txtName = MakeField(body, "Tên nguyên liệu", "Ví dụ: Thịt bò, Cà chua, Nước mắm...", PX, 24, inner);

            txtUnit = MakeField(body, "Đơn vị tính", "kg, lon, hộp, gói...", PX, 100, half);
            txtStock = MakeField(body, "Số lượng tồn", "0.00", PX + half + 16, 100, half);

            txtMinStock = MakeField(body, "Mức tồn tối thiểu", "0.00", PX, 176, half);
            txtPurchasePrice = MakeField(body, "Giá nhập (VNĐ)", "0", PX + half + 16, 176, half);

            // Đường phân cách
            body.Controls.Add(new Panel
            {
                Location = new Point(PX, 252),
                Size = new Size(inner, 1),
                BackColor = C_BORDER
            });

            body.Controls.Add(new Label
            {
                Text = "✦  Tất cả các trường đều bắt buộc nhập.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Location = new Point(PX, 276),
                AutoSize = true,
                BackColor = Color.Transparent
            });

            this.Controls.Add(body);
            this.Controls.Add(footer);
            this.Controls.Add(header);
        }

        // ── Field Helper ─────────────────────────────────────────
        private Guna2TextBox MakeField(Panel parent, string label, string placeholder, int x, int y, int w)
        {
            parent.Controls.Add(new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = C_MUTED,
                Location = new Point(x, y), // Đã sửa lại tọa độ y để Label không bị đè lên TextBox
                AutoSize = true,
                BackColor = Color.Transparent
            });

            var txt = new Guna2TextBox
            {
                Location = new Point(x, y + 20),
                Size = new Size(w, 44),
                BorderRadius = 10,
                BorderColor = C_BORDER,
                FillColor = C_FIELD_BG,
                PlaceholderText = placeholder,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                PlaceholderForeColor = C_MUTED,
                Padding = new Padding(8, 0, 0, 0)
            };

            txt.FocusedState.BorderColor = C_PURPLE;
            txt.FocusedState.FillColor = C_WHITE;
            txt.HoverState.BorderColor = Color.FromArgb(185, 165, 255);

            parent.Controls.Add(txt);
            return txt;
        }
    }
}