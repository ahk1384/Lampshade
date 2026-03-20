namespace _0_Framework.Application.ZarinPal;

public class PaymentRequest
{
    public string mobile { get; set; } // mobile -> zarinpal dose not understand it !!!
    public string email { get; set; }
    public string callback_url { get; set; }
    public string description { get; set; }
    public int amount { get; set; }
    public string merchant_id { get; set; }

    public string currency { get; set; }
}