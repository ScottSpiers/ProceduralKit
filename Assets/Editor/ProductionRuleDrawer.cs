using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ProductionRule))]
public class ProductionRuleDrawer : PropertyDrawer
{

    private float height;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        int indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;
        //property.isExpanded = true;

        SerializedProperty preProp = property.FindPropertyRelative("pre");
        SerializedProperty sucProp = property.FindPropertyRelative("suc");

        SerializedProperty preParamsProp = preProp.FindPropertyRelative("parameters");

        //SerializedProperty preParamsSizeProp = preParamsProp
        int preParamsSize = preParamsProp.arraySize;
        int sucSize = sucProp.arraySize;

        float preParamsHeight = (preParamsProp.isExpanded ? preParamsSize * 15 : 15);
        float sucParamsHeight = (preParamsProp.isExpanded ? sucSize * 15 : 15);

        float preHeight = (preProp.isExpanded ? 60 + preParamsHeight : 15);
        float sucHeight = (sucProp.isExpanded ? 30 + sucParamsHeight : 15);

        SerializedProperty curProp;

        //foreach element in suc
        for(int i = 0; i < sucSize; ++i)
        {
            //Get the cur element 
            curProp = sucProp.GetArrayElementAtIndex(i);
            //if the cur element is expanded, add 60 to the height
            if (curProp.isExpanded)
                sucHeight += 60;

            //Get params size for cur element
            int paramsSize = curProp.FindPropertyRelative("parameters").arraySize;
            //add onto suc height 15 * size
            sucHeight += 15 * paramsSize;
            
        }

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, preHeight + preParamsHeight), preProp, true);
        EditorGUI.PropertyField(new Rect(position.x, position.y + preHeight + preParamsHeight + 10, position.width, sucHeight + sucParamsHeight), sucProp, true);
        EditorGUI.PropertyField(new Rect(position.x, position.y + preHeight + sucHeight +  preParamsHeight + sucParamsHeight + 10, position.width, 15), property.FindPropertyRelative("prob"));

        height = preHeight + preParamsHeight + sucHeight + sucParamsHeight + 20;

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();

        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + height;
    }
}
