using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecalibrateBtn : MonoBehaviour
{
    Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        btn.onClick.AddListener(() =>
        {
            GameManager.serviceLocator.GetService<IGyroSystem>().Recalibrate();
        });
    }
}
