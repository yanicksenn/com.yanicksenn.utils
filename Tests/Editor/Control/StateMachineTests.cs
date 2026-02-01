using NUnit.Framework;
using Util;

namespace YanickSenn.Utils.Editor.Tests.Control
{
    public class StateMachineTests
    {
        private class MockState : IStateMachineState<MockState>
        {
            public int EnterCount { get; private set; }
            public int ExitCount { get; private set; }
            public MockState LastEnteredFrom { get; private set; }
            public MockState LastExitedTo { get; private set; }

            public void EnterState(MockState previousState)
            {
                EnterCount++;
                LastEnteredFrom = previousState;
            }

            public void ExitState(MockState nextState)
            {
                ExitCount++;
                LastExitedTo = nextState;
            }
        }

        [Test]
        public void StateMachine_InitialState_Entered()
        {
            var initial = new MockState();
            var sm = new StateMachine<MockState>(initial);

            Assert.AreEqual(1, initial.EnterCount);
            Assert.IsNull(initial.LastEnteredFrom);
            Assert.AreEqual(initial, sm.CurrentState);
        }

        [Test]
        public void StateMachine_SetCurrentState_TransitionsCorrectly()
        {
            var initial = new MockState();
            var next = new MockState();
            var sm = new StateMachine<MockState>(initial);

            sm.SetCurrentState(next);

            // Check initial state exited
            Assert.AreEqual(1, initial.ExitCount);
            Assert.AreEqual(next, initial.LastExitedTo);

            // Check next state entered
            Assert.AreEqual(1, next.EnterCount);
            Assert.AreEqual(initial, next.LastEnteredFrom);

            Assert.AreEqual(next, sm.CurrentState);
        }
    }
}
