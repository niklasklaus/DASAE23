using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class InsertEMailsForProposalSendService
{
    private readonly MySqlConnectionManager connection;
    private List<bool> checkIfEMailExists = new List<bool>();
    private List<string> customerEMail = new List<string>();
    private List<string> constructerEmail = new List<string>();
    private List<string> officeEmail = new List<string>();
    
    public InsertEMailsForProposalSendService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task CheckIfEMailExists(string email, int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT email FROM EMAILS where email = '{email}' and user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                    { 
                        checkIfEMailExists.Add(true);
                    }

                    else
                    {
                        checkIfEMailExists.Add(false);
                    }
                }
            }
        }
    }

    public async Task<bool> ReturnCheckIfEmailExists(string email, int uid, int pid)
    {
        await CheckIfEMailExists(email, uid, pid);
        return checkIfEMailExists.Count > 0 ? checkIfEMailExists[0] : false; // Return the first value if available, otherwise return a default value
    } 
    
    public async Task SelectCustomerEMail(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT email FROM EMAILS where type = 'Kunde' and user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                    {
                        string val = reader.GetString("email");
                        customerEMail.Add(val);
                    }

                    else
                    {
                        customerEMail.Add("");
                    }
                }
            }
        }
    }

    public async Task<string> ReturnCustomerEMail(int uid, int pid)
    {
        await SelectCustomerEMail(uid, pid);
        return customerEMail.Count > 0 ? customerEMail[0] : ""; // Return the first value if available, otherwise return a default value
    } 
    
    public async Task SelectConstructerEMail(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT email FROM EMAILS where type = 'Bauleiter' and user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                    {
                        string val = reader.GetString("email");
                        constructerEmail.Add(val);
                    }

                    else
                    {
                        constructerEmail.Add("");
                    }
                }
            }
        }
    }

    public async Task<string> ReturnConstructerEMail(int uid, int pid)
    {
        await SelectConstructerEMail(uid, pid);
        return constructerEmail.Count > 0 ? constructerEmail[0] : ""; // Return the first value if available, otherwise return a default value
    } 
    
    public async Task SelectOfficeEMail(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT email FROM EMAILS where type = 'Büro' and user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("email")))
                    {
                        string val = reader.GetString("email");
                        officeEmail.Add(val);
                    }

                    else
                    {
                        officeEmail.Add("");
                    }
                }
            }
        }
    }

    public async Task<string> ReturnOfficeEMail(int uid, int pid)
    {
        await SelectOfficeEMail(uid, pid);
        return officeEmail.Count > 0 ? officeEmail[0] : ""; // Return the first value if available, otherwise return a default value
    } 
    
    
    
    public async Task InsertEmailsForProposalSend(string email, string type, int uid, int pid, bool doesExist)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            if (!doesExist)
            {
                string insertEmail = $"INSERT INTO EMAILS (user_id, proposal_id, email, type) VALUES ('{uid}', '{pid}', '{email}', '{type}')";
                MySqlCommand command1 = new MySqlCommand(insertEmail, mysqlconnection);
                command1.ExecuteNonQuery();   
            }
            else
            {
                string updateEmail = $"UPDATE EMAILS SET email = '{email}' WHERE user_id = '{uid}' and proposal_id = '{pid}' and type = '{type}'";
                MySqlCommand command1 = new MySqlCommand(updateEmail, mysqlconnection);
                command1.ExecuteNonQuery();   
            }
        }
    }
}