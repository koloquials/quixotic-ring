﻿using System.Collections;
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

    [Header("Opponent Name Text")]
    public SpriteRenderer opponentName;
    public Sprite[] opponentFightNameSprites;

    [Header("Fighting")]
    public int oRow;
    public int oKey;


    private KeyCode[] topRowKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P };
    private KeyCode[] midRowKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Semicolon };
    private KeyCode[] botRowKeys = { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Comma, KeyCode.Period };

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

        opponentName = GameObject.Find("opponent fight name").GetComponent<SpriteRenderer>();

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
            
            MovePlayer();
        }
    }

    public IEnumerator Fight(float waitTime, int opponent){
        // log that we're fighting
        Debug.Log("(PLAYER FIGHT) waiting! " + Time.time);
        fighting = true;
        fightScreen.SetActive(true);

        // find the opponent name image if we haven't yet
        if(opponentName == null)
            opponentName = GameObject.Find("opponent fight name").GetComponent<SpriteRenderer>();
        // set the opponent's name to whoever we're fighting
        opponentName.sprite = opponentFightNameSprites[opponent];

        // Opponent picks a random key in a random row
        oRow = Random.Range(0, 3);
        oKey = oRow == 2 ? Random.Range(0, 10) : Random.Range(1, 11);

        yield return new WaitForSeconds(waitTime);

        // close the fighting screen
        fightScreen.SetActive(false);

        // update the opponent and then resume them
        npcs[opponent].GetComponent<NPCMovement>().UpdateRivals();
        npcs[opponent].GetComponent<NPCMovement>().Resume();
        // resume myself
        Resume();
        // make the opponent not angry, tell them they aren't fighting the player, they aren't waiting, and telling them to keep the result
        npcs[opponent].GetComponent<NPCMovement>().isAngry = false;
        npcs[opponent].GetComponent<NPCMovement>().fightingPlayer = false;
        npcs[opponent].GetComponent<NPCMovement>().waiting = false;
        npcs[opponent].GetComponent<NPCMovement>().keepResult = true;
        // not that i'm not fighting
        fighting = false;

        yield return null;
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
