using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TintOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color originalColor;
    private Image img;
    private Button btn;
    [SerializeField] float lerpForce = 5f;
    [SerializeField] Color tint = Color.white;
    private bool hover = false;

    void Start()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        originalColor = img.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color targetColor = hover && (btn == null || btn.interactable) ? originalColor * tint : originalColor;
        Color currentColor = Color.Lerp(img.color, targetColor, lerpForce * Time.fixedDeltaTime);
        img.color = currentColor;
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
