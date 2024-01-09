using MySqlConnector;

namespace DA_Angebotserstellungssoftware.InsertCustomerData;

public class InsertCustomerDataService
{
    private List<int> resultCustomerId = new List<int>();
    private List<string> resultSalutation = new List<string>();
    private List<string> resultFirstName = new List<string>();
    private List<string> resultLastName = new List<string>();
    private List<string> resultAddress = new List<string>();
    private List<string> resultUid = new List<string>();
    private readonly MySqlConnectionManager connection;
    
    public InsertCustomerDataService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public void InsertCustomerData(string salutation, string firstname, string lastname, string address, string uid, int pid, int customerid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            string insertQuery;
            string updateQuery;
            mysqlconnection.Open();
            if (uid == null || uid == "")
            {
                insertQuery = $"INSERT INTO CUSTOMERS (customer_id, salutation, first_name, last_name, address, uid_nr) VALUES ('{customerid}', '{salutation}', '{firstname}', '{lastname}', '{address}', NULL)";
                updateQuery =  $"UPDATE PROPOSALS SET customer_id = '{customerid}'  WHERE proposal_id = '{pid}'";

            }

            else
            {
                insertQuery = $"INSERT INTO CUSTOMERS (customer_id, salutation, first_name, last_name, address, uid_nr) VALUES ('{customerid}','{salutation}', '{firstname}', '{lastname}', '{address}', '{uid}')";
                updateQuery =  $"UPDATE PROPOSALS SET customer_id = '{customerid}'  WHERE proposal_id = '{pid}'";
            }
           
            MySqlCommand command1 = new MySqlCommand(insertQuery, mysqlconnection);
            command1.ExecuteNonQuery();
            MySqlCommand command2 = new MySqlCommand(updateQuery, mysqlconnection);
            command2.ExecuteNonQuery();
        }
    }
    
    public async Task SelectCustomerIdFromProposal(int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT customer_id FROM PROPOSALS WHERE proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("customer_id")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("customer_id");
                        resultCustomerId.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        int val = 0;
                        resultCustomerId.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<int> ReturnUserIdForProposal(int pid)
    {
        await SelectCustomerIdFromProposal(pid);
        return resultCustomerId.Count > 0 ? resultCustomerId[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCustomerSalutation(int cid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT salutation FROM CUSTOMERS WHERE customer_id = '{cid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("salutation")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("salutation");
                        resultSalutation.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultSalutation.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<string> ReturnSalutation(int cid)
    {
        await SelectCustomerSalutation(cid);
        return resultSalutation.Count > 0 ? resultSalutation[0] : ""; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCustomerFirstName(int cid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT first_name FROM CUSTOMERS WHERE customer_id = '{cid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("first_name")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("first_name");
                        resultFirstName.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultFirstName.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<string> ReturnFirstName(int cid)
    {
        await SelectCustomerFirstName(cid);
        return resultFirstName.Count > 0 ? resultFirstName[0] : ""; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCustomerLastName(int cid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT last_name FROM CUSTOMERS WHERE customer_id = '{cid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("last_name")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("last_name");
                        resultLastName.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultLastName.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<string> ReturnLastName(int cid)
    {
        await SelectCustomerLastName(cid);
        return resultLastName.Count > 0 ? resultLastName[0] : ""; // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectCustomerAddress(int cid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT address FROM CUSTOMERS WHERE customer_id = '{cid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("address")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("address");
                        resultAddress.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultAddress.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<string> ReturnAddress(int cid)
    {
        await SelectCustomerAddress(cid);
        return  resultAddress.Count > 0 ?  resultAddress[0] : ""; // Return the first value if available, otherwise return a default value
    }

    
    public async Task SelectCustomerUID(int cid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT uid_nr FROM CUSTOMERS WHERE customer_id = '{cid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("uid_nr")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("uid_nr");
                        resultUid.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        string val = "";
                        resultUid.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<string> ReturnUID(int cid)
    {
        await SelectCustomerUID(cid);
        return resultUid.Count > 0 ? resultUid[0] : ""; // Return the first value if available, otherwise return a default value
    }
    
    
    
}