using System;
using UnityEngine;

namespace YanickSenn.Utils.Misc
{
    /// <summary>
    /// Represents a schedule component for a <see cref="SchedulerOrder"/>.
    /// </summary>
    [Serializable]
    public class Schedule
    {
        [SerializeField] private float delay;
        [SerializeField] private float interval;
        [SerializeField] private bool isPeriodic;

        public float Delay => delay;
        public float Interval => interval;
        public bool IsPeriodic => isPeriodic;

        public Schedule(float delay, float interval = 0f, bool isPeriodic = false)
        {
            this.delay = delay;
            this.interval = interval;
            this.isPeriodic = isPeriodic;
        }
    }
}