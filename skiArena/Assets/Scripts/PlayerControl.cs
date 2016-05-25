using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    public float thrustX, thrustY, thrustZ;
    public float startX, startY, startZ;
    public float maxX, maxY, maxZ;
    public float myForceFactor;
    public Rigidbody rb;
    public bool isCatching;
    public float gravityY;
    public float brakeFactor = 0.97f;
    Vector3 brakeSpeed;
    Vector3 startPos, oldVelocity;
    public Vector3 dirFacing;
    public bool crazyFlag;
    public float thrustTimeStart;
    GameObject ball;
    bool shrinkFlag;
    float spawnTimeP1, spawnTimeP2;
    bool respawningFlagP1, respawningFlagP2;
    GameObject player1, player2;
    AudioSource beamUpSource;


    //1 game must end
    //2 player 2 must move
    //3 must be able to move
    // make it so you can shoot
    //tutorial



    // Use this for initialization
    void Start () {
        ball = GameObject.Find("Ball");

        rb = GetComponent<Rigidbody>();
        isCatching = false;
        gravityY = Physics.gravity.y;
        myForceFactor = 0.15f;
        startPos = rb.position;
        brakeSpeed = new Vector3(brakeFactor, brakeFactor, brakeFactor);
        Renderer rend = GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("Specular");
        //rend.material.SetColor("_SpecColor", Color.red);

        oldVelocity = rb.velocity;
        shrinkFlag = false;
        spawnTimeP1 = spawnTimeP2 = 1;

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        beamUpSource = allMyAudioSources[1]; //gettijg the 2nd audio on the component after the clang

    }

    void FixedUpdate()
    {
        //rb.AddForce(transform.forward * thrust);

        if ((this.name == "Player1") && ((Input.GetAxis("P1_Horizontal") != 0) || (Input.GetAxis("P1_Vertical") != 0)))
        {
            myAddThrust();
        }
        else if ((this.name == "Player2") && ((Input.GetAxis("P2_Horizontal") != 0) || (Input.GetAxis("P2_Vertical") != 0)))
        {
            myAddThrust();
        }

        if ((this.name == "Player1") && ((Input.GetKey("joystick 1 button 5"))))
        {
            //Debug.Log("player 1 braking");
            rb.velocity.Scale(brakeSpeed);
            rb.velocity.Set(0, 0, 0);

        }

        if ((this.name == "Player2") && ((Input.GetKeyDown("joystick 2 button 5"))))
        {
            //Debug.Log("Player 2 braking");
            rb.velocity.Set(0, 0, 0);
        }

        if (shrinkFlag)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.94f, 0.94f, 0.94f));
        }


    }

    //getting momemtum for a flick would be like, 
    //if get max, get min, get magnitude between multiply it by 1/deltatime 


    //12-25-2015 new idea :: input is a small constat force plus flick that diminsihes over time by a fctor that scales it to infiitely small
    //(thrustXYZ * (much smaller than 20f) * .97)  + (flick) - first part should barely overtake gravity
    //flick = (distance * (1/time to travel))  an impulse that should also barely overtake gravity?
    //distance = magnitude(vector(endx,startx))
    //

    void myAddThrust()
    {
        //if (Time.frameCount - thrustTimeStart >= 7.0f) // lock you out for 7f to feel clunkyish
        {
            rb.AddForce(thrustX * 20f, 0, thrustY * 20f, ForceMode.Acceleration);
            //rb.AddForce(0,0, thrustY*100f); //try three swings for edge);
            thrustTimeStart = Time.frameCount;
            dirFacing = rb.velocity.normalized;
        }
    }

    /*
    BALL CONTROLS FROM BENNET 
    ballRB.MovePosition (
    player.transform.position
    + playerRB.velocity.normalized * (playerRadius + ballRadius))
    */

    // Update is called once per frame
    void Update () {

        
        //thrustX = Input.GetAxis("P1_Horizontal") * Mathf.Abs(oldVelocity.x + 1) ;
        if (this.name == "Player1")
        {
            thrustX = Input.GetAxis("P1_Horizontal");
            thrustY = Input.GetAxis("P1_Vertical");
        }
        else if (this.name == "Player2")
        {
            thrustX = Input.GetAxis("P2_Horizontal");
            thrustY = Input.GetAxis("P2_Vertical");
        }
        Physics.gravity.Set(0, gravityY, 0);


        //try moving the ball
        if (this.name == "Player1")
        {
            thrustX = Input.GetAxis("P1_Horizontal");
            thrustY = Input.GetAxis("P1_Vertical");
        }

        // P1 controls
        if ((this.gameObject.name == "Player1") && ((Input.GetKey("joystick 1 button 10")) || (Input.GetKey("joystick 1 button 5"))))
        {
            isCatching = true;
            ball.GetComponent<BallBehaviourScript>().player_holding = 1;
            if (!GetComponent<PlayerPhysics>().CheckHasBall()) 
            {
                //make the catch button also the brake so you cant just roll around catching
                rb.velocity.Scale(brakeSpeed);


            }
            else if ((this.gameObject.name == "Player1") && (Input.GetKey("joystick 1 button 5")) && ((Input.GetKey("joystick 1 button 7"))))
            {
                //SHOOT the ball sigh
                if (GetComponent<PlayerPhysics>().CheckHasBall())
                {
                    isCatching = false; //let go of ball
                    ball.GetComponent<BallBehaviourScript>().isCaught_P1 = false;
                    ball.GetComponent<BallBehaviourScript>().isCaught = false;
                    ball.GetComponent<BallBehaviourScript>().player_holding = 0;
                    ball.GetComponent<Rigidbody>().AddForce(dirFacing*10.0f + rb.velocity, ForceMode.Impulse);
                    Debug.Log("player 1 shooting! ");
                }
            }
            //Debug.Log("player 1 catching! ");
        }
        
        else if ((this.gameObject.name == "Player2") && ((Input.GetKey("joystick 2 button 10")) || 
                                                            (Input.GetKey("joystick 2 button 5")|| 
                                                            (Input.GetKey("joystick 1 button 0"))))) //NOTICE P1 SQ
        {
            isCatching = true;
            ball.GetComponent<BallBehaviourScript>().player_holding = 2;
            if (!GetComponent<PlayerPhysics>().CheckHasBall())
            {
                //make the catch button also the brake so you cant just roll around catching
                rb.velocity.Scale(brakeSpeed);
                Debug.Log("player 2 braking! ");
            }
            else if ((this.gameObject.name == "Player2") && (Input.GetKey("joystick 2 button 5")) && ((Input.GetKey("joystick 2 button 7"))))
            {
                //shoot the ball sigh
                if (GetComponent<PlayerPhysics>().CheckHasBall())
                {
                    isCatching = false; //let go of ball
                    ball.GetComponent<BallBehaviourScript>().isCaught = false;
                    ball.GetComponent<Rigidbody>().AddForce(rb.velocity/5.0f,ForceMode.Impulse);
                    Debug.Log("player 2 shooting! ");
                }
            }
            //Debug.Log("player 2 catching! ");
        }
        else
        {
            isCatching = false;
            //Debug.Log("player 1 let go! ");
            ball.GetComponent<BallBehaviourScript>().player_holding = 0;
        }

        if ((this.name == "Player1") && (Input.GetKey("joystick 1 button 9"))){
            if (!crazyFlag)
            {
                crazyFlag = true;
            }
            else
            {
                if (GetComponent<PlayerScore>().ReturnScore() < 3)
                crazyFlag = false;
            }
        }

        //reset shortcut

 

        if ((Input.GetKey("joystick 1 button 6")) && (Input.GetKey("joystick 1 button 7")))
        {
            Reset();
        } else if ((Input.GetKey("joystick 1 button 4")) && Input.GetKey("joystick 1 button 6") && !(respawningFlagP1))
        {
            respawningFlagP1 = true;
//            beamUpSource.pitch = beamUpSource.pitch * (spawnTimeP1 *0.05f );
            beamUpSource.Play();
            Renderer p1rend = player1.GetComponent<Renderer>();
            p1rend.enabled = false;
            Invoke("ResetPositionP1", spawnTimeP1);
        }

        if ((Input.GetKey("joystick 2 button 4")) && Input.GetKey("joystick 2 button 6"))
        {
            respawningFlagP2 = true;
            beamUpSource.Play();
            Renderer p2rend = player2.GetComponent<Renderer>();
            p2rend.enabled = false;
            Invoke("ResetPositionP2", spawnTimeP2);

            ResetPositionP2();
        }
        oldVelocity = rb.velocity;

    }


    void Reset() { //duh

        ball.GetComponent<BallBehaviourScript>().ResetBall();
        GetComponent<PlayerScore>().Reset();
        rb.position = startPos;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

   
    void ResetPositionP1()
    {
        Renderer p1rend = player1.GetComponent<Renderer>();
        p1rend.enabled = true;

        Rigidbody rb = GameObject.Find("Player1").GetComponent<Rigidbody>();
        rb.position = startPos;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;

        respawningFlagP1 = false;

        spawnTimeP1++;
    }

    void ResetPositionP2()
    {
        Rigidbody rb = GameObject.Find("Player2").GetComponent<Rigidbody>();
        rb.position = startPos + new Vector3(7f, 0, 0);
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    public void ShrinkPlayer()
    {
        shrinkFlag = true;
    }
}
