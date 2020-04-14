using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireController : MonoBehaviour
{
    public static ClaireController me;

    [Header("Fundamnetal Components")]
    Rigidbody rb;
    CapsuleCollider capsCol;
    public Camera cam;
    public GameObject camDir; //Object which helps determine direction of movement by factoring in direction of camera
    public GameObject model; //The model for the object, childed to the main object
    public GameObject horiMotionDir; //Points in the direction of horizontal motion. Similar to camDir in purpose. 

    [Header("Inputs")]
    public float horizontalInput; 
    public float verticalInput;
    public bool mainInput; //Main Input is for when A is pressed
    public bool interactInput; //If A is pressed for an interaction, this is turned on
    public static bool jumpInput; //If A is pressed to jump, this is turned on
    public bool climbInput; //If A is pressed to climb, this is turned on
    public bool glideInput; //If A is pressed to glide, this is turned on

    [Header("Important Charecter Bools")]
    public static bool onGround;
    public bool canClimb;

    [Header("Important Charecter Ints")]
    public float goldenFeathers;
    public int goldenFeathersMax;


    [Header("General Movement")]    
    public float moveSpeed; //This speed of movement
    public Vector3 velocity; //Velocity is added to transform.position every frame
    public Vector3 targetVelocity;
    public Vector3 floorOffset; //How far the player is placed off the floor

    [Header("Climbing Movement")]
    public bool climbing;
    public float climbSpeed;
    public float disFromWall; //Offset from wall
    public float goldenFeathersSub;  //Every frame Claire is climbing, this number is subtracted from goldenFeathers

    [Header("AirMovement - Generic Stuff")]
    public bool initJump; //IF this bool is true, regular physics is turned off and jump physics is turned on
    public float verticalSpeed; //The current vertical speed, the y component of velocity. Kept separate as vertical calculations are quite different to horizontal
    public float jumpSpeed; //The speed that vertical speed is set to when jumping

    [Header("AirMovement - MoveSpeed")]
    public float glideMoveSpeed; //The speed moved when gliding.
    public float baseGlideMS; //The lowest speed one can move when gliding
    public float extraGlideMS;  //The extra speed one can move when gliding, this is added to baseGlideMS to make glideMoveSpeed
    public float additiveGlide; //The number that is added to extraGlideMS when you are diving
    public float subtractiveGlide; //Subtracted from extraGlideMS every frame, to slowly reduce glideMoveSpeed
    public float extraGlideMSMax; //The max extraGlideMS can be.

    [Header("AirMovement - Gravity")]
    public float gravity; //if the player is in the air, gravity is subtracted from vertical speed every frame
    public float baseGravity; //The base gravity for when the player is not gliding
    public float glideGravity; //The gravity that is subtracted from vertical speed when gliding
    public float baseGlideGrav; //The minimum gravity that will act on the player when gliding
    public float extraGlideGrav; //The extra gliding gravity that will, added to baseGlideGrav and then set to glideGravity. extraGlideGrav goes up if the left stick is centered

    [Header("AirMovement - Terminal Velocity")]
    public float terminalVelocity; //The negative max verticalSpeed can be set to
    public float baseTerminalVelocity; //The base terminal velocity for when the player is not gliding
    public float glideTerminalVelocity; //The terminal velocity for when the player is gliding
    public float baseGlideTV; //The minimum terminal velocity for when the player is gliding
    public float extraGlideTV; //The extra terminal velocity, added to baseGlideTV and then set to glideTerminalVelocity. extraGlideTV goes up if the left stick is centered, allowing for diving

    [Header("RayCast")]
    public float floorRayDistance; //The distance that the ground checking rayCast goes.
    public float climbRayDistance; //The distance that the wall checking rayCast goes.
    public Ray floorChecker;
    public RaycastHit floorHit;
    public Ray climbChecker;
    public RaycastHit climbHit;


    [Header("TimersAndTheirLimits")]
    public int jumpTimer; //A timer that tracks when verticalSpeed should be set to jumpSpeed
    public int jumpLimit; //If A is held and jumpTimer is below limit, verticalSpeed is set to jumpSpeed. If jumpTimer is above jumpLimit, this is not true.
    public int initTimer; //A simple timer that decreases goldenFeathers by 1 if you jump
    public int timeToGlideTimer; //A timer that tracks when the player can start gliding
    public int timeToGlideLimit;//If timeToGlideTimer is bigger than limit, you can glide
    public int climbTimer; //A timer that tracks how long since the climbChecker raycast has been inactive
    public int climbLimit; //If climbTimer is smaller than this, the player can continue climbing. This allows players to climb over objects, as opposed to falling just as they crest.


    //variable added by Robert
    public float rotateSpeed;
    public static float ClaireSpeed;

    void Awake()
    {
        me = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsCol = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        SetDirs();
        Inputs();
    }

    void SetDirs() //Sets the orientation of a childed object to the same as the following camera, minus any vertical orientation. 
    {
        camDir.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        if (Mathf.Abs(velocity.x) > 0.025f || Mathf.Abs(velocity.z) > 0.025f)
        {
            Vector3 motionDir = new Vector3(velocity.x, 0, velocity.z);
            horiMotionDir.transform.LookAt(transform.position + motionDir);
        }
    }

    void Inputs() //All inputs are got here
    {
        horizontalInput = Input.GetAxis("Horizontal"); //Input for Left Stick Movement - Horizontal
        verticalInput = Input.GetAxis("Vertical"); //Input for Left Stick Movement - Vertical

        if(Input.GetButtonDown("Jump")) //Input for A button. Named main input as it can be talk, interact, pick up, jump, climb and glide. The correct outcome is determined later
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
        RayCasts();
        if (mainInput) //First we check main input, as it determines alot fo what comes next
        {
            CheckMainInput();
        }
        else //Set everything CheckMainInput could change to false or 0. These parts especially can be cleaned up.
        {
            jumpInput = false;
            interactInput = false;
            climbInput = false;
            glideInput = false;
            jumpTimer = 0;
            initTimer = 0;
            timeToGlideTimer = 0;
            gravity = baseGravity;
            terminalVelocity = baseTerminalVelocity;
        }


        if (jumpInput)
        {
            JumpInput();
        }
        else
        {
            initJump = false;
        }

        if (glideInput)
        {
            GlideInput();
        }
        else
        {
            gravity = baseGravity;
            terminalVelocity = baseTerminalVelocity;
        }

        if (onGround)
        {         
            if (goldenFeathers != goldenFeathersMax)
            {             
                goldenFeathers = goldenFeathersMax; //Currently sets goldenFeaters back to its max if youre on the ground. Can be adjusted to slowly increase goldenFeathers as your on the ground
            }
            glideInput = false;
            model.GetComponent<ClaireAnimatorController>().ChangeToRed(); // change cape color to red when on the ground
            extraGlideMS = 0;
        }

        VerticalMovement();
        Velocity();
        ModelRotation();
    }

    void RayCasts()
    {
        floorChecker = new Ray(transform.position + (transform.up * -0.65f), transform.up * -1); //This ray checks for the floor
        Debug.DrawRay(floorChecker.origin, floorChecker.direction * floorRayDistance, Color.cyan);
        climbChecker = new Ray(transform.position, horiMotionDir.transform.forward); //This ray checks for climbable walls
        Debug.DrawRay(climbChecker.origin, climbChecker.direction * climbRayDistance, Color.magenta);

        if (Physics.Raycast(floorChecker, out floorHit, floorRayDistance)) //If the floorChecker ray hits the floor
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (Physics.Raycast(climbChecker, out climbHit, climbRayDistance)) //If the climbChecker hits a wall
        {
            canClimb = true;
        }
        else
        {
            canClimb = false;
        }
    }

    void CheckMainInput() //Check Main Input is what determines how a press or hold of the A button should be treated. Given its multi-use functionality, this runs first to determine how to handle the input.
    {
        jumpTimer++;
        climbTimer++;
        initTimer++;
        timeToGlideTimer++;
        //if there is an interact prompt (talking, reading or picking up)
        //{
        //    interactInput = true;
        //}
        //else it is related to jumping, climbing and or gliding, so
        if (canClimb && goldenFeathers > 0.1f)
        {
            climbInput = true;
            climbTimer = 0;
            goldenFeathers -= goldenFeathersSub;
        }
        else
        {
            climbInput = false;
        }

        if (!canClimb && jumpTimer < jumpLimit && (goldenFeathers > 0 || onGround)) //Currently you need a goldenFeather to jump at all, but being close to the ground sets it back to Max. Will need to add a check for being on the ground, thus not requiring golden feather
        {
            jumpInput = true; //if jumpTimer is less than JumpLimit, and you either are on the ground or have golden feathers, the input is registed as a jump. Hence, jumpInput is true
            if (initTimer == 1)
            {
                if (goldenFeathers == 1)
                {
                    //run the function that change the color of the cape and cloth
                    model.GetComponent<ClaireAnimatorController>().ChangeToBlue();

                }
                goldenFeathers -= 1; //If you do jump, then on the first frame of the jump goldenFeathers is decreased by 1
                model.GetComponent<ClaireAnimatorController>().Jump();
            }

        }
        else
        {
            jumpInput = false;
        }

        if (!climbInput && timeToGlideTimer > timeToGlideLimit) //If you have been holding A long enough, timeToGlideTimer will be above timeToGlideLimit, thus you can glide
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
        float totalInput = Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput); //Creates a float that moves roughly between 0 and 1.5 depending on how the stick is being held
        if(totalInput > 1)
        {
            totalInput = 1; //TotalInput can be equal to 1 if you are holding the stick all the way in a single direction, and larger given diagonals. totalInput being more than 1 means you are moving pretty fully, and thus to keep the math simple, it is reduced back to 1 for all later calculations
        }
        else if(totalInput < 0.2)
        {
            totalInput = 0.2f; //Dividing by 0 is impossible, and dividing by close to 0 can be damaging. Keeping it 0.2 keeps the later math simple and controllabe
        }
        extraGlideMS += (additiveGlide / totalInput); //Little enough input is being registered for a dive to be triggered. Hence, additiveGlide is added to extraGlideMS
        extraGlideMS += subtractiveGlide; //extraGlideMS is subtracted from each frame by subtractiveGlide to slowly reduce the speed of the glide
        if (extraGlideMS > extraGlideMSMax)
        {
            extraGlideMS = extraGlideMSMax; //Stops extraGlideMS from getting too large
        }
        if(extraGlideMS < 0)
        {
            extraGlideMS = 0; //Stops extraGlideMS from getting too small
        }
        glideMoveSpeed = baseGlideMS + extraGlideMS; //glideMoveSpeed = the minimum glideMoveSpeed + extraGlideMoveSpeed, affected by whether the player has been gliding or not       
    }

    void VerticalMovement() //VerticalSpeed is determined separatly from the rest of velocity, and is calculated here
    {  
        if (!initJump && !(climbInput && climbTimer < climbLimit) && !glideInput) //If you are not jumping
        {
            Debug.Log("Main Physics is on");
            MainPhysics(); //Regular physics applies
        }
        else if(initJump)//If you are jumping
        {
            JumpPhysics(); //Jump physics applies
        }
        else if(climbTimer < climbLimit)
        {
            ClimbPhysics();
        }
        
    }

    void MainPhysics()
    {
        if (onGround) //If the floorChecker ray hits the floor
        {
            rb.MovePosition(floorHit.point + floorOffset); //Player position is set to a specific offset above the floor
            verticalSpeed = 0; //Vertical Speed is 0
        }
        else //If theres no floor
        {
            verticalSpeed += gravity; //verticalSpeed is minused from gravity
            if (verticalSpeed < terminalVelocity) 
            {
                verticalSpeed = terminalVelocity; //verticalSpeed is set to terminalVelocity if it is lower
            }
        }
    }

    void JumpPhysics()
    {
        verticalSpeed = jumpSpeed; 
    }

    void ClimbPhysics()
    {
        verticalSpeed = 0;
        Vector3 mainMotion = climbHit.point + (transform.up * 0.1f) + (horiMotionDir.transform.forward * -disFromWall);
        Vector3 extraMotion = horiMotionDir.transform.forward * 0.1f;
        rb.MovePosition(mainMotion + extraMotion);
    }

    void Velocity() //This is when velocity is finally determined and applied, once all inputs and thus player state is confirmed
    {
        targetVelocity = Vector3.zero;
        if (!glideInput)
        {
            
            targetVelocity = (moveSpeed * (horizontalInput * camDir.transform.right)) + (moveSpeed * (verticalInput * camDir.transform.forward)); //if not gliding, targetVelocity uses normal moveSpeed to determine horizontal velocity
        }
        else
        {
            targetVelocity = (glideMoveSpeed * (horizontalInput * camDir.transform.right)) + (glideMoveSpeed * (verticalInput * camDir.transform.forward)); //if gliding, targetVelocity uses normal moveSpeed to determine horizontal velocity
        }

        if (!glideInput)
        {
            velocity = Vector3.Lerp(velocity, targetVelocity, 0.25f); //velocity is lerped to targetVelocity to keep it smoother

            velocity.y = verticalSpeed; //verticalSpeed is set after the lerp and all other calculations to ensure it is not affected by other parts
        }
        else
        {
            velocity = Vector3.Lerp(velocity, targetVelocity, 0.03f); //Slower Lerp when gliding
            velocity.y = GlidePhyics();
        }
        rb.MovePosition(transform.position + velocity); //Finally, velocity is added to transform.position
    }

    float GlidePhyics()
    {
        float totalHoriVelocity = Mathf.Abs(velocity.x) + Mathf.Abs(velocity.z);
        Debug.Log(totalHoriVelocity);
        if(totalHoriVelocity < 0.05f)
        {
            totalHoriVelocity = 0.05f;
        }
        glideGravity = baseGravity + (extraGlideGrav / (7.5f * totalHoriVelocity)); //glideGravity = the minimum glideGravity + (extraGlideGrav dividied by the total Input. If totalInput is at its Max (1), extraGlideGrav is at its smallest. If totalInput is at its smallest (0.2), extraGlideGrav is at its biggest.
        glideTerminalVelocity = baseGlideTV + (extraGlideTV / (7.5f * totalHoriVelocity)); //glideTerminalVelocity = the minimum glideTerminalVelocity + (extraglideTerminalVelocity dividied by the total Input. If totalInput is at its Max (1), extraglideTerminalVelocity is at its smallest. If totalInput is at its smallest (0.2), extraglideTerminalVelocity is at its biggest.

        gravity = glideGravity;
        terminalVelocity = glideTerminalVelocity;

        verticalSpeed += gravity; //verticalSpeed is minused from gravity
        if (verticalSpeed < terminalVelocity)
        {
            verticalSpeed = terminalVelocity; //verticalSpeed is set to terminalVelocity if it is lower
        }

        return verticalSpeed;
    }

    void ModelRotation()
    {
        //   model.transform.LookAt(transform.position + velocity);
        //Debug.Log("Speed: " + Mathf.Sqrt(velocity.x*velocity.x + velocity.z*velocity.z));

        ClaireSpeed = Mathf.Sqrt(verticalInput * verticalInput + horizontalInput * horizontalInput);
        Quaternion newRotation;

        //rotate character model
        // transform.rotation = Quaternion.Euler(0f,camDir.transform.rotation.eulerAngles.y,0f);
        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (!glideInput)
            {
                newRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z));
            }
            else
            {
                newRotation = Quaternion.LookRotation(new Vector3(velocity.x, -1000, velocity.z));
            }
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
