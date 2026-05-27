using Guna.UI2.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    partial class SettingForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls UI
        private Guna2TextBox txtCategoryID, txtCategoryName;
        private Guna2Button btnSaveCat, btnDeleteCat;
        private Guna2DataGridView dgvCategories;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Name = "SettingForm";
            this.ResumeLayout(false);
        }

        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor = C_BG;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(20);

            // --- Layout chính: Panel trái (Grid) & Phải (Form) ---
            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // Grid Danh mục
            dgvCategories = CreateGrid();
            dgvCategories.CellClick += DgvCategories_CellClick;
            tlp.Controls.Add(BuildCard("Danh sách danh mục", dgvCategories), 0, 0);

            // Form nhập liệu
            tlp.Controls.Add(BuildFormCard(), 1, 0);

            this.Controls.Add(tlp);
        }

        private Panel BuildFormCard()
        {
            Panel p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0) };
            Guna2Panel card = new Guna2Panel { Dock = DockStyle.Fill, FillColor = C_WHITE, BorderRadius = 10, Padding = new Padding(20) };

            txtCategoryID = new Guna2TextBox { Visible = false };
            txtCategoryName = new Guna2TextBox { PlaceholderText = "Tên danh mục", Width = 300, Height = 40, BorderRadius = 5, Margin = new Padding(0, 0, 0, 10) };

            btnSaveCat = new Guna2Button { Text = "Lưu", Size = new Size(100, 40), FillColor = Color.FromArgb(210, 210, 218), Enabled = false };
            btnSaveCat.Click += BtnSaveCat_Click;

            btnDeleteCat = new Guna2Button { Text = "Xóa", Size = new Size(100, 40), FillColor = Color.FromArgb(210, 210, 218), Enabled = false };
            btnDeleteCat.Click += BtnDeleteCat_Click;

            card.Controls.AddRange(new Control[] { txtCategoryID, txtCategoryName, btnSaveCat, btnDeleteCat });
            p.Controls.Add(card);
            return p;
        }

        // --- Helper cho Grid và Card ---
        private Panel BuildCard(string title, Control ctrl)
        {
            Guna2Panel card = new Guna2Panel { Dock = DockStyle.Fill, FillColor = C_WHITE, BorderRadius = 10 };
            Label lbl = new Label { Text = title, Font = new Font("Segoe UI", 12F, FontStyle.Bold), Location = new Point(20, 20) };
            ctrl.Location = new Point(20, 60);
            ctrl.Size = new Size(card.Width - 40, card.Height - 80);
            card.Controls.Add(lbl); card.Controls.Add(ctrl);
            return card;
        }

        private Guna2DataGridView CreateGrid()
        {
            return new Guna2DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        }

        private void BtnDelete_Click(object sender, System.EventArgs e)
        {
            // No-op safe implementation; replace with real delete logic.
            if (string.IsNullOrEmpty(txtCategoryID?.Text))
                return;

            // TODO: delete the category from your data source and refresh dgvCategories.
            // Example UI updates:
            txtCategoryID.Text = "";
            txtCategoryName.Text = "";
            btnSaveCat.Enabled = false;
            btnDeleteCat.Enabled = false;
        }
    }
}