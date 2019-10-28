using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour {
    StatManager statManager;
    public PlayerInfo player;
    public NPCInfo npc;
    string zodiac;
    string archetype;

    public bool statsSet = false;
    bool hideStats = false;

    public Sprite[] dots = new Sprite[18];
    public GameObject[] stats = new GameObject[4];

    public Sprite[] zodi = new Sprite[2];
    public Sprite[] arche = new Sprite[2];
    public SpriteRenderer zodiRenderer;
    public SpriteRenderer archeRenderer;


    void Start () {
        statManager = GetComponent<StatManager>();
        if (npc && statsSet == false){
            zodiac = npc.zodiac;
            archetype = npc.archetype;
            hideStats = true;
            SetStats(zodiac, archetype);
            npc.hostility = statManager.zHostility + statManager.aHostility;
            npc.savvy = statManager.zSavvy + statManager.aSavvy;
            npc.interest = statManager.zInterest + statManager.aInterest;
            npc.grudge = statManager.zGrudge + statManager.aGrudge;
        }
    }

    void Update () {
        if (npc){
            if (npc.rivalries[0] >= 12){
                hideStats = false;
                SetStats(zodiac, archetype);
                zodiRenderer.sprite = zodi[1];
                archeRenderer.sprite = arche[1];
            }
        }
        if (player && statsSet == false){
            switch (player.zodiac){
                case 0:
                    zodiac = "dog";
                    break;
                case 1:
                    zodiac = "tiger";
                    break;
                case 2:
                    zodiac = "rat";
                    break;
                case 3:
                    zodiac = "monkey";
                    break;
                case 4:
                    zodiac = "ox";
                    break;
                case 5:
                    zodiac = "snake";
                    break;
                default:
                    zodiac = "oops";
                    print("empty/incorrect zodiac input");
                    break;
            }
            switch (player.archetype){
                case 0:
                    archetype = "reformer";
                    break;
                case 1:
                    archetype = "achiever";
                    break;
                case 2:
                    archetype = "individualist";
                    break;
                case 3:
                    archetype = "investigator";
                    break;
                case 4:
                    archetype = "loyalist";
                    break;
                case 5:
                    archetype = "challenger";
                    break;
                default:
                    zodiac = "oops";
                    print("empty/incorrect archetype input");
                    break;
            }

            SetStats(zodiac, archetype);
            player.hostility = statManager.zHostility + statManager.aHostility;
            player.savvy = statManager.zSavvy + statManager.aSavvy;
            player.interest = statManager.zInterest + statManager.aInterest;
            player.grudge = statManager.zGrudge + statManager.zGrudge;

        }
    }

    void SetStats (string zodiac, string archetype){
        statManager.CalculateStats(zodiac, archetype);
        if ((statManager.zHostility != 0 || statManager.aHostility != 0) && hideStats == false){
            for (int i = 0; i < statManager.zHostility; i++){
                stats[0].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i];
            }
            for (int i = statManager.zHostility; i < (statManager.aHostility + statManager.zHostility); i++){
                stats[0].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i+6];
            }
            for (int i = statManager.aHostility + statManager.zHostility; i < 6; i++){
                stats[0].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        } else if ((statManager.zHostility != 0 || statManager.aHostility != 0) && hideStats){
            for (int i = 0; i < (statManager.aHostility + statManager.zHostility); i++){
                stats[0].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i + 12];
            }
        } else {
            for (int i = 0; i < 6; i++){
                stats[0].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        if ((statManager.zSavvy != 0 || statManager.aSavvy != 0) && hideStats == false){
            for (int i = 0; i < statManager.zSavvy; i++){
                stats[1].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i];
            }
            for (int i = statManager.zSavvy; i < (statManager.aSavvy + statManager.zSavvy); i++){
                stats[1].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i+6];
            }
            for (int i = statManager.aSavvy + statManager.zSavvy; i < 6; i++){
                stats[1].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        } else if ((statManager.zSavvy != 0 || statManager.aSavvy != 0) && hideStats){
            for (int i = 0; i < (statManager.aSavvy + statManager.zSavvy); i++){
                stats[1].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i + 12];
            }
        } else{
            for (int i = 0; i < 6; i++){
                stats[1].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        if ((statManager.zInterest != 0 || statManager.aInterest != 0) && hideStats == false){
            for (int i = 0; i < statManager.zInterest; i++){
                stats[2].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i];
            }
            for (int i = statManager.zInterest; i < (statManager.aInterest + statManager.zInterest); i++){
                stats[2].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i+6];
            }
            for (int i = statManager.aInterest + statManager.zInterest; i < 6; i++){
                stats[2].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        } else if ((statManager.zInterest != 0 || statManager.aInterest != 0) && hideStats){
            for (int i = 0; i < (statManager.aInterest + statManager.zInterest); i++){
                stats[2].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i + 12];
            }
        } else{
            for (int i = 0; i < 6; i++){
                stats[2].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        if ((statManager.zGrudge != 0 || statManager.aGrudge != 0) && hideStats == false){
            for (int i = 0; i < statManager.zGrudge; i++){
                stats[3].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i];
            }
            for (int i = statManager.zGrudge; i < (statManager.aGrudge + statManager.zGrudge); i++){
                stats[3].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i+6];
            }
            for (int i = statManager.aGrudge + statManager.zGrudge; i < 6; i++){
                stats[3].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        } else if ((statManager.zGrudge != 0 || statManager.aGrudge != 0) && hideStats){
            for (int i = 0; i < (statManager.aGrudge + statManager.zGrudge); i++){
                stats[3].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dots[i + 12];
            }
        }else{
            for (int i = 0; i < 6; i++){
                stats[3].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
        }
        statsSet = true;
    }
}
