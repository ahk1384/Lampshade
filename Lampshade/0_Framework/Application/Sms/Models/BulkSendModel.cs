namespace _0_Framework.Application.Sms.Models;

public class BulkSendModel
{
    public BulkSendModel(long lineNumber, string messageText, string[] mobiles)
    {
        this.lineNumber = lineNumber;
        MessageText = messageText;
        Mobiles = mobiles;
    }

    public long lineNumber { get; set; }
    public string MessageText { get; set; }
    public string[] Mobiles { get; set; }
}