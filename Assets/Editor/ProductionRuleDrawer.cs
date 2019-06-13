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

        string pre = "";
        string suc = "";
        int nParams = 0;
        float prob = 0.0f;
        List<Module> mods = new List<Module>();


        float lblWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = lblWidth * 0.334f;

        SerializedProperty preModProp = property.FindPropertyRelative("pre");
        SerializedProperty preProp = preModProp.FindPropertyRelative("sym");
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width * 0.15f, position.height), preProp, new GUIContent("Pre: "));

        EditorGUIUtility.labelWidth = lblWidth * 0.5f;
        SerializedProperty paramProp = preModProp.FindPropertyRelative("parameters");
        nParams = EditorGUI.IntField(new Rect(position.x + position.width * 0.15f, position.y, position.width * 0.15f, position.height), "#Params: ", paramProp.arraySize);
        paramProp.arraySize = nParams;

        EditorGUIUtility.labelWidth = lblWidth * 0.334f;
        SerializedProperty repProp = property.FindPropertyRelative("sucRep");

        suc = EditorGUI.DelayedTextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.55f, position.height), "Suc: ", repProp.stringValue);
        repProp.stringValue = suc;

        //SerializedProperty sucProp = property.FindPropertyRelative("suc");

        //mods = ModuleParser.StringToModuleList(suc);
        //sucProp.arraySize = mods.Count;

        //for(int i = 0; i < mods.Count; ++i)
        //{
        //    sucProp.GetArrayElementAtIndex(i).FindPropertyRelative("sym").intValue = mods[i].sym;

        //    SerializedProperty paramsProp = sucProp.GetArrayElementAtIndex(i).FindPropertyRelative("parameters");
        //    paramsProp.arraySize = mods[i].parameters.Count;
        //    for(int j = 0; j < mods[i].parameters.Count; ++j)
        //    {
        //        paramsProp.GetArrayElementAtIndex(j).floatValue = mods[i].parameters[j];
        //    }

        //    //HOW THE FUCK DO I SET THE TRANSITION FUNCTION!

        //    Module.Transition t = mods[i].trans;
            
            
        //}

        //m = "";
        //foreach(Module mod in mods)
        //{
        //    m += mod;
        //}

        //parse suc into List<Module> (Static parser?)
        //set array size (sucprop.arraySize = mods.Count)
        //go through list setting arrayElementAtIndex to mods[i]
        //that should be it

        SerializedProperty probProp = property.FindPropertyRelative("prob");
        EditorGUI.PropertyField(new Rect(position.x + position.width * 0.85f, position.y, position.width * 0.15f, position.height), probProp, new GUIContent("Prob: "));
        

        EditorGUIUtility.labelWidth = lblWidth;
        EditorGUI.EndProperty();        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + height;
    }
}
        //int indent = EditorGUI.indentLevel;
        ////EditorGUI.indentLevel = 0;
        ////property.isExpanded = true;

        //SerializedProperty preProp = property.FindPropertyRelative("pre");
        //SerializedProperty sucProp = property.FindPropertyRelative("suc");

        //SerializedProperty preParamsProp = preProp.FindPropertyRelative("parameters");

        ////SerializedProperty preParamsSizeProp = preParamsProp
        //int preParamsSize = preParamsProp.arraySize;
        //int sucSize = sucProp.arraySize;

        //float preParamsHeight = (preParamsProp.isExpanded ? preParamsSize * 15 : 15);
        //float sucParamsHeight = (preParamsProp.isExpanded ? sucSize * 15 : 15);

        //float preHeight = (preProp.isExpanded ? 60 + preParamsHeight : 15);
        //float sucHeight = (sucProp.isExpanded ? 30 + sucParamsHeight : 15);

        //SerializedProperty curProp;

        ////foreach element in suc
        //for(int i = 0; i < sucSize; ++i)
        //{
        //    //Get the cur element 
        //    curProp = sucProp.GetArrayElementAtIndex(i);
        //    //if the cur element is expanded, add 60 to the height
        //    if (curProp.isExpanded)
        //        sucHeight += 60;

        //    //Get params size for cur element
        //    int paramsSize = curProp.FindPropertyRelative("parameters").arraySize;
        //    //add onto suc height 15 * size
        //    sucHeight += 15 * paramsSize;
        //    string str_in = "";
            
            
        //}

        //EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, preHeight + preParamsHeight), preProp, true);
        //EditorGUI.PropertyField(new Rect(position.x, position.y + preHeight + preParamsHeight + 10, position.width, sucHeight + sucParamsHeight), sucProp, true);
        //EditorGUI.PropertyField(new Rect(position.x, position.y + preHeight + sucHeight +  preParamsHeight + sucParamsHeight + 10, position.width, 15), property.FindPropertyRelative("prob"));

        //height = preHeight + preParamsHeight + sucHeight + sucParamsHeight + 20;

        //EditorGUI.indentLevel = indent;
