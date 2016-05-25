using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {

    GameObject ball;
    bool hasBall;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        ball = GameObject.Find("Ball");
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision thisCollision)
    {
        if ((thisCollision.collider.tag == "Player") && (this.name == "Player1")) // play clang noise
        {
            GameObject playerColl = thisCollision.gameObject;

            //set the volume of the sound depending on how fast the collision was
            GetComponent<AudioSource>().volume = Mathf.Clamp01((thisCollision.relativeVelocity.magnitude * 5) * 0.0005f * rb.mass);
            GetComponent<AudioSource>().pitch = Mathf.Clamp01((thisCollision.relativeVelocity.magnitude * 5) * 0.0005f * rb.mass);

            //play the attached default audio clip (set in the AudioSource component)
            GetComponent<AudioSource>().pitch = 1.0f;
            GetComponent<AudioSource>().pitch += Random.Range(-0.05F, 0.05F);
            GetComponent<AudioSource>().Play();

        }
    }


    public void SetHasBall(bool ball_bool)
    {
        hasBall = ball_bool;
    }

    public bool CheckHasBall()
    {
        return hasBall;
    }

    void addMomemtum()
    {
        //if any axis changes, add momemtum by adding the velocity in the other direction
        //if(rb.velocity.)
    }

    // Update is called once per frame
    void Update ()
    {

        //Debug.Log("player velocity is " + rb.velocity.ToString());


        if ((!this.GetComponent<PlayerControl>().isCatching) && (this == ball.GetComponent<BallBehaviourScript>().playerPoss)) ///release ball if button isn't held
        {
            ball.GetComponent<BallBehaviourScript>().isCaught_P1 = false;
            ball.GetComponent<BallBehaviourScript>().isCaught = false;
            //add force if we were holding it
            if (hasBall)
            {
                hasBall = false;
                ball.GetComponent<BallBehaviourScript>().wasShot_P1 = true;
            }
        }
    }
}
