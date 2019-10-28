using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameToggle : MonoBehaviour{
    public Sprite[] sprites = new Sprite[3];
    private SpriteRenderer spriteRenderer;
    int sprite = 0;

    public PlayerInfo player;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    void Update(){
     
    }

    void OnMouseDown() {
        if (sprite == 2) {
            sprite = 0;
        } else {
            sprite++;
        }
        spriteRenderer.sprite = sprites[sprite];
        player.pName = sprite;
    }
}
