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
    private List<int> resultCheckIfUserExists = new List<int>();
    private readonly MySqlConnectionManager connection;
    
    public InsertCustomerDataService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public void InsertCustomerData(string salutation, string firstname, string lastname, string address, string uid,  int customerid, int checkIfUserExists, int userid, int proposalid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            string insertQuery;
            string updateQuery;
            string updateQuery2;
            mysqlconnection.Open();
            
            if (checkIfUserExists  != 0)
            {
                //Console.WriteLine("do muas i eine");
                //Console.WriteLine(checkIfUserExists);
                if (uid == null || uid == "")
                {
                    updateQuery =
                        $"UPDATE CUSTOMERS SET salutation = '{salutation}', first_name = '{firstname}', last_name = '{lastname}', address = '{address}', uid_nr =  NULL WHERE customer_id = '{checkIfUserExists}'";
                    TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
                    DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
                    updateQuery2 = $"UPDATE PROPOSALS SET updated_at = '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}', customer_id = '{checkIfUserExists}' WHERE user_id ='{userid}' and proposal_id = '{proposalid}'";
                    
                    MySqlCommand command11 = new MySqlCommand(updateQuery, mysqlconnection);
                    command11.ExecuteNonQuery();
                    MySqlCommand command33 = new MySqlCommand(updateQuery2, mysqlconnection);
                    command33.ExecuteNonQuery();

                }

                else
                {
                    updateQuery =
                        $"UPDATE CUSTOMERS SET salutation = '{salutation}', first_name = '{firstname}', last_name = '{lastname}', address = '{address}', uid_nr = '{uid}' WHERE customer_id = '{customerid}'";
                    TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
                    DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
                    updateQuery2 = $"UPDATE PROPOSALS SET updated_at = '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}', customer_id = '{checkIfUserExists}' WHERE user_id ='{userid}' and proposal_id = '{proposalid}'";
                    
                    MySqlCommand command1 = new MySqlCommand(updateQuery, mysqlconnection);
                    command1.ExecuteNonQuery();
                    MySqlCommand command3 = new MySqlCommand(updateQuery2, mysqlconnection);
                    command3.ExecuteNonQuery();
                }
               
            }

            else
            {
                string selectProposal = $"SELECT customer_id FROM PROPOSALS WHERE user_id ='{userid}' and proposal_id = '{proposalid}'";
                MySqlCommand commandA = new MySqlCommand(selectProposal, mysqlconnection);
                object currentProposal = commandA.ExecuteScalar();
                int currentProposalId = (currentProposal != null && currentProposal != DBNull.Value) ? Convert.ToInt32(currentProposal) : 0;

                
                if (customerid == currentProposalId)
                {
                    if (uid == null || uid == "")
                    {
                        updateQuery =
                            $"UPDATE CUSTOMERS SET salutation = '{salutation}', first_name = '{firstname}', last_name = '{lastname}', address = '{address}', uid_nr =  NULL WHERE customer_id = '{customerid}'";
                        TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
                        DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
                        updateQuery2 = $"UPDATE PROPOSALS SET updated_at = '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE user_id ='{userid}' and proposal_id = '{proposalid}'";

                        
                        MySqlCommand command111 = new MySqlCommand(updateQuery, mysqlconnection);
                        command111.ExecuteNonQuery();
                        MySqlCommand command333 = new MySqlCommand(updateQuery2, mysqlconnection);
                        command333.ExecuteNonQuery();
                    }

                    else
                    {
                        updateQuery =
                            $"UPDATE CUSTOMERS SET salutation = '{salutation}', first_name = '{firstname}', last_name = '{lastname}', address = '{address}', uid_nr = '{uid}' WHERE customer_id = '{customerid}'";
                        TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
                        DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
                        updateQuery2 = $"UPDATE PROPOSALS SET updated_at = '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE user_id ='{userid}' and proposal_id = '{proposalid}'";
                        
                        MySqlCommand command1 = new MySqlCommand(updateQuery, mysqlconnection);
                        command1.ExecuteNonQuery();
                        MySqlCommand command3 = new MySqlCommand(updateQuery2, mysqlconnection);
                        command3.ExecuteNonQuery();
                    }
                   
                }

                else
                {
                    if (uid == null || uid == "")
                    {
                        insertQuery = $"INSERT INTO CUSTOMERS (customer_id, salutation, first_name, last_name, address, uid_nr) VALUES ('{customerid}', '{salutation}', '{firstname}', '{lastname}', '{address}', NULL)";
                        updateQuery =  $"UPDATE PROPOSALS SET customer_id = '{customerid}' WHERE proposal_id = '{proposalid}' and user_id = '{userid}'";

                    }

                    else
                    {
                        insertQuery = $"INSERT INTO CUSTOMERS (customer_id, salutation, first_name, last_name, address, uid_nr) VALUES ('{customerid}','{salutation}', '{firstname}', '{lastname}', '{address}', '{uid}')";
                        updateQuery =  $"UPDATE PROPOSALS SET customer_id = '{customerid}'  WHERE proposal_id = '{proposalid}'  and user_id = '{userid}'";
                    }
                    MySqlCommand command1 = new MySqlCommand(insertQuery, mysqlconnection);
                    command1.ExecuteNonQuery();
                    MySqlCommand command2 = new MySqlCommand(updateQuery, mysqlconnection);
                    command2.ExecuteNonQuery();
                }
               
                
            }
            
        }
    }
    
    public async Task SelectCustomerIdFromProposal(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT customer_id FROM PROPOSALS WHERE proposal_id = '{pid}' and user_id = '{uid}'";
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
                        Random rand = new Random();
                        int val = rand.Next(1, 90001);
                        resultCustomerId.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<int> ReturnUserIdForProposal(int uid, int pid)
    {
        await SelectCustomerIdFromProposal(uid, pid);
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
    
    public async Task CheckIfCustomerExists(string salutation, string fname, string lname, string address)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectCustomer = $"SELECT customer_id FROM CUSTOMERS WHERE salutation = '{salutation}' and first_name = '{fname}' and last_name = '{lname}' and address = '{address}'";
            MySqlCommand command1 = new MySqlCommand(selectCustomer, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("customer_id")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        int val = reader.GetInt32("customer_id");
                        resultCheckIfUserExists.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        int val = 0;
                        resultCheckIfUserExists.Add(val);
                    }
                }
            }
        }
    }
    
    public async Task<int> ReturnCheckIfUserExists(string salutation, string fname, string lname, string address)
    {
        await CheckIfCustomerExists(salutation, fname, lname, address);
        return resultCheckIfUserExists.Count > 0 ? resultCheckIfUserExists[0] : 0; // Return the first value if available, otherwise return a default value
    }

    
}