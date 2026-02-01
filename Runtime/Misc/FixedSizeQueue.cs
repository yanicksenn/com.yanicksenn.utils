using System.Collections;
using System.Collections.Generic;

namespace YanickSenn.Utils.Misc {
    public class FixedSizeQueue<T> : IEnumerable<T> {
        private readonly Queue<T> _queue;
        private readonly Dictionary<T, int> _itemCounts;

        public int Capacity { get; }
        public int Count => _queue.Count;

        public FixedSizeQueue(int capacity) {
            Capacity = capacity;
            _queue = new Queue<T>(capacity + 1);
            _itemCounts = new Dictionary<T, int>(capacity + 1);
        }

        public void Enqueue(T item) {
            _queue.Enqueue(item);
            if (_itemCounts.TryGetValue(item, out var count)) {
                _itemCounts[item] = count + 1;
            } else {
                _itemCounts[item] = 1;
            }

            if (_queue.Count > Capacity) {
                var evicted = _queue.Dequeue();
                var newCount = _itemCounts[evicted] - 1;
                if (newCount <= 0) {
                    _itemCounts.Remove(evicted);
                } else {
                    _itemCounts[evicted] = newCount;
                }
            }
        }

        public bool Contains(T item) {
            return _itemCounts.ContainsKey(item);
        }

        public void Clear() {
            _queue.Clear();
            _itemCounts.Clear();
        }

        public IEnumerator<T> GetEnumerator() {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
