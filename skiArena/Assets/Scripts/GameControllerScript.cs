using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {

    GameObject ball;

	// Use this for initialization
	void Start () {
        ball = GameObject.Find("ball");
    }
	
	// Update is called once per frame
	void Update () {

        //Handle Inputs
        float rotation = Input.GetAxis("Horizontal");

        //GetComponent<>

        print(rotation);


    }
}
