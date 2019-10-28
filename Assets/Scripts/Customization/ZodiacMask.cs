using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacMask : MonoBehaviour {
    public Sprite[] sprites = new Sprite[6];
    public SpriteRenderer spriteRenderer;
    int sprite = 0;

    public PlayerInfo player;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprite = Random.Range(0, 6);
        spriteRenderer.sprite = sprites[sprite];

        player.zodiac = sprite;
        player.SetSprite(sprite);
    }
}
