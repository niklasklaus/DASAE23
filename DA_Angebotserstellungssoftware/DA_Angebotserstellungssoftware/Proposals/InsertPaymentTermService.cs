using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class InsertPaymentTermService
{
    private readonly MySqlConnectionManager connection;
    private List<int> resultsPaymentTerm = new List<int>(); // Variable to store the results
    private List<double> resultsSkontoPercent = new List<double>(); // Variable to store the results
    private List<int> resultsSkontoDays = new List<int>(); // Variable to store the results
    private List<string> resultsProjectName = new List<string>(); // Variable to store the results
    
    public InsertPaymentTermService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task SelectPaymentTerm(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT payment_term FROM PROPOSALS WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("payment_term")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("payment_term");
                        resultsPaymentTerm.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<int> ReturnPaymentTerm(int uid, int pid)
    {
        await SelectPaymentTerm(uid, pid);
        return resultsPaymentTerm.Count > 0 ? resultsPaymentTerm[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectSkontoPercent(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT skonto_percent FROM PROPOSALS WHERE proposal_id = '{pid}'  and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("skonto_percent")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("skonto_percent");
                        resultsSkontoPercent.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<double> ReturnSkontoPercent(int uid, int pid)
    {
        await SelectSkontoPercent(uid, pid);
        return resultsSkontoPercent.Count > 0 ? resultsSkontoPercent[0] : 0.0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectSkontoDays(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT skonto_days FROM PROPOSALS WHERE proposal_id = '{pid}'  and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("skonto_days")))
                        { // Assuming the column name is 'discount', change it accordingly
                            int val = reader.GetInt32("skonto_days");
                            resultsSkontoDays.Add(val); // Add the retrieved value to the list
                            
                        }
                       
                    }
            }
        }
        connection.CloseConnection();
    }

    public async Task<int> ReturnSkontoDays(int uid, int pid)
    {
        await SelectSkontoDays(uid, pid);
        return resultsSkontoDays.Count > 0 ? resultsSkontoDays[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectProjectName(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT project_name FROM PROPOSALS WHERE proposal_id = '{pid}'  and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("project_name")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("project_name");
                        resultsProjectName.Add(val); // Add the retrieved value to the list   
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<string> ReturnProjectName(int uid, int pid)
    {
        await SelectProjectName(uid, pid);
        return resultsProjectName.Count > 0 ? resultsProjectName[0] : ""; // Return the first value if available, otherwise return a default value
    }

    
    public void InsertPaymentTerms(int payment_term, double skonto_percent, int skonto_days, string project_name, int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
            DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
            //mysqlconnection.Open();
            string updateProposal = $"UPDATE PROPOSALS SET payment_term = '{payment_term}', skonto_percent = CAST(REPLACE('{skonto_percent}', ',', '.') AS DECIMAL(10,2)), skonto_days = '{skonto_days}', project_name = '{project_name}', updated_at = '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE proposal_id = '{pid}'  and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(updateProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
        }
        connection.CloseConnection();
    }
}