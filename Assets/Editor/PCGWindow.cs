using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class PCGWindow : EditorWindow
{
    bool isInit = true;
    private string str = "Default";
    private int num = 1;
    private float angle = 22.7f;
    private int numOut = 1;

    private int createdCount = 0;
    private Dictionary<string, int> dupMap = new Dictionary<string, int>();
    private int dupCount;

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

        //Moves the genertae button
        Repaint();

        angle = EditorGUILayout.FloatField("Angle: ", angle);

        num = EditorGUILayout.IntField("Iterations: ", num);

        numOut = EditorGUILayout.IntField("Output Count: ", numOut);

        if (GUILayout.Button("Generate"))
        {
            //Debug.Log("Generating...");
            foreach(ProductionRule r in pRules)
            {
                lSys.AddRule(r);
            }


            float xOffset = 10f;

            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
            AssetDatabase.CreateAsset(mat, "Assets/GeometryMaterial.mat");

            dupCount = 0;
            dupMap.Clear();
            for (int i = 0; i < numOut; ++i)
            {
                List<Module> mods = lSys.RunSystem(num);
                string str_out = "";
                foreach (Module m in mods)
                {
                    str_out += m;
                }


                if (dupMap.ContainsKey(str_out))
                {
                    dupMap[str_out] += 1;
                }
                else
                {
                    dupMap.Add(str_out, 1);
                }
                //Debug.Log(str_out);

                    

                Interpreter intptr = new Interpreter();
                GameObject go = new GameObject("Geometry" + i);
                //go.transform.position = Vector3.zero;
                go.transform.position = new Vector3(0f, 0f, 0f);
                MeshFilter mf = go.AddComponent<MeshFilter>();
                MeshRenderer mr = go.AddComponent<MeshRenderer>();
            
               //Material mat = new Material(Shader.Find("Sprites/Default"));
                mr.sharedMaterial = mat;
                //mr.sharedMaterial.color = Color.black;
                mf.mesh = intptr.InterpretSystem(mods, 1.0f, 1.0f, angle);

                ++createdCount;

                AssetDatabase.CreateAsset(mf.sharedMesh, "Assets/NewGO" + createdCount + ".mesh");
                //AssetDatabase.CreateAsset(mr.sharedMaterial, "Assets/NewGO" + createdCount + ".mat");
                PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/NewGO" + createdCount + ".prefab", InteractionMode.AutomatedAction);               
                
            }

            foreach (KeyValuePair<string, int> kv in dupMap)
            {
                if (kv.Value > 1)
                {
                    dupCount += kv.Value - 1;
                    Debug.Log("Duplicate Reuslt: " + kv.Key + "\nTimes Generated: " + kv.Value);
                }
            }
            Debug.Log("Num Duplicates: " + dupCount);
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
