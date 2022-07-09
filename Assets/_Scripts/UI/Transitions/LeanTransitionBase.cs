using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LeanTransitionBase : MonoBehaviour
{
    public bool isVisible = false;
    protected Vector2 srcPos;
    protected Quaternion srcRot;
    protected Vector2 srcLocalScale;
    protected RectTransform rectTransform;

    protected abstract void Init();
    protected abstract LTDescr ShowAnimation();
    protected abstract LTDescr HideAnimation();

    [SerializeField] float delay = 0f;
    [SerializeField] protected UnityEvent onShow;
    [SerializeField] protected UnityEvent onShown;
    [SerializeField] protected UnityEvent onHide;
    [SerializeField] protected UnityEvent onHidden;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        srcPos = rectTransform.anchoredPosition;
        srcRot = rectTransform.rotation;
        srcLocalScale = transform.localScale;
    }

    private void Start()
    {
        Init();
        if (isVisible)
            Show();
        else
            gameObject.SetActive(false);
    }


    public void Show()
    {
        isVisible = true;
        onShow?.Invoke();
        gameObject.SetActive(true);
        ShowAnimation().setOnComplete(() => onShown?.Invoke()).delay = delay;
    }

    public void Hide()
    {
        isVisible = false;
        onHide?.Invoke();
        HideAnimation().setOnComplete(() => {
            onHidden?.Invoke();
            gameObject.SetActive(false);
            });
    }
}
