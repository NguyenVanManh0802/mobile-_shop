using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MobileShop.models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MobileShop.Ui.Employee
{
    public partial class ConfirmDetails : Form
    {
        public ConfirmDetails()
        {
            InitializeComponent();
        }
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn mua đơn hàng này không?",
                "Xác nhận mua hàng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LuuDonHang();
                this.Close();
            }
          
        }

        private void LuuDonHang()
        {
            string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                

                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    if (string.IsNullOrEmpty(txtTenKhachHang.Text))
                    {
                        MessageBox.Show("Vui lòng nhập Tên khách hàng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tran.Rollback(); // Rollback transaction vì validation thất bại
                        return; // Ngừng thực hiện hàm
                    }
                    if (string.IsNullOrEmpty(txtSDT.Text))
                    {
                        MessageBox.Show("Vui lòng nhập Số điện thoại khách hàng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tran.Rollback();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtEmail.Text)) // MỚI: Kiểm tra email không bỏ trống
                    {
                        MessageBox.Show("Vui lòng nhập Email khách hàng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tran.Rollback();
                        return;
                    }
                    Random rand = new Random();
                    // Tạo customerId
                    string customerId = "C" + DateTime.Now.Ticks.ToString().Substring(5, 5);

                    // INSERT tbl_customer
                    string queryCustomer = "INSERT INTO tbl_customer (customerId, cusName, numphone, email, address) " +
                                           "VALUES (@customerId, @name, @phone, @email, @address)";
                    SqlCommand cmdCustomer = new SqlCommand(queryCustomer, conn, tran);
                    cmdCustomer.Parameters.AddWithValue("@customerId", customerId);
                    cmdCustomer.Parameters.AddWithValue("@name", txtTenKhachHang.Text);
                    cmdCustomer.Parameters.AddWithValue("@phone", txtSDT.Text);
                    cmdCustomer.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmdCustomer.Parameters.AddWithValue("@address", txtDiaChi.Text);
                    cmdCustomer.ExecuteNonQuery();
                
                   
   

                    // Parse giá
                    float price;
                    string rawPrice = txtPrice.Text.Replace(",", "").Replace("VND", "").Trim();
                    if (!float.TryParse(rawPrice, out price))
                    {
                        MessageBox.Show("Giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string saleId = "";
                    Random rand1 = new Random();
                    bool saleIdExists = true;
                    while (saleIdExists)
                    {
                        saleId = "S" + rand.Next(0, 9999999);
                        string checkQuery = "SELECT COUNT(*) FROM tbl_sales WHERE saleId = @saleId";
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn, tran);
                        checkCmd.Parameters.AddWithValue("@saleId", saleId);
                        int count = (int)checkCmd.ExecuteScalar();
                        saleIdExists = count > 0;
                    }
                    string imeiNo=txtIMEI.Text.Trim();

                    // INSERT tbl_sales
                    string querySales = "INSERT INTO tbl_sales (saleId,imeiNo, purchaseDate,price, cusId) " +
                                        "VALUES (@saleId,@imeiNo, @purchageDate, @price, @cusId)";
                    SqlCommand cmdSales = new SqlCommand(querySales, conn, tran);
                    cmdSales.Parameters.AddWithValue("@saleId", saleId);
                    cmdSales.Parameters.AddWithValue("@imeiNo", imeiNo);
                    cmdSales.Parameters.AddWithValue("@purchageDate", DateTime.Now);
                    cmdSales.Parameters.AddWithValue("@price", price);
                    cmdSales.Parameters.AddWithValue("@cusId", customerId);
                    cmdSales.ExecuteNonQuery();

                    // INSERT tbl_transaction
                    string tranId = "TR" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                    string modelId = txtModel.Text.Trim();
                    string queryTran = "INSERT INTO tbl_transaction (tranId, modelId, quantity, date, amount, TransactionType) " +
                                       "VALUES (@tranId, @modelId, @quantity, @date, @amount, @type)";
                    SqlCommand cmdTran = new SqlCommand(queryTran, conn, tran);
                    cmdTran.Parameters.AddWithValue("@tranId", tranId);
                    cmdTran.Parameters.AddWithValue("@modelId", modelId);
                    cmdTran.Parameters.AddWithValue("@quantity", 1);
                    cmdTran.Parameters.AddWithValue("@date", DateTime.Now);
                    cmdTran.Parameters.AddWithValue("@amount", price);
                    cmdTran.Parameters.AddWithValue("@type", "Sale");
                    cmdTran.ExecuteNonQuery();


                    // UPDATE tbl_model (trừ số lượng)
                    string updateModel = "UPDATE tbl_model SET availableQty = availableQty - 1 WHERE modelId = @modelId";
                    SqlCommand cmdUpdateModel = new SqlCommand(updateModel, conn, tran);
                    cmdUpdateModel.Parameters.AddWithValue("@modelId", modelId);
                    cmdUpdateModel.ExecuteNonQuery();


                    //Update mobile status
                    string updateMobile = "UPDATE tbl_mobile SET status = @status where imeino = @imeinoId";
                    SqlCommand cmdupdateMobile = new SqlCommand(updateMobile, conn, tran);
                    cmdupdateMobile.Parameters.AddWithValue("@status", "yes");
                    cmdupdateMobile.Parameters.AddWithValue("@imeinoId", imeiNo);
                    cmdupdateMobile.ExecuteNonQuery();


                    tran.Commit();
                    MessageBox.Show("Đơn hàng lưu  thành công");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Đơn hàng lưu không thành công" ,"Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private Mobile selectedMobile;
        private string selectedModelNum;
        // Constructor nhận đối tượng Mobile từ HomePageEmployee
        public ConfirmDetails(Mobile mobile,string modelNum)
        {
            InitializeComponent();
            selectedModelNum = modelNum.Trim();
            selectedMobile = mobile;
        }

        private void ConfirmDetails_Load(object sender, EventArgs e)
        {
            if (selectedMobile != null)
            {
                txtTenSanPham.Text = selectedModelNum;
                txtModel.Text = selectedMobile.ModelId;
                txtIMEI.Text = selectedMobile.ImeiNo;
                txtPrice.Text = selectedMobile.Price.ToString("N0") + " VND";
                txtWarranty.Text = selectedMobile.Warranty.ToString();

                // Load hình ảnh nếu có
                if (!string.IsNullOrEmpty(selectedMobile.Image) && File.Exists(selectedMobile.Image))
                {
                    picMobileImage.Image = Image.FromFile(selectedMobile.Image);
                    picMobileImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    picMobileImage.Image = null;
                    picMobileImage.BackColor = Color.Gray;
                }

                // Các thông tin Khách hàng để trống cho người dùng nhập
                txtTenKhachHang.Text = "";
                txtSDT.Text = "";
                txtEmail.Text = "";
                txtDiaChi.Text = "";
                
            }

        }

        private void LoadMobileData()
        {
            if (selectedMobile != null)
            {
                txtIMEI.Text = selectedMobile.ImeiNo;
                txtModel.Text = selectedMobile.ModelId;
                txtPrice.Text = $"{selectedMobile.Price:N0} VND";

                // Nếu Warranty từ DB có thì hiển thị, không thì để trống
                txtWarranty.Text = selectedMobile.Warranty.ToString() ?? "";

                // Ảnh
                if (!string.IsNullOrEmpty(selectedMobile.Image) && File.Exists(selectedMobile.Image))
                {
                    picMobileImage.Image = Image.FromFile(selectedMobile.Image);
                    picMobileImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    picMobileImage.Image = null;
                    picMobileImage.BackColor = Color.Gray;
                }

                // ⚠️ Các ô khác như SDT, Gmail, Địa chỉ thì để trống
                txtSDT.Text = "";
                txtEmail.Text = "";
                txtDiaChi.Text = "";
            }
        }

        // Các sự kiện click hiện đang trống, có thể bỏ hoặc để lại nếu cần xử lý sau
        private void lbtSoDienThoai_Click(object sender, EventArgs e) { }
        private void lblGmail_Click(object sender, EventArgs e) { }
        private void lblDiaChi_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void blSanPham_Click(object sender, EventArgs e) { }

        private void guna2btnHuyTTDH_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2btnHuyTTDH_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
