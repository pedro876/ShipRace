using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SfxBtn : MonoBehaviour
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
        GameManager.instance.SetSFX(!GameManager.instance.IsSfxOn());
        UpdateText();
    }

    void UpdateText()
    {
        if (text == null) return;
        if (GameManager.instance.IsSfxOn())
        {
            text.text = "ON";
        }
        else
        {
            text.text = "OFF";
        }
    }
}
