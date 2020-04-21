using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem WalkParticleSystem;
    public ParticleSystem FeatherParticleSystem;

    public ParticleSystem RightFootStep;
    public ParticleSystem LeftFootStep;
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


    public void FeatherParticle()
    {
        if (FeatherParticleSystem != null)
        {
            FeatherParticleSystem.Emit(1);
        }

    }


    public void LeftFootStepParticle()
    {
        LeftFootStep.Emit(1);
        Debug.Log("footstep");
    }
    public void RightFootStepParticle()
    {
        RightFootStep.Emit(1);
    }
}
