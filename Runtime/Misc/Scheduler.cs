using System;
using System.Collections.Generic;
using UnityEngine;

namespace YanickSenn.Utils.Misc {
    /// <summary>
    /// A scheduler component for managing one-time and periodic orders.
    /// </summary>
    public class Scheduler : MonoBehaviour {
        private class ScheduledOrder {
            public ISchedulerEventHandler Callback { get; }
            public string OrderId { get; }
            public float Delay { get; }
            public float Interval { get; }
            public bool IsPeriodic { get; }
            public float TriggerTime { get; set; }

            public ScheduledOrder(SchedulerOrder order) {
                Callback = order.Callback;
                OrderId = order.OrderId;
                Delay = order.Delay;
                Interval = order.Interval;
                IsPeriodic = order.IsPeriodic;
                TriggerTime = Time.time + Delay;
            }
        }

        private readonly List<ScheduledOrder> _orders = new();
        private readonly List<ScheduledOrder> _pendingAdditions = new();
        private readonly List<ScheduledOrder> _pendingRemovals = new();

        private void Update() {
            // Process additions
            if (_pendingAdditions.Count > 0) {
                _orders.AddRange(_pendingAdditions);
                _pendingAdditions.Clear();
            }

            // Process removals
            if (_pendingRemovals.Count > 0) {
                _orders.RemoveAll(IsPendingRemoval);
                _pendingRemovals.Clear();
            }

            // Process current orders
            var currentTime = Time.time;
            for (var i = _orders.Count - 1; i >= 0; i--) {
                var order = _orders[i];

                if (IsPendingRemoval(order)) {
                    _orders.RemoveAt(i);
                    continue;
                }

                if (!(currentTime >= order.TriggerTime)) continue;
                // Invoke callback
                order.Callback?.OnScheduledEvent(order.OrderId);

                // Re-check cancellation after callback in case it was cancelled synchronously
                if (IsPendingRemoval(order)) {
                    _orders.RemoveAt(i);
                    continue;
                }

                if (order.IsPeriodic) {
                    // Set the next trigger time based on current time to prevent rapid fire
                    // if the game lagged significantly.
                    order.TriggerTime = currentTime + order.Interval;
                } else {
                    _orders.RemoveAt(i);
                }
            }
        }

        private bool IsPendingRemoval(ScheduledOrder order) {
            for (int i = 0; i < _pendingRemovals.Count; i++) {
                var removal = _pendingRemovals[i];
                if (removal.OrderId == order.OrderId && removal.Callback == order.Callback) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Schedules a new order.
        /// </summary>
        /// <param name="order">The order to be scheduled.</param>
        public void Schedule(SchedulerOrder order) {
            _pendingAdditions.Add(new ScheduledOrder(order));
        }

        /// <summary>
        /// Cancels a specific order by its callback and order ID.
        /// </summary>
        /// <param name="callback">The event handler of the order.</param>
        /// <param name="orderId">The ID of the order.</param>
        public void CancelOrder(ISchedulerEventHandler callback, string orderId) {
            CancelWhere(o => o.Callback == callback && o.OrderId == orderId);
        }

        /// <summary>
        /// Cancels all orders associated with a specific callback.
        /// </summary>
        /// <param name="callback">The event handler.</param>
        public void CancelAllOrders(ISchedulerEventHandler callback) {
            CancelWhere(o => o.Callback == callback);
        }

        /// <summary>
        /// Cancels all orders matching a specific order ID.
        /// </summary>
        /// <param name="orderId">The ID of the orders.</param>
        public void CancelAllOrders(string orderId) {
            CancelWhere(o => o.OrderId == orderId);
        }

        private void CancelWhere(Predicate<ScheduledOrder> match) {
            foreach (var order in _orders) {
                if (match(order)) {
                    _pendingRemovals.Add(order);
                }
            }

            _pendingAdditions.RemoveAll(match);
        }
    }
}
