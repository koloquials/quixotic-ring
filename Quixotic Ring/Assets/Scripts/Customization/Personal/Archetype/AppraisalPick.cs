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

    public void ProcessConfirmation(){
        player.archetype = num; //finally puts change in the player manager.
    }
}
