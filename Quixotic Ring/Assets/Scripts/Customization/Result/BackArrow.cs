using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackArrow : MonoBehaviour {

    public GameObject currentScreen;
    public GameObject lastScreen;

    void OnMouseDown(){
        AudioManager.instance.PlaySound("pageturn");
        lastScreen.SetActive(true);
        currentScreen.SetActive(false);
    }
}
