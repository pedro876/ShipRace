using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{

    private void OnEnable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onEscape += Resume;
    }

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onEscape -= Resume;
    }

    void Resume()
    {
        GameManager.instance.SetState(GameManager.GameState.Game);
    }
}
