using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager _INSTANCE;
    private AudioSource source;

    void Awake()
    {
        if (_INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }

        _INSTANCE = this;
        //DontDestroyOnLoad(this);

    }

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySoundAtRandom(AudioClip[] clips)
    {
        if(clips != null && clips.Length >= 1)
        {
            var clip = clips[Random.Range(0, clips.Length)];
            source.PlayOneShot(clip);
        }
        
    }

    public void PlaySound(AudioClip clip)
    {
        if(clip != null)
            source.PlayOneShot(clip);
    }

}
