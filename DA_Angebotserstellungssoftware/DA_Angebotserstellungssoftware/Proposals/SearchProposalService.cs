﻿using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class SearchProposalService
{
    private readonly MySqlConnectionManager connection;

    private List<string> resultProjects = new List<string>();
    private List<string> resultCustomers = new List<string>();
    public List<string> resultProposals = new List<string>();
    private List<string> resultFoundProposals = new List<string>();
    private List<int> resultCustomerNameToID = new List<int>();

    public SearchProposalService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task SelectProjectNames()
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT project_name FROM PROPOSALS";
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
    }

    public async Task<List<string>> ReturnProjectNames()
    {
        await SelectProjectNames();
        return  resultProjects.Count > 0 ? resultProjects : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectCustomers()
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT first_name, last_name FROM CUSTOMERS";
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
    }

    public async Task<List<string>> ReturnCustomers()
    {
        await SelectCustomers();
        return  resultCustomers.Count > 0 ? resultCustomers : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    
    public async Task SelectProposalShort()
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT proposal_short FROM PROPOSALS";
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
    }

    public async Task<List<string>> ReturnProposalShort()
    {
        await SelectProposalShort();
        return resultProposals.Count > 0 ? resultProposals : new List<string>(); // Return the first value if available, otherwise return a default value
    }
    
    public async Task SearchCustomerNameToID(string name)
    {
        string[] nameParts = name.Split(' ');
        string firstName;
        string lastName;
        firstName = nameParts[0]; // Der erste Teil ist der Vorname
        lastName = nameParts[1]; // Der zweite Teil ist der Nachname
        
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectProposal = $"SELECT customer_id FROM CUSTOMERS WHERE first_name = '{firstName}' and last_name = '{lastName}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("customer_id")))
                    {
                        int val = reader.GetInt32("customer_id");
                        resultCustomerNameToID.Add(val); // Füge den kombinierten Wert zur Liste hinzu
                    }
                }
            }
        }
    }

    public async Task<int> ReturnCustomerNameToID(string name)
    {
        await SearchCustomerNameToID(name);
        return resultCustomerNameToID.Count > 0 ? resultCustomerNameToID[0] : 0; // Return the first value if available, otherwise return a default value
    }

    public async Task SearchProposal(string projectname, int customer_id, string proposal_short)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT proposal_id, proposal_short FROM PROPOSALS WHERE project_name = '{projectname}' and proposal_short = '{proposal_short}' and customer_id = '{customer_id}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
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
                }
            }
        }
    }

    public async Task<string> ReturnFoundProposal(string projectname, int customer_id, string proposal_short)
    {
        await SearchProposal(projectname, customer_id, proposal_short);
        return resultFoundProposals.Count > 0 ? resultFoundProposals[0] : ""; // Return the first value if available, otherwise return a default value
    }
}