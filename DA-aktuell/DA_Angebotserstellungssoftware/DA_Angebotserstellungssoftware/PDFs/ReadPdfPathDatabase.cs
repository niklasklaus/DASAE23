using MySqlConnector;

namespace DA_Angebotserstellungssoftware.PDFs;

public class ReadPdfPathDatabase
{
    private readonly MySqlConnectionManager connection;
    
    public ReadPdfPathDatabase(MySqlConnectionManager connection)
    {
        this.connection = connection;
    }
    public async Task<string> GetPathOfPdf(int userId, int proposalId)
    {
        string path = null; // Variable, um den Pfad zu speichern

        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            await mysqlconnection.OpenAsync(); // Verwenden Sie OpenAsync, um asynchron zu öffnen
            string selectPath = $"SELECT path FROM PDFS WHERE user_id = {userId} AND proposal_id = {proposalId}";
            MySqlCommand command1 = new MySqlCommand(selectPath, mysqlconnection);
            await command1.ExecuteNonQueryAsync(); // Verwenden Sie ExecuteNonQueryAsync, um asynchron auszuführen

            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("path")))
                    {
                        // Lesen Sie den Pfadwert aus der Datenbank und speichern Sie ihn in der path-Variable
                        path = reader.GetString("path");
                        Console.WriteLine(path);
                    }
                }
            }
        }

        // Rückgabe des Pfadwerts
        return path;
    }
}
