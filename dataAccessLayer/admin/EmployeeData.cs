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
    internal class EmployeeData
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        // Phương thức để lấy UserId lớn nhất hiện có
        private string GetMaxUserId()
        {
            string maxId = null;
            string query = "SELECT MAX(userId) FROM tbl_user WHERE userId LIKE 'E%'"; // Giả sử ID bắt đầu bằng 'E' (Employee)

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
                        Console.WriteLine("Error getting max User ID: " + ex.Message);
                        // Tùy chọn: MessageBox.Show("Error getting max User ID: " + ex.Message);
                    }
                }
            }
            return maxId;
        }

        // Phương thức để tạo ID User mới
        private string GenerateNewUserId()
        {
            string maxId = GetMaxUserId();
            string prefix = "E"; // Tiền tố ID của bạn
            int number = 0;

            if (!string.IsNullOrEmpty(maxId) && maxId.StartsWith(prefix) && maxId.Length > prefix.Length)
            {
                string numberPart = maxId.Substring(prefix.Length);
                if (int.TryParse(numberPart, out number))
                {
                    number++; // Tăng số lên 1
                }
                else
                {
                    number = 1; // Nếu không phân tích được số, bắt đầu từ 1
                }
            }
            else
            {
                number = 1; // Nếu chưa có ID nào hoặc ID không đúng định dạng, bắt đầu từ 1
            }

            return prefix + number.ToString("D4"); // "D4" để đảm bảo 4 chữ số, ví dụ E0001
        }

        // Hàm kiểm tra tính duy nhất của Username
        public bool IsUsernameUnique(string username)
        {
            string query = "SELECT COUNT(*) FROM tbl_user WHERE Username = @Username";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count == 0; // Trả về true nếu không có user nào trùng username này
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database Error checking username uniqueness: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error checking username uniqueness: " + ex.Message);
                        return false; // Coi như không duy nhất nếu có lỗi database
                    }
                }
            }
        }

        // Hàm kiểm tra tính duy nhất của Email
        public bool IsEmailUnique(string email)
        {
            string query = "SELECT COUNT(*) FROM tbl_user WHERE email = @Email";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count == 0; // Trả về true nếu không có user nào trùng email này
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database Error checking email uniqueness: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error checking email uniqueness: " + ex.Message);
                        return false; // Coi như không duy nhất nếu có lỗi database
                    }
                }
            }
        }

        // Hàm thêm nhân viên mới
        public bool AddEmployee(User user)
        {
            user.userId = GenerateNewUserId(); // Tạo ID mới

            string query = "INSERT INTO tbl_user (userId, Username, email, password, numberPhone, address, fullName) VALUES (@UserId, @Username, @email, @password, @numberPhone, @address, @fullName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user.userId);
                    command.Parameters.AddWithValue("@Username", user.UserName);
                    command.Parameters.AddWithValue("@email", user.email);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@numberPhone", user.numberPhone);
                    command.Parameters.AddWithValue("@address", user.Address);
                    command.Parameters.AddWithValue("@fullName", user.fullName);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error adding employee: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error adding employee: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error adding employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error adding employee: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Hàm lấy tất cả nhân viên
        public List<User> GetAllEmployees() // Đổi tên hàm nếu bạn dùng Employee model
        {
            List<User> employees = new List<User>();
            string query = "SELECT userId, Username, email, password, numberPhone, address, fullName FROM tbl_user";

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
                                User employee = new User
                                {
                                    userId = reader["userId"].ToString(),
                                    UserName = reader["Username"].ToString(),
                                    email = reader["email"].ToString(),
                                    Password = reader["password"].ToString(),
                                    numberPhone = reader["numberPhone"].ToString(),
                                    Address = reader["address"].ToString(),
                                    fullName = reader["fullName"].ToString()
                                };
                                employees.Add(employee);
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error getting employees: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error getting employees: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error getting employees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error getting employees: " + ex.Message);
                    }
                }
            }
            return employees;
        }

        // Hàm xóa nhân viên
        public bool DeleteEmployee(string userId) // Đổi tên hàm nếu bạn dùng Employee model
        {
            string query = "DELETE FROM tbl_user WHERE userId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Number == 547) // Mã lỗi khóa ngoại
                        {
                            MessageBox.Show("Cannot delete employee because there are related records (e.g., sales, transactions). Please delete related items first.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Database Error deleting employee: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Console.WriteLine("SQL Error deleting employee: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error deleting employee: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}