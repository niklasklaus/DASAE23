using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;

namespace DA_Angebotserstellungssoftware;

public class LoginService
{
    private readonly MySqlConnectionManager connection;
    
    public LoginService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

 
    public async Task<bool> Authenticate(string username, string password)
    {
        int count;
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            //mysqlconnection.Open();
            string query = $"SELECT COUNT(*) FROM USERS WHERE username = '{username}' AND password = '{password}'";
            MySqlCommand command = new MySqlCommand(query, mysqlconnection);
            count = Convert.ToInt32(command.ExecuteScalar());
        }
        
        connection.CloseConnection();
        return count > 0;
        
    }
}