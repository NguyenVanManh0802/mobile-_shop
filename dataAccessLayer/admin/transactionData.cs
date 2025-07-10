using MobileShop.models;
using System;
using System.Collections.Generic;
using System.Data; // Thêm using này để dùng SqlDbType
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // Dùng để hiển thị MessageBox từ DAL khi lỗi

namespace MobileShop.dataAccessLayer.admin
{
    internal class transactionData
    {
        private string connectionString = "Data Source=NGUYENMANH;Initial Catalog=mobileShop;Integrated Security=True";

        // Hàm tạo TransId tự động tăng (tương tự các ID khác)
        private string GenerateNewTransId()
        {
            string maxId = null;
            string query = "SELECT MAX(TranId) FROM tbl_transaction WHERE TranId LIKE 'TR%'"; // Giả sử ID bắt đầu bằng 'TR'

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
                        Console.WriteLine("Error getting max Transaction ID: " + ex.Message);
                    }
                }
            }

            string prefix = "TR";
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
            return prefix + number.ToString("D5"); // Ví dụ: TR00001
        }


        // Phương thức để thêm một giao dịch mới
        public bool AddTransaction(Transaction transaction)
        {
            transaction.TransId = GenerateNewTransId(); // Tạo ID giao dịch mới

            string query = "INSERT INTO tbl_transaction (TranId, ModelId, Quantity, Date, Amount, TransactionType) VALUES (@TransId, @ModelId, @Quantity, @Date, @Amount, @TransactionType)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Mở kết nối
                SqlTransaction sqlTrans = connection.BeginTransaction(); // Bắt đầu SQL Transaction

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection, sqlTrans))
                    {
                        command.Parameters.AddWithValue("@TransId", transaction.TransId);
                        command.Parameters.AddWithValue("@ModelId", transaction.ModelId);
                        command.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                        command.Parameters.AddWithValue("@Date", transaction.Date);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);

                        int rowsAffectedTrans = command.ExecuteNonQuery();

                        if (rowsAffectedTrans > 0)
                        {
                            // Nếu thêm giao dịch thành công, tiếp tục cập nhật tồn kho
                            bool stockUpdated = UpdateModelStock(transaction.ModelId, transaction.Quantity, transaction.TransactionType, connection, sqlTrans);

                            if (stockUpdated)
                            {
                                sqlTrans.Commit(); // Commit transaction nếu cả hai thao tác đều thành công
                                return true;
                            }
                            else
                            {
                                sqlTrans.Rollback(); // Rollback nếu cập nhật kho thất bại
                                MessageBox.Show("Giao dịch được ghi nhận nhưng cập nhật tồn kho thất bại. Giao dịch đã bị hủy.", "Lỗi Cập nhật Kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            sqlTrans.Rollback(); // Rollback nếu không thêm được giao dịch
                            return false;
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    sqlTrans.Rollback(); // Rollback nếu có lỗi SQL
                    if (sqlEx.Number == 547) // Lỗi khóa ngoại (ModelId không tồn tại)
                    {
                        MessageBox.Show("Cannot add transaction: The selected Model ID does not exist.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Lỗi trùng khóa chính (TransId)
                    {
                        MessageBox.Show("Cannot add transaction: Transaction ID already exists. Please try again.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Database Error adding transaction: " + sqlEx.Message + "\nSQL Error Code: " + sqlEx.Number, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Console.WriteLine("SQL Error adding transaction: " + sqlEx.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    sqlTrans.Rollback(); // Rollback nếu có lỗi chung
                    MessageBox.Show("General Error adding transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("General Error adding transaction: " + ex.Message);
                    return false;
                }
            }
        }

        // Phương thức nội bộ để cập nhật số lượng tồn kho trong tbl_model
        // Sử dụng cùng SqlConnection và SqlTransaction để đảm bảo tính toàn vẹn
        private bool UpdateModelStock(string modelId, int quantity, string transactionType, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "";
            if (transactionType == "Sale") // Nếu là giao dịch bán, giảm số lượng
            {
                // Kiểm tra đủ tồn kho trước khi giảm
                query = "UPDATE tbl_model SET AvailableQty = AvailableQty - @Quantity WHERE ModelId = @ModelId AND AvailableQty >= @Quantity";
            }
            else if (transactionType == "Purchase") // Nếu là giao dịch nhập, tăng số lượng
            {
                query = "UPDATE tbl_model SET AvailableQty = AvailableQty + @Quantity WHERE ModelId = @ModelId";
            }
            else
            {
                Console.WriteLine("Invalid transaction type for stock update.");
                return false;
            }

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@ModelId", modelId);
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating model stock within transaction: " + ex.Message);
                    return false;
                }
            }
        }

        // Phương thức để lấy tất cả các giao dịch
        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            // Đảm bảo SELECT tất cả các cột cần thiết, bao gồm TransactionType
            string query = "SELECT TranId, ModelId, Quantity, Date, Amount, TransactionType FROM tbl_transaction";

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
                                Transaction transaction = new Transaction
                                {
                                    TransId = reader["TranId"].ToString(),
                                    ModelId = reader["ModelId"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    Date = Convert.ToDateTime(reader["Date"]),
                                    Amount = Convert.ToSingle(reader["Amount"]), // Đọc Amount là float
                                    TransactionType = reader["TransactionType"].ToString()
                                };
                                transactions.Add(transaction);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error getting transactions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error getting transactions: " + ex.Message);
                    }
                }
            }
            return transactions;
        }

        // Phương thức để lấy giá của một Model (cần cho việc tính toán Amount)
        // Đây là ModelData, nhưng vì nó được dùng trong transactionData, tôi đặt ở đây tạm.
        // Bạn có thể cân nhắc di chuyển nó về ModelData.cs nếu hợp lý hơn.
        public float GetModelPrice(string modelId)
        {
            float price = 0;
            string query = "SELECT price FROM tbl_mobile WHERE ModelId = @ModelId"; // Lấy giá từ bảng mobile (giả định giá nằm ở đây)

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
                            price = Convert.ToSingle(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error getting model price: " + ex.Message);
                        MessageBox.Show("Error getting model price: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return price;
        }
    }
}