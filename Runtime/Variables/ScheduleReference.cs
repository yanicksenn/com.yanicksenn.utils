using System;
using YanickSenn.Utils.Misc;

namespace YanickSenn.Utils.Variables
{
    [Serializable]
    public class ScheduleReference : Reference<Schedule> {
        public ScheduleReference(Schedule value = default) : base(value) { }
    }
}