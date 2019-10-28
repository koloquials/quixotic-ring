using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public GameObject camera;

    // Use this for initialization
    void Start () {
        camera = GameObject.Find("camera");
    }
	
	void LateUpdate () {
        Vector3 currentPos = camera.transform.position;
        currentPos.y = 0f;
        currentPos.z = -0.3f;
        transform.position = currentPos;
    }
}
