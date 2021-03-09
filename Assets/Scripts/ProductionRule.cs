using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ProductionRule
{
    public ProductionRule()
    {
        pre = new Module('F');
        suc = new List<Module>();
        sucRep = "";
        prob = 1.0f;
        desiredProb = 1.0f;
    }

    public ProductionRule(Module p, List<Module> s, float probability, float desProb)
    {
        pre = p;
        suc = s;
        sucRep = "";
        prob = probability;
        desiredProb = desProb;
    }

    public Module pre;
    public List<Module> suc;
    public string sucRep; //string representation of the successor
    public float prob;
    [Range(0f, 1f)] public float desiredProb;
}
