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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    public void RotateAxisAngle(Vector3 axis, float a) // should this also have the axis? Or just pass what we should be changing as we can access it easily
    {
        rot = Quaternion.Euler(rot.eulerAngles + (axis * a));
    }
}
