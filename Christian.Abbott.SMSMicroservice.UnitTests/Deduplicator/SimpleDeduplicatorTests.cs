using Christian.Abbott.SMSMicroservice.Platform.Deduplicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Christian.Abbott.SMSMicroservice.UnitTests.Deduplicator
{
    [TestFixture]
    public class SimpleDeduplicatorTests
    {
        [Test]
        public void GivenDifferentMessageIds_ReturnsNotDuplicate()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var deduplicator = new SimpleDeduplicator();
            deduplicator.MarkAsHandled(id1);
            Assert.IsFalse(deduplicator.IsDuplicateMessage(id2));
        }

        [Test]
        public void GivenSameMessageIds_ReturnsIsDuplicateMessage()
        {
            var id1 = Guid.Parse("bd06a710-8c2a-44dc-90ec-1ed2db67c42a");
            var id2 = Guid.Parse("bd06a710-8c2a-44dc-90ec-1ed2db67c42a");
            var deduplicator = new SimpleDeduplicator();
            deduplicator.MarkAsHandled(id1);
            Assert.IsTrue(deduplicator.IsDuplicateMessage(id2));
        }

        [Test]
        public void GivenMultipleIds_WhenMessageIdIsSentAgain_ReturnsIsDuplicateMessage()
        {
            var id1 = Guid.Parse("bd06a710-8c2a-44dc-90ec-1ed2db67c42a");
            var id2 = Guid.Parse("bd06a710-8c2a-44dc-90ec-1ed2db67c42a");
            var deduplicator = new SimpleDeduplicator();
            deduplicator.MarkAsHandled(id1);
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());

            Assert.IsTrue(deduplicator.IsDuplicateMessage(id2));
        }

        [Test]
        public void GivenMoreThan5Ids_WillNotGoOverCapacity()
        {
            var deduplicator = new SimpleDeduplicator();

            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());
            deduplicator.MarkAsHandled(Guid.NewGuid());

            Assert.That(deduplicator.CurrentListCount,Is.EqualTo(5));
        }
    }
}
