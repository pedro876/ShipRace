using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAudioSource : MonoBehaviour
{
    [SerializeField] AudioClip[] allClips;
    [HideInInspector] public Dictionary<string, AudioClip> allClipsDict = new Dictionary<string, AudioClip>();

    [SerializeField] float musicTransitionTime = 1f;
    [SerializeField] private int poolSize = 10;
    private AudioSource[] audioSources;
    private Stack<AudioSource> avaibleAudioSources = new Stack<AudioSource>();
    private HashSet<AudioSource> inUseAudioSources = new HashSet<AudioSource>();
    private GameObject pool;

    private void Awake()
    {
        if(pool == null)
        {
            audioSources = new AudioSource[poolSize];
            foreach (var a in allClips) allClipsDict.Add(a.name, a);
            pool = new GameObject("AudioPool");
            pool.transform.position = Camera.main.transform.position;
            GameObject obj;
            for (int i = 0; i < poolSize; i++)
            {
                obj = new GameObject("AudioSource" + i);
                obj.transform.parent = pool.transform;
                audioSources[i] = obj.AddComponent(typeof(AudioSource)) as AudioSource;
                audioSources[i].playOnAwake = false;
                obj.AddComponent(typeof(AudioRegulator));
                obj.SetActive(false);
                avaibleAudioSources.Push(audioSources[i]);
            }
            DontDestroyOnLoad(pool);
        } else
        {
            Debug.Log("AudioPool already present");
        }
    }

    private void Update()
    {
        List<AudioSource> audiosToStop = new List<AudioSource>();
        foreach(AudioSource source in inUseAudioSources)
        {
            if (!source.isPlaying && !source.GetComponent<AudioRegulator>().isMusic)
            {
                audiosToStop.Add(source);
            }
        }
        for(int i = 0; i < audiosToStop.Count; i++)
        {
            StopSource(audiosToStop[i], true);
        }
        audiosToStop.Clear();
    }

    //PUBLIC

    public void StopClip(AudioClip clip)
    {
        List<AudioSource> audiosToStop = new List<AudioSource>();
        foreach (AudioSource source in inUseAudioSources)
        {
            if(source && source.clip == clip)
            {
                audiosToStop.Add(source);
            }
        }
        for (int i = 0; i < audiosToStop.Count; i++)
        {
            StopSource(audiosToStop[i]);
        }
        audiosToStop.Clear();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        bool alreadyPlaying = false;
        List<AudioSource> otherMusic = new List<AudioSource>();
        AudioRegulator regulator;
        foreach(AudioSource source in inUseAudioSources)
        {
            if(source && source.clip == clip)
            {
                alreadyPlaying = true;
                break;
            }
            regulator = source.GetComponent<AudioRegulator>();
            if (regulator.isMusic)
            {
                otherMusic.Add(source);
            }
        }

        if (!alreadyPlaying)
        {
            foreach (var source in otherMusic) StopSource(source);
            TryPlayClip(clip, true, true);
        }
    }

    public void StopAllMusic()
    {
        List<AudioSource> audiosToStop = new List<AudioSource>();
        AudioRegulator regulator;
        foreach (AudioSource source in inUseAudioSources)
        {
            if (source == null) continue;
            regulator = source.GetComponent<AudioRegulator>();
            
            if (regulator.isMusic)
            {
                audiosToStop.Add(source);
            }
        }
        for (int i = 0; i < audiosToStop.Count; i++)
        {
            StopSource(audiosToStop[i]);
        }
        audiosToStop.Clear();
    }

    public void PlayEffect(AudioClip clip)
    {
        if (clip != null)
            TryPlayClip(clip, false, false);
    }

    public void PlayEffectByName(string clipName)
    {
        if(allClipsDict.TryGetValue(clipName, out var clip))
        {
            PlayEffect(clip);
        }
    }

    private void TryPlayClip(AudioClip clip, bool isMusic, bool loop = false)
    {
        if (avaibleAudioSources.Count > 0)
        {
            var source = avaibleAudioSources.Pop();
            if (!source) return;
            inUseAudioSources.Add(source);
            source.gameObject.SetActive(true);
            source.clip = clip;
            source.loop = loop;
            source.Play();
            var regulator = source.GetComponent<AudioRegulator>();
            regulator.isMusic = isMusic;
            if (isMusic)
            {
                regulator.FadeIn(musicTransitionTime);
            }
            regulator.UpdateVolume();
        }
    }

    private void StopSource(AudioSource source, bool forced = false)
    {
        var regulator = source.GetComponent<AudioRegulator>();
        if (regulator.isMusic && !forced)
        {
            regulator.FadeOut(musicTransitionTime);
            regulator.onEndFade = () =>
            {
                StopSource(source, true);
            };
        }
        else
        {
            regulator.onEndFade = null;
            source.gameObject.SetActive(false);
            inUseAudioSources.Remove(source);
            avaibleAudioSources.Push(source);
        }
    }
}
