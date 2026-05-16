using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class IngredientEditForm : Form
    {
        private readonly int? _ingredientID;

        private Guna2TextBox txtName, txtUnit, txtStock, txtMinStock, txtPurchasePrice;
        private Guna2Button btnSave, btnCancel;

        // ── Palette ──────────────────────────────────────────────
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
        // Header : 90
        // Body   : 20 + 69 + 69 + 69 + 1(div) + 12 + 20(note) + 20(pad) = 300  → use 300
        // Footer : 68
        // Total  : 458
        private const int W = 520;
        private const int PX = 32;    // horizontal padding

        public IngredientEditForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;
            InitializeComponent();
            Controls.Clear();

            bool edit = ingredientID.HasValue;
            Text = edit ? "Cập nhật nguyên liệu" : "Thêm nguyên liệu mới";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = C_BG;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(W, 458);
            Font = new Font("Segoe UI", 9.5F);

            BuildUI(edit);
            if (edit) LoadIngredient(ingredientID.Value);
        }

        // ─────────────────────────────────────────────────────────
        private void BuildUI(bool isEdit)
        {
            int inner = W - PX * 2;          // 456
            int half = (inner - 16) / 2;    // 220

            /* ── HEADER (Dock Top, h=90) ─────────────────────── */
            var header = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = C_PURPLE };
            header.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // shimmer
                using (var br = new LinearGradientBrush(header.ClientRectangle,
                    Color.FromArgb(35, Color.White), Color.Transparent, 40f))
                    g.FillRectangle(br, header.ClientRectangle);
                // decorative right circle
                using (var br2 = new SolidBrush(Color.FromArgb(18, Color.White)))
                    g.FillEllipse(br2, W - 110, -40, 160, 160);
            };

            // Icon badge
            var badge = new Panel { Size = new Size(46, 46), Location = new Point(PX, 22), BackColor = Color.Transparent };
            badge.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new SolidBrush(Color.FromArgb(52, Color.White)))
                    g.FillEllipse(br, 0, 0, 45, 45);
                using (var p = new Pen(Color.White, 2f) { LineJoin = LineJoin.Round })
                {
                    g.DrawRectangle(p, 10, 16, 25, 19);  // box body
                    g.DrawLine(p, 10, 16, 22, 8);       // lid left
                    g.DrawLine(p, 35, 16, 23, 8);       // lid right
                    g.DrawLine(p, 10, 23, 35, 23);       // shelf
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
                Text = isEdit ? "Chỉnh sửa thông tin nguyên liệu trong kho"
                                   : "Điền đầy đủ thông tin để thêm nguyên liệu mới",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(210, 255, 255, 255),
                Location = new Point(PX + 58, 46),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            header.Controls.AddRange(new Control[] { badge, lblTitle, lblSub });

            /* ── FOOTER (Dock Bottom, h=68) ──────────────────── */
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 68, BackColor = C_FOOTER };
            footer.Paint += (s, e) =>
                e.Graphics.DrawLine(new Pen(C_BORDER), 0, 0, W, 0);

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

            /* ── BODY (fills between header & footer) ────────── */
            // Use absolute Panel so positions are exact
            var body = new Panel { Dock = DockStyle.Fill, BackColor = C_BG };

            //  Row positions (relative to body):
            //  y=20  → label, y+20 → field(h=44) → row bottom = 84
            //  y=94  → label, y+20 → field        → row bottom = 158
            //  y=168 → label, y+20 → field        → row bottom = 232
            //  y=244 → divider(h=1)
            //  y=258 → note label

            // Row 1 – full width  (label y=24, field y=44, bottom=88)
            txtName = MakeField(body, "Tên nguyên liệu", "Ví dụ: Thịt bò, Cà chua, Nước mắm...",
                                PX, 24, inner);

            // Row 2 – half+half  (label y=100, field y=120, bottom=164)
            txtUnit = MakeField(body, "Đơn vị tính", "kg, lon, hộp, gói...", PX, 100, half);
            txtStock = MakeField(body, "Số lượng tồn", "0.00", PX + half + 16, 100, half);

            // Row 3 – half+half  (label y=176, field y=196, bottom=240)
            txtMinStock = MakeField(body, "Mức tồn tối thiểu", "0.00", PX, 176, half);
            txtPurchasePrice = MakeField(body, "Giá nhập (VNĐ)", "0", PX + half + 16, 176, half);

            // Divider  (y=252)
            body.Controls.Add(new Panel
            {
                Location = new Point(PX, 252),
                Size = new Size(inner, 1),
                BackColor = C_BORDER
            });

            // Note  (y=264)
            body.Controls.Add(new Label
            {
                Text = "✦  Tất cả các trường đều bắt buộc nhập.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = C_MUTED,
                Location = new Point(PX, 276),
                AutoSize = true,
                BackColor = Color.Transparent
            });

            // Add in correct Z-order: body fills, footer docks bottom, header docks top
            Controls.Add(body);
            Controls.Add(footer);
            Controls.Add(header);
        }

        // ── Field helper ─────────────────────────────────────────
        private Guna2TextBox MakeField(Panel parent, string label, string placeholder,
                                        int x, int y, int w)
        {
            parent.Controls.Add(new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = C_MUTED,
                Location = new Point(x, y+ 20),
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

        // ── Load ─────────────────────────────────────────────────
        private void LoadIngredient(int id)
        {
            try
            {
                var ing = IngredientBLL.GetIngredientByID(id);
                if (ing == null)
                {
                    MessageBox.Show("Không tìm thấy nguyên liệu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close(); return;
                }
                txtName.Text = ing.IngredientName;
                txtUnit.Text = ing.Unit;
                txtStock.Text = ing.StockQuantity.ToString();
                txtMinStock.Text = ing.MinStock.ToString();
                txtPurchasePrice.Text = ing.PurchasePrice.ToString("0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        // ── Save ─────────────────────────────────────────────────
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string unit = txtUnit.Text.Trim();

                if (string.IsNullOrWhiteSpace(name)) throw new Exception("Tên nguyên liệu không được để trống.");
                if (string.IsNullOrWhiteSpace(unit)) throw new Exception("Đơn vị tính không được để trống.");
                if (!float.TryParse(txtStock.Text.Trim(), out float stock)) throw new Exception("Số lượng tồn không hợp lệ.");
                if (!float.TryParse(txtMinStock.Text.Trim(), out float minStock)) throw new Exception("Mức tồn tối thiểu không hợp lệ.");
                if (!decimal.TryParse(txtPurchasePrice.Text.Trim(), out decimal price)) throw new Exception("Giá nhập không hợp lệ.");
                if (stock < 0) throw new Exception("Số lượng tồn không được âm.");
                if (minStock < 0) throw new Exception("Mức tồn tối thiểu không được âm.");
                if (price < 0) throw new Exception("Giá nhập không được âm.");

                var ing = new IngredientDTO
                {
                    IngredientName = name,
                    Unit = unit,
                    StockQuantity = stock,
                    MinStock = minStock,
                    PurchasePrice = price,
                    IsActive = true
                };

                if (_ingredientID.HasValue)
                {
                    ing.IngredientID = _ingredientID.Value;
                    IngredientBLL.UpdateIngredient(ing);
                    MessageBox.Show("Cập nhật nguyên liệu thành công.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    IngredientBLL.AddIngredient(ing);
                    MessageBox.Show("Thêm nguyên liệu thành công.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}