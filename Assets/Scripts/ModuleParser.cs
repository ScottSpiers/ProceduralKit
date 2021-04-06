using System.Collections.Generic;
using System.Linq.Expressions;

public class ModuleParser
{
   public static List<Module> StringToModuleList(string s, LSystem l)
    {
        List<Module> mods = new List<Module>();
        int modIndex = -1;

        for(int i = 0; i < s.Length; ++i)
        {
            switch(s[i])
            {
                case '(': string sParams = s.Substring(i+1, s.IndexOf(')', i) - (i+1)); /*Debug.Log(sParams);*/  ParseParams(sParams, mods[modIndex], l); i += sParams.Length + 1;  break; //trying to ignore brackets!
                default: mods.Add(new Module(s[i])); ++modIndex; break;

            }
        }
        return mods;
    }

    private static void ParseParams(string s, Module m, LSystem l)
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
        s = s.Replace(" ", "");
        string[] sParams = s.Split(',');
        float val = 0.0f;

        List<Expression> exps = new List<Expression>();

        //Our input f
        ParameterExpression f = Expression.Parameter(typeof(float[]), "f");
        ParameterExpression newFs = Expression.Variable(typeof(float[]), "s");
     
        //Create a new param array based on the number of params we now have
        //ParameterExpression newFs = Expression.Variable(typeof(float[]), "s");
                

        NewArrayExpression nArray = Expression.NewArrayBounds(typeof(float), Expression.Constant(sParams.Length));
        BinaryExpression assignS = Expression.Assign(newFs, nArray);


        //exps.Add(f);
        exps.Add(newFs);
        exps.Add(nArray);
        exps.Add(assignS);

        int nArrayIndex = 0;


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
                string str_t1 = p.Substring(0, opIndex);
                bool isT1 = float.TryParse(str_t1, out temp1);
                float temp2 = 0.0f;
                string str_t2 = p.Substring(opIndex + 1);
                bool isT2 = float.TryParse(str_t2, out temp2); //assuming no other ops and stuff

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
                    Expression e = Expression.Assign(Expression.ArrayAccess(newFs, Expression.Constant(nArrayIndex)), Expression.Constant(val));
                    exps.Add(e);
                    m.parameters.Add(val);
                }
                else
                {
                    //the param is a transition, add 0.0f to params and build expression tree storing compiled version to trans

                    //Should this maybe go into a method?
                    //going to be using this stuff multiple times
                    //what gets passed? p? or just the substring till or after the op?
                    //returns a list of exprs to add to the list...
                    Expression exp1 = null;
                    Expression exp2 = null;

                    if (!isT1 && !isT2)
                    {
                        //then both are an issue
                        //create exprs for t1 and t2
                        if (str_t1[0] == '$')
                            exp1 = Expression.ArrayAccess(f, Expression.Constant(int.Parse(str_t1.Substring(1))));
                        else
                        {
                            exp1 = Expression.Call(Expression.Constant(l), typeof(LSystem).GetMethod("GetVar"), Expression.Constant(str_t1));
                            //Debug.Log(exp1);
                        }

                        if (str_t2[0] == '$')
                            exp2 = Expression.ArrayAccess(f, Expression.Constant(int.Parse(str_t2.Substring(1))));
                        else
                            exp2 = Expression.Call(Expression.Constant(l), typeof(LSystem).GetMethod("GetVar"), Expression.Constant(str_t2));
                    }
                    else if (isT1)
                    {
                        exp1 = Expression.Constant(temp1);
                        
                        if(str_t2[0] == '$')
                            exp2 = Expression.ArrayAccess(f, Expression.Constant(int.Parse(str_t2.Substring(1))));
                        else
                            exp2 = Expression.Call(Expression.Constant(l), typeof(LSystem).GetMethod("GetVar"), Expression.Constant(str_t2));
                    }
                    //then t2 is the issue
                    else
                    {
                        exp2 = Expression.Constant(temp2);

                        if (str_t1[0] == '$')
                        {
                            exp1 = Expression.ArrayAccess(f, Expression.Constant(int.Parse(str_t1.Substring(1))));
                        }
                        else
                        {
                            exp1 = Expression.Call(Expression.Constant(l), typeof(LSystem).GetMethod("GetVar"), Expression.Constant(str_t1));
                            //Debug.Log(exp1);
                        }


                    } 
                    //t1 is the issue
                    Expression opExp = null;

                    if (op == '*') opExp = Expression.Multiply(exp1, exp2);
                    if (op == '/') opExp = Expression.Divide(exp1, exp2);
                    if (op == '+') opExp = Expression.Add(exp1, exp2);
                    if (op == '-') opExp = Expression.Subtract(exp1, exp2);

                    Expression setS = Expression.Assign(Expression.ArrayAccess(newFs, Expression.Constant(nArrayIndex)), opExp);

                    exps.Add(exp1);
                    exps.Add(exp2);
                    exps.Add(opExp);
                    exps.Add(setS);
                    //Keep the params array the correct length with default value, it will get overwritten
                    m.parameters.Add(0.0f);
                }

            }
            else
            {
                val = 0.0f;
                bool isVal = float.TryParse(p, out val);
                Expression e;
                if (isVal)
                {
                    e = Expression.Assign(Expression.ArrayAccess(newFs, Expression.Constant(nArrayIndex)), Expression.Constant(val));
                    exps.Add(e);
                    m.parameters.Add(val);
                }

                else
                {
                    if(p[0] == '$')
                    {
                        int index = int.Parse(p.Substring(1));
                        e = Expression.Assign(Expression.ArrayAccess(newFs, Expression.Constant(nArrayIndex)),Expression.ArrayAccess(f, Expression.Constant(index)));
                        exps.Add(e);
                        m.parameters.Add(0.0f);
                    }
                    else
                    {

                        //Need the lSys instance for this... hmmm
                        
                        e = Expression.Assign(Expression.ArrayAccess(newFs, Expression.Constant(nArrayIndex)), Expression.Call(Expression.Constant(l), typeof(LSystem).GetMethod("GetVar"), Expression.Constant(p)));
                        //Debug.Log(e);
                        exps.Add(e);
                        m.parameters.Add(l.GetVar(p));
                    }                    
                }

                //Expression<Module.Transition> trans = Expression.Lambda<Module.Transition>();
                //trans.Compile();
                //need to test for vars / params here
                //if const then add, if var then methodcall, if param then array access
                
            }

            ++nArrayIndex;
        }

        LabelTarget returnTarget = Expression.Label(typeof(float[]));
        GotoExpression returnExp = Expression.Return(returnTarget, newFs);
        Expression defaultValue = Expression.Constant(new float[sParams.Length]);
        LabelExpression returnLabel = Expression.Label(returnTarget,defaultValue);

        exps.Add(returnExp);
        exps.Add(returnLabel);
        BlockExpression block = Expression.Block(new[] { newFs }, exps);
        Expression<Module.Transition> trans = Expression.Lambda<Module.Transition>(block, f);
        m.trans = trans.Compile();
    }
}
