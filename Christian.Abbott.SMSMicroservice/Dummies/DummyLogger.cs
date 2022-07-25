using Christian.Abbott.SMSMicroservice.Platform.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Dummies
{
    public class DummyLogger : IEventLogger
    {
        public void LogError(string message, Exception ex = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}");
            Console.ResetColor();
        }

        public void LogMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
