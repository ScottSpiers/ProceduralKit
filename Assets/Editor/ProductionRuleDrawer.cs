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
        //property.isExpanded = true;

        SerializedProperty preProp = property.FindPropertyRelative("pre");
        SerializedProperty sucProp = property.FindPropertyRelative("suc");

        float preHeight = (preProp.isExpanded ? 150 : 30);
        float sucHeight = (sucProp.isExpanded ? 150 : 30);

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, preHeight), preProp, true);
        EditorGUI.PropertyField(new Rect(position.x, position.y + preHeight + 5, position.width, sucHeight), sucProp, true);
        EditorGUI.PropertyField(new Rect(position.x, position.y + preHeight + sucHeight + 5, position.width, 30), property.FindPropertyRelative("prob"));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return 1.5f * EditorGUI.GetPropertyHeight(property, label);
        }
        return EditorGUI.GetPropertyHeight(property, label);
    }
}
