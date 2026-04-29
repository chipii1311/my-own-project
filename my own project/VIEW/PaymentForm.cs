using Guna.UI2.WinForms;
using my_own_project.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace my_own_project.VIEW // Nhớ đổi namespace cho khớp với project của bạn nhé
{
    public partial class PaymentForm : Form
    {
        private int currentOrderID;
        private decimal totalAmount = 0;

        // Các control giao diện
        private Label lblTotalAmount;
        private Guna2TextBox txtCashGiven;
        private Label lblChangeAmount;
        private Guna2Button btnConfirm;
        private Guna2Button btnCancel;
        private Guna2ShadowForm shadowForm;

        public PaymentForm(int orderID, int tableID = -1) // Tham số tableID giữ lại để không bị lỗi code cũ gọi sang
        {
            this.currentOrderID = orderID;
            InitializeModernUI();
            LoadTotalAmount();
        }

        private void InitializeModernUI()
        {
            this.Size = new Size(450, 550);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent; // Hiện giữa màn hình cha
            this.BackColor = Color.White;

            // Tạo bóng đổ cho xịn xò
            shadowForm = new Guna2ShadowForm(this);
            shadowForm.ShadowColor = Color.Black;

            // --- HEADER ---
            Guna2Panel pnlHeader = new Guna2Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.FillColor = Color.FromArgb(88, 28, 230); // Màu tím chủ đạo

            Label lblTitle = new Label();
            lblTitle.Text = "THANH TOÁN";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            pnlHeader.Controls.Add(lblTitle);

            // Nút [X] tắt form
            Guna2ControlBox btnClose = new Guna2ControlBox();
            btnClose.Dock = DockStyle.Right;
            btnClose.Size = new Size(60, 60);
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = Color.White;
            btnClose.HoverState.FillColor = Color.FromArgb(255, 71, 87);
            pnlHeader.Controls.Add(btnClose);
            this.Controls.Add(pnlHeader);

            // --- NỘI DUNG CHÍNH ---
            int currentY = 90;

            // 1. TỔNG TIỀN CẦN THANH TOÁN
            Label lblTextTotal = new Label { Text = "Tổng tiền thanh toán:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, Location = new Point(30, currentY), AutoSize = true };
            this.Controls.Add(lblTextTotal);
            currentY += 30;

            lblTotalAmount = new Label { Text = "0 đ", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 71, 87), Location = new Point(30, currentY), AutoSize = true };
            this.Controls.Add(lblTotalAmount);
            currentY += 60;

            // Đường kẻ ngang
            Guna2Panel line1 = new Guna2Panel { Size = new Size(390, 1), Location = new Point(30, currentY), FillColor = Color.FromArgb(235, 235, 235) };
            this.Controls.Add(line1);
            currentY += 30;

            // 2. KHÁCH ĐƯA (Ô NHẬP TIỀN)
            Label lblTextGiven = new Label { Text = "Tiền khách đưa (VNĐ):", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Black, Location = new Point(30, currentY), AutoSize = true };
            this.Controls.Add(lblTextGiven);
            currentY += 30;

            txtCashGiven = new Guna2TextBox();
            txtCashGiven.Size = new Size(390, 50);
            txtCashGiven.Location = new Point(30, currentY);
            txtCashGiven.BorderRadius = 10;
            txtCashGiven.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            txtCashGiven.ForeColor = Color.Black;
            txtCashGiven.TextAlign = HorizontalAlignment.Right;
            txtCashGiven.PlaceholderText = "0";
            txtCashGiven.TextChanged += TxtCashGiven_TextChanged; // Bắt sự kiện gõ chữ để tính tiền thừa
            txtCashGiven.KeyPress += TxtCashGiven_KeyPress;       // Chặn gõ chữ cái
            this.Controls.Add(txtCashGiven);
            currentY += 80;

            // 3. TIỀN THỪA
            Label lblTextChange = new Label { Text = "Tiền thừa trả khách:", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, Location = new Point(30, currentY), AutoSize = true };
            this.Controls.Add(lblTextChange);

            lblChangeAmount = new Label { Text = "0 đ", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(46, 204, 113), Location = new Point(250, currentY - 5), Size = new Size(170, 35), TextAlign = ContentAlignment.MiddleRight, AutoSize = false };
            this.Controls.Add(lblChangeAmount);
            currentY += 70;

            // --- NÚT BẤM (BOTTOM) ---
            btnCancel = new Guna2Button();
            btnCancel.Text = "Hủy bỏ";
            btnCancel.Size = new Size(180, 55);
            btnCancel.Location = new Point(30, currentY);
            btnCancel.BorderRadius = 10;
            btnCancel.FillColor = Color.FromArgb(240, 240, 240);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) => { this.Close(); };
            this.Controls.Add(btnCancel);

            btnConfirm = new Guna2Button();
            btnConfirm.Text = "Xác nhận Thanh toán";
            btnConfirm.Size = new Size(190, 55);
            btnConfirm.Location = new Point(230, currentY);
            btnConfirm.BorderRadius = 10;
            btnConfirm.FillColor = Color.FromArgb(88, 28, 230);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.Click += BtnConfirm_Click;
            this.Controls.Add(btnConfirm);

            // Gắn viền cho form cho đẹp
            Guna2BorderlessForm borderlessForm = new Guna2BorderlessForm();
            borderlessForm.ContainerControl = this;
            borderlessForm.BorderRadius = 20;
        }

        private void LoadTotalAmount()
        {
            // Tận dụng hàm cũ bên BLL để tính tổng tiền giống y hệt lúc hiển thị Bill
            DataTable dtDetails = OrderDetailBLL.GetOrderDetailsByOrderID(currentOrderID);
            totalAmount = 0;
            foreach (DataRow row in dtDetails.Rows)
            {
                totalAmount += Convert.ToDecimal(row["SubTotal"]);
            }

            lblTotalAmount.Text = totalAmount.ToString("N0") + " đ";
        }

        // Chặn không cho nhập chữ, chỉ cho nhập số và nút Backspace
        private void TxtCashGiven_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Tự động format số có dấu phẩy và tính tiền thừa ngay khi gõ
        private void TxtCashGiven_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCashGiven.Text))
            {
                lblChangeAmount.Text = "0 đ";
                lblChangeAmount.ForeColor = Color.Gray;
                return;
            }

            // Xóa dấu phẩy cũ đi để parse về số
            string rawNumber = txtCashGiven.Text.Replace(",", "");
            if (decimal.TryParse(rawNumber, out decimal cashGiven))
            {
                // Format lại có dấu phẩy cho đẹp (VD: 100000 -> 100,000)
                txtCashGiven.TextChanged -= TxtCashGiven_TextChanged; // Tạm tắt event để tránh lặp vô hạn
                txtCashGiven.Text = cashGiven.ToString("N0");
                txtCashGiven.SelectionStart = txtCashGiven.Text.Length; // Đưa con trỏ về cuối ô
                txtCashGiven.TextChanged += TxtCashGiven_TextChanged;

                // Tính tiền thừa
                decimal change = cashGiven - totalAmount;
                if (change >= 0)
                {
                    lblChangeAmount.Text = change.ToString("N0") + " đ";
                    lblChangeAmount.ForeColor = Color.FromArgb(46, 204, 113); // Đủ tiền -> Màu Xanh lá
                }
                else
                {
                    lblChangeAmount.Text = "Khách đưa thiếu!";
                    lblChangeAmount.ForeColor = Color.Red; // Thiếu tiền -> Màu Đỏ
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            string rawNumber = txtCashGiven.Text.Replace(",", "");
            decimal cashGiven = 0;
            decimal.TryParse(rawNumber, out cashGiven);

            if (cashGiven < totalAmount)
            {
                MessageBox.Show("Khách chưa đưa đủ tiền để thanh toán!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Cập nhật Order thành 'Completed'
                my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE Orders SET Status = 'Completed' WHERE OrderID = {currentOrderID}");

                // 2. Trả bàn về trạng thái 'Trống'
                my_own_project.DAL.DataHelper.ExecuteNonQuery($"UPDATE DiningTable SET Status = N'Trống' WHERE TableID = (SELECT TableID FROM Orders WHERE OrderID = {currentOrderID})");

                MessageBox.Show("Thanh toán thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Báo về cho POSForm biết là đã xong
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
            }
        }
    }
}