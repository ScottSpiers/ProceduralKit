using System.Collections.Generic;
using UnityEngine;

public class Interpreter
{
    public enum TurtlePos
    {
        BOTTOM_LEFT = 0,
        BOTTOM_CENTRE,
        CENTRE_CENTRE
    }

    public Interpreter()
    {

    }

    public Mesh InterpretSystem(List<Module> modules, TurtlePos turtlePos, float stepSize, float width, float angleDelta)
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

        Vector3 offset = Vector3.right;

        int index = 0;

        Quaternion q = Quaternion.Euler(Vector3.up);

        foreach(Module m in modules)
        {
            Vector3 rotated = q * (Vector3.up);

            switch (m.sym)
            {
                case 'F':
                    {
                        //Check params: Assume 1st is stepSize 2nd is width 3rd is stepDelta 4th is branchDelta - how do I make this flexible
                        if (m.parameters.Count > 0)
                            curState.stepSize = m.parameters[0];

                        if (m.parameters.Count > 1)
                            curState.width = m.parameters[1];

                        nextState.pos += rotated * curState.stepSize;

                        Vector3 offsetPoint = nextState.pos + (q *offset) * curState.width;
                        
                        if(turtlePos == TurtlePos.BOTTOM_CENTRE)
                        {
                            Vector3 rotatedRight = (q *offset) * (curState.width /2);
                            offsetPoint = nextState.pos + rotatedRight;
                            verts.Add(curState.pos - rotatedRight);
                            verts.Add(curState.pos + rotatedRight);
                            verts.Add(nextState.pos - rotatedRight);
                            verts.Add(nextState.pos + rotatedRight);
                        }
                        else if(turtlePos == TurtlePos.CENTRE_CENTRE)
                        {
                            Vector3 rotatedRight = (q *offset) * (curState.width /2);
                            Vector3 rotatedUp = (q * Vector3.up) * (curState.stepSize / 2);
                            offsetPoint = nextState.pos + (rotatedUp + rotatedRight);
                            verts.Add(curState.pos - (rotatedUp + rotatedRight)); //BL -( 0.5, 0.5) = (-0.5,-0.5)
                            verts.Add(curState.pos - (rotatedUp - rotatedRight)); //BR -(-0.5,0.5) = ( 0.5,-0.5)
                            verts.Add(curState.pos + (rotatedUp - rotatedRight)); //TL +(-0.5,0.5) = (-0.5, 0.5)
                            verts.Add(curState.pos + (rotatedUp + rotatedRight)); //TR +( 0.5,0.5) = ( 0.5, 0.5)
                        }
                        else 
                        {
                            verts.Add(curState.pos);
                            verts.Add(curState.pos + ((q * offset) * curState.width));
                            verts.Add(nextState.pos);
                            verts.Add(offsetPoint);
                        }

                        //Add Indices
                        indices.Add(index++); //0
                        indices.Add(index++); //1
                        indices.Add(index++); //2

                        indices.Add(index - 1); //2
                        indices.Add(index - 2); //1
                        indices.Add(index++);   //3                      
                        

                        //create vertices in cur and next pos, create mid vertex as ((up * width) + cur.pos)
                        //creating 4 vertices on 1st run 2 after that: the next pos and it's width offset

                        //if branchReturn
                        //do proper index things
                        //else
                        //indices = Ic-2, Ic-1, Ic++

                        //Vector3 a = new Vector3(-0.02f, -1.0f, 0.0f);
                        //Quaternion temp = Quaternion.FromToRotation(a, rotated);
                        
                        //Vector3 temp = Vector3.LerpUnclamped(rotated, a, 0.27f);
                        //q = Quaternion.Euler(q.x + temp.x, q.y + temp.y, q.z + temp.z);
                        //q.SetLookRotation(temp, Vector3.up);
                        //q = Quaternion.Euler(q.x, q.z, q.z);
                        break;
                    }
                case 'f':
                    {
                        if (m.parameters.Count >= 1)
                            curState.stepSize = m.parameters[0];

                        nextState.pos += curState.stepSize * rotated;
                        //THIS IS STILL RELEVANT
                        //index = m_indices.size(); //need to increase the index, in case we  go back or just so we don't draw where we shouldn't
                        break;
                    }
                case '+':
                    {
                        float angle = angleDelta;
                        if (m.parameters.Count >= 1)
                            angle = m.parameters[0];

                        //some nastiness our rotation is right but the next step will be weird so fake it!
                        if(turtlePos == TurtlePos.BOTTOM_LEFT)
                        {
                            nextState.pos = curState.pos + ((q * Vector3.down) * curState.width);
                        }

                        Debug.Log(q.eulerAngles);
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.forward * angle));
                        Debug.Log(q.eulerAngles);
                        //nextState.RotateAxisAngle(Vector3.forward, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], angleDelta);
                        break;
                    }
                case '-':
                    {
                        float angle = angleDelta;
                        if (m.parameters.Count >= 1)
                            angle = m.parameters[0];
                        //nextState.RotateAxisAngle(Vector3.forward, -angleDelta);

                        //some nastiness our rotation is right but the next step will be weird so fake it!
                        if(turtlePos == TurtlePos.BOTTOM_LEFT)
                        {
                            nextState.pos = curState.pos + ((q * offset) * curState.width); 
                        }

                        q = Quaternion.Euler(q.eulerAngles + (Vector3.forward * -angle));
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], -angleDelta);
                        break;
                    }
                case '&':
                    {
                        float angle = angleDelta;
                        if (m.parameters.Count >= 1)
                            angle = m.parameters[0];

                        q = Quaternion.Euler(q.eulerAngles + (Vector3.right * angle));
                        //nextState.RotateAxisAngle(Vector3.right, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[0], angleDelta);
                        break;
                    }
                case '^':
                    {
                        float angle = angleDelta;
                        if (m.parameters.Count >= 1)
                            angle = m.parameters[0];

                        q = Quaternion.Euler(q.eulerAngles + (Vector3.right * -angle));
                        //nextState.RotateAxisAngle(Vector3.right, -angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[0], -angleDelta);
                        break;
                    }
                case '\\':
                    {
                        float angle = angleDelta;
                        if (m.parameters.Count >= 1)
                            angle = m.parameters[0];

                        q = Quaternion.Euler(q.eulerAngles + (Vector3.up * angle));
                        //nextState.RotateAxisAngle(Vector3.up, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[1], angleDelta);
                        break;
                    }
                case '/':
                    {
                        float angle = angleDelta;
                        if (m.parameters.Count >= 1)
                            angle = m.parameters[0];

                        q = Quaternion.Euler(q.eulerAngles + (Vector3.up * -angle));
                        //nextState.RotateAxisAngle(Vector3.up, angleDelta);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[1], -angleDelta);
                        break;
                    }
                case '|':
                    {                        
                        q = Quaternion.Euler(q.eulerAngles + (Vector3.forward * 180.0f));
                        Debug.Log(q.eulerAngles);
                        //nextState.RotateAxisAngle(Vector3.forward, 180.0f);
                        //rotMatrix *= XMMatrixRotationAxis(rotMatrix.r[2], (180.0f * XM_PI) / 180);
                        break;
                    }
                case '[':
                    {
                        curState.rot = q;
                        
                        turtleStack.Push(new TurtleState(curState));
                        indexStack.Push(index);
                        break;
                    }
                case ']':
                    {
                        nextState = new TurtleState(turtleStack.Pop());
                        q = nextState.rot;
                        break;
                    }
                default:
                    break;
            }
            curState = new TurtleState(nextState);
           
        }
        mesh.SetVertices(verts);
        //mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        
        //mesh.SetTriangles(indices, 0);
        mesh.name = "Tree";
        //mesh.RecalculateNormals();
        return mesh;
    }
}
