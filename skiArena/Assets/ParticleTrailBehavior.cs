using UnityEngine;
using System.Collections;

public class ParticleTrailBehavior : MonoBehaviour {

    GameObject player1, player2, ball, gameManager;

    // Use this for initialization
    void Start () {
        player1 = GameObject.Find("Player1");
    }

    void FixedUpdate()
    {
        transform.position = player1.GetComponent<Transform>().position;
        transform.position.Set(transform.position.x, transform.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
