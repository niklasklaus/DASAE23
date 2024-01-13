using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class UpdateShortNameService
{
    private readonly MySqlConnectionManager connection;
    private List<string> resultLvName = new List<string>();

    public UpdateShortNameService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public void UpdateProposalShortName(string lv_name, string customer_name, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            string[] splittedLVName = lv_name.Split('-');
            
            Random rand = new Random();
            int number = rand.Next(1, 11);
            mysqlconnection.Open();
            
            string updateShortName = $"UPDATE PROPOSALS SET proposal_short = '{splittedLVName[0].Trim()}_{customer_name}_{number}' WHERE proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(updateShortName, mysqlconnection);
            command1.ExecuteNonQuery();
        }
    }
    
    public async Task SelectLVNames(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT short_text FROM LVS WHERE proposal_id = '{pid}' and oz = '01'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("short_text")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("short_text");
                        resultLvName.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }

    public async Task<string> ReturnLVName(int pid)
    {
        await SelectLVNames(pid);
        return  resultLvName.Count > 0 ? resultLvName[0] : ""; // Return the first value if available, otherwise return a default value
    }
}