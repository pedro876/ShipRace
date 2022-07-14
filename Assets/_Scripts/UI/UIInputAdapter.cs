using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerInput))]
public class UIInputAdapter : MonoBehaviour
{
    PlayerInput input;

    public event Action onUp;
    public event Action onDown;
    public event Action onRight;
    public event Action onLeft;
    public event Action onSelect;
    public event Action onAny;
    public event Action onEscape;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        InputAction upAction = input.actions["Up"];
        InputAction downAction = input.actions["Down"];
        InputAction rightAction = input.actions["Right"];
        InputAction leftAction = input.actions["Left"];
        InputAction selectAction = input.actions["Select"];
        InputAction anyAction = input.actions["Any"];
        InputAction escapeAction = input.actions["Escape"];
        upAction.performed += ctx => onUp?.Invoke();
        downAction.performed += ctx => onDown?.Invoke();
        rightAction.performed += ctx => onRight?.Invoke();
        leftAction.performed += ctx => onLeft?.Invoke();
        selectAction.started += ctx => onSelect?.Invoke();
        anyAction.performed += ctx => onAny?.Invoke();
        escapeAction.performed += ctx => onEscape?.Invoke();



        InputAction leftStick = input.actions["LeftStick"];
        leftStick.performed += ctx =>
        {
            var axis = ctx.ReadValue<Vector2>();
            if(Mathf.Abs(axis.x) > Mathf.Abs(axis.y))
            {
                if (axis.x > 0f)
                    onRight?.Invoke();
                else
                    onLeft?.Invoke();
            }
            else
            {
                if (axis.y > 0f)
                    onUp?.Invoke();
                else
                    onDown?.Invoke();
            }
        };
    }
}
