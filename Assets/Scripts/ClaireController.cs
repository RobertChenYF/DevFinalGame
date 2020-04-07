using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireController : MonoBehaviour
{
    public static ClaireController me;

    [Header("Fundamnetal Components")]
    //CharacterController charCont;
    Rigidbody rb;
    CapsuleCollider capsCol;
    public Camera cam;
    public GameObject camDir;
    public GameObject model;

    [Header("Inputs")]
    public float horizontalInput;
    public float verticalInput;
    public bool mainInput;
    public bool interactInput;
    public bool jumpInput;
    public bool glideInput;

    [Header("Important Charecter Bools")]
    public bool onGround;

    [Header("Important Charecter Ints")]
    public int goldenFeathers;

    [Header("General Movement")]    
    public float moveSpeed;
    public Vector3 velocity;
    public Vector3 floorOffset;

    public float rotateSpeed;

    [Header("AirMovement - Generic Stuff")]
    public bool initJump;
    public float verticalSpeed;
    public float jumpSpeed;

    [Header("AirMovement - MoveSpeed")]
    public float glideMoveSpeed;
    public float baseGlideMS;
    public float extraGlideMS;
    public float additiveGlide;
    public float subtractiveGlide;
    public float extraGlideMSMax;

    [Header("AirMovement - Gravity")]
    public float gravity;
    public float baseGravity;
    public float glideGravity;
    public float baseGlideGrav;
    public float extraGlideGrav;

    [Header("AirMovement - Terminal Velocity")]
    public float terminalVelocity;
    public float baseTerminalVelocity;
    public float glideTerminalVelocity;
    public float baseGlideTV;
    public float extraGlideTV;

    [Header("RayCast")]
    public float rayDistance;

    [Header("TimersAndTheirLimits")]
    public int jumpTimer;
    public int jumpLimit;
    public int initTimer;
    public int timeToGlideTimer;
    public int timeToGlideLimit;

    void Awake()
    {
        me = this;
    }

    void Start()
    {
        //charCont = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        capsCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCameraDir();
        Inputs();

        //rotate character model
        // transform.rotation = Quaternion.Euler(0f,camDir.transform.rotation.eulerAngles.y,0f);
        if (horizontalInput!=0 || verticalInput != 0)
        {
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation,newRotation,rotateSpeed*Time.deltaTime);
        }
        
    }

    void GetCameraDir()
    {
        camDir.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
    }

    void Inputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            mainInput = true;
        }

        if(Input.GetButtonUp("Jump"))
        {
            mainInput = false;
        }
    }

    void FixedUpdate()
    {
        if (mainInput)
        {
            Debug.Log("Checking Main Input");
            CheckMainInput();
        }
        else
        {
            Debug.Log("Setting All Jump Stuff to No");
            jumpInput = false;
            interactInput = false;
            glideInput = false;
            jumpTimer = 0;
            initTimer = 0;
            timeToGlideTimer = 0;
        }

        if (onGround)
        {
            goldenFeathers = 1;
        }

        if(jumpInput)
        {
            Debug.Log("Trying to Jump");
            JumpInput();
        }
        else
        {
            initJump = false;
        }

        if(glideInput)
        {
            Debug.Log("Trying to Glide");
            GlideInput();
        }
        else
        {
            gravity = baseGravity;
            terminalVelocity = baseTerminalVelocity;
        }

        VerticalMovement();
        Velocity();
        ModelRotation();
    }

    void CheckMainInput()
    {
        jumpTimer++;
        initTimer++;
        timeToGlideTimer++;
        //if there is an interact prompt
        //{
        //    interactInput = true;
        //}
        //else
        if (jumpTimer < jumpLimit && goldenFeathers > 0)
        {
            Debug.Log("Trying to Jump");
            jumpInput = true;
            if (initTimer == 1)
            {
                goldenFeathers -= 1;
            }
        }
        else
        {
            jumpInput = false;
        }

        if(timeToGlideTimer > timeToGlideLimit)
        {
            glideInput = true;
        }
        else
        {
            glideInput = false;
        }
    }

    void JumpInput()
    {
        initJump = true;
    }

    void GlideInput()
    {
        float totalInput = Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput);
        if(totalInput > 1)
        {
            totalInput = 1;
        }
        else if(totalInput < 0.1)
        {

            totalInput = 0.1f;
            extraGlideMS += additiveGlide;
        }
        extraGlideMS += subtractiveGlide;
        if (extraGlideMS > extraGlideMSMax)
        {
            extraGlideMS = extraGlideMSMax;
        }
        if(extraGlideMS < 0)
        {
            extraGlideMS = 0;
        }
        glideMoveSpeed = baseGlideMS + extraGlideMS;
        glideGravity = baseGravity + (extraGlideGrav/totalInput);
        glideTerminalVelocity = baseGlideTV + (extraGlideTV/totalInput);

        Debug.Log("GlideGravity = " + glideGravity);
        Debug.Log("GlideTerminalVelocity = " + glideTerminalVelocity);

        gravity = glideGravity;
        terminalVelocity = glideTerminalVelocity;        
    }

    void VerticalMovement()
    {  
        if (!initJump)
        {
            MainPhysics();
        }
        else
        {
            JumpPhysics();
        }
        
    }

    void MainPhysics()
    {
        Ray floorChecker = new Ray(transform.position + (transform.up * -0.65f), transform.up * -1);
        RaycastHit floorHit;
        Debug.DrawRay(floorChecker.origin, floorChecker.direction * rayDistance, Color.cyan);

        if (Physics.Raycast(floorChecker, out floorHit, rayDistance))
        {
            onGround = true;
            rb.MovePosition(floorHit.point + floorOffset);
            verticalSpeed = 0;
        }
        else
        {
            onGround = false;
            verticalSpeed += gravity;
            if (verticalSpeed < terminalVelocity)
            {
                verticalSpeed = terminalVelocity;
            }
            Debug.Log("Not on Ground");
        }
    }

    void JumpPhysics()
    {
        verticalSpeed = jumpSpeed;
    }

    void Velocity()
    {
        Vector3 targetVelocity = Vector3.zero;
        if (!glideInput)
        {
            targetVelocity = (moveSpeed * (horizontalInput * camDir.transform.right)) + (moveSpeed * (verticalInput * camDir.transform.forward));
        }
        else
        {
            targetVelocity = (glideMoveSpeed * (horizontalInput * camDir.transform.right)) + (glideMoveSpeed * (verticalInput * camDir.transform.forward));
        }
        velocity = Vector3.Lerp(velocity, targetVelocity, 0.25f);
        velocity.y = verticalSpeed;

        rb.MovePosition(transform.position + velocity);
    }

    void ModelRotation()
    {
     //   model.transform.LookAt(transform.position + velocity);
    }
}
