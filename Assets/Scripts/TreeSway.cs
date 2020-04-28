using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    public Animator animator;

    int timer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if (timer > 250)
        {
            timer = 0;
            int RandomNum = Random.Range(1, 3);
            animator.SetInteger("swayChoice", RandomNum);
        }
    }
}
