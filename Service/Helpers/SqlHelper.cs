using MySql.Data.MySqlClient;

namespace Service;

public static class SqlHelper
{
    public static void InsertData(Dictionary<string, string> dictionaryData, string connectionString, string tableName) 
    {
        // Check and add column if it doesn't exist
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = $"CREATE TABLE IF NOT EXISTS {tableName} (Id TEXT)";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();    

            foreach (var pair in dictionaryData)
            {
                string columnName = pair.Key;

                if (!IsColumnExists(columnName, tableName, connection))
                {
                    // Column doesn't exist, so add it to the table
                    AddColumn(columnName, tableName, connection);
                }
            }

            query = $"INSERT INTO {tableName} ({string.Join(", ", dictionaryData.Keys)}) VALUES ({string.Join(", ", dictionaryData.Keys.Select(key => "@" + key))})";

            // Insert data into the table
            command = new MySqlCommand(query, connection);
            foreach (var pair in dictionaryData)
            {
                command.Parameters.AddWithValue("@" + pair.Key, pair.Value);
            }

            command.ExecuteNonQuery();    
        }        
    }

    public static void InsertDataDynamic(Dictionary<dynamic, dynamic> dictionaryData, string connectionString, string tableName) 
    {
        // Check and add column if it doesn't exist
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = $"CREATE TABLE IF NOT EXISTS {tableName} (Id TEXT)";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();    

            foreach (var pair in dictionaryData)
            {
                string columnName = pair.Key;

                if (!IsColumnExists(columnName, tableName, connection))
                {
                    // Column doesn't exist, so add it to the table
                    AddColumn(columnName, tableName, connection);
                }
            }

            query = $"INSERT INTO {tableName} ({string.Join(", ", dictionaryData.Keys)}) VALUES ({string.Join(", ", dictionaryData.Keys.Select(key => "@" + key))})";

            // Insert data into the table
            command = new MySqlCommand(query, connection);
            foreach (var pair in dictionaryData)
            {
                command.Parameters.AddWithValue("@" + pair.Key, pair.Value);
            }

            command.ExecuteNonQuery();    
        }        
    }


    private static bool IsColumnExists(string columnName, string tableName, MySqlConnection connection)
    {
        string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = '{columnName}' AND TABLE_NAME = '{tableName}'";
        MySqlCommand command = new MySqlCommand(query, connection);
        int count = Convert.ToInt32(command.ExecuteScalar());
        return count > 0;
    }

    private static void AddColumn(string columnName,string tableName, MySqlConnection connection)
    {
        if (columnName.Length <= 3) 
        {
            columnName = columnName.ToUpper();
        } 
        else
        {
            columnName = $"{char.ToUpper(columnName[0])}{columnName[1..]}";
        } 
        
        string query = $"ALTER TABLE {tableName} ADD {columnName} TEXT NULL";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.ExecuteNonQuery();
    }

    public static string GetNullableString(MySqlDataReader reader, string columnName)
    {
        int columnIndex = reader.GetOrdinal(columnName);
        return !reader.IsDBNull(columnIndex) ? reader.GetString(columnIndex) : null;
    }
}
