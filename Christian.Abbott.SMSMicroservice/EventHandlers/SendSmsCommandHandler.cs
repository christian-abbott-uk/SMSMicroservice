using Christian.Abbott.SMSMicroservice.Platform.Deduplicator;
using Christian.Abbott.SMSMicroservice.Platform.Logging;
using Christian.Abbott.SMSMicroservice.Platform.ServiceBus;
using Christian.Abbott.SMSMicroservice.Shared.Events.SMS;
using Christian.Abbott.SMSMicroservice.SmsApiClient;
using Christian.Abbott.SMSMicroservice.SmsApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.EventHandlers
{
    public class SendSmsCommandHandler : IServiceBusConsumer<SendSmsCommand>
    {
        private const int MAX_RETRY_COUNT = 5;
        private readonly IEventLogger _eventLogger;
        private readonly ISmsApiClient _smsApiClient;
        private readonly IServiceBus _serviceBus;
        private readonly IDeduplicator _deduplicator;

        public SendSmsCommandHandler(
            IEventLogger eventLogger,
            IDeduplicator deduplicator,
            ISmsApiClient smsApiClient,
            IServiceBus serviceBus)
        {
            _eventLogger = eventLogger;
            _smsApiClient = smsApiClient;
            _serviceBus = serviceBus;
            _deduplicator = deduplicator;

        }

        public async Task Handle(SendSmsCommand command)
        {
            //Possible issue with GDPR here, as could contain sensitive information in text. Assumption this would not be in standard code.
            _eventLogger.LogMessage($"#{command.MessageId} Message Send Command Raised - Number: {command.PhoneNumber} - Text: {command.SmsText}");

            if (_deduplicator.IsDuplicateMessage(command.MessageId))
            {
                _eventLogger.LogMessage($"#{command.MessageId} is a duplicate message");
                return;
            }

            var messageSentComplete = false;
            var retryCount = 0;
            while (!messageSentComplete && retryCount < MAX_RETRY_COUNT)
            {  
                try
                {
                    await _smsApiClient.SendSmsMessageAsync(new SmsApiClientRequest
                    {
                        SmsText = command.SmsText,
                        PhoneNumber = command.PhoneNumber
                    });
                    messageSentComplete = true;
                }
                catch(Exception ex)
                {
                    _eventLogger.LogError($"#{command.MessageId} Could not send Text Message, retry count: {retryCount} - {ex.Message}");
                }
                retryCount++;
            }
            if (!messageSentComplete)
            {
                _eventLogger.LogError($"#{command.MessageId} Could not send Text Message, max retries reached");
                //In production I would have sent this to another queue or raised a major error to a monitoring service
                return;
            }

            _deduplicator.MarkAsHandled(command.MessageId);


            _eventLogger.LogMessage($"#{command.MessageId} Sending SmsSentEvent - Number: {command.PhoneNumber} - Text: {command.SmsText}");
            _serviceBus.Publish(new SmsSentEvent()
            {
                MessageId = Guid.NewGuid(),
                PhoneNumber = command.PhoneNumber,
                SmsText = command.SmsText
            });
        }
    }
}
