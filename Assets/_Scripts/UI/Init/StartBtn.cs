using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    Button btn;
    [SerializeField] GameManager.GameState nextState;
    [SerializeField] Vector2 displacementOnPressed = Vector2.zero;
    [SerializeField] float transitionTime = 1f;
    Vector2 originalPos;
    Vector2 targetPos;

    private void Awake()
    {
        originalPos = transform.localPosition;
        targetPos = originalPos + displacementOnPressed;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(StartAnimation);
    }

    private void OnEnable()
    {
        transform.localPosition = targetPos;
        transform.LeanMoveLocal(originalPos, transitionTime).setEaseOutBack();
    }

    private void StartAnimation()
    {
        GameManager.instance.SetState(nextState);
        transform.LeanMoveLocal(targetPos, transitionTime)
               .setEaseInBack()/*.setOnComplete(OnFinishedAnimation)*/;
    }

    /*private void OnFinishedAnimation()
    {
        GameManager.instance.SetState(nextState);
    }*/
}
