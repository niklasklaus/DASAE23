using System.Globalization;
using MySqlConnector;

namespace DA_Angebotserstellungssoftware;

public class UpdateLVService
{
    private readonly MySqlConnectionManager connection;
    private List<string> resultsshortText = new List<string>();
    private List<string> resultOz = new List<string>();
    private List<string> resultsLongText = new List<string>();
    private List<double> resultsLVamount = new List<double>();
    private List<double> resultsbasicEp = new List<double>();
    private List<double> resultscalculatedEp = new List<double>();
    private List<string> resultsepCurrency = new List<string>();
    private List<double> resultsbasicGb = new List<double>();
    private List<double> resultscalculatedGb = new List<double>();
    private List<string> resultsgbCurrency = new List<string>();
    private List<double> resultseffortFactor = new List<double>();
    private List<string> resultsLVAmountUnit = new List<string>();
    public UpdateLVService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task SelectOz(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT oz FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("oz")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("oz");
                        resultOz.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnOz(int uid, int pid, string lv_type)
    {
        await SelectOz(uid, pid, lv_type);
        return resultOz.Count > 0 ? resultOz : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectLongText(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT long_text FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("long_text")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("long_text");
                        resultsLongText.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultsLongText.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnLongText(int uid, int pid, string lv_type)
    {
        await SelectLongText(uid, pid, lv_type);
        return resultsLongText.Count > 0 ? resultsLongText : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectEpCurrency(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT ep_currency FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ep_currency")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("ep_currency");
                        resultsepCurrency.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultsepCurrency.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnEpCurrency(int uid, int pid, string lv_type)
    {
        await SelectEpCurrency(uid, pid, lv_type);
        return resultsepCurrency.Count > 0 ? resultsepCurrency : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectBasicEp(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT basic_ep FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("basic_ep")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("basic_ep");
                        resultsbasicEp.Add(val); // Add the retrieved value to the list
                    }
                    else
                    {
                        double val = 0.00;
                        resultsbasicEp.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnBasicEp(int uid, int pid, string lv_type)
    {
        await SelectBasicEp(uid, pid, lv_type);
        return resultsbasicEp.Count > 0 ? resultsbasicEp : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCalculatedEp(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT calculated_ep FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
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
    
    public async Task<List<double>> ReturnCalculatedEp(int uid, int pid, string lv_type)
    {
        await SelectCalculatedEp(uid, pid, lv_type);
        return resultscalculatedEp.Count > 0 ? resultscalculatedEp : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectBasicGb(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT basic_gb FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("basic_gb")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("basic_gb");
                        resultsbasicGb.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnBasicGb(int uid, int pid, string lv_type)
    {
        await SelectBasicGb(uid, pid, lv_type);
        return resultsbasicGb.Count > 0 ? resultsbasicGb : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCalculatedGb(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT calculated_gb FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("calculated_gb")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("calculated_gb");
                        resultscalculatedGb.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        double val = 0.00;
                        resultscalculatedGb.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnCalculatedGb(int uid, int pid, string lv_type)
    {
        await SelectCalculatedGb(uid, pid, lv_type);
        return resultscalculatedGb.Count > 0 ? resultscalculatedGb : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectEffortFactor(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT effort_factor FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("effort_factor")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("effort_factor");
                        resultseffortFactor.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        double val = 0.00;
                        resultseffortFactor.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnEffortFactor(int uid, int pid, string lv_type)
    {
        await SelectEffortFactor(uid, pid, lv_type);
        return resultseffortFactor.Count > 0 ? resultseffortFactor : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectGbCurrency(int uid, int pid, string lv_type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;
            
            string selectProposal = $"SELECT gb_currency FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
            command1 = new MySqlCommand(selectProposal, mysqlconnection);
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("gb_currency")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("gb_currency");
                        resultsgbCurrency.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnGbCurrency(int uid, int pid, string lv_type)
    {
        await SelectGbCurrency(uid, pid, lv_type);
        return resultsgbCurrency.Count > 0 ? resultsgbCurrency : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    

    public async Task SelectShortText(int uid, int pid, string lv_type, int doesExist)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;

            if (doesExist == 1)
            {
                string selectProposal = $"SELECT short_text FROM LVS WHERE proposal_id = '{pid}'  and user_id = '{uid}' and lv_type = '{lv_type}'";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();   
            }

            else
            {
                string selectProposal = $"SELECT short_text FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery(); 
            }
            
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
    
    public async Task<List<string>> ReturnLVShortText(int uid, int pid, string lv_type, int doesExist)
    {
        await SelectShortText(uid, pid, lv_type, doesExist);
        return resultsshortText.Count > 0 ? resultsshortText : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectLVAmount(int uid, int pid, string lv_type, int doesExist)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;

            if (doesExist == 1)
            {
                string selectProposal = $"SELECT lv_amount FROM LVS WHERE proposal_id = '{pid}'  and user_id = '{uid}' and lv_type = '{lv_type}'";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();   
            }

            else
            {
                string selectProposal = $"SELECT lv_amount FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery(); 
            }
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_amount")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("lv_amount");
                        resultsLVamount.Add(val); // Add the retrieved value to the list
                    }
                    else
                    {
                        int val = 0;
                        resultsLVamount.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<List<double>> ReturnLVAmount(int uid, int pid, string lv_type, int doesExist)
    {
        await SelectLVAmount(uid, pid, lv_type, doesExist);
        return resultsLVamount.Count > 0 ? resultsLVamount : new List<double>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectLVAmountUnit(int uid, int pid, string lv_type, int doesExist)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command1;

            if (doesExist == 1)
            {
                string selectProposal = $"SELECT lv_amount_unit FROM LVS WHERE proposal_id = '{pid}'  and user_id = '{uid}' and lv_type = '{lv_type}'";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();   
            }

            else
            {
                string selectProposal = $"SELECT lv_amount_unit FROM MASTER_LVS WHERE lv_type = '{lv_type}'";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery(); 
            }
            
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
                        string val = "";
                        resultsLVAmountUnit.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }
    
    public async Task<List<string>> ReturnLVAmountUnit(int uid, int pid, string lv_type, int doesExist)
    {
        await SelectLVAmountUnit(uid, pid, lv_type, doesExist);
        return resultsLVAmountUnit.Count > 0 ? resultsLVAmountUnit : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    

    public async Task UpdateLV(int uid, int pid, string lv_type, string oz, string lvshort, string longtext, double lvAmount, string lv_amount_unit, double basic_ep, double calculated_ep, string ep_currency, double basic_gb, double calculated_gb, string gb_currency, double effort_factor, int doesExist) //Datentypen noch überarbeiten
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            MySqlCommand command;

            if (doesExist == 1)
            {
                string updateLV1 = $"UPDATE LVS SET lv_amount = @LvAmount, calculated_gb = calculated_ep * @LVAmount WHERE proposal_id = @Pid and short_text = @ST and user_id = '{uid}' and lv_type = '{lv_type}'";
                command = new MySqlCommand(updateLV1, mysqlconnection);
                command.Parameters.AddWithValue("@LvAmount", lvAmount);
                command.Parameters.AddWithValue("@Pid", pid);
                command.Parameters.AddWithValue("@ST", lvshort);
                command.ExecuteNonQuery();   
            }

            else
            {
                
                string insertLV =
                    $"INSERT INTO LVS (proposal_id, user_id, lv_type, oz, pa, short_text, long_text, lv_amount, lv_amount_unit, basic_ep, calculated_ep, ep_currency, basic_gb, calculated_gb, gb_currency, effort_factor) " +
                    $"VALUES ('{pid}', '{uid}', '{lv_type}', '{oz}', NULL, '{lvshort}', '{longtext}', '{lvAmount}', '{lv_amount_unit}', CAST(REPLACE('{basic_ep}', ',', '.') AS DECIMAL(10,2)), '{calculated_ep}', '{ep_currency}', '{basic_gb}', '{calculated_gb}', '{gb_currency}', '{effort_factor}')";
                command = new MySqlCommand(insertLV, mysqlconnection);
                command.ExecuteNonQuery(); 
            }
        }
    }
}