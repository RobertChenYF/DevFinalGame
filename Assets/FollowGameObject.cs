﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public GameObject followedGameObject;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - followedGameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followedGameObject.transform.position + offset;
    }
}
