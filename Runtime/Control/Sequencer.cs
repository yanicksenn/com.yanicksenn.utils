using System;
using System.Collections.Generic;

namespace YanickSenn.Utils.Control
{
    public class Sequencer<T> {
        public event Action<T, T> OnValueChanged;

        private readonly List<T> _elements;
        private readonly int _startingIndex;
        private int _index;

        public T Value => _elements[_index];

        public Sequencer(List<T> elements, int startingIndex = 0) {
            _elements = elements;
            _startingIndex = startingIndex;
            _index = startingIndex;
        }

        public void Next() {
            var previousValue = Value;
            var previousIndex = _index;
            _index = Math.Clamp(_index + 1, 0, _elements.Count - 1);
            if (_index != previousIndex) {
                OnValueChanged?.Invoke(previousValue, Value);
            }
        }

        public void Previous() {
            var previousValue = Value;
            var previousIndex = _index;
            _index = Math.Clamp(_index - 1, 0, _elements.Count - 1);
            if (_index != previousIndex) {
                OnValueChanged?.Invoke(previousValue, Value);
            }
        }

        public void SelectStartingValue() {
            var previousValue = Value;
            var previousIndex = _index;
            _index = _startingIndex;
            if (_index != previousIndex) {
                OnValueChanged?.Invoke(previousValue, Value);
            }
        }
    }
}
