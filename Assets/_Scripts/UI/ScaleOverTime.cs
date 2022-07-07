using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScaleOverTime : MonoBehaviour
{
    private float originalScale;
    private RectTransform rectTransform;
    [SerializeField] float speed = 1f;
    [SerializeField] float scale = 1.2f;
    private float time = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*float targetScale = hover && (btn==null || btn.interactable) ? scale : originalScale;
        float currentScale = Mathf.Lerp(rectTransform.localScale.x, targetScale, lerpForce * Time.fixedDeltaTime);
        rectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);*/

        time += Time.fixedDeltaTime * speed;
        if(time > 2*Mathf.PI * 1000)
        {
            time -= 2*Mathf.PI * 1000;
        }

        float targetScale = Mathf.Lerp(originalScale, scale, Mathf.Sin(time)*0.5f+0.5f);
        rectTransform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }
}
