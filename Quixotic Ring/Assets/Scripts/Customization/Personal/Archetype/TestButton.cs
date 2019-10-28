using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour {
    public Sprite[] sprites = new Sprite[2];

    private SpriteRenderer spriteRenderer;
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        spriteRenderer.sprite = sprites[1];
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = sprites[0];
    }
}
