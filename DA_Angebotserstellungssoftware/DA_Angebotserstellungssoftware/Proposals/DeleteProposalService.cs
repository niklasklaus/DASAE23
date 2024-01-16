using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class DeleteProposalService
{
    private readonly MySqlConnectionManager connection;

    public DeleteProposalService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public void DeleteProposal(int proposal_id, int user_id)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            
            string deleteLV = $"DELETE FROM LVS WHERE proposal_id = '{proposal_id}' and user_id = '{user_id}'";
            string deleteProposal = $"DELETE FROM PROPOSALS WHERE proposal_id = '{proposal_id}' and user_id = '{user_id}'";
            
            MySqlCommand command1 = new MySqlCommand(deleteLV, mysqlconnection);
            command1.ExecuteNonQuery();
            MySqlCommand command2 = new MySqlCommand(deleteProposal, mysqlconnection);
            command2.ExecuteNonQuery();
        }
    }
}