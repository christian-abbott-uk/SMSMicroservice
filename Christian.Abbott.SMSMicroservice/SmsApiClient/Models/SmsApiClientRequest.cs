using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.SmsApiClient.Models
{
    public class SmsApiClientRequest
    {
        public string PhoneNumber { get; set; }
        public string SmsText { get; set; }
    }
}
