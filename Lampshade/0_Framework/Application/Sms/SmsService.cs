using System.Text;
using System.Text.Json;
using _0_Framework.Application.Sms.Models;
using Microsoft.Extensions.Configuration;

namespace _0_Framework.Application.Sms;

public class SmsService : ISmsService
{
    private readonly string _apiKey;
    private readonly IConfiguration _configuration;
    private readonly HttpClient httpClient;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
        _apiKey = _configuration.GetSection("SmsSecrets")["APIKey"];
        httpClient = new HttpClient();
    }

    public async Task<VerifyReceiveModel?> SendOtp(string number, string code)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        var model = new VerifySendModel
        {
            Mobile = number,
            TemplateId = Convert.ToInt32(_configuration.GetSection("SmsTemplateId")["OTP"]),
            Parameters = new[]
            {
                new VerifySendParameterModel
                {
                    Name = "OTP", Value = code
                }
            }
        };

        var payload = JsonSerializer.Serialize(model);
        StringContent stringContent = new(payload, Encoding.UTF8, "application/json");

        var response =
            await httpClient.PostAsync(_configuration.GetSection("SmsConnectionUrls")["Verify"], stringContent);
        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<VerifyReceiveModel>(result);
    }


    public async Task<VerifyReceiveModel?> SendFactor(string number, string Code)
    {
        throw new NotImplementedException();
    }

    public async Task<VerifyReceiveModel?> SendLoginMessage(string number, string userName)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        var model = new VerifySendModel
        {
            Mobile = number,
            TemplateId = Convert.ToInt32(_configuration.GetSection("SmsTemplateId")["Login"]),
            Parameters = new[]
            {
                new VerifySendParameterModel
                {
                    Name = "NAME", Value = userName
                },
                new VerifySendParameterModel
                {
                    Name = "DATE", Value = DateTime.Now.ToFarsi()
                }
            }
        };

        var payload = JsonSerializer.Serialize(model);
        StringContent stringContent = new(payload, Encoding.UTF8, "application/json");

        var response =
            await httpClient.PostAsync(_configuration.GetSection("SmsConnectionUrls")["Verify"], stringContent);
        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<VerifyReceiveModel>(result);
    }

    public async Task<VerifyReceiveModel?> SendLogoutMessage(string number, string userName)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        var model = new VerifySendModel
        {
            Mobile = number,
            TemplateId = Convert.ToInt32(_configuration.GetSection("SmsTemplateId")["Logout"]),
            Parameters = new[]
            {
                new VerifySendParameterModel
                {
                    Name = "NAME", Value = userName
                },
                new VerifySendParameterModel
                {
                    Name = "DATE", Value = DateTime.Now.ToFarsi()
                }
            }
        };

        var payload = JsonSerializer.Serialize(model);
        StringContent stringContent = new(payload, Encoding.UTF8, "application/json");

        var response =
            await httpClient.PostAsync(_configuration.GetSection("SmsConnectionUrls")["Verify"], stringContent);
        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<VerifyReceiveModel>(result);
    }

    public async Task<BulkReceiveModel?> SendAd(string[] number, string message)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key",
            _apiKey);
        var model = new BulkSendModel(Convert.ToInt64(_configuration.GetSection("SmsSecrets")["BulkLineNumber"]),
            message, number);
        var payload = JsonSerializer.Serialize(model);

        StringContent stringContent = new(payload, Encoding.UTF8, "application/json");
        var response =
            await httpClient.PostAsync(_configuration.GetSection("SmsConnectionUrls")["Bulk"], stringContent);
        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BulkReceiveModel>(result);
    }
}