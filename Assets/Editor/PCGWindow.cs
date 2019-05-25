using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class PCGWindow : EditorWindow
{
    bool isInit = true;
    private string str = "Default";
    private int num = 1;

    private List<string> rules = new List<string>();

    [MenuItem("Window/PCGKit")]
    public static void ShowWindow()
    {
        GetWindow<PCGWindow>("PCG");
    }

    private void OnGUI()
    {
        if(isInit)
        {
            rules.Add("");
            isInit = false;
        }

        GUILayout.Label("Trees", EditorStyles.boldLabel);
        num = EditorGUILayout.DelayedIntField("#Rules", num);

        if (num != rules.Count)
            UpdateList();      
        

        for(int i = 0; i < num; ++i)
        {
            rules[i] = EditorGUILayout.TextField("Rule #" + (i+1) + ": ", rules[i]);
        }

    }

    private void UpdateList()
    {
        int n = rules.Count;
        if (num < n)
        {
            while (n > num)
            {
                rules.RemoveAt(n - 1);
                --n;
            }
        }
        else if (num > n)
        {
            while (n < num)
            {
                rules.Add("");
                ++n;
            }
        }
    }
}
