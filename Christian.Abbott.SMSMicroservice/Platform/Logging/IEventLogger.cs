using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Platform.Logging
{
    public interface IEventLogger
    {
        void LogMessage(string message);
        void LogError(string message, Exception ex = null);
    }
}
