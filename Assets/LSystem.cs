using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class LSystem
{
    private List<Module> axiom;
    private List<ProductionRule> rules;
    private Dictionary<char, float> varMap;

    public LSystem()
    {
        axiom = new List<Module>();
        rules = new List<ProductionRule>();
        varMap = new Dictionary<char, float>();
    }

    public LSystem(string a) : this()
    {
        axiom = new List<Module>();

        foreach(char c in a)
        {
            axiom.Add(new Module(c));
        }
    }

    public LSystem(Module a) : this()
    {
        axiom.Add(a);
    }

    public LSystem(List<Module> a) : this()
    {        
        axiom = a;
    }


    public List<Module> RunSystem(int iterations)
    {
        List<Module> nextWord = new List<Module>();
        List<Module> curWord = axiom;

        for (int i = 0; i < iterations;  ++i)
        {
            foreach (Module m in curWord)
            {
                Debug.Log(m);
                bool replaced = false;
                foreach (ProductionRule r in rules)
                {
                    if (r.pre.Equals(m))
                    {
                        if (ApplyRule(r.prob))
                        {
                            for (int j = 0; j < r.suc.Count; ++j)
                            {
                                float[] paramList = r.suc[j].parameters.ToArray();
                                Debug.Log(r.suc[j] + "has " + paramList.Length + " params.");
                                paramList = r.suc[j].trans(paramList);
                                List<float> ps = new List<float>();
                                ps.InsertRange(0, paramList);
                                r.suc[j].SetParams(ps);
                                nextWord.Add(r.suc[j]);
                                replaced = true;
                            }
                        }
                    }
                }

                if (!replaced)
                    nextWord.Add(m);
            }

            curWord.Clear();
            curWord.InsertRange(curWord.Count, nextWord);
            nextWord.Clear();
        }
        return curWord;
    }

    public void AddRule(Module p, List<Module> s, float prob)
    {
        rules.Add(new ProductionRule(p, s, prob));
    }

    public void AddRule(Module p, List<Module> s)
    {
        AddRule(p, s, 1.0f);
    }

    public void AddRule(char c, List<Module> s)
    {
        Module m = new Module(c);
        AddRule(m, s);
    }

    public void AddRule(char p, string s)
    {
        AddRule(p, s, 1.0f);
    }

    public void AddRule(char p, string s, float prob)
    {
        Module m = new Module(p);
        List<Module> w = new List<Module>();

        foreach (char c in s)
        {
            w.Add(new Module(c));
        }
        AddRule(m, w, prob);
    }

    private bool ApplyRule(float prob)
    {
        float r = Random.Range(0.0f, 1.0f);
        return r <= prob;
    }

    private int CountParams(char[] paramList)
    {
        int paramCount = 1;
        foreach(char c in paramList)
        {
            if (c == ',')
                ++paramCount;
        }
        return paramCount;
    }

    public void SetVar(char c, float f)
    {
        varMap[c] = f;
    }

    public float GetVar(char c)
    {
        return varMap[c];
    }
}
