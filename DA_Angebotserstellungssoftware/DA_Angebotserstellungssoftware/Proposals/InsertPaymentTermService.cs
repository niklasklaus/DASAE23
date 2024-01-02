using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class InsertPaymentTermService
{
    private readonly MySqlConnectionManager connection;
    
    public InsertPaymentTermService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public void InsertPaymentTerms(int payment_term, double skonto_percent, int skonto_days, string project_name, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string updateProposal = $"UPDATE PROPOSALS SET payment_term = '{payment_term}', skonto_percent = CAST(REPLACE('{skonto_percent}', ',', '.') AS DECIMAL(10,2)), skonto_days = '{skonto_days}', project_name = '{project_name}', updated_at = DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s') WHERE proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(updateProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
        }
    }
}