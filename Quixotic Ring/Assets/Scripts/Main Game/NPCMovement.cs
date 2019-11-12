using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {
    NPCInfo npc;
    public GameObject player;

    SpriteRenderer spriteRenderer;
    GameObject frustrationBubble;
    GameObject commandText;
    public Sprite[] cmd = new Sprite[2];
    SpriteRenderer commandTextRenderer;
    GameObject infoBox;

    public GameObject fightBubble;
    public GameObject rightWinBubble;
    public GameObject leftWinBubble;

    public GameObject temp;

    public bool waiting = false;

    int behaviorID;
    float direction;
    float duration;
    float speed = 3f;
    bool initiateMove = false;
    bool moving = false;
    float currentTime = 0f;

    public bool inRange;
    bool infoActive = false;

    public bool isAngry = false;
    bool frustrating = false;

    bool maxRivalsPlayer = false;
    public Sprite[] rivalName = new Sprite[7];
    public Sprite[] rivalMeter = new Sprite[13];
    public GameObject[] rivals = new GameObject[4];
    int rivalCap;

    float rand;
    int opponent;
    bool fighting = false;
    public bool fightingPlayer = false;
    bool frozen = false;

    bool resWaiting = false;
    public bool keepResult = false;

    Coroutine wait;
    Coroutine frustrate;
    Coroutine fight;
    Coroutine resultPersist;

    public float residualWinFrustration; //grudge
    public float residualLoseFrustration; //grudge
    public int loseTargetRivalryFactor; //grudge
    public int loseOtherRivalryFactor; //interest
    public int winRivalryFactor; //interest

    public GameObject[] othernpcs = new GameObject[6];
    public Dialog start_fight_dialogue;
    public Dialog won_fight_dialogue;
    public Dialog lost_fight_dialogue;

    void Start(){
        player = GameObject.Find("player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        npc = GetComponent<NPCInfo>();
        frustrationBubble = GameObject.Find("npcs/" + npc.nName + "/frustrated bubble");
        commandText = GameObject.Find("npcs/" + npc.nName + "/npc commands");
        commandTextRenderer = commandText.GetComponent<SpriteRenderer>();
        infoBox = GameObject.Find("npcs/" + npc.nName + "/info box");

        Vector2 currentPos = transform.position;
        currentPos.y = -4.13f;
        transform.position = currentPos;

        othernpcs[0] = GameObject.Find("aeris");
        othernpcs[1] = GameObject.Find("boreas");
        othernpcs[2] = GameObject.Find("clemise");
        othernpcs[3] = GameObject.Find("icarus");
        othernpcs[4] = GameObject.Find("nadine");
        othernpcs[5] = GameObject.Find("riri");
        //othernpcs[npc.rivalKey - 1] = null;

        switch (npc.hostility){
            case 0:
                npc.frustration = 0.0f;
                break;
            case 1:
                npc.frustration = 10.0f;
                break;
            case 2:
                npc.frustration = 20.0f;
                break;
            case 3:
                npc.frustration = 30.0f;
                break;
            case 4:
                npc.frustration = 40.0f;
                break;
            case 5:
                npc.frustration = 50.0f;
                break;
            case 6:
                npc.frustration = 60.0f;
                break;
            default:
                npc.frustration = 0.0f;
                print("empty/incorrect hostility input");
                break;
        }

        switch (npc.interest){
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

        switch (npc.grudge){
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

        UpdateRivals();
    }

    void Update(){
        if (frozen == false){
            // If the player is nearby show the button text
            if (inRange == true){
                commandText.SetActive(true);
            // Otherwise they are hidden and the info screen is closed
            } else{
                commandText.SetActive(false);
                infoBox.SetActive(false);
                infoActive = false;
                commandTextRenderer.sprite = cmd[0];
            }

            // Toggles whether the info screen is open
            if (inRange && Input.GetKeyDown(KeyCode.E) && infoActive == false){
                infoBox.SetActive(true);
                infoActive = true;
                commandTextRenderer.sprite = cmd[1];
            } else if (inRange && Input.GetKeyDown(KeyCode.E) && infoActive == true) {
                infoBox.SetActive(false);
                infoActive = false;
                commandTextRenderer.sprite = cmd[0];
            }

            // if your not angry and your not becoming frustrated run the frustrate coroutine
            if (isAngry == false && frustrating == false){
                frustrate = StartCoroutine(Frustrate());
            }

            // if your not moving and your not initiating movement and your not waiting start waiting
            if (moving == false && initiateMove == false && waiting == false){
                wait = StartCoroutine(Wait(Random.Range(4, 10)));
            }

            // if you should begin moving and you want to move right
            if (initiateMove && behaviorID == 0){ //right
                waiting = false;
                spriteRenderer.flipX = true;
                direction = 1.2f;
                currentTime = 0f;
                duration = Random.Range(4, 10);
                moving = true;
            // if you should begin moving and you want to move left
            } else if (initiateMove && behaviorID == 1){ //left
                waiting = false;
                spriteRenderer.flipX = false;
                direction = -1.2f;
                currentTime = 0f;
                duration = Random.Range(4, 10);
                moving = true;
            }

            // if your moving
            if (moving){
                //Debug.Log("walking! " + direction + " | duration: " + duration);
                initiateMove = false;
                Vector2 currentPos = transform.position;
                currentPos.y = -4.13f;
                //Debug.Log("position: " + currentPos.x);
                if (currentTime < duration){
                    currentTime += 0.1f;
                    currentPos.x += speed * direction * Time.deltaTime;
                    transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time*10) * 7);

                    if (currentPos.x < -34.65f){
                        currentPos.x = 38.5f;
                    }
                    if (currentPos.x > 38.5f){
                        currentPos.x = -34.65f;
                    }

                    transform.position = currentPos;
                    //Debug.Log("position (move): " + currentPos.x);
                }
                if (currentTime >= duration){
                    moving = false;
                    //Debug.Log("done walking! " + direction + " | duration: " + currentTime);
                }
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }

            // if the player is in range and pressed R and you're not already fighting and the player isn't fighting
            if (inRange && Input.GetKeyDown(KeyCode.R) && fighting == false && fightingPlayer == false && start_fight_dialogue.gameObject.activeSelf == false){
                // freeze the player
                player.GetComponent<PlayerMovement>().Freeze();
                // freeze me
                Freeze();
                // note that i'm fighting the player
                fightingPlayer = true;
                start_fight_dialogue.gameObject.SetActive(true);
                AudioManager.instance.PlaySound("next");
                AudioManager.instance.PlaySound("pageturn");
                // on the player start the fight coroutine passing a random wait time and my rivalKe
            }

            // if i'm angry and i'm moving
            if (isAngry && moving){
                // if the player isn't fighting and I'm near the player 
                if (player.GetComponent<PlayerMovement>().fighting == false && Mathf.Abs(player.transform.position.x - transform.position.x) <= 4.45f && Mathf.Abs(player.transform.position.x - transform.position.x) >= 4.05f){
                    rand = Random.Range(0f, 100f) * (1f + 0.1f * npc.rivalries[0]);
                    Debug.Log(rand);
                    // pick if we fight based on our rivalry
                    if (rand >= 80f){
                        AudioManager.instance.PlaySound("barfull");
                        Debug.Log("[PLAYER] fight!!!");

                        // face the player
                        if (transform.position.x > player.transform.position.x){
                            //this npc is on the right
                            spriteRenderer.flipX = false;
                            player.GetComponent<SpriteRenderer>().flipX = true;
                        } else {
                            //this npc is on the left
                            spriteRenderer.flipX = true;
                            player.GetComponent<SpriteRenderer>().flipX = false;
                        }

                        frustrationBubble.SetActive(false);

                        //[WIP] freeze and resume function on the player.
                        player.GetComponent<PlayerMovement>().Freeze();
                        Freeze();
                        fightingPlayer = true;

                        player.GetComponent<PlayerMovement>().fight = StartCoroutine(player.GetComponent<PlayerMovement>().Fight(Random.Range(8, 14), npc.rivalKey - 1));
                    }
                }
            }

            // if i'm angry and moving
            if (isAngry && moving){
                // check each npc
                for (int i = 0; i < 6; i++){
                    if (i != npc.rivalKey-1 && othernpcs[i].GetComponent<NPCMovement>().fightingPlayer == false && othernpcs[i].GetComponent<NPCMovement>().fighting == false && 
                        Mathf.Abs(othernpcs[i].transform.position.x - transform.position.x) <= 4.35f && Mathf.Abs(othernpcs[i].transform.position.x - transform.position.x) >= 4.15f){

                        rand = Random.Range(0f, 100f) * (1f + 0.1f * npc.rivalries[i]);
                        Debug.Log(rand);
                        if (rand >= 80f){
                            Debug.Log("fight!!!");
                            opponent = i;
                            if (transform.position.x > othernpcs[opponent].transform.position.x){
                                //this npc is on the right
                                spriteRenderer.flipX = false;
                                othernpcs[opponent].GetComponent<SpriteRenderer>().flipX = true;
                            } else {
                                //this npc is on the left
                                spriteRenderer.flipX = true;
                                othernpcs[opponent].GetComponent<SpriteRenderer>().flipX = false;
                            }

                            //display fight bubble;
                            temp = Instantiate(fightBubble, new Vector3((othernpcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);

                            frustrationBubble.SetActive(false);
                            othernpcs[opponent].GetComponent<NPCMovement>().frustrationBubble.SetActive(false);

                            othernpcs[opponent].GetComponent<NPCMovement>().Freeze();
                            Freeze();
                            fighting = true;
                        }
                    }
                }
            }

        }

        // if i'm fighting and waiting start the FightWait coroutine
        if (fighting && waiting == false){
            fight = StartCoroutine(FightWait(Random.Range(8, 14)));
        }
        
        if (keepResult && resWaiting == false){
            resultPersist = StartCoroutine(PersistRes(3.8f));
        }
        if (start_fight_dialogue.done)
        {
            Debug.Log("A");
            player.GetComponent<PlayerMovement>().fight = StartCoroutine(player.GetComponent<PlayerMovement>().Fight(Random.Range(8, 14), npc.rivalKey - 1));
            start_fight_dialogue.done = false;
            start_fight_dialogue.gameObject.SetActive(false);
        }

    }

    IEnumerator FightWait(float waitTime){
        Debug.Log("(fight) waiting! " + Time.time);
        waiting = true;
        yield return new WaitForSeconds(waitTime);

        float npcRoll = Random.Range(0f, 100f) * (1f + 0.2f * npc.savvy) * (1f + 0.1f * npc.hostility);
        float rivalRoll = Random.Range(0f, 100f) * (1f + 0.2f * othernpcs[opponent].GetComponent<NPCInfo>().savvy) * (1f + 0.1f * othernpcs[opponent].GetComponent<NPCInfo>().hostility);
        Destroy(temp);

        if (npcRoll >= rivalRoll){ //win
            if (transform.position.x > othernpcs[opponent].transform.position.x){ //npc is on right
                temp = Instantiate(rightWinBubble, new Vector3((othernpcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            } else{
                temp = Instantiate(leftWinBubble, new Vector3((othernpcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            }
            ResolveFight(0);
            Debug.Log(npcRoll + " > " + rivalRoll);
        } else { //lose
            if (transform.position.x > othernpcs[opponent].transform.position.x){ //npc is on right
                temp = Instantiate(leftWinBubble, new Vector3((othernpcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            } else{
                temp = Instantiate(rightWinBubble, new Vector3((othernpcs[opponent].transform.position.x + transform.position.x) / 2, 1.45f, 0f), Quaternion.identity);
            }
            ResolveFight(1);
            Debug.Log(npcRoll + " < " + rivalRoll);
        }

        UpdateRivals();
        othernpcs[opponent].GetComponent<NPCMovement>().UpdateRivals();
        othernpcs[opponent].GetComponent<NPCMovement>().Resume();
        Resume();
        isAngry = false;
        fighting = false;
        waiting = false;
        keepResult = true;
    }

    void ResolveFight(int i){
        if (i == 0){ //win
            npc.frustration = residualWinFrustration;
            othernpcs[opponent].GetComponent<NPCInfo>().frustration = othernpcs[opponent].GetComponent<NPCMovement>().residualLoseFrustration;

            npc.rivalries[opponent+1] += winRivalryFactor + othernpcs[opponent].GetComponent<NPCMovement>().loseTargetRivalryFactor;
            if (npc.rivalries[opponent + 1] > 12){
                npc.rivalries[opponent + 1] = 12;
            }
            othernpcs[opponent].GetComponent<NPCInfo>().rivalries[npc.rivalKey] += winRivalryFactor + othernpcs[opponent].GetComponent<NPCMovement>().loseTargetRivalryFactor;
            if (othernpcs[opponent].GetComponent<NPCInfo>().rivalries[npc.rivalKey] > 12){
                othernpcs[opponent].GetComponent<NPCInfo>().rivalries[npc.rivalKey] = 12;
            }
        
            for (int n = 0; n < 7; n++){
                if (n != npc.rivalKey){
                    othernpcs[opponent].GetComponent<NPCInfo>().rivalries[n] += othernpcs[opponent].GetComponent<NPCMovement>().loseOtherRivalryFactor;
                    if (othernpcs[opponent].GetComponent<NPCInfo>().rivalries[n] < 0){
                        othernpcs[opponent].GetComponent<NPCInfo>().rivalries[n] = 0;
                    }
                    if (n > 0){
                        othernpcs[n-1].GetComponent<NPCInfo>().rivalries[opponent+1] += othernpcs[opponent].GetComponent<NPCMovement>().loseOtherRivalryFactor;
                        if (othernpcs[n - 1].GetComponent<NPCInfo>().rivalries[opponent + 1] < 0){
                            othernpcs[n - 1].GetComponent<NPCInfo>().rivalries[opponent + 1] = 0;
                        }
                        othernpcs[n-1].GetComponent<NPCMovement>().UpdateRivals();
                    }
                }
            }
        } else if (i == 1){ //lose
            npc.frustration = residualLoseFrustration;
            othernpcs[opponent].GetComponent<NPCInfo>().frustration = othernpcs[opponent].GetComponent<NPCMovement>().residualWinFrustration;
            npc.rivalries[opponent+1] += loseTargetRivalryFactor + othernpcs[opponent].GetComponent<NPCMovement>().winRivalryFactor;
            othernpcs[opponent].GetComponent<NPCInfo>().rivalries[npc.rivalKey] += loseTargetRivalryFactor + othernpcs[opponent].GetComponent<NPCMovement>().winRivalryFactor;

            for (int n = 0; n < 7; n++){
                if (n != opponent+1){
                    npc.rivalries[n] += loseOtherRivalryFactor;
                    if (npc.rivalries[n] < 0){
                        npc.rivalries[n] = 0;
                    }
                    if (n > 0){
                        othernpcs[n-1].GetComponent<NPCInfo>().rivalries[npc.rivalKey] += loseOtherRivalryFactor;
                        if (othernpcs[n - 1].GetComponent<NPCInfo>().rivalries[npc.rivalKey] < 0){
                            othernpcs[n - 1].GetComponent<NPCInfo>().rivalries[npc.rivalKey] = 0;
                        }
                        othernpcs[n - 1].GetComponent<NPCMovement>().UpdateRivals();
                    }
                }
            }
        }
    }

    public void Teleport(int direction){
        Vector2 currentPos = transform.position;
        currentPos.y = -4.13f;

        if (direction == 0){
            currentPos.x += 73f;
        }
        if (direction == 1){
            currentPos.x -= 73f;
        }
        transform.position = currentPos;
    }

    public void UpdateRivals(){
        int[] temp = new int[4];
        bool notValid = false;

        for (int i = 0; i < rivalCap; i++){ //iterate through each temp[] node
            for (int n = 0; n < npc.rivalries.Length; n++){ //iterate through each rivalries[] in the npc object
                if (npc.rivalries[n] > npc.rivalries[temp[i]]){ //check to make sure it's not already recorded
                    if (i > 0){
                        for (int j = i; j >= 0; j--){
                            if (temp[j] == n){
                                notValid = true;
                            }
                        }
                    }
                    if (notValid){
                        notValid = false;
                    } else{
                        temp[i] = n;
                    }
                }
            }
        }
        Debug.Log(temp[0] + ", " + temp[1] + ", " + temp[2] + ", " + temp[3]);

        for (int i = 0; i < rivalCap; i++){
            rivals[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = rivalName[temp[i]]; //NAME
            rivals[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = rivalMeter[npc.rivalries[temp[i]]]; //METER
        }
        for (int i = rivalCap; i < 4; i++){
            rivals[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null; //NAME
            rivals[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null; //METER
        }
    }

    public void Freeze(){
        frozen = true;
        if (waiting){
            StopCoroutine(wait);
        }
        if (frustrating){
            StopCoroutine(frustrate);
        }
    }

    public void Resume(){
        frozen = false;
        waiting = false;
        initiateMove = false;
        moving = false;
        currentTime = 0f;
        inRange = false;
        infoActive = false;
        isAngry = false;
        frustrating = false;
        keepResult = false;
    }

    IEnumerator PersistRes(float waitTime){
        //Debug.Log("waiting! " + Time.time);
        resWaiting = true;
        yield return new WaitForSeconds(waitTime);
        Destroy(temp);
        resWaiting = false;
        keepResult = false;
        //Debug.Log("done waiting! " + Time.time + " | new id: " + behaviorID);
    }

    IEnumerator Wait(float waitTime){
        //Debug.Log("waiting! " + Time.time);
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        behaviorID = Random.Range(0, 2);
        initiateMove = true;
        //Debug.Log("done waiting! " + Time.time + " | new id: " + behaviorID);
    }

    IEnumerator Frustrate(){
        frustrating = true;
        float fFactor;
        switch (npc.hostility){
            case 0:
                fFactor = 1.0f;
                break;
            case 1:
                fFactor = 1.2f;
                break;
            case 2:
                fFactor = 1.4f;
                break;
            case 3:
                fFactor = 1.6f;
                break;
            case 4:
                fFactor = 1.8f;
                break;
            case 5:
                fFactor = 2.4f;
                break;
            case 6:
                fFactor = 3.0f;
                break;
            default:
                fFactor = 1.0f;
                print("(frustration) empty/incorrect hostility input");
                break;
        }
        while (npc.frustration < 100f){
            npc.frustration += fFactor * Time.deltaTime;
            yield return null;
        }
        isAngry = true;
        frustrationBubble.SetActive(true);
        frustrating = false;
    }

}
