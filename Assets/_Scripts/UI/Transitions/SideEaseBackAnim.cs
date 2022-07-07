using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SideEaseBackAnim : LeanTransitionBase
{
    [SerializeField] float transitionTime = 0.5f;
    [SerializeField] Vector2 hideDisplacement;
    Vector2 hidePos;

    protected override void Init()
    {
        hidePos = srcPos + hideDisplacement;
    }

    protected override LTDescr ShowAnimation()
    {
        rectTransform.anchoredPosition = hidePos;
        return LeanTween.move(rectTransform, srcPos, transitionTime).setEaseOutBack();
        //return transform.LeanMove(srcPos, transitionTime).setEaseOutBack();
    }

    protected override LTDescr HideAnimation()
    {
        return LeanTween.move(rectTransform, hidePos, transitionTime).setEaseInBack();
    }
}
