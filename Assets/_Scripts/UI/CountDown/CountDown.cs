using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] float delay = 1f;
    [SerializeField] float textDuration = 0.5f;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip goClip;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "";
        text.enabled = false;
        GameManager.instance.onStateChanged += state =>
        {
            if (state == GameManager.GameState.CountDown)
            {
                text.enabled = true;
                StartCoroutine(CountDownCoroutine());
            }
        };
    }

    IEnumerator CountDownCoroutine()
    {
        var audio = GameManager.serviceLocator.GetService<PersistentAudioSource>();
        text.text = "";
        yield return new WaitForSeconds(delay);
        text.text = "3";
        audio.PlayEffect(clip);
        yield return new WaitForSeconds(textDuration);
        text.text = "2";
        audio.PlayEffect(clip);
        yield return new WaitForSeconds(textDuration);
        text.text = "1";
        audio.PlayEffect(clip);
        yield return new WaitForSeconds(textDuration);
        text.text = "GO!";
        audio.PlayEffect(goClip);
        GameManager.instance.SetState(GameManager.GameState.Game);
        yield return new WaitForSeconds(textDuration);
        text.text = "";
        text.enabled = false;
    }
}
