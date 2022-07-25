using Christian.Abbott.SMSMicroservice.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Platform.ServiceBus
{
    public interface IServiceBusConsumer<T> where T : ServiceBusMessage
    {
        Task Handle(T message);
    }
}
