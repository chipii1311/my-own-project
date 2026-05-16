using Guna.UI2.WinForms;
using my_own_project.BLL;
using my_own_project.DTO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class AccountForm : Form
    {
        public int LoggedInUserID { get; set; }
        private UserDTO currentUser;

        private Guna2TextBox txtFullName, txtEmail, txtPhone, txtRole;
        private Guna2TextBox txtOldPass, txtNewPass, txtConfirmPass;

        // ── Palette ──────────────────────────────────────────────
        private static readonly Color C_BG = Color.FromArgb(245, 246, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(99, 88, 255);
        private static readonly Color C_PURPLE_DARK = Color.FromArgb(78, 68, 220);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(237, 235, 255);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_GREEN_DARK = Color.FromArgb(22, 163, 74);
        private static readonly Color C_TEXT = Color.FromArgb(22, 22, 38);
        private static readonly Color C_MUTED = Color.FromArgb(130, 128, 158);
        private static readonly Color C_BORDER = Color.FromArgb(225, 224, 240);
        private static readonly Color C_READONLY = Color.FromArgb(244, 244, 250);
        private static readonly Color C_DIVIDER = Color.FromArgb(235, 234, 248);

        public AccountForm(int userID)
        {
            InitializeComponent();
            Controls.Clear();
            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            Font = new Font("Segoe UI", 9.5F);

            LoggedInUserID = userID;

            BuildUI();
            LoadAccountData();
        }

        // ─────────────────────────────────────────────────────────
        //  BUILD UI
        // ─────────────────────────────────────────────────────────
        private void BuildUI()
        {
            // ── Top header bar ───────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = C_BG
            };

            // Avatar circle
            var pnlAvatar = new Panel
            {
                Size = new Size(52, 52),
                Location = new Point(36, 14),
                BackColor = Color.Transparent
            };
            pnlAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // 1. Sử dụng khối using truyền thống cho LinearGradientBrush
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, 52, 52),
                    C_PURPLE, Color.FromArgb(140, 120, 255), 135f))
                {
                    e.Graphics.FillEllipse(brush, 1, 1, 50, 50);
                }

                // Person silhouette (Hình bóng người)
                // 2. Sử dụng khối using cho Pen
                using (var pen = new Pen(Color.White, 2f))
                {
                    e.Graphics.FillEllipse(Brushes.White, 17, 10, 18, 18);

                    // 3. Sử dụng khối using cho GraphicsPath
                    using (var clip = new GraphicsPath())
                    {
                        clip.AddEllipse(4, 30, 44, 30);
                        e.Graphics.SetClip(clip);
                        e.Graphics.FillEllipse(Brushes.White, 8, 28, 36, 28);
                        e.Graphics.ResetClip();
                    }
                }
            };

            var lblTitle = new Label
            {
                Text = "Tài khoản của tôi",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                Location = new Point(100, 14),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            var lblSub = new Label
            {
                Text = "Quản lý thông tin cá nhân và bảo mật tài khoản",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = C_MUTED,
                Location = new Point(102, 46),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            pnlHeader.Controls.AddRange(new Control[] { pnlAvatar, lblTitle, lblSub });

            // ── Tab strip ────────────────────────────────────────
            var pnlTabs = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = C_DIVIDER
            };

            // ── Main content: two cards side-by-side ─────────────
            var pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG,
                Padding = new Padding(36, 20, 36, 36)
            };

            // LEFT card — personal info
            var cardInfo = MakeCard();
            cardInfo.Location = new Point(36, 20);
            cardInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            BuildInfoCard(cardInfo);

            // RIGHT card — change password
            var cardPass = MakeCard();
            cardPass.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            BuildPassCard(cardPass);

            // Resize handler to make cards fill width evenly
            pnlContent.SizeChanged += (s, e) =>
            {
                int gap = 20;
                int margin = 36;
                int w = (pnlContent.Width - margin * 2 - gap) / 2;
                int h = pnlContent.Height - 56;

                cardInfo.SetBounds(margin, 20, w, h);
                cardPass.SetBounds(margin + w + gap, 20, w, h);
            };

            pnlContent.Controls.Add(cardInfo);
            pnlContent.Controls.Add(cardPass);

            Controls.Add(pnlContent);
            Controls.Add(pnlTabs);
            Controls.Add(pnlHeader);

            pnlHeader.BringToFront();
        }

        // ── Card shell ───────────────────────────────────────────
        private Panel MakeCard()
        {
            var card = new Panel { BackColor = C_WHITE };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);

                // 1. Khối using truyền thống cho GraphicsPath
                using (var path = RoundedRect(rect, 14))
                {
                    // 2. Khối using cho SolidBrush dùng để tô nền card
                    using (var brush = new SolidBrush(C_WHITE))
                    {
                        g.FillPath(brush, path);
                    }

                    // 3. Khối using cho Pen vẽ đường viền (Border)
                    using (var pen = new Pen(C_BORDER, 1f))
                    {
                        g.DrawPath(pen, path);
                    }
                }

                // 4. Khối using độc lập cho Pen vẽ bóng đổ phía dưới (Drop shadow)
                // Subtle drop shadow hint (bottom edge)
                using (var shadow = new Pen(Color.FromArgb(18, 99, 88, 255), 1f))
                {
                    g.DrawLine(shadow, 14, card.Height - 1, card.Width - 14, card.Height - 1);
                }
            };

            card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, 9999, 9999, 14, 14));
            return card;
        }

        // ── Info card ────────────────────────────────────────────
        private void BuildInfoCard(Panel card)
        {
            // Section header
            var pnlTop = SectionHeader("👤  Thông tin cá nhân",
                "Họ tên và số điện thoại có thể thay đổi", C_PURPLE, C_PURPLE_SOFT);
            card.Controls.Add(pnlTop);

            // Fields
            var pnlFields = new Panel
            {
                Location = new Point(0, 78),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackColor = Color.Transparent,
                Padding = new Padding(28, 16, 28, 0)
            };
            card.Controls.Add(pnlFields);

            card.SizeChanged += (s, e) =>
            {
                pnlFields.Size = new Size(card.Width, card.Height - 78);
                LayoutFields(pnlFields);
            };

            void LayoutFields(Panel p)
            {
                int w = p.Width - 56;
                int y = 16;

                if (txtFullName == null)
                {
                    txtFullName = MakeField(p, "Họ và tên", "Nhập họ và tên đầy đủ...", 28, y, w, false);
                    y += 78;
                    txtPhone = MakeField(p, "Số điện thoại", "VD: 0901 234 567", 28, y, w, false);
                    y += 78;
                    txtEmail = MakeField(p, "Email đăng nhập", "", 28, y, w, true);
                    y += 78;
                    txtRole = MakeField(p, "Chức vụ", "", 28, y, w, true);
                    y += 90;

                    var btnSave = MakeButton("  Cập nhật thông tin", C_PURPLE, C_PURPLE_DARK);
                    btnSave.Location = new Point(28, y);
                    btnSave.Width = w;
                    btnSave.Click += BtnSaveInfo_Click;
                    p.Controls.Add(btnSave);
                }
                else
                {
                    int bx = 28;
                    txtFullName.SetBounds(bx, 16, w, 42);
                    txtPhone.SetBounds(bx, 16 + 78, w, 42);
                    txtEmail.SetBounds(bx, 16 + 156, w, 42);
                    txtRole.SetBounds(bx, 16 + 234, w, 42);

                    foreach (Control c in p.Controls)
                        if (c is Guna2Button btn) { btn.Location = new Point(bx, 16 + 316); btn.Width = w; }
                }
            }
        }

        // ── Pass card ────────────────────────────────────────────
        private void BuildPassCard(Panel card)
        {
            var pnlTop = SectionHeader("🔒  Đổi mật khẩu",
                "Sử dụng mật khẩu mạnh để bảo vệ tài khoản", C_GREEN, Color.FromArgb(220, 252, 231));
            card.Controls.Add(pnlTop);

            var pnlFields = new Panel
            {
                Location = new Point(0, 78),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackColor = Color.Transparent,
                Padding = new Padding(28, 16, 28, 0)
            };
            card.Controls.Add(pnlFields);

            card.SizeChanged += (s, e) =>
            {
                pnlFields.Size = new Size(card.Width, card.Height - 78);
                LayoutPassFields(pnlFields);
            };

            void LayoutPassFields(Panel p)
            {
                int w = p.Width - 56;
                int y = 16;

                if (txtOldPass == null)
                {
                    txtOldPass = MakeField(p, "Mật khẩu hiện tại", "", 28, y, w, false, '●');
                    y += 78;
                    txtNewPass = MakeField(p, "Mật khẩu mới", "", 28, y, w, false, '●');
                    y += 78;
                    txtConfirmPass = MakeField(p, "Xác nhận mật khẩu mới", "", 28, y, w, false, '●');
                    y += 90;

                    // Password strength hint
                    var lblHint = new Label
                    {
                        Text = "💡  Mật khẩu nên có ít nhất 8 ký tự, bao gồm chữ và số.",
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                        ForeColor = C_MUTED,
                        Location = new Point(28, y),
                        Size = new Size(w, 36),
                        BackColor = Color.Transparent
                    };
                    p.Controls.Add(lblHint);
                    y += 44;

                    var btnChange = MakeButton("  Lưu mật khẩu mới", C_GREEN, C_GREEN_DARK);
                    btnChange.Location = new Point(28, y);
                    btnChange.Width = w;
                    btnChange.Click += BtnChangePass_Click;
                    p.Controls.Add(btnChange);
                }
                else
                {
                    int bx = 28;
                    txtOldPass.SetBounds(bx, 16, w, 42);
                    txtNewPass.SetBounds(bx, 16 + 78, w, 42);
                    txtConfirmPass.SetBounds(bx, 16 + 156, w, 42);

                    foreach (Control c in p.Controls)
                    {
                        if (c is Guna2Button btn) { btn.Location = new Point(bx, 16 + 316); btn.Width = w; }
                        if (c is Label lbl && lbl.Font.Italic) lbl.SetBounds(bx, 16 + 256, w, 36);
                    }
                }
            }
        }

        // ── Section header strip ─────────────────────────────────
        private Panel SectionHeader(string title, string subtitle, Color accent, Color bg)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 78,
                BackColor = bg,
                Padding = new Padding(28, 14, 28, 0)
            };

            pnl.Paint += (s, e) =>
            {
                // left accent bar
                // Đã sửa thành khối using truyền thống để fix lỗi CS8370
                using (var brush = new SolidBrush(accent))
                {
                    e.Graphics.FillRectangle(brush, 0, 18, 4, 42);
                }
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Location = new Point(32, 14),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblSub = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = C_MUTED,
                Location = new Point(34, 42),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            pnl.Controls.AddRange(new Control[] { lblTitle, lblSub });
            return pnl;
        }

        // ── Field factory ────────────────────────────────────────
        private Guna2TextBox MakeField(Panel parent, string label, string placeholder,
                                       int x, int y, int width, bool readOnly, char passChar = '\0')
        {
            var lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = C_MUTED,
                Location = new Point(x, y),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var txt = new Guna2TextBox
            {
                Location = new Point(x, y + 20),
                Size = new Size(width, 42),
                BorderRadius = 10,
                BorderColor = C_BORDER,
                FillColor = readOnly ? C_READONLY : C_WHITE,
                PlaceholderText = placeholder,
                Font = new Font("Segoe UI", 10F),
                ForeColor = readOnly ? C_MUTED : C_TEXT,
                PlaceholderForeColor = C_MUTED,
                ReadOnly = readOnly,
                Padding = new Padding(6, 0, 0, 0)
            };

            if (passChar != '\0') txt.PasswordChar = passChar;
            if (!readOnly)
            {
                txt.FocusedState.BorderColor = C_PURPLE;
                txt.HoverState.BorderColor = Color.FromArgb(180, 160, 255);
            }

            parent.Controls.Add(lbl);
            parent.Controls.Add(txt);
            return txt;
        }

        // ── Button factory ───────────────────────────────────────
        private Guna2Button MakeButton(string text, Color fill, Color hover)
        {
            var btn = new Guna2Button
            {
                Text = text,
                Height = 44,
                BorderRadius = 10,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FillColor = fill,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btn.HoverState.FillColor = hover;
            return btn;
        }

        // ── GDI helpers ──────────────────────────────────────────
        private static GraphicsPath RoundedRect(Rectangle b, int r)
        {
            int d = r * 2;
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, d, d, 180, 90);
            path.AddArc(b.Right - d, b.Y, d, d, 270, 90);
            path.AddArc(b.Right - d, b.Bottom - d, d, d, 0, 90);
            path.AddArc(b.X, b.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        // ─────────────────────────────────────────────────────────
        //  DATA & EVENTS
        // ─────────────────────────────────────────────────────────
        private void LoadAccountData()
        {
            try
            {
                currentUser = UserBLL.GetUserByID(LoggedInUserID);
                if (currentUser != null)
                {
                    txtFullName.Text = currentUser.FullName;
                    txtEmail.Text = currentUser.Email;
                    txtPhone.Text = currentUser.Phone;
                    txtRole.Text = currentUser.Role;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser == null) return;
                currentUser.FullName = txtFullName.Text.Trim();
                currentUser.Phone = txtPhone.Text.Trim();

                bool success = UserBLL.UpdateUser(currentUser);
                if (success)
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPass.Text != txtConfirmPass.Text)
                { MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                bool success = UserBLL.ChangePassword(LoggedInUserID, txtOldPass.Text, txtNewPass.Text);
                if (success)
                {
                    MessageBox.Show("Đổi mật khẩu thành công! Hãy ghi nhớ mật khẩu mới.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOldPass.Clear(); txtNewPass.Clear(); txtConfirmPass.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}