using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    void Start()
    {
        
        GameManager.instance.onStateChanged += state =>
        {
            gameObject.SetActive(
                state != GameManager.GameState.Game && 
                state != GameManager.GameState.CountDown
                );
        };
    }
}
