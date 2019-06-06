﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter
{
    public Interpreter()
    {

    }

    public Mesh InterpretSystem(List<Module> modules, float stepSize, float width, float angleDelta)
    {
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        TurtleState curState = new TurtleState();
        TurtleState nextState = new TurtleState();
        curState.stepSize = stepSize;
        curState.width = width;
        curState.rot = Quaternion.identity;
        curState.pos = Vector3.zero;
        nextState.stepSize = stepSize;
        nextState.width = width;

        Stack<TurtleState> turtleStack = new Stack<TurtleState>();
        Stack<int> indexStack = new Stack<int>();

        Vector3 offset = Vector3.up * width;
        offset.Normalize();

        int index = 0;
        bool isBranchReturn = false;
        bool isStart = true;

        Quaternion q = Quaternion.identity;

        foreach(Module m in modules)
        {
            //nextState = new TurtleState(curState);

            Vector3 rotated = q * (Vector3.up);
            
            //Quaternion q = new Quaternion();

            switch (m.sym)
            {
                case 'F':
                    {
                        //Check params: Assume 1st is stepSize 2nd is width 3rd is stepDelta 4th is branchDelta
                        if (m.parameters.Count >= 1) //will need to extend this for width
                            curState.stepSize = m.parameters[0];
                        
                        nextState.pos += rotated * curState.stepSize; //XMVectorAdd(nextState.pos, XMVectorScale(rotated, curState.stepSize));

                        Vector3 offsetPoint = nextState.pos + offset * nextState.width;
                        
                        if(isStart) //we just started, create 4 vertices
                        {
                        
                            //Add Vertices
                            verts.Add(curState.pos);
                            //verts.Add(curState.pos + (offset * width));
                            verts.Add(nextState.pos);
                            //verts.Add(offsetPoint);

                            //Add Indices
                            indices.Add(0);
                            indices.Add(1);
                            //indices.Add(2);

                            index++;
                            isStart = false;

                            //indices.Add(index - 2);
                            //indices.Add(index - 1);
                            //indices.Add(index++);
                        }
                        else
                        {
                            //Add Vertices
                            verts.Add(nextState.pos);
                            //verts.Add(offsetPoint);
                            int tempIndex = indices[indices.Count - 1];
                            indices.Add(index);
                            index = tempIndex + 1;
                            indices.Add(index);

                            //Add Indices
                            //if (isBranchReturn)
                            //{
                            //    indices.Add(index);
                            //    index = indices[indices.Count];
                            //    indices.Add(index);
                            //    //indices.Add(index + 1);

                            //    //indices.Add(index + 1);
                            //    //indices.Add(indices[indices.Count - 1]);
                            //    //index = indices[indices.Count - 1] +1;
                            //    //indices.Add(index++);
                            //    isBranchReturn = false;
                            //}
                            //else
                            //{
                            //    //indices.Add(index - 2);
                            //    indices.Add(index - 1);
                            //    indices.Add(index++);

                            //    //indices.Add(index - 2);
                            //    //indices.Add(index - 1);
                            //    //indices.Add(index++);
                            //}

                        }

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
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.forward * angleDelta));
                        //nextState.RotateAxisAngle(Vector3.forward, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], angleDelta);
                        break;
                    }
                case '-':
                    {
                        //check for params: 1st is angle, should there be any more?
                        //change these back to 2!
                        //nextState.RotateAxisAngle(Vector3.forward, -angleDelta);
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.forward * -angleDelta));
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], -angleDelta);
                        break;
                    }
                case '&':
                    {
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.right * angleDelta));
                        //nextState.RotateAxisAngle(Vector3.right, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[0], angleDelta);
                        break;
                    }
                case '^':
                    {
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.right * -angleDelta));
                        //nextState.RotateAxisAngle(Vector3.right, -angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[0], -angleDelta);
                        break;
                    }
                case '\\':
                    {
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.up * angleDelta));
                        //nextState.RotateAxisAngle(Vector3.up, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[1], angleDelta);
                        break;
                    }
                case '/':
                    {
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.up * -angleDelta));
                        //nextState.RotateAxisAngle(Vector3.up, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[1], -angleDelta);
                        break;
                    }
                case '|':
                    {
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.forward * 180.0f));
                        //nextState.RotateAxisAngle(Vector3.forward, 180.0f);
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
                        curState.rot = q;
                        
                        turtleStack.Push(new TurtleState(curState));
                        indexStack.Push(index);
                        break;
                    }
                case ']':
                    {
                        TurtleState retState = new TurtleState(turtleStack.Pop());
                        nextState.pos = retState.pos;
                        //nextState.rot = retState.rot;
                        nextState.stepSize = retState.stepSize;
                        nextState.width = retState.width;
                        q = retState.rot;
                        index = indexStack.Pop();
                        isBranchReturn = true;
                        break;
                    }
                default:
                    break;
            }
            curState = new TurtleState(nextState); //this might cause issues
           
        }
        mesh.SetVertices(verts);
        
        mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        //mesh.SetTriangles(indices, 0);
        mesh.name = "Tree";
        //mesh.RecalculateNormals();
        return mesh;
    }
}