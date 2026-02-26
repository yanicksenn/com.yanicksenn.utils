using System.Collections;
using System.Collections.Generic;

namespace YanickSenn.Utils.Misc
{
    /// <summary>
    /// Event handler interface for the scheduler.
    /// </summary>
    public interface ISchedulerEventHandler
    {
        /// <summary>
        /// Invoked when a scheduled order is triggered.
        /// </summary>
        /// <param name="orderId">The ID of the triggered order.</param>
        void OnScheduledEvent(string orderId);
    }
}
