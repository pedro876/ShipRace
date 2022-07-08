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
    bool confirmSelected = false;

    protected override void Init()
    {
        showPos = rectTransform.anchoredPosition;
        hidePos = showPos + hideDisplacement;
        confirmBtn.onClick.AddListener(() =>
        {
            GameManager.instance.ExitGame();
        });

        rectTransform.anchoredPosition = hidePos;
        gameObject.SetActive(false);

        
    }

    private void OnEnable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onRight += SelectConfirm;
        input.onLeft += SelectCancel;
        input.onSelect += Press;
        SelectCancel();
    }

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onRight -= SelectConfirm;
        input.onLeft -= SelectCancel;
        input.onSelect -= Press;
    }

    void SelectConfirm()
    {
        confirmBtn.Select();
        confirmSelected = true;
    }

    void SelectCancel()
    {
        cancelBtn.Select();
        confirmSelected = false;
    }

    void Press()
    {
        if (confirmSelected)
            confirmBtn.onClick?.Invoke();
        else
            cancelBtn.onClick?.Invoke();
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
