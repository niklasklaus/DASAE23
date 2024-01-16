using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class InsertEffortAndDiscountService
{
    private List<double> resultsEffort = new List<double>(); // Variable to store the results
    private List<double> resultsDiscount = new List<double>(); // Variable to store the results
    private readonly MySqlConnectionManager connection;
    
    public InsertEffortAndDiscountService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public async Task SelectEffort(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectLV = $"SELECT effort_factor FROM LVS WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectLV, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("effort_factor")))
                    {
                        // Assuming the column name is 'column_name', change it accordingly
                        double value = reader.GetDouble("effort_factor");
                        resultsEffort.Add(value); // Store the retrieved value into the results list    
                    }
                }
            }
        }
    }

    public async Task<double> ReturnEffort(int uid, int pid)
    {
        await SelectEffort(uid, pid);
        return resultsEffort.Count > 0 ? resultsEffort[0] : 1.0;
    }
    
    public async Task SelectDiscount(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT discount FROM PROPOSALS WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("discount")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double discount = reader.GetDouble("discount");
                        resultsDiscount.Add(discount); // Add the retrieved value to the list
                    }
                   
                }
            }
        }
    }

    public async Task<double> ReturnDiscount(int uid, int pid)
    {
        await SelectDiscount(uid, pid);
        return resultsDiscount.Count > 0 ? resultsDiscount[0] : 0.0; // Return the first value if available, otherwise return a default value
    }
    
    
    public void InsertEffortAndDiscount(double effort, double discount, int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            
            string updateLV1 = $"UPDATE LVS SET effort_factor = CAST(REPLACE('{effort}', ',', '.') AS DECIMAL(3,2)) WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            string updateLV2 = $"UPDATE LVS SET calculated_ep = basic_ep * effort_factor  WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            string updateProposal1 = $"UPDATE PROPOSALS SET discount = CAST(REPLACE('{discount}', ',', '.') AS DECIMAL(10,2)) WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            string updateProposal2 = $"UPDATE PROPOSALS SET updated_at = DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s') WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            
            
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