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

    [Header("Opponent Name Text")]
    public SpriteRenderer opponentName;
    public Sprite[] opponentFightNameSprites;

    [Header("Fighting")]
    public int oRow;
    public int oKey;
    public KeyCode oKeyCode ()
    {
        switch(oRow)
        {
            case 0:
                return topRowKeys[oKey];
            case 1:
                return midRowKeys[oKey];
            case 2:
                return botRowKeys[oKey];
        }
        return KeyCode.Space;
    }
    public KeyCode pKeyCode;
    public int pRow ()
    {
        foreach (KeyCode k in topRowKeys)
            if (k == pKeyCode)
                return 0;
        foreach (KeyCode k in midRowKeys)
            if (k == pKeyCode)
                return 1;
        foreach (KeyCode k in botRowKeys)
            if (k == pKeyCode)
                return 2;
        return 0;
    }
    public int pKey ()
    {
        for (int i = 0; i < topRowKeys.Length; i++)
        {
            if (topRowKeys[i] == pKeyCode)
                return i;
        }
        for (int i = 0; i < midRowKeys.Length; i++)
        {
            if (midRowKeys[i] == pKeyCode)
                return i;
        }
        for (int i = 0; i < botRowKeys.Length; i++)
        {
            if (botRowKeys[i] == pKeyCode)
                return i;
        }
        return -1;
    }
    public float score;
    public float secondsPerOpponentPress;
    private float opponentTimer;
    public float opponentAttackTimer;
    public float playerAttackTimer;
    public bool oStunned;

    [Header("Fight Screen")]
    public SpriteRenderer oTopRend;
    public SpriteRenderer oMidRend;
    public SpriteRenderer oBotRend;
    public SpriteRenderer oTopKey;
    public SpriteRenderer oMidKey;
    public SpriteRenderer oBotKey;
    public Sprite oInactive;
    public Sprite oWinning, oLosing;

    public SpriteRenderer pTopRend;
    public SpriteRenderer pMidRend;
    public SpriteRenderer pBotRend;
    public SpriteRenderer pTopKey;
    public SpriteRenderer pMidKey;
    public SpriteRenderer pBotKey;
    public Sprite pInactive;
    public Sprite pWinning, pLosing;

    public Sprite[] fightCharacters;
    public Sprite questionMark;

    public Transform fightBar;

    private KeyCode[] topRowKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P };
    private KeyCode[] midRowKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Semicolon };
    private KeyCode[] botRowKeys = { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Comma, KeyCode.Period };

    private int currentOpponent;

    private Coroutine oSwitch;
    public Dialog intro_fight;

    public Sprite[] poses;
    public SpriteRenderer showPose;
    public SpriteRenderer attackBar;
    public SpriteRenderer opponentAttackBar;

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

        if (opponentName == null)
        {
            opponentName = GameObject.Find("opponent fight name").GetComponent<SpriteRenderer>();
        }
        if (oTopRend == null)
            oTopRend = GameObject.Find("top button (opponent)").GetComponent<SpriteRenderer>();
        if (oMidRend == null)
            oMidRend = GameObject.Find("mid button (opponent)").GetComponent<SpriteRenderer>();
        if (oBotRend == null)
            oBotRend = GameObject.Find("bot button (opponent)").GetComponent<SpriteRenderer>();
        if (oTopKey == null)
            oTopKey = GameObject.Find("top key (opponent)").GetComponent<SpriteRenderer>();
        if (oMidKey == null)
            oMidKey = GameObject.Find("mid key (opponent)").GetComponent<SpriteRenderer>();
        if (oBotKey == null)
            oBotKey = GameObject.Find("bot key (opponent)").GetComponent<SpriteRenderer>();

        if (pTopRend == null)
            pTopRend = GameObject.Find("top button (player)").GetComponent<SpriteRenderer>();
        if (pMidRend == null)
            pMidRend = GameObject.Find("mid button (player)").GetComponent<SpriteRenderer>();
        if (pBotRend == null)
            pBotRend = GameObject.Find("bot button (player)").GetComponent<SpriteRenderer>();
        if (pTopKey == null)
            pTopKey = GameObject.Find("top key (player)").GetComponent<SpriteRenderer>();
        if (pMidKey == null)
            pMidKey = GameObject.Find("mid key (player)").GetComponent<SpriteRenderer>();
        if (pBotKey == null)
            pBotKey = GameObject.Find("bot key (player)").GetComponent<SpriteRenderer>();
        if (showPose == null)
            showPose = GameObject.Find("fight poses").GetComponent<SpriteRenderer>();
        if(attackBar == null)
        {
            GameObject temp = GameObject.Find("player attack bar");
            attackBar = temp.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }if(opponentAttackBar == null)
        {
            GameObject temp = GameObject.Find("opponent attack bar");
            opponentAttackBar = temp.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        fightBar = GameObject.Find("fight victory slider").transform;

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

        if(opponentName== null)
        {
            opponentName = GameObject.Find("opponent fight name").GetComponent<SpriteRenderer>();
        }
        if (oTopRend == null)
            oTopRend = GameObject.Find("top button (opponent)").GetComponent<SpriteRenderer>();
        if (oMidRend == null)
            oMidRend = GameObject.Find("mid button (opponent)").GetComponent<SpriteRenderer>();
        if (oBotRend == null)
            oBotRend = GameObject.Find("bot button (opponent)").GetComponent<SpriteRenderer>();
        if (oTopKey == null)
            oTopKey = GameObject.Find("top key (opponent)").GetComponent<SpriteRenderer>();
        if (oMidKey == null)
            oMidKey = GameObject.Find("mid key (opponent)").GetComponent<SpriteRenderer>();
        if (oBotKey == null)
            oBotKey = GameObject.Find("bot key (opponent)").GetComponent<SpriteRenderer>();

        if (pTopRend == null)
            pTopRend = GameObject.Find("top button (player)").GetComponent<SpriteRenderer>();
        if (pMidRend == null)                       
            pMidRend = GameObject.Find("mid button (player)").GetComponent<SpriteRenderer>();
        if (pBotRend == null)                       
            pBotRend = GameObject.Find("bot button (player)").GetComponent<SpriteRenderer>();
        if (pTopKey == null)
            pTopKey = GameObject.Find("top key (player)").GetComponent<SpriteRenderer>();
        if (pMidKey == null)                    
            pMidKey = GameObject.Find("mid key (player)").GetComponent<SpriteRenderer>();
        if (pBotKey == null)                    
            pBotKey = GameObject.Find("bot key (player)").GetComponent<SpriteRenderer>();

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

        if(fighting)
            Fighting();
    }

    private void Fighting()
    {
        float lerpValue = (score+50)/100;
        fightBar.localPosition = new Vector3(Mathf.Lerp(3.5f, -3.5f, lerpValue), 0);
        Transform playerBar = fightBar.GetChild(0).GetChild(1);
        Transform opponentBar = fightBar.GetChild(0).GetChild(0);
        playerBar.transform.localScale = new Vector3(Mathf.Lerp(345*2,0,lerpValue), 33.75f, 1);
        opponentBar.transform.localScale = new Vector3(Mathf.Lerp(0,345*2,lerpValue), 33.75f, 1);

        DrawOpponentFightKey();
        DrawPlayerFightKey();

        // Opponent key presses
        opponentTimer -= Time.deltaTime;
        if(opponentTimer < 0)
        {
            AudioManager.instance.PlaySound("push_npc");
            opponentTimer = secondsPerOpponentPress;
            int dif = player.hostility - npcs[currentOpponent].GetComponent<NPCInfo>().hostility;
            if (dif == 0)
                score += 1;
            else if (dif == 1)
                score += 1;
            else if (dif == 2)
                score += 1f;
            else if (dif == 3)
                score += 1f;
            else if (dif == 4)
                score += 1f;
            else if (dif == 5)
                score += 1f;
            else if (dif == 6)
                score += 1f;
            else if (dif == -1)
                score += 1.1f;
            else if (dif == -2)
                score += 1.66f;
            else if (dif == -3)
                score += 2.307f;
            else if (dif == -4)
                score += 2.66f;
            else if (dif == -5)
                score += 2.94f;
            else if (dif == -6)
                score += 3.16f;
        }

        switch(npcs[currentOpponent].GetComponent<NPCInfo>().savvy)
        {
            case 0:
                opponentAttackTimer += 1f / 3f * Time.deltaTime;
                break;
            case 1:
                opponentAttackTimer += 1f / 2.5f * Time.deltaTime;
                break;
            case 2:
                opponentAttackTimer += 1f / 2.2f * Time.deltaTime;
                break;
            case 3:
                opponentAttackTimer += 1f / 2f * Time.deltaTime;
                break;
            case 4:
                opponentAttackTimer += 1f / 1.8f * Time.deltaTime;
                break;
            case 5:
                opponentAttackTimer += 1f / 1.5f * Time.deltaTime;
                break;
            case 6:
                opponentAttackTimer += 1f / 1.2f * Time.deltaTime;
                break;
        }
        if (opponentAttackBar)
        {
            opponentAttackBar.transform.localScale = new Vector3(Mathf.Lerp(0f, 128f, opponentAttackTimer), attackBar.transform.localScale.y, attackBar.transform.localScale.z);
        }
        if (opponentAttackTimer > 1f && oSwitch == null)
        {
            oSwitch = StartCoroutine(OpponenetSwitch());
        }

        // Player key presses
        if(Input.GetKeyDown(pKeyCode))
        {
            AudioManager.instance.PlaySound("push_player");
            int dif = npcs[currentOpponent].GetComponent<NPCInfo>().hostility - player.hostility;
            if (dif == 0)
                score -= 1;
            else if (dif == 1)
                score -= 1;
            else if (dif == 2)
                score -= 1f;
            else if (dif == 3)
                score -= 1f;
            else if (dif == 4)
                score -= 1f;
            else if (dif == 5)
                score -= 1f;
            else if (dif == 6)
                score -= 1f;
            else if (dif == -1)
                score -= 1.1f;
            else if (dif == -2)
                score -= 1.66f;
            else if (dif == -3)
                score -= 2.307f;
            else if (dif == -4)
                score -= 2.66f;
            else if (dif == -5)
                score -= 2.94f;
            else if (dif == -6)
                score -= 3.16f;
        }

        float oldTimer = playerAttackTimer;
        switch (player.savvy)
        {
            case 0:
                playerAttackTimer += 1f / 3f * Time.deltaTime;
                break;
            case 1:
                playerAttackTimer += 1f / 2.5f * Time.deltaTime;
                break;
            case 2:
                playerAttackTimer += 1f / 2.2f * Time.deltaTime;
                break;
            case 3:
                playerAttackTimer += 1f / 2f * Time.deltaTime;
                break;
            case 4:
                playerAttackTimer += 1f / 1.8f * Time.deltaTime;
                break;
            case 5:
                playerAttackTimer += 1f / 1.5f * Time.deltaTime;
                break;
            case 6:
                playerAttackTimer += 1f / 1.2f * Time.deltaTime;
                break;
        }
        //display attack bir
        if (attackBar)
        {
            attackBar.transform.localScale = new Vector3(Mathf.Lerp(0f,128f,playerAttackTimer), attackBar.transform.localScale.y, attackBar.transform.localScale.z);
        }
        if(playerAttackTimer > 1f && oldTimer < 1f)
        {
            AudioManager.instance.PlaySound("barfull");
        }

        if(playerAttackTimer > 1f && Input.anyKeyDown)
        {
            KeyCode n = KeyCode.Space;
            foreach(KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(k))
                    n = k;
            }

            pKeyCode = n;

            StartCoroutine(Stun());
        }
        //DRAW poses
        showPose.sprite = poses[0];
        if(score >= 0)
        {
            //enemy winning
            if(oRow == 0)
            {
                showPose.sprite = poses[5];
            }
            else if(oRow == 1)
            {
                showPose.sprite = poses[4];
            }
            else
            {
                showPose.sprite = poses[3];
            }
        }
        else
        {
            //player winning
            if (oRow == 0)
            {
                showPose.sprite = poses[2];
            }
            else if (oRow == 1)
            {
                showPose.sprite = poses[1];
            }
            else
            {
                showPose.sprite = poses[0];
            }
        }

        if (score >= 50)
        {
            ResolveFight(currentOpponent, 1);
            fighting = false;
        }
        else if (score <= -50)
        {
            ResolveFight(currentOpponent, 0);
            fighting = false;
        }
    }

    private IEnumerator Stun()
    {
        float waitTime = 2f;

        int bond = npcs[currentOpponent].GetComponent<NPCInfo>().rivalries[0];

        if (bond >= 12)
            waitTime -= 1.2f;
        else if (bond >= 11)
            waitTime -= 1.5f;
        else if (bond >= 9)
            waitTime -= 1f;
        else if (bond >= 7)
            waitTime -= 0.8f;
        else if (bond >= 4)
            waitTime -= 0.6f;

        oStunned = true;

        yield return new WaitForSeconds(waitTime);

        oStunned = false;
        oRow = pRow();
        oKey = pKey();

        playerAttackTimer = 0;

        yield return null;
    }

    private IEnumerator OpponenetSwitch()
    {
        float waitTime = Random.Range(1f, 4f);

        int newRow = Random.Range(0, 3);
        AudioManager.instance.PlaySound("switch");
        while(newRow == oRow)
        {
            newRow = Random.Range(0, 3);
        }

        int newKey = 0;
        if(newRow == 0)
        {
            newKey = Random.Range(0, topRowKeys.Length-1);
        }
        if (newRow == 1)
        {
            newKey = Random.Range(0, midRowKeys.Length-1);

        }
        if (newRow == 2)
        {
            newKey = Random.Range(0, botRowKeys.Length-1);
        }

        yield return new WaitForSeconds(waitTime);

        int bond = npcs[currentOpponent].GetComponent<NPCInfo>().rivalries[0];

        oRow = newRow;
        oKey = newKey;

        pKeyCode = oKeyCode();

        opponentAttackTimer = 0;

        oSwitch = null;

        yield return null;
    }

    private void DrawOpponentFightKey()
    {
        switch(oRow)
        {
            case 0:
                oTopRend.sprite = score > 0 ? oWinning : oLosing;
                oMidRend.sprite = oInactive;
                oBotRend.sprite = oInactive;

                Sprite s = questionMark;

                foreach (Sprite sprite in fightCharacters)
                {
                    if (oKeyCode().ToString().ToLower() == sprite.name || (oKeyCode() == KeyCode.Period && sprite.name == "period"))
                    {
                        s = sprite;
                        break;
                    }

                }

                oTopKey.sprite = s;
                oMidKey.sprite = questionMark;
                oBotKey.sprite = questionMark;

                break;
            case 1:
                oTopRend.sprite = oInactive;
                oMidRend.sprite = score > 0 ? oWinning : oLosing;
                oBotRend.sprite = oInactive;

                Sprite p = questionMark;

                foreach (Sprite sprite in fightCharacters)
                {
                    if (oKeyCode().ToString().ToLower() == sprite.name || (oKeyCode() == KeyCode.Period && sprite.name == "period"))
                    {
                        p = sprite;
                        break;
                    }

                }

                oTopKey.sprite = questionMark;
                oMidKey.sprite = p;
                oBotKey.sprite = questionMark;
                break;
            case 2:
                oTopRend.sprite = oInactive;
                oMidRend.sprite = oInactive;
                oBotRend.sprite = score > 0 ? oWinning : oLosing;

                Sprite r = questionMark;

                foreach (Sprite sprite in fightCharacters)
                {
                    if (oKeyCode().ToString().ToLower() == sprite.name || (oKeyCode() == KeyCode.Period && sprite.name == "period"))
                    {
                        r = sprite;
                        break;
                    }

                }

                oTopKey.sprite = questionMark;
                oMidKey.sprite = questionMark;
                oBotKey.sprite = r;
                break;
        }
    }

    private void DrawPlayerFightKey()
    {
        switch (pRow())
        {
            case 0:
                pTopRend.sprite = score < 0 ? pWinning : pLosing;
                pMidRend.sprite = pInactive;
                pBotRend.sprite = pInactive;

                Sprite s = questionMark;

                foreach (Sprite sprite in fightCharacters)
                {
                    if (pKeyCode.ToString().ToLower() == sprite.name || (pKeyCode == KeyCode.Period && sprite.name == "period"))
                    {
                        s = sprite;
                        break;
                    }

                }

                pTopKey.sprite = s;
                pMidKey.sprite = questionMark;
                pBotKey.sprite = questionMark;

                break;
            case 1:
                pTopRend.sprite = pInactive;
                pMidRend.sprite = score < 0 ? pWinning : pLosing;
                pBotRend.sprite = pInactive;

                Sprite p = questionMark;

                foreach (Sprite sprite in fightCharacters)
                {
                    if (pKeyCode.ToString().ToLower() == sprite.name || (pKeyCode == KeyCode.Period && sprite.name == "period"))
                    {
                        p = sprite;
                        break;
                    }

                }

                pTopKey.sprite = questionMark;
                pMidKey.sprite = p;
                pBotKey.sprite = questionMark;
                break;
            case 2:
                pTopRend.sprite = pInactive;
                pMidRend.sprite = pInactive;
                pBotRend.sprite = score < 0 ? pWinning : pLosing;

                Sprite r = questionMark;

                foreach (Sprite sprite in fightCharacters)
                {
                    if (pKeyCode.ToString().ToLower() == sprite.name || (pKeyCode == KeyCode.Period && sprite.name == "period"))
                    {
                        r = sprite;
                        break;
                    }

                }

                pTopKey.sprite = questionMark;
                pMidKey.sprite = questionMark;
                pBotKey.sprite = r;
                break;
        }
    }

    public IEnumerator Fight(float waitTime, int opponent){

        currentOpponent = opponent;
        score = 0;

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
        oKey = 0;
        if (oRow == 0)
        {
            oKey = Random.Range(0, topRowKeys.Length - 1);
        }
        if (oRow == 1)
        {
            oKey = Random.Range(0, midRowKeys.Length - 1);

        }
        if (oRow == 2)
        {
            oKey = Random.Range(0, botRowKeys.Length - 1);
        }
        pKeyCode = oKeyCode();

        yield return new WaitUntil(() => fighting == false);

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
            AudioManager.instance.PlaySound("victory");
            npcs[opponent].GetComponent<NPCMovement>().lost_fight_dialogue.gameObject.SetActive(true);
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
            AudioManager.instance.PlaySound("defeat");
            npcs[opponent].GetComponent<NPCMovement>().won_fight_dialogue.gameObject.SetActive(true);
        }
    }

    void MovePlayer(){
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        transform.eulerAngles = Vector3.zero;
        if (moveHorizontal > 0f){
            player.spriteRenderer.flipX = true;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * 10) * 7);
        } else if (moveHorizontal < 0f){
            player.spriteRenderer.flipX = false;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * 10) * 7);
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
