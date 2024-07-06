using System;

namespace ByteSizedAttributes {

	/// <summary>
	/// Forces any numerical value to be a negative numerical value.
	/// </sumary>

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class NegativeValueAttribute : ValidatorAttribute { }
}