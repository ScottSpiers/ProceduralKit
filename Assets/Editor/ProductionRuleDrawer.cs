
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ProductionRule))]
public class ProductionRuleDrawer : PropertyDrawer
{

    private float height;
    private const string TOOLTIP_PARAMS = "The number of params the predecessor must have to have this rule applied. e.g value of 1 for predecessor X will catch X(var) but not X or X(var1, var2).";
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        string suc = "";
        int nParams = 0;
        List<Module> mods = new List<Module>();

        float lblWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = lblWidth * 0.25f;

        //Improve these widths!!
        SerializedProperty preModProp = property.FindPropertyRelative("pre");
        SerializedProperty preProp = preModProp.FindPropertyRelative("sym");
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width * 0.15f, position.height), preProp, new GUIContent("Pre: "));

        EditorGUIUtility.labelWidth = lblWidth * 0.5f;

        SerializedProperty paramProp = preModProp.FindPropertyRelative("parameters");
        nParams = EditorGUI.IntField(new Rect(position.x + position.width * 0.15f, position.y, position.width * 0.15f, position.height), new GUIContent("#Params: ", TOOLTIP_PARAMS), paramProp.arraySize);
        paramProp.arraySize = nParams;

        EditorGUIUtility.labelWidth = lblWidth * 0.334f;

        SerializedProperty repProp = property.FindPropertyRelative("sucRep");
        suc = EditorGUI.DelayedTextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.55f, position.height), "Suc: ", repProp.stringValue);
        repProp.stringValue = suc;

        //Range from 0->1
        SerializedProperty probProp = property.FindPropertyRelative("desiredProb");
        EditorGUI.PropertyField(new Rect(position.x + position.width * 0.85f, position.y, position.width * 0.15f, position.height), probProp, new GUIContent("Prob: "));
        

        EditorGUIUtility.labelWidth = lblWidth;
        EditorGUI.EndProperty();        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + height;
    }
}
