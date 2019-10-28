using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
    public GameObject screen;

    void OnMouseDown()
    {
        screen.SetActive(false);
    }
}
