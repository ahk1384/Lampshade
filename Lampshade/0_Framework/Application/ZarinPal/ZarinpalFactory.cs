using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
namespace _0_Framework.Application.ZarinPal;

public class ZarinPalFactory : IZarinPalFactory
{
    private readonly string _baseUrl;
    private readonly IConfiguration _configuration;

    public ZarinPalFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        Prefix = _configuration.GetSection("payment")["method"];
        MerchantId = _configuration.GetSection("payment")["merchant"];
        _baseUrl = $"https://sandbox.zarinpal.com/pg/v4/payment";
    }

    private string MerchantId { get; }

    public string Prefix { get; set; }

    public PaymentResponse CreatePaymentRequest(string amount, string mobile, string email, string description,
        long orderId)
    {
        amount = amount.Replace(",", "");
        var finalAmount = int.Parse(amount);
        var siteUrl = _configuration.GetSection("payment")["siteUrl"];

        var client = new RestClient(_baseUrl);
        //var request = new RestRequest(Method.POST);
        var request = new RestRequest("request.json", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        var body = new PaymentRequest
        {
            mobile = mobile,
            callback_url = $"{siteUrl}/Checkout?handler=CallBack&oId={orderId}",
            description = description,
            email = email,
            amount = finalAmount,
            merchant_id = MerchantId,
            currency = "IRT"
        };

        request.AddJsonBody(body);
        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
    }

    public VerificationResponse CreateVerificationRequest(string authority, string amount)
    {
        var client = new RestClient(_baseUrl);
        var request = new RestRequest("verify.json", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        amount = amount.Replace(",", "");
        var finalAmount = int.Parse(amount);

        request.AddJsonBody(new VerificationRequest
        {
            amount = finalAmount,
            merchant_id = MerchantId,
            authority = authority
        });

        var response = client.Execute(request);
        return JsonConvert.DeserializeObject<VerificationResponse>(response.Content);
    }
}