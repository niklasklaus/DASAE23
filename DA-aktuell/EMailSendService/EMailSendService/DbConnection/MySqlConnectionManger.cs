using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class MySqlConnectionManager
{
    private readonly string connectionString; // Verbindungszeichenfolge zur MySQL-Datenbank

    public MySqlConnectionManager(IConfiguration con)
    {
        this.connectionString = con.GetConnectionString("DefaultConnection");
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}