using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurtleState
{
    //The turtle's position
    public Vector3 pos { get; set; }
    //The amount the turtle will move in the next step
    public float stepSize { get; set; }
    //The width of the ine drawn by the Turtle
    public float width { get; set; }
    //The rotation of the turtle
    public Quaternion rot { get; set; }

    public TurtleState()
    {
        pos = Vector3.zero;
        stepSize = 1.0f;
        width = 1.0f;
        rot = Quaternion.identity;
    }

    public TurtleState(TurtleState t)
    {
        pos = new Vector3(t.pos.x, t.pos.y, t.pos.z);
        stepSize = t.stepSize;
        width = t.width;
        rot = Quaternion.Euler(t.rot.eulerAngles);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    public void RotateAxisAngle(Vector3 axis, float a) // should this also have the axis? Or just pass what we should be changing as we can access it easily
    {
        rot = Quaternion.Euler(rot.eulerAngles + (axis * a));
    }
}
