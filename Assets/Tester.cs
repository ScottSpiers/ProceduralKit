using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Module a = new Module('A', new List<float>{ 1.0f });
        LSystem lSys = new LSystem(a);
        lSys.SetVar("c", 1.0f);
        lSys.SetVar("p", 0.3f);
        lSys.SetVar("q", lSys.GetVar("c") - lSys.GetVar("p"));
        lSys.SetVar("h", Mathf.Pow(lSys.GetVar("p") * lSys.GetVar("q"), 0.5f));

        lSys.SetVar("R", 1.456f);


        Module m = new Module('F', new List<float> { 1.0f }, (float[] f) => { return f; });
        Module m1 = new Module('A', new List<float> { 1.0f }, (float[] f) => { float[] s = new float[f.Length];  s[0] = f[0] / lSys.GetVar("R"); return s; });
        Module m11 = new Module('A', new List<float> { 1.0f }, (float[] f) => { float[] s = new float[f.Length]; s[0] = f[0] / lSys.GetVar("R"); return s; });
        Module m2 = new Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar("q"); return f; });

        Module mPlus = new Module('+');
        Module mMinus = new Module('-');
        Module mOpen = new Module('[');
        Module mClose = new Module(']');
        
        List<Module> suc = new List<Module> { new Module(m), mOpen, mPlus,
                                                new Module('A', new List<float> { 1.0f }, (float[] f) => { float[] s = new float[f.Length];  s[0] = f[0] / lSys.GetVar("R"); return s; }),
                                                mClose, mOpen, mMinus,
                                                new Module('A', new List<float> { 1.0f }, (float[] f) => { float[] s = new float[f.Length];  s[0] = f[0] / lSys.GetVar("R"); return s; }),
                                            mClose };
        lSys.AddRule(a, suc);


        List<Module> mods = lSys.RunSystem(3);
        Debug.Log(mods.Count);
        string str_out = "";
        foreach(Module mod in mods)
        {
            str_out += mod;
        }
        Debug.Log(str_out);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
