﻿using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class UpdateShortNameService
{
    private readonly MySqlConnectionManager connection;
    private List<string> resultLvName = new List<string>();
    private List<int> resultProposalCustomerId = new List<int>();
    private List<string> resultProposalCustomerName = new List<string>();

    public UpdateShortNameService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public void UpdateProposalShortName(string lv_name, string customer_name, int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            string[] splittedLVName;

            if (lv_name.Contains("-"))
            {
                splittedLVName = lv_name.Split('-');
            }
            else
            {
                splittedLVName = lv_name.Split(' ');
            }
            
            Random rand = new Random();
            int number = rand.Next(1, 11);
            mysqlconnection.Open();
            
            string updateShortName = $"UPDATE PROPOSALS SET proposal_short = '{splittedLVName[0].Trim()}_{customer_name}_{number}' WHERE proposal_id = '{pid}' and user_id = '{uid}'";
            MySqlCommand command1 = new MySqlCommand(updateShortName, mysqlconnection);
            command1.ExecuteNonQuery();
        }
    }
    
    public async Task SelectLVNames(string type)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT short_text FROM MASTER_LVS WHERE lv_type = '{type}'";
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
                        resultLvName.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }

    public async Task<string> ReturnLVName(string type)
    {
        await SelectLVNames(type);
        return  resultLvName.Count > 0 ? resultLvName[0] : ""; // Return the first value if available, otherwise return a default value
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
                        resultProposalCustomerId.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }

    public async Task<int> ReturnCustomerIdFromProposal(int uid, int pid)
    {
        await SelectCustomerIdFromProposal(uid, pid);
        return  resultProposalCustomerId.Count > 0 ? resultProposalCustomerId[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCustomerNameFromProposalCustomer(int cid)
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
                        resultProposalCustomerName.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }

    public async Task<string> ReturnCustomerNameFromProposalCustomer(int cid)
    {
        await SelectCustomerNameFromProposalCustomer(cid);
        return  resultProposalCustomerName.Count > 0 ? resultProposalCustomerName[0] : ""; // Return the first value if available, otherwise return a default value
    }
    
    
}