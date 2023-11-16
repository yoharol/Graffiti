using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetZFromY : MonoBehaviour
{
    void alignZFromY()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.01f);
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        alignZFromY();
    }

    private void Update()
    {
        alignZFromY();
    }


    // Update is called once per frame
}
