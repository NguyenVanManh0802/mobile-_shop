using MobileShop.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // Dùng để hiển thị MessageBox từ DAL khi lỗi

namespace MobileShop.dataAccessLayer.admin
{
    internal class MobileData
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        // Hàm tạo IMEI tự động tăng (tương tự Company và Model)
        private string GenerateNewImeiNo()
        {
            string maxId = null;
            // Giả sử IMEI có tiền tố 'IMEI' và theo sau là số
            string query = "SELECT MAX(ImeiNo) FROM tbl_mobile WHERE ImeiNo LIKE 'IMEI%'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            maxId = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error getting max IMEI No: " + ex.Message);
                    }
                }
            }

            string prefix = "IMEI";
            int number = 0;

            if (!string.IsNullOrEmpty(maxId) && maxId.StartsWith(prefix) && maxId.Length > prefix.Length)
            {
                string numberPart = maxId.Substring(prefix.Length);
                if (int.TryParse(numberPart, out number))
                {
                    number++;
                }
                else
                {
                    number = 1;
                }
            }
            else
            {
                number = 1;
            }
            return prefix + number.ToString("D5"); // Ví dụ: IMEI00001
        }

        // Hàm thêm Mobile mới
        public bool AddMobile(Mobile mobile)
        {
            mobile.ImeiNo = GenerateNewImeiNo(); // Tạo IMEI mới

            // Đảm bảo tên cột 'image' trùng khớp trong DB
            string query = "INSERT INTO tbl_mobile (ImeiNo, ModelId, status, price, image) VALUES (@ImeiNo, @ModelId, @Status, @Price, @Image)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImeiNo", mobile.ImeiNo);
                    command.Parameters.AddWithValue("@ModelId", mobile.ModelId);
                    command.Parameters.AddWithValue("@Status", mobile.Status);
                    command.Parameters.AddWithValue("@Price", mobile.Price);
                    // Nếu mobile.Image có thể null, dùng DBNull.Value
                    command.Parameters.AddWithValue("@Image", (object)mobile.Image ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        // ... (xử lý lỗi như đã có) ...
                        MessageBox.Show("Database Error adding mobile: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error adding mobile: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        // ... (xử lý lỗi như đã có) ...
                        MessageBox.Show("General Error adding mobile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error adding mobile: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Hàm lấy tất cả Mobile
        public List<Mobile> GetAllMobiles()
        {
            List<Mobile> mobiles = new List<Mobile>();
            // Có thể JOIN với tbl_model và tbl_company nếu bạn muốn hiển thị thông tin chi tiết
            string query = "SELECT ImeiNo, ModelId, status, price, image FROM tbl_mobile";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Mobile mobile = new Mobile
                                {
                                    ImeiNo = reader["ImeiNo"].ToString(),
                                    ModelId = reader["ModelId"].ToString(),
                                    Status = reader["status"].ToString(),
                                    Price = Convert.ToSingle(reader["price"]), // Đọc giá là float
                                    Image = reader["image"] == DBNull.Value ? null : reader["image"].ToString()
                                    // Warranty không có trong schema bạn cung cấp, nếu có thì thêm vào đây
                                    // Warranty = Convert.ToDateTime(reader["Warranty"])
                                };
                                mobiles.Add(mobile);
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error getting mobiles: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error getting mobiles: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error getting mobiles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error getting mobiles: " + ex.Message);
                    }
                }
            }
            return mobiles;
        }

        // Hàm xóa Mobile
        public bool DeleteMobile(string imeiNo)
        {
            string query = "DELETE FROM tbl_mobile WHERE ImeiNo = @ImeiNo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImeiNo", imeiNo);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Number == 547) // Mã lỗi khóa ngoại (nếu MobileId đang được tham chiếu bởi Sale/Transaction)
                        {
                            MessageBox.Show("Cannot delete mobile: It is associated with existing sales or transactions. Please delete related records first.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Database Error deleting mobile: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Console.WriteLine("SQL Error deleting mobile: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error deleting mobile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error deleting mobile: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Hàm cập nhật Mobile (nếu cần)
        public bool UpdateMobile(Mobile mobile)
        {
            string query = "UPDATE tbl_mobile SET ModelId = @ModelId, status = @Status, price = @Price, image = @Image WHERE ImeiNo = @ImeiNo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModelId", mobile.ModelId);
                    command.Parameters.AddWithValue("@Status", mobile.Status);
                    command.Parameters.AddWithValue("@Price", mobile.Price);
                    command.Parameters.AddWithValue("@Image", (object)mobile.Image ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImeiNo", mobile.ImeiNo);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error updating mobile: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error updating mobile: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error updating mobile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error updating mobile: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}