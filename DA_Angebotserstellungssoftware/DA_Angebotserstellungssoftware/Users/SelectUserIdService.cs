using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Users;

public class SelectUserIdService
{
    private List<int> resultUserId = new List<int>();
    private List<string> resultUserName = new List<string>();
    private readonly MySqlConnectionManager connection;
    
    public SelectUserIdService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task SelectUserId(string username, string password)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            //mysqlconnection.Open();
            string selectUser = $"SELECT user_id FROM USERS WHERE username = '{username}' and password = '{password}'";
            MySqlCommand command1 = new MySqlCommand(selectUser, mysqlconnection);
            command1.ExecuteNonQuery();
        
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("user_id")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("user_id");
                        resultUserId.Add(val); // Add the retrieved value to the list
                    }
                    else
                    {
                        int val = 0;
                        resultUserId.Add(val);
                    }
                }
            }
        } 

        // Nachdem der using-Block beendet ist, wird die MySqlConnection automatisch geschlossen
        // Jetzt rufen wir die CloseConnection()-Methode auf, um den SSH-Tunnel zu schließen
        connection.CloseConnection();
    }



    public async Task<int> ReturnUserId(string username, string password)
    {
        await SelectUserId(username, password);
        return resultUserId.Count > 0 ? resultUserId[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectUserName(int userid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectUser = $"SELECT username FROM USERS WHERE user_id = '{userid}'";
            MySqlCommand command1 = new MySqlCommand(selectUser, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("username")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("username");
                        resultUserName.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultUserName.Add(val);
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<string> ReturnUserName(int userid)
    {
        await SelectUserName(userid);
        return resultUserName.Count > 0 ? resultUserName[0] : ""; // Return the first value if available, otherwise return a default value
    }
    
    
}