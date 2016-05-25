using UnityEngine;
using System.Collections;

public class HingePlayerControl : MonoBehaviour
{
    public float thrustX, thrustY, thrustZ;
    public float myForceFactor;
    public Rigidbody rb;
    public bool isCatching;
    public float gravityY;
    public float brakeFactor = 0.97f;
    Vector3 brakeSpeed;
    Vector3 startPos, oldVelocity;
    public bool crazyFlag;


    //1 game must end
    //2 player 2 must move
    //3 must be able to move
    // make it so you can shoot



    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isCatching = false;
        gravityY = Physics.gravity.y;
        myForceFactor = 0.15f;
        startPos = rb.position;
        brakeSpeed = new Vector3(brakeFactor, brakeFactor, brakeFactor);
        //rend.material.shader = Shader.Find("Specular");
        //rend.material.SetColor("_SpecColor", Color.red);

        oldVelocity = rb.velocity;
        /*
        int i = 0;
        while (i < 4)
        {
            if (Mathf.Abs(Input.GetAxis("Joy" + i + "X")) > 0.2F || Mathf.Abs(Input.GetAxis("Joy" + i + "Y")) > 0.2F)
                Debug.Log(Input.GetJoystickNames()[i] + " is moved");

            i++;
        }
        */
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

    }

    //getting momemtum for a flick would be like, 
    //if get max, get min, get magnitude between multiply it by 1/deltatime 

    void myAddThrust()
    {
        rb.AddForce(thrustX * Mathf.Abs((gravityY / myForceFactor * rb.position.y)), 0, 0); //try three swings for edge);
        rb.AddForce(0, 0, thrustY * Mathf.Abs(gravityY / myForceFactor * rb.position.y)); //try three swings for edge);
    }

    // Update is called once per frame
    void Update()
    {
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


        // P1 controls
        if ((this.gameObject.name == "Player1") && ((Input.GetKey("joystick 1 button 10")) || (Input.GetKey("joystick 1 button 5"))))
        {
            isCatching = true;
            if (!GetComponent<PlayerPhysics>().CheckHasBall())
            {
                //make the catch button also the brake so you cant just roll around catching
                rb.velocity.Scale(brakeSpeed);

                //Debug.Log("player 1 braking! ");
            }
            //Debug.Log("player 1 catching! ");
        }
        else if ((this.gameObject.name == "Player2") && ((Input.GetKey("joystick 2 button 10")) || (Input.GetKey("joystick 2 button 5") || (Input.GetKey("joystick 1 button 0")))))
        {
            isCatching = true;
            if (!GetComponent<PlayerPhysics>().CheckHasBall())
            {
                //make the catch button also the brake so you cant just roll around catching
                rb.velocity.Scale(brakeSpeed);
                Debug.Log("player 2 braking! ");
            }
            //Debug.Log("player 2 catching! ");
        }
        else
        {
            isCatching = false;
            //Debug.Log("player 1 let go! ");
        }

        if ((this.name == "Player1") && (Input.GetKey("joystick 1 button 9")))
        {
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
            reset();
        }
        else if ((Input.GetKey("joystick 1 button 12")))
        {
            ResetPositionP1();
        }

        if ((Input.GetKey("joystick 2 button 12")))
        {
            ResetPositionP2();
        }
        oldVelocity = rb.velocity;

    }


    void reset()
    { //duh
        GameObject ball;

        ball = GameObject.Find("Ball");

        ball.GetComponent<BallBehaviourScript>().ResetBall();
        GetComponent<PlayerScore>().Reset();
        rb.position = startPos;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    void ResetPositionP1()
    {
        Rigidbody rb = GameObject.Find("Player1").GetComponent<Rigidbody>();
        rb.position = startPos;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    void ResetPositionP2()
    {
        Rigidbody rb = GameObject.Find("Player2").GetComponent<Rigidbody>();
        rb.position = startPos + new Vector3(7f, 0, 0);
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }
}
