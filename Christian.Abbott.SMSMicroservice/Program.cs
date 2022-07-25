using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Christian.Abbott.SMSMicroservice.Platform.Logging;
using Christian.Abbott.SMSMicroservice.Dummies;
using Christian.Abbott.SMSMicroservice.SmsApiClient;
using Christian.Abbott.SMSMicroservice.Platform.ServiceBus;
using Christian.Abbott.SMSMicroservice.Platform.Deduplicator;
using Christian.Abbott.SMSMicroservice.EventHandlers;
using Christian.Abbott.SMSMicroservice;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IEventLogger, DummyLogger>();
        services.AddTransient<ISmsApiClient, DummySmsApiClient>();
        services.AddSingleton<IDeduplicator, SimpleDeduplicator>();
        services.AddHostedService<SMSMicroservice>();
    })
    .Build()
    .RunAsync();
