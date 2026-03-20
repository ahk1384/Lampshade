using _0_Framework.Application.Sms.Models;

namespace _0_Framework.Application.Sms;

public interface ISmsService
{
    Task<VerifyReceiveModel?> SendOtp(string number, string Otp);
    Task<VerifyReceiveModel?> SendFactor(string number, string Code);
    Task<VerifyReceiveModel?> SendLoginMessage(string number, string userName);
    Task<VerifyReceiveModel?> SendLogoutMessage(string number, string userName);
    Task<BulkReceiveModel?> SendAd(string[] number, string message);
}