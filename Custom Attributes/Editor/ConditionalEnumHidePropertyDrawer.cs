using UnityEngine;
using UnityEditor;

namespace ByteSizedAttributes.Editor {

    /// <summary>
    /// This script is the logic for hiding variables using a condition
    /// of another variable.
    /// </summary>

    [CustomPropertyDrawer(typeof(ConditionalEnumHideAttribute))]
    public class ConditionalEnumHidePropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            ConditionalEnumHideAttribute condHAtt = (ConditionalEnumHideAttribute)attribute;
            int enumValue = GetCondtionalEnumHideAttributeResult(condHAtt, property);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = ((condHAtt.EnumValue1 == enumValue) || (condHAtt.EnumValue2 == enumValue));
            if (!condHAtt.HideInInspector || (condHAtt.EnumValue1 == enumValue) || (condHAtt.EnumValue2 == enumValue)) {
                EditorGUI.PropertyField(position, property, label, true);
            }
            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            ConditionalEnumHideAttribute condHAtt = (ConditionalEnumHideAttribute)attribute;
            int enumValue = GetCondtionalEnumHideAttributeResult(condHAtt, property);

            if (!condHAtt.HideInInspector || (condHAtt.EnumValue1 == enumValue) || (condHAtt.EnumValue2 == enumValue)) {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        private int GetCondtionalEnumHideAttributeResult(ConditionalEnumHideAttribute condHAtt, SerializedProperty property) {
            int enumValue = 0;
            SerializedProperty sourcePropertyValue = null;
            if (!property.isArray) {
                string propertyPath = property.propertyPath;
                string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
                sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

                if (sourcePropertyValue == null) {
                    sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
                }
            }
            else {
                sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
            }

            if (sourcePropertyValue != null) {
                enumValue = sourcePropertyValue.enumValueIndex;
            }

            return enumValue;
        }
    }
}