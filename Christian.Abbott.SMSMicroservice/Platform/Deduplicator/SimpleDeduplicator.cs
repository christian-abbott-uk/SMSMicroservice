using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.Platform.Deduplicator
{
    public class SimpleDeduplicator : IDeduplicator
    {
        public int CurrentListCount => _currentList.Count;

        private const int Max_Size = 5;
        private readonly List<Guid> _currentList = new();
        public bool IsDuplicateMessage(Guid messageId)
        {
            return _currentList.Contains(messageId);
        }

        public void MarkAsHandled(Guid messageId)
        {
            if (_currentList.Count == Max_Size)
                _currentList.RemoveAt(0);
            _currentList.Add(messageId);
        }

    }
}
