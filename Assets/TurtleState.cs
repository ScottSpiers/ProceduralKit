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
    public Matrix4x4 rot { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    public void RotateAxisAngle(Vector3 axis, float a) // should this also have the axis? Or just pass what we should be changing as we can access it easily
    {
        //Update the rotation matrix
        if(axis != Vector3.up && axis != Vector3.forward && axis != Vector3.right)
        {
            return;
        }

        Vector3 rotation = rot.rotation.eulerAngles;
        rotation += axis * a;
        rot = Matrix4x4.Rotate(Quaternion.Euler(rotation));
    }
}
