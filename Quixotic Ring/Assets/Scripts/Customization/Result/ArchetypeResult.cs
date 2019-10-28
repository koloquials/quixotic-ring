using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchetypeResult : MonoBehaviour {
    public Sprite[] sprites = new Sprite[6];
    public SpriteRenderer spriteRenderer;
    int sprite = 0;

    public PlayerInfo player;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update () {
        sprite = player.archetype;
        spriteRenderer.sprite = sprites[sprite];
    }
}
