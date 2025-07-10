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
    internal class ModelData
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        // Phương thức để lấy ModelId lớn nhất hiện có
        private string GetMaxModelId()
        {
            string maxId = null;
            string query = "SELECT MAX(modelId) FROM tbl_model WHERE ModelId LIKE 'M%'"; // Giả sử ID bắt đầu bằng 'M'

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
                        Console.WriteLine("Error getting max Model ID: " + ex.Message);
                        // Tùy chọn: MessageBox.Show("Error getting max Model ID: " + ex.Message);
                    }
                }
            }
            return maxId;
        }
        public string GetModelNumById(string modelId)
        {
            string modelNum = null;
            string query = "SELECT modelNum FROM tbl_model WHERE modelId = @ModelId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModelId", modelId);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            modelNum = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error getting ModelNum by ID: " + ex.Message);
                        // Bạn có thể chọn hiển thị MessageBox ở đây hoặc chỉ log lỗi
                        // MessageBox.Show("Lỗi khi lấy tên Model: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return modelNum;
        }

        // Phương thức để tạo ID Model mới
        private string GenerateNewModelId()
        {
            string maxId = GetMaxModelId();
            string prefix = "M"; // Tiền tố ID của bạn
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

            return prefix + number.ToString("D3"); // "D3" để đảm bảo 3 chữ số, ví dụ 1 -> 001
        }

        // Phương thức để thêm một Model mới
        public bool AddModel(Model model)
        {
            // Tạo ModelId mới trước khi insert
            model.ModelId = GenerateNewModelId();

            string query = "INSERT INTO tbl_model (ModelId, comId, ModelNum, AvailableQty) VALUES (@ModelId, @CompId, @ModelNum, @AvailableQty)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModelId", model.ModelId);
                    command.Parameters.AddWithValue("@CompId", model.CompId);
                    command.Parameters.AddWithValue("@ModelNum", model.ModelNum);
                    command.Parameters.AddWithValue("@AvailableQty", model.AvailableQty);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        // Kiểm tra lỗi khóa ngoại nếu CompId không tồn tại
                        if (sqlEx.Number == 547) // Mã lỗi khóa ngoại trong SQL Server
                        {
                            MessageBox.Show("Cannot add model: The Company ID (CompId) does not exist.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Database Error adding model: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Console.WriteLine("SQL Error adding model: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error adding model: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error adding model: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Phương thức để lấy tất cả các Model
        public List<Model> GetAllModels()
        {
            List<Model> models = new List<Model>();
            // Có thể JOIN với tbl_company nếu bạn muốn hiển thị tên công ty thay vì chỉ CompId
            string query = "SELECT ModelId, ComId, ModelNum, AvailableQty FROM tbl_model";

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
                                Model model = new Model
                                {
                                    ModelId = reader["modelId"].ToString(),
                                    CompId = reader["comId"].ToString(),
                                    ModelNum = reader["modelNum"].ToString(),
                                    AvailableQty = Convert.ToInt32(reader["availableQty"])
                                };
                                models.Add(model);
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database Error getting models: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("SQL Error getting models: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error getting models: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error getting models: " + ex.Message);
                    }
                }
            }
            return models;
        }

        // Phương thức để xóa một Model
        public bool DeleteModel(string modelId)
        {
            string query = "DELETE FROM tbl_model WHERE ModelId = @ModelId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ModelId", modelId);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException sqlEx)
                    {
                        // Xử lý lỗi khóa ngoại nếu ModelId đang được tham chiếu bởi bảng khác (ví dụ: tbl_mobile, tbl_sale)
                        if (sqlEx.Number == 547) // Mã lỗi khóa ngoại trong SQL Server
                        {
                            MessageBox.Show("Cannot delete model because it is associated with existing records (e.g., in Stock, Sales). Please delete related items first.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Database Error deleting model: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Console.WriteLine("SQL Error deleting model: " + sqlEx.Message);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General Error deleting model: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("General Error deleting model: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}