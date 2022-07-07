using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] float delay = 1f;
    [SerializeField] float textDuration = 0.5f;

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
        text.text = "";
        yield return new WaitForSeconds(delay);
        text.text = "3";
        yield return new WaitForSeconds(textDuration);
        text.text = "2";
        yield return new WaitForSeconds(textDuration);
        text.text = "1";
        yield return new WaitForSeconds(textDuration);
        text.text = "GO!";
        GameManager.instance.SetState(GameManager.GameState.Game);
        yield return new WaitForSeconds(textDuration);
        text.text = "";
        text.enabled = false;
    }
}
