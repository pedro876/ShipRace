using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public void Play(AudioClip clip)
    {
        GameManager.serviceLocator.GetService<PersistentAudioSource>().PlayMusic(clip);
    }

    public void Stop()
    {
        GameManager.serviceLocator.GetService<PersistentAudioSource>().StopAllMusic();
    }

    public void PlayEffect(AudioClip clip)
    {
        GameManager.serviceLocator.GetService<PersistentAudioSource>().PlayEffect(clip);
    }
}
