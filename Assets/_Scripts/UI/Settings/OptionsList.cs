using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsList : MonoBehaviour
{
    [SerializeField] RectTransform selectImg;
    [SerializeField] RectTransform[] options;
    private List<RectTransform> clearedOptions;
    Button[] buttons;
    int currentSetting = 0;
    Vector2 localAnchoredPosition;
    [SerializeField] bool horizontal = false;
    private bool initialized = false;

    private void Init()
    {
        clearedOptions = new List<RectTransform>();
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] != null)
                clearedOptions.Add(options[i]);
        }

        localAnchoredPosition = selectImg.anchoredPosition;
        buttons = new Button[clearedOptions.Count];
        for (int i = 0; i < clearedOptions.Count; i++)
        {
            int j = i;
            buttons[i] = clearedOptions[i].GetComponentInChildren<Button>();
            buttons[i].onClick.AddListener(() =>
            {
                currentSetting = j;
                UpdateSelectImgPos();
            });
        }
        initialized = true;
    }

    private void OnEnable()
    {
        if (initialized)
            Prepare();
    }

    private void Start()
    {
        if (!initialized)
        {
            Init();
            Prepare();
        }
    }

    void Prepare()
    {
        currentSetting = 0;
        UpdateSelectImgPos();
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        if (!horizontal)
        {
            input.onDown += MoveDown;
            input.onUp += MoveUp;
        }
        else
        {
            input.onRight += MoveDown;
            input.onLeft += MoveUp;
        }

        input.onSelect += Press;
    }

    /*private int CheckNonDestroyedOptions()
    {
        int count = 0;
        for(int i = 0; i < options.Length; i++)
        {
            if (options[i] != null)
                count++;
        }
        return count;
    }*/

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        if (!horizontal)
        {
            input.onDown -= MoveDown;
            input.onUp -= MoveUp;
        }
        else
        {
            input.onRight -= MoveDown;
            input.onLeft -= MoveUp;
        }
        
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
        if (currentSetting < clearedOptions.Count-1)
        {
            currentSetting++;
            UpdateSelectImgPos();
        }
    }

    private void UpdateSelectImgPos()
    {
        selectImg.SetParent(clearedOptions[currentSetting]);
        selectImg.anchoredPosition = localAnchoredPosition;
    }

    private void Press()
    {
        buttons[currentSetting].onClick?.Invoke();
    }
}
