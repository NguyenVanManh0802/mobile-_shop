using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using MobileShop.models;

namespace MobileShop.dataAccessLayer
{
    public class MobileDAL
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        public List<Mobile> GetAllMobiles()
        {
            List<Mobile> mobiles = new List<Mobile>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // ✅ Câu truy vấn đơn giản chỉ lấy từ bảng tbl_mobile
                    string sql = "SELECT imeiNo, modelId, status, price, image FROM tbl_mobile WHERE status='no'";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        float price = 0;
                        float.TryParse(reader["price"]?.ToString(), out price);

                        Mobile mobile = new Mobile
                        {
                            ImeiNo = reader["imeiNo"]?.ToString(),
                            ModelId = reader["modelId"]?.ToString(),
                            Status = reader["status"]?.ToString(),
                            Price = price,
                            Image = reader["image"]?.ToString()
                        };
                        mobiles.Add(mobile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message + "\n" + ex.StackTrace);
            }

            return mobiles;
        }

       
    }
}