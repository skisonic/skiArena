using UnityEngine;
using System.Collections;


// match contrast of bowls
//feed back like glow to indicate controller
// feed back like ring to indicate direction of stick
// 
public class RotateSkybox : MonoBehaviour {

    public float rotatingSpeed;

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().Play();

        rotatingSpeed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.right * Time.deltaTime * 0.25f * rotatingSpeed, Space.World);
    }
}
