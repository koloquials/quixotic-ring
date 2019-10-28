using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickButton : MonoBehaviour {
    public Sprite[] sprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;

    public GameObject pickAppraisalScreen;
    public AppraisalPick pickManager;
    public PlayerInfo player;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    void OnMouseDown(){
        pickManager.num = player.archetype; //resets the menu positioning so it doesn't save a reverted state.
        pickAppraisalScreen.SetActive(true);
    }

    void OnMouseOver(){
        spriteRenderer.sprite = sprites[1];
    }

    void OnMouseExit(){
        spriteRenderer.sprite = sprites[0];
    }
}
