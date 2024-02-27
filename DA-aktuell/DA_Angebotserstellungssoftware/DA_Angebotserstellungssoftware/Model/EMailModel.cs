namespace EMailSendService.Model;

public class EMailModel
{
    public int EMailId { get; set; }
    public int UserId { get; set; }
    public int ProposalId { get; set; }
    public string EMail { get; set; }
    public string Type { get; set; }

}