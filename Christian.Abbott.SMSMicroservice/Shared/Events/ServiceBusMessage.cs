using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Shared.Events
{
    public abstract class ServiceBusMessage
    {
        public Guid MessageId { get; set; }

        public override string ToString()
        {
            return $"#{MessageId} ";
        }
    }
}
