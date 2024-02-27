namespace DA_PDF_Generierung_Result;

public class PathClass
{
    public static string CreatPathForPdf(int proposal_id, int user_id)
    {
        string workDirectory =
            @"G:\5CHIT\Diplomarbeit\DA-aktuell\DA_Angebotserstellungssoftware\DA_Angebotserstellungssoftware\wwwroot";
        string userFolderName = $"{user_id}";
        string userFolderPath = Path.Combine(workDirectory, "pdfs", userFolderName);
        if (!Directory.Exists(userFolderPath))
        {
            Directory.CreateDirectory(userFolderPath);
        }
        string pdfFileName = $"{user_id}_{proposal_id}_Angebot.pdf";
        string pdfFilePath = Path.Combine(userFolderPath, pdfFileName);

        return pdfFilePath;
    }
}