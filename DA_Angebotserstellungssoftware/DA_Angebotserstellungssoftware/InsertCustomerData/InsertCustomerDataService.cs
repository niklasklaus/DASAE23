using MySqlConnector;

namespace DA_Angebotserstellungssoftware.InsertCustomerData;

public class InsertCustomerDataService
{
    private readonly MySqlConnectionManager connection;
    
    public InsertCustomerDataService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }

    public void InsertCustomerData(string salutation, string firstname, string lastname, string address, string uid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            string insertQuery;
            mysqlconnection.Open();
            if (uid == null || uid == "")
            {
                insertQuery = $"INSERT INTO CUSTOMERS (salutation, first_name, last_name, address, uid_nr) VALUES ('{salutation}', '{firstname}', '{lastname}', '{address}', NULL)";

            }

            else
            {
                insertQuery = $"INSERT INTO CUSTOMERS (salutation, first_name, last_name, address, uid_nr) VALUES ('{salutation}', '{firstname}', '{lastname}', '{address}', '{uid}')";
            }
           
            MySqlCommand command = new MySqlCommand(insertQuery, mysqlconnection);
            command.ExecuteNonQuery();
        }
    }
}