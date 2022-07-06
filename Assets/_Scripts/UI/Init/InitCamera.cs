using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onStateChanged += state =>
        {
            gameObject.SetActive(state == GameManager.GameState.Init);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
