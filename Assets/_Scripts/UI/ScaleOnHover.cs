using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float originalScale;
    private RectTransform rectTransform;
    private Button btn;
    [SerializeField] float lerpForce = 5f;
    [SerializeField] float scale = 1.2f;
    private bool hover = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        btn = GetComponent<Button>();
        originalScale = rectTransform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float targetScale = hover && (btn==null || btn.interactable) ? scale : originalScale;
        float currentScale = Mathf.Lerp(rectTransform.localScale.x, targetScale, lerpForce * Time.fixedDeltaTime);
        rectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }
}
