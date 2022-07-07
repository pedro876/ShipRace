using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlatform : MonoBehaviour
{
    [SerializeField] float delay = 0.5f;
    [SerializeField] float time = 2f;
    [SerializeField] float downDisplacement;
    Vector3 originalPos;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        targetPos = originalPos + Vector3.up * downDisplacement;
        GameManager.instance.onStateChanged += state =>
        {
            if(state == GameManager.GameState.CountDown || state == GameManager.GameState.Game)
            {
                transform.LeanMoveLocal(targetPos, time).setEaseInCubic().delay = delay;
            }
            else
            {
                transform.LeanMoveLocal(originalPos, time).setEaseInCubic();
            }
        };
    }
}
