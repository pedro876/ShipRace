using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 showScale;
    /*private Vector2 halfHideScale;
    private Vector2 hideScale;*/
    private float height;
    private TextMeshProUGUI text;
    private float originalFontSize;
    private Color originalFontColor;

    private const float halfScaleMult = 0.75f;
    private SmoothFloat transitionVal;
    Button button;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        originalFontSize = text.fontSize;
        originalFontColor = text.color;
        rectTransform = GetComponent<RectTransform>();
        showScale = rectTransform.sizeDelta;
        /*halfHideScale = showScale * halfScaleMult;
        hideScale = Vector2.zero;*/
        height = rectTransform.rect.height;
        transitionVal = new SmoothFloat(1f);
    }

    public float GetAnchoredPosY() => rectTransform.anchoredPosition.y;

    public void SetAnchoredPosY(float y) => rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
    public float GetHeight() => height;

    public void Move(float amount, float time)
    {
        LeanTween.move(rectTransform, rectTransform.anchoredPosition + new Vector2(0f, amount), time).setEaseInOutBack();
    }

    public void Show(float time/*, bool instantly*/)
    {
        transitionVal.SetMaxTime(time);
        transitionVal.SetValue(1f);
        button.interactable = true;
    }

    public void HalfHide(float time/*, bool instantly*/)
    {
        transitionVal.SetMaxTime(time);
        transitionVal.SetValue(halfScaleMult);
        button.interactable = false;
    }

    public void Hide(float time/*, bool instantly*/)
    {
        transitionVal.SetMaxTime(time);
        transitionVal.SetValue(0f);
        button.interactable = false;
    }

    private void Update()
    {
        if (transitionVal.Update(Time.deltaTime))
        {
            float val = transitionVal.Value;
            rectTransform.sizeDelta = new Vector2(val, val) * showScale;
            text.fontSize = val * originalFontSize;
            originalFontColor.a = val;
            text.color = originalFontColor;
        }
    }
}
