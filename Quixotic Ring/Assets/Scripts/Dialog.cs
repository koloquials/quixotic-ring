using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public TextMeshPro textMesh;
    public int idx;
    public string goalText;

    public float waitTime;

    public string[] script;

    private Coroutine coroutine;
    private bool typing = false;

    public enum FinishBehavior { LOAD_SCENE, CLOSE,START_FIGHT };
    public FinishBehavior finishBehavior;

    public int sceneToLoad;
    public bool done;

    void Start()
    {
        idx = 0;
        coroutine = StartCoroutine(ShowText());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && typing)
        {
            AudioManager.instance.PlaySound("next");
            typing = false;
            StopCoroutine(coroutine);
            textMesh.text = goalText;
            textMesh.text = textMesh.text.Replace("@", "<sprite=\"WishDataSource\" index=0>");
            textMesh.text = textMesh.text.Replace("%", "<color=#FFB8BD>");
            textMesh.text = textMesh.text.Replace("$", "</color>");
        }
        else if(Input.GetKeyDown(KeyCode.R) && idx == script.Length - 1)
        {
            AudioManager.instance.PlaySound("next");
            idx = 0;
            switch (finishBehavior)
            {
                case FinishBehavior.LOAD_SCENE:
                    SceneManager.LoadScene(sceneToLoad);
                    break;
                case FinishBehavior.CLOSE:
                    gameObject.SetActive(false);
                    break;
                case FinishBehavior.START_FIGHT:
                    done = true;
                    break;
            }
        }
        else if(Input.GetKeyDown(KeyCode.R) && !typing)
        {
            AudioManager.instance.PlaySound("next");
            idx++;
            coroutine = StartCoroutine(ShowText());
        }
    }

    public IEnumerator ShowText()
    {
        goalText = script[idx];
        textMesh.text = "";
        typing = true;
        goalText = goalText.Replace("@", "<sprite=\"WishDataSource\" index=0>");
        goalText = goalText.Replace("%", "<color=#FFB8BD>");
        goalText = goalText.Replace("$", "</color>");
        while (textMesh.text != goalText)
        {
            textMesh.text = goalText.Substring(0, textMesh.text.Length + 1);

            if(textMesh.text[textMesh.text.Length - 1] == '<')
            {
                while(textMesh.text[textMesh.text.Length-1] != '>')
                    textMesh.text = goalText.Substring(0, textMesh.text.Length + 1);
            }
            yield return new WaitForSeconds(waitTime);
        }
        typing = false;
        yield return null;
    }
}
