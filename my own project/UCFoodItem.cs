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
        }

        // Hàm này dùng để đổ dữ liệu từ SQL vào cái thẻ này
        public void SetData(int id, string name, decimal price, string imgPath)
        {
            this.FoodID = id;
            this.Price = price;
            lblFoodName.Text = name;
            lblPrice.Text = price.ToString("N0") + "đ";

            try
            {
                if (System.IO.File.Exists(imgPath))
                    picImage.Image = Image.FromFile(imgPath);
            }
            catch { }
        }

        // Lấy số lượng hiện tại đang chọn trên thẻ
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
