using Christian.Abbott.SMSMicroservice.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Platform.ServiceBus
{
    public interface IServiceBus
    {
        void SubscribeConsumer<T>(IServiceBusConsumer<T> handler)
            where T : ServiceBusMessage;
        void Publish(ServiceBusMessage message);
    }
}
