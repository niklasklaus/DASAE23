using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class InsertEffortAndDiscountService
{
    private readonly MySqlConnectionManager connection;
    
    public InsertEffortAndDiscountService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public void InsertEffortAndDiscount(double effort, double discount, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            
            string updateLV1 = $"UPDATE LVS SET effort_factor = CAST(REPLACE('{effort}', ',', '.') AS DECIMAL(3,2)) WHERE proposal_id = '{pid}'";
            string updateLV2 = $"UPDATE LVS SET calculated_ep = basic_ep * effort_factor  WHERE proposal_id = '{pid}'";
            string updateProposal1 = $"UPDATE PROPOSALS SET discount = CAST(REPLACE('{discount}', ',', '.') AS DECIMAL(10,2)) WHERE proposal_id = '{pid}'";
            string updateProposal2 = $"UPDATE PROPOSALS SET updated_at = DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s') WHERE proposal_id = '{pid}'";
            
            
            MySqlCommand command1 = new MySqlCommand(updateLV1, mysqlconnection);
            command1.ExecuteNonQuery();
            MySqlCommand command2 = new MySqlCommand(updateLV2, mysqlconnection);
            command2.ExecuteNonQuery();
            MySqlCommand command3 = new MySqlCommand(updateProposal1, mysqlconnection);
            command3.ExecuteNonQuery();
            MySqlCommand command4 = new MySqlCommand(updateProposal2, mysqlconnection);
            command4.ExecuteNonQuery();
        }
    }
}