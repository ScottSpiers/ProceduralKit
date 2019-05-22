using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LSystem.Module a = new LSystem.Module('F', new List<float>{ 1.0f });
        LSystem lSys = new LSystem(a);
        lSys.SetVar('c', 1.0f);
        lSys.SetVar('p', 0.3f);
        lSys.SetVar('q', lSys.GetVar('c') - lSys.GetVar('p'));
        lSys.SetVar('h', Mathf.Pow(lSys.GetVar('p') * lSys.GetVar('q'), 0.5f));


        LSystem.Module m = new LSystem.Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('p'); return f; });
        LSystem.Module m1 = new LSystem.Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('h'); return f; });
        LSystem.Module m11 = new LSystem.Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('h'); return f; });
        LSystem.Module m2 = new LSystem.Module('F', new List<float> { 1.0f }, (float[] f) => { f[0] *= lSys.GetVar('q'); return f; });

        LSystem.Module mPlus = new LSystem.Module('+');
        LSystem.Module mMinus = new LSystem.Module('-');
        List<LSystem.Module> suc = new List<LSystem.Module> { m, mPlus, m1, mMinus, mMinus, m11,mPlus, m2 };
        lSys.AddRule(a, suc);


        List<LSystem.Module> mods = lSys.RunSystem(1);
        Debug.Log(mods.Count);
        string str_out = "";
        foreach(LSystem.Module mod in mods)
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
