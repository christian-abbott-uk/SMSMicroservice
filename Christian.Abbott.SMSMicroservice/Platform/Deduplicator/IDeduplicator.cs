using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Platform.Deduplicator
{
    public interface IDeduplicator
    {
        bool IsDuplicateMessage(Guid messageId);
        void MarkAsHandled(Guid messageId);
    }
}
