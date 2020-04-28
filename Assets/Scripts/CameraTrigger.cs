using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private Animator CameraAnimator;
    public int triggerState;
    // Start is called before the first frame update
    void Start()
    {
       CameraAnimator =  GameObject.Find("playerBody").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Claire")
        {
            CameraAnimator.SetInteger("CameraState",triggerState );
        }
    }
}
