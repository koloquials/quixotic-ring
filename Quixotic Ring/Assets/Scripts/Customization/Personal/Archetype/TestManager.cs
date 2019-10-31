using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public int page;

    public int[] answers;

    public int value;
    public bool done;

    [Header("Scene Refs")]
    public GameObject page0;
    public GameObject page1, page2, page3;

    public SpriteRenderer slider0, slider1, slider2, slider3;

    public GameObject leftArrow;

    public SpriteRenderer dots;

    public GameObject test, results;

    public SpriteRenderer testResultName;

    public PlayerInfo info;

    [Header("Sprite Refs")]
    public Sprite[] sliders;
    public Sprite[] dotSprites;
    public Sprite[] testResultNameSprites;

    private float[] answerMultipliers = { -1, -0.5f, 0f, .5f, 1 };

    void Start()
    {
        answers = new int[16];
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i] = 2;
        }
    }
    
    void Update()
    {
        if (!done)
        {
            page0.SetActive(page == 0);
            page1.SetActive(page == 1);
            page2.SetActive(page == 2);
            page3.SetActive(page == 3);

            leftArrow.SetActive(page != 0);

            slider0.sprite = sliders[answers[4 * page]];
            slider1.sprite = sliders[answers[4 * page + 1]];
            slider2.sprite = sliders[answers[4 * page + 2]];
            slider3.sprite = sliders[answers[4 * page + 3]];

            dots.sprite = dotSprites[page];
        }
        else
        {
            test.SetActive(false);
            results.SetActive(true);

            testResultName.sprite = testResultNameSprites[value];
        }
    }

    public void SetAnswer(int slider, int value)
    {
        answers[4 * page + slider] = value;
    }

    public void NextPage()
    {
        if(page!= 3)
            page++;
        else if (page == 3)
        {
            value = Calculate();
            GetComponent<AppraisalPick>().num = value;
            done = true;
            info.archetype = value;
        }
    }
    public void LastPage()
    {
        if(page != 0)
            page--;
    }

    private int Calculate()
    {
        float reformer = 0f;
        float achiever = 0f;
        float individualist = 0f;
        float investigator = 0f;
        float loyalist = 0f;
        float challenger = 0f;

        //question 1
        reformer += answerMultipliers[answers[0]] * 2;
        //question 2
        individualist += answerMultipliers[answers[1]] * 3;
        //question 3
        loyalist += answerMultipliers[answers[2]] * 2;
        investigator += answerMultipliers[answers[2]] * 1;
        //question 4
        challenger += answerMultipliers[answers[3]] * 2;
        achiever += answerMultipliers[answers[3]] * -1;

        //question 5
        investigator += answerMultipliers[answers[4]] * 2;
        reformer += answerMultipliers[answers[4]] * 1;
        //question 6
        achiever += answerMultipliers[answers[5]] * 2;
        //question 7
        loyalist += answerMultipliers[answers[6]] * 2;
        challenger += answerMultipliers[answers[6]] * -1;
        //question 8
        reformer += answerMultipliers[answers[7]] * 2;
        achiever += answerMultipliers[answers[7]] * 2;

        //question 9
        individualist += answerMultipliers[answers[8]] * 2;
        //question 10
        reformer += answerMultipliers[answers[9]] * 2;
        investigator += answerMultipliers[answers[9]] * 1;
        //question 11
        achiever += answerMultipliers[answers[10]] * 2;
        //question 12
        loyalist += answerMultipliers[answers[11]] * 2;
        individualist += answerMultipliers[answers[11]] * 1;

        //question 13
        challenger += answerMultipliers[answers[12]] * 2;
        individualist += answerMultipliers[answers[12]] * 1;
        //question 14
        reformer += answerMultipliers[answers[13]] * 2;
        //question 15
        investigator += answerMultipliers[answers[14]] * 3;
        //question 16
        challenger += answerMultipliers[answers[15]] * 2;
        loyalist += answerMultipliers[answers[15]] * -1;

        float[] asArray = { reformer, achiever, individualist, investigator, loyalist, challenger };

        List<int> highest = new List<int>();

        highest.Add(0);
        for (int i = 1; i < asArray.Length; i++)
        {
            if(asArray[highest[0]] < asArray[i])
            {
                highest.Clear();
                highest.Add(i);
            }
            else if(asArray[highest[0]] == asArray[i])
            {
                highest.Add(i);
            }
        }

        if (highest.Count == 1)
            return highest[0];
        else
            return highest[Random.Range(0, highest.Count)];
    }
}
