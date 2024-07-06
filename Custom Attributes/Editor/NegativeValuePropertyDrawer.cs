using UnityEngine;
using UnityEditor;

namespace ByteSizedAttributes.Editor {

    /// <summary>
    /// This script contains the logic for the "Negative Value" attribute.
    /// The script will check for the type of numerical variable as well as
    /// checking if the value is over zero to multiply by -1.
    /// </sumary>

    [CustomPropertyDrawer(typeof(NegativeValueAttribute))]
    public class NegativeValuePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType == SerializedPropertyType.Integer) {
                if (property.intValue > 0) {
                    property.intValue *= -1;
                }

                property.intValue = EditorGUI.IntField(position, label, property.intValue);
            }
            else if (property.propertyType == SerializedPropertyType.Float) {
                if (property.floatValue > 0) {
                    property.floatValue *= -1;
                }

                property.floatValue = EditorGUI.FloatField(position, label, property.floatValue);
            }
            else {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}