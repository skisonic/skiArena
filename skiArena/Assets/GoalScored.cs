using UnityEngine;
using System.Collections;

public class GoalScored : MonoBehaviour {

    GameObject scoreboard_1;
    GameObject player1, player2;
	// Use this for initialization
	void Start () {
        scoreboard_1 = GameObject.Find("Scoreboard1");
        //player1 = GameObject.Find("Player1");
        //player2 = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update () {
	}

    public void goalScored (int player){
        if(player == 1){
            scoreboard_1.GetComponent<TextMesh>().text = "1";
        }
    }
}
