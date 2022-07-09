using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsList : MonoBehaviour
{
    [SerializeField] RectTransform selectImg;
    [SerializeField] RectTransform[] options;
    Button[] buttons;
    int currentSetting = 0;
    Vector2 localAnchoredPosition;

    private void Awake()
    {
        localAnchoredPosition = selectImg.anchoredPosition;
        buttons = new Button[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            int j = i;
            buttons[i] = options[i].GetComponentInChildren<Button>();
            buttons[i].onClick.AddListener(() =>
            {
                currentSetting = j;
                UpdateSelectImgPos();
            });
        }
            
    }

    private void OnEnable()
    {
        currentSetting = 0;
        UpdateSelectImgPos();
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onDown += MoveDown;
        input.onUp += MoveUp;
        input.onSelect += Press;
    }

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onDown -= MoveDown;
        input.onUp -= MoveUp;
        input.onSelect -= Press;
    }

    private void MoveUp()
    {
        if(currentSetting > 0)
        {
            currentSetting--;
            UpdateSelectImgPos();
        }
    }

    private void MoveDown()
    {
        if (currentSetting < options.Length-1)
        {
            currentSetting++;
            UpdateSelectImgPos();
        }
    }

    private void UpdateSelectImgPos()
    {
        selectImg.SetParent(options[currentSetting]);
        selectImg.anchoredPosition = localAnchoredPosition;
    }

    private void Press()
    {
        buttons[currentSetting].onClick?.Invoke();
    }
}
