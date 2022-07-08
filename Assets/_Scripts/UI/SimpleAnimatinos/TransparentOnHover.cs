using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TransparentOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float originalAlpha;
    private Image img;
    private Button btn;
    [SerializeField] float lerpForce = 5f;
    [SerializeField] float transparentAlpha = 0.5f;
    private bool hover = false;

    void Start()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        originalAlpha = img.color.a;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float targetAlpha = hover && (btn == null || btn.interactable) ? transparentAlpha : originalAlpha;
        float currentAlpha = Mathf.Lerp(img.color.a, targetAlpha, lerpForce * Time.fixedDeltaTime);
        Color c = img.color;
        c.a = currentAlpha;
        img.color = c;
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
