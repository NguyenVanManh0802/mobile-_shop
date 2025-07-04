using MobileShop.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // Thêm dòng này để sử dụng MessageBox

namespace MobileShop.dataAccessLayer.admin
{
    internal class companyData
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        private string GetMaxCompanyId()
        {
            string maxId = null;
            string query = "SELECT MAX(comId) FROM tbl_company WHERE comId LIKE 'C%'"; // Giả sử ID bắt đầu bằng 'C'

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
                        Console.WriteLine("Error getting max Company ID: " + ex.Message);
                        // Xử lý lỗi, có thể throw lại hoặc trả về null/empty string
                    }
                }
            }
            return maxId;
        }

        // Phương thức để tạo ID công ty mới
        private string GenerateNewCompanyId()
        {
            string maxId = GetMaxCompanyId();
            string prefix = "C"; // Tiền tố ID của bạn
            int number = 0;

            if (!string.IsNullOrEmpty(maxId) && maxId.StartsWith(prefix) && maxId.Length > prefix.Length)
            {
                // Cố gắng phân tích phần số từ ID lớn nhất
                string numberPart = maxId.Substring(prefix.Length);
                if (int.TryParse(numberPart, out number))
                {
                    number++; // Tăng số lên 1
                }
                else
                {
                    // Nếu không phân tích được số, bắt đầu từ 1
                    number = 1;
                }
            }
            else
            {
                // Nếu chưa có ID nào hoặc ID không đúng định dạng, bắt đầu từ 1
                number = 1;
            }

            // Định dạng số thành chuỗi với độ dài cố định (ví dụ: C001, C010, C100)
            return prefix + number.ToString("D3"); // "D3" để đảm bảo 3 chữ số, ví dụ 1 -> 001
        }

        public bool AddCompany(Company company)
        {
            // Tạo ID mới trước khi insert
            company.CompId = GenerateNewCompanyId();

            // Đảm bảo truy vấn SQL của bạn bao gồm cột comId và giá trị mới tạo
            string query = "INSERT INTO tbl_company (comId, comName) VALUES (@CompanyId, @CompanyName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", company.CompId); // Gán giá trị ID mới
                    command.Parameters.AddWithValue("@CompanyName", company.CName);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error adding company: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error adding company: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error adding company: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error adding company: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Hàm để lấy tất cả các công ty
        public List<Company> GetAllCompanies()
        {
            List<Company> companies = new List<Company>();
            string query = "SELECT comId, comName FROM tbl_company";

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
                                Company company = new Company
                                {
                                    CompId = reader["comId"].ToString(),
                                    CName = reader["comName"].ToString()
                                };
                                companies.Add(company);
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error getting companies: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error getting companies: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error getting companies: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error getting companies: " + ex.Message);
                    }
                }
            }
            return companies;
        }

        // Hàm MỚI để xóa công ty
        public bool DeleteCompany(string comId)
        {
            // Kiểm tra xem có bất kỳ bản ghi nào trong bảng liên quan (ví dụ: tbl_mobile, tbl_model)
            // đang tham chiếu đến comId này không. Nếu có, SQL Server sẽ báo lỗi khóa ngoại.
            // Bạn có thể thêm kiểm tra ở đây hoặc xử lý lỗi khóa ngoại.
            // Ví dụ:
            // string checkQuery = "SELECT COUNT(*) FROM tbl_mobile WHERE comId = @CompanyId";
            // ... (thực hiện truy vấn kiểm tra)

            string query = "DELETE FROM tbl_company WHERE comId = @CompanyId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", comId);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        // Đặc biệt xử lý lỗi khóa ngoại nếu có
                        if (sqlEx.Number == 547) // Mã lỗi khóa ngoại trong SQL Server
                        {
                            MessageBox.Show("Cannot delete company because it is associated with existing mobile models or products. Please delete related items first.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Database Error deleting company: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Console.WriteLine("SQL Error deleting company: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error deleting company: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error deleting company: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}