using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Shared.Events.SMS
{
    public class SmsSentEvent : ServiceBusMessage
    {
        public string PhoneNumber { get; set; }
        public string SmsText { get; set; }

        public override string ToString()
        {
            return base.ToString() + @$"Sms Sent Event:
    PhoneNumber: {PhoneNumber}
    SmsText: {SmsText}";
        }
    }
}
