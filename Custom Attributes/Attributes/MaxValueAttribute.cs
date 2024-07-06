using System;

namespace ByteSizedAttributes {

    /// <summary>
    /// Creates a maximum possible value for any given numerical variable.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute {
        public float maxValue { get; private set; }

        public MaxValueAttribute(int MaxVlaue) {
            maxValue = MaxVlaue;
        }

        public MaxValueAttribute(float MaxVlaue) {
            maxValue = MaxVlaue;
        }
    }
}