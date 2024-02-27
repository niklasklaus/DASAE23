using MySqlConnector;

namespace DA_PDF_Generierung_Result;

public class MySqlConnector
{
    public string GetConeection()
    {
        return "Server=localhost;Database=da_dbschema;User Id=root;Password=root;";
    }
    
    public List<Dictionary<string, object>> RetrieveDataListFromDatabase(string connectionString, string sqlStatement)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> data = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                data.Add(reader.GetName(i), reader.GetValue(i));
                            }

                            dataList.Add(data);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Abrufen von Daten aus der Datenbank: " + ex.Message);
        }

        return dataList;
    }

    public void UpdateDataFromDatabase(string connectionString, string sqlStatement)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    command.ExecuteNonQuery();
                    //Console.WriteLine($"Anzahl der betroffenen Zeilen: {rowsAffected}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Aktualisieren der Daten in der Datenbank: " + ex.Message);
        }
    }

}