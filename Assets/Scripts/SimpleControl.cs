using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour
{
    private bool exited;
    // Start is called before the first frame update
    void Start()
    {
        exited = false;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Vertical") && !exited)
            transform.position += new Vector3(0f, 0.2f, 0f) * Time.deltaTime;


    }

    private void OnTriggerExit(Collider collision)
    {
        exited = true;
        Debug.Log("Collided!");
        
    }
}
