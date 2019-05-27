using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ProductionRule))]
public class ProductionRuleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        int indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;


        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 100), property.FindPropertyRelative("pre"), true);
        EditorGUI.PropertyField(new Rect(position.x, position.y + 100, position.width, 100), property.FindPropertyRelative("suc"), true);
        EditorGUI.PropertyField(new Rect(position.x, position.y +200, position.width, position.height), property.FindPropertyRelative("prob"));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }
}
