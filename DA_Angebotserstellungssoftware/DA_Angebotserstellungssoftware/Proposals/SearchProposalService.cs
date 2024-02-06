using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class SearchProposalService
{
    private readonly MySqlConnectionManager connection;

    private List<string> resultProjects = new List<string>();
    private List<string> resultCustomers = new List<string>();
    public List<string> resultProposals = new List<string>();
    private List<string> resultFoundProposals = new List<string>();
    private List<int> resultCustomerNameToID = new List<int>();
    private List<DateTime> resultProposalLastUpdatedAt = new List<DateTime>();
    private List<DateTime> resultProposalLastUpdatedAtforMultipleProjects = new List<DateTime>();
    private List<int> resultProposalId = new List<int>();

    public SearchProposalService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task SelectProjectNames(int uid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT project_name FROM PROPOSALS where user_id = '{uid}'";
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
                        resultProjects.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnProjectNamesList(int uid)
    {
        await SelectProjectNames(uid);
        return  resultProjects.Count > 0 ? resultProjects : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectCustomers()
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT first_name, last_name FROM CUSTOMERS ";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("first_name")) && !reader.IsDBNull(reader.GetOrdinal("last_name")))
                    {
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        string val = $"{firstName} {lastName}"; // Kombiniere die Werte der beiden Spalten
                        resultCustomers.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnCustomersList()
    {
        await SelectCustomers();
        return  resultCustomers.Count > 0 ? resultCustomers : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectProposalShort(int uid, int limit)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT proposal_short FROM PROPOSALS where user_id = '{uid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC LIMIT {limit}";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("proposal_short")))
                    {
                        string val = reader.GetString("proposal_short");
                        resultProposals.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnProposalShortList(int uid, int limit)
    {
        await SelectProposalShort(uid, limit);
        return resultProposals.Count > 0 ? resultProposals : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
     public async Task SelectProjectNamesProjectSearch(int uid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT DISTINCT project_name FROM PROPOSALS where user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            resultProjects.Add("Projektname auswählen");
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("project_name")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        string val = reader.GetString("project_name");
                        resultProjects.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnProjectNamesListProjectSearch(int uid)
    {
        await SelectProjectNamesProjectSearch(uid);
        return  resultProjects.Count > 0 ? resultProjects : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectCustomersProjectSearch()
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT first_name, last_name FROM CUSTOMERS ";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            resultCustomers.Add("Kundenname auswählen");
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("first_name")) && !reader.IsDBNull(reader.GetOrdinal("last_name")))
                    {
                        string firstName = reader.GetString("first_name");
                        string lastName = reader.GetString("last_name");
                        string val = $"{firstName} {lastName}"; // Kombiniere die Werte der beiden Spalten
                        resultCustomers.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnCustomersListProjectSearch()
    {
        await SelectCustomersProjectSearch();
        return  resultCustomers.Count > 0 ? resultCustomers : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectProposalShortProjectSearch(int uid, int limit)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            string selectProposal = $"SELECT proposal_short FROM PROPOSALS where user_id = '{uid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC LIMIT {limit}";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            resultProposals.Add("Angebotsname auswählen");
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("proposal_short")))
                    {
                        string val = reader.GetString("proposal_short");
                        resultProposals.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnProposalShortListProjectSearch(int uid, int limit)
    {
        await SelectProposalShortProjectSearch(uid, limit);
        return resultProposals.Count > 0 ? resultProposals : new List<string>(); // Return the first value if available, otherwise return a default value
    }

    public async Task SearchCustomerNameToID(string name)
    {
        string firstName;
        string lastName;
        if (name != "" && !string.IsNullOrEmpty(name) && name != "Kundenname auswählen")
        {
            string[] nameParts = name.Split(' ');
            firstName = nameParts[0]; // Der erste Teil ist der Vorname
            lastName = nameParts[1]; // Der zweite Teil ist der Nachname
        }

        else
        {
            firstName = "";
            lastName = "";
        }


        using (MySqlConnection mysqlconnection = connection.GetConnection())
            {
               // mysqlconnection.Open();
                string selectProposal =
                    $"SELECT customer_id FROM CUSTOMERS WHERE first_name = '{firstName}' and last_name = '{lastName}'";
                MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine(selectProposal);

                using (var reader = await command1.ExecuteReaderAsync())
                {
                    resultCustomerNameToID.Clear();
                    while (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("customer_id")))
                        {
                            int val = reader.GetInt32("customer_id");
                            Console.WriteLine(val);
                            resultCustomerNameToID.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                        }

                        else
                        {
                            int val = 0;
                            resultCustomerNameToID.Add(val);
                        }
                    }
                }
            }
        connection.CloseConnection();
    }

    public async Task<int> ReturnCustomerNameToID(string name)
    {
        await SearchCustomerNameToID(name);
        return resultCustomerNameToID.Count > 0 ? resultCustomerNameToID[0] : 0; // Return the first value if available, otherwise return a default value
    }

    public async Task SearchProposal(int userid, string projectname, int customer_id, string proposal_short)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

           // mysqlconnection.Open();
            string selectProposal;
            MySqlCommand command1 = new MySqlCommand();

            if ((projectname == "" || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname)) &&
                (proposal_short == "" || proposal_short == "Angebotsname auswählen" ||
                 string.IsNullOrEmpty(proposal_short)) && customer_id == 0)
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("7");
            }
            
            else if ((projectname == ""  || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname)) && customer_id == 0)
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("1");
            }

            else if ((projectname == ""  || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname)) && (proposal_short == "" || proposal_short == "Angebotsname auswählen" || string.IsNullOrEmpty(proposal_short)))
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE customer_id = '{customer_id}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("2");
            }

            else if (customer_id == 0 && (proposal_short == "" || proposal_short == "Angebotsname auswählen" || string.IsNullOrEmpty(proposal_short)))
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("3");
            }

            else if (projectname == ""  || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname))
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and customer_id = '{customer_id}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("4");
            }
            
            else if (customer_id == 0)
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("5");
            }

            else if (proposal_short == "" || proposal_short == "Angebotsname auswählen" || string.IsNullOrEmpty(proposal_short))
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE customer_id = '{customer_id}' and project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("6");
            }

            else
            {
                selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and customer_id = '{customer_id}' and project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("8");
            }
            
            
            
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("proposal_id")) && !reader.IsDBNull(reader.GetOrdinal("proposal_short")))
                    {
                        string proid = reader.GetInt32("proposal_id").ToString();
                        string proshort = reader.GetString("proposal_short");
                        string val = $"{proid} {proshort}";
                        resultFoundProposals.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }

                    else
                    {
                        string val = "Nothing found";
                        resultFoundProposals.Add(val);
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<string>> ReturnFoundProposal(int uid, string projectname, int customer_id, string proposal_short)
    {
        await SearchProposal(uid, projectname, customer_id, proposal_short);
        return resultFoundProposals.Count > 0 ? resultFoundProposals : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SearchProposalLastUpdatedAt(int userid, int limit)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            string selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC LIMIT {limit}";


            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("updated_at")))
                    {
                        DateTime val = reader.GetDateTime("updated_at");
                        string converted = val.ToString("yyyy-MM-dd HH:mm:ss");
                        val = DateTime.Parse(converted);
                        resultProposalLastUpdatedAt.Add(val); 
                    }

                    else if (!reader.IsDBNull(reader.GetOrdinal("created_at")))
                    {
                        DateTime val = reader.GetDateTime("created_at");
                        string converted = val.ToString("yyyy-MM-dd HH:mm:ss");
                        val = DateTime.Parse(converted);
                        resultProposalLastUpdatedAt.Add(val); 
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<DateTime>> ReturnProposalLastUpdatedAtList(int userid, int limit)
    {
        await SearchProposalLastUpdatedAt(userid, limit);
        return resultProposalLastUpdatedAt.Count > 0 ? resultProposalLastUpdatedAt : new List<DateTime>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SearchProposalLastUpdatedAtForProjectSearch(int userid, string projectname, int customer_id, string proposal_short)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            //mysqlconnection.Open();
            TimeZoneInfo cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            MySqlCommand command1 = new MySqlCommand();

            string selectProposal;

            if ((projectname == "" || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname)) &&
                (proposal_short == "" || proposal_short == "Angebotsname auswählen" ||
                 string.IsNullOrEmpty(proposal_short)) && customer_id == 0)
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("7");
            }
            
            else if ((projectname == ""  || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname)) && customer_id == 0)
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("1");
            }

            else if ((projectname == ""  || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname)) && (proposal_short == "" || proposal_short == "Angebotsname auswählen" || string.IsNullOrEmpty(proposal_short)))
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE customer_id = '{customer_id}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("2");
            }
            
            else if (customer_id == 0 && (proposal_short == "" || proposal_short == "Angebotsname auswählen" || string.IsNullOrEmpty(proposal_short)))
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("3");
            }

            else if (projectname == ""  || projectname == "Projektname auswählen" || string.IsNullOrEmpty(projectname))
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and customer_id = '{customer_id}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("4");
            }
            
            else if (customer_id == 0)
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("5");
            }

            else if (proposal_short == "" || proposal_short == "Angebotsname auswählen" || string.IsNullOrEmpty(proposal_short))
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE customer_id = '{customer_id}' and project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("6");
            }

            else
            {
                selectProposal = $"SELECT created_at, updated_at FROM PROPOSALS WHERE proposal_short = '{proposal_short}' and customer_id = '{customer_id}' and project_name = '{projectname}' and user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC";
                command1 = new MySqlCommand(selectProposal, mysqlconnection);
                command1.ExecuteNonQuery();
                Console.WriteLine("8");
            }
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("updated_at")))
                    {
                        DateTime val = reader.GetDateTime("updated_at");
                        string converted = val.ToString("yyyy-MM-dd HH:mm:ss");
                        val = DateTime.Parse(converted);
                        resultProposalLastUpdatedAt.Add(val); 
                    }

                    else if (!reader.IsDBNull(reader.GetOrdinal("created_at")))
                    {
                        DateTime val = reader.GetDateTime("created_at");
                        string converted = val.ToString("yyyy-MM-dd HH:mm:ss");
                        val = DateTime.Parse(converted);
                        resultProposalLastUpdatedAt.Add(val); 
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<DateTime>> ReturnProposalLastUpdatedAtListForProjectSearch(int userid, string projectname, int customer_id, string proposal_short)
    {
        await SearchProposalLastUpdatedAtForProjectSearch(userid, projectname, customer_id, proposal_short);
        return resultProposalLastUpdatedAt.Count > 0 ? resultProposalLastUpdatedAt : new List<DateTime>(); // Return the first value if available, otherwise return a default value
    }

    
    public async Task SearchProposalId(int userid, int limit)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

           // mysqlconnection.Open();
            string selectProposal = $"SELECT proposal_id FROM PROPOSALS WHERE user_id = '{userid}' ORDER BY CASE WHEN updated_at IS NULL THEN created_at ELSE updated_at END DESC LIMIT {limit}";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("proposal_id")))
                    {
                        int val = reader.GetInt32("proposal_id");
                        resultProposalId.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }
                }
            }
        }
        connection.CloseConnection();
    }

    public async Task<List<int>> ReturnProposalIdList(int userid, int limit)
    {
        await SearchProposalId(userid, limit);
        return resultProposalId.Count > 0 ? resultProposalId : new List<int>(); // Return the first value if available, otherwise return a default value
    }
}