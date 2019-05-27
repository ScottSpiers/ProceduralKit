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
        prob = 1.0f;
    }

    public ProductionRule(Module p, List<Module> s, float probability)
    {
        pre = p;
        suc = s;
        prob = probability;
    }

    public Module pre;
    public List<Module> suc;

    public float prob;
}
