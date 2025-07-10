using MobileShop.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data; // Thêm using này
using System.Windows.Forms; // Để dùng MessageBox từ DAL

namespace MobileShop.dataAccessLayer.admin
{
    internal class ReportData
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        // Phương thức để lấy báo cáo chi tiết bán hàng
        public DataTable GetSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            DataTable salesReport = new DataTable();
            // Chuỗi truy vấn SQL JOIN các bảng để lấy thông tin chi tiết
            // SỬ DỤNG ALIAS CHO CÁC CỘT CÓ TÊN TRÙNG NHAU (ví dụ: price)
            string query = @"
                SELECT
                    ROW_NUMBER() OVER (ORDER BY S.purchaseDate) AS STT, -- Thêm cột STT
                    C.cusName AS TenKhachHang,
                    M.ModelNum AS TenSanPham,
                    S.price AS GiaBan, -- Giá từ bảng Sale
                    S.purchaseDate AS NgayMua
                FROM
                    tbl_sales AS S
                INNER JOIN
                    tbl_customer AS C ON S.cusId = C.customerId
                INNER JOIN
                    tbl_mobile AS MB ON S.imeino = MB.ImeiNo -- Lấy ModelId từ Mobile
                INNER JOIN
                    tbl_model AS M ON MB.modelId = M.ModelId
                WHERE
                    S.purchaseDate >= @DateFrom AND S.purchaseDate <= @DateTo
                ORDER BY
                    S.purchaseDate;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DateFrom", dateFrom.Date); // Chỉ lấy phần ngày
                    command.Parameters.AddWithValue("@DateTo", dateTo.Date.AddDays(1).AddSeconds(-1)); // Lấy đến cuối ngày DateTo

                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(salesReport); // Đổ dữ liệu vào DataTable
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error generating report: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error generating report: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error generating report: " + ex.Message);
                    }
                }
            }
            return salesReport;
        }

        // Bạn có thể thêm các phương thức báo cáo khác ở đây (ví dụ: Report theo Model, theo Company...)
    }
}