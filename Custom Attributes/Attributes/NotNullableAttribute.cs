using System;

namespace ByteSizedAttributes {

    /// <summary>
    /// Not Nullable Attribute forces any given script 
    /// to no be nullable or have no value
    /// </summary>

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class NotNullableAttribute : ValidatorAttribute { }
}