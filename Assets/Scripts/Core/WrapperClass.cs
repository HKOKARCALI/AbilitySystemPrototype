using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Core
{
 
    public class Attribute
    {
        readonly float _baseValue;

        readonly List<Modifier> _modifiers = new List<Modifier>();

      
        public float Value { get; private set; }

       
        public Attribute(float baseValue)
        {
            _baseValue = baseValue;
            CalculateValue();
        }

        void CalculateValue()
        {
            Value = _baseValue;
            foreach (Modifier modifier in _modifiers)
            {
                Value += modifier.GetModification(_baseValue);
            }
        }

        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
            CalculateValue();
        }

        public void Reset()
        {
            _modifiers.Clear();
            CalculateValue();
        }
    }


    public abstract class Modifier
    {
        protected readonly float _modificationValue;

        protected readonly string _modificationName;

     
        public Modifier(float value, string name)
        {
            _modificationValue = value;
            _modificationName = name;
        }

        public abstract float GetModification(float baseValue);

        public override bool Equals(object obj)
        {
            return _modificationName.Equals(obj.ToString());
        }

        public override int GetHashCode() { return ToString().GetHashCode(); }
    }

    public class AdditionModifier : Modifier
    {
        public AdditionModifier(float value, string name) : base(value, name)
        { }

        public override float GetModification(float baseValue)
        {
            return _modificationValue;
        }

        public override string ToString()
        {
            string sign = _modificationValue > 0 ? "+" : _modificationValue < 0 ? "-" : "";
            return _modificationName + " (" + sign + Mathf.Abs(_modificationValue) + ")";
        }
    }

    public class MultiplicationModifier : Modifier
    {
        public MultiplicationModifier(float value, string name) : base(value, name)
        { }

        public override float GetModification(float baseValue)
        {
            return baseValue * _modificationValue;
        }

        public override string ToString()
        {
            string sign = _modificationValue > 0 ? "+" : _modificationValue < 0 ? "-" : "";
            return _modificationName + " (" + sign + (Mathf.Abs(_modificationValue) * 100f) + "%)";
        }
    }
}
