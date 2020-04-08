using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireAnimatorController : MonoBehaviour
{
    public Animator animator;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", ClaireController.ClaireSpeed);
        if (ClaireController.ClaireSpeed>0 && ClaireController.ClaireSpeed < 0.5f)
        {
            animator.speed = (ClaireController.ClaireSpeed/0.3f);
        }
        else if (ClaireController.ClaireSpeed >= 0.5f)
        {
            animator.speed = 0.8f + (ClaireController.ClaireSpeed-0.5f)/0.6f;
        }
        else
        {
            animator.speed = 1;
        }
    }
}
