using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireAnimatorController : MonoBehaviour
{//control all the visual aspest of the character;
    [SerializeField] private Animator animator;
    [SerializeField] private Color red;
    [SerializeField] private Color blue;
    [SerializeField] private Material CapeMaterial;
    private float colorLerpFloat = 0;
    [SerializeField] private float colorChangeRate;
    private bool changeToBlue = false;
    private bool changeToRed = false;
    public TrailRenderer glideTrail1;
    public TrailRenderer glideTrail2;
    public Shader ColorToon;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", ClaireController.ClaireSpeed);
        animator.SetBool("onGround", ClaireController.onGround);
        //change animation speed based on control
        if (ClaireController.ClaireSpeed > 0 && ClaireController.ClaireSpeed < 0.5f)
        {
            animator.speed = (ClaireController.ClaireSpeed / 0.3f);
        }
        else if (ClaireController.ClaireSpeed >= 0.5f)
        {
            animator.speed = 0.8f + (ClaireController.ClaireSpeed - 0.5f) / 0.6f;
        }
        else
        {
            animator.speed = 1;
        }

        //color lerp for the cape

        if (changeToBlue)
        {
            Color lerpedColor = Color.Lerp(red, blue, colorLerpFloat);
            CapeMaterial.SetColor("ColorOfCape", lerpedColor);
            colorLerpFloat += Time.deltaTime * colorChangeRate;
            if (colorLerpFloat >= 1)
            {
                changeToBlue = false;
                colorLerpFloat = 0;
            }
        }
        else if (changeToRed)
        {
            Color lerpedColor = Color.Lerp(blue, red, colorLerpFloat);
            CapeMaterial.SetColor("ColorOfCape", lerpedColor);
            colorLerpFloat += Time.deltaTime * colorChangeRate;
            if (colorLerpFloat >= 1)
            {
                changeToRed = false;
                colorLerpFloat = 0;
            }
        }
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");

    }

    public void Glide()
    {
        animator.SetBool("Glide", true);
        Debug.Log("glide");
    }
    public void StopGlide()
    {
        animator.SetBool("Glide", false);
        Debug.Log("glide");
    }


    public void ChangeToBlue()
    {
        if (CapeMaterial.GetColor("ColorOfCape") != blue)
        {
            changeToBlue = true;
        }
    }

    public void ChangeToRed()
    {
        if (CapeMaterial.GetColor("ColorOfCape") != red)
        {
            changeToRed = true;
        }

    }

    public void GlideTrail()
    {
        
        glideTrail1.emitting = true;
        glideTrail2.emitting = true;
        
    }

    public void GlideTrailOff()
    {
        glideTrail1.emitting = false;
        glideTrail2.emitting = false;

    }

    public void Climb()
    {
        animator.SetBool("Climb", true);
        Debug.Log("climb");
    }
    public void StopClimb()
    {
        animator.SetBool("Climb", false);
        Debug.Log("climb");
    }

}
