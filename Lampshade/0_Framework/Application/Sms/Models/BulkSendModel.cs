namespace _0_Framework.Application.Sms.Models;

public class BulkRecieveModel
{
    public long lineNumber { get; set; }
    public string MessageText { get; set; }
    public string[] Mobiles { get; set; }

    public BulkRecieveModel(long lineNumber, string messageText, string[] mobiles)
    {
        this.lineNumber = lineNumber;
        MessageText = messageText;
        Mobiles = mobiles;
    }
}