using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PCGWindow : EditorWindow
{
    bool isInit = true;
    private string str = "Default";
    private int num = 1;

    private List<string> rules = new List<string>();
    [SerializeField] private List<ProductionRule> pRules = new List<ProductionRule>();

    
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
        SerializedProperty prop = obj.FindProperty("pRules");
        EditorGUILayout.PropertyField(prop, new GUIContent("Rules: "), true);
        obj.ApplyModifiedProperties();
        Repaint();

        num = EditorGUILayout.IntField("Iterations: ", num);

        if (GUILayout.Button("Generate"))
        {
            Debug.Log("Generating...");
            foreach(ProductionRule r in pRules)
            {
                lSys.AddRule(r);
            }

            string str_out = "";
            foreach(Module m in lSys.RunSystem(num))
            {
                str_out += m;
            }
            Debug.Log(str_out);
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
