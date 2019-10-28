using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrnArrow : MonoBehaviour {
    public Sprite[] sprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;
    public int side; // 0 = back, 1 = next

    public PronounToggle pronoun; //interacts with visual pronoun sprite 
    int prn; //index of pronoun sprite

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        pronoun = GameObject.Find("pronoun").GetComponent<PronounToggle>();
    }

    void OnMouseDown(){
        spriteRenderer.sprite = sprites[1];
    }

    void OnMouseUp()
    {
        spriteRenderer.sprite = sprites[0];

        if (side == 1)
        {
            if (pronoun.sprite == 2){
                pronoun.sprite = 0;
            }
            else{
                pronoun.sprite++;
            }
        }
        if (side == 0)
        {
            if (pronoun.sprite == 0){
                pronoun.sprite = 2;
            }
            else{
                pronoun.sprite--;
            }
        }
    }
}
