using System;
using System.Collections.Generic;

namespace YanickSenn.Utils
{
    public class Sequencer<T> {
        private readonly List<T> elements;
        private int index;
    
        public T Current => elements[index];

        public Sequencer(List<T> elements, int startingIndex = 0) {
            this.elements = elements;
            index = startingIndex;
        }

        public void Next() {
            index = Math.Clamp(index + 1, 0, elements.Count - 1);
        }

        public void Previous() {
            index = Math.Clamp(index - 1, 0, elements.Count - 1);
        }
    }
}