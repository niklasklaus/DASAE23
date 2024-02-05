using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class InsertEffortAndDiscountService
{
    private List<double> resultsEffort = new List<double>(); // Variable to store the results
    private List<double> resultsDiscount = new List<double>(); // Variable to store the results
    private List<int> resultslvAmount = new List<int>(); // Variable to store the results
    private List<double> resultscalculatedEp = new List<double>(); // Variable to store the results
    private List<int> resultLvID = new List<int>(); // Variable to store the results
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
    
    public async Task SelectLvAmount(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT lv_amount FROM LVS WHERE user_id  = '{uid}' and proposal_id = '{pid}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_amount")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("lv_amount");
                        resultslvAmount.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        int val = 0;
                        resultslvAmount.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<int>> ReturnLvAmount(int uid, int pid)
    {
        await SelectLvAmount(uid, pid);
        return resultslvAmount.Count > 0 ? resultslvAmount : new List<int>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectLvId(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT lv_id FROM LVS WHERE user_id  = '{uid}' and proposal_id = '{pid}' and oz = '01'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_id")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("lv_id");
                        resultLvID.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        int val = 0;
                        resultLvID.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<int> ReturnLvId(int uid, int pid)
    {
        await SelectLvId(uid, pid);
        return resultLvID.Count > 0 ? resultLvID[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCalculatedEp(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT calculated_ep FROM LVS WHERE user_id  = '{uid}' and proposal_id = '{pid}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("calculated_ep")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("calculated_ep");
                        resultscalculatedEp.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        double val = 0.00;
                        resultscalculatedEp.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnCalculatedEp(int uid, int pid)
    {
        await SelectCalculatedEp(uid, pid);
        return resultscalculatedEp.Count > 0 ? resultscalculatedEp : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task InsertEffortAndDiscount(double effort, double discount, int uid, int pid, List<int> lvAmount, int lvId)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            
            string updateLV1 = $"UPDATE LVS SET effort_factor = CAST(REPLACE('{effort}', ',', '.') AS DECIMAL(3,2)) WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            
            MySqlCommand command1 = new MySqlCommand(updateLV1, mysqlconnection);
            command1.ExecuteNonQuery();
            
            string updateLV2 = $"UPDATE LVS SET calculated_ep = basic_ep * effort_factor  WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            
            MySqlCommand command2 = new MySqlCommand(updateLV2, mysqlconnection);
            command2.ExecuteNonQuery();
            
            string updateProposal1 = $"UPDATE PROPOSALS SET discount = CAST(REPLACE('{discount}', ',', '.') AS DECIMAL(10,2)) WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            
            MySqlCommand command3 = new MySqlCommand(updateProposal1, mysqlconnection);
            command3.ExecuteNonQuery();
            
            TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
            DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
            string updateProposal2 = $"UPDATE PROPOSALS SET updated_at = '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE proposal_id = '{pid}' and user_id = '{uid}'";
           
            MySqlCommand command4 = new MySqlCommand(updateProposal2, mysqlconnection);
            command4.ExecuteNonQuery();

            foreach (double amount in lvAmount)
            {
    
                string updateProposalX = $"UPDATE LVS SET calculated_gb = calculated_ep * {amount} WHERE proposal_id = '{pid}' AND user_id = '{uid}' and lv_id = '{lvId}'";

                using (MySqlCommand commandX = new MySqlCommand(updateProposalX, mysqlconnection))
                {
                    commandX.ExecuteNonQuery();
                }

                lvId++;
            }


        }
    }
}