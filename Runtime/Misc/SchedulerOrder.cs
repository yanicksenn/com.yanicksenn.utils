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
        public float Delay { get; }
        public float Interval { get; }
        public bool IsPeriodic { get; }

        private SchedulerOrder(ISchedulerEventHandler callback, string orderId, float delay, float interval, bool isPeriodic)
        {
            Callback = callback;
            OrderId = orderId;
            Delay = delay;
            Interval = interval;
            IsPeriodic = isPeriodic;
        }

        /// <summary>
        /// Builder for <see cref="SchedulerOrder"/>.
        /// </summary>
        public class Builder
        {
            private readonly ISchedulerEventHandler _callback;
            private readonly string _orderId;
            private readonly float _delay;
            private float _interval = 0f;
            private bool _isPeriodic = false;

            public Builder(ISchedulerEventHandler callback, string orderId, float delay)
            {
                _callback = callback;
                _orderId = orderId;
                _delay = delay;
            }

            public Builder AsPeriodic(float interval)
            {
                _interval = interval;
                _isPeriodic = true;
                return this;
            }

            public SchedulerOrder Build()
            {
                return new SchedulerOrder(_callback, _orderId, _delay, _interval, _isPeriodic);
            }
        }
    }
}
