using DA_Angebotserstellungssoftware;
using EMailSendService.Model;
using MySqlConnector;

namespace EMailSendService.DBStatements;

public class PerformedStatements
{
    private readonly MySqlConnectionManager connection;

    public PerformedStatements(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public async Task<List<EMailModel>> GetNeededEmails(int uid, int pid)
    {
        List<EMailModel> emails = new List<EMailModel>();
        
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectEMails = $"SELECT * FROM EMAILS WHERE user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectEMails, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    EMailModel model = new EMailModel()
                    {
                        EMailId = reader.GetInt32(reader.GetOrdinal("email_id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                        ProposalId = reader.GetInt32(reader.GetOrdinal("proposal_id")),
                        EMail = reader.GetString(reader.GetOrdinal("email")),
                        Type = reader.GetString(reader.GetOrdinal("type"))
                    };
                    
                    emails.Add(model);
                }
            }
        }

        return emails;
    }
}