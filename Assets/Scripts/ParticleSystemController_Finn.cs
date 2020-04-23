using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController_Finn : MonoBehaviour
{
    public ParticleSystem WalkParticleSystem;
    public ParticleSystem FeatherParticleSystem;

    public ParticleSystem RightFootStep;
    public ParticleSystem LeftFootStep;

    public AudioSource aS;

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
        aS.pitch = Random.Range(0.75f, 1);
        aS.Play();
        Debug.Log("footstep");
    }
    public void RightFootStepParticle()
    {
        aS.pitch = Random.Range(0.75f, 1);
        aS.Play();
        RightFootStep.Emit(1);
    }
}
