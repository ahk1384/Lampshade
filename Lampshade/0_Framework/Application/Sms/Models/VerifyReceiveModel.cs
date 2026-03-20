namespace _0_Framework.Application.Sms.Models;

public class VerifyReceiveModel
{
    public int status { get; set; }
    public string message { get; set; }
    public VerifyReceiveDataModel data { get; set; }
}