using System;

namespace YanickSenn.Utils
{
    public interface IChangeEventEmitter<T>
    {
        event Action<T, T> OnValueChanged;
    }
}
