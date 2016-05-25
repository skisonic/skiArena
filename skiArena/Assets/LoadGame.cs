using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {


    public GameObject loadingImage;


    void Update()
    {
        // P1 controls
        if ((Input.GetKey("joystick 1 button 10")) || (Input.GetKey("joystick 1 button 5")))
        {
            //Debug.Log("player 1 catching! ");
            LoadScene();
        }
    }
    public void LoadScene()
    {
        //loadingImage.SetActive(true);
        Application.LoadLevel(1);
    }
}
