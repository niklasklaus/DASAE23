using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class UpdateLVService
{
    private readonly MySqlConnectionManager connection;
    private List<string> resultsshortText = new List<string>();
    private List<double> resultsLVamount = new List<double>();
    private List<string> resultsLVAmountUnit = new List<string>();
    public UpdateLVService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public async Task SelectShortText(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT short_text FROM LVS WHERE proposal_id = '{pid}'";
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
                        resultsshortText.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnLVShortText(int pid)
    {
        await SelectShortText(pid);
        return resultsshortText.Count > 0 ? resultsshortText : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectLVAmount(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT lv_amount FROM LVS WHERE proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_amount")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("lv_amount");
                        resultsLVamount.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        double val = 0.0;
                        resultsLVamount.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnLVAmount(int pid)
    {
        await SelectLVAmount(pid);
        return resultsLVamount.Count > 0 ? resultsLVamount : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectLVAmountUnit(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT lv_amount_unit FROM LVS WHERE proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_amount_unit")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("lv_amount_unit");
                        resultsLVAmountUnit.Add(val); // Add the retrieved value to the list
                    }
                    else
                    {
                        string val = "-";
                        resultsLVAmountUnit.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnLVAmountUnit(int pid)
    {
        await SelectLVAmountUnit(pid);
        return resultsLVAmountUnit.Count > 0 ? resultsLVAmountUnit : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    

    public async Task UpdateLV(int pid, double lvAmount, string lvshort)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            
            string updateLV1 = "UPDATE LVS SET lv_amount = @LvAmount, calculated_gb = calculated_ep * @LVAmount WHERE proposal_id = @Pid and short_text = @ST";
            MySqlCommand command = new MySqlCommand(updateLV1, mysqlconnection);
            command.Parameters.AddWithValue("@LvAmount", lvAmount);
            command.Parameters.AddWithValue("@Pid", pid);
            command.Parameters.AddWithValue("@ST", lvshort);
            command.ExecuteNonQuery();
            
        }
    }
}