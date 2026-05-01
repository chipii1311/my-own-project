namespace my_own_project.VIEW
{
    // Thêm : System.Windows.Forms.Form để VS nhớ ra đây là Form
    partial class ProductForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "ProductForm";
            this.Text = "ProductForm";
            this.ResumeLayout(false);
        }

        #endregion
    }
}