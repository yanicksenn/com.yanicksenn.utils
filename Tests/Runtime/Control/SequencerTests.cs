using NUnit.Framework;
using System.Collections.Generic;
using YanickSenn.Utils.Control;

namespace YanickSenn.Utils.Tests.Control
{
    public class SequencerTests
    {
        [Test]
        public void Next_ShouldInvokeOnValueChanged_WithNewAndPreviousValue()
        {
            var elements = new List<int> { 1, 2, 3 };
            var sequencer = new Sequencer<int>(elements);
            
            bool invoked = false;
            int capturedNew = -1;
            int capturedOld = -1;
            
            sequencer.OnValueChanged += (newValue, oldValue) => {
                invoked = true;
                capturedNew = newValue;
                capturedOld = oldValue;
            };
            
            sequencer.Next();
            
            Assert.IsTrue(invoked);
            Assert.AreEqual(2, capturedNew);
            Assert.AreEqual(1, capturedOld);
            Assert.AreEqual(2, sequencer.Value);
        }
        
        [Test]
        public void Next_AtEnd_ShouldNotInvokeOnValueChanged()
        {
            var elements = new List<int> { 1, 2 };
            var sequencer = new Sequencer<int>(elements);
            
            sequencer.Next(); // To 2 (index 1)
            
            bool invoked = false;
            sequencer.OnValueChanged += (newValue, oldValue) => invoked = true;
            
            sequencer.Next(); // To 2 (index 1, clamped)
            
            Assert.IsFalse(invoked);
            Assert.AreEqual(2, sequencer.Value);
        }
        
        [Test]
        public void Previous_ShouldInvokeOnValueChanged_WithNewAndPreviousValue()
        {
            var elements = new List<int> { 1, 2 };
            var sequencer = new Sequencer<int>(elements, 1); // Start at 2 (index 1)
            
            bool invoked = false;
            int capturedNew = -1;
            int capturedOld = -1;
            
            sequencer.OnValueChanged += (newValue, oldValue) => {
                invoked = true;
                capturedNew = newValue;
                capturedOld = oldValue;
            };
            
            sequencer.Previous(); // To 1 (index 0)
            
            Assert.IsTrue(invoked);
            Assert.AreEqual(1, capturedNew);
            Assert.AreEqual(2, capturedOld);
            Assert.AreEqual(1, sequencer.Value);
        }
        
        [Test]
        public void Previous_AtStart_ShouldNotInvokeOnValueChanged()
        {
            var elements = new List<int> { 1, 2 };
            var sequencer = new Sequencer<int>(elements); // Start at 1 (index 0)
            
            bool invoked = false;
            sequencer.OnValueChanged += (newValue, oldValue) => invoked = true;
            
            sequencer.Previous(); // To 1 (index 0, clamped)
            
            Assert.IsFalse(invoked);
            Assert.AreEqual(1, sequencer.Value);
        }
        
        [Test]
        public void SelectStartingValue_ShouldInvokeOnValueChanged_WithNewAndPreviousValue()
        {
            var elements = new List<int> { 1, 2, 3 };
            var sequencer = new Sequencer<int>(elements, 0); // Start at 1
            
            sequencer.Next(); // To 2
            
            bool invoked = false;
            int capturedNew = -1;
            int capturedOld = -1;
            
            sequencer.OnValueChanged += (newValue, oldValue) => {
                invoked = true;
                capturedNew = newValue;
                capturedOld = oldValue;
            };
            
            sequencer.SelectStartingValue(); // Back to 1
            
            Assert.IsTrue(invoked);
            Assert.AreEqual(1, capturedNew);
            Assert.AreEqual(2, capturedOld);
            Assert.AreEqual(1, sequencer.Value);
        }
    }
}