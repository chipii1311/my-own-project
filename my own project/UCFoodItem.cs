using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms; 

namespace my_own_project // Nhớ đổi đúng namespace của bạn nhé
{
    public partial class UCFoodItem : UserControl
    {
        public int FoodID { get; private set; }
        public string FoodName { get; private set; }
        public decimal Price { get; private set; }

        public event EventHandler OnSelect;

        private Guna2Panel cardPanel;
        private PictureBox picFood;
        private Label lblName;
        private Label lblPrice;
        private Guna2CircleButton btnAdd;

        public UCFoodItem()
        {
            // Bỏ qua InitializeComponent() vì ta tự code giao diện
            InitializeModernCard();
        }

        private void InitializeModernCard()
        {
            // Kích thước thẻ và khoảng cách (Margin giúp các thẻ không bị dính vào nhau)
            this.Size = new Size(180, 240);
            this.BackColor = Color.Transparent;
            this.Margin = new Padding(12);

            // Tấm nền màu trắng bo góc, đổ bóng
            cardPanel = new Guna2Panel();
            cardPanel.Dock = DockStyle.Fill;
            cardPanel.BorderRadius = 20;
            cardPanel.FillColor = Color.White;
            cardPanel.ShadowDecoration.Enabled = true;
            cardPanel.ShadowDecoration.Depth = 10; // Bóng mờ cực xịn
            cardPanel.ShadowDecoration.BorderRadius = 20;

            // Ảnh món ăn (Canh giữa bên trên)
            picFood = new PictureBox();
            picFood.Size = new Size(140, 120);
            picFood.Location = new Point(20, 15);
            picFood.SizeMode = PictureBoxSizeMode.Zoom;
            cardPanel.Controls.Add(picFood);

            // Tên món ăn
            lblName = new Label();
            lblName.Font = new Font("Segoe UI Semibold", 11F);
            lblName.Location = new Point(15, 145);
            lblName.Size = new Size(150, 45); // Để cao xíu lỡ tên món dài rớt dòng
            cardPanel.Controls.Add(lblName);

            // Giá tiền
            lblPrice = new Label();
            lblPrice.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPrice.Location = new Point(15, 195);
            lblPrice.AutoSize = true;
            cardPanel.Controls.Add(lblPrice);

            // Nút Dấu Cộng (+) tròn màu đen y hệt bản thiết kế
            btnAdd = new Guna2CircleButton();
            btnAdd.Size = new Size(35, 35);
            btnAdd.Location = new Point(130, 190);
            btnAdd.FillColor = Color.FromArgb(30, 30, 30); // Đen nhám
            btnAdd.ForeColor = Color.White;
            btnAdd.Text = "+";
            btnAdd.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Animated = true;
            btnAdd.HoverState.FillColor = Color.FromArgb(88, 28, 230); // Hover biến thành màu tím

            // Khi click dấu + thì gọi sự kiện báo ra ngoài POSForm
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
                string imagePath = System.IO.Path.Combine(Application.StartupPath, "Images", imgUrl);
                picFood.ImageLocation = imagePath;
            }
            catch { }
        }

        // --- SỬA LẠI LOGIC: KHÔNG DÙNG Ô CHỌN SỐ LƯỢNG NỮA ---
        public int GetQuantity()
        {
            // Thiết kế mới: Cứ bấm 1 cái dấu (+) là tự động ném 1 món vào giỏ hàng!
            return 1;
        }

        public void ResetQuantity()
        {
            // Hàm này giờ bỏ trống, giữ lại để không báo lỗi bên POSForm
        }
    }
}