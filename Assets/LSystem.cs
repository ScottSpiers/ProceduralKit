using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class LSystem
{
    private List<Module> axiom;
    private List<ProductionRule> rules;
    private Dictionary<string, float> varMap;
    private string axiomRep;


    private Dictionary<char, float> probMap;

    private MetricCounter mc;
    public LSystem()
    {
        axiomRep = "";
        axiom = new List<Module>();
        rules = new List<ProductionRule>();
        varMap = new Dictionary<string, float>();
        mc = new MetricCounter();
        probMap = new Dictionary<char, float>();
    }

    public LSystem(string a) : this()
    {
        axiomRep = a;
        axiom = new List<Module>();

        foreach(char c in a)
        {
            axiom.Add(new Module(c));
        }
    }

    public LSystem(Module a) : this()
    {
        axiomRep = a.ToString();
        axiom.Add(a);
    }

    public LSystem(List<Module> a) : this()
    {
        foreach(Module m in a)
        {
            axiomRep += m;
        }
        axiom = a;
    }


    public List<Module> RunSystem(int iterations)
    {
        
        //mc.ResetMetrics();
        List<Module> nextWord = new List<Module>();
        List<Module> curWord = new List<Module>();
        curWord.InsertRange(0, axiom);

        for (int i = 0; i < iterations;  ++i)
        {
            foreach (Module m in curWord)
            {
                //Debug.Log(m);
                bool replaced = false;
                foreach (ProductionRule r in rules)
                {
                    if (r.pre.Equals(m))
                    {
                        if (ApplyRule(r.prob) && !replaced)
                        {
                            for (int j = 0; j < r.suc.Count; ++j)
                            {
                                //might want to check for params first
                                float[] paramList = m.parameters.ToArray();
                                List<float> ps = new List<float>();
                                
                                //Debug.Log("Module: " + m.sym + " has param: " + paramList[0]);

                                if(r.suc[j].parameters.Count > 0)
                                {
                                    ps.Clear();
                                    paramList = r.suc[j].trans(paramList);
                                    ps.InsertRange(0, paramList);
                                    //r.suc[j].SetParams(ps);
                                }
                                nextWord.Add(new Module(r.suc[j].sym, ps));
                                replaced = true;
                            }
                        }
                    }
                }

                if (!replaced)
                    nextWord.Add(m);
            }

            //if(i < iterations - 1)
            //{
            //    string mods = "";
            //    foreach(Module m in nextWord)
            //    {
            //        mods += m;
            //    }

            //    mc.MetricCount(mods);
            //}
            curWord.Clear();
            curWord.InsertRange(curWord.Count, nextWord);
            nextWord.Clear();
        }

        //string mods2 = "";
        //foreach (Module m in curWord)
        //{
        //    mods2 += m;
        //}

        //Debug.Log("NumRooms: " + mc.GetRooms() + "\nNumCorridors: " + mc.GetCorridors() + "\nNumTurns: " + mc.GetTurns(mods2));
        //mc.UpdateLevelCount();
        //Debug.Log(mc.GetLevelCount());

        return curWord;
    }

    public void Clear()
    {
        rules.Clear();
        probMap.Clear();
    }

    public void AddRule(ProductionRule r)
    {
        float p = r.desiredProb;
        if (probMap.ContainsKey(r.pre.sym))
        {
            r.prob = p / (1 - probMap[r.pre.sym]);
            probMap[r.pre.sym] += p;
        }
        else
        {
            r.prob = p;
            probMap.Add(r.pre.sym, p);
        }

        Debug.Log("Desired: " + r.desiredProb + "\tEffective: " + r.prob);
        rules.Add(r);
    }

    public void AddRule(Module p, List<Module> s, float prob)
    {

        float pTemp = prob;
        if (probMap.ContainsKey(p.sym))
        {
            pTemp = prob / (1 - probMap[p.sym]);
            probMap[p.sym] += prob;
        }
        else
        {
            probMap.Add(p.sym, prob);
        }
        rules.Add(new ProductionRule(p, s, pTemp, prob));
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

    //Need to be careful about params here, will possibly 
    //need to parse or be creative with GUI
    public void SetAxiom(string s)
    {
        axiomRep = s;
        axiom = ModuleParser.StringToModuleList(s, this);
        //axiom = new List<Module>();
        //foreach(Module m in axiom)
        //{
        //    axiomRep += m;
        //}

        //foreach (char c in s)
        //{
        //    axiom.Add(new Module(c));
        //}
    }

    public string GetAxiom()
    {
        return axiomRep;
        //string str_out = "";
        //foreach(Module m in axiom)
        //{
        //    str_out += m;
        //}
        //return str_out;
    }

    public void SetVar(string s, float f)
    {
        varMap[s] = f;
    }

    public float GetVar(string s)
    {
        return varMap[s];
    }
}
