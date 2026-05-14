using Guna.UI2.WinForms;
using my_own_project.DAL;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class RecipeManagementForm : Form
    {
        // ============================================================
        // DESIGN TOKENS
        // ============================================================
        private static readonly Color C_BG = Color.FromArgb(244, 245, 250);
        private static readonly Color C_WHITE = Color.White;
        private static readonly Color C_PURPLE = Color.FromArgb(108, 99, 255);
        private static readonly Color C_PURPLE_SOFT = Color.FromArgb(238, 237, 254);
        private static readonly Color C_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color C_RED = Color.FromArgb(239, 68, 68);
        private static readonly Color C_AMBER = Color.FromArgb(245, 158, 11);
        private static readonly Color C_TEXT = Color.FromArgb(30, 30, 46);
        private static readonly Color C_MUTED = Color.FromArgb(122, 122, 140);
        private static readonly Color C_BORDER = Color.FromArgb(232, 232, 240);

        // ============================================================
        // CONTROLS
        // ============================================================
        private ComboBox cboMenuItem;
        private ComboBox cboIngredient;

        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtSearchIngredient;

        private Guna2Button btnAdd;
        private Guna2Button btnUpdate;
        private Guna2Button btnDelete;
        private Guna2Button btnClear;
        private Guna2Button btnRefresh;

        private Guna2DataGridView dgvRecipe;
        private Guna2DataGridView dgvIngredients;

        private Label lblSelectedFood;
        private Label lblSummary;

        private int selectedRecipeID = 0;

        public RecipeManagementForm()
        {
            InitializeComponent();
            Controls.Clear();

            BackColor = C_BG;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;

            BuildUI();

            LoadMenuItems();
            LoadIngredients();
            LoadRecipeBySelectedMenu();
        }

        // ============================================================
        // UI BUILD
        // ============================================================
        private void BuildUI()
        {
            SuspendLayout();

            Panel header = BuildHeader();
            Panel main = BuildMainLayout();

            Controls.Add(main);
            Controls.Add(header);

            header.BringToFront();

            ResumeLayout(false);
        }

        private Panel BuildHeader()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 66,
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
                Text = "QUẢN LÝ CÔNG THỨC MÓN ĂN",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_PURPLE,
                AutoSize = true,
                Location = new Point(24, 19)
            };

            btnRefresh = CreateButton("↻ Làm mới", C_PURPLE);
            btnRefresh.Size = new Size(118, 38);
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Click += BtnRefresh_Click;

            header.Controls.Add(title);
            header.Controls.Add(btnRefresh);

            header.Resize += (s, e) =>
            {
                btnRefresh.Location = new Point(header.Width - 145, 14);
            };

            return header;
        }

        private Panel BuildMainLayout()
        {
            Panel wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG,
                Padding = new Padding(16)
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));

            layout.Controls.Add(BuildLeftCard(), 0, 0);
            layout.Controls.Add(BuildRightCard(), 1, 0);

            wrapper.Controls.Add(layout);

            return wrapper;
        }

        private Control BuildLeftCard()
        {
            Guna2Panel card = CreateCard();

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = C_WHITE
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel topPanel = BuildFoodSelectorPanel();

            dgvRecipe = CreateGrid();
            dgvRecipe.CellClick += DgvRecipe_CellClick;
            dgvRecipe.CellDoubleClick += DgvRecipe_CellDoubleClick;
            dgvRecipe.DataBindingComplete += DgvRecipe_DataBindingComplete;

            layout.Controls.Add(topPanel, 0, 0);
            layout.Controls.Add(dgvRecipe, 0, 1);

            card.Controls.Add(layout);

            return card;
        }

        private Panel BuildFoodSelectorPanel()
        {
            Panel top = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE
            };

            Label lblFood = new Label
            {
                Text = "Chọn món ăn",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                Location = new Point(16, 14),
                AutoSize = true
            };

            cboMenuItem = new ComboBox
            {
                Location = new Point(16, 38),
                Size = new Size(360, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            cboMenuItem.SelectedIndexChanged += CboMenuItem_SelectedIndexChanged;

            lblSelectedFood = new Label
            {
                Text = "Công thức hiện tại",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Location = new Point(16, 80),
                AutoSize = true
            };

            lblSummary = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                AutoSize = true
            };

            top.Controls.Add(lblFood);
            top.Controls.Add(cboMenuItem);
            top.Controls.Add(lblSelectedFood);
            top.Controls.Add(lblSummary);

            top.Resize += (s, e) =>
            {
                cboMenuItem.Width = Math.Min(420, top.Width - 32);
                lblSummary.Location = new Point(Math.Max(16, top.Width - lblSummary.Width - 18), 82);
            };

            return top;
        }

        private Control BuildRightCard()
        {
            Guna2Panel card = CreateCard();

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = C_WHITE
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel formPanel = BuildRecipeInputPanel();
            Panel ingredientHeader = BuildIngredientHeaderPanel();

            dgvIngredients = CreateGrid();
            dgvIngredients.CellDoubleClick += DgvIngredients_CellDoubleClick;
            dgvIngredients.DataBindingComplete += DgvIngredients_DataBindingComplete;

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(ingredientHeader, 0, 1);
            layout.Controls.Add(dgvIngredients, 0, 2);

            card.Controls.Add(layout);

            return card;
        }

        private Panel BuildRecipeInputPanel()
        {
            Panel formPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE
            };

            Label title = new Label
            {
                Text = "Thêm / sửa nguyên liệu trong công thức",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Location = new Point(16, 14),
                AutoSize = true
            };

            Label lblIngredient = new Label
            {
                Text = "Nguyên liệu",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                Location = new Point(16, 54),
                AutoSize = true
            };

            cboIngredient = new ComboBox
            {
                Location = new Point(16, 76),
                Size = new Size(320, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            Label lblQty = new Label
            {
                Text = "Định lượng / 1 món",
                Font = new Font("Segoe UI", 9F),
                ForeColor = C_MUTED,
                Location = new Point(16, 116),
                AutoSize = true
            };

            txtQuantity = new Guna2TextBox
            {
                Location = new Point(16, 138),
                Size = new Size(180, 38),
                BorderRadius = 8,
                BorderColor = C_BORDER,
                FillColor = C_BG,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "VD: 0.25"
            };

            txtQuantity.FocusedState.BorderColor = C_PURPLE;
            txtQuantity.HoverState.BorderColor = C_PURPLE;

            btnAdd = CreateButton("+ Thêm", C_GREEN);
            btnUpdate = CreateButton("Cập nhật", C_PURPLE);
            btnDelete = CreateButton("Xóa", C_RED);
            btnClear = CreateButton("Làm trống", C_AMBER);

            btnAdd.Size = new Size(95, 36);
            btnUpdate.Size = new Size(105, 36);
            btnDelete.Size = new Size(80, 36);
            btnClear.Size = new Size(100, 36);

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClear.Click += BtnClear_Click;

            formPanel.Controls.Add(title);
            formPanel.Controls.Add(lblIngredient);
            formPanel.Controls.Add(cboIngredient);
            formPanel.Controls.Add(lblQty);
            formPanel.Controls.Add(txtQuantity);
            formPanel.Controls.Add(btnAdd);
            formPanel.Controls.Add(btnUpdate);
            formPanel.Controls.Add(btnDelete);
            formPanel.Controls.Add(btnClear);

            formPanel.Resize += (s, e) =>
            {
                cboIngredient.Width = formPanel.Width - 32;

                int y = 194;
                int gap = 8;

                btnAdd.Location = new Point(16, y);
                btnUpdate.Location = new Point(btnAdd.Right + gap, y);
                btnDelete.Location = new Point(btnUpdate.Right + gap, y);
                btnClear.Location = new Point(btnDelete.Right + gap, y);

                if (btnClear.Right > formPanel.Width - 12)
                {
                    btnClear.Location = new Point(16, y + 42);
                }
            };

            return formPanel;
        }

        private Panel BuildIngredientHeaderPanel()
        {
            Panel ingredientHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_WHITE
            };

            Label lblIngTitle = new Label
            {
                Text = "Danh sách nguyên liệu",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = C_TEXT,
                Location = new Point(16, 10),
                AutoSize = true
            };

            txtSearchIngredient = new Guna2TextBox
            {
                PlaceholderText = "Tìm nguyên liệu...",
                Font = new Font("Segoe UI", 9F),
                FillColor = C_BG,
                BorderColor = C_BORDER,
                BorderRadius = 8,
                Size = new Size(210, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            txtSearchIngredient.FocusedState.BorderColor = C_PURPLE;
            txtSearchIngredient.HoverState.BorderColor = C_PURPLE;
            txtSearchIngredient.TextChanged += TxtSearchIngredient_TextChanged;

            ingredientHeader.Controls.Add(lblIngTitle);
            ingredientHeader.Controls.Add(txtSearchIngredient);

            ingredientHeader.Resize += (s, e) =>
            {
                txtSearchIngredient.Location = new Point(ingredientHeader.Width - 226, 12);
            };

            return ingredientHeader;
        }

        private Guna2Panel CreateCard()
        {
            return new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = C_WHITE,
                BorderRadius = 12,
                Margin = new Padding(6),
                Padding = new Padding(0)
            };
        }

        private Guna2DataGridView CreateGrid()
        {
            Guna2DataGridView grid = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = C_WHITE,
                BorderStyle = BorderStyle.None,
                GridColor = C_BORDER,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Cursor = Cursors.Hand
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = C_MUTED;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            grid.DefaultCellStyle.BackColor = C_WHITE;
            grid.DefaultCellStyle.ForeColor = C_TEXT;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            grid.DefaultCellStyle.SelectionBackColor = C_PURPLE_SOFT;
            grid.DefaultCellStyle.SelectionForeColor = C_TEXT;
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            grid.RowTemplate.Height = 38;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            return grid;
        }

        private Guna2Button CreateButton(string text, Color color)
        {
            return new Guna2Button
            {
                Text = text,
                FillColor = color,
                ForeColor = Color.White,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // ============================================================
        // DATA LOAD
        // ============================================================
        private void LoadMenuItems()
        {
            try
            {
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_MenuItem_GetAllLite", null);

                cboMenuItem.DataSource = dt;
                cboMenuItem.DisplayMember = "ItemName";
                cboMenuItem.ValueMember = "MenuItemID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách món ăn: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadIngredients()
        {
            try
            {
                DataTable dt = DataHelper.ExecuteSPGetTable("sp_Ingredient_GetAll", null);

                cboIngredient.DataSource = dt.Copy();
                cboIngredient.DisplayMember = "IngredientName";
                cboIngredient.ValueMember = "IngredientID";

                dgvIngredients.DataSource = dt;

                FormatIngredientGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải nguyên liệu: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadRecipeBySelectedMenu()
        {
            int menuItemID = GetSelectedMenuItemID();

            if (menuItemID <= 0)
            {
                if (dgvRecipe != null)
                    dgvRecipe.DataSource = null;

                UpdateSelectedFoodText(0);
                return;
            }

            try
            {
                string query = @"
                    SELECT
                        r.RecipeID,
                        r.MenuItemID,
                        m.ItemName,
                        r.IngredientID,
                        i.IngredientName,
                        i.Unit,
                        r.Quantity,
                        ISNULL(i.StockQuantity, 0) AS StockQuantity,
                        ISNULL(i.MinStock, 0) AS MinStock
                    FROM dbo.Recipe r
                    INNER JOIN dbo.MenuItem m ON r.MenuItemID = m.MenuItemID
                    INNER JOIN dbo.Ingredient i ON r.IngredientID = i.IngredientID
                    WHERE r.MenuItemID = " + menuItemID + @"
                    ORDER BY i.IngredientName;
                ";

                DataTable dt = DataHelper.ExecuteQuery(query);

                dgvRecipe.DataSource = dt;

                FormatRecipeGrid();
                UpdateSelectedFoodText(dt.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải công thức: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // EVENTS
        // ============================================================
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadMenuItems();
            LoadIngredients();
            LoadRecipeBySelectedMenu();
        }

        private void CboMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearInput(false);
            LoadRecipeBySelectedMenu();
        }

        private void TxtSearchIngredient_TextChanged(object sender, EventArgs e)
        {
            FilterIngredientGrid();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInput(true);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int menuItemID = GetSelectedMenuItemID();
                int ingredientID = GetSelectedIngredientID();
                decimal quantity = ReadQuantity();

                if (menuItemID <= 0)
                    throw new Exception("Vui lòng chọn món ăn.");

                if (ingredientID <= 0)
                    throw new Exception("Vui lòng chọn nguyên liệu.");

                if (quantity <= 0)
                    throw new Exception("Định lượng phải lớn hơn 0.");

                if (RecipeExists(menuItemID, ingredientID))
                {
                    DialogResult confirm = MessageBox.Show(
                        "Nguyên liệu này đã có trong công thức. Bạn muốn cập nhật định lượng mới không?",
                        "Nguyên liệu đã tồn tại",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm != DialogResult.Yes)
                        return;

                    string updateExisting = @"
                        UPDATE dbo.Recipe
                        SET Quantity = " + SqlDecimal(quantity) + @"
                        WHERE MenuItemID = " + menuItemID + @"
                          AND IngredientID = " + ingredientID;

                    DataHelper.ExecuteNonQuery(updateExisting);
                }
                else
                {
                    string insert = @"
                        INSERT INTO dbo.Recipe(MenuItemID, IngredientID, Quantity)
                        VALUES(" + menuItemID + ", " + ingredientID + ", " + SqlDecimal(quantity) + ")";

                    DataHelper.ExecuteNonQuery(insert);
                }

                MessageBox.Show(
                    "Lưu công thức thành công.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRecipeID <= 0)
                    throw new Exception("Vui lòng chọn dòng công thức cần cập nhật.");

                int ingredientID = GetSelectedIngredientID();
                decimal quantity = ReadQuantity();

                if (ingredientID <= 0)
                    throw new Exception("Vui lòng chọn nguyên liệu.");

                if (quantity <= 0)
                    throw new Exception("Định lượng phải lớn hơn 0.");

                string query = @"
                    UPDATE dbo.Recipe
                    SET IngredientID = " + ingredientID + @",
                        Quantity = " + SqlDecimal(quantity) + @"
                    WHERE RecipeID = " + selectedRecipeID;

                DataHelper.ExecuteNonQuery(query);

                MessageBox.Show(
                    "Cập nhật công thức thành công.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRecipeID <= 0)
                    throw new Exception("Vui lòng chọn dòng công thức cần xóa.");

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc muốn xóa nguyên liệu này khỏi công thức?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                string query = "DELETE FROM dbo.Recipe WHERE RecipeID = " + selectedRecipeID;

                DataHelper.ExecuteNonQuery(query);

                MessageBox.Show(
                    "Xóa khỏi công thức thành công.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearInput(false);
                LoadRecipeBySelectedMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DgvRecipe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            FillInputFromRecipeRow(e.RowIndex);
        }

        private void DgvRecipe_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            FillInputFromRecipeRow(e.RowIndex);
        }

        private void DgvIngredients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvIngredients.Rows[e.RowIndex];

            if (row.Cells["IngredientID"].Value != DBNull.Value)
                cboIngredient.SelectedValue = Convert.ToInt32(row.Cells["IngredientID"].Value);
        }

        private void DgvRecipe_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatRecipeGrid();
        }

        private void DgvIngredients_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatIngredientGrid();
        }

        // ============================================================
        // CRUD HELPERS
        // ============================================================
        private bool RecipeExists(int menuItemID, int ingredientID)
        {
            string query = @"
                SELECT RecipeID
                FROM dbo.Recipe
                WHERE MenuItemID = " + menuItemID + @"
                  AND IngredientID = " + ingredientID;

            DataTable dt = DataHelper.ExecuteQuery(query);

            return dt != null && dt.Rows.Count > 0;
        }

        private void FillInputFromRecipeRow(int rowIndex)
        {
            DataGridViewRow row = dgvRecipe.Rows[rowIndex];

            selectedRecipeID = Convert.ToInt32(row.Cells["RecipeID"].Value);

            if (row.Cells["IngredientID"].Value != DBNull.Value)
                cboIngredient.SelectedValue = Convert.ToInt32(row.Cells["IngredientID"].Value);

            if (row.Cells["Quantity"].Value != DBNull.Value)
            {
                txtQuantity.Text = Convert.ToDecimal(row.Cells["Quantity"].Value)
                    .ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        // ============================================================
        // FORMAT GRID
        // ============================================================
        private void FormatRecipeGrid()
        {
            if (dgvRecipe == null || dgvRecipe.Columns.Count == 0)
                return;

            HideColumn(dgvRecipe, "RecipeID");
            HideColumn(dgvRecipe, "MenuItemID");
            HideColumn(dgvRecipe, "IngredientID");

            SetHeader(dgvRecipe, "ItemName", "Món ăn");
            SetHeader(dgvRecipe, "IngredientName", "Nguyên liệu");
            SetHeader(dgvRecipe, "Unit", "Đơn vị");
            SetHeader(dgvRecipe, "Quantity", "Định lượng");
            SetHeader(dgvRecipe, "StockQuantity", "Tồn kho");
            SetHeader(dgvRecipe, "MinStock", "Tối thiểu");

            AlignRight(dgvRecipe, "Quantity");
            AlignRight(dgvRecipe, "StockQuantity");
            AlignRight(dgvRecipe, "MinStock");
        }

        private void FormatIngredientGrid()
        {
            if (dgvIngredients == null || dgvIngredients.Columns.Count == 0)
                return;

            HideColumn(dgvIngredients, "IngredientID");
            HideColumn(dgvIngredients, "IsActive");
            HideColumn(dgvIngredients, "PurchasePrice");

            SetHeader(dgvIngredients, "IngredientName", "Nguyên liệu");
            SetHeader(dgvIngredients, "Unit", "Đơn vị");
            SetHeader(dgvIngredients, "StockQuantity", "Tồn kho");
            SetHeader(dgvIngredients, "MinStock", "Tối thiểu");
            SetHeader(dgvIngredients, "StockStatus", "Trạng thái");

            AlignRight(dgvIngredients, "StockQuantity");
            AlignRight(dgvIngredients, "MinStock");
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

        private void AlignRight(DataGridView grid, string columnName)
        {
            if (grid.Columns.Contains(columnName))
            {
                grid.Columns[columnName].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }
        }

        // ============================================================
        // GENERAL HELPERS
        // ============================================================
        private int GetSelectedMenuItemID()
        {
            if (cboMenuItem == null || cboMenuItem.SelectedValue == null)
                return 0;

            if (cboMenuItem.SelectedValue is DataRowView)
                return 0;

            return Convert.ToInt32(cboMenuItem.SelectedValue);
        }

        private int GetSelectedIngredientID()
        {
            if (cboIngredient == null || cboIngredient.SelectedValue == null)
                return 0;

            if (cboIngredient.SelectedValue is DataRowView)
                return 0;

            return Convert.ToInt32(cboIngredient.SelectedValue);
        }

        private decimal ReadQuantity()
        {
            string text = txtQuantity.Text.Trim().Replace(",", ".");

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal quantity))
                throw new Exception("Định lượng không hợp lệ. Ví dụ đúng: 0.25 hoặc 1.");

            return quantity;
        }

        private string SqlDecimal(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        private void ClearInput(bool clearIngredient)
        {
            selectedRecipeID = 0;

            if (txtQuantity != null)
                txtQuantity.Clear();

            if (clearIngredient && cboIngredient != null && cboIngredient.Items.Count > 0)
                cboIngredient.SelectedIndex = 0;
        }

        private void UpdateSelectedFoodText(int recipeCount)
        {
            string foodName = cboMenuItem == null ? "" : cboMenuItem.Text;

            if (lblSelectedFood != null)
                lblSelectedFood.Text = "Công thức: " + foodName;

            if (lblSummary != null)
                lblSummary.Text = recipeCount + " nguyên liệu";
        }

        private void FilterIngredientGrid()
        {
            if (dgvIngredients == null)
                return;

            if (!(dgvIngredients.DataSource is DataTable dt))
                return;

            string keyword = txtSearchIngredient.Text.Trim().Replace("'", "''");

            if (string.IsNullOrWhiteSpace(keyword))
            {
                dt.DefaultView.RowFilter = "";
            }
            else
            {
                dt.DefaultView.RowFilter =
                    "IngredientName LIKE '%" + keyword + "%' OR Unit LIKE '%" + keyword + "%'";
            }
        }
    }
}