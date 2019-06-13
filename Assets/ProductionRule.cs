﻿using System.Collections;
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
    }

    public ProductionRule(Module p, List<Module> s, float probability)
    {
        pre = p;
        suc = s;
        sucRep = "";
        prob = probability;
    }

    public Module pre;
    public List<Module> suc;
    public string sucRep; //string representation of the successor
    public float prob;
}
