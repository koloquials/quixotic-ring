using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PronounToggle : MonoBehaviour {
    public Sprite[] sprites = new Sprite[3];
    private SpriteRenderer spriteRenderer;
    public int sprite = 0;

    public PlayerInfo player;
    public NPCInfo npc;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }
	
	void Update(){
        spriteRenderer.sprite = sprites[sprite];
        if (player){
            player.pronoun = sprite;
        }
        if (npc){
            spriteRenderer.sprite = sprites[npc.pronoun];
        }
    }
}
