using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Module a = new Module('F', new List<float>{ 1.0f });
        LSystem lSys = new LSystem(a);
        lSys.SetVar('c', 1.0f);
        lSys.SetVar('p', 0.3f);
        lSys.SetVar('q', lSys.GetVar('c') - lSys.GetVar('p'));
        lSys.SetVar('h', Mathf.Pow(lSys.GetVar('p') * lSys.GetVar('q'), 0.5f));


        Module m = new Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('p'); return f; });
        Module m1 = new Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('h'); return f; });
        Module m11 = new Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('h'); return f; });
        Module m2 = new Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('q'); return f; });

        Module mPlus = new Module('+');
        Module mMinus = new Module('-');
        List<Module> suc = new List<Module> { m, mPlus, m1, mMinus, mMinus, m11,mPlus, m2 };
        lSys.AddRule(a, suc);


        List<Module> mods = lSys.RunSystem(1);
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
