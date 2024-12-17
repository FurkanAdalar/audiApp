using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace audiApp
{
    public class DatabaseHelper
    {
        private string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\MyComp\\Documents\\audiApp\\audiApp\\audi.mdb;";

        // Veritabanına bağlanır
        private OleDbConnection GetConnection()
        {
            var connection = new OleDbConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Access veritabanı bağlantısı başarılı!");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bağlantı hatası: " + ex.Message);
                throw;
            }
        }

        // Kullanıcı ekleme
        public void AddUser(string eposta, string sifre)
        {
            using (var connection = GetConnection())
            {
                string query = "INSERT INTO Users (ePosta, parola) VALUES (?, ?)";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", eposta);
                    command.Parameters.AddWithValue("?", sifre);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Kullanıcı silme
        public void DeleteUser(int userId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Users WHERE Id = ?";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Kullanıcı güncelleme
        public void UpdateUser(int userId, string newEposta, string newSifre)
        {
            using (var connection = GetConnection())
            {
                string query = "UPDATE Users SET ePosta = ?, parola = ? WHERE Id = ?";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", newEposta);
                    command.Parameters.AddWithValue("?", newSifre);
                    command.Parameters.AddWithValue("?", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool CheckUserLogin(string eposta, string sifre)
        {
            using (var connection = GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Users WHERE ePosta = ? AND parola = ?";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", eposta);
                    command.Parameters.AddWithValue("?", sifre);

                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    return userCount > 0; // Eğer kullanıcı varsa true döner
                }
            }
        }

        public void AddModel(string name, string type, decimal price, string color, int year, byte[] photoData)
        {
            using (var connection = GetConnection())
            {
                string query = "INSERT INTO Models (_name, _type, _price, _color, _year, _photo) VALUES (?, ?, ?, ?, ?, ?)";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", name);
                    command.Parameters.AddWithValue("?", type);
                    command.Parameters.AddWithValue("?", price);
                    command.Parameters.AddWithValue("?", color);
                    command.Parameters.AddWithValue("?", year);
                    command.Parameters.AddWithValue("?", photoData);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Model ve resim başarıyla eklendi!");
                }
            }
        }

        public void DeleteModelByAttributes(string modelName, string modelType, string modelColor)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Models WHERE _name = ? AND _color = ? AND _type = ?";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", modelName);
                    command.Parameters.AddWithValue("?", modelColor);
                    command.Parameters.AddWithValue("?", modelType);

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows == 0)
                    {
                        throw new Exception("Silme işlemi başarısız. Eşleşen model bulunamadı.");
                    }

                    Console.WriteLine("Model başarıyla silindi!");
                }
            }
        }

        public void LoadDataById(int id, out string name, out string type, out string price, out string color, out string year, out byte[] photo)
        {
            name = type = price = color = year = null;
            photo = null;

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Models WHERE ID = ?";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", id);

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            name = reader["_name"].ToString();
                            type = reader["_type"].ToString();
                            price = reader["_price"].ToString();
                            color = reader["_color"].ToString();
                            year = reader["_year"].ToString();

                            if (reader["_photo"] != DBNull.Value)
                            {
                                photo = (byte[])reader["_photo"];
                            }
                        }
                    }
                }
            }
        }

        public List<string> GetDataById(int id)
        {
            var dataList = new List<string>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Models WHERE ID = ?";
                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dataList.Add($"Name: {reader["_name"]}");
                            dataList.Add($"Type: {reader["_type"]}");
                            dataList.Add($"Price: {reader["_price"]}");
                            dataList.Add($"Color: {reader["_color"]}");
                            dataList.Add($"Year: {reader["_year"]}");

                            if (reader["_photo"] != DBNull.Value)
                            {
                                dataList.Add("Photo: Exists");
                            }
                            else
                            {
                                dataList.Add("Photo: Not Available");
                            }
                        }
                    }
                }
            }

            return dataList;
        }

        public bool DeleteById(int id)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Models WHERE ID = ?";
                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", id);
                    int rowsAffected = command.ExecuteNonQuery();

                    // Silinen satır sayısını kontrol et
                    return rowsAffected > 0;
                }
            }
        }

        public List<string> GetModelNames()
        {
            var modelNames = new List<string>();

            using (var connection = GetConnection())
            {
                string query = "SELECT [_name] FROM Models";
                using (var command = new OleDbCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            modelNames.Add(reader["_name"].ToString());
                        }
                    }
                }
            }

            return modelNames;
        }

        public List<(string modelName, double price)> GetModelsData()
        {
            var modelsData = new List<(string, double)>();

            using (var connection = new OleDbConnection(connectionString))
            {
                string query = "SELECT _name, _price FROM Models";
                var command = new OleDbCommand(query, connection);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string modelName = reader.GetString(reader.GetOrdinal("_name"));
                        double price = reader.GetDouble(reader.GetOrdinal("_price"));
                        modelsData.Add((modelName, price));
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Veritabanı bağlantı hatası: " + ex.Message);
                }
            }

            return modelsData;
        }

        public DataTable ExecuteQuery(string query)
        {
            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var adapter = new OleDbDataAdapter(query, connection);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Veri tabanı hatası: {ex.Message}");
                }
            }
        }

        public List<string> GetUserDataById(int id)
        {
            var dataList = new List<string>();

            using (var connection = GetConnection())
            {
                string query = "SELECT ePosta, parola FROM Users WHERE ID = ?";
                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", id);

                    try
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dataList.Add($"E-posta: {reader["ePosta"]}");
                                dataList.Add($"Şifre: {reader["parola"]}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Veri okuma hatası: {ex.Message}");
                    }
                }
            }

            return dataList;
        }
        public int GetModelIdByName(string modelName)
{
    int modelId = -1;

    using (var connection = GetConnection())
    {
        string query = "SELECT ID FROM Models WHERE ModelName = ?";
        using (var command = new OleDbCommand(query, connection))
        {
            command.Parameters.AddWithValue("?", modelName);

            connection.Open(); // Bağlantıyı aç
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    modelId = Convert.ToInt32(reader["ID"]); // ID'yi int olarak döndür
                }
            }
        }
    }

    return modelId;
}


        public List<string> GetModelPropertiesById(int id)
        {
            var dataList = new List<string>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Models WHERE ID = ?";
                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", id);

                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dataList.Add($"Name: {reader["_name"]}");
                                dataList.Add($"Type: {reader["_type"]}");
                                dataList.Add($"Price: {reader["_price"]}");
                                dataList.Add($"Color: {reader["_color"]}");
                                dataList.Add($"Year: {reader["_year"]}");

                                if (reader["_photo"] != DBNull.Value)
                                {
                                    dataList.Add("Photo: Exists");
                                }
                                else
                                {
                                    dataList.Add("Photo: Not Available");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Veri tabanı hatası: {ex.Message}", ex);
                    }
                }
            }

            return dataList;
        }

        public void SellModel(int modelId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Models WHERE ID = ?";
                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", modelId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}

