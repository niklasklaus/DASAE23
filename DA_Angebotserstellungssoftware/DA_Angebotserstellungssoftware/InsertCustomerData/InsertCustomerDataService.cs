using MySqlConnector;

namespace DA_Angebotserstellungssoftware.InsertCustomerData;

public class InsertCustomerDataService
{
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
}