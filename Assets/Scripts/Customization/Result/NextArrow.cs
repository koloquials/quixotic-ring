using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextArrow : MonoBehaviour {

    public GameObject currentScreen;
    public GameObject nextScreen;
    public ResultManager resultManager;

    void OnMouseDown(){
        nextScreen.SetActive(true);
        resultManager.statsSet = false;
        currentScreen.SetActive(false);
    }
}
