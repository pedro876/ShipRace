using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicBtn : MonoBehaviour
{
    Button btn;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        btn.onClick.AddListener(Press);
        UpdateText();
    }

    private void OnEnable()
    {
        UpdateText();
    }

    void Press()
    {
        GameManager.instance.SetMusic(!GameManager.instance.IsMusicOn());
        UpdateText();
    }

    void UpdateText()
    {
        if (text == null) return;
        if (GameManager.instance.IsMusicOn())
        {
            text.text = "ON";
        }
        else
        {
            text.text = "OFF";
        }
    }
}
