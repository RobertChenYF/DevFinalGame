﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem WalkParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WalkingParticle(int particleAmount)
    {
        if (WalkParticleSystem != null)
        {
        WalkParticleSystem.Emit(particleAmount);
        }
        
    }
}
