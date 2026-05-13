using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class InventoryForm : Form
    {
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color C_BORDER = Color.FromArgb(232, 232, 240);

        private Guna2TextBox txtSearch;

        private Guna2Button btnAdd;
        private Guna2Button btnEdit;
        private Guna2Button btnDelete;
        private Guna2Button btnImport;
        private Guna2Button btnExport;
        private Guna2Button btnRefresh;

        private Guna2DataGridView dgvInventory;
        private Guna2DataGridView dgvTransactions;

        public InventoryForm()
        {
            InitializeComponent();

            Controls.Clear();
            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;

            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            SuspendLayout();

            Panel header = BuildHeader();
            Panel searchPanel = BuildSearchPanel();

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 430,
                BackColor = C_BG,
                BorderStyle = BorderStyle.None
            };

            dgvInventory = CreateGrid();
            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
            dgvInventory.DataBindingComplete += DgvInventory_DataBindingComplete;
            dgvInventory.CellDoubleClick += DgvInventory_CellDoubleClick;

            dgvTransactions = CreateGrid();
            dgvTransactions.DataBindingComplete += DgvTransactions_DataBindingComplete;

            split.Panel1.Controls.Add(CreateCardPanel("Danh sách nguyên liệu", dgvInventory));
            split.Panel2.Controls.Add(CreateCardPanel("Lịch sử nhập / xuất gần đây", dgvTransactions));

            Controls.Add(split);
            Controls.Add(searchPanel);
            Controls.Add(header);

            ResumeLayout(false);
        }

        private Panel BuildHeader()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = C_WHITE,
                Padding = new Padding(24, 0, 24, 0)
            };

            header.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(C_BORDER, 1))
                {
                    e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
                }
            };

            Label title = new Label
            {
                Text = "QUẢN LÝ KHO NGUYÊN LIỆU",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(24, 21)
            };

            btnAdd = CreateButton("+ Thêm", C_GREEN);
            btnEdit = CreateButton("Sửa", C_PURPLE);
            btnDelete = CreateButton("Xóa", C_RED);
            btnImport = CreateButton("Nhập kho", C_PURPLE);
            btnExport = CreateButton("Xuất kho", C_RED);
            btnRefresh = CreateButton("↻", C_PURPLE);

            btnAdd.Size = new Size(90, 38);
            btnEdit.Size = new Size(80, 38);
            btnDelete.Size = new Size(80, 38);
            btnImport.Size = new Size(110, 38);
            btnExport.Size = new Size(110, 38);
            btnRefresh.Size = new Size(46, 38);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnRefresh.Click += (s, e) => LoadData();

            header.Controls.Add(title);
            header.Controls.Add(btnAdd);
            header.Controls.Add(btnEdit);
            header.Controls.Add(btnDelete);
            header.Controls.Add(btnImport);
            header.Controls.Add(btnExport);
            header.Controls.Add(btnRefresh);

            header.Resize += (s, e) =>
            {
                int right = header.Width - 24;
                int y = 16;
                int gap = 8;

                btnRefresh.Location = new Point(right - btnRefresh.Width, y);
                right -= btnRefresh.Width + gap;

                btnExport.Location = new Point(right - btnExport.Width, y);
                right -= btnExport.Width + gap;

                btnImport.Location = new Point(right - btnImport.Width, y);
                right -= btnImport.Width + gap;

                btnDelete.Location = new Point(right - btnDelete.Width, y);
                right -= btnDelete.Width + gap;

                btnEdit.Location = new Point(right - btnEdit.Width, y);
                right -= btnEdit.Width + gap;

                btnAdd.Location = new Point(right - btnAdd.Width, y);
            };

            return header;
        }

        private Panel BuildSearchPanel()
        {
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = C_WHITE,
                Padding = new Padding(24, 10, 24, 10)
            };

            txtSearch = new Guna2TextBox
            {
                PlaceholderText = "Tìm kiếm nguyên liệu...",
                Font = new Font("Segoe UI", 11F),
                FillColor = C_BG,
                BorderColor = C_BORDER,
                BorderRadius = 8,
                Size = new Size(380, 38),
                Location = new Point(24, 10)
            };

            txtSearch.FocusedState.BorderColor = C_PURPLE;
            txtSearch.HoverState.BorderColor = C_PURPLE;
            txtSearch.TextChanged += (s, e) => ApplyFilter();

            searchPanel.Controls.Add(txtSearch);

            return searchPanel;
        }

        private Panel CreateCardPanel(string titleText, Control content)
        {
            Panel wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = C_BG
            };

            Guna2Panel card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Padding = new Padding(0)
            };

            Label title = new Label
            {
                Text = titleText,
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(16, 13, 0, 0),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT
            };

            Panel divider = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = C_BORDER
            };

            card.Controls.Add(content);
            card.Controls.Add(divider);
            card.Controls.Add(title);

            content.BringToFront();

            wrapper.Controls.Add(card);

            return wrapper;
        }

        private Guna2DataGridView CreateGrid()
        {
            Guna2DataGridView grid = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = C_BORDER,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Cursor = Cursors.Hand
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            grid.DefaultCellStyle.BackColor = C_WHITE;
            grid.DefaultCellStyle.ForeColor = C_TEXT;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            grid.DefaultCellStyle.SelectionForeColor = C_TEXT;
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            grid.RowTemplate.Height = 42;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            return grid;
        }

        private Guna2Button CreateButton(string text, Color color)
        {
            return new Guna2Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FillColor = color,
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
        }

        private void LoadData()
        {
            LoadIngredients();
            LoadTransactions();
        }

        private void LoadIngredients()
        {
            try
            {
                DataTable data = IngredientBLL.GetAllIngredients();
                dgvInventory.DataSource = data;

                FormatInventoryGrid();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách nguyên liệu: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadTransactions()
        {
            try
            {
                DataTable data = InventoryTransactionBLL.GetRecentTransactions();
                dgvTransactions.DataSource = data;

                FormatTransactionGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải lịch sử kho: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            if (!(dgvInventory.DataSource is DataTable data))
                return;

            string keyword = txtSearch.Text.Trim().Replace("'", "''");

            if (string.IsNullOrWhiteSpace(keyword))
            {
                data.DefaultView.RowFilter = "";
            }
            else
            {
                data.DefaultView.RowFilter =
                    $"IngredientName LIKE '%{keyword}%' OR Unit LIKE '%{keyword}%'";
            }
        }

        private void FormatInventoryGrid()
        {
            if (dgvInventory.Columns.Count == 0)
                return;

            HideColumn(dgvInventory, "IngredientID");
            HideColumn(dgvInventory, "IsActive");

            SetHeader(dgvInventory, "IngredientName", "Tên nguyên liệu");
            SetHeader(dgvInventory, "Unit", "Đơn vị");
            SetHeader(dgvInventory, "StockQuantity", "Tồn kho");
            SetHeader(dgvInventory, "MinStock", "Tồn tối thiểu");
            SetHeader(dgvInventory, "PurchasePrice", "Giá nhập");
            SetHeader(dgvInventory, "StockStatus", "Trạng thái");

            if (dgvInventory.Columns.Contains("PurchasePrice"))
            {
                dgvInventory.Columns["PurchasePrice"].DefaultCellStyle.Format = "N0";
                dgvInventory.Columns["PurchasePrice"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvInventory.Columns.Contains("StockQuantity"))
                dgvInventory.Columns["StockQuantity"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

            if (dgvInventory.Columns.Contains("MinStock"))
                dgvInventory.Columns["MinStock"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
        }

        private void FormatTransactionGrid()
        {
            if (dgvTransactions.Columns.Count == 0)
                return;

            HideColumn(dgvTransactions, "TransactionID");

            SetHeader(dgvTransactions, "IngredientName", "Nguyên liệu");
            SetHeader(dgvTransactions, "Unit", "Đơn vị");
            SetHeader(dgvTransactions, "QuantityChanged", "Số lượng");
            SetHeader(dgvTransactions, "TransactionType", "Loại");
            SetHeader(dgvTransactions, "TransactionDate", "Thời gian");
            SetHeader(dgvTransactions, "StaffName", "Nhân viên");
            SetHeader(dgvTransactions, "Note", "Ghi chú");

            if (dgvTransactions.Columns.Contains("QuantityChanged"))
                dgvTransactions.Columns["QuantityChanged"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

            if (dgvTransactions.Columns.Contains("TransactionDate"))
                dgvTransactions.Columns["TransactionDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void HideColumn(DataGridView grid, string columnName)
        {
            if (grid.Columns.Contains(columnName))
                grid.Columns[columnName].Visible = false;
        }

        private void SetHeader(DataGridView grid, string columnName, string header)
        {
            if (grid.Columns.Contains(columnName))
                grid.Columns[columnName].HeaderText = header;
        }

        private int? GetSelectedIngredientID()
        {
            if (dgvInventory.CurrentRow == null)
                return null;

            if (!dgvInventory.Columns.Contains("IngredientID"))
                return null;

            object value = dgvInventory.CurrentRow.Cells["IngredientID"].Value;

            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToInt32(value);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (IngredientEditForm form = new IngredientEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedIngredientID();

            if (!id.HasValue)
            {
                MessageBox.Show(
                    "Vui lòng chọn nguyên liệu cần sửa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (IngredientEditForm form = new IngredientEditForm(id.Value))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    LoadData();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedIngredientID();

            if (!id.HasValue)
            {
                MessageBox.Show(
                    "Vui lòng chọn nguyên liệu cần xóa.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc muốn xóa nguyên liệu này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                IngredientBLL.DeleteIngredient(id.Value);

                MessageBox.Show(
                    "Xóa nguyên liệu thành công.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa nguyên liệu: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (ImportStockForm form = new ImportStockForm(GetSelectedIngredientID()))
            {
                form.ShowDialog();
            }

            LoadData();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (ExportStockForm form = new ExportStockForm(GetSelectedIngredientID()))
            {
                form.ShowDialog();
            }

            LoadData();
        }

        private void DgvInventory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            BtnEdit_Click(sender, EventArgs.Empty);
        }

        private void DgvInventory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatInventoryGrid();
        }

        private void DgvTransactions_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatTransactionGrid();

            if (!dgvTransactions.Columns.Contains("TransactionType"))
                return;

            foreach (DataGridViewRow row in dgvTransactions.Rows)
            {
                object value = row.Cells["TransactionType"].Value;

                if (value == null)
                    continue;

                string type = value.ToString();

                if (type == "IMPORT")
                {
                    row.Cells["TransactionType"].Value = "Nhập kho";
                    row.Cells["TransactionType"].Style.ForeColor = C_GREEN;
                    row.Cells["TransactionType"].Style.Font =
                        new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
                else if (type == "EXPORT")
                {
                    row.Cells["TransactionType"].Value = "Xuất kho";
                    row.Cells["TransactionType"].Style.ForeColor = C_RED;
                    row.Cells["TransactionType"].Style.Font =
                        new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
            }
        }

        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = dgvInventory.Columns[e.ColumnIndex].Name;

            if (columnName != "StockStatus")
                return;

            string status = e.Value?.ToString();

            if (status == "Hết hàng")
            {
                e.CellStyle.ForeColor = C_RED;
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
            else if (status == "Sắp hết")
            {
                e.CellStyle.ForeColor = C_AMBER;
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.ForeColor = C_GREEN;
                e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
        }
    }
}