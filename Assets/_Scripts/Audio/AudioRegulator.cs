using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRegulator : MonoBehaviour
{
    public static HashSet<AudioRegulator> regulators = new HashSet<AudioRegulator>();
    AudioSource audioSource;
    [HideInInspector] public float originalVolume;
    float multiplier = 1f;
    public bool isMusic = true;

    bool fadingIn = false;
    bool fadingOut = false;

    float timer = 0f;
    float maxTime = 1f;

    public delegate void OnEndFade();
    public OnEndFade onEndFade;

    [SerializeField] bool isPlaying;

    private void OnDestroy()
    {
        regulators.Remove(this);
    }

    // Start is called before the first frame update
    void Awake()
    {
        regulators.Add(this);
        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;
    }

    private void Start()
    {
        UpdateVolume();
    }

    private void Update()
    {
        if(fadingIn || fadingOut)
        {
            timer += Time.deltaTime;
            if(timer >= maxTime)
            {
                if (fadingOut)
                {
                    audioSource.Stop();
                }
                fadingIn = false;
                fadingOut = false;
                multiplier = fadingOut ? 0f : 1f;
                onEndFade?.Invoke();
                
            } else
            {
                multiplier = UsefulFuncs.SmoothStep(0f, 1f, timer / maxTime);
                if (fadingOut) multiplier = 1f - multiplier;
            }
            //UpdateVolume();
        }
        UpdateVolume();
        isPlaying = audioSource.isPlaying;
    }

    public void UpdateVolume()
    {
        //Debug.Log(GameManager.effectsVolume);
        audioSource.volume = originalVolume * multiplier * (isMusic ? GameManager.instance.musicVolume : GameManager.instance.effectsVolume);
    }

    public void FadeIn(float time, OnEndFade _onEndFade = null)
    {
        timer = 0f;
        fadingIn = true;
        fadingOut = false;
        maxTime = time;
        onEndFade = _onEndFade;
        audioSource.Play();
    }

    public void FadeOut(float time, OnEndFade _onEndFade = null)
    {
        timer = 0f;
        fadingIn = false;
        fadingOut = true;
        maxTime = time;
        onEndFade = _onEndFade;
    }

    public static void UpdateAllVolumes()
    {
        foreach(var r in regulators)
        {
            if(r != null)
                r.UpdateVolume();
        }
    }
}
