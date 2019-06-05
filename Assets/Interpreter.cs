using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter
{
    public Interpreter()
    {

    }

    public MeshFilter InterpretSystem(List<Module> modules, float stepSize, float width, float angleDelta)
    {
        TurtleState curState = new TurtleState();
        TurtleState nextState = new TurtleState();

        Stack<TurtleState> turtleStack = new Stack<TurtleState>();
        Stack<int> indexStack = new Stack<int>();

        int index = 0;

        foreach(Module m in modules)
        {
            nextState = curState;
            Vector3 rotated = curState.rot.MultiplyVector(Vector3.up);
            Quaternion q = new Quaternion();
            


            switch (m.sym)
            {
                case 'F':
                    {
                        nextState.pos += rotated * curState.stepSize; //XMVectorAdd(nextState.pos, XMVectorScale(rotated, curState.stepSize));

                        //Check params: Assume 1st is stepSize 2nd is width 3rd is stepDelta 4th is branchDelta
                        
                        //create vertices in cur and next pos, create mid vertex as ((up * width) + cur.pos)
                        //creating 4 vertices on 1st run 2 after that: the next pos and it's width offset
                        
                        //if branchReturn 
                            //do proper index things
                        //else
                            //indices = Ic-2, Ic-1, Ic++

                        break;
                    }
                case 'f':
                    {
                        nextState.pos += curState.stepSize * rotated;
                        //THIS IS STILL RELEVANT
                        //index = m_indices.size(); //need to increase the index, in case we  go back or just so we don't draw where we shouldn't 
                        break;
                    }
                case '+':
                    {
                        nextState.RotateAxisAngle(Vector3.forward, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], angleDelta);
                        break;
                    }
                case '-':
                    {
                        //check for params: 1st is angle, should there be any more?
                        //change these back to 2!
                        nextState.RotateAxisAngle(Vector3.forward, -angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], -angleDelta);
                        break;
                    }
                case '&':
                    {
                        nextState.RotateAxisAngle(Vector3.right, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[0], angleDelta);
                        break;
                    }
                case '^':
                    {
                        nextState.RotateAxisAngle(Vector3.right, -angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[0], -angleDelta);
                        break;
                    }
                case '\\':
                    {
                        nextState.RotateAxisAngle(Vector3.up, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[1], angleDelta);
                        break;
                    }
                case '/':
                    {
                        nextState.RotateAxisAngle(Vector3.up, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[1], -angleDelta);
                        break;
                    }
                case '|':
                    {
                        nextState.RotateAxisAngle(Vector3.forward, 180.0f);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], (180.0f * XM_PI) / 180);
                        break;
                    }
                case '[':
                    {
                        //nextState.stepSize = curState.stepSize + 1.0f;
                        //make this a percentage of the curradius?
                        //nextState.width = curState.width - branchRedDelta;
                        //if (nextState.radius < 0.05f)
                        //    nextState.radius = 0.05f;
                        turtleStack.Push(curState);
                        indexStack.Push(index);
                        break;
                    }
                case ']':
                    {
                        nextState = turtleStack.Pop();
                        index = indexStack.Pop();
                        break;
                    }
                default:
                    break;
            }
            curState = nextState;
        }
        return null;
    }
}
