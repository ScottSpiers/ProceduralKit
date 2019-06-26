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
        str = EditorGUILayout.DelayedTextField("Axiom: ", lSys.GetAxiom());
        lSys.SetAxiom(str);

        SerializedObject obj = new SerializedObject(this);

        SerializedProperty keyProp = obj.FindProperty("keyList");
        EditorGUILayout.PropertyField(keyProp, new GUIContent("Variables: "), true);

        SerializedProperty valProp = obj.FindProperty("valList");
        EditorGUILayout.PropertyField(valProp, new GUIContent("Values: "), true);


        int lowCount = keyList.Count;
        if (valList.Count < lowCount)
        {
            lowCount = valList.Count;
            
        }

        for(int i = 0; i < lowCount; ++i)
        {
            lSys.SetVar(keyList[i], valList[i]);
        }

        //TIE THESE IN ^^^^^^!!!!!

        SerializedProperty ruleProp = obj.FindProperty("pRules");
        EditorGUILayout.PropertyField(ruleProp, new GUIContent("Rules: "), true);
        
        obj.ApplyModifiedProperties();

        
        foreach(ProductionRule pr in pRules)
        {
            pr.suc.Clear();
            if(pr.sucRep.Length > 0)
            {
                List<Module> mods = ModuleParser.StringToModuleList(pr.sucRep, lSys);
                pr.suc.InsertRange(0, mods);
            }
        }

        //can go into rules and then get trans from there... how do I know what to set it to though?
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
            string str_out = "";
            foreach (Module m in mods)
            {
                str_out += m;
            }
            Debug.Log(str_out);

            Interpreter intptr = new Interpreter();
            GameObject go = new GameObject("Tree");
            go.transform.position = Vector3.zero;
            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mr.sharedMaterial = mat;
            mr.sharedMaterial.color = Color.black;
            mf.mesh = intptr.InterpretSystem(mods, .1f, .01f, 60.0f);
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
