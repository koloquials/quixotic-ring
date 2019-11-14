using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmArrow : MonoBehaviour {

	void OnMouseDown(){
        AudioManager.instance.PlaySound("next");
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}
