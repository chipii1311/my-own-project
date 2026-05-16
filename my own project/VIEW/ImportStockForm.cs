using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class ImportStockForm : Form
    {
        private int? _ingredientID;

        private ComboBox cboIngredient;
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtPrice;
        private Guna2TextBox txtNote;
        private Guna2Button btnSave;
        private Guna2Button btnCancel;

        // ── Palette ──────────────────────────────────────────────
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

        public ImportStockForm(int? ingredientID = null)
        {
            _ingredientID = ingredientID;
            InitializeComponent();
            Controls.Clear();

            Text = "Nhập kho nguyên liệu";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(560, 456);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = C_BG;
            Font = new Font("Segoe UI", 9.5F);

            BuildUI();
            LoadIngredients();

            Shown += (s, e) => { if (_ingredientID.HasValue) SetIngredient(_ingredientID.Value); };
        }

        private void BuildUI()
        {
            // ── Header ───────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 88, BackColor = C_PURPLE };

            var pnlIcon = new Panel { Size = new Size(46, 46), Location = new Point(28, 21), BackColor = Color.Transparent };
            pnlIcon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (var brush = new SolidBrush(Color.FromArgb(45, 255, 255, 255)))
                    e.Graphics.FillEllipse(brush, 0, 0, 45, 45);

                using (var pen = new Pen(Color.White, 2f))
                {
                    e.Graphics.DrawLine(pen, 22, 10, 22, 26);
                    e.Graphics.DrawLine(pen, 15, 20, 22, 28);
                    e.Graphics.DrawLine(pen, 29, 20, 22, 28);
                    e.Graphics.DrawLine(pen, 12, 32, 12, 38);
                    e.Graphics.DrawLine(pen, 32, 32, 32, 38);
                    e.Graphics.DrawLine(pen, 12, 38, 32, 38);
                }
            };

            var lblTitle = new Label
            {
                Text = "NHẬP KHO NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(86, 18),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            var lblSub = new Label
            {
                Text = "Ghi nhận phiếu nhập hàng vào kho",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(200, 255, 255, 255),
                Location = new Point(88, 46),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.AddRange(new Control[] { pnlIcon, lblTitle, lblSub });

            // ── Body ─────────────────────────────────────────────
            var pnlBody = new Panel { Location = new Point(0, 88), Size = new Size(560, 306), BackColor = C_BG };

            AddLabel(pnlBody, "Nguyên liệu", 28, 16);
            cboIngredient = new ComboBox
            {
                Location = new Point(28, 42),
                Size = new Size(488, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                BackColor = C_WHITE,
                ForeColor = C_TEXT
            };
            if (_ingredientID.HasValue) cboIngredient.Enabled = false;
            pnlBody.Controls.Add(cboIngredient);

            txtQuantity = AddField(pnlBody, "Số lượng nhập", "VD: 10, 5.5...", 28, 86, 228);
            txtPrice = AddField(pnlBody, "Giá nhập (VNĐ)", "VD: 50000", 288, 86, 228);
            txtNote = AddField(pnlBody, "Ghi chú", "Ghi chú nếu có (không bắt buộc)...", 28, 157, 488);

            var pnlBadge = new Panel { Location = new Point(28, 268), Size = new Size(488, 38), BackColor = C_TAG_BG };
            pnlBadge.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, pnlBadge.Width - 1, pnlBadge.Height - 1), 8))
                {
                    using (var brush = new SolidBrush(C_TAG_BG)) e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(C_BORDER)) e.Graphics.DrawPath(pen, path);
                }
            };
            var lblBadge = new Label
            {
                Text = "ℹ  Số lượng sẽ được cộng vào tồn kho. Giá nhập sẽ cập nhật giá mới nhất.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = C_PURPLE,
                Location = new Point(10, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlBadge.Controls.Add(lblBadge);
            pnlBody.Controls.Add(pnlBadge);

            // ── Footer ───────────────────────────────────────────
            var pnlFooter = new Panel { Location = new Point(0, 394), Size = new Size(560, 62), BackColor = C_WHITE };
            pnlFooter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER), 0, 0, 560, 0);

            btnCancel = new Guna2Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_CANCEL_BG,
                ForeColor = C_MUTED,
                BorderColor = C_BORDER,
                BorderThickness = 1,
                BorderRadius = 10,
                Size = new Size(120, 40),
                Location = new Point(216, 11),
                Cursor = Cursors.Hand
            };
            btnCancel.HoverState.FillColor = C_CANCEL_HVR;
            btnCancel.Click += (s, e) => Close();

            btnSave = new Guna2Button
            {
                Text = "  Xác nhận nhập",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = C_PURPLE,
                ForeColor = Color.White,
                BorderRadius = 10,
                Size = new Size(168, 40),
                Location = new Point(348, 11),
                Cursor = Cursors.Hand
            };
            btnSave.HoverState.FillColor = C_PURPLE_DARK;
            btnSave.Click += BtnSave_Click;

            pnlFooter.Controls.AddRange(new Control[] { btnCancel, btnSave });
            Controls.AddRange(new Control[] { pnlHeader, pnlBody, pnlFooter });
        }

        // ── Helpers ──────────────────────────────────────────────
        private void AddLabel(Panel parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                Location = new Point(x, y),
                AutoSize = true,
                BackColor = Color.Transparent
            });
        }

        private Guna2TextBox AddField(Panel parent, string label, string placeholder, int x, int y, int width)
        {
            AddLabel(parent, label, x, y);
            var txt = new Guna2TextBox
            {
                Location = new Point(x, y + 26),
                Size = new Size(width, 42),
                BorderRadius = 10,
                BorderColor = C_BORDER,
                FillColor = C_WHITE,
                PlaceholderText = placeholder,
                Font = new Font("Segoe UI", 10F),
                ForeColor = C_TEXT,
                PlaceholderForeColor = C_MUTED,
                Padding = new Padding(6, 0, 0, 0)
            };
            txt.FocusedState.BorderColor = C_PURPLE;
            txt.HoverState.BorderColor = Color.FromArgb(180, 160, 255);
            parent.Controls.Add(txt);
            return txt;
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

        private void SetIngredient(int id)
        {
            if (cboIngredient.Items.Count > 0)
                cboIngredient.SelectedValue = id;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboIngredient.SelectedItem == null)
                { MessageBox.Show("Vui lòng chọn nguyên liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                if (!float.TryParse(txtQuantity.Text.Trim(), out float qty) || qty <= 0)
                { MessageBox.Show("Số lượng nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtQuantity.Focus(); return; }

                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
                { MessageBox.Show("Giá nhập không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPrice.Focus(); return; }

                int ingredientID = Convert.ToInt32(cboIngredient.SelectedValue);
                string note = txtNote.Text.Trim();

                InventoryTransactionBLL.ImportIngredient(ingredientID, qty, 0, note);

                IngredientDTO ing = IngredientBLL.GetIngredientByID(ingredientID);
                if (ing != null) { ing.PurchasePrice = price; IngredientBLL.UpdateIngredient(ing); }

                MessageBox.Show("Nhập kho thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}