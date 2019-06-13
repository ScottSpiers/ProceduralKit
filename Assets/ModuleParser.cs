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
                //setup initial expressions (float[] input, float[] s creation, return s) Might also need a block expression 
                //if temp1 is a char, then MethodCall GetVar(<char>) from lSys
                //else if temp1 is $<int> then create param from the float[]?
                //else constant
                //if temp2 ...

                //if(either temp1 or 2 isn't const)
                //create op expression

                //compile the Expression<Transition>? 
                //set m.Transition to this
                
                float temp1 = 0.0f;
                bool isT1 = float.TryParse(p.Substring(0, opIndex), out temp1);
                float temp2 = 0.0f;
                bool isT2 = float.TryParse(p.Substring(opIndex + 1), out temp2); //assuming no other ops and stuff

                char op = p[opIndex];

                if(isT1 && isT2)
                {
                    if (op == '*') val = temp1 * temp2;
                    if (op == '/') val = temp1 / temp2;
                    if (op == '+') val = temp1 + temp2;
                    if (op == '-') val = temp1 - temp2;
                    //if (op == '&') val = temp1 & temp2;
                    //if (op == '^') val = temp1 ^ temp2;
                    //if (op == '|') val = temp1 | temp2;
                    //if (op == '!') val = temp1 ! temp2;
                    m.parameters.Add(val);
                }
                else
                {
                    //the param is a transition, add 0.0f to params and build expression tree storing compiled version to trans
                }

            }
            else
            {
                Expression input = Expression.Parameter(typeof(float[]), "f");
                List<Expression> exps = new List<Expression>();
                val = 0.0f;
                bool isVal = float.TryParse(p, out val);

                if (isVal)
                    m.parameters.Add(val);

                else
                {
                    if(p[0] == '$')
                    {
                        int index = int.Parse(p.Substring(1));
                        exps.Add(Expression.ArrayAccess(input, new Expression[]{ Expression.Constant(index)}));
                    }
                    else
                    {
                        //Need the lSys instance for this... hmmm
                        //exps.Add(Expression.Call());
                    }
                }

                //Expression<Module.Transition> trans = Expression.Lambda<Module.Transition>();
                //trans.Compile();
                //need to test for vars / params here
                //if const then add, if var then methodcall, if param then array access
                
            }
        }
    }
}
