using my_own_project.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEW
{
    public partial class PaymentForm : Form
    {

        private int currentOrderID;
        private decimal subTotal = 0;
        private decimal finalTotal = 0;

        public PaymentForm(int orderID, int tableName)
        {
            InitializeComponent();
            currentOrderID = orderID;

            // Gán thông tin lên Label
            lblOrderID.Text = "Mã Đơn: #" + currentOrderID;
            lblTableName.Text = "Bàn: " + tableName;
        }


        private void PaymentForm_Load(object sender, EventArgs e)
        {
            LoadBillDetails();
        }
        public PaymentForm()
        {
            InitializeComponent();
        }

        private void LoadBillDetails()
        {
            try
            {
                // Dùng hàm sp_OrderDetail_GetByOrderID mà chúng ta đã viết dưới SQL
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderID", currentOrderID)
                };

                DataTable dtBill = DataHelper.ExecuteSPGetTable("sp_OrderDetail_GetByOrderID", parameters);

                // Đổ dữ liệu vào DataGridView
                dgvBill.DataSource = dtBill;

                // Format lại các cột cho đẹp (Ẩn cột ID thừa, format giá tiền...)
                if (dgvBill.Columns.Contains("OrderDetailID")) dgvBill.Columns["OrderDetailID"].Visible = false;
                if (dgvBill.Columns.Contains("OrderID")) dgvBill.Columns["OrderID"].Visible = false;
                if (dgvBill.Columns.Contains("MenuItemID")) dgvBill.Columns["MenuItemID"].Visible = false;

                // Tính tổng tiền các món
                subTotal = 0;
                foreach (DataRow row in dtBill.Rows)
                {
                    subTotal += Convert.ToDecimal(row["SubTotal"]);
                }

                // Hiển thị lên màn hình (tạm thời finalTotal bằng subTotal vì chưa tính khuyến mãi)
                finalTotal = subTotal;
                lblSubTotal.Text = subTotal.ToString("N0") + " VNĐ";
                lblFinalTotal.Text = finalTotal.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết đơn hàng: " + ex.Message);
            }
        }
    }
}
