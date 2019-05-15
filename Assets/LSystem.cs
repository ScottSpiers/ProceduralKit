using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem
{
    struct ProductionRule
    {
        public ProductionRule(char p, string s) { pre = p; suc = s; prob = 1.0f; }
        public ProductionRule(char p, string s, float probability) { pre = p; suc = s; prob = probability; }
        public char pre { get; }
        public string suc { get; }
        public float prob { get; }
    };

    private string axiom;
    private List<ProductionRule> rules;

    public LSystem()
    {
        axiom = "";
        rules = new List<ProductionRule>();
    }

    public LSystem(string a) : this()
    {        
        axiom = a;
    }

    public void AddRule(char p, string s)
    {
        AddRule(p, s, 1.0f);
    }

    public void AddRule(char p, string s, float prob)
    {
        rules.Add(new ProductionRule(p, s, prob));
    }

    public string RunSystem(int iterations)
    {
        string nextWord = "";
        string curWord = axiom;
        bool replaced = false;

        for(int i = 0; i < iterations; ++i)
        {
            foreach(char c in curWord)
            {
                replaced = false;
                foreach(ProductionRule r in rules)
                {
                    if(c == r.pre)
                    {
                        if (ApplyRule(r.prob))
                        {
                            nextWord += r.suc;
                            replaced = true;
                            break;
                        }
                    }
                }

                if (!replaced)
                    nextWord += c;
            }

            curWord = nextWord;
            nextWord = "";
        }

        return curWord;
    }

    private bool ApplyRule(float prob)
    {
        float r = Random.Range(0.0f, 1.0f);
        return r <= prob;
    }
}
