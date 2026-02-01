using NUnit.Framework;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Editor.Tests.Misc
{
    public class FixedSizeQueueTests
    {
        [Test]
        public void FixedSizeQueue_RespectsCapacity()
        {
            var queue = new FixedSizeQueue<int>(3);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);

            Assert.AreEqual(3, queue.Count);
            // Should contain 2, 3, 4
            Assert.IsFalse(queue.Contains(1));
            Assert.IsTrue(queue.Contains(2));
            Assert.IsTrue(queue.Contains(3));
            Assert.IsTrue(queue.Contains(4));
        }

        [Test]
        public void FixedSizeQueue_Contains_TracksItemsCorrectly()
        {
            var queue = new FixedSizeQueue<string>(5);
            queue.Enqueue("A");
            Assert.IsTrue(queue.Contains("A"));
            Assert.IsFalse(queue.Contains("B"));
        }

        [Test]
        public void FixedSizeQueue_DuplicateItems_CountedCorrectly()
        {
            var queue = new FixedSizeQueue<string>(3);
            queue.Enqueue("A");
            queue.Enqueue("A");
            queue.Enqueue("B");

            Assert.IsTrue(queue.Contains("A"));
            Assert.IsTrue(queue.Contains("B"));
            
            // Queue: A, A, B. Full.
            // Enqueue C. Evict first A.
            queue.Enqueue("C");
            
            // Queue: A, B, C. Still has one A.
            Assert.IsTrue(queue.Contains("A"));
            Assert.IsTrue(queue.Contains("B"));
            Assert.IsTrue(queue.Contains("C"));

            // Enqueue D. Evict second A.
            queue.Enqueue("D");
            
            // Queue: B, C, D. No A left.
            Assert.IsFalse(queue.Contains("A"));
            Assert.IsTrue(queue.Contains("B"));
        }

        [Test]
        public void FixedSizeQueue_Clear_ResetsQueue()
        {
            var queue = new FixedSizeQueue<int>(3);
            queue.Enqueue(1);
            queue.Clear();
            Assert.AreEqual(0, queue.Count);
            Assert.IsFalse(queue.Contains(1));
        }
    }
}
