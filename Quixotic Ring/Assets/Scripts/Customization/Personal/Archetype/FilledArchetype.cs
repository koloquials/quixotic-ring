using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledArchetype : MonoBehaviour {
    public Sprite[] sprites = new Sprite[6];
    private SpriteRenderer spriteRenderer;
    public int sprite = 0;
    public static bool filled = false;

    public int type; // 0 = tested, 1 = picked

    //public GameObject testAppraisalScreen; //appraisal screen (test)
    //public AppraisalPick testManager; //appraisal manager (test)

    public GameObject pickAppraisalScreen; //appraisal screen (pick)
    public AppraisalPick pickManager; //appraisal manager (pick)

    public PlayerInfo player;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        sprite = player.archetype;
        spriteRenderer.sprite = sprites[sprite];
    }

    void OnMouseDown(){
        if (type == 0){ //not implemented atm
            //testManager.num = player.archetype;
            //testAppraisalScreen.SetActive(true);
        } else if (type == 1){
            pickManager.num = player.archetype; //resets the menu positioning so it doesn't save a reverted state.
            pickAppraisalScreen.SetActive(true);
        }
    }

    public void SetFilled(){
        filled = true;
    }
}
