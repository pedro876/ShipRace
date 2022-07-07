using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWithGameState : MonoBehaviour
{
    [SerializeField] GameManager.GameState[] visibleStates;

    private void Start()
    {
        GameManager.instance.onStateChanged += CheckState;
        CheckState(GameManager.instance.currentState);
    }

    private void CheckState(GameManager.GameState state)
    {
        bool visible = false;
        for(int i = 0; i < visibleStates.Length && !visible; i++)
        {
            if (state == visibleStates[i])
                visible = true;
        }
        if(gameObject.activeSelf != visible)
            gameObject.SetActive(visible);
    }
}
