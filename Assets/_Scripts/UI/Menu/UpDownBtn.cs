using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpDownBtn : MonoBehaviour
{
    Button btn;
    Image img;
    Color showColor;
    Color hideColor;
    RectTransform rectTransform;
    [SerializeField] float animTime = 0.5f;
    [SerializeField] Vector2 displacement;
    Vector2 srcPos;
    Vector2 dstPos;
    SmoothFloat outAnimVal;

    public void ShowAnimation()
    {
        outAnimVal.SetValue(1f);
    }

    public void HideAnimation()
    {
        outAnimVal.SetValue(0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        outAnimVal = new SmoothFloat(1f, animTime);
        btn = GetComponent<Button>();
        img = transform.GetChild(0).GetComponent<Image>();
        showColor = img.color;
        hideColor = showColor;
        hideColor.a = 0f;
        rectTransform = GetComponent<RectTransform>();
        srcPos = rectTransform.anchoredPosition;
        dstPos = srcPos + displacement;
        btn.onClick.AddListener(() =>
        {
            rectTransform.anchoredPosition = srcPos;
            LeanTween.move(rectTransform, dstPos, animTime).setEaseInOutCirc().setLoopPingPong(1);
        });
    }

    private void Update()
    {
        if (outAnimVal.Update(Time.deltaTime))
        {
            img.color = Color.Lerp(hideColor, showColor, outAnimVal.Value);
        }
    }
}
