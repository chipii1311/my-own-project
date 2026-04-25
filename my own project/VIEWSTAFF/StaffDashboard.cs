
using my_own_project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using my_own_project.BLL;

namespace my_own_project.VIEWSTAFF
{
    public partial class StaffDashboard : Form
    {
        private int currentRestaurantID = 1;
        private string currentUserName = "Nguyễn Văn An";
        private DataTable tablesData;

        public StaffDashboard()
        {
            InitializeComponent();
            // ❌ XÓA tất cả dòng tạo manual pnlTables, statTables, timerClock
            // Vì Designer đã có rồi!
        }

        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                lblUser.Text = $"👤 {currentUserName} (Nhân viên)";
                timerClock.Start();
                LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khởi tạo: " + ex.Message, "Lỗi");
            }
        }

        private void LoadTables()
        {
            try
            {
                tablesData = DataHelper.ExecuteSPGetTable($"EXEC sp_DiningTable_GetAll");

                if (tablesData != null && tablesData.Rows.Count > 0)
                {
                    pnlTables.Controls.Clear();

                    int totalTables = tablesData.Rows.Count;
                    int emptyCount = tablesData.AsEnumerable().Count(r => r["Status"].ToString() == "Trống");
                    int occupiedCount = tablesData.AsEnumerable().Count(r => r["Status"].ToString() == "Đang dùng");
                    int reservedCount = tablesData.AsEnumerable().Count(r => r["Status"].ToString() == "Đã đặt");

                    statTables.Text = $"🪑 Tổng số bàn\n{totalTables}";
                    statEmpty.Text = $"● Trống\n{emptyCount}";
                    statOccupied.Text = $"● Đang sử dụng\n{occupiedCount}";
                    statReserved.Text = $"● Đã đặt trước\n{reservedCount}";

                    foreach (DataRow row in tablesData.Rows)
                    {
                        CreateTableCard(row);
                    }
                }
                else
                {
                    pnlTables.Controls.Clear();
                    statTables.Text = "🪑 Tổng số bàn\n0";
                    statEmpty.Text = "● Trống\n0";
                    statOccupied.Text = "● Đang sử dụng\n0";
                    statReserved.Text = "● Đã đặt trước\n0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi tải danh sách bàn: " + ex.Message, "Lỗi");
            }
        }

        private void CreateTableCard(DataRow tableRow)
        {
            int tableID = Convert.ToInt32(tableRow["TableID"]);
            int tableNumber = Convert.ToInt32(tableRow["TableNumber"]);
            int capacity = Convert.ToInt32(tableRow["Capacity"]);
            string status = tableRow["Status"].ToString();

            Guna.UI2.WinForms.Guna2Panel card = new Guna.UI2.WinForms.Guna2Panel();
            card.AutoRoundedCorners = true;
            card.BackColor = Color.White;
            card.BorderColor = Color.FromArgb(200, 200, 200);
            card.BorderRadius = 10;
            card.BorderThickness = 2;
            card.Padding = new Padding(15);
            card.Size = new Size(220, 140);
            card.Margin = new Padding(10);
            card.Cursor = Cursors.Hand;
            card.Tag = tableID;
            card.Click += (s, e) => OpenTableOrder(tableID, tableNumber);

            Color bgColor = status == "Trống" ? Color.FromArgb(200, 230, 201) :
                           status == "Đang dùng" ? Color.FromArgb(255, 205, 210) :
                           Color.FromArgb(255, 243, 224);
            card.FillColor = bgColor;

            Label lblTableIcon = new Label();
            lblTableIcon.Text = $"🪑 Bàn {tableNumber:D2}";
            lblTableIcon.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTableIcon.ForeColor = Color.FromArgb(50, 50, 50);
            lblTableIcon.AutoSize = true;
            lblTableIcon.Location = new Point(15, 15);
            card.Controls.Add(lblTableIcon);

            Label lblCapacity = new Label();
            lblCapacity.Text = $"{capacity} chỗ";
            lblCapacity.Font = new Font("Segoe UI", 9);
            lblCapacity.ForeColor = Color.FromArgb(100, 100, 100);
            lblCapacity.AutoSize = true;
            lblCapacity.Location = new Point(15, 45);
            card.Controls.Add(lblCapacity);

            Label lblStatus = new Label();
            lblStatus.Text = status;
            lblStatus.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblStatus.ForeColor = status == "Trống" ? Color.FromArgb(76, 175, 80) :
                                  status == "Đang dùng" ? Color.FromArgb(244, 67, 54) :
                                  Color.FromArgb(255, 152, 0);
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(15, 70);
            card.Controls.Add(lblStatus);

            if (status == "Đang dùng")
            {
                Label lblTime = new Label();
                lblTime.Text = "⏱️ 00:45";
                lblTime.Font = new Font("Segoe UI", 9);
                lblTime.ForeColor = Color.FromArgb(100, 100, 100);
                lblTime.AutoSize = true;
                lblTime.Location = new Point(15, 100);
                card.Controls.Add(lblTime);
            }

            pnlTables.Controls.Add(card);
        }

        private void OpenTableOrder(int tableID, int tableNumber)
        {
            try
            {
                StaffOrderForm orderForm = new StaffOrderForm(tableID, tableNumber);
                orderForm.ShowDialog();
                LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi: " + ex.Message, "Lỗi");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void TimerClock_Tick(object sender, EventArgs e)
        {
            lblTime.Text = $"⏰ {DateTime.Now:HH:mm:ss} | {DateTime.Now:dd/MM/yyyy}";
        }
    }
}
