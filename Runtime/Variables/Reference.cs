using System;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils
{
    public abstract class Reference<TValue> : IValue<TValue>, IChangeEventEmitter<TValue>
    {
        public event Action<TValue, TValue> OnValueChanged;

        [SerializeField] private TValue _const;
        [SerializeField] private Variable<TValue> _variable;
        [SerializeField] private bool useVariable;

        public TValue Value
        {
            get => useVariable ? _variable.Value : _const;
            set
            {
                var oldValue = Value;
                if (Equals(oldValue, value))
                {
                    return;
                }

                if (useVariable)
                {
                    _variable.Value = value;
                }
                else
                {
                    _const = value;
                }

                OnValueChanged?.Invoke(value, oldValue);
            }
        }

        public Reference(TValue value = default) {
            _const = value;
        }
    }
}
