using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class PCGWindow : EditorWindow
{
    bool isInit = true;
    private string str = "Default";
    private int num = 1;
    private float stepSize = 1f;
    private float width = 1f;
    private float angle = 22.7f;
    private int numOut = 1;

    private Vector2 scrollPos;
    private int createdCount = 0;

    private int dupCount;

    private List<string> rules = new List<string>();
    [SerializeField] private List<ProductionRule> pRules = new List<ProductionRule>();
    [SerializeField] private List<string> keyList = new List<string>();
    [SerializeField] private List<float> valList = new List<float>(); //eeeeeeewwwwwwww


    LSystem lSys = new LSystem();
    

    [MenuItem("Window/PCGKit")]
    public static void ShowWindow()
    {
        GetWindow<PCGWindow>("PCG");
    }


    /*
     *  EditorGUILayout.BeginHorizontal();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();
        */

    private void OnGUI()
    {
        lSys.Clear();
        
        if(isInit && rules.Count <= 0)
        {
            rules.Add("");
            pRules.Add(new ProductionRule(new Module('F'), new List<Module>(), 1.0f));
            isInit = false;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        //scrollPos = EditorGUILayout.BeginVertical(GUILayout.Height(100));

        GUILayout.Label("Variables: ", EditorStyles.boldLabel);
        SerializedObject obj = new SerializedObject(this);

        SerializedProperty keyProp = obj.FindProperty("keyList");
        EditorGUILayout.PropertyField(keyProp, new GUIContent("Names: "), true);

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

        GUILayout.Label("L-Systems:", EditorStyles.boldLabel);
        
        str = EditorGUILayout.DelayedTextField("Axiom: ", lSys.GetAxiom());
        lSys.SetAxiom(str);


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

        //Moves the generate button
        Repaint();

        stepSize = EditorGUILayout.FloatField("Segment Length: ", stepSize);

        width = EditorGUILayout.FloatField("Segment Width: ", width);

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

            List<Mesh> meshes = new List<Mesh>();
            dupCount = 0;
            //dupMap.Clear();

            string symbols = "Ff+-[]|";
            for (int i = 0; i < numOut; ++i)
            {
                List<Module> mods = lSys.RunSystem(num);
                string str_out = "";
                foreach (Module m in mods)
                {
                    if(symbols.IndexOf(m.sym) != -1)
                        str_out += m;
                }


                //str_out = Regex.Replace(str_out, "[^Ff+*/ ()0-9|,-]", "", RegexOptions.Compiled);
                //str_out.Replace("^[Ff+-()[][0-9]", "");
                //Debug.Log(str_out);

                //if (dupMap.ContainsKey(str_out))
                //{
                //    dupMap[str_out] += 1;
                //}
                //else
                //{
                //    dupMap.Add(str_out, 1);
                //}
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
                Mesh newMesh = intptr.InterpretSystem(mods, stepSize, width, angle);
                mf.mesh = newMesh;

                Dictionary<Vector3, int> vertexCount = new Dictionary<Vector3, int>();
                foreach(Vector3 v in newMesh.vertices)
                {
                    if(vertexCount.ContainsKey(v))
                    {
                        vertexCount[v] += 1;
                    }
                    else
                    {
                        vertexCount.Add(v, 1);
                    }
                }

                bool isDuplicate = false;
                foreach(Mesh m in meshes)
                {
                    Dictionary<Vector3, int> vCount = new Dictionary<Vector3, int>();
                    foreach(Vector3 v in m.vertices)
                    {
                        if (vCount.ContainsKey(v))
                        {
                            vCount[v] += 1;
                        }
                        else
                        {
                            vCount.Add(v, 1);
                        }
                    }

                    bool isDup = true;
                    if (vertexCount.Keys.Count == vCount.Keys.Count)
                    {
                        foreach (KeyValuePair<Vector3, int> kv in vertexCount)
                        {
                            if (!vCount.ContainsKey(kv.Key))
                            {
                                isDup = false;
                                break;
                            }
                            else if (vCount[kv.Key] != kv.Value)
                            {
                                isDup = false;
                                break;
                            }
                        }
                        if (isDup)
                        {
                            isDuplicate = true;
                            ++dupCount;
                        }
                    }
                }
                if (!isDuplicate)
                    meshes.Add(newMesh);

                ++createdCount;

                AssetDatabase.CreateAsset(mf.sharedMesh, "Assets/NewGO" + createdCount + ".mesh");
                //AssetDatabase.CreateAsset(mr.sharedMaterial, "Assets/NewGO" + createdCount + ".mat");
                PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/NewGO" + createdCount + ".prefab", InteractionMode.AutomatedAction);               
                
            }

            //foreach (KeyValuePair<Tuple<Vector3[], int[]>, int> kv in dupMap)
            //{
            //    if (kv.Value > 1)
            //    {
            //        dupCount += kv.Value - 1;
            //        Debug.Log("Duplicate Reuslt: " + kv.Key + "\nTimes Generated: " + kv.Value);
            //    }
            //}
            Debug.Log("Num Duplicates: " + dupCount);
        }
        EditorGUILayout.EndScrollView();

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
