using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LSystem lSys = new LSystem("F");
        lSys.AddRule('F', "FF");

        Debug.Log(lSys.RunSystem(4));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
