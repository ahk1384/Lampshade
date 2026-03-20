namespace _0_Framework.Application.ZarinPal;

public class PaymentResponse
{
    public Data data { get; set; }
    // public Error[] errors { get; set; }
}

public class Data
{
    public string authority { get; set; }
    public int code { get; set; }
    public string message { get; set; }
    public string card_hash { get; set; }
    public string card_pan { get; set; }
    public int ref_id { get; set; }
    public string fee_type { get; set; }
    public int fee { get; set; }
}

public class Error
{
    public string message { get; set; }

    public int code { get; set; }

    public string[] validations { get; set; }
}