using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace DA_PDF_Generierung_Result.Controllers;

[ApiController]
[Route("/Generation")]
public class PDFController : ControllerBase
{
    private readonly PdfBackendAccess _backendAccess;

    public PDFController(PdfBackendAccess backendAccess)
    {
        _backendAccess = backendAccess;
    }
    
    [HttpPost]
    public async Task<IActionResult> GenerateOffer([FromBody] Pdf pdfData)
    {
        List<Pdf> pdfDatas = new List<Pdf>();
        
        ProgramLogic.overviewList = new List<List<Tuple<string, string, double>>>();
        ProgramLogic.innerList = new List<Tuple<string, string, double>>();
        ProgramLogic.yPos = 546;
        ProgramLogic.PageCnt = 0;
        ProgramLogic.SubLvPrice = 0.0;
        ProgramLogic.LvPrice = 0.0;
        ProgramLogic.TupleSubLvPrice = 0.0;
        ProgramLogic.TupleLvPrice = 0.0;
        ProgramLogic.proposalId = pdfData.proposalId;
        ProgramLogic.LvName = "";
        ProgramLogic.TotalPriceOfLv = 0.0;
        ProgramLogic.DiscountValue = "0.00";
        ProgramLogic.userId = pdfData.userId;
        
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine("Current working directory: " + currentDirectory);
        string path = PathClass.CreatPathForPdf(pdfData.proposalId, pdfData.userId);
        var d = new Pdf
        {
            pdfPath = path,
            proposalId = pdfData.proposalId,
            userId = pdfData.userId
        };
        
        _backendAccess.InsertPdfPath(d.userId, d.proposalId, d.pdfPath);
        PriceCalculator.CalculatePrice(d.proposalId, d.userId);
        ProgramLogic.ProgramLogicWorkflow(path);
        pdfDatas.Add(d);
        
        
        return Ok(pdfDatas);
    }

    [HttpGet]
    public async Task<IActionResult> GetPdfPath(int proposal_id, int user_id)
    {
        List<Pdf> pdfDatas = new List<Pdf>();
        
        var d = new Pdf
        {
            pdfPath = _backendAccess.GetPdfPath(user_id, proposal_id),
            proposalId = proposal_id,
            userId = user_id
        };
        
        pdfDatas.Add(d);
        return Ok(pdfDatas[0]);
    }
    
    [HttpPost]
    [Route("InsertSignature")]
    public async Task<IActionResult> InsertSignature([FromBody] SignatureData signatureData)
    {
        Console.WriteLine("Hallo Signature Methode from Generate");
        // Decode the base64 signature image
        byte[] signatureBytes = Convert.FromBase64String(signatureData.SignatureImage.Split(',')[1]);

        // Get the path of the PDF file
        string pdfPath = _backendAccess.GetPdfPath(signatureData.UserId, signatureData.ProposalId);
        
        
        PdfDocument pdfDocument = new PdfDocument(new PdfReader(_backendAccess.GetPdfPath(signatureData.UserId, signatureData.ProposalId)), new PdfWriter(_backendAccess.GetPdfPath(signatureData.UserId, signatureData.ProposalId) + "_signature.pdf"));
        Document document = new Document(pdfDocument);
        ImageData imageData = ImageDataFactory.Create(signatureBytes);
        Image image = new Image(imageData).ScaleAbsolute(200, 35).SetFixedPosition(1, 340, 155);
        Image image2 = new Image(imageData).ScaleAbsolute(200, 35).SetFixedPosition(pdfDocument.GetNumberOfPages(), 326, 530);
        document.Add(image);
        document.Add(image2);
        document.Close();

        _backendAccess.InsertPdfPath(signatureData.UserId, signatureData.ProposalId, pdfPath + "_signature.pdf");
        
        // Return the path of the signed PDF
        return Ok(new { SignedPdfPath = pdfPath + "_signed" });
    }

}

public class SignatureData
{
    public string SignatureImage { get; set; }
    public int ProposalId { get; set; }
    public int UserId { get; set; }
}


public class Pdf
{
    public string pdfPath { get; set; }
    public int proposalId { get; set; }
    public int userId { get; set; }
}