using System;

namespace ByteSizedAttributes {

    /// <summary>
    /// Enum Flags Attribute allows for multiple enum
    /// values to be selected at once.
    /// </summary>

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class EnumFlagsAttribute : DrawerAttribute {
        public EnumFlagsAttribute() { }
    }
}