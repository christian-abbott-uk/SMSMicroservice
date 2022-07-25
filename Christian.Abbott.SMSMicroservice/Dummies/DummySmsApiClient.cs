using Christian.Abbott.SMSMicroservice.SmsApiClient;
using Christian.Abbott.SMSMicroservice.SmsApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Dummies
{
    public class DummySmsApiClient : ISmsApiClient
    {
        public async Task SendSmsMessageAsync(SmsApiClientRequest request)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            await Task.Delay(10);
            Console.WriteLine("Sent Call to Sms Api");
            Console.ResetColor();

        }
    }
}
