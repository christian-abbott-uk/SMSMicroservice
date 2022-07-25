using Christian.Abbott.SMSMicroservice.Platform.ServiceBus;
using Christian.Abbott.SMSMicroservice.Shared.Events;
using Christian.Abbott.SMSMicroservice.Shared.Events.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Dummies
{
    public class DummyServiceBus : IServiceBus
    {
        private IServiceBusConsumer<SendSmsCommand> _consumer;
        public void Publish(ServiceBusMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Published {message}");
            Console.ResetColor();
        }

        public void SubscribeConsumer<T>(IServiceBusConsumer<T> handler)
            where T : ServiceBusMessage
        {
            Console.WriteLine($"Handler subscribed");
        }

        public void SubscribeConsumer(IServiceBusConsumer<SendSmsCommand> handler)
        {
            Console.WriteLine($"Handler subscribed -- send test data");
            _consumer = handler;
        }

        public async Task SendStaggeredTestData()
        {
            var commands = GenerateTestCommands();
            foreach(var command in commands)
            {
                Thread.Sleep(500);
                await _consumer.Handle(command);
            }
        }

        private IEnumerable<SendSmsCommand> GenerateTestCommands()
        {
            for(int i = 0; i<5; i++)
            {
                yield return new SendSmsCommand
                {
                    MessageId = Guid.NewGuid(),
                    PhoneNumber = RandomPhoneNumber(),
                    SmsText = RandomText()
                };
            }
        }

        private string RandomPhoneNumber()
        {
            var random = new Random();
            var s = new StringBuilder("+447");
            for (int i = 0; i < 9; i++)
            {
                s.Append(random.Next(10));
            }
            return s.ToString();
        }

        private string RandomText()
        {
            var random = new Random();
            var texts = new string[] {"Lorem ipsum dolor sit amet, consectetur adipiscing elit", "Mauris euismod urna arcu. Ut tristique, diam sit amet dapibus pulvinar", "leo neque tempor felis, nec egestas quam risus sed felis",
               "Etiam placerat imperdiet justo nec porttitor. Nunc vitae sem a leo luctus blandit sit amet pulvinar nulla.", "Etiam lorem elit, posuere at leo in, congue maximus ligula.",
               "Donec feugiat ullamcorper massa, sed scelerisque velit finibus vel. Sed aliquet tempus lobortis. Mauris blandit pretium pretium. Praesent rhoncus volutpat nulla non aliquet." };
            return texts[random.Next(texts.Length)];
        }
    }
}
