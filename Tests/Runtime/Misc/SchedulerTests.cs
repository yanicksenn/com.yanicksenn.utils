using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Tests.Misc
{
    public class SchedulerTests
    {
        private GameObject _go;
        private Scheduler _scheduler;
        private MockEventHandler _handler;
        private MockEventHandler _handler2;

        private class MockEventHandler : ISchedulerEventHandler
        {
            public int InvocationCount { get; private set; }
            public string LastOrderId { get; private set; }

            public void OnScheduledEvent(string orderId)
            {
                InvocationCount++;
                LastOrderId = orderId;
            }
        }

        [SetUp]
        public void Setup()
        {
            _go = new GameObject();
            _scheduler = _go.AddComponent<Scheduler>();
            _handler = new MockEventHandler();
            _handler2 = new MockEventHandler();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_go);
        }

        [UnityTest]
        public IEnumerator Schedule_OneTimeOrder_InvokesCallbackAfterDelay()
        {
            var order = new SchedulerOrder.Builder(_handler, "test1", 0.1f)
                .Build();
            
            _scheduler.Schedule(order);
            
            yield return new WaitForSeconds(0.05f);
            Assert.AreEqual(0, _handler.InvocationCount);
            
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(1, _handler.InvocationCount);
            Assert.AreEqual("test1", _handler.LastOrderId);

            // Ensure it does not fire again
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(1, _handler.InvocationCount);
        }

        [UnityTest]
        public IEnumerator SchedulePeriodic_InvokesCallbackMultipleTimes()
        {
            var order = new SchedulerOrder.Builder(_handler, "test2", 0.1f)
                .AsPeriodic(0.1f)
                .Build();
                
            _scheduler.Schedule(order);
            
            yield return new WaitForSeconds(0.15f);
            Assert.AreEqual(1, _handler.InvocationCount);
            
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(2, _handler.InvocationCount);
        }

        [UnityTest]
        public IEnumerator CancelOrder_ByCallbackAndOrderId_CancelsCorrectly()
        {
            var order = new SchedulerOrder.Builder(_handler, "test3", 0.1f)
                .Build();
                
            _scheduler.Schedule(order);
            _scheduler.CancelOrder(_handler, "test3");
            
            yield return new WaitForSeconds(0.15f);
            Assert.AreEqual(0, _handler.InvocationCount);
        }

        [UnityTest]
        public IEnumerator CancelAllOrders_ByCallback_CancelsCorrectly()
        {
            _scheduler.Schedule(new SchedulerOrder.Builder(_handler, "testA", 0.1f).Build());
            _scheduler.Schedule(new SchedulerOrder.Builder(_handler, "testB", 0.1f).Build());
            _scheduler.Schedule(new SchedulerOrder.Builder(_handler2, "testA", 0.1f).Build());

            _scheduler.CancelAllOrders(_handler);
            
            yield return new WaitForSeconds(0.15f);
            Assert.AreEqual(0, _handler.InvocationCount);
            Assert.AreEqual(1, _handler2.InvocationCount);
        }

        [UnityTest]
        public IEnumerator CancelAllOrders_ByOrderId_CancelsCorrectly()
        {
            _scheduler.Schedule(new SchedulerOrder.Builder(_handler, "testA", 0.1f).Build());
            _scheduler.Schedule(new SchedulerOrder.Builder(_handler, "testB", 0.1f).Build());
            _scheduler.Schedule(new SchedulerOrder.Builder(_handler2, "testA", 0.1f).Build());

            _scheduler.CancelAllOrders("testA");
            
            yield return new WaitForSeconds(0.15f);
            Assert.AreEqual(1, _handler.InvocationCount);
            Assert.AreEqual("testB", _handler.LastOrderId);
            Assert.AreEqual(0, _handler2.InvocationCount);
        }

        [UnityTest]
        public IEnumerator PauseOrder_DoesNotInvokeCallback()
        {
            var order = new SchedulerOrder.Builder(_handler, "testPause", 0.1f)
                .AsPaused()
                .Build();
                
            _scheduler.Schedule(order);
            
            yield return new WaitForSeconds(0.15f);
            Assert.AreEqual(0, _handler.InvocationCount);
        }

        [UnityTest]
        public IEnumerator ResumeOrder_InvokesCallbackAfterRemainingTime()
        {
            var order = new SchedulerOrder.Builder(_handler, "testResume", 0.2f)
                .Build();
                
            _scheduler.Schedule(order);
            
            yield return new WaitForSeconds(0.1f);
            _scheduler.PauseOrder(_handler, "testResume");
            
            yield return new WaitForSeconds(0.2f);
            Assert.AreEqual(0, _handler.InvocationCount);
            
            _scheduler.ResumeOrder(_handler, "testResume");
            
            yield return new WaitForSeconds(0.15f);
            Assert.AreEqual(1, _handler.InvocationCount);
        }
    }
}
