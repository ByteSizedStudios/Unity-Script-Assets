using System;
using UnityEngine;
using UnityEditor;

namespace ByteSizedAttributes.Editor {

    [CustomPropertyDrawer(typeof(LineDividerAttribute))]
    public class LineDividerPropertyDrawer : DecoratorDrawer {
        public override float GetHeight() {
            LineDividerAttribute LineAttr = (LineDividerAttribute)attribute;
            return EditorGUIUtility.singleLineHeight + LineAttr.Height;
        }

        public override void OnGUI(Rect position) {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 3.0f;
            LineDividerAttribute LineAttr = (LineDividerAttribute)attribute;

            rect.height = LineAttr.Height;
            EditorGUI.DrawRect(rect, LineAttr.Color.GetColor());
        }
    }
}