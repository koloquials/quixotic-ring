using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour {

    public int zHostility;
    public int zSavvy;
    public int zInterest;
    public int zGrudge;

    public int aHostility;
    public int aSavvy;
    public int aInterest;
    public int aGrudge;

    public void CalculateStats(string zodiac, string archetype){
        switch (zodiac){
            case "rat":
                zHostility = 0;
                zSavvy = 3;
                zInterest = 2;
                zGrudge = 0;
                break;
            case "ox":
                zHostility = 1;
                zSavvy = 0;
                zInterest = 2;
                zGrudge = 2;
                break;
            case "tiger":
                zHostility = 3;
                zSavvy = 0;
                zInterest = 1;
                zGrudge = 1;
                break;
            case "rabbit":
                zHostility = 0;
                zSavvy = 1;
                zInterest = 3;
                zGrudge = 1;
                break;
            case "dragon":
                zHostility = 2;
                zSavvy = 0;
                zInterest = 3;
                zGrudge = 0;
                break;
            case "snake":
                zHostility = 1;
                zSavvy = 2;
                zInterest = 2;
                zGrudge = 0;
                break;
            case "horse":
                zHostility = 1;
                zSavvy = 2;
                zInterest = 0;
                zGrudge = 2;
                break;
            case "goat":
                zHostility = 3;
                zSavvy = 0;
                zInterest = 0;
                zGrudge = 2;
                break;
            case "monkey":
                zHostility = 1;
                zSavvy = 3;
                zInterest = 0;
                zGrudge = 1;
                break;
            case "rooster":
                zHostility = 0;
                zSavvy = 2;
                zInterest = 0;
                zGrudge = 3;
                break;
            case "dog":
                zHostility = 1;
                zSavvy = 0;
                zInterest = 1;
                zGrudge = 3;
                break;
            case "boar":
                zHostility = 2;
                zSavvy = 2;
                zInterest = 1;
                zGrudge = 0;
                break;
            default:
                zHostility = 0;
                zSavvy = 0;
                zInterest = 0;
                zGrudge = 0;
                print("empty/incorrect zodiac input");
                break;
        }

        switch (archetype){
            case "reformer":
                aHostility = 0;
                aSavvy = 2;
                aInterest = 3;
                aGrudge = 0;
                break;
            case "achiever":
                aHostility = 2;
                aSavvy = 2;
                aInterest = 1;
                aGrudge = 0;
                break;
            case "individualist":
                aHostility = 2;
                aSavvy = 0;
                aInterest = 2;
                aGrudge = 1;
                break;
            case "investigator":
                aHostility = 1;
                aSavvy = 0;
                aInterest = 3;
                aGrudge = 1;
                break;
            case "loyalist":
                aHostility = 0;
                aSavvy = 0;
                aInterest = 2;
                aGrudge = 3;
                break;
            case "challenger":
                aHostility = 3;
                aSavvy = 0;
                aInterest = 2;
                aGrudge = 0;
                break;
            default:
                aHostility = 0;
                aSavvy = 0;
                aInterest = 0;
                aGrudge = 0;
                print("empty/incorrect archetype input");
                break;
        }
    }
}
