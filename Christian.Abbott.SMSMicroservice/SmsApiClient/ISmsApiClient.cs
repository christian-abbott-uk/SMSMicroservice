using Christian.Abbott.SMSMicroservice.SmsApiClient.Models;

namespace Christian.Abbott.SMSMicroservice.SmsApiClient
{
    public interface ISmsApiClient
    {
        Task SendSmsMessageAsync(SmsApiClientRequest request);
    }
}