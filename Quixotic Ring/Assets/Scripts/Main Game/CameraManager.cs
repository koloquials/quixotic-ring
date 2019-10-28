using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject player;       //Public variable to store a reference to the player game object
    //private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    void Start () {
        //offset = transform.position - player.transform.position;
        player = GameObject.Find("player");
        Vector3 currentPos = player.transform.position;
        currentPos.y = 0f;
        currentPos.z = -1.0f;
        transform.position = currentPos;
    }

    void LateUpdate () {
        player = GameObject.Find("player");
        //transform.position = player.transform.position + offset;
        //transform.position = player.transform.position;
        Vector3 currentPos = player.transform.position;
        currentPos.y = 0f;
        currentPos.z = -1.0f;
        transform.position = currentPos;
    }
}
