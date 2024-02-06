using MySqlConnector;
using OfficeOpenXml;

namespace DA_Angebotserstellungssoftware;

public class InsertLVsService
{
    private readonly MySqlConnectionManager connection;
    private List<string> resultLVsPV = new List<string>();
    private List<string> resultLVsHausanschluss = new List<string>();
    private List<int> resultCheckIfValuesExistInNormalLVPV = new List<int>();
    private List<int> resultCheckIfValuesExistInNormalLVHausanschluss = new List<int>();
    public InsertLVsService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public async Task CheckPVLVExists(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            //mysqlconnection.Open();
            string selectCustomer = $"SELECT lv_id FROM LVS WHERE lv_type = 'PV' and user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectCustomer, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_id")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        resultCheckIfValuesExistInNormalLVPV.Add(1); // Add the retrieved value to the list
                    }

                    else
                    {
                        resultCheckIfValuesExistInNormalLVPV.Add(0);
                    }
                }
            }
        }
        connection.CloseConnection();
    }
    
    public async Task<int> ReturnCheckIfPVLVExists(int uid, int pid)
    {
        await CheckPVLVExists(uid, pid);
        return resultCheckIfValuesExistInNormalLVPV.Count > 0 ? resultCheckIfValuesExistInNormalLVPV[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task CheckPVHausanschlussExists(int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            //mysqlconnection.Open();
            string selectCustomer = $"SELECT lv_id FROM LVS WHERE lv_type = 'Hausanschluss' and user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectCustomer, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("lv_id")))
                    {
                        // Assuming the column name is 'discount', change it accordingly
                        resultCheckIfValuesExistInNormalLVHausanschluss.Add(1); // Add the retrieved value to the list
                    }

                    else
                    {
                        resultCheckIfValuesExistInNormalLVHausanschluss.Add(0);
                    }
                }
            }
        }
        connection.CloseConnection();
    }
    
    public async Task<int> ReturnCheckIfHausAnschlussLV(int uid, int pid)
    {
        await CheckPVHausanschlussExists(uid, pid);
        return resultCheckIfValuesExistInNormalLVHausanschluss.Count > 0 ? resultCheckIfValuesExistInNormalLVHausanschluss[0] : 0; // Return the first value if available, otherwise return a default value
    }
    
    public async Task SelectPVLV(int doesExist, int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            //mysqlconnection.Open();
            MySqlCommand command1;

            if (doesExist == 1)
            {
                string selectCustomer = $"SELECT short_text FROM LVS WHERE lv_type = 'PV' and oz = '01' and user_id = '{uid}' and proposal_id = '{pid}'";
                command1 = new MySqlCommand(selectCustomer, mysqlconnection);
                command1.ExecuteNonQuery();
            }

            else
            {
                string selectCustomer = $"SELECT short_text FROM MASTER_LVS WHERE lv_type = 'PV'";
                command1 = new MySqlCommand(selectCustomer, mysqlconnection);
                command1.ExecuteNonQuery();
            }
            
           
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("short_text")))
                    {
                        string val = reader.GetString("short_text");
                        resultLVsPV.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
        connection.CloseConnection();
    }
    
    public async Task<string> ReturnPVLV(int doesExist, int uid, int pid)
    {
        await SelectPVLV(doesExist, uid, pid);
        return resultLVsPV.Count > 0 ? resultLVsPV[0] : ""; // Return the first value if available, otherwise return a default value
    } 
    
    public async Task SelectHausanschlussLV(int doesExist, int uid, int pid)
    {
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
           // mysqlconnection.Open();
            MySqlCommand command1;

            if (doesExist == 1)
            {
                string selectCustomer = $"SELECT short_text FROM LVS WHERE lv_type = 'Hausanschluss' and oz = '01' and user_id = '{uid}' and proposal_id = '{pid}'";
                command1 = new MySqlCommand(selectCustomer, mysqlconnection);
                command1.ExecuteNonQuery();
            }

            else
            {
                string selectCustomer = $"SELECT short_text FROM MASTER_LVS WHERE lv_type = 'Hausanschluss'";
                command1 = new MySqlCommand(selectCustomer, mysqlconnection);
                command1.ExecuteNonQuery();
            }
            
           
            
            using (var reader = await command1.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("short_text")))
                    {
                        string val = reader.GetString("short_text");
                        resultLVsHausanschluss.Add(val); // Add the retrieved value to the list
                    }
                }
            }
        }
        connection.CloseConnection();
    }
    
    public async Task<string> ReturnHausanschlussLV(int doesExist, int uid, int pid)
    {
        await SelectHausanschlussLV(doesExist, uid, pid);
        return resultLVsHausanschluss.Count > 0 ? resultLVsHausanschluss[0] : ""; // Return the first value if available, otherwise return a default value
    } 
    
    
    
    
    public void InsertProposal(int pid, int uid)
    {
        using (MySqlConnection mySqlConnection = this.connection.GetConnection())
        {
            MySqlCommand commandA = mySqlConnection.CreateCommand();

            TimeZoneInfo austrianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // CET
            DateTime currentDateTimeInAustria = TimeZoneInfo.ConvertTime(DateTime.Now, austrianTimeZone);
            // Öffne die Verbindung
            //mySqlConnection.Open();

            //Random rand = new Random();
            //int id = rand.Next(1, 5000001);
            commandA.CommandText =
                $"INSERT INTO PROPOSALS (proposal_id, user_id, customer_id, proposal_short, discount, payment_term, skonto_percent, skonto_days, project_name, created_at, updated_at, proposal_price) VALUES ('{pid}', '{uid}', null, null, null, null, null, null, null, '{currentDateTimeInAustria.ToString("yyyy-MM-dd HH:mm:ss")}', null, 0.00)";
            commandA.ExecuteNonQuery();
            commandA.Parameters.Clear();
            
        }
        connection.CloseConnection();
         
    }
    
    // neue InsertLVs-Methode (mit Master LV Tabelle)
    
   public void InsertMasterLVsPV(string filepath)
{
    using (MySqlConnection mySqlConnection = this.connection.GetConnection())
    {
        //mySqlConnection.Open();
        MySqlCommand command = mySqlConnection.CreateCommand();

        using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming it's the first worksheet

            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;

            for (int row = 11; row <= rowCount; row++)
            {
                string valueA = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                string valueB = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                string valueC = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                string valueD = worksheet.Cells[row, 6].Value?.ToString() ?? "";
                string valueE = worksheet.Cells[row, 7].Value?.ToString().Replace(',', '.') ?? "";
                string valueF = worksheet.Cells[row, 9].Value?.ToString() ?? "";
                string valueG = worksheet.Cells[row, 10].Value?.ToString().Replace(',', '.') ?? "";
                string valueH = worksheet.Cells[row, 11].Value?.ToString() ?? "";
                string valueI = worksheet.Cells[row, 20].Value?.ToString().Replace(',', '.') ?? "";
                string valueJ = worksheet.Cells[row, 21].Value?.ToString() ?? "";

                object dbValueA = string.IsNullOrWhiteSpace(valueA) ? DBNull.Value : (object)valueA;
                object dbValueB = string.IsNullOrWhiteSpace(valueB) ? DBNull.Value : (object)valueB;
                object dbValueC = string.IsNullOrWhiteSpace(valueC) ? DBNull.Value : (object)valueC;
                object dbValueD = string.IsNullOrWhiteSpace(valueD) ? DBNull.Value : (object)valueD;
                object dbValueE = string.IsNullOrWhiteSpace(valueE) ? DBNull.Value : (object)valueE;
                object dbValueF = string.IsNullOrWhiteSpace(valueF) ? DBNull.Value : (object)valueF;
                object dbValueG = string.IsNullOrWhiteSpace(valueG) ? DBNull.Value : (object)valueG;
                object dbValueH = string.IsNullOrWhiteSpace(valueH) ? DBNull.Value : (object)valueH;
                object dbValueI = string.IsNullOrWhiteSpace(valueI) ? DBNull.Value : (object)valueI;
                object dbValueJ = string.IsNullOrWhiteSpace(valueJ) ? DBNull.Value : (object)valueJ;

                if (string.IsNullOrWhiteSpace(valueA) &&
                    string.IsNullOrWhiteSpace(valueB) &&
                    string.IsNullOrWhiteSpace(valueC) &&
                    string.IsNullOrWhiteSpace(valueD) &&
                    string.IsNullOrWhiteSpace(valueE) &&
                    string.IsNullOrWhiteSpace(valueF) &&
                    string.IsNullOrWhiteSpace(valueG) &&
                    string.IsNullOrWhiteSpace(valueH) &&
                    string.IsNullOrWhiteSpace(valueI) &&
                    string.IsNullOrWhiteSpace(valueJ))
                {
                    // Skip empty rows
                }
                else
                {
                    command.CommandText = "INSERT INTO MASTER_LVS (lv_type, oz, pa, short_text, long_text, lv_amount, lv_amount_unit, basic_ep, calculated_ep, ep_currency, basic_gb, calculated_gb, gb_currency, effort_factor) " +
                                          "VALUES (@ValueT, @ValueB, @ValueC, @ValueD, @ValueE, @ValueF, @ValueG, @ValueH, @ValueI, @ValueJ, @ValueK, @ValueL, @ValueM, @ValueN)";
                    command.Parameters.AddWithValue("@ValueB", dbValueA);
                    command.Parameters.AddWithValue("@ValueC", DBNull.Value);
                    command.Parameters.AddWithValue("@ValueD", dbValueC);
                    command.Parameters.AddWithValue("@ValueE", dbValueD);
                    command.Parameters.AddWithValue("@ValueF", dbValueE);
                    command.Parameters.AddWithValue("@ValueG", dbValueF);
                    command.Parameters.AddWithValue("@ValueH", dbValueG);
                    command.Parameters.AddWithValue("@ValueI", DBNull.Value);
                    command.Parameters.AddWithValue("@ValueJ", dbValueH);
                    command.Parameters.AddWithValue("@ValueK", dbValueI);
                    command.Parameters.AddWithValue("@ValueL", DBNull.Value);
                    command.Parameters.AddWithValue("@ValueM", "EUR");
                    command.Parameters.AddWithValue("@ValueN", 1.00);
                    command.Parameters.AddWithValue("@ValueT", "PV");

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }
    }
    connection.CloseConnection();
}
   
    public void InsertMasterLVsHausanschluss(string filepath)
{
    using (MySqlConnection mySqlConnection = this.connection.GetConnection())
    {
       // mySqlConnection.Open();
        MySqlCommand command = mySqlConnection.CreateCommand();

        using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming it's the first worksheet

            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;

            for (int row = 11; row <= rowCount; row++)
            {
                string valueA = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                string valueB = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                string valueC = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                string valueD = worksheet.Cells[row, 6].Value?.ToString() ?? "";
                string valueE = worksheet.Cells[row, 7].Value?.ToString().Replace(',', '.') ?? "";
                string valueF = worksheet.Cells[row, 9].Value?.ToString() ?? "";
                string valueG = worksheet.Cells[row, 10].Value?.ToString().Replace(',', '.') ?? "";
                string valueH = worksheet.Cells[row, 11].Value?.ToString() ?? "";
                string valueI = worksheet.Cells[row, 20].Value?.ToString().Replace(',', '.') ?? "";
                string valueJ = worksheet.Cells[row, 21].Value?.ToString() ?? "";

                object dbValueA = string.IsNullOrWhiteSpace(valueA) ? DBNull.Value : (object)valueA;
                object dbValueB = string.IsNullOrWhiteSpace(valueB) ? DBNull.Value : (object)valueB;
                object dbValueC = string.IsNullOrWhiteSpace(valueC) ? DBNull.Value : (object)valueC;
                object dbValueD = string.IsNullOrWhiteSpace(valueD) ? DBNull.Value : (object)valueD;
                object dbValueE = string.IsNullOrWhiteSpace(valueE) ? DBNull.Value : (object)valueE;
                object dbValueF = string.IsNullOrWhiteSpace(valueF) ? DBNull.Value : (object)valueF;
                object dbValueG = string.IsNullOrWhiteSpace(valueG) ? DBNull.Value : (object)valueG;
                object dbValueH = string.IsNullOrWhiteSpace(valueH) ? DBNull.Value : (object)valueH;
                object dbValueI = string.IsNullOrWhiteSpace(valueI) ? DBNull.Value : (object)valueI;
                object dbValueJ = string.IsNullOrWhiteSpace(valueJ) ? DBNull.Value : (object)valueJ;

                if (string.IsNullOrWhiteSpace(valueA) &&
                    string.IsNullOrWhiteSpace(valueB) &&
                    string.IsNullOrWhiteSpace(valueC) &&
                    string.IsNullOrWhiteSpace(valueD) &&
                    string.IsNullOrWhiteSpace(valueE) &&
                    string.IsNullOrWhiteSpace(valueF) &&
                    string.IsNullOrWhiteSpace(valueG) &&
                    string.IsNullOrWhiteSpace(valueH) &&
                    string.IsNullOrWhiteSpace(valueI) &&
                    string.IsNullOrWhiteSpace(valueJ))
                {
                    // Skip empty rows
                }
                else
                {
                    command.CommandText = "INSERT INTO MASTER_LVS (lv_type, oz, pa, short_text, long_text, lv_amount, lv_amount_unit, basic_ep, calculated_ep, ep_currency, basic_gb, calculated_gb, gb_currency, effort_factor) " +
                                          "VALUES (@ValueT, @ValueB, @ValueC, @ValueD, @ValueE, @ValueF, @ValueG, @ValueH, @ValueI, @ValueJ, @ValueK, @ValueL, @ValueM, @ValueN)";
                    command.Parameters.AddWithValue("@ValueB", dbValueA);
                    command.Parameters.AddWithValue("@ValueC", DBNull.Value);
                    command.Parameters.AddWithValue("@ValueD", dbValueC);
                    command.Parameters.AddWithValue("@ValueE", dbValueD);
                    command.Parameters.AddWithValue("@ValueF", dbValueE);
                    command.Parameters.AddWithValue("@ValueG", dbValueF);
                    command.Parameters.AddWithValue("@ValueH", dbValueG);
                    command.Parameters.AddWithValue("@ValueI", DBNull.Value);
                    command.Parameters.AddWithValue("@ValueJ", dbValueH);
                    command.Parameters.AddWithValue("@ValueK", dbValueI);
                    command.Parameters.AddWithValue("@ValueL", DBNull.Value);
                    command.Parameters.AddWithValue("@ValueM", "EUR");
                    command.Parameters.AddWithValue("@ValueN", 1.00);
                    command.Parameters.AddWithValue("@ValueT", "Hausanschluss");

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }
    }
    connection.CloseConnection();
}

    
    
}