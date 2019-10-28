using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInfo: MonoBehaviour {
    public Sprite[] sprites = new Sprite[1]; //will just be the mask sprite later
    public SpriteRenderer spriteRenderer;

    public int rivalKey = 0; //[0]player, [1]aeris, [2]boreas, [3]clemise, [4]icarus, [5]nadine, [6]riri
    public string nName = ""; //0 to 2, temporary
    public int pronoun = 0; //0 to 2, pronoun set
    public string zodiac = ""; //dragon, rabbit, horse, goat, rooster, pig
    public string archetype = ""; //reformer, achiever, individualist, investigator, loyalist, challenger

    public int hostility;
    public int savvy;
    public int interest;
    public int grudge;

    public int[] rivalries = new int[7]; //[0]player, [1]aeris, [2]boreas, [3]clemise, [4]icarus, [5]nadine, [6]riri

    public float frustration;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
		
	}
}
