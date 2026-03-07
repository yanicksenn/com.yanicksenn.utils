using System;
using UnityEngine;

namespace YanickSenn.Utils.Misc 
{
    public enum TimeUnit
    {
        Milliseconds,
        Seconds,
        Minutes,
        Hours
    }

    [Serializable]
    public struct Duration 
    {
        [Min(0)] public int value;
        public TimeUnit unit;

        public Duration(int value, TimeUnit unit) 
        {
            this.value = value;
            this.unit = unit;
        }

        public static Duration OfMilliseconds(int milliseconds) => new Duration(milliseconds, TimeUnit.Milliseconds);
        public static Duration OfSeconds(int seconds) => new Duration(seconds, TimeUnit.Seconds);
        public static Duration OfMinutes(int minutes) => new Duration(minutes, TimeUnit.Minutes);
        public static Duration OfHours(int hours) => new Duration(hours, TimeUnit.Hours);

        public float TotalMilliseconds => unit switch
        {
            TimeUnit.Milliseconds => value,
            TimeUnit.Seconds => value * 1000f,
            TimeUnit.Minutes => value * 60000f,
            TimeUnit.Hours => value * 3600000f,
            _ => throw new ArgumentOutOfRangeException()
        };

        public float TotalSeconds => unit switch
        {
            TimeUnit.Milliseconds => value / 1000f,
            TimeUnit.Seconds => value,
            TimeUnit.Minutes => value * 60f,
            TimeUnit.Hours => value * 3600f,
            _ => throw new ArgumentOutOfRangeException()
        };

        public TimeSpan TimeSpan => unit switch
        {
            TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(value),
            TimeUnit.Seconds => TimeSpan.FromSeconds(value),
            TimeUnit.Minutes => TimeSpan.FromMinutes(value),
            TimeUnit.Hours => TimeSpan.FromHours(value),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}