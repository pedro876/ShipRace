using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpDownBtn : MonoBehaviour
{
    Button btn;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    [SerializeField] float animTime = 0.5f;
    [SerializeField] Vector2 displacement;
    Vector2 srcPos;
    Vector2 dstPos;

    public void ShowAnimation()
    {
        canvasGroup.LeanAlpha(1f, animTime).setEaseInOutCubic();
    }

    public void HideAnimation()
    {
        canvasGroup.LeanAlpha(0f, animTime).setEaseInOutCubic();
    }

    private void Awake()
    {
        btn = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        srcPos = rectTransform.anchoredPosition;
        dstPos = srcPos + displacement;
        btn.onClick.AddListener(() =>
        {
            rectTransform.anchoredPosition = srcPos;
            LeanTween.move(rectTransform, dstPos, animTime).setEaseInOutCirc().setLoopPingPong(1);
        });
    }
}
