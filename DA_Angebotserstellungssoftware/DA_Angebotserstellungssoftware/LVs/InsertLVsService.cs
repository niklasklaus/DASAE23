using MySqlConnector;
using OfficeOpenXml;

namespace DA_Angebotserstellungssoftware;

public class InsertLVsService
{
    private readonly MySqlConnectionManager connection;
    
    public InsertLVsService(MySqlConnectionManager connectionManager)
    {
        this.connection = connectionManager;
    }
    
    public void InsertLVs(string filepath, int id)
    {
          // Verbindung zur MySQL-Datenbank herstellen
        string connectionString = "Server=localhost;Database=da_dbschema;User Id=root;Password=root;";
        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand command = connection.CreateCommand();
        MySqlCommand commandA = connection.CreateCommand();


        // Öffne die Verbindung
        connection.Open();

        //Random rand = new Random();
        //int id = rand.Next(1, 5000001);
        commandA.CommandText =
            $"INSERT INTO PROPOSALS (proposal_id, customer_id, proposal_short, discount, payment_term, skonto_percent, skonto_days, project_name, created_at, updated_at) VALUES ('{id}', null, null, null, null, null, null, null, DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'), null)";
        commandA.ExecuteNonQuery();
        commandA.Parameters.Clear();
        
        using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Annahme: Erstes Arbeitsblatt

            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;

            for (int row = 11; row <= rowCount; row++)
            {
                
                
                string valueA = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                string valueB = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                string valueC = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                string valueD = worksheet.Cells[row, 6].Value?.ToString() ?? "";
                string valueE = worksheet.Cells[row, 7].Value?.ToString().Replace(',' ,'.') ?? "";
                string valueF = worksheet.Cells[row, 9].Value?.ToString() ?? "";
                string valueG = worksheet.Cells[row, 10].Value?.ToString().Replace(',' ,'.') ?? "";
                string valueH = worksheet.Cells[row, 11].Value?.ToString() ?? "";
                string valueI = worksheet.Cells[row, 20].Value?.ToString().Replace(',' ,'.') ?? "";
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
                   
                }

                else
                {
                    command.CommandText = "INSERT INTO LVS (proposal_id, oz, pa, short_text, long_text, lv_amount, lv_amount_unit, basic_ep, calculated_ep, ep_currency, basic_gb, calculated_gb, gb_currency, effort_factor) " +
                                          "VALUES (@ValueA, @ValueB, @ValueC, @ValueD, @ValueE, @ValueF, @ValueG, @ValueH, @ValueI, @ValueJ, @ValueK, @ValueL, @ValueM, @ValueN)";
                    command.Parameters.AddWithValue("@ValueA", id);
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
                    command.Parameters.AddWithValue("@ValueN", DBNull.Value);
                
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }

        connection.Close();

        //Console.WriteLine("Daten aus Excel in die Datenbank importiert!");
    }
}