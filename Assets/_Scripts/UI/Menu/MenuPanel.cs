using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] Button up;
    UpDownBtn upAnim;
    [SerializeField] Button down;
    UpDownBtn downAnim;
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] float buttonSeparation = 10f;
    [SerializeField] float halfVisibleButtonScale = 0.5f;
    [SerializeField] MenuButton defaultButton;
    [SerializeField] MenuButton quitButton;
    [SerializeField] List<MenuButton> buttons;

    int buttonIdx;
    private bool transitioning = false;
    private int moveUpOrders = 0;
    private int moveDownOrders = 0;

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        buttons.Remove(quitButton);
        Destroy(quitButton.gameObject);
#endif

        upAnim = up.GetComponent<UpDownBtn>();
        downAnim = down.GetComponent<UpDownBtn>();
        buttonIdx = buttons.IndexOf(defaultButton);

        defaultButton.SetAnchoredPosY(0f);
        float defaultHeight = defaultButton.GetHeight();
        float nextY = defaultHeight / 2f + buttonSeparation;
        for (int i = 0; i < buttonIdx; i++)
        {
            MenuButton button = buttons[i];
            float height = button.GetHeight();
            nextY += height * 0.5f;
            buttons[i].SetAnchoredPosY(nextY);
            nextY += height * 0.5f;
            nextY += buttonSeparation;
        }
        nextY = -defaultHeight / 2f - buttonSeparation;
        for(int i = buttonIdx+1; i < buttons.Count; i++)
        {
            MenuButton button = buttons[i];
            float height = button.GetHeight();
            nextY -= height * 0.5f;
            buttons[i].SetAnchoredPosY(nextY);
            nextY -= height * 0.5f;
            nextY -= buttonSeparation;
        }
        CheckVisibility(0.01f);

        up.onClick.AddListener(()=>moveUpOrders++);
        down.onClick.AddListener(() => moveDownOrders++);

        if (buttonIdx == 0)
            upAnim.HideAnimation(true);
        if (buttonIdx >= buttons.Count - 1)
            downAnim.HideAnimation(true);
    }

    private void OnEnable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onDown += AddMoveDownOrder;
        input.onUp += AddMoveUpOrder;
        input.onSelect += PressCurrentButton;
    }

    private void OnDisable()
    {
        var input = GameManager.serviceLocator.GetService<UIInputAdapter>();
        input.onDown -= AddMoveDownOrder;
        input.onUp -= AddMoveUpOrder;
        input.onSelect -= PressCurrentButton;
    }

    private void Update()
    {
        if (!transitioning)
        {
            if(moveUpOrders > 0)
            {
                moveUpOrders--;
                MoveUp();
            }
            else if (moveDownOrders > 0)
            {
                moveDownOrders--;
                MoveDown();
            }
        }
    }

    private void CheckVisibility(float time)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int diff = buttonIdx - i;

            if (diff == 0)
            {
                buttons[i].Show(time);
            }
            else if (diff == 1 || diff == -1)
            {
                buttons[i].HalfHide(time, halfVisibleButtonScale);
            }
            else
            {
                buttons[i].Hide(time);
            }
        }
    }

    private void AddMoveUpOrder()
    {
        moveUpOrders++;
    }

    private void AddMoveDownOrder()
    {
        moveDownOrders++;
    }

    private void MoveUp()
    {
        if (buttonIdx <= 0) return;
        if (transitioning) return;
        float amount = buttons[buttonIdx].GetHeight() * 0.5f + buttonSeparation + buttons[buttonIdx - 1].GetHeight() * 0.5f;
        MoveButtons(-amount);
        buttonIdx--;
        CheckVisibility(moveTime);
        StartCoroutine(DuringTransition());

        if (buttonIdx <= 0)
            upAnim.HideAnimation();
        else
            downAnim.ShowAnimation();
    }

    private void MoveDown()
    {
        if (buttonIdx >= buttons.Count -1) return;
        if (transitioning) return;
        float amount = buttons[buttonIdx].GetHeight() * 0.5f + buttonSeparation + buttons[buttonIdx + 1].GetHeight() * 0.5f;
        MoveButtons(amount);
        buttonIdx++;
        CheckVisibility(moveTime);
        StartCoroutine(DuringTransition());

        if (buttonIdx >= buttons.Count - 1)
            downAnim.HideAnimation();
        else
            upAnim.ShowAnimation();
    }

    private void MoveButtons(float amount)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Move(amount, moveTime);
        }
    }

    private void PressCurrentButton()
    {
        buttons[buttonIdx].Press();
    }

    IEnumerator DuringTransition()
    {
        transitioning = true;
        /*up.interactable = false;
        down.interactable = false;*/
        yield return new WaitForSeconds(moveTime);
        /*up.interactable = true;
        down.interactable = true;*/
        transitioning = false;
    }
    
}
