using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Christian.Abbott.SMSMicroservice.EventHandlers;
using Christian.Abbott.SMSMicroservice.Platform.Logging;
using Christian.Abbott.SMSMicroservice.Platform.Deduplicator;
using Christian.Abbott.SMSMicroservice.SmsApiClient;

namespace Christian.Abbott.SMSMicroservice
{
    public class SMSMicroservice : IHostedService
    {
        private readonly IServiceProvider _provider;

        public SMSMicroservice(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serviceBus = new Dummies.DummyServiceBus();
            var eventHandler = new SendSmsCommandHandler(
                _provider.GetRequiredService<IEventLogger>(),
                _provider.GetRequiredService<IDeduplicator>(),
                _provider.GetRequiredService<ISmsApiClient>(),
                serviceBus);
            serviceBus.SubscribeConsumer(eventHandler);
            await serviceBus.SendStaggeredTestData();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
