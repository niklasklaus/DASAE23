using System.Text;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace DA_Angebotserstellungssoftware.ServiceConnection;

public class GetGenerationServiceData
{
    public List<PDFData> GetPlayers(int id, int userId)
    {
        List<PDFData> data = new();
        Console.WriteLine(id);
        List<PDFData> players = new List<PDFData>();
        string content = HttpGetAsync($"http://localhost:5552/Generation?proposal_id={id}&user_id={userId}").Result;
        data = JsonConvert.DeserializeObject<List<PDFData>>(content);

        Console.WriteLine(content);
        return data;
    }
    
    public async static Task<string> HttpGetAsync(string uri)
    {
        string content = null;

        var client = new HttpClient();
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            content = await response.Content.ReadAsStringAsync();
        }

        return content;
    }
    
    public void WritePDFData(int proposalId, int userId)
    {
        var pdfData = new PDFData()
        {
            proposalId = proposalId,
            userId = userId,
            pdfPath = ""
        };

        List<PDFData> pdfDataList = new List<PDFData>();
        pdfDataList.Add(pdfData);
        
        foreach (var data in pdfDataList)
        {
            string json = JsonSerializer.Serialize(data);
            //Console.WriteLine(json);
            PostAsync("http://localhost:5552/Generation", json);
        }
        
    }
    
    public void WritePDFDataSignature(int proposalId, int userId, string image)
    {
        var pdfSignatureData = new SignatureData()
        {
            SignatureImage = image,
            UserId = userId,
            ProposalId = proposalId
        };

        List<SignatureData> pdfSignatureDataList = new List<SignatureData>();
        pdfSignatureDataList.Add(pdfSignatureData);
        
        foreach (var data in pdfSignatureDataList)
        {
            string json = JsonSerializer.Serialize(data);
            //Console.WriteLine(json);
            PostAsync("http://localhost:5552/Generation/InsertSignature", json);
        }
        
    }
    
    public async static Task PostAsync(string uri, string json)
    {
        Console.WriteLine("Test");
        var client = new HttpClient();
        var response = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Success");
        }

        
    }
}

public class PDFData
{
    public string pdfPath { get; set; }
    public int proposalId { get; set; }
    public int userId { get; set; }
    
}

public class SignatureData
{
    public string SignatureImage { get; set; }
    public int ProposalId { get; set; }
    public int UserId { get; set; }
}