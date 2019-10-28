using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    PlayerInfo player;
    public GameObject fightScreen; 

    float speed = 3f;
    public bool fighting = false;
    bool frozen = false;

    public GameObject[] npcs = new GameObject[6];

    public Coroutine fight;

    int rivalCap = 1;
    public float residualWinFrustration; //grudge
    public float residualLoseFrustration; //grudge
    public int loseTargetRivalryFactor; //grudge
    public int loseOtherRivalryFactor; //interest
    public int winRivalryFactor; //interest

    void Start(){
        Vector2 currentPos = transform.position;
        player = GetComponent<PlayerInfo>();
        currentPos.y = -4.13f;
        currentPos.x = 0f;
        transform.position = currentPos;

        npcs[0] = GameObject.Find("aeris");
        npcs[1] = GameObject.Find("boreas");
        npcs[2] = GameObject.Find("icarus");
        npcs[3] = GameObject.Find("riri");
        npcs[4] = GameObject.Find("clemise");
        npcs[5] = GameObject.Find("nadine");
        fightScreen = GameObject.Find("fight");
        fightScreen.SetActive(false);

        switch (player.interest){
            case 0:
                rivalCap = 1;
                loseOtherRivalryFactor = -3;
                winRivalryFactor = 1;
                break;
            case 1:
                rivalCap = 1;
                loseOtherRivalryFactor = -2;
                winRivalryFactor = 1;
                break;
            case 2:
                rivalCap = 2;
                loseOtherRivalryFactor = -2;
                winRivalryFactor = 1;
                break;
            case 3:
                rivalCap = 3;
                loseOtherRivalryFactor = -1;
                winRivalryFactor = 2;
                break;
            case 4:
                rivalCap = 3;
                loseOtherRivalryFactor = -1;
                winRivalryFactor = 2;
                break;
            case 5:
                rivalCap = 4;
                loseOtherRivalryFactor = -1;
                winRivalryFactor = 3;
                break;
            case 6:
                rivalCap = 4;
                loseOtherRivalryFactor = 0;
                winRivalryFactor = 3;
                break;
            default:
                rivalCap = 1;
                loseOtherRivalryFactor = -3;
                winRivalryFactor = 1;
                print("empty/incorrect interest input");
                break;
        }

        switch (player.grudge){
            case 0:
                residualWinFrustration = 0f;
                residualLoseFrustration = 10f;
                loseTargetRivalryFactor = 0;
                break;
            case 1:
                residualWinFrustration = 2f;
                residualLoseFrustration = 16f;
                loseTargetRivalryFactor = 0;
                break;
            case 2:
                residualWinFrustration = 4f;
                residualLoseFrustration = 22f;
                loseTargetRivalryFactor = 1;
                break;
            case 3:
                residualWinFrustration = 6f;
                residualLoseFrustration = 30f;
                loseTargetRivalryFactor = 1;
                break;
            case 4:
                residualWinFrustration = 8f;
                residualLoseFrustration = 42f;
                loseTargetRivalryFactor = 2;
                break;
            case 5:
                residualWinFrustration = 14f;
                residualLoseFrustration = 54f;
                loseTargetRivalryFactor = 2;
                break;
            case 6:
                residualWinFrustration = 22f;
                residualLoseFrustration = 66f;
                loseTargetRivalryFactor = 3;
                break;
            default:
                residualWinFrustration = 0f;
                residualLoseFrustration = 10f;
                loseTargetRivalryFactor = 0;
                print("empty/incorrect grudge input");
                break;
        }
    } 
    void Update(){

        if (frozen == false){
            for (int i = 0; i < npcs.Length; i++){
                NPCMovement npcMove = npcs[i].GetComponent<NPCMovement>();
                if (Mathf.Abs(npcs[i].transform.position.x - transform.position.x) <= 1.4f){
                    npcMove.inRange = true;
                } else{
                    npcMove.inRange = false;
                }
            }
        }
    }

    void FixedUpdate(){
        if (frozen != true){
            MovePlayer();
        }
    }

    public IEnumerator Fight(float waitTime, int opponent){
        Debug.Log("(PLAYER FIGHT) waiting! " + Time.time);
        fighting = true;
        fightScreen.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        float playerRoll = Random.Range(0f, 100f) * (1f + 0.2f * player.savvy) * (1f + 0.1f * player.hostility);
        float npcRoll = Random.Range(0f, 100f) * (1f + 0.2f * npcs[opponent].GetComponent<NPCInfo>().savvy) * (1f + 0.1f * npcs[opponent].GetComponent<NPCInfo>().hostility);
        //npcs[opponent].GetComponent<NPCMovement>().Destroy(temp);

        if (playerRoll >= npcRoll){ //player win
            if (transform.position.x > npcs[opponent].transform.position.x){ //player is on right
                npcs[opponent].GetComponent<NPCMovement>().temp = Instantiate(npcs[opponent].GetComponent<NPCMovement>().rightWinBubble, new Vector3((npcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            } else{
                npcs[opponent].GetComponent<NPCMovement>().temp = Instantiate(npcs[opponent].GetComponent<NPCMovement>().leftWinBubble, new Vector3((npcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            }
            ResolveFight(opponent, 0);
            Debug.Log(playerRoll + " > " + npcRoll);
        } else { //lose
            if (transform.position.x > npcs[opponent].transform.position.x){ //player is on right
                npcs[opponent].GetComponent<NPCMovement>().temp = Instantiate(npcs[opponent].GetComponent<NPCMovement>().leftWinBubble, new Vector3((npcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            } else{
                npcs[opponent].GetComponent<NPCMovement>().temp = Instantiate(npcs[opponent].GetComponent<NPCMovement>().rightWinBubble, new Vector3((npcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            }
            ResolveFight(opponent, 1);
            Debug.Log(playerRoll + " < " + npcRoll);
        }
        fightScreen.SetActive(false);

        npcs[opponent].GetComponent<NPCMovement>().UpdateRivals();
        npcs[opponent].GetComponent<NPCMovement>().Resume();
        Resume();
        npcs[opponent].GetComponent<NPCMovement>().isAngry = false;
        npcs[opponent].GetComponent<NPCMovement>().fightingPlayer = false;
        npcs[opponent].GetComponent<NPCMovement>().waiting = false;
        npcs[opponent].GetComponent<NPCMovement>().keepResult = true;
        fighting = false;
    }

    public void ResolveFight(int opponent, int i){
        if (i == 0){ //win
            npcs[opponent].GetComponent<NPCInfo>().frustration = npcs[opponent].GetComponent<NPCMovement>().residualLoseFrustration;
            npcs[opponent].GetComponent<NPCInfo>().rivalries[0] += winRivalryFactor + npcs[opponent].GetComponent<NPCMovement>().loseTargetRivalryFactor;

            if (npcs[opponent].GetComponent<NPCInfo>().rivalries[0] > 12){
                npcs[opponent].GetComponent<NPCInfo>().rivalries[0] = 12;
            }
            player.rivalries[opponent] = npcs[opponent].GetComponent<NPCInfo>().rivalries[0];

            for (int n = 0; n < 7; n++){
                if (n != 0){
                    npcs[opponent].GetComponent<NPCInfo>().rivalries[n] += npcs[opponent].GetComponent<NPCMovement>().loseOtherRivalryFactor;
                    if (npcs[opponent].GetComponent<NPCInfo>().rivalries[n] < 0){
                        npcs[opponent].GetComponent<NPCInfo>().rivalries[n] = 0;
                    }
                    if (n > 0){
                        npcs[n-1].GetComponent<NPCInfo>().rivalries[opponent+1] += npcs[opponent].GetComponent<NPCMovement>().loseOtherRivalryFactor;
                        if (npcs[n - 1].GetComponent<NPCInfo>().rivalries[opponent + 1] < 0){
                            npcs[n - 1].GetComponent<NPCInfo>().rivalries[opponent + 1] = 0;
                        }
                        npcs[n-1].GetComponent<NPCMovement>().UpdateRivals();
                    }
                }
            }
        } else if (i == 1){ //lose
            npcs[opponent].GetComponent<NPCInfo>().frustration = npcs[opponent].GetComponent<NPCMovement>().residualWinFrustration;
            npcs[opponent].GetComponent<NPCInfo>().rivalries[0] += loseTargetRivalryFactor + npcs[opponent].GetComponent<NPCMovement>().winRivalryFactor;

            if (npcs[opponent].GetComponent<NPCInfo>().rivalries[0] > 12){
                npcs[opponent].GetComponent<NPCInfo>().rivalries[0] = 12;
            }
            player.rivalries[opponent] = npcs[opponent].GetComponent<NPCInfo>().rivalries[0];

            for (int n = 0; n < 7; n++){
                if (n != opponent+1){
                    player.rivalries[n] += loseOtherRivalryFactor;
                    if (player.rivalries[n] < 0){
                        player.rivalries[n] = 0;
                    }
                    if (n > 0){
                        npcs[n-1].GetComponent<NPCInfo>().rivalries[0] += loseOtherRivalryFactor;
                        if (npcs[n - 1].GetComponent<NPCInfo>().rivalries[0] < 0){
                            npcs[n - 1].GetComponent<NPCInfo>().rivalries[0] = 0;
                        }
                        npcs[n - 1].GetComponent<NPCMovement>().UpdateRivals();
                    }
                }
            }
        }
    }

    void MovePlayer(){
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (moveHorizontal > 0f){
            player.spriteRenderer.flipX = true;
        } else if (moveHorizontal < 0f){
            player.spriteRenderer.flipX = false;
        }

        Vector2 currentPos = transform.position;
        currentPos.x += speed * moveHorizontal * Time.deltaTime;

        if (currentPos.x < -22.6f){
            currentPos.x = 50.6f;
            for (int i = 0; i < npcs.Length; i++){
                if (npcs[i].transform.position.x <= -10.6f){
                    NPCMovement npcMove = npcs[i].GetComponent<NPCMovement>();
                    npcMove.Teleport(0);
                }
            }
        }
        if (currentPos.x > 50.6f){
            currentPos.x = -22.6f;
            for (int i = 0; i < npcs.Length; i++){
                if (npcs[i].transform.position.x >= 38.6f){
                    NPCMovement npcMove = npcs[i].GetComponent<NPCMovement>();
                    npcMove.Teleport(1);
                }
            }
        }

        transform.position = currentPos;
    }

    public void Freeze(){
        frozen = true;
    }

    public void Resume(){
        frozen = false;
    }

}
