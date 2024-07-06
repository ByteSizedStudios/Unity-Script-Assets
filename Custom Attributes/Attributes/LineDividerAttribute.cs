using System;
using UnityEngine;

namespace ByteSizedAttributes {

    /// <summary>
    /// This attributes creates a line divider in the inspector.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class LineDividerAttribute : PropertyAttribute {

        // Default height of line 
        public const float DefaultHeight = 2.0f;
        // Default color of line
        public const LineColors DefaultColor = LineColors.Gray;

        public float Height { get; private set; }
        public LineColors Color { get; private set; }

        public LineDividerAttribute(float height = DefaultHeight, LineColors color = DefaultColor) {
            Height = height;
            Color = color;
        }
    }
}