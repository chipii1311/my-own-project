using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.IO; // Thêm thư viện này để xử lý file ảnh
using System.Windows.Forms;

namespace my_own_project
{
    public partial class UCFoodItem : UserControl
    {
        public int FoodID { get; private set; }
        public string FoodName { get; private set; }
        public decimal Price { get; private set; }

        public event EventHandler OnSelect;

        private Guna2Panel cardPanel;
        private Guna2PictureBox picFood; // Đã lên đời Guna2PictureBox xịn sò
        private Label lblName;
        private Label lblPrice;
        private Guna2CircleButton btnAdd;

        public UCFoodItem()
        {
            InitializeModernCard();
        }

        private void InitializeModernCard()
        {
            this.Size = new Size(180, 240);
            this.BackColor = Color.Transparent;
            this.Margin = new Padding(12);

            cardPanel = new Guna2Panel();
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.BorderRadius = 20;
            cardPanel.FillColor = Color.White;
            cardPanel.ShadowDecoration.Enabled = false;
            cardPanel.ShadowDecoration.Depth = 10;
            cardPanel.ShadowDecoration.BorderRadius = 20;
            cardPanel.BackColor = Color.Transparent;

            // ==========================================
            // ẢNH MÓN ĂN (Dùng Guna2PictureBox)
            // ==========================================
            picFood = new Guna2PictureBox();
            picFood.Size = new Size(140, 120);
            picFood.Location = new Point(20, 15);
            picFood.SizeMode = PictureBoxSizeMode.Zoom;
            picFood.BorderRadius = 15; // Bo góc ảnh cho hiện đại
            picFood.ErrorImage = null;   // Bùa chống X đỏ
            picFood.InitialImage = null; // Bùa chống X đỏ
            cardPanel.Controls.Add(picFood);

            // ==========================================
            // TÊN VÀ GIÁ
            // ==========================================
            lblName = new Label();
            lblName.Font = new Font("Segoe UI Semibold", 11F);
            lblName.Location = new Point(15, 145);
            lblName.Size = new Size(150, 45);
            cardPanel.Controls.Add(lblName);

            lblPrice = new Label();
            lblPrice.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPrice.Location = new Point(15, 195);
            lblPrice.AutoSize = true;
            cardPanel.Controls.Add(lblPrice);

            // ==========================================
            // NÚT CỘNG MÀU ĐEN
            // ==========================================
            btnAdd = new Guna2CircleButton();
            btnAdd.Size = new Size(35, 35);
            btnAdd.Location = new Point(130, 190);
            btnAdd.FillColor = Color.FromArgb(30, 30, 30);
            btnAdd.ForeColor = Color.White;
            btnAdd.Text = "+";
            btnAdd.Font = new Font("Arial", 16F, FontStyle.Bold);
            btnAdd.TextOffset = new Point(1, -2); // Căn giữa dấu cộng
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Animated = true;
            btnAdd.HoverState.FillColor = Color.FromArgb(88, 28, 230);

            btnAdd.Click += (s, e) => { OnSelect?.Invoke(this, EventArgs.Empty); };
            cardPanel.Controls.Add(btnAdd);

            this.Controls.Add(cardPanel);
        }

        public void SetData(int id, string name, decimal price, string imgUrl)
        {
            FoodID = id;
            FoodName = name;
            Price = price;

            lblName.Text = name;
            lblPrice.Text = price.ToString("N0") + "đ";

            try
            {
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    string imagePath = Path.Combine(Application.StartupPath, "MenuImages", imgUrl);
                    if (File.Exists(imagePath))
                    {
                        picFood.ImageLocation = imagePath; // Guna2PictureBox load cực mượt
                    }
                    else
                    {
                        picFood.ImageLocation = null;
                        picFood.Image = null;
                    }
                }
                else
                {
                    picFood.Image = null;
                }
            }
            catch { }
        }

        public int GetQuantity() { return 1; }
        public void ResetQuantity() { }
    }
}