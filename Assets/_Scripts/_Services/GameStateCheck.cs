using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateCheck : MonoBehaviour
{
    [SerializeField] private GameManager.GameState[] activeStates;

    [SerializeField] UnityEvent onActiveState;
    [SerializeField] UnityEvent onInactiveState;

    bool isActive = false;

    private void Start()
    {
        GameManager.instance.onStateChanged += CheckState;
        isActive = IsActiveState(GameManager.instance.currentState);
        InvokeEvents();
    }

    private bool IsActiveState(GameManager.GameState state)
    {
        bool isActiveState = false;
        for (int i = 0; i < activeStates.Length && !isActiveState; i++)
        {
            if (state == activeStates[i])
                isActiveState = true;
        }
        return isActiveState;
    }

    private void CheckState(GameManager.GameState state)
    {
        bool isActiveState = IsActiveState(state);

        if (isActiveState != isActive)
        {
            isActive = isActiveState;
            InvokeEvents();
        }
    }

    private void InvokeEvents()
    {
        if (isActive)
            onActiveState?.Invoke();
        else
            onInactiveState?.Invoke();
    }
}
