using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler instance { get; private set; }
    AudioSource audioSource;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip cogFix;

    public AudioClip quest;

    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            instance = this;
        }
        // which is a special C# keyword that means “the object that currently runs that function”.
    }
    // Start is called before the first frame update

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayWin()
    {
        PlaySound(win);
    }
    public void PlayLose()
    {
        PlaySound(lose);
    }
    public void PlayFix()
    {
        PlaySound(cogFix);
    }

    public void PlayQuest()
    {
        PlaySound(quest);
    }

}
