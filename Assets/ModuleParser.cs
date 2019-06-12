using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;

public class ModuleParser
{


   public static List<Module> StringToModuleList(string s)
    {
        List<Module> mods = new List<Module>();
        int modIndex = -1;

        for(int i = 0; i < s.Length; ++i)
        {
            switch(s[i])
            {
                case '(': string sParams = s.Substring(i+1, s.IndexOf(')', i) - (i+1)); Debug.Log(sParams);  ParseParams(sParams, mods[modIndex]); i += sParams.Length + 1;  break; //trying to ignore brackets!
                default: mods.Add(new Module(s[i])); ++modIndex; break;

            }
        }
        return mods;
    }

    private static void ParseParams(string s, Module m)
    {
        //substr to , if exists
        //if it does
        //parse the substr
        //else check for ops
        //if(ops) 
        //perform op on thing before and thing after
        //add value to m.params
        //else add value to m.params

        //String.Split!!! split on , and we have the array for every element in this array parse it! We don't need recursion this way right?!?!?
        //Just do it for every string in the split 

        string[] sParams = s.Split(',');
        float val = 0.0f;

        foreach(string p in sParams)
        {
            //need to handle not (!) and negative numbers (-1)!
            int opIndex = p.IndexOfAny(new char[] { '*', '/', '+', '-', '&', '^', '|'});

            if (opIndex >= 1)
            {
                float temp1 = float.Parse(p.Substring(0, opIndex));
                float temp2 = float.Parse(p.Substring(opIndex + 1)); //assuming no other ops and stuff

                char op = p[opIndex];
                if (op == '*') val = temp1 * temp2;
                if (op == '/') val = temp1 / temp2;
                if (op == '+') val = temp1 + temp2;
                if (op == '-') val = temp1 - temp2;
                //if (op == '&') val = temp1 & temp2;
                //if (op == '^') val = temp1 ^ temp2;
                //if (op == '|') val = temp1 | temp2;
                //if (op == '!') val = temp1 ! temp2;
            }
            else
            {
                val = float.Parse(p);
            }
            m.parameters.Add(val);
        }
    }
}
