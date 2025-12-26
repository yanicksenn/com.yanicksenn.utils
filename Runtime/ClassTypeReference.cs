using System;
using UnityEngine;

namespace YanickSenn.Utils
{
    [Serializable]
    public class ClassTypeReference : ISerializationCallbackReceiver
    {
        [SerializeField] private string _assemblyQualifiedName;

        private Type _type;

        public Type Type
        {
            get
            {
                if (_type == null && !string.IsNullOrEmpty(_assemblyQualifiedName))
                {
                    _type = Type.GetType(_assemblyQualifiedName);
                }
                return _type;
            }
            set
            {
                _type = value;
                _assemblyQualifiedName = value?.AssemblyQualifiedName;
            }
        }

        public void OnBeforeSerialize()
        {
            if (_type != null)
            {
                _assemblyQualifiedName = _type.AssemblyQualifiedName;
            }
        }

        public void OnAfterDeserialize()
        {
            // Validating or loading the type here can be done, but lazy loading in the getter is often safer
            // to avoid loading order issues during deserialization.
            // However, we can try to eagerly load if the string is present.
            if (!string.IsNullOrEmpty(_assemblyQualifiedName))
            {
                _type = Type.GetType(_assemblyQualifiedName);
            }
        }

        public override string ToString()
        {
            return Type?.Name ?? "(None)";
        }
        
        public static implicit operator Type(ClassTypeReference reference)
        {
            return reference?.Type;
        }
    }
}
