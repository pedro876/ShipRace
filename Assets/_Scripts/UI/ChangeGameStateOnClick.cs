using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeGameStateOnClick : MonoBehaviour
{
    Button btn;
    [SerializeField] GameManager.GameState nextState;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => GameManager.instance.SetState(nextState));
    }
}
