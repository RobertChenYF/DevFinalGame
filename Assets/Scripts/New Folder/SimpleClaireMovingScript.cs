﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleClaireMovingScript : MonoBehaviour
{
    
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(-speed,0,0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(speed,0,0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0,-1,0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0,1,0));
        }
    }
    
}