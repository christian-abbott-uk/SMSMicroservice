using Christian.Abbott.SMSMicroservice.EventHandlers;
using Christian.Abbott.SMSMicroservice.Platform.Deduplicator;
using Christian.Abbott.SMSMicroservice.Platform.Logging;
using Christian.Abbott.SMSMicroservice.Platform.ServiceBus;
using Christian.Abbott.SMSMicroservice.Shared.Events.SMS;
using Christian.Abbott.SMSMicroservice.SmsApiClient;
using Christian.Abbott.SMSMicroservice.SmsApiClient.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.UnitTests.EventHandlers
{
    [TestFixture]
    public class EventHandlerTests
    {
        private Mock<IEventLogger> _eventLogger;
        private Mock<ISmsApiClient> _smsApiClient;
        private Mock<IServiceBus> _serviceBus;
        private Mock<IDeduplicator> _deDuplicator;
        private SendSmsCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _eventLogger = new Mock<IEventLogger>();
            _smsApiClient = new Mock<ISmsApiClient>();
            _serviceBus = new Mock<IServiceBus>();
            _deDuplicator = new Mock<IDeduplicator>();

            _handler = new SendSmsCommandHandler(_eventLogger.Object, _deDuplicator.Object, _smsApiClient.Object, _serviceBus.Object);
        }

        [Test]
        public async Task GivenCommandToSendSms_SendsRequestToSmsApi_ThenSendsSmsSentEvent()
        {
            var messageId = Guid.NewGuid();
            var testCommand = new SendSmsCommand
            {
                MessageId = messageId,
                PhoneNumber = "+447000001",
                SmsText = "Test Message"
            };

            _deDuplicator.Setup(deduplicator => deduplicator.IsDuplicateMessage(messageId)).Returns(false);

            await _handler.Handle(testCommand);

            _smsApiClient.Verify(sms => sms.SendSmsMessageAsync(It.Is<SmsApiClientRequest>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Once);
            _deDuplicator.Verify(deduplicator => deduplicator.MarkAsHandled(messageId), Times.Once);
            _serviceBus.Verify(serviceBus => serviceBus.Publish(It.Is<SmsSentEvent>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Once);
        }

        [Test]
        public async Task GivenCommandToSendSms_WhenMessageIsDuplicate_SendsNoMessageRequest()
        {
            var messageId = Guid.NewGuid();
            var testCommand = new SendSmsCommand
            {
                MessageId = messageId,
                PhoneNumber = "+447000001",
                SmsText = "Test Message"
            };

            _deDuplicator.Setup(deduplicator => deduplicator.IsDuplicateMessage(messageId)).Returns(true);

            await _handler.Handle(testCommand);

            _smsApiClient.Verify(sms => sms.SendSmsMessageAsync(It.Is<SmsApiClientRequest>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Never);
            _deDuplicator.Verify(deduplicator => deduplicator.MarkAsHandled(messageId), Times.Never);
            _serviceBus.Verify(serviceBus => serviceBus.Publish(It.Is<SmsSentEvent>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Never);
        }

        [Test]
        public async Task GivenCommandToSendSms_WhenMessageFailsToSend_WillAttemptToRetry5Times()
        {
            var messageId = Guid.NewGuid();
            var testCommand = new SendSmsCommand
            {
                MessageId = messageId,
                PhoneNumber = "+447000001",
                SmsText = "Test Message"
            };

            _deDuplicator.Setup(deduplicator => deduplicator.IsDuplicateMessage(messageId)).Returns(false);
            _smsApiClient.Setup(sms => sms.SendSmsMessageAsync(It.Is<SmsApiClientRequest>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001"))).Throws(
                () => new HttpRequestException("Error by design"));

            await _handler.Handle(testCommand);

            _smsApiClient.Verify(sms => sms.SendSmsMessageAsync(It.Is<SmsApiClientRequest>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Exactly(5));
            _deDuplicator.Verify(deduplicator => deduplicator.MarkAsHandled(messageId), Times.Never);
            _serviceBus.Verify(serviceBus => serviceBus.Publish(It.Is<SmsSentEvent>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Never);
        }


        [Test]
        public async Task GivenCommandToSendSms_WhenMessageFailsToSendFirstTime_WillRetryAndThenContinue()
        {
            var messageId = Guid.NewGuid();
            var testCommand = new SendSmsCommand
            {
                MessageId = messageId,
                PhoneNumber = "+447000001",
                SmsText = "Test Message"
            };

            _deDuplicator.Setup(deduplicator => deduplicator.IsDuplicateMessage(messageId)).Returns(false);
            var count = 0;
            _smsApiClient.Setup(sms => sms.SendSmsMessageAsync(It.Is<SmsApiClientRequest>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001"))).Callback(
                () =>
                {
                    count++;
                    if (count == 1)
                    {
                        throw new HttpRequestException("Error by design");
                    }
                });

            await _handler.Handle(testCommand);

            _smsApiClient.Verify(sms => sms.SendSmsMessageAsync(It.Is<SmsApiClientRequest>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Exactly(2));
            _deDuplicator.Verify(deduplicator => deduplicator.MarkAsHandled(messageId), Times.Once);
            _serviceBus.Verify(serviceBus => serviceBus.Publish(It.Is<SmsSentEvent>(x => x.SmsText == "Test Message" && x.PhoneNumber == "+447000001")), Times.Once);
        }
    }
}
