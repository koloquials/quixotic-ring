using UnityEngine;
using System.Collections;

public class AudioFadeIn : MonoBehaviour{
    public AudioSource song;

    //IEnumerator fade = FadeIn(song, 0.5f);
    Coroutine fade;
    

    void Start(){
        song = GetComponent<AudioSource>();
        fade = StartCoroutine(FadeIn(song, 8f));
    }


    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime){
        float startVolume = 0.2f;

        while (audioSource.volume < 0.60f){
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.volume = 0.60f;

    }

}