using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Shared.Events.SMS
{
    public class SendSmsCommand : ServiceBusMessage
    {
        public string PhoneNumber { get; set; }
        public string SmsText { get; set; }

        public override string ToString()
        {
            return base.ToString() + @$"Send Sms Command:
    PhoneNumber: {PhoneNumber}
    SmsText: {SmsText}";
        }
    }
}
