using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : LeanTransitionBase
{
    [SerializeField] Button confirmBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] Vector2 hideDisplacement;
    [SerializeField] float animTime = 1f;
    Vector2 showPos;
    Vector2 hidePos;

    protected override void Init()
    {
        showPos = rectTransform.anchoredPosition;
        hidePos = showPos + hideDisplacement;
        confirmBtn.onClick.AddListener(() =>
        {
            GameManager.instance.ExitGame();
        });

        /*cancelBtn.onClick.AddListener(() => GameManager.instance.SetState(GameManager.GameState.Menu));

        GameManager.instance.onStateChanged += state =>
        {
            if (state == GameManager.GameState.Exit)
            {
                Show();
            }
            else
            {
                Hide();
            }
        };*/

        rectTransform.anchoredPosition = hidePos;
        gameObject.SetActive(false);
    }

    protected override LTDescr HideAnimation()
    {
        return LeanTween.move(rectTransform, hidePos, animTime).setEaseInBack();
    }

    protected override LTDescr ShowAnimation()
    {
        rectTransform.anchoredPosition = hidePos;
        return LeanTween.move(rectTransform, showPos, animTime).setEaseOutBack();
    }
}
