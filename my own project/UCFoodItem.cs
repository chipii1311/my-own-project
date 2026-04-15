using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my_own_project                
{           
    public partial class UCFoodItem : UserControl
    {
        // Tạo một sự kiện để báo cho POSForm biết khi nào nút "Thêm" được bấm
        public event EventHandler OnSelect = null;

        // Các thuộc tính để chứa dữ liệu món ăn
        public int FoodID { get; set; }
        public decimal Price { get; set; }
                
        public UCFoodItem()
        {
            InitializeComponent();
            btnBuy.Click += btnBuy_Click;
        }

        // Hàm này dùng để đổ dữ liệu từ SQL vào cái thẻ này
        public void SetData(int id, string name, decimal price, string imgFileName)
        {
            lblFoodName.Text = name;
            lblPrice.Text = price.ToString("N0") + "đ";
            this.FoodID = id;
            this.Price = price;

            try
            {
                if (!string.IsNullOrEmpty(imgFileName))
                {
                    // Tự động tìm thư mục MenuImages nằm cùng cấp với file .exe
                    string fullPath = Path.Combine(Application.StartupPath, "MenuImages", imgFileName);

                    if (File.Exists(fullPath))
                    {
                        // Giải phóng ảnh cũ nếu có để tránh tràn bộ nhớ
                        if (picImage.Image != null) picImage.Image.Dispose();

                        picImage.Image = Image.FromFile(fullPath);
                    }
                }
            }
            catch (Exception)
            {
                // Nếu lỗi hoặc không thấy ảnh, có thể hiện một ảnh mặc định
            }
        }
        public int GetQuantity()
        {
            return (int)nudQuantity.Value;
        }

        // Reset lại số lượng về 1 sau khi thêm thành công
        public void ResetQuantity()
        {
            nudQuantity.Value = 1;
        }

        // Khi bấm nút Thêm trên thẻ, nó sẽ kích hoạt sự kiện OnSelect
        private void btnBuy_Click(object sender, EventArgs e)
        {
            if (OnSelect != null)
                OnSelect(this, e);
        }


    }

}
