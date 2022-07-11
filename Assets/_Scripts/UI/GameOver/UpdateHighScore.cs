using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHighScore : MonoBehaviour
{

    private void OnEnable()
    {
        GameManager.instance.UpdateHighScore();
    }
}
