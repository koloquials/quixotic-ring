using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour {
    public Sprite[] sprites = new Sprite[4];
    private SpriteRenderer spriteRenderer;
    int sprite = 0;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }
	
	// Update is called once per frame
	void Update () {
        spriteRenderer.sprite = sprites[sprite];
    }

    void OnMouseDown(){
        //hardcoded for now, could use separate game objects hooked up later.
        if (Input.mousePosition.y >= 615f){
            sprite = 0;
        } else if (Input.mousePosition.y >= 565f){
            sprite = 1;
        } else if (Input.mousePosition.y >= 510f){
            sprite = 2;
        } else if (Input.mousePosition.y >= 450f){
            sprite = 3;
        }
    }
}
