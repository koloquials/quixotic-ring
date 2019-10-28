using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppraisalPick : MonoBehaviour {
    public int num = 0; //temporarily holds archetype id, doesn't confirm until exit is pressed.
    float cursorPos = 0;
    public PlayerInfo player;

    public GameObject cursor;

    void Update(){
        cursorPos = 2.45f - (1f * num);
        cursor.transform.position = new Vector2(cursor.transform.position.x, cursorPos);
    }

    void OnMouseDown(){
        //hardcoded for now, could use separate game objects hooked up to player info later.
        if (Input.mousePosition.y <= 830f && Input.mousePosition.y >= 740f){
            num = 0;
        } else if (Input.mousePosition.y <= 730f && Input.mousePosition.y >= 640f){
            num = 1;
        } else if (Input.mousePosition.y <= 630f && Input.mousePosition.y >= 540f){
            num = 2;
        } else if (Input.mousePosition.y <= 530f && Input.mousePosition.y >= 440f){
            num = 3;
        } else if (Input.mousePosition.y <= 430f && Input.mousePosition.y >= 340f){
            num = 4;
        } else if (Input.mousePosition.y <= 330f && Input.mousePosition.y >= 240f){
            num = 5;
        }
    }

    public void ProcessConfirmation(){
        player.archetype = num; //finally puts change in the player manager.
    }

}
