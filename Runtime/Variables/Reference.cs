using System;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils
{
    public abstract class Reference<TValue>
    {
        [SerializeField] private TValue _const;
        [SerializeField] private Variable<TValue> _variable;
        [SerializeField] private bool useVariable;

        public TValue Value => useVariable ? _variable.Value : _const;

        public Reference(TValue value = default) {
            _const = value;
        }
    }
}
