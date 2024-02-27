using MySqlConnector;

namespace DA_Angebotserstellungssoftware.Proposals;

public class UpdateProposalPriceService
{
    private readonly MySqlConnectionManager connection;
    private List<double> resultProposalDiscount = new List<double>();
    private List<double> resultcurrentProposalPrice = new List<double>();
    private List<double> resultCalculatedGb = new List<double>();
    
    
    public UpdateProposalPriceService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }


    public async Task SelectProposalDiscount(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT discount FROM PROPOSALS where user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("discount")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("discount");
                        resultProposalDiscount.Add(val); // Add the retrieved value to the list
                    }

                    else
                    {
                        double val = 0.00;
                        resultProposalDiscount.Add(val);
                    }
                }
            }
        }
    }

    public async Task<double> ReturnProposalDiscount(int uid, int pid)
    {
        await SelectProposalDiscount(uid, pid);
        return  resultProposalDiscount.Count > 0 ? resultProposalDiscount[0] : 0.00; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectProposalPrice(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT proposal_price FROM PROPOSALS where user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("proposal_price")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("proposal_price");
                        resultcurrentProposalPrice.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }

    public async Task<double> ReturnProposalPrice(int uid, int pid)
    {
        await SelectProposalPrice(uid, pid);
        return  resultcurrentProposalPrice.Count > 0 ? resultcurrentProposalPrice[0] : 0.00; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectCalculatedGb(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {

            mysqlconnection.Open();
            string selectProposal = $"SELECT calculated_gb FROM LVS where user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectProposal, mysqlconnection);
            command1.ExecuteNonQuery();
            
            resultCalculatedGb.Clear();
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("calculated_gb")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        double val = reader.GetDouble("calculated_gb");
                        resultCalculatedGb.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
    }

    public async Task<List<double>> ReturnCalculatedGb(int uid, int pid)
    {
        await SelectCalculatedGb(uid, pid);
        return  resultCalculatedGb.Count > 0 ? resultCalculatedGb : new List<double>(); // Return the first value if available, otherwise return a default value
    }

    public async Task<double> UpdateProposalPriceWithCalculatedGb(int uid, int pid, List<double> calculatedGbs, double currentproposalPrice)
    {
        double Price = 0.00;
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            
            mysqlconnection.Open();
            if (currentproposalPrice >= 0.00)
            {
                string updateProposal1 = $"UPDATE PROPOSALS SET proposal_price = 0.00 WHERE user_id = '{uid}' AND proposal_id = '{pid}'";
                MySqlCommand command1 = new MySqlCommand(updateProposal1, mysqlconnection);
                command1.ExecuteNonQuery();

                foreach (double amount in calculatedGbs)
                {
                    string updateProposalX =
                        $"UPDATE PROPOSALS SET proposal_price = CAST(REPLACE(proposal_price, ',', '.') AS DECIMAL(10,2)) + CAST(REPLACE('{amount}', ',', '.') AS DECIMAL(10,2)) WHERE user_id = '{uid}' AND proposal_id = '{pid}'";
                    //Console.WriteLine(updateProposalX);
                    Price = Price + amount;

                    using (MySqlCommand commandX = new MySqlCommand(updateProposalX, mysqlconnection))
                    {
                        commandX.ExecuteNonQuery();
                    }
                }
            }

            else
            {
                foreach (double amount in calculatedGbs)
                {
                    //Console.WriteLine(amount);

                    string updateProposalX =
                        $"UPDATE PROPOSALS SET proposal_price = CAST(REPLACE(proposal_price, ',', '.') AS DECIMAL(10,2)) + CAST(REPLACE('{amount}', ',', '.') AS DECIMAL(10,2)) WHERE user_id = '{uid}' AND proposal_id = '{pid}'";
                    //Console.WriteLine(updateProposalX);
                    Price = Price + amount;
                    using (MySqlCommand commandX = new MySqlCommand(updateProposalX, mysqlconnection))
                    {
                        commandX.ExecuteNonQuery();
                    }
                }
            }
            
        }

        return Price;
    }

    
    public async Task UpdateProposalPriceWithDiscount(int uid, int pid, double discount, double currentproposalPrice)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();

            if (currentproposalPrice != 0.00)
            {
                string updateProposal = $"UPDATE PROPOSALS SET proposal_price =  CAST(REPLACE(proposal_price, ',', '.') AS DECIMAL(10,2)) - CAST(REPLACE('{discount}', ',', '.') AS DECIMAL(10,2)) WHERE user_id = '{uid}' and proposal_id = '{pid}'";
                MySqlCommand command1 = new MySqlCommand(updateProposal, mysqlconnection);
                command1.ExecuteNonQuery();   
            }

            else
            {
                Console.WriteLine("Kein Preisnachlass möglich");
            }
            
        }
    }
}