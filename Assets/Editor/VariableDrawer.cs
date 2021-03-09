/// Unused property drawer that was made to try keep draw code for 
/// LSystem variables out of PCGWindow.cs. Decided it was easier to
/// just do it in PCGWindow though...


// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomPropertyDrawer(typeof(VariableMap<string,float>))]
// public class VariableDrawer : PropertyDrawer
// {  
//     Color bgColour;
//     float varHeight = 0.0f;
//     float varButtonWidthScale = 1.0f;
//     bool isExpanded = false;
//     public static float propHeight = 0.0f;
    
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         EditorGUI.BeginProperty(position, label, property);
//         isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, 25f, 25f), isExpanded, "Variables:", true);
//         if(isExpanded)
//         {
//             float lblWidth = EditorGUIUtility.labelWidth;
//             //EditorGUIUtility.labelWidth = lblWidth * 0.5f;

//             SerializedProperty keysProp = property.FindPropertyRelative("keys");
//             int numKeys = keysProp.arraySize;
//             SerializedProperty valuesProp = property.FindPropertyRelative("values");
//             valuesProp.arraySize = numKeys;

//             varHeight = position.y * numKeys + 25f;
//             propHeight = varHeight + position.height; //why does this only work once?
//             //need something to handle duplicate keys. Add 3 delete var2 and add 1
            
//             for(int i = 0; i < numKeys; ++i)
//             {
//                 EditorGUI.PropertyField(new Rect(position.x, position.y * (i+1) + 25f, position.width *0.35f, position.height), keysProp.GetArrayElementAtIndex(i), new GUIContent("Key " + (i+1)));
//                 EditorGUI.PropertyField(new Rect (position.x + 5 + position.width *0.35f, position.y *(i+1) + 25f, position.width * 0.35f, position.height), valuesProp.GetArrayElementAtIndex(i), new GUIContent("Value " + (i+1)));

//                 if(GUI.Button(new Rect(position.x + 5 + position.width * 0.7f, position.y * (i+1) + 25f, 35f, position.height),"x"))
//                 {
//                     keysProp.DeleteArrayElementAtIndex(i);
//                     valuesProp.DeleteArrayElementAtIndex(i);
//                     --numKeys;
//                 }
//             }
            
//             varButtonWidthScale = numKeys > 0 ? 0.5f : 1.0f;
//             if(GUI.Button(new Rect(position.x, position.y + varHeight, position.width * varButtonWidthScale, position.height), "+"))
//             {
//                 ++keysProp.arraySize;
//                 ++valuesProp.arraySize;

//                 keysProp.GetArrayElementAtIndex(keysProp.arraySize-1).stringValue = "var" + keysProp.arraySize;
//             }

//             if(numKeys > 0)
//             {
//                 if(GUI.Button(new Rect(position.x + position.width * varButtonWidthScale, position.y + varHeight, position.width * varButtonWidthScale, position.height), "-"))
//                 {
//                     --keysProp.arraySize;
//                     --valuesProp.arraySize;
//                 }
//             }
            
//             EditorGUIUtility.labelWidth = lblWidth;
//         }
//         else
//         {
//             propHeight = 0.0f;
//         }
//         EditorGUI.EndProperty();
//     }

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         return base.GetPropertyHeight(property,label);
//     }
// }