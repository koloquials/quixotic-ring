using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpEnter : MonoBehaviour {
    public Sprite[] sprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;

    public GameObject helpScreen;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }
	
	void OnMouseDown(){
        AudioManager.instance.PlaySound("pageturn");
        helpScreen.SetActive(true);
    }

    void OnMouseOver(){
        spriteRenderer.sprite = sprites[1];
    }

    void OnMouseExit(){
        spriteRenderer.sprite = sprites[0];
    }
}
