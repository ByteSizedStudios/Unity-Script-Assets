using System;

namespace ByteSizedAttributes {

    /// <summary>
    /// Creates a minimum possible value for any given numerical variable.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : ValidatorAttribute {
        public float minValue { get; private set; }

        public MinValueAttribute(int MinVlaue) {
            minValue = MinVlaue;
        }

        public MinValueAttribute(float MinVlaue) {
            minValue = MinVlaue;
        }
    }
}