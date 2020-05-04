using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideAudio : MonoBehaviour
{
    // Start is called before the first frame update

    public bool audioOn;
    public float audioVol;

    public AudioSource aS;

    public int delayTimer;
    public int delayLimit;

    void Start()
    {
        aS = GetComponent<AudioSource>();
        audioVol = aS.volume;
        aS.volume = 0;
    }

    // Update is called once per frame

    void Update()
    {
        audioOn = ClaireController.me.GlideAudioOn;
    }

    void FixedUpdate()
    {
        float totalHoriVelocity = Mathf.Abs(ClaireController.me.velocity.x) + Mathf.Abs(ClaireController.me.velocity.z);
       // Debug.Log(totalHoriVelocity);

        float trueAudio = 0;

        if (audioOn)
        {
            delayTimer++;
            if (delayTimer > delayLimit)
            {
                trueAudio = audioVol * totalHoriVelocity / 0.3f;
            }
        }

        else
        {
            if (delayTimer > 0)
            {
                delayTimer--;
            }
            trueAudio = 0;
        }

        if(ClaireController.onGround)
        {
            delayTimer = 0;
        }

        aS.volume = Mathf.Lerp(aS.volume, trueAudio, 0.0075f);

        if (aS.volume < 0.001 && trueAudio < 0.001)
        {
            aS.volume = 0;
        }
    }

}
