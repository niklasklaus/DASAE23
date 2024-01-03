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
    
    public async Task SelectPaymentTerm(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT payment_term FROM PROPOSALS WHERE proposal_id = '{pid}'";
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
    }

    public async Task<int> ReturnPaymentTerm(int pid)
    {
        await SelectPaymentTerm(pid);
        return resultsPaymentTerm.Count > 0 ? resultsPaymentTerm[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectSkontoPercent(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT skonto_percent FROM PROPOSALS WHERE proposal_id = '{pid}'";
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
    }

    public async Task<double> ReturnSkontoPercent(int pid)
    {
        await SelectSkontoPercent(pid);
        return resultsSkontoPercent.Count > 0 ? resultsSkontoPercent[0] : 0.0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectSkontoDays(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT skonto_days FROM PROPOSALS WHERE proposal_id = '{pid}'";
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
    }

    public async Task<int> ReturnSkontoDays(int pid)
    {
        await SelectSkontoDays(pid);
        return resultsSkontoDays.Count > 0 ? resultsSkontoDays[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectProjectName(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT project_name FROM PROPOSALS WHERE proposal_id = '{pid}'";
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
    }

    public async Task<string> ReturnProjectName(int pid)
    {
        await SelectProjectName(pid);
        return resultsProjectName.Count > 0 ? resultsProjectName[0] : ""; // Return the first value if available, otherwise return a default value
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