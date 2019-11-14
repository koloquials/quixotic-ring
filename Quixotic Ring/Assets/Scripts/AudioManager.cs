using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public GameObject audioPrefab;
    public AudioSource[] sounds;
    float baseVolume;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        sounds = new AudioSource[32];
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject obj = Instantiate(audioPrefab, transform);
            sounds[i] = obj.GetComponent<AudioSource>();
        }
        baseVolume = sounds[0].volume;
    }

    public void PlaySound(string clipName)
    {
        AudioClip clip = Resources.Load(clipName) as AudioClip;
        if(clip == null)
        {
            return;
        }
        for (int i = 0; i < sounds.Length; i++)
        {
            AudioSource audio = sounds[i];
            if (audio.isPlaying)
            {
                continue;
            }
            audio.clip = clip;
            audio.volume = baseVolume;
            audio.Play();
        }
    }
}
