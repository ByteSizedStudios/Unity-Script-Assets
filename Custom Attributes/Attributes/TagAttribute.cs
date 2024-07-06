using System;
using UnityEngine;

namespace ByteSizedAttributes {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : PropertyAttribute {
        public bool UseDefaultTagFieldDrawer = false;
    }
}