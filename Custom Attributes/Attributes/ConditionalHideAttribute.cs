using System;

namespace ByteSizedAttributes {

    /// <summary>
    /// This script allows to hide/show, enable or disable variables
    /// based on another variable's value.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : DrawerAttribute {
        public string ConditionalSourceField = "";
        public bool HideInInspector = false;

        public ConditionalHideAttribute(string conditionalSourceField) {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector) {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
        }
    }
}