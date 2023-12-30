using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class LoginService
{
    private readonly MySqlConnectionManager connection;
    
    public LoginService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public bool Authenticate(string username, string password)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string query = $"SELECT COUNT(*) FROM USERS WHERE username = '{username}' AND password = '{password}'";
            MySqlCommand command = new MySqlCommand(query, mysqlconnection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
    
    /*private readonly ApplicationDbContext _dbContext;

    public LoginService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Authenticate(string username, string password)
    {
        var user = _dbContext.Users2.FirstOrDefault(u => u.Username == username && u.Password == password);
        return user != null;
    }*/
}