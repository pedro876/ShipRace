using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void OnEnable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onEscape += Press;
    }

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onEscape -= Press;
    }

    void Press()
    {
        btn.onClick?.Invoke();
    }
}
