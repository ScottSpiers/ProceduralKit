using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PCGWindow : EditorWindow
{
    private SerializedObject serObj;
    private string axiom = "";
    private int num = 1;
    private float stepSize = 1f;
    private float width = 1f;
    private float angle = 22.7f;
    private int numOut = 1;
    private int createdCount = 0;
    private int varsCreated = 0;

    private Vector2 scrollPos;
    private Vector2 variableScrollPos;
    private Vector2 prodRuleScrollPos;

    private Interpreter.TurtlePos turtlePos;

    private Dictionary<string, int> dupMap = new Dictionary<string, int>();
    private int dupCount;

    [SerializeField] private List<ProductionRule> pRules = new List<ProductionRule>();
    [SerializeField] private VariableMap<string,float> variables = new VariableMap<string,float>();
    
    //Removed Code - Will remove once confident everything is working as part of a future commmit.
    
    //bool isInit = true;
    //[SerializeField] private List<string> keyList = new List<string>();
    //[SerializeField] private List<float> valList = new List<float>(); //eeeeeeewwwwwwww
    //private List<string> rules = new List<string>(); //Is this the original Prod Rules?
    //[SerializeField] List<Variable> varList = variables.

    LSystem lSys = new LSystem();
    

    [MenuItem("Window/PCGKit")]
    public static void ShowWindow()
    {
        GetWindow<PCGWindow>("PCG");
    }

    public void OnEnable()
    {
        serObj = new SerializedObject(this);
        
        if(pRules.Count < 1)
        {
                pRules.Add(new ProductionRule(new Module('F'), new List<Module>(), 1.0f, 1.0f));
        }
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
        // if(isInit)
        // {
        //     //lSys = new LSystem();
        //     //rules.Add("");
        //     //varsCreated = variables.Count;
            
        //     //serObj = new SerializedObject(this);
        //     isInit = false;
        // }

        lSys.Clear(); //Could do this first thing on clicking "Generate"? Will that keep adding production rules...

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos); 
        //scrollPos = EditorGUILayout.BeginVertical(GUILayout.Height(100));

        GUILayout.Label("L-Systems:", EditorStyles.boldLabel);
        //SerializedObject obj = new SerializedObject(this);

        // SerializedProperty keyProp = obj.FindProperty("keyList");
        // EditorGUILayout.PropertyField(keyProp, new GUIContent("Names: "), true);

        // SerializedProperty valProp = obj.FindProperty("valList");
        // EditorGUILayout.PropertyField(valProp, new GUIContent("Values: "), true);

        DrawVariableMap();
        //GUILayout.Space(VariableDrawer.propHeight);
        // int lowCount = keyList.Count;
        // if (valList.Count < lowCount)
        // {
        //     lowCount = valList.Count;
            
        // }

        //change to just copy dict?
        foreach(KeyValuePair<string, float> kv in variables)
        {
            //Debug.Log("Key: " + kv.Key + " = " + kv.Value);
            lSys.SetVar(kv.Key, kv.Value);
            // lSys.SetVar(variables[i].key, variables[i].value);
        }

        axiom = EditorGUILayout.DelayedTextField("Axiom: ", lSys.GetAxiom());
        lSys.SetAxiom(axiom);

        DrawProductionRules();
        
        serObj.ApplyModifiedProperties();

        
        foreach(ProductionRule pr in pRules)
        {
            pr.suc.Clear();
            if(pr.sucRep.Length > 0)
            {
                List<Module> mods = ModuleParser.StringToModuleList(pr.sucRep, lSys);
                pr.suc.InsertRange(0, mods);
            }
        }

        stepSize = EditorGUILayout.FloatField("Segment Length: ", stepSize);
        width = EditorGUILayout.FloatField("Segment Width: ", width);
        angle = EditorGUILayout.FloatField("Angle: ", angle);
        num = EditorGUILayout.IntField("Iterations: ", num);
        numOut = EditorGUILayout.IntField("Output Count: ", numOut);

        turtlePos = (Interpreter.TurtlePos) EditorGUILayout.EnumPopup("Turtle Position: ", turtlePos);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Generate"))
        {
            //Debug.Log("Generating...");

            //Debug.Log(pRules.Count);
            foreach (ProductionRule r in pRules)
            {
                lSys.AddRule(r);
            }


            float xOffset = 10f;

            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            AssetDatabase.CreateAsset(mat, "Assets/GeometryMaterial.mat");

            List<Mesh> meshes = new List<Mesh>();
            dupCount = 0;
            dupMap.Clear();

            //string symbols = "Ff+-[]|";
            for (int i = 0; i < numOut; ++i)
            {
                EditorUtility.DisplayProgressBar("Generating Geometry...", "Generating Geometry...", ((float)i / numOut));
                List<Module> mods = lSys.RunSystem(num);
                string str_out = "";
                foreach (Module m in mods)
                {
                    str_out += m;
                    //if (symbols.IndexOf(m.sym) != -1)
                }


                //str_out = Regex.Replace(str_out, "[^Ff+*/ ()0-9|,-]", "", RegexOptions.Compiled);
                //str_out.Replace("^[Ff+-()[][0-9]", "");
                //Debug.Log(str_out);

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
                Mesh newMesh = intptr.InterpretSystem(mods, turtlePos, stepSize, width, angle);
                mf.mesh = newMesh;

                //Dictionary<Vector3, int> vertexCount = new Dictionary<Vector3, int>();
                //foreach (Vector3 v in newMesh.vertices)
                //{
                //    if (vertexCount.ContainsKey(v))
                //    {
                //        vertexCount[v] += 1;
                //    }
                //    else
                //    {
                //        vertexCount.Add(v, 1);
                //    }
                //}

                //bool isDuplicate = false;
                //foreach (Mesh m in meshes)
                //{
                //    Dictionary<Vector3, int> vCount = new Dictionary<Vector3, int>();
                //    foreach (Vector3 v in m.vertices)
                //    {
                //        if (vCount.ContainsKey(v))
                //        {
                //            vCount[v] += 1;
                //        }
                //        else
                //        {
                //            vCount.Add(v, 1);
                //        }
                //    }

                //    bool isDup = true;
                //    if (vertexCount.Keys.Count == vCount.Keys.Count)
                //    {
                //        foreach (KeyValuePair<Vector3, int> kv in vertexCount)
                //        {
                //            if (!vCount.ContainsKey(kv.Key))
                //            {
                //                isDup = false;
                //                break;
                //            }
                //            else if (vCount[kv.Key] != kv.Value)
                //            {
                //                isDup = false;
                //                break;
                //            }
                //        }
                //        if (isDup)
                //        {
                //            isDuplicate = true;
                //            ++dupCount;
                //        }
                //    }
                //}
                //if (!isDuplicate)
                //    meshes.Add(newMesh);

                ++createdCount;

                AssetDatabase.CreateAsset(mf.sharedMesh, "Assets/NewGO" + createdCount + ".mesh");
                //AssetDatabase.CreateAsset(mr.sharedMaterial, "Assets/NewGO" + createdCount + ".mat");
                PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/NewGO" + createdCount + ".prefab", InteractionMode.AutomatedAction);               
                
            }
            EditorUtility.ClearProgressBar();

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
        EditorGUILayout.EndScrollView();

    }

    private bool prodRulesExpanded = false;

    private void DrawProductionRules()
    {
        float lblWidth = EditorGUIUtility.labelWidth;
        SerializedProperty ruleProp = serObj.FindProperty("pRules");

        prodRulesExpanded = EditorGUILayout.Foldout(prodRulesExpanded, "Rules:", true);
        if(prodRulesExpanded)
        {
            //This is affecting the labels for the proprty drawer for some reason.
            // Should improve field widths in there but this does for now.
            EditorGUIUtility.labelWidth = lblWidth * 0.6f;
            bool shouldScroll = pRules.Count >= 5;

            if(shouldScroll)
            {
                prodRuleScrollPos = EditorGUILayout.BeginScrollView(prodRuleScrollPos, GUILayout.Height(100f));
            }

            //Want to add Rule # before drawing property?
            for(int i = 0; i < ruleProp.arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(ruleProp.GetArrayElementAtIndex(i));

                if(GUILayout.Button("x", GUILayout.Width(35f))) //put this in a method? Get remove button? Pass behaviour default width = 35f? Probs not worth
                {
                    ruleProp.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            if(shouldScroll)
            {
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("+"))
            {
                ++ruleProp.arraySize;
            }

            if(pRules.Count > 0)
            {
                if(GUILayout.Button("-"))
                {
                    --ruleProp.arraySize;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private bool variablesExpanded = false;
    private string varName = "var";
    private void DrawVariableMap()
    {
        float lblWidth = EditorGUIUtility.labelWidth;

        SerializedProperty varProp = serObj.FindProperty("variables");

        variablesExpanded = EditorGUILayout.Foldout(variablesExpanded,"Variables:",  true);
        if(variablesExpanded)
        {
            EditorGUIUtility.labelWidth = lblWidth * 0.334f;

            SerializedProperty keysProp = varProp.FindPropertyRelative("keys");
            int numKeys = keysProp.arraySize;
            SerializedProperty valuesProp = varProp.FindPropertyRelative("values");
            valuesProp.arraySize = numKeys;

            bool shouldScroll = numKeys >= 5;
            if(shouldScroll)
            {
                variableScrollPos = EditorGUILayout.BeginScrollView(variableScrollPos, GUILayout.Height(100f));
            }
                      
            for(int i = 0; i < numKeys; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(keysProp.GetArrayElementAtIndex(i), new GUIContent("Key " + (i+1)));
                EditorGUILayout.PropertyField(valuesProp.GetArrayElementAtIndex(i), new GUIContent("Value " + (i+1)));

                if(GUILayout.Button("x", GUILayout.Width(35f)))
                {
                    keysProp.DeleteArrayElementAtIndex(i);
                    valuesProp.DeleteArrayElementAtIndex(i);
                    --numKeys;
                }
                EditorGUILayout.EndHorizontal();
            }

            if(shouldScroll)
            {
                EditorGUILayout.EndScrollView();
            }
            
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("+"))
            {
                ++keysProp.arraySize;
                ++valuesProp.arraySize;
                ++varsCreated;

                //Get to a key we haven't used. This shouldn't be needed too often.
                while(variables.ContainsKey(varName + varsCreated))
                {
                    ++varsCreated;
                }

                keysProp.GetArrayElementAtIndex(keysProp.arraySize-1).stringValue = varName + varsCreated;
            }

            if(numKeys > 0)
            {
                if(GUILayout.Button("-"))
                {
                    --keysProp.arraySize;
                    --valuesProp.arraySize;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUIUtility.labelWidth = lblWidth;
        }
    }
}
