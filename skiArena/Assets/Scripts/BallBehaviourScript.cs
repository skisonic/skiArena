using UnityEngine;
using System.Collections;

public class BallBehaviourScript : MonoBehaviour {
    public Rigidbody rb;
    public bool isCaught_P1, wasShot_P1, isCaught;
    GameObject player1, player2, ball, gameManager;
    private SphereCollider ballColl;
    public bool debugging_movement;
    public float debugging_offset;

    Vector3 startPosition;
    public GameObject playerPoss;
    GameObject notifyboard;
    Renderer rend;
    public int player_holding;

    // attach a to b
    // make sure its exactly on it

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        ballColl = GetComponent<SphereCollider>();
        ball = GameObject.Find("Ball");
        gameManager = GameObject.Find("GameManager");
        wasShot_P1 = false;
        startPosition = GetComponent<Transform>().position;

        notifyboard = GameObject.Find("Notifyboard");
        rend = GetComponent<Renderer>();
        player_holding = 0;


        //declaring a dummy game object to hold a player 
        playerPoss = new GameObject();
        Rigidbody playerPoss_RB = playerPoss.gameObject.AddComponent<Rigidbody>();
        PlayerControl playerPoss_PC = playerPoss.gameObject.AddComponent<PlayerControl>();

        
       
        //MY DEBUGS MY PRECIOS PRECIOUS
        debugging_movement = true;
        debugging_offset = 0.5f;
    }

    void FixedUpdate()
    {
        //        if (isCaught_P1)
        //if (!playerPoss.GetComponent<PlayerControl>().isCatching)
        if (!playerPoss.GetComponent<PlayerControl>().isCatching == true)
        {
                isCaught = false;
                playerPoss.GetComponent<PlayerPhysics>().SetHasBall(false);
        }

        if (isCaught) //move the ball as if connected
        {
            if (debugging_movement)
            {
                newCaughtMovement();
            }
            else
            { 
                caughtMovement();
            }
        }
        else
        {
            this.GetComponent<Transform>().parent = null;

        }
        //        else if ((!ballColl.enabled) && ((!isCaught_P1)))

        /*
        else if ((!ballColl.enabled) && ((!isCaught)) )
        {
            //detach everything if collider is disabled and not caught, meaniing someone let go
            this.transform.parent = null;
            rb.isKinematic = false;
            ballColl.enabled = true;
            Debug.Log("detaching 1 ");
        }
        */
        //        if (!isCaught_P1)
        if (!isCaught)
        {
            if (debugging_movement)
            {
                Physics.IgnoreCollision(player1.GetComponent<Collider>(), GetComponent<Collider>(), false);
                Physics.IgnoreCollision(player2.GetComponent<Collider>(), GetComponent<Collider>(), false);
                
                //Physics.IgnoreCollision(player1.GetComponent<Collider>(), GetComponent<Collider>(), false);
                
                //Physics.IgnoreCollision(playerPoss.GetComponent<Collider>(), GetComponent<Collider>(), false);
                this.GetComponent<Transform>().parent = null;
                //Debug.Log("detaching 2 ");
            }
        }
        if (wasShot_P1) // "shoot" the ball if it was held and released
        {
            //rb.velocity = player1.GetComponent<Rigidbody>().velocity;
            rb.velocity = Vector3.zero;
            rb.AddForce(player1.GetComponent<Rigidbody>().velocity.x ,0, player1.GetComponent<Rigidbody>().velocity.z); //tr
            wasShot_P1 = false;
            Debug.Log("yea ");
            // change this to a function that does same thing 
            isCaught = false; //tell ball its caught to move it.
        }
    }


    //Scoring
    void OnCollisionEnter(Collision thisCollision) // scoring, put this in separate script maybe
    {
        if (thisCollision.collider.name == "Basket2")
        {   //Debug.Log("score P1");
            //gameManager.GetComponent<GoalScored>().goalScored(1);
            //tell gamemanger to score p1
            //or hould i just tell the player?
            Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.Find("Basket2").GetComponent<Collider>());
            rend.enabled = false;
            isCaught = false;
            player1.GetComponent<PlayerScore>().addPoint();

            //rend.material.shader = Shader.Find("Specular");
            //rend.material.SetColor("_SpecColor", Color.red);

            //StartCoroutine("PlayerScored");
            //InvokePlayerScored();
        }
        else if (thisCollision.collider.name == "Basket1")
        {   //Debug.Log("score P2");
            //next line stops multiple scoring
            //Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.Find("Goal1").GetComponent<Collider>());
            Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.Find("Basket1").GetComponent<Collider>());
            rend.enabled = false;
            isCaught = false;
            player2.GetComponent<PlayerScore>().addPoint();

            //notifyboard.GetComponent<TextMesh>().text = player2.name + " Scored!";
            //StartCoroutine("PlayerScored");
        }
        if(thisCollision.collider.tag == "Player") // play clang noise
        {
            //Debug.Log("found a player");
            GameObject playerColl = thisCollision.gameObject;
            //playter catches the ball section
            if (playerColl.GetComponent<PlayerControl>().isCatching) // if he's catching attach
            {
                if (!isCaught) { //make sure not already caught
                //Debug.Log("I'm caught! " + playerColl.ToString());
                //ball.GetComponent<BallBehaviourScript>().isCaught_P1 = true; //tell ball its caught to move it.
                playerPoss = playerColl; //tell ball its caught to move it.
                isCaught = true; //tell ball its caught to move it.
                playerPoss.GetComponent<PlayerPhysics>().SetHasBall(true);
                }       
            }

            else if(!(playerColl.GetComponent<PlayerControl>().isCatching)) //clang if he wasnt catching
            {
                //set the volume of the sound depending on how fast the collision was
                GetComponent<AudioSource>().volume = Mathf.Clamp01((thisCollision.relativeVelocity.magnitude * 5) * 0.0005f);

                //play the attached default audio clip (set in the AudioSource component)
                GetComponent<AudioSource>().pitch = 1.0f;
                GetComponent<AudioSource>().pitch += Random.Range(-0.05F, 0.05F);
                GetComponent<AudioSource>().Play();
            }

        }
    }

    


    // Update is called once per frame
    void Update () {
    }


    void newCaughtMovement() //turn off collisions between ball and player, and move it with the player to attach
    {
        //        Physics.IgnoreCollision(player1.GetComponent<SphereCollider>(), GetComponent<SphereCollider>());
        Physics.IgnoreCollision(playerPoss.GetComponent<SphereCollider>(), GetComponent<SphereCollider>());

        GameObject player1, player2;
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        Physics.IgnoreCollision(player1.GetComponent<Collider>(), GetComponent<Collider>(), true);
        Physics.IgnoreCollision(player2.GetComponent<Collider>(), GetComponent<Collider>(), true);



        //attach 
        //this.GetComponent<Transform>().parent = player1.transform;
        //this.GetComponent<Transform>().parent = playerPoss.transform;
        if (playerPoss == player1)
        {
            //this.GetComponent<Transform>().localPosition = Vector3.zero + player1.GetComponent<Rigidbody>().velocity.normalized.Scale(new Vector3(0.5f, 0.5f, 0.5f));

            //attach the ball to the player and reposition it in the direction the player is moving.
            //this.GetComponent<Transform>().localPosition =  player.transform.position  + playerRB.velocity.normalized * (playerRadius + ballRadius))

            //this.GetComponent<Transform>().localPosition = Vector3.zero + player1.GetComponent<Rigidbody>().velocity.normalized + new Vector3(0.5f, 0.0f, 0.5f);

            //instead of placing it where im headed, place it where i steer
            //just used the magnitude as the offset and it worked BEAUTIFULLY
            Vector3 followDistance = new Vector3(0.01f, 0f, 0.01f);
            //this.GetComponent<Transform>().localPosition = Vector3.zero + player1.GetComponent<PlayerControl>().dirFacing;

            //without being attached as child 
            this.GetComponent<Transform>().position = player1.GetComponent<Transform>().position + player1.GetComponent<PlayerControl>().dirFacing;

            //transform.localPosition = Vector3.Scale(transform.localPosition, new Vector3(0.75f, 0.0f, 0.75f));


            //reposition ball according to player but it leave it facing the camera 
            //this.GetComponent<Transform>().localPosition = Vector3.zero + new Vector3(0.0f, 0.0f, -0.5f);




        }
        else if (playerPoss == player2)
        {
            this.GetComponent<Transform>().localPosition = Vector3.zero + new Vector3(-0.5f, 0.0f, 0.5f);
        }
        this.GetComponent<Transform>().localRotation = Quaternion.identity;
        if (!player1.GetComponent<PlayerControl>().crazyFlag)
        {
            rb.velocity = Vector3.zero;
        }


        //playerPoss.GetComponent<Transform>().rotation = Quaternion.identity;
        //this.GetComponent<Transform>().localPosition += new Vector3(0.5f, 0, 0.5f); //offset so as not to fall through floor
    }

    void caughtMovement()
    {
        // Disable collisions with the object being attached
        /*
        if (ballColl)
        {
            ballColl.enabled = false;
        }

        // Don't allow physics to affect the object
        if (rb)
        {
            rb.isKinematic = true;
        }

        // Attach object 1 to object 2
        this.transform.parent = player1.transform;

        // Center object 1 on object 2 (no offset)
        this.transform.localPosition = Vector3.zero;
        Debug.Log("caught movement");
        */
    }

    public void ResetBall() //reset the ball after agoal is scored, shoot it out or something.
    {
        rb.position = startPosition; //move it back to its tart position 
        rb.velocity = new Vector3(0, 1.0f, 0);

        Physics.IgnoreCollision(player1.GetComponent<Collider>(), GetComponent<Collider>(), false);
        Physics.IgnoreCollision(player2.GetComponent<Collider>(), GetComponent<Collider>(), false);

        //reset its possessions to none
        ball.GetComponent<BallBehaviourScript>().player_holding = 0;
        playerPoss = null;

        Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.Find("Basket2").GetComponent<Collider>(), false);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), GameObject.Find("Basket1").GetComponent<Collider>(), false);
        rend.enabled = true;
    }

    public void Reset() //function for resetting the game
    {
        rb.position = startPosition;
        rb.velocity = Vector3.zero;

        Physics.IgnoreCollision(player1.GetComponent<Collider>(), GetComponent<Collider>(), false);
        Physics.IgnoreCollision(player2.GetComponent<Collider>(), GetComponent<Collider>(), false);

    }

}