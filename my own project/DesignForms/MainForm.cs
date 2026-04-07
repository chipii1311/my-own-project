using my_own_project.DesignForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private Button currentButton;


        public void AddControls(Form f)
        {
            ControlsPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            ControlsPanel.Controls.Add(f);
            f.Show();

        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                // 1. Trả tất cả các nút trong menu về màu mặc định
                DisableButton();

                // 2. "Bôi xanh" nút vừa được bấm
                currentButton = (Button)btnSender;
                currentButton.BackColor = Color.FromArgb(241,85,126); // Màu xanh đậm hiện đại
                currentButton.ForeColor = Color.White; // Chữ trắng cho nổi
            }
        }

        private void DisableButton()
        {
            // Duyệt qua tất cả linh kiện trong panelMenu
            foreach (Control previousBtn in pnlFunction.Controls)
            {
                // Nếu là nút bấm thì trả về màu nền tối ban đầu
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(50,55,89); // Màu tối mặc định của bạn
                    previousBtn.ForeColor = Color.Gainsboro; // Màu chữ xám nhạt
                }
            }
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            AddControls(new CategoryForm());
            
        }

        

        private void btnTable_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            AddControls(new ProductForm());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {

            ActivateButton(sender);
            AddControls(new POSForm());
        }
    }
}
