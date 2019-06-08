using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
public class Module
{
    public delegate float[] Transition(float[] args);
    public Module(char a)
    {
        sym = a; parameters = new List<float>();
        trans = new Transition((float[] f) => { return f; });
    }
    public Module(char a, List<float> ps)
    {
        sym = a;
        parameters = ps;
        trans = new Transition((float[] f) => { return f; });
    }
    public Module(char a, List<float> ps, Transition t)
    {
        sym = a;
        parameters = ps;
        trans = t;
    }

    public Module(Module m)
    {
        sym = m.sym;
        parameters = m.parameters;
        trans = m.trans;
    }

    public char sym;
    public List<float> parameters;
    public Transition trans { get; }

    public void SetParams(List<float> ps)
    {
        parameters.Clear();
        parameters.InsertRange(0, ps);
    }

    public override bool Equals(object obj)
    {
        Expression<Transition> test;
        Module other = (Module)obj;
        return (sym == other.sym) && (parameters.Count == other.parameters.Count);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string str_out = sym.ToString();
        if (parameters.Count > 0)
        {
            str_out += "(";

            foreach (float f in parameters)
            {
                str_out += f;
                str_out += ",";
            }

            str_out = str_out.Remove(str_out.Length - 1);
            str_out += ")";
        }
        return str_out;
    }
}
