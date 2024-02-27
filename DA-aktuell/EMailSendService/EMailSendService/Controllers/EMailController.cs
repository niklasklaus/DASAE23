using DA_Angebotserstellungssoftware;
using EMailSendService.DBStatements;
using EMailSendService.Model;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EMailSendService.Controllers;

[ApiController]
[Route("[controller]")]
public class EMailController : ControllerBase
{
    private readonly MySqlConnectionManager connection;
    private readonly PerformedStatements _performedStatements;
    private readonly string sendGridApiKey = "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";

    public EMailController(MySqlConnectionManager connection)
    {
        this.connection = connection;
        _performedStatements = new PerformedStatements(connection);
    }

    [HttpGet("NeededEmails")] 
    public ActionResult<IEnumerable<EMailModel>> GetNeededEmails(int uid, int pid)
    {
        List<EMailModel> emails = new List<EMailModel>();
        
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectEMails = $"SELECT * FROM EMAILS WHERE user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectEMails, mysqlconnection);
            command1.ExecuteNonQuery();
            
            using (var reader =  command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    EMailModel model = new EMailModel()
                    {
                        EMailId = reader.GetInt32(reader.GetOrdinal("email_id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                        ProposalId = reader.GetInt32(reader.GetOrdinal("proposal_id")),
                        EMail = reader.GetString(reader.GetOrdinal("email")),
                        Type = reader.GetString(reader.GetOrdinal("type"))
                    };
                    
                    emails.Add(model);
                }
            }
        }
        return Ok(emails);
    }
    
    [HttpGet("ProposalName")] 
    public IActionResult GetProposalName(int uid, int pid)
    {
        List<Proposals> proposals = new List<Proposals>();
        using (MySqlConnection mysqlconnection = connection.GetConnection())
        {
            mysqlconnection.Open();
            string selectEMails = $"SELECT proposal_short FROM PROPOSALS WHERE user_id = '{uid}' and proposal_id = '{pid}'";
            MySqlCommand command1 = new MySqlCommand(selectEMails, mysqlconnection);
            command1.ExecuteNonQuery();
        
            using (var reader =  command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    var proposal = new Proposals()
                    {
                        Name = reader.GetString(reader.GetOrdinal("proposal_short")),
                    };
                
                    proposals.Add(proposal);
                }
            }
        }

        Console.WriteLine(proposals[0].Name);
        return Ok(proposals[0]);
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailWithPDF([FromForm] string[] emailAddresses, [FromForm] string pdfPath, [FromForm] string pdfName)
    {
        try
        {
            var client = new SendGridClient(sendGridApiKey);
            var message = new SendGridMessage();

            foreach (var email in emailAddresses)
            {
                message.AddTo(email);
            }

            message.SetFrom(new EmailAddress("n.klaus@htlkrems.at", "Niklas Klaus"));
            message.SetSubject("PDF Document");
            message.AddContent(MimeType.Text, "Im Anhang befindet sich das fertig unterschriebene Angebot");

            var pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            var pdfBase64 = Convert.ToBase64String(pdfBytes);
            message.AddAttachment($"{pdfName}.pdf", pdfBase64, "application/pdf");

            var response = await client.SendEmailAsync(message);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                return StatusCode((int)response.StatusCode, $"Failed to send email. Status code: {response.StatusCode}");
            }

            return Ok("Email sent successfully");
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, $"Failed to send email: {ex.Message}");
        }
    }
    
    
}