using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using my_own_project.BLL;
using my_own_project.DTO;

namespace my_own_project.VIEW
{
    // Đã đổi từ : SampleView thành : Form
    public partial class frmTable : Form
    {
        private List<DiningTableDTO> listAllTables = new List<DiningTableDTO>();

        public frmTable()
        {
            InitializeComponent();
        }

        private void frmTable_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho các bộ lọc Guna2
            if (cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;
            if (cbCapacity.Items.Count > 0) cbCapacity.SelectedIndex = 0;

            LoadData();
        }

        public void LoadData()
        {
            DataTable dt = DiningTableBLL.GetAllTables();
            listAllTables.Clear();

            foreach (DataRow row in dt.Rows)
            {
                DiningTableDTO table = new DiningTableDTO
                {
                    TableID = Convert.ToInt32(row["TableID"]),
                    TableNumber = Convert.ToInt32(row["TableNumber"]),
                    Capacity = Convert.ToInt32(row["Capacity"]),
                    Status = row["Status"].ToString()
                };
                listAllTables.Add(table);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string status = cbStatus.SelectedItem?.ToString() ?? "All Status";
            string capacityStr = cbCapacity.SelectedItem?.ToString() ?? "All Capacity";
            string search = txtSearch.Text.ToLower();

            var filtered = listAllTables.Where(t =>
                (status == "All Status" || t.Status == status) &&
                (capacityStr == "All Capacity" || (t.Capacity + " Person") == capacityStr) &&
                (string.IsNullOrEmpty(search) ||
                 t.TableNumber.ToString().Contains(search) ||
                 ("t-" + t.TableNumber.ToString("D2")).Contains(search))
            ).ToList();

            DisplayTables(filtered);
        }

        private void DisplayTables(List<DiningTableDTO> tables)
        {
            flpTables.Controls.Clear();

            foreach (var t in tables)
            {
                UCTable uc = new UCTable();

                uc.SetTableData(t);
                uc.Margin = new Padding(15);

                uc.Click += (s, e) => {
                    MessageBox.Show($"Bạn đã chọn Bàn: T-{t.TableNumber.ToString("D2")}");
                };

                flpTables.Controls.Add(uc);
            }
        }

        private void cbStatus_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cbCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}