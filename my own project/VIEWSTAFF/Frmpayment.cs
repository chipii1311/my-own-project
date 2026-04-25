using my_own_project.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project.VIEWSTAFF
{
    public class frmPayment : Form
    {
        private readonly int _tableID;
        private readonly string _tableName;
        private readonly DataTable _items;
        private readonly decimal _discountPct;
        private readonly UserDTO _user;
        private string _payMethod = "Cash";

        private Color Purple = Color.FromArgb(106, 90, 205);
        private Color Dark = Color.FromArgb(25, 23, 60);

        // Controls
        private System.Windows.Forms.Label lblTableTitle;
        private System.Windows.Forms.Label lblInvoiceCode, lblInvoiceCodeVal;
        private System.Windows.Forms.Label lblTable, lblTableVal;
        private System.Windows.Forms.Label lblItemCount, lblItemCountVal;
        private System.Windows.Forms.Label lblTime, lblTimeVal;
        private System.Windows.Forms.Label lblStaff, lblStaffVal;
        private System.Windows.Forms.Label lblSubTotal, lblSubTotalVal;
        private System.Windows.Forms.Label lblDiscount, lblDiscountVal;
        private System.Windows.Forms.Label lblServiceFee, lblServiceFeeVal;
        private System.Windows.Forms.Label lblGrandTotal, lblGrandTotalVal;
        private System.Windows.Forms.Label lblCustomerPay, lblChangeMoney;
        private Guna.UI2.WinForms.Guna2TextBox txtCustomerPay;
        private Guna.UI2.WinForms.Guna2Button btnCash, btnCard, btnEWallet, btnTransfer;
        private Guna.UI2.WinForms.Guna2Button btnCancel, btnConfirm;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmPayment
            // 
            this.ClientSize = new System.Drawing.Size(839, 356);
            this.Name = "frmPayment";
            this.ResumeLayout(false);

        }

        public frmPayment(int tableID, string tableName, DataTable items,
                          decimal discountPct, UserDTO user)
        {
            _tableID = tableID;
            _tableName = tableName;
            _items = items;
            _discountPct = discountPct;
            _user = user;
            InitUI();
            FillInfo();
        }

        private void InitUI()
        {
            this.Text = "Thanh toán";
            this.Size = new Size(780, 680);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f);

            lblTableTitle = MakeLabel($"THANH TOÁN - {_tableName}", 24, 20, Dark, new Font("Segoe UI Semibold", 14f, FontStyle.Bold));

            var sepH = new Panel { BackColor = Color.FromArgb(230, 230, 240), Location = new Point(24, 54), Size = new Size(720, 1) };

            // ── Left: Invoice info ─────────────────────
            var pnlLeft = new Panel { BackColor = Color.FromArgb(248, 248, 252), Location = new Point(24, 64), Size = new Size(340, 300) };
            pnlLeft.BorderStyle = BorderStyle.FixedSingle;

            var lblInfoTitle = MakeLabel("THÔNG TIN HÓA ĐƠN", 12, 12, Color.FromArgb(80, 80, 110), new Font("Segoe UI Semibold", 9f, FontStyle.Bold));
            AddInfoRow(pnlLeft, "Mã hóa đơn:", out lblInvoiceCode, out lblInvoiceCodeVal, 40);
            AddInfoRow(pnlLeft, "Bàn:", out lblTable, out lblTableVal, 70);
            AddInfoRow(pnlLeft, "Số món:", out lblItemCount, out lblItemCountVal, 100);
            AddInfoRow(pnlLeft, "Giờ vào:", out lblTime, out lblTimeVal, 130);
            AddInfoRow(pnlLeft, "Nhân viên:", out lblStaff, out lblStaffVal, 160);
            pnlLeft.Controls.Add(lblInfoTitle);

            // ── Right: Payment detail ──────────────────
            var pnlRight = new Panel { BackColor = Color.FromArgb(248, 248, 252), Location = new Point(380, 64), Size = new Size(364, 300) };
            pnlRight.BorderStyle = BorderStyle.FixedSingle;

            var lblPayTitle = MakeLabel("CHI TIẾT THANH TOÁN", 12, 12, Color.FromArgb(80, 80, 110), new Font("Segoe UI Semibold", 9f, FontStyle.Bold));
            AddPayRow(pnlRight, "Tạm tính:", out lblSubTotal, out lblSubTotalVal, 40);
            AddPayRow(pnlRight, "Giảm giá:", out lblDiscount, out lblDiscountVal, 70);
            AddPayRow(pnlRight, "Phí phục vụ:", out lblServiceFee, out lblServiceFeeVal, 100);

            var pnlTotalSep = new Panel { BackColor = Color.FromArgb(200, 200, 215), Location = new Point(8, 134), Size = new Size(348, 1) };
            lblGrandTotal = MakeLabel("Tổng thanh toán", 8, 144, Dark, new Font("Segoe UI Semibold", 11f, FontStyle.Bold));
            lblGrandTotalVal = MakeLabel("0 đ", 250, 140, Color.FromArgb(229, 57, 53), new Font("Segoe UI Semibold", 14f, FontStyle.Bold));
            lblGrandTotalVal.AutoSize = true;

            var pnlSep2 = new Panel { BackColor = Color.FromArgb(200, 200, 215), Location = new Point(8, 178), Size = new Size(348, 1) };
            lblCustomerPay = MakeLabel("Khách thanh toán", 8, 192, Color.FromArgb(80, 80, 110), new Font("Segoe UI", 9.5f));
            txtCustomerPay = new Guna.UI2.WinForms.Guna2TextBox();
            txtCustomerPay.Text = "0";
            txtCustomerPay.BorderRadius = 8;
            txtCustomerPay.FillColor = Color.White;
            txtCustomerPay.Font = new Font("Segoe UI", 10f);
            txtCustomerPay.Size = new Size(100, 30);
            txtCustomerPay.Location = new Point(220, 188);
            txtCustomerPay.TextChanged += (s, e) => CalcChange();

            var lblDong = MakeLabel("đ", 326, 196, Color.FromArgb(120, 120, 150), new Font("Segoe UI", 9f));
            lblChangeMoney = MakeLabel("Tiền thừa:  0 đ", 8, 228, Color.FromArgb(76, 175, 80), new Font("Segoe UI", 9.5f));

            pnlRight.Controls.AddRange(new Control[] {
                lblPayTitle, pnlTotalSep, lblGrandTotal, lblGrandTotalVal,
                pnlSep2, lblCustomerPay, txtCustomerPay, lblDong, lblChangeMoney
            });

            // ── Payment methods ─────────────────────────
            var lblMethodTitle = MakeLabel("PHƯƠNG THỨC THANH TOÁN", 24, 382, Color.FromArgb(80, 80, 110), new Font("Segoe UI Semibold", 9f, FontStyle.Bold));
            btnCash = MakeMethodBtn("💰\nTiền mặt", 24, 408, "Cash");
            btnCard = MakeMethodBtn("💳\nThẻ ngân hàng", 138, 408, "Card");
            btnEWallet = MakeMethodBtn("📱\nVí điện tử", 252, 408, "EWallet");
            btnTransfer = MakeMethodBtn("🏦\nChuyển khoản", 366, 408, "Transfer");

            btnCash.FillColor = Purple; btnCash.ForeColor = Color.White; // default

            btnCash.Click += (s, e) => SelectMethod("Cash", btnCash);
            btnCard.Click += (s, e) => SelectMethod("Card", btnCard);
            btnEWallet.Click += (s, e) => SelectMethod("EWallet", btnEWallet);
            btnTransfer.Click += (s, e) => SelectMethod("Transfer", btnTransfer);

            // ── Action buttons ──────────────────────────
            var pnlSepBottom = new Panel { BackColor = Color.FromArgb(230, 230, 240), Location = new Point(0, 560), Size = new Size(780, 1) };

            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            btnCancel.Text = "Hủy";
            btnCancel.BorderRadius = 10;
            btnCancel.FillColor = Color.FromArgb(245, 246, 250);
            btnCancel.ForeColor = Dark;
            btnCancel.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            btnCancel.Size = new Size(160, 48);
            btnCancel.Location = new Point(280, 580);
            btnCancel.Click += (s, e) => this.Close();

            btnConfirm = new Guna.UI2.WinForms.Guna2Button();
            btnConfirm.Text = "✓  Xác nhận thanh toán";
            btnConfirm.BorderRadius = 10;
            btnConfirm.FillColor = Purple;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            btnConfirm.Size = new Size(260, 48);
            btnConfirm.Location = new Point(454, 580);
            btnConfirm.Click += BtnConfirm_Click;

            this.Controls.AddRange(new Control[] {
                lblTableTitle, sepH,
                pnlLeft, pnlRight,
                lblMethodTitle,
                btnCash, btnCard, btnEWallet, btnTransfer,
                pnlSepBottom, btnCancel, btnConfirm
            });
        }

        private void FillInfo()
        {
            lblTableVal.Text = _tableName;
            lblItemCountVal.Text = $"{_orderItems().Rows.Count} món";
            lblTimeVal.Text = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
            lblStaffVal.Text = _user?.FullName ?? "—";
            lblInvoiceCodeVal.Text = $"HD{DateTime.Now:yyMMddHHmmss}";

            decimal sub = CalcSubTotal();
            decimal disc = sub * _discountPct / 100;
            decimal total = sub - disc;

            lblSubTotalVal.Text = $"{sub:N0} đ";
            lblDiscountVal.Text = $"{disc:N0} đ";
            lblServiceFeeVal.Text = "0 đ";
            lblGrandTotalVal.Text = $"{total:N0} đ";
            txtCustomerPay.Text = total.ToString("N0");
        }

        private DataTable _orderItems() => _items;

        private decimal CalcSubTotal()
        {
            decimal sub = 0;
            foreach (DataRow r in _items.Rows)
                sub += Convert.ToDecimal(r["Price"]) * Convert.ToInt32(r["Qty"]);
            return sub;
        }

        private void CalcChange()
        {
            if (!decimal.TryParse(txtCustomerPay.Text.Replace(",", ""), out decimal paid)) return;
            decimal sub = CalcSubTotal();
            decimal disc = sub * _discountPct / 100;
            decimal total = sub - disc;
            decimal change = paid - total;
            lblChangeMoney.Text = $"Tiền thừa:  {Math.Max(change, 0):N0} đ";
            lblChangeMoney.ForeColor = change >= 0 ? Color.FromArgb(76, 175, 80) : Color.FromArgb(229, 57, 53);
        }

        private void SelectMethod(string method, Guna.UI2.WinForms.Guna2Button active)
        {
            _payMethod = method;
            foreach (Control c in this.Controls)
            {
                if (c is Guna.UI2.WinForms.Guna2Button b &&
                    (b == btnCash || b == btnCard || b == btnEWallet || b == btnTransfer))
                {
                    b.FillColor = b == active ? Purple : Color.FromArgb(245, 246, 250);
                    b.ForeColor = b == active ? Color.White : Dark;
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                decimal sub = CalcSubTotal();
                decimal disc = sub * _discountPct / 100;
                decimal total = sub - disc;

                // TODO: Gọi OrderBLL.CreateOrder + PaymentBLL.CreatePayment
                MessageBox.Show($"Thanh toán thành công!\nTổng: {total:N0} đ\nPhương thức: {_payMethod}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Helpers ───────────────────────────────────
        private System.Windows.Forms.Label MakeLabel(string text, int x, int y,
            Color fore, Font font)
        {
            return new System.Windows.Forms.Label
            {
                Text = text,
                Location = new Point(x, y),
                ForeColor = fore,
                Font = font,
                BackColor = Color.Transparent,
                AutoSize = true
            };
        }

        private void AddInfoRow(Panel parent, string lbl,
            out System.Windows.Forms.Label lblOut,
            out System.Windows.Forms.Label valOut, int y)
        {
            lblOut = MakeLabel(lbl, 12, y, Color.FromArgb(100, 100, 130), new Font("Segoe UI", 9.5f));
            valOut = MakeLabel("—", 180, y, Dark, new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold));
            parent.Controls.AddRange(new Control[] { lblOut, valOut });
        }

        private void AddPayRow(Panel parent, string lbl,
            out System.Windows.Forms.Label lblOut,
            out System.Windows.Forms.Label valOut, int y)
        {
            lblOut = MakeLabel(lbl, 8, y, Color.FromArgb(100, 100, 130), new Font("Segoe UI", 9.5f));
            valOut = MakeLabel("0 đ", 250, y, Dark, new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold));
            valOut.AutoSize = true;
            parent.Controls.AddRange(new Control[] { lblOut, valOut });
        }

        private Guna.UI2.WinForms.Guna2Button MakeMethodBtn(string text, int x, int y, string method)
        {
            var btn = new Guna.UI2.WinForms.Guna2Button();
            btn.Text = text;
            btn.BorderRadius = 12;
            btn.FillColor = Color.FromArgb(245, 246, 250);
            btn.ForeColor = Dark;
            btn.Font = new Font("Segoe UI", 9f);
            btn.Size = new Size(106, 90);
            btn.Location = new Point(x, y);
            btn.Tag = method;
            return btn;
        }

        // ── Declarations ──────────────────────────────
        private System.Windows.Forms.Label lblOrderItems;
    }
}
