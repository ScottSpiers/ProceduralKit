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
    

    LSystem lSys = new LSystem();
    

    [MenuItem("Window/PCGKit")]
    public static void ShowWindow()
    {
        GetWindow<PCGWindow>("PCG");
    }

    public void OnEnable()
    {
        serObj = new SerializedObject(this);
    }

    private void OnGUI()
    { 
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos); 

        GUILayout.Label("L-Systems:", EditorStyles.boldLabel);

        DrawVariableMap();        

        axiom = EditorGUILayout.DelayedTextField("Axiom: ", axiom);
        
        DrawProductionRules();
        
        serObj.ApplyModifiedProperties();
        
        //What exactly is this doing? What effect will moving it have?
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
            lSys.Clear();

            //Debug.Log("Generating...");
            lSys.SetAxiom(axiom);

            //change to just copy dict?
            foreach(KeyValuePair<string, float> kv in variables)
            {
                //Debug.Log("Key: " + kv.Key + " = " + kv.Value);
                lSys.SetVar(kv.Key, kv.Value);
            }

            //Debug.Log(pRules.Count);
            foreach (ProductionRule r in pRules)
            {
                lSys.AddRule(r);
            }

            //Grab this from scene or set it via UI?
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            AssetDatabase.CreateAsset(mat, "Assets/GeometryMaterial.mat");

            List<Mesh> meshes = new List<Mesh>();
            dupCount = 0;
            dupMap.Clear();

            for (int i = 0; i < numOut; ++i)
            {
                EditorUtility.DisplayProgressBar("Generating Geometry...", "Generating Geometry...", ((float)i / numOut));
                List<Module> mods = lSys.RunSystem(num);

                //Detect duplicates made by the system - somewhat useful but do 
                //we want to disable by default and have option to switch on?
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

                Interpreter intptr = new Interpreter();
                GameObject go = new GameObject("Geometry" + i);
                go.transform.position = new Vector3(0f, 0f, 0f);
                MeshFilter mf = go.AddComponent<MeshFilter>();
                MeshRenderer mr = go.AddComponent<MeshRenderer>();
                mr.sharedMaterial = mat;

                Mesh newMesh = intptr.InterpretSystem(mods, turtlePos, stepSize, width, angle);
                mf.mesh = newMesh;

                ++createdCount;

                AssetDatabase.CreateAsset(mf.sharedMesh, "Assets/NewGO" + createdCount + ".mesh");
                PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/NewGO" + createdCount + ".prefab", InteractionMode.AutomatedAction);               
                
            }
            EditorUtility.ClearProgressBar();

            //See above dupMap comment
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
            //This is affecting the labels for the proprty drawer as it's not reset before it grabs the label width.
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

                if(GUILayout.Button("x", GUILayout.Width(35f)))
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
