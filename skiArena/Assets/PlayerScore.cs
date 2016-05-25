using UnityEngine;
using System.Collections;

//Updates scoreboard and end game. does not handle players or ball
public class PlayerScore : MonoBehaviour {

    int score;
    GameObject scoreboard_1, scoreboard_2;
    GameObject player1, player2;
    GameObject notifyboard;
    GameObject ball;
    GameObject skybox;
    Rigidbody ball_rb;
    Renderer ball_rend;
    GameObject goal1, goal2;



    int POINTS_TO_WIN = 3;
    // Use this for initialization
    void Start () {
        scoreboard_1 = GameObject.Find("Scoreboard1");
        score = 0;
        scoreboard_1.GetComponent<TextMesh>().text = score.ToString();

        scoreboard_2 = GameObject.Find("Scoreboard2");
        score = 0;
        scoreboard_2.GetComponent<TextMesh>().text = score.ToString();

        notifyboard = GameObject.Find("Notifyboard");
        ball = GameObject.Find("Ball");
        ball_rb = ball.GetComponent<Rigidbody>();
        ball_rend = ball.GetComponent<Renderer>();

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        skybox = GameObject.Find("Skybox_B");
        goal1 = GameObject.Find("Goal1");
        goal2 = GameObject.Find("Goal2");
    }


    void endParticles()
    {

    }
    public void addPoint()
    {

        if (this.name == "Player1")
        {
            score++;
            scoreboard_1.GetComponent<TextMesh>().text = score.ToString();

            notifyboard.GetComponent<TextMesh>().text = player1.name + " Scored!";
            ParticleSystem particlesP1 = player1.GetComponentInChildren<ParticleSystem>();
            particlesP1.startSize += 50.0f;
            particlesP1.enableEmission = true;
            particlesP1.Play();
            skybox.GetComponent<RotateSkybox>().rotatingSpeed = 50.0f;

            InvokePlayerScored();
        }
        else if (this.name == "Player2")
        {
            score++;
            scoreboard_2.GetComponent<TextMesh>().text = score.ToString();

            notifyboard.GetComponent<TextMesh>().text = player2.name + " Scored!";
            ParticleSystem particlesP2 = player2.GetComponentInChildren<ParticleSystem>();
            particlesP2.enableEmission = true;
            particlesP2.Play();

            skybox.GetComponent<RotateSkybox>().rotatingSpeed = -50.0f;

            InvokePlayerScored();
        }

       
    }

    //play some scoring effects
    void InvokePlayerScored()
    {
        //say who scored
        //pause for 2 seconds
        ball.GetComponent<Transform>().position += new Vector3(0, -1000f, 0); //jiust get it otu the way

        //Play some victory noises
        AudioSource chimeSource;
        AudioSource clapSource;

        AudioSource[] allMyAudioSources = ball.GetComponents<AudioSource>();
        chimeSource = allMyAudioSources[1];
        clapSource = allMyAudioSources[2];
        chimeSource.Play();
        //clapSource.Play();

        //wait 2s then do these)

        Invoke("InvokeFinishScoring", 3.5f);

        // if this works give it some velocity
    }

    //turn the ball back on and clear the board
    void InvokeFinishScoring()
    {
        ball.GetComponent<BallBehaviourScript>().ResetBall();
        notifyboard.GetComponent<TextMesh>().text = "";


        // THIS MEANS A WINNER 
        if (score >= POINTS_TO_WIN)
        {
            AudioSource[] allMyAudioSources = ball.GetComponents<AudioSource>();
            AudioSource clapSource;

            clapSource = allMyAudioSources[2];
            clapSource.Play();
            if (this.name == "Player1")
            {
                //Renderer p2rend = player2.GetComponent<Renderer>();
                //p2rend.enabled = false;
                //do a cool shrink instead of just disappearing the balls
                player2.GetComponent<PlayerControl>().ShrinkPlayer();
                notifyboard.GetComponent<TextMesh>().text = "Player 1 Wins!";
                celebrateVictory();
            }
            else if (this.name == "Player2")
            {
                player1.GetComponent<PlayerControl>().ShrinkPlayer();
                notifyboard.GetComponent<TextMesh>().text = "Player 2 Wins!";
                celebrateVictory();
            }
            else
            {
                skybox.GetComponent<RotateSkybox>().rotatingSpeed = 1.0f;
            }
        }
    }


    public int ReturnScore()
    {
        return score;
    }

    public void Reset() //reset scores
    {
        score = 0;
        scoreboard_1.GetComponent<TextMesh>().text = score.ToString();
        scoreboard_2.GetComponent<TextMesh>().text = score.ToString();
        notifyboard.GetComponent<TextMesh>().text = "";
    }

    void ClearNotifyBoard() //clear the board
    {
        notifyboard.GetComponent<TextMesh>().text = "";
    }

    void celebrateVictory()
    {
        // lets try tinting the bowl in regions or waves
        GameObject bowl = GameObject.Find("Bowl");
        Rigidbody bowlRB = bowl.GetComponent<Rigidbody>();
        Renderer rend = bowl.GetComponent<Renderer>();

        Light[] vLights = GameObject.Find("LightManager").GetComponentsInChildren<Light>();
        if (this.name == "Player1") {
            vLights[0].enabled = true;
            goal2.GetComponent<Renderer>().sharedMaterial = player1.GetComponent<Renderer>().sharedMaterial;
            goal1.GetComponent<Renderer>().sharedMaterial = player1.GetComponent<Renderer>().sharedMaterial;
        }
        else if (this.name == "Player2")
        {
            vLights[1].enabled = true;
            goal2.GetComponent<Renderer>().sharedMaterial = player2.GetComponent<Renderer>().sharedMaterial;
            goal1.GetComponent<Renderer>().sharedMaterial = player2.GetComponent<Renderer>().sharedMaterial;
        }
        //rend.material.shader = Shader.Find("Specular");
        //rend.material.SetColor("_SpecColor", Color.red);

    }


    // Update is called once per frame
    void Update () {
	
	}

}
