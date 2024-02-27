using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DA_PDF_Generierung_Result;

public class PdfBackendAccess : DbContext
{
    private MySqlConnector _connector = new MySqlConnector();
    
    public PdfBackendAccess(DbContextOptions<PdfBackendAccess> options) : base(options)
    {
    }
    
    
    public void InsertPdfPath(int user_id, int proposal_id, string path)
    {
        using (MySqlConnection mysqlconnection = new MySqlConnection(_connector.GetConeection()))
        {
            mysqlconnection.Open();
            
            string checkAlreadyExist = $"SELECT * FROM PDFS WHERE proposal_id = {proposal_id} AND user_id = {user_id}";
            
            MySqlCommand getCommand = new MySqlCommand(checkAlreadyExist, mysqlconnection);

            object result = getCommand.ExecuteScalar();
        
            // Überprüfen, ob das Ergebnis nicht NULL ist, und es in die Variable path speichern
            if (result != null)
            {
                Console.WriteLine("Already Exits Method Hello");
                string updatePdf = "UPDATE PDFS SET path = @path WHERE proposal_id = @proposal_id AND user_id = @user_id";
                MySqlCommand updateCommand = new MySqlCommand(updatePdf, mysqlconnection);
                updateCommand.Parameters.AddWithValue("@user_id", user_id);
                updateCommand.Parameters.AddWithValue("@proposal_id", proposal_id);
                updateCommand.Parameters.AddWithValue("@path", path);
                updateCommand.ExecuteNonQuery();
            }
            else
            {
                string insertPdf = "INSERT INTO PDFS (user_id, proposal_id, path) VALUES (@user_id, @proposal_id, @path)";

                MySqlCommand insertCommand = new MySqlCommand(insertPdf, mysqlconnection);
                insertCommand.Parameters.AddWithValue("@user_id", user_id);
                insertCommand.Parameters.AddWithValue("@proposal_id", proposal_id);
                insertCommand.Parameters.AddWithValue("@path", path);

                insertCommand.ExecuteNonQuery();
            }
        }
    }

    public string GetPdfPath(int user_id, int proposal_id)
    {
        string path = "";
        using (MySqlConnection mysqlconnection = new MySqlConnection(_connector.GetConeection()))
        {
            mysqlconnection.Open();

            string getPdf = $"SELECT path FROM PDFS WHERE user_id = {user_id} AND proposal_id = {proposal_id}";

            MySqlCommand getCommand = new MySqlCommand(getPdf, mysqlconnection);

            object result = getCommand.ExecuteScalar();
        
            // Überprüfen, ob das Ergebnis nicht NULL ist, und es in die Variable path speichern
            if (result != null)
            {
                path = result.ToString();
            }
        }

        return path;
    }
}

public class Pdf
{
    public int PdfId { get; set; }
    public int UserId { get; set; }
    public int ProposalId { get; set; }
    public string Path { get; set; }
}