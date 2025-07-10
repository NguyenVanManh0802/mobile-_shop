using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MobileShop.dataAccessLayer.employee
{
    public class SaleDAL
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        public string GenerateNewImei()
        {
            string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";
            string lastImei = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 imeino FROM tbl_sales ORDER BY saleId DESC", conn);
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lastImei = result.ToString(); // ví dụ: IMEI00025
                    int number = int.Parse(lastImei.Substring(4)); // cắt từ vị trí thứ 4 lấy số: 25
                    number++;
                    return "IMEI" + number.ToString("D5"); // IMEI00026
                }
                else
                {
                    return "IMEI00001";
                }
            }
        }
        public List<string> GetAllImeinos()
        {
            List<string> imeinoList = new List<string>();
            string query = "SELECT Imeino FROM tbl_sale";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string imeino = reader["Imeino"].ToString();
                    imeinoList.Add(imeino);
                }

                reader.Close();
            }

            return imeinoList;
        }
    }
}
