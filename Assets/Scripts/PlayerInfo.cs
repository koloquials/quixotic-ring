using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour {
    public Sprite[] sprites = new Sprite[6]; //will just be the mask sprite later
    public SpriteRenderer spriteRenderer;
    PlayerMovement moveManager;

    public int rivalKey = 0; //[0]player, [1]aeris, [2]boreas, [3]clemise, [4]icarus, [5]nadine, [6]riri
    public int pName = 0; //0 to 2, temporary
    public int pronoun = 0; //0 to 2, pronoun set
    public int zodiac = 0; //0 to 5, zodiac mask
    public int archetype = 0; //0 to 5, personality archetype 

    public int hostility;
    public int savvy;
    public int interest;
    public int grudge;

    public int[] rivalries = new int[7]; //[0]player, [1]aeris, [2]boreas, [3]clemise, [4]icarus, [5]nadine, [6]riri

    void Start () {
        Screen.SetResolution(1920, 1080, true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveManager = GetComponent<PlayerMovement>();
        GetComponent<PlayerMovement>().enabled = false;
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)){
            Debug.Log("name:" + pName + " | pronoun:" + pronoun + " | zodiac:" + zodiac + " | archetype:" + archetype + 
                      "\nhostility:" + hostility + " | savvy:" + savvy + " | interest:" + interest + " | grudge:" + grudge);
        }
        if (GetComponent<PlayerMovement>().enabled == false && SceneManager.GetActiveScene().name == "MainGame"){
            GetComponent<PlayerMovement>().enabled = true;
        }
    }

    public void SetSprite(int i){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[i];
    }


}
