using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitButton : MonoBehaviour
{
    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onAny += Press;
    }

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onAny -= Press;
    }

    void Press()
    {
        if (isActiveAndEnabled)
        {
            btn.onClick?.Invoke();
        }
            
    }
}
