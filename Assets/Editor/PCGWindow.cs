using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class PCGWindow : EditorWindow
{
    bool isInit = true;
    private string str = "Default";
    private int num = 1;

    private List<string> rules = new List<string>();
    [SerializeField] private List<ProductionRule> pRules = new List<ProductionRule>();
    [SerializeField] private List<char> keyList = new List<char>();
    [SerializeField] private List<float> valList = new List<float>(); //eeeeeeewwwwwwww


    LSystem lSys = new LSystem();
    

    [MenuItem("Window/PCGKit")]
    public static void ShowWindow()
    {
        GetWindow<PCGWindow>("PCG");
    }

    private void OnGUI()
    {
        lSys.Clear();
        
        if(isInit)
        {
            rules.Add("");
            pRules.Add(new ProductionRule(new Module('F'), new List<Module>(), 1.0f));
            isInit = false;
        }

        GUILayout.Label("Trees", EditorStyles.boldLabel);
        str = EditorGUILayout.TextField("Axiom: ", lSys.GetAxiom());
        lSys.SetAxiom(str);

        SerializedObject obj = new SerializedObject(this);

        SerializedProperty keyProp = obj.FindProperty("keyList");
        EditorGUILayout.PropertyField(keyProp, new GUIContent("Variables: "), true);

        SerializedProperty valProp = obj.FindProperty("valList");
        EditorGUILayout.PropertyField(valProp, new GUIContent("Values: "), true);


        //TIE THESE IN ^^^^^^!!!!!

        SerializedProperty ruleProp = obj.FindProperty("pRules");
        EditorGUILayout.PropertyField(ruleProp, new GUIContent("Rules: "), true);
        obj.ApplyModifiedProperties();
        Repaint();

        num = EditorGUILayout.IntField("Iterations: ", num);

        if (GUILayout.Button("Generate"))
        {
            //Debug.Log("Generating...");
            foreach(ProductionRule r in pRules)
            {
                lSys.AddRule(r);
            }

            List<Module> mods = lSys.RunSystem(num);
            //string str_out = "";
            //foreach(Module m in mods)
            //{
            //    str_out += m;
            //}
            //Debug.Log(str_out);

            Interpreter intptr = new Interpreter();
            GameObject go = new GameObject("Tree");
            go.transform.position = Vector3.zero;
            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            Material mat = new Material(Shader.Find("Standard"));
            mr.sharedMaterial = mat;
            mr.sharedMaterial.color = Color.black;
            mf.mesh = intptr.InterpretSystem(mods, 0.02f, 100.0f, 25.7f);
        }

    }

    //private void UpdateList()
    //{
    //    int n = rules.Count;
    //    if (num < n)
    //    {
    //        while (n > num)
    //        {
    //            rules.RemoveAt(n - 1);
    //            //pRules.RemoveAt(n - 1);
    //            --n;
    //        }
    //    }
    //    else if (num > n)
    //    {
    //        while (n < num)
    //        {
    //            rules.Add("");
    //            //pRules.Add(new ProductionRule(new Module('F'), new List<Module>(), 1.0f));
    //            ++n;
    //        }
    //    }
    //}

    //private ProductionRule EditRule()
    //{
    //    return null;
    //}
}
