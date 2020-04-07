using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    private Animator animator;
    private float BlinkTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BlinkTimer <= 0)
        {
            animator.SetTrigger("Close");
            BlinkTimer = Random.Range(3.0f,7.0f);
        }
        BlinkTimer -= Time.deltaTime;
    }
}
