using UnityEngine;

namespace YanickSenn.Utils.Misc
{
    /// <summary>
    /// Represents an order to be scheduled.
    /// </summary>
    public class SchedulerOrder
    {
        public ISchedulerEventHandler Callback { get; }
        public string OrderId { get; }
        public Schedule Schedule { get; }
        public bool IsPaused { get; }

        private SchedulerOrder(ISchedulerEventHandler callback, string orderId, Schedule schedule, bool isPaused)
        {
            Callback = callback;
            OrderId = orderId;
            Schedule = schedule;
            IsPaused = isPaused;
        }

        /// <summary>
        /// Builder for <see cref="SchedulerOrder"/>.
        /// </summary>
        public class Builder
        {
            private readonly ISchedulerEventHandler _callback;
            private readonly string _orderId;
            private Schedule _schedule;
            private bool _isPaused;

            public Builder(ISchedulerEventHandler callback, string orderId, Schedule schedule)
            {
                _callback = callback;
                _orderId = orderId;
                _schedule = schedule;
            }

            public Builder(ISchedulerEventHandler callback, string orderId, float delay)
            {
                _callback = callback;
                _orderId = orderId;
                _schedule = new Schedule(delay);
            }

            public Builder AsPeriodic(float interval)
            {
                _schedule = new Schedule(_schedule.Delay, interval, true);
                return this;
            }

            public Builder AsPaused(bool isPaused = true)
            {
                _isPaused = isPaused;
                return this;
            }

            public SchedulerOrder Build()
            {
                return new SchedulerOrder(_callback, _orderId, _schedule, _isPaused);
            }
        }
    }
}
