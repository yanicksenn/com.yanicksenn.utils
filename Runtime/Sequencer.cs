using System;
using System.Collections.Generic;

namespace YanickSenn.Utils
{
    public class Sequencer<T> {
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
            _index = Math.Clamp(_index + 1, 0, _elements.Count - 1);
        }

        public void Previous() {
            _index = Math.Clamp(_index - 1, 0, _elements.Count - 1);
        }

        public void SelectStartingValue() {
            _index = _startingIndex;
        }
    }
}