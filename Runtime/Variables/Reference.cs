using System;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils
{
    public abstract class Reference<TValue, TVariable> 
            where TValue : IComparable<TValue>
            where TVariable : Variable<TValue>
    {
        [SerializeField] private TValue _const;
        [SerializeField] private TVariable _variable;
        [SerializeField] private bool useVariable;

        public TValue Value => useVariable ? _variable.Value : _const;

        public Reference(TValue value = default) {
            _const = value;
        }
    }
}